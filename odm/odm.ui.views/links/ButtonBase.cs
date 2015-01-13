using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Commands;

using onvif.services;

using odm.core;
using odm.ui.views;
using odm.ui.core;
using odm.ui.controls;
using utils;
using odm.ui.viewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Windows.Controls.Primitives;

namespace odm.ui.links {
	public class LinkButtonSwitch : DependencyObject, IDisposable {
		public LinkButtonSwitch(IEventAggregator eventAggregator) {
			this.eventAggregator = eventAggregator;
			var RefreshSubscription = eventAggregator.GetEvent<RefreshSelection>().Subscribe(RefreshSelection, false);
			disposables.Add(Disposable.Create(() => {
				eventAggregator.GetEvent<RefreshSelection>().Unsubscribe(RefreshSubscription);
			}));
		}
		
		IEventAggregator eventAggregator;
		CompositeDisposable disposables = new CompositeDisposable();

		public Action ClearSelection;

		void RefreshSelection(object sender) {
			if (sender != this) {
				if (ClearSelection != null)
					ClearSelection();
			}
		}

		public ButtonBase SelectedButton { get { return (ButtonBase)GetValue(SelectedButtonProperty); } set { SetValue(SelectedButtonProperty, value); } }
		public static readonly DependencyProperty SelectedButtonProperty =
			DependencyProperty.Register("SelectedButton", typeof(ButtonBase), typeof(LinkButtonSwitch), new FrameworkPropertyMetadata(null, (obj, ev) => {
				if (ev.NewValue == null) {
					return;
				}
				try {
					var btn = ev.NewValue as ButtonBase;
					btn.ButtonClick();
				} catch (Exception err) {
					//swallow error
					log.WriteError("button click handler raise exception: " + err.Message);
					dbg.Break();
				}
				var vm = (LinkButtonSwitch)obj;
				vm.eventAggregator.GetEvent<RefreshSelection>().Publish(obj);
			}));


		public void Dispose() {
			disposables.Dispose();
		}
	}
	public class ButtonBase : ToggleButton{
        public ButtonBase(IEventAggregator eventAggregator, INvtSession session, Account currentAccount) {
			this.eventAggregator = eventAggregator;
			this.session = session;
            this.currentAccount = currentAccount;
			InitCommands();
		
			this.Cursor = Cursors.Hand;
		}
		#region Commands
		void InitCommands() {
			BtnClick = new DelegateCommand(() => {
				ButtonClick();
			});
		}
		public ICommand BtnClick {
			get { return (ICommand)GetValue(BtnClickProperty); }
			set { SetValue(BtnClickProperty, value); }
		}
		// Using a DependencyProperty as the backing store for ErrorBtnClick.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BtnClickProperty =
			DependencyProperty.Register("BtnClick", typeof(ICommand), typeof(ButtonBase));
		public virtual void ButtonClick() { 
		}

        public virtual void CheckButtonSwitched(bool current) { 
        }
		#endregion

		protected IEventAggregator eventAggregator;
		protected INvtSession session;
        protected Account currentAccount;

		public string LinkName {get { return (string)GetValue(LinkNameProperty); }set { SetValue(LinkNameProperty, value); }}
		public static readonly DependencyProperty LinkNameProperty = DependencyProperty.Register("LinkName", typeof(string), typeof(ButtonBase));

		public bool IsChBoxEnabled {get { return (bool)GetValue(IsChBoxEnabledProperty); }set { SetValue(IsChBoxEnabledProperty, value); }}
		public static readonly DependencyProperty IsChBoxEnabledProperty = DependencyProperty.Register("IsChBoxEnabled", typeof(bool), typeof(ButtonBase), new UIPropertyMetadata(true));

		public Visibility IsCheckBox {get { return (Visibility)GetValue(IsCheckBoxProperty); }set { SetValue(IsCheckBoxProperty, value); }}
		public static readonly DependencyProperty IsCheckBoxProperty = DependencyProperty.Register("IsCheckBox", typeof(Visibility), typeof(ButtonBase), new UIPropertyMetadata(Visibility.Hidden));

