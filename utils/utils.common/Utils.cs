using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace utils {
	public static class Utils {

		public static string MapPath(string path) {
			//TODO: GetEntryAssembly in some cases may return null, see msdn for details
			var assembly = Assembly.GetEntryAssembly();
			var baseDir = Path.GetDirectoryName(assembly.Location);
			string fullPath = null;
			if (path.StartsWith("~/")) {
				fullPath = Path.Combine(baseDir, path.Substring(2, path.Length - 2).Replace('/', '\\'));
			} else {
				fullPath = path.Replace('/', '\\');
			}
			return fullPath;
		}
		
		/// <summary>
		/// Add specified search pathes to environment variable PATH
		/// </summary>
		/// <param name="searchPath">pathes to add</param>
		public static void AddEnvironmentPath(IEnumerable<string> searchPath) {
			searchPath = searchPath.Select(x => x.Trim()).ToArray();
			string pathVar = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
			var pathes = pathVar.Split(';').Select(x=>x.Trim());
			Environment.SetEnvironmentVariable("PATH", 
				String.Join(";", 
					searchPath.Concat(pathes.Where(x => !searchPath.Contains(x)))
				)
			);
		}

		/// <summary>
		/// Add specified search path to environment variable PATH
		/// </summary>
		/// <param name="searchPath">path to add</param>
		public static void AddEnvironmentPath(string searchPath) {
			searchPath = searchPath.Trim();
			string pathVar = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
			var pathes = pathVar.Split(';').Select(x => x.Trim());
			Environment.SetEnvironmentVariable("PATH",
				String.Join(";",
					pathes.Where(x => x != searchPath).Prepend(searchPath)
				)
			);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetDllDirectory(string lpPathName);
	}
}
