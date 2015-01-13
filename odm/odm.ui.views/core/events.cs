using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Microsoft.Practices.Prism.Events;
using odm.core;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.viewModels;
using onvif.services;
using onvif.utils;

namespace odm.ui.views {

    public class MetaDataConfigEvent : CompositePresentationEvent<string> { }

    public class UpgradeEventArgs {
        public OdmSession facade;
        public string path;
    }
    public class UpgradeStartedEvent : CompositePresentationEvent<UpgradeEventArgs> { }

    public class DeviceSelectedEventArgs { 
        public DeviceDescriptionHolder devHolder;
        public NvtSessionFactory sessionFactory;
    }
    public class DeviceSelectedEvent : CompositePresentationEvent<DeviceSelectedEventArgs> { }

    public class RefreshSelection : CompositePresentationEvent<object> { }
	public class ChannelLoadedEventArgs { 
		public INvtSession session;
		public String token;
	}
	public class ChannelLoadedEvent : CompositePresentationEvent<ChannelLoadedEventArgs> { }

    public class DeviceEventsEventArgs : DeviceLinkEventArgs {
        public EventsStorage events;
		public ObservableCollection<FilterExpression> filters;
    }
	public class MaintenanceLinkEventArgs : DeviceLinkEventArgs, IMaintenanceLinkEventArgs {
        public global::onvif.services.Capabilities caps;

		public string DeviceModel { get; set; }
		public string Manufacturer {get; set; }
	}
	public interface IMaintenanceLinkEventArgs {
		string DeviceModel { get; }
		string Manufacturer { get; }
	}
	public class UserManagementEventArgs : DeviceLinkEventArgs, IUserManagementEventArgs {
		public string DeviceModel { get; set; }
		public string Manufacturer { get; set; }
	}
	public interface IUserManagementEventArgs {
		string DeviceModel { get; }
		string Manufacturer { get; }
	}

	public class DeviceLinkEventArgs {
        public INvtSession session;
        public Account currentAccount;
    }
    public class BatchTaskEventArgs {
        public INvtSession session;
        public Account currentAccount;
        public List<DeviceDescriptionHolder> Devices;
    }
	public class SysLogLinkEventArgs {
		public INvtSession session;
		public Account currentAccount;
		public SysLogDescriptor sysLog;
	}

	public class SysLogType {
		public SysLogType(global::onvif.services.SystemLogType type) {
			this.type = type;
			Name = typeNames.ContainsKey(type) ? typeNames[type] : "";
		}
		Dictionary<global::onvif.services.SystemLogType, string> typeNames = new Dictionary<SystemLogType, string>() { 
			{ global::onvif.services.SystemLogType.access, "Access" }, 
			{ global::onvif.services.SystemLogType.system, "System" } 
		};
		public SystemLogType type;
		public string Name { get; set; }
	}
	public class SysLogDescriptor {
		public SysLogDescriptor(SysLogType logType, AttachmentData att, string log, bool isReceived = false) {
			this.LogType = LogType;
			this.Attachment = att;
			this.SystemLog = log;
			IsReceived = isReceived;
		}
		public bool IsReceived { get; private set; }

		public void FillData(global::onvif.services.SystemLog log, SysLogType slogType) {
			arriveTime = System.DateTime.Now;
			LogType = slogType;
			Attachment = log.binary;
			SystemLog = log.@string;

			IsReceived = true;
		}

		System.DateTime arriveTime;
		
		public string AttachmentFileName {
			get {
				return String.Format("{0:yyyy.MM.dd-hh.mm}-{1}.bin", arriveTime, LogType.Name);
			}
		}
		public string SysLogFileName {
			get {
				return String.Format("{0:yyyy.MM.dd-hh.mm}-{1}.txt", arriveTime, LogType.Name);
			}
		}
		
		public string Info {
			get {
				return String.Format("{1} log at {0:t} on {0:d}", arriveTime, LogType.Name);
			}
		}

		private SysLogType logType;
		public SysLogType LogType {
			get { return logType; }
			private set {logType = value; }
		}

		private AttachmentData attachment;
		public AttachmentData Attachment {
			get { return attachment; }
			private set {attachment = value; }
		}

		private string systemLog;
		public string SystemLog {
			get { return systemLog; }
			private set {systemLog = value; }
		}

	}
	public class SystemLogReceived : CompositePresentationEvent<SysLogDescriptor> { }

	#region commonDevice
	public class WebPageClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class IdentificationClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
	public class ReceiversClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class DateTimeClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class MaintenanceClick : CompositePresentationEvent<MaintenanceLinkEventArgs> { }
    public class SystemLogClick : CompositePresentationEvent<SysLogLinkEventArgs> { }
    public class DigitalIOClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class ActionsClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class ActionTriggersClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class DeviceEventsClick : CompositePresentationEvent<DeviceEventsEventArgs> { }
	public class AddEventsFilterClick : CompositePresentationEvent<DeviceEventsEventArgs> { }
    public class NetworkClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class XMLExplorerClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
	public class UserManagerClick : CompositePresentationEvent<UserManagementEventArgs> { }
    public class SecurityClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class AccountManagerClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class AppSettingsClick : CompositePresentationEvent<bool> { }
    public class UpgradeButchClick : CompositePresentationEvent<bool> { }
    public class UpgradeButchReady : CompositePresentationEvent<BatchTaskEventArgs> { }
    public class RestoreButchClick : CompositePresentationEvent<bool> { }
    public class RestoreButchReady : CompositePresentationEvent<BatchTaskEventArgs> { }
    public class BackgroundTasksClick : CompositePresentationEvent<bool> { }
    public class AboutClick : CompositePresentationEvent<DeviceLinkEventArgs> { }
    public class Refresh : CompositePresentationEvent<bool> { }
	public class ReleaseUI : CompositePresentationEvent<bool> { }
	#endregion commonDevice

