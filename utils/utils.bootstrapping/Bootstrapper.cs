using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Win32.SafeHandles;

namespace utils {
	/// <summary>
	/// typed presentation of '-dir.info' file.
	/// </summary>
	[XmlRoot("dir-info")]
	public class BootstrapperDirInfo {
		public const string fileName = @"-dir.info";
		[XmlAttribute]
		public string type;
	}
	public static class Bootstrapper {

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool AllocConsole();

		//[StructLayout(LayoutKind.Sequential)]
		//private class SECURITY_ATTRIBUTES {
		//	public int nLength;
		//	public unsafe byte* pSecurityDescriptor = null;
		//	public int bInheritHandle;
		//}

		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, /*SECURITY_ATTRIBUTES*/ IntPtr securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

		private static SafeFileHandle hConOut = new SafeFileHandle(IntPtr.Zero, true);

		public static bool CreateConsole() {
			if (AllocConsole()) {
				//vs debugger may redirect console out into debug out, so we need to redirect it back
				if (!hConOut.IsInvalid) {
					hConOut.Dispose();
				}
				hConOut = CreateFile("CONOUT$", 0x40000000, FileShare.Write, /*null*/ IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
				if (!hConOut.IsInvalid) {
					SetStdHandle(-11, hConOut.DangerousGetHandle());
				}
				var cw = new StreamWriter(Console.OpenStandardOutput());
				cw.AutoFlush = true;
				Console.SetOut(cw);
				return true;
			}
			return false;
		}

		/// <summary>
		/// create console if there are any console trace listener
		/// </summary>
		public static void CreateConsoleForTracing() {
			if (Trace.Listeners != null && Trace.Listeners.OfType<ConsoleTraceListener>().Any()) {
				var created = Bootstrapper.CreateConsole();
				foreach (var cl in Trace.Listeners.OfType<ConsoleTraceListener>()) {
					cl.Writer = Console.Out;
				}
				if(created) {
					log.WriteInfo("console has been created...");
				}
			}
		}

		public class SpecialFolderDescription {
			public BootstrapperDirInfo info;
			public DirectoryInfo directory;
		}

		public class SpecialFolders {
			public List<SpecialFolderDescription> plugins = new List<SpecialFolderDescription>();
			public List<SpecialFolderDescription> locales = new List<SpecialFolderDescription>();
			public List<SpecialFolderDescription> dlls = new List<SpecialFolderDescription>();
			public List<SpecialFolderDescription> others = new List<SpecialFolderDescription>();
		}

		public static IEnumerable<FileInfo> GetFiles(this IEnumerable<SpecialFolderDescription> folders, string pattern) {
			foreach (var sfd in folders) {
				foreach (var fi in sfd.directory.GetFiles(pattern)) {
					yield return fi;
				}
			}
		}

		static readonly Lazy<SpecialFolders> _specialFolders = new Lazy<SpecialFolders>(
			()=>GetSpecialFolders(new DirectoryInfo(Utils.MapPath(@"~/"))),
			LazyThreadSafetyMode.ExecutionAndPublication
		);
		public static SpecialFolders specialFolders {
			get{return _specialFolders.Value;}
		}

		public static SpecialFolders GetSpecialFolders(DirectoryInfo di) {
			var sf = new SpecialFolders();
			foreach(var sfd in EnumSpecialFolders(di)){
				var sfType = sfd.info.type;
				sfType = sfType != null ? sfType.ToLower() : null;
				switch (sfType) {
					case "dlls":
						sf.dlls.Add(sfd);
						break;
					case "locales":
						sf.locales.Add(sfd);
						break;
					case "plugins":
						sf.plugins.Add(sfd);
						break;
					default:
						sf.others.Add(sfd);
						break;
				}
			}
			return sf;
		}

		public static IEnumerable<SpecialFolderDescription> EnumSpecialFolders(DirectoryInfo di) {
			var fi = di.GetFiles(BootstrapperDirInfo.fileName).FirstOrDefault();
			if (fi != null) {
				SpecialFolderDescription sfd = null;
				try {
					using (var fs = fi.OpenRead()) {
						using (var xr = new XmlTextReader(fs)) {
							var dirInfo = xr.Deserialize<BootstrapperDirInfo>();
							sfd = new SpecialFolderDescription { info = dirInfo, directory = di };
						}
					}
				} catch (Exception err) {
					log.WriteError(String.Format("failed to deserialize dir.info file with error:{0}", err.Message));
					dbg.Break();
				}
				if (sfd != null) {
					yield return sfd;
				}
			}
			foreach (var sdi in di.GetDirectories()) {
				foreach (var sfd in EnumSpecialFolders(sdi)) {
					yield return sfd;
				}
			}
		}

		/// <summary>
		/// recursively looks for all folders containing '-dir.info' file, which describe kind of special folder. 
		/// "dlls" folders - added to dll search path.
		/// </summary>
		/// <param name="di">directory where scan will be done</param>
		public static void ScanSpecialFolders(DirectoryInfo di) {
			var fi = di.GetFiles(BootstrapperDirInfo.fileName).FirstOrDefault();
			if (fi != null) {
				try {
					using (var fs = fi.OpenRead()) {
						using (var xr = new XmlTextReader(fs)) {
							var dirInfo = xr.Deserialize<BootstrapperDirInfo>();
							if (dirInfo.type == "dlls") {
								if (!Utils.SetDllDirectory(di.FullName)) {
									log.WriteError(String.Format("failed to set dll search path '{0}'", di.FullName));
								}
							}
						}
					}
				} catch (Exception err) {
					log.WriteError(String.Format("failed to deserialize dir.info file with error:{0}", err.Message));
					dbg.Break();
				}
			}
			foreach (var sdi in di.GetDirectories()) {
				ScanSpecialFolders(sdi);
			}
		}
		
		/// <summary>
		/// recursively looks for all folders containing '-dir.info' file, which describe kind of special folder. 
		/// "dlls" folders - added to dll search path.
		/// </summary>
		/// <param name="path">directory where scan will be done</param>
		public static void ScanSpecialFolders(string path) {
			var di = new DirectoryInfo(path);
			if (di.Exists) {
				ScanSpecialFolders(new DirectoryInfo(path));
			}
		}

		/// <summary>
		/// recursively looks for all folders containing '-dir.info' file, which describe kind of special folder. 
		/// "dlls" folders - added to dll search path.
		/// the scan performed from application root folder.
		/// </summary>
		public static void ScanSpecialFolders() {
			ScanSpecialFolders(Utils.MapPath(@"~/"));
		}
	}
}
