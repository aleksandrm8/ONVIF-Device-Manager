using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
//using System.ServiceModel;
using odm.core;
using odm.ui.core;
using odm.ui.links;
using odm.ui.views;
using onvif.services;
using onvif.utils;
using utils;

namespace odm.ui.viewModels {
	public class ChannelViewModel : DependencyObject, INotifyPropertyChanged, IDisposable {
		SerialDisposable snapshotSubscription = new SerialDisposable();
		CompositeDisposable subscriptions = new CompositeDisposable();
		Capabilities devcap;
		OdmSession _facade;
		OdmSession facade {
			get {
				if (_facade == null)
					_facade = new OdmSession(session);
				return _facade;
			}
		}
		IEventAggregator eventAggregator;
		SubscriptionToken UIsubscription;
		SubscriptionToken RefreshSubscription;
		SubscriptionToken ProfileSubscription;
		VideoSource videoSource;
		IUnityContainer container;
		IChannel channel;
		//Profile profile;
		INvtSession session;

		VideoInfo dataProcInfo;
		//Timer tmr;

		public odm.controllers.InfoFormStrings InfoStrings { get { return odm.controllers.InfoFormStrings.instance; } }
		public LocalSnapshot SnapshotTooltip { get { return LocalSnapshot.instance; } }

		public ChannelViewModel(INvtSession session, ChannelDescription channelDescr, IUnityContainer container, Capabilities devcap) {
			this.eventAggregator = container.Resolve<IEventAggregator>();
			this.devcap = devcap;
			UIsubscription = eventAggregator.GetEvent<VideoChangedEvent>().Subscribe(VideoChangedEvent, false);
			RefreshSubscription = eventAggregator.GetEvent<RefreshSelection>().Subscribe(RefreshSelection, false);
			ProfileSubscription = eventAggregator.GetEvent<ProfileChangedEvent>().Subscribe(ProfileChangedEvent, false);
			this.videoSource = channelDescr.videoSource;
			this.session = session;
			this.container = container;
			Buttons = new ObservableCollection<ButtonBase>();
			Name = InfoStrings.loadingData;

			SnapShotClick = new DelegateCommand(() => {
				if (profToken != null) {
					GetSnapshot(profToken);
				}
			});

			BindData();
			Load();
		}
		void RefreshSelection(object sender) {
			if (sender != this) {
				SelectedButton = null;
			}
		}
		public void Dispose() {
			//if (tmr != null)
			//    tmr.Dispose();

			//            if (playThread != null) {
			//              playThread.Abort();
			//        }

			//StopVideoStreaming();

			eventAggregator.GetEvent<VideoChangedEvent>().Unsubscribe(UIsubscription);
			eventAggregator.GetEvent<VideoChangedEvent>().Unsubscribe(RefreshSubscription);
			eventAggregator.GetEvent<ProfileChangedEvent>().Unsubscribe(ProfileSubscription);
			if (snapshotSubscription != null && snapshotSubscription.Disposable != null)
				snapshotSubscription.Disposable.Dispose();
			if (subscriptions != null)
				subscriptions.Dispose();
			subscriptions = new CompositeDisposable();
			snapshotSubscription = new SerialDisposable();
		}

		void VideoChangedEvent(ChannelLinkEventArgs args) {
			if (args.token == videoSource.token) {
				//StopVideoStreaming();
				LoadFromToken(args.profile.token);
			}
		}

		void BindData() {
			this.CreateBinding(StateCommonProperty, this, x => { return x.Current == States.Common ? Visibility.Visible : Visibility.Collapsed; });
			this.CreateBinding(StateLoadingProperty, this, x => { return x.Current == States.Loading ? Visibility.Visible : Visibility.Collapsed; });
			this.CreateBinding(StateErrorProperty, this, x => { return x.Current == States.Error ? Visibility.Visible : Visibility.Collapsed; });
		}

		void ProfileChangedEvent(ProfileChangedEventArgs evargs) {
			if (evargs.vsToken != videoSource.token)
				return;
			LoadFromToken(evargs.profToken);
		}

		void LoadFromToken(string profToken) {
			subscriptions.Add(
				session.GetProfiles()
					.ObserveOnCurrentDispatcher()
					.Subscribe(profiles => {
						Profile prof = profiles
							.Where(x => {
								return x.token == profToken;
							})
							.FirstOrDefault();
						Load(prof);
					}, err => {
						dbg.Error(err);
						Load(null);
					})
			);
		}