	public class InitAccountEventArgs {
		public Account currentAccount;
		public bool needrefresh;
	}
	public class InitAccounts : CompositePresentationEvent<InitAccountEventArgs> { }

    public class MetadataEventArgs {
        public Profile profile;
        public INvtSession session;
        public String token;
        public string profileToken;
        public Account currentAccount;
        public IVideoInfo videoInfo;
    }
    public class ChannelLinkEventArgs {
        public INvtSession session;
        public String token;
        public Profile profile;
		public Account currentAccount;
        public IVideoInfo videoInfo;
    }
	public class UITestEventArgs {
		public Profile profile;
		public INvtSession session;
		public String token;
		public string profileToken;
		public Account currentAccount;
		public IVideoInfo videoInfo;
	}
	public class UITestClick : CompositePresentationEvent<UITestEventArgs> { }

	#region Channels
	public class ProfilesClick : CompositePresentationEvent<ChannelLinkEventArgs> { }
    public class PTZClick : CompositePresentationEvent<ChannelLinkEventArgs> { }
    public class AnalyticsClick : CompositePresentationEvent<ChannelLinkEventArgs> { }
    public class RulesClick : CompositePresentationEvent<ChannelLinkEventArgs> { }
    public class LiveVideoClick : CompositePresentationEvent<ChannelLinkEventArgs> { }
    public class VideoStreamingClick : CompositePresentationEvent<ChannelLinkEventArgs> { }
    public class MetadataClick : CompositePresentationEvent<MetadataEventArgs> { }
    public class EventsClick : CompositePresentationEvent<ChannelLinkEventArgs> { }
    public class ImagingClick : CompositePresentationEvent<ChannelLinkEventArgs> { }

    public class VideoChangedEvent : CompositePresentationEvent<ChannelLinkEventArgs> { }
	#endregion Channels

	#region NVAEvents
	public class NVALinkEventArgs {
		public INvtSession session;
		public AnalyticsEngine engine;
		public AnalyticsEngineControl control;
		public Account currentAccount;
		public IVideoInfo videoInfo;
	}
	public class ControlChangedEventArgs {
		public ControlChangedEventArgs(INvtSession session, AnalyticsEngine engine, string controlToken) {
			this.session = session;
			this.engine = engine;
			this.controlToken = controlToken;
		}
		public INvtSession session;
		public string controlToken;
		public AnalyticsEngine engine;
	}
	public class ControlChangedPreviewEvent : CompositePresentationEvent<ControlChangedEventArgs> { }
	public class ControlChangedEvent : CompositePresentationEvent<ControlChangedEventArgs> { }

	public class NVALiveVideoClick : CompositePresentationEvent<NVALinkEventArgs> { }
	public class NVAControlsClick : CompositePresentationEvent<NVALinkEventArgs> { }
	public class NVAAnalyticsClick : CompositePresentationEvent<NVALinkEventArgs> { }
	public class NVAInputsClick : CompositePresentationEvent<NVALinkEventArgs> { }
    public class NVAMetadataClick : CompositePresentationEvent<NVALinkEventArgs> { }
	public class NVASettingsClick : CompositePresentationEvent<NVALinkEventArgs> { }

	#endregion NVAEvents

	public class ProfileChangedEventArgs{
        public ProfileChangedEventArgs(INvtSession session, String vsToken, string profToken) {
            this.session = session;
	        this.vsToken = vsToken;
            this.profToken = profToken;
	    }
	    public INvtSession session;
        public String vsToken;
		public string profToken;
    }
    public class ProfileChangedPreviewEvent : CompositePresentationEvent<ProfileChangedEventArgs>{}
	public class ProfileChangedEvent : CompositePresentationEvent<ProfileChangedEventArgs> { }

    public class DeviceEventArgs {
        public DeviceEventArgs(String vsToken, EventDescriptor data) {
            this.vsToken = vsToken;
            this.data = data;
        }
        public String vsToken;
        public EventDescriptor data;
		public MessageContentFilter[] messageContentFilters;
		public TopicExpressionFilter[] topicExpressionFilters;
    }
    public class DeviceEventReceived : CompositePresentationEvent<DeviceEventArgs> { }
    public class DeviceMetadataArgs {
        public DeviceMetadataArgs(String vsToken, XmlDocument xml) {
            this.vsToken = vsToken;
			this.xml = xml;
        }
        public String vsToken;
		public XmlDocument xml;
    }
    public class MetadataEventReceived : CompositePresentationEvent<DeviceMetadataArgs> { }
}