		public bool IsChBoxChecked {get { return (bool)GetValue(IsChBoxCheckedProperty); }set { SetValue(IsChBoxCheckedProperty, value); }}
        public static readonly DependencyProperty IsChBoxCheckedProperty = DependencyProperty.Register("IsChBoxChecked", typeof(bool), typeof(ButtonBase), new UIPropertyMetadata(true));

	}
    public class ChannelButtonBase : ButtonBase{
        public ChannelButtonBase(IEventAggregator eventAggregator, INvtSession session, Account currentAccount, String channelToken, Profile profile, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount) {
            this.channelToken = channelToken;
            this.profile = profile;
            this.videoInfo = videoInfo;
	    }
        protected String channelToken;
		protected Profile profile;
        protected IVideoInfo videoInfo;
        protected ChannelLinkEventArgs GetEventArg(){
            var evArg = new ChannelLinkEventArgs();
            evArg.currentAccount = currentAccount;
            evArg.profile = profile;
            evArg.session = session;
            evArg.token = channelToken;
            evArg.videoInfo = videoInfo;
            return evArg;
        }
    }
    public class DeviceButtonBase : ButtonBase{
        public DeviceButtonBase(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
	    }
        protected DeviceLinkEventArgs GetEventArg(){
            var evArg = new DeviceLinkEventArgs();
            evArg.currentAccount = currentAccount;
            evArg.session = session;
            return evArg;
        }
    }
	public class NVAButtonBase : ButtonBase{
		public NVAButtonBase(IEventAggregator eventAggregator, INvtSession session, Account currentAccount, AnalyticsEngine engine, AnalyticsEngineControl control, IVideoInfo videoInfo = null)
            : base(eventAggregator, session, currentAccount) {
            this.engine = engine;
            this.control = control;
            this.videoInfo = videoInfo;
	    }
        protected AnalyticsEngine engine;
		protected AnalyticsEngineControl control;
        protected IVideoInfo videoInfo;
        protected NVALinkEventArgs GetEventArg(){
            var evArg = new NVALinkEventArgs();
            evArg.currentAccount = currentAccount;
            evArg.control = control;
            evArg.session = session;
            evArg.engine = engine;
            evArg.videoInfo = videoInfo;
            return evArg;
        }
    }
	#region NVAButtons
	public class NVALiveVideoButton : NVAButtonBase {
		public NVALiveVideoButton(IEventAggregator eventAggregator, INvtSession session, AnalyticsEngine engine, AnalyticsEngineControl control, Account currentAccount, IVideoInfo videoInfo = null)
			: base(eventAggregator, session, currentAccount, engine, control, videoInfo) {
			Init();
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<NVALiveVideoClick>().Publish(GetEventArg());
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.liveVideo);
		}

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
	}
	public class NVAControlsButton : NVAButtonBase {
		public NVAControlsButton(IEventAggregator eventAggregator, INvtSession session, AnalyticsEngine engine, AnalyticsEngineControl control, Account currentAccount, IVideoInfo videoInfo=null)
			: base(eventAggregator, session, currentAccount, engine, control, videoInfo) {
			Init();
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<NVAControlsClick>().Publish(GetEventArg());
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.nvaControls);
		}

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
	}
	public class NVAAnalyticsButton : NVAButtonBase {
		public NVAAnalyticsButton(IEventAggregator eventAggregator, INvtSession session, AnalyticsEngine engine, AnalyticsEngineControl control, Account currentAccount, IVideoInfo videoInfo = null)
			: base(eventAggregator, session, currentAccount, engine, control, videoInfo) {
			Init();
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<NVAAnalyticsClick>().Publish(GetEventArg());
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.nvaAnalytics);
		}

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
	}
	public class NVAInputsButton : NVAButtonBase {
		public NVAInputsButton(IEventAggregator eventAggregator, INvtSession session, AnalyticsEngine engine, AnalyticsEngineControl control, Account currentAccount, IVideoInfo videoInfo = null)
			: base(eventAggregator, session, currentAccount, engine, control, videoInfo) {
			Init();
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<NVAInputsClick>().Publish(GetEventArg());
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.nvaInputs);
		}

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
	}
    public class NVAMetadataButton : NVAButtonBase
    {
        public NVAMetadataButton(IEventAggregator eventAggregator, INvtSession session, AnalyticsEngine engine, AnalyticsEngineControl control, Account currentAccount, IVideoInfo videoInfo = null)
            : base(eventAggregator, session, currentAccount, engine, control, videoInfo)
        {
            Init();
        }
        public override void ButtonClick()
        {
            eventAggregator.GetEvent<NVAMetadataClick>().Publish(GetEventArg());
        }
        void Init()
        {
            this.CreateBinding(LinkNameProperty, Titles, x => x.nvaMetadata);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
	public class NVASettingsButton : NVAButtonBase {
		public NVASettingsButton(IEventAggregator eventAggregator, INvtSession session, AnalyticsEngine engine, AnalyticsEngineControl control, Account currentAccount, IVideoInfo videoInfo = null)
			: base(eventAggregator, session, currentAccount, engine, control, videoInfo) {
			Init();
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<NVASettingsClick>().Publish(GetEventArg());
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.nvaSettings);
		}

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
	}
	#endregion NVAButtons

	#region ChannelsButtons
	public class AnalyticsButton : ChannelButtonBase {
        public AnalyticsButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<AnalyticsClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.analytics);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class RulesButton : ChannelButtonBase {
        public RulesButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<RulesClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.ruleEngine);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class PTZButton : ChannelButtonBase {
		public PTZButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<PTZClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.ptz);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class ProfilesButton : ChannelButtonBase {
		public ProfilesButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<ProfilesClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.profileEditor);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class LiveVideoButton : ChannelButtonBase {
		public LiveVideoButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<LiveVideoClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.liveVideo);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class VideoStreamingButton : ChannelButtonBase {
		public VideoStreamingButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<VideoStreamingClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.videoStreaming);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class ImagingButton : ChannelButtonBase {
		public ImagingButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<ImagingClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.sensorSettings);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class MetadataButton : ChannelButtonBase {
		public MetadataButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
                this.profile = profile;
            Init();
        }
        Profile profile;
        
        MetadataEventArgs GetMetaEventArg() {
            var evArg = new MetadataEventArgs();
            evArg.currentAccount = currentAccount;
            evArg.profileToken = profile.token;
            evArg.profile = profile;
            evArg.session = session;
            evArg.token = channelToken;
            evArg.videoInfo = videoInfo;
            return evArg;
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<MetadataClick>().Publish(this.GetMetaEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.metadata);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
	public class UITestButton : ChannelButtonBase {
		public UITestButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, Account currentAccount, IVideoInfo videoInfo)
			: base(eventAggregator, session, currentAccount, channelToken, profile, videoInfo) {
			this.profile = profile;
			Init();
		}
		Profile profile;

		UITestEventArgs GetTestEventArg() {
			var evArg = new UITestEventArgs();
			evArg.currentAccount = currentAccount;
			evArg.profileToken = profile.token;
			evArg.profile = profile;
			evArg.session = session;
			evArg.token = channelToken;
			evArg.videoInfo = videoInfo;
			return evArg;
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<UITestClick>().Publish(this.GetTestEventArg());
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.uiTest);
		}

		public LocalTitles Titles { get { return LocalTitles.instance; } }
	}
	#endregion

    #region DeviceButtons
    public class SequrityButton : DeviceButtonBase {
        public SequrityButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<SecurityClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.sequrity);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class UserManagerButton : DeviceButtonBase {
		public UserManagerButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount, string DevModel, string Manufact)
			: base(eventAggregator, session, currentAccount) {
			this.devName = DevModel;
			this.manufact = Manufact;
            Init();
        }
		string devName;
		string manufact;
		UserManagementEventArgs GetUserManagementEventArgs() {
			var evArg = new UserManagementEventArgs();
			evArg.DeviceModel = devName;
			evArg.Manufacturer = manufact;
			evArg.currentAccount = currentAccount;
			evArg.session = session;
			return evArg;
		}
        public override void ButtonClick() {
			eventAggregator.GetEvent<UserManagerClick>().Publish(GetUserManagementEventArgs());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.usermanager);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
	public class WebPageButton : DeviceButtonBase {
		public WebPageButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
			: base(eventAggregator, session, currentAccount) {
			Init();
		}
		public override void ButtonClick() {
			var vs = AppDefaults.visualSettings;
			if (vs.OpenInExternalWebBrowser) {
				try {
					var evarg = GetEventArg();
					Process.Start("IExplore.exe", evarg.session.deviceUri.GetLeftPart(UriPartial.Authority));
				} catch (Exception err) {
					dbg.Error(err);
				}
			} else {
				eventAggregator.GetEvent<WebPageClick>().Publish(GetEventArg());
			}
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.webPage);
		}

		public LocalTitles Titles { get { return LocalTitles.instance; } }
	}
	public class IdentificationButton : DeviceButtonBase {
        public IdentificationButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
			: base(eventAggregator, session, currentAccount) {
			Init();
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<IdentificationClick>().Publish(GetEventArg());			
		}
		void Init(){
			this.CreateBinding(LinkNameProperty, Titles, x => x.identificationAndStatus);
		}
		
		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
	}
	public class ReceiversButton : DeviceButtonBase {
		public ReceiversButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
			: base(eventAggregator, session, currentAccount) {
			Init();
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<ReceiversClick>().Publish(GetEventArg());
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, LocalTitles.instance, x => x.receivers);
		}

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
	}
    public class DateTimeButton : DeviceButtonBase
    {
        public DateTimeButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick(){
            eventAggregator.GetEvent<DateTimeClick>().Publish(GetEventArg());
        }
        void Init(){
            this.CreateBinding(LinkNameProperty, Titles, x => x.timesettings);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class DigitalIOButton : DeviceButtonBase {
        public DigitalIOButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<DigitalIOClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.digitalIO);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class ActionsButton : DeviceButtonBase {
        public ActionsButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<ActionsClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, LocalTitles.instance, x => x.actions);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class ActionTriggersButton : DeviceButtonBase {
        public ActionTriggersButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<ActionTriggersClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, LocalTitles.instance, x => x.triggers);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class DeviceEventsButton : DeviceButtonBase {
        public DeviceEventsButton(IEventAggregator eventAggregator, ObservableCollection<FilterExpression> filters, EventsStorage events, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
            this.events = events;
			this.filters = filters;
        }
		ObservableCollection<FilterExpression> filters;
        EventsStorage events;
        public override void ButtonClick() {
            DeviceEventsEventArgs evargs = new DeviceEventsEventArgs() {
                session = session,
                currentAccount = currentAccount,
                events = events,
				filters = filters
            };
            eventAggregator.GetEvent<DeviceEventsClick>().Publish(evargs);
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.events);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class MaintenanceButton : DeviceButtonBase{
        public MaintenanceButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount, global::onvif.services.Capabilities caps, string DevModel, string Manufact)
            : base(eventAggregator, session, currentAccount) {
                this.caps = caps;
				this.devName = DevModel;
				this.manufact = Manufact;
                Init();
        }
		string devName;
		string manufact;
        global::onvif.services.Capabilities caps;
        MaintenanceLinkEventArgs GetMaintenanceEventsArgs(){
            var evArg = new MaintenanceLinkEventArgs();
			evArg.DeviceModel = devName;
			evArg.Manufacturer = manufact;
            evArg.currentAccount = currentAccount;
            evArg.session = session;
            evArg.caps = caps;
            return evArg;
        }
        public override void ButtonClick(){
            eventAggregator.GetEvent<MaintenanceClick>().Publish(GetMaintenanceEventsArgs());
        }
        void Init(){
            this.CreateBinding(LinkNameProperty, Titles, x => x.maintenance);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class SystemLogButton : DeviceButtonBase {
        public SystemLogButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount, SysLogDescriptor slog)
            : base(eventAggregator, session, currentAccount) {
				this.slog = slog;
				Init();
        }
		SysLogDescriptor slog;

		SysLogLinkEventArgs GetSysLogEventsArgs() {
			var evArg = new SysLogLinkEventArgs();
			evArg.currentAccount = currentAccount;
			evArg.session = session;
			evArg.sysLog = this.slog;
			return evArg;
		}

        public override void ButtonClick() {
			eventAggregator.GetEvent<SystemLogClick>().Publish(GetSysLogEventsArgs());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.systemLog);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class NetworkButton : DeviceButtonBase
    {
        public NetworkButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick(){
            eventAggregator.GetEvent<NetworkClick>().Publish(GetEventArg());
        }
        void Init(){
            this.CreateBinding(LinkNameProperty, Titles, x => x.networkSettings);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class XMLExplorerButton : DeviceButtonBase
    {
        public XMLExplorerButton(IEventAggregator eventAggregator, INvtSession session, Account currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick(){
            eventAggregator.GetEvent<XMLExplorerClick>().Publish(GetEventArg());
        }
        void Init(){
            this.CreateBinding(LinkNameProperty, Titles, x => x.onvifExplorer);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    #endregion
}