		void CallDefaultProfile(string chanTok) {
			string defProfile = null;
			subscriptions.Add(session.GetProfiles()
				.ObserveOnCurrentDispatcher()
				.Subscribe(profiles => {
					Profile prof = profiles.Where(x => {
						if (x.videoSourceConfiguration == null)
							return false;
						return x.videoSourceConfiguration.sourceToken == chanTok;
					}).FirstOrDefault();
					if (prof == null)
						defProfile = null;
					else {
						defProfile = prof.token;
					}

					Load(prof);
				}, err => {
					dbg.Error(err);

					Load(null);
				}));
		}

		void FillVideoInfo() {
			dataProcInfo = new VideoInfo();
			try {
				dataProcInfo.ChanToken = videoSource.token;
				//dataProcInfo.MediaUri = channel.mediaUri.Uri;
				//dataProcInfo.Resolution = new System.Windows.Size(channel.encoderResolution.Width, channel.encoderResolution.Height);
			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		string profToken;
		void Load(Profile profile) {
			if (profile == null) {
				//dbg.Error("Could not find profile");
				//ErrorMessage = "Could not find profile";
				//Current = States.Error;
				//ErrorBtnClick = new DelegateCommand(() => { Load(); });
				Current = States.Common;
				Buttons.Clear();
				string proftoken = null;
				Buttons.Clear();
				CreateEmergencyButtons(proftoken);
			} else {
				var profileToken = profile.token;
				Current = States.Loading;
				Buttons.Clear();
				//StopVideoStreaming();

				string curProfToken;
				if (profileToken == null)
					curProfToken = null;
				else
					curProfToken = profileToken;

				profToken = curProfToken;

				subscriptions.Add(facade.GetChannel(curProfToken)
					.ObserveOnCurrentDispatcher()
					.Subscribe(channel => {
						Current = States.Common;
						Name = videoSource.token + " : " + channel.name;
						curProfToken = channel.profileToken;
						this.channel = channel;

						//starts only servers
						//StartVideoStreaming();

						FillVideoInfo();

						CreateButtons(profile);
					}, err => {
						dbg.Error(err);
						ErrorMessage = err.Message;
						Current = States.Error;
						ErrorBtnClick = new DelegateCommand(() => { Load(); });
					}));

				GetSnapshot(curProfToken);
			}
		}
		//static internal ImageSource doGetImageSourceFromResource(string psAssemblyName, string psResourceName) {
		//    Uri oUri = new Uri("pack://application:,,,/" + psAssemblyName + ";component/" + psResourceName, UriKind.RelativeOrAbsolute);
		//    return BitmapFrame.Create(oUri);
		//}
		void GetSnapshot(string curProfToken) {
			//Snapshot = doGetImageSourceFromResource("odm-ui-views", "images/loading-icon.png");
			//ImageSource imgsrc = doGetImageSourceFromResource("odm-ui-views", "images/snapshot.png");
			Snapshot = Resources.loading_icon.ToBitmapSource();
			ImageSource imgsrc = Resources.snapshot.ToBitmapSource();
			snapshotToolTip = LocalDevice.instance.loading;

			this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.loading);

			if (AppDefaults.visualSettings.Snapshot_IsEnabled) {
				snapshotSubscription.Disposable =
					facade.GetSnapshot(curProfToken)
					.ObserveOnCurrentDispatcher()
					.Subscribe(imgStream => {
						try {
							BitmapImage bitmap = new BitmapImage();
							bitmap.BeginInit();
							bitmap.CacheOption = BitmapCacheOption.OnLoad;
							bitmap.StreamSource = imgStream;
							bitmap.EndInit();
							Snapshot = bitmap;
							this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.snapshot);
						} catch (Exception err) {
							dbg.Error(err);
							Snapshot = imgsrc;
							this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.imagecorrupt);
						}
					}, err => {
						dbg.Error(err);
						Snapshot = imgsrc;
						snapshotToolTip = "shapshot loading error";
						this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.imageloadingerror);
					});
			} else {
				Snapshot = imgsrc;
				this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.disabled);
			}
		}
		void Load() {
			CallDefaultProfile(videoSource.token);
		}
		void CreateEmergencyButtons(string profToken) {
			var curAccount = AccountManager.Instance.CurrentAccount;
			Name = "No profile available. Video source name: " + profToken;
			//Snapshot = doGetImageSourceFromResource("odm-ui-views", "images/snapshot.png");
			Snapshot = Resources.snapshot.ToBitmapSource();
			snapshotToolTip = "No image available";
			Buttons.Add(new ProfilesButton(container.Resolve<EventAggregator>(), session, videoSource.token, null, curAccount, dataProcInfo));
		}
		void CreateButtons(Profile profile) {
			var curAccount = AccountManager.Instance.CurrentAccount;

			if (profile.videoEncoderConfiguration != null) {
				Buttons.Add(new LiveVideoButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));
				Buttons.Add(new VideoStreamingButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));
			}
			if (devcap.imaging != null && devcap.imaging.xAddr != null && devcap.imaging.xAddr != string.Empty)
				Buttons.Add(new ImagingButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));

			if (profile.videoEncoderConfiguration != null) {
				if (profile.videoAnalyticsConfiguration != null && devcap.analytics != null && devcap.analytics.analyticsModuleSupport)
					Buttons.Add(new AnalyticsButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));
				if (profile.videoAnalyticsConfiguration != null && devcap.analytics != null && devcap.analytics.ruleSupport)
					Buttons.Add(new RulesButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));
				if (profile.metadataConfiguration != null)
					Buttons.Add(new MetadataButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));
				if (profile.ptzConfiguration != null)
					Buttons.Add(new PTZButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));
			}
			Buttons.Add(new ProfilesButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));

			//Buttons.Add(new UITestButton(container.Resolve<EventAggregator>(), session, videoSource.token, profile, curAccount, dataProcInfo));
		}

		#region Commands
		void InitCommands() {
			ErrorBtnClick = new DelegateCommand(() => {
				//LoadModel(CurrentSession);
			});
		}

		public ICommand ErrorBtnClick {
			get { return (ICommand)GetValue(ErrorBtnClickProperty); }
			set { SetValue(ErrorBtnClickProperty, value); }
		}
		// Using a DependencyProperty as the backing store for ErrorBtnClick.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ErrorBtnClickProperty =
			DependencyProperty.Register("ErrorBtnClick", typeof(ICommand), typeof(ChannelViewModel));

		#endregion

		#region States
		public enum States {
			Loading,
			Common,
			Error
		}
		States current;
		public States Current {
			get {
				return current;
			}
			set {
				current = value;
				OnPropertyChanged(() => Current);
			}
		}
		protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyEvaluator) {
			var lambda = propertyEvaluator as LambdaExpression;
			var member = lambda.Body as MemberExpression;
			var handler = PropertyChanged;
			if (handler != null) {
				handler(this, new PropertyChangedEventArgs(member.Member.Name));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region StatesProperties
		public Visibility StateCommon {
			get { return (Visibility)GetValue(StateCommonProperty); }
			set { SetValue(StateCommonProperty, value); }
		}
		// Using a DependencyProperty as the backing store for StateCommon.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StateCommonProperty =
			DependencyProperty.Register("StateCommon", typeof(Visibility), typeof(ChannelViewModel), new PropertyMetadata(Visibility.Collapsed));

		public Visibility StateError {
			get { return (Visibility)GetValue(StateErrorProperty); }
			set { SetValue(StateErrorProperty, value); }
		}
		// Using a DependencyProperty as the backing store for StateError.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StateErrorProperty =
			DependencyProperty.Register("StateError", typeof(Visibility), typeof(ChannelViewModel), new PropertyMetadata(Visibility.Collapsed));

		public Visibility StateLoading {
			get { return (Visibility)GetValue(StateLoadingProperty); }
			set { SetValue(StateLoadingProperty, value); }
		}
		// Using a DependencyProperty as the backing store for StateLoading.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StateLoadingProperty =
			DependencyProperty.Register("StateLoading", typeof(Visibility), typeof(ChannelViewModel), new PropertyMetadata(Visibility.Collapsed));

		public string ErrorMessage {
			get { return (string)GetValue(ErrorMessageProperty); }
			set { SetValue(ErrorMessageProperty, value); }
		}
		// Using a DependencyProperty as the backing store for ErrorMessage.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ErrorMessageProperty =
			DependencyProperty.Register("ErrorMessage", typeof(string), typeof(ChannelViewModel));

		#endregion StatesProperties

		public string snapshotToolTip {
			get { return (string)GetValue(snapshotToolTipProperty); }
			set { SetValue(snapshotToolTipProperty, value); }
		}
		public static readonly DependencyProperty snapshotToolTipProperty =
			DependencyProperty.Register("snapshotToolTip", typeof(string), typeof(ChannelViewModel));

		public DelegateCommand SnapShotClick {
			get { return (DelegateCommand)GetValue(SnapShotClickProperty); }
			set { SetValue(SnapShotClickProperty, value); }
		}
		public static readonly DependencyProperty SnapShotClickProperty =
			DependencyProperty.Register("SnapShotClick", typeof(DelegateCommand), typeof(ChannelViewModel));

		public string Name {
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}
		// Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register("Name", typeof(string), typeof(ChannelViewModel), new UIPropertyMetadata("test"));

		public ObservableCollection<ButtonBase> Buttons {
			get { return (ObservableCollection<ButtonBase>)GetValue(ButtonsProperty); }
			set { SetValue(ButtonsProperty, value); }
		}
		// Using a DependencyProperty as the backing store for Buttons.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonsProperty =
			DependencyProperty.Register("Buttons", typeof(ObservableCollection<ButtonBase>), typeof(ChannelViewModel));

		public ButtonBase SelectedButton {
			get { return (ButtonBase)GetValue(SelectedButtonProperty); }
			set { SetValue(SelectedButtonProperty, value); }
		}
		// Using a DependencyProperty as the backing store for SelectedButton.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedButtonProperty =
			DependencyProperty.Register("SelectedButton", typeof(ButtonBase), typeof(ChannelViewModel), new FrameworkPropertyMetadata(null, (obj, ev) => {
				if (ev.NewValue == null)
					return;
				try {
					var btn = ev.NewValue as ButtonBase;
					btn.ButtonClick();
					var vm = (ChannelViewModel)obj;
					vm.eventAggregator.GetEvent<RefreshSelection>().Publish(obj);
				} catch (Exception err) {
					dbg.Error(err);
				}
			}));

		public ImageSource Snapshot {
			get { return (ImageSource)GetValue(SnapshotProperty); }
			set { SetValue(SnapshotProperty, value); }
		}
		// Using a DependencyProperty as the backing store for Snapshot.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SnapshotProperty =
			DependencyProperty.Register("Snapshot", typeof(ImageSource), typeof(ChannelViewModel));


	}

	public class ImageHelper {
		public static readonly DependencyProperty IsClickProperty =
			DependencyProperty.RegisterAttached("IsClick", typeof(bool), typeof(ImageHelper), new PropertyMetadata(OnIsClickChanged));

		public static bool GetIsClick(DependencyObject dependencyObject) {
			return (bool)dependencyObject.GetValue(IsClickProperty);
		}

		public static void SetIsClick(DependencyObject dependencyObject, bool IsClck) {
			dependencyObject.SetValue(IsClickProperty, IsClck);
		}

		private static void OnIsClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var img = (System.Windows.Controls.Image)d;
			img.MouseDown += new MouseButtonEventHandler((obj, ev) => {
				if (GetClick(img).CanExecute())
					GetClick(img).Execute();
			});
		}

		//private static void OnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		//    var img = (System.Windows.Controls.Image)d;
		//    img.PreviewMouseRightButtonDown += new MouseButtonEventHandler((obj, ev) => {

		//    });
		//}
		public static readonly DependencyProperty ClickProperty =
			DependencyProperty.RegisterAttached("Click", typeof(DelegateCommand), typeof(ImageHelper));//, new PropertyMetadata(OnClickChanged));

		public static DelegateCommand GetClick(DependencyObject dependencyObject) {
			return (DelegateCommand)dependencyObject.GetValue(ClickProperty);
		}

		public static void SetClick(DependencyObject dependencyObject, DelegateCommand Clck) {
			dependencyObject.SetValue(IsClickProperty, Clck);
		}

	}
}
