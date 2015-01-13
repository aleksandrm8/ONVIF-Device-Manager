using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using onvif.services;
using utils;

namespace odm.ui {
	public class AppDefaults {

		static protected string SystemFolder {
			get {
				return "Synesis";
			}
		}
		static protected string AppDataFolder {
			get {
				return "Onvif Device Manager";
			}
		}
		static public string SystemFolderPath {
			get {
				//string path = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + SystemFolder + @"\";
				string path = AppDomain.CurrentDomain.BaseDirectory + @"\";// +SystemFolder + @"\";
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				return path;
			}
		}
		static public string AppDataPath {
			get {
				string path = SystemFolderPath;// +AppDataFolder + @"\";
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				return path;
			}
		}
		static public string MetadataFolderPath {
			get {
				string path = AppDataPath + @"meta\";
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				return path;
			}
		}
		static public FileInfo MetadataFileInfo {
			get {
				var fi = new FileInfo(Utils.MapPath("~/meta/metadata.txt"));
				if (!fi.Directory.Exists) {
					fi.Directory.Create();
				}
				return fi;
			}
		}
		static public string ConfigFolderPath {
			get {
				string path = AppDataPath + @"config\";
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				return path;
			}
		}

		/// <summary> default visual settings </summary>
		private static VisualSettings defaultVisualSettings = new VisualSettings() {
			ItemSelector_IsPropertiesExpanded = false,
			Events_IsEnabled = true,
			EngineerTool_IsEnabled = false,
			Snapshot_IsEnabled = true,
			Version = version,
			CustomAnalytics_IsEnabled = true,
			EventsCollect_IsEnabled = false,
			WndSize = new Rect(50, 50, 640, 480),
			WndState = WindowState.Normal,
			ui_video_rendering_fps = 30,
			Base_Subscription_Port = 8085,
			ShowVideoPlaybackStatistics = false,
			Event_Subscription_Type = VisualSettings.EventType.TRY_PULL,
			Transport_Type = TransportProtocol.rtsp,
			UseOnlyCommonFilterView = false,
			OpenInExternalWebBrowser = false,
            EnableGraphicAnnotation = true,
			DefEventFilter = ""
		};

		//TODO: is thread safety required?
		private static VisualSettings _visualSettings = null;
		/// <summary>
		/// current visual settings
		/// </summary>
		public static VisualSettings visualSettings {
			get {
				if (_visualSettings != null) {
					return _visualSettings;
				}
				var fi = new FileInfo(Path.Combine(ConfigFolderPath, VisualSettings.name));
				if (!fi.Exists) {
					_visualSettings = defaultVisualSettings;
					return _visualSettings;
				}
				try {
					using (var fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
						using (var xr = new XmlTextReader(fs)) {
							_visualSettings = xr.Deserialize<VisualSettings>();
						}
					}
				} catch (Exception err) {
					//swallow error
					dbg.Error(err);
					_visualSettings = defaultVisualSettings;
				}
				return _visualSettings;
			}
		}
		//TODO: is thread safety required?
		/// <summary>
		/// save visual settings and set as current if succeeded
		/// </summary>
		/// <param name="visualSettings">visual settings to save</param>
		public static void UpdateVisualSettings(VisualSettings visualSettings) {
			if (visualSettings == null) {
				visualSettings = defaultVisualSettings;
			}
			var fi = new FileInfo(Path.Combine(ConfigFolderPath, VisualSettings.name));
			if (!fi.Exists) {
				fi.Directory.Create();
			}
			try {
				using (var fs = fi.Open(FileMode.Create, FileAccess.Write, FileShare.None)) {
					using (var xw = new XmlTextWriter(fs, Encoding.UTF8)) {
						xw.Formatting = Formatting.Indented;
						visualSettings.Serialize().WriteTo(xw);
					}
				}
				_visualSettings = visualSettings;
			} catch {
				log.WriteError("failed to update visual settings");
				dbg.Break();
			}
		}

		//TODO: is thread safety required?
		private static string _version = null;
		/// <summary>
		/// current version of visual settings
		/// </summary>
		public static string version {
			get {
				if (_version == null) {
					var assembly = Assembly.GetExecutingAssembly();
					Version ver = assembly.GetName().Version;
					_version = "v" + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString();
				}
				return _version;
			}
		}
	}

	[XmlRootAttribute(ElementName = "VisualSettings", IsNullable = false)]
	public class VisualSettings {
		public VisualSettings() {
			Events_IsEnabled = true;
			Snapshot_IsEnabled = true;
			ItemSelector_IsPropertiesExpanded = false;
			EngineerTool_IsEnabled = false;
			EventsCollect_IsEnabled = false;
			CustomAnalytics_IsEnabled = true;
			wndSize = new Rect(50, 50, 640, 480);
			WndState = WindowState.Normal;
			ui_video_rendering_fps = 30;
			Event_Subscription_Type = EventType.TRY_PULL;
			Base_Subscription_Port = 8085;
			ShowVideoPlaybackStatistics = false;
			Transport_Type = TransportProtocol.rtsp;
			UseOnlyCommonFilterView = false;
			//TODO: railway build
			DefEventFilter = "";
			OpenInExternalWebBrowser = false;
            EnableGraphicAnnotation = true;
		}
		//TODO: railway build
		public string DefEventFilter { get; set; }

		public string Version { get; set; }
		public enum TlsMode {
			USE_ALWAYS,
			USE_IF_POSSIBLE
		}
		public enum EventType {
			ONLY_PULL,
			TRY_PULL,
			ONLY_BASE
		}
		public bool OpenInExternalWebBrowser { get; set; }
        public bool EnableGraphicAnnotation { get; set; }
		public bool UseOnlyCommonFilterView { get; set; }
		public bool ShowVideoPlaybackStatistics { get; set; }
		public int Base_Subscription_Port { get; set; }
		public EventType Event_Subscription_Type { get; set; }
		public TransportProtocol Transport_Type { get; set; }
		public static readonly string name = "VisualSettings.xml";
		public bool EngineerTool_IsEnabled { get; set; }
		public int ui_video_rendering_fps { get; set; }
		public bool ItemSelector_IsPropertiesExpanded { get; set; }
		public bool Events_IsEnabled { get; set; }
		public bool EventsCollect_IsEnabled { get; set; }
		public bool Snapshot_IsEnabled { get; set; }
		public bool CustomAnalytics_IsEnabled { get; set; }
		Rect wndSize;
		public Rect WndSize {
			get {
				return wndSize;
			}
			set {
				wndSize = value;
				wndSize.X = wndSize.X < 0 ? 0 : wndSize.X;
				wndSize.Y = wndSize.Y < 0 ? 0 : wndSize.Y;
			}
		}
		public WindowState WndState { get; set; }
	}
	[XmlRootAttribute(ElementName = "MetaConfig", IsNullable = false)]
	public class MetaConfig {
		public static readonly string name = "MetaConfig.xml";
		public bool CollectMeta { get; set; }
	}
}
