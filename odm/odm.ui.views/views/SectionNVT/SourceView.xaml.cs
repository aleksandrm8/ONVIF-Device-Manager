using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using onvif.utils;
using odm.core;
using onvif.services;
using Microsoft.FSharp.Control;
using odm.ui.core;
using utils;
using Microsoft.FSharp.Core;
using System.Reactive.Disposables;
using odm.ui.links;
using Microsoft.Practices.Prism.Events;
using System.Reactive.Linq;
using Microsoft.Practices.Unity;
using odm.ui.controls;

namespace odm.ui.views.SectionNVT {
	/// <summary>
	/// Interaction logic for SourceView.xaml
	/// </summary>
	public partial class SourceView : UserControl, IDisposable {
		#region Loader
		public static FSharpAsync<SourceViewArgs> Load(ChannelDescription channelDescr,
														Capabilities capabilities,
														INvtSession nvtSession,
														OdmSession odmsession,
														string prof = null) {
			SourceViewArgs args = new SourceViewArgs();
			args.nvtSession = nvtSession;
			args.odmSession = odmsession;
			args.capabilities = capabilities;
			args.channelDescr = channelDescr;
			args.selectedProfileToken = prof;

			return Apm.Iterate(LoadImpl(args)).Select(f => { return args; });
		}
		static IEnumerable<FSharpAsync<Unit>> LoadImpl(SourceViewArgs args) {
			Profile[] profiles;
			yield return args.nvtSession.GetProfiles().Select(pr => {
				profiles = pr;
				if (args.selectedProfileToken == null) {
					//load default (first in list)
					var prof = pr.FirstOrDefault(f => {
						if (f.videoSourceConfiguration == null)
							return false;//to show profile even if it corrupted
						return f.videoSourceConfiguration.sourceToken == args.channelDescr.videoSource.token;
					});
					if (prof == null) {
						args.selectedProfile = null;
						args.selectedProfileToken = null;
						return (Unit)null;
					}
					args.selectedProfile = prof;
					args.selectedProfileToken = args.selectedProfile.token;
				} else {
					var prof = pr.FirstOrDefault(f => {
						if (f.videoSourceConfiguration == null)
							return false;
						return f.videoSourceConfiguration.sourceToken == args.channelDescr.videoSource.token && f.token == args.selectedProfileToken;
					});
					if (prof == null) {
						args.selectedProfile = null;
						args.selectedProfileToken = null;
						return (Unit)null;
					}
					args.selectedProfile = prof;
				}
				return (Unit)null;
			});
		}
		#endregion Loader
		public SourceView(IUnityContainer container) {
			InitializeComponent();

			this.container = container;
		}

		CompositeDisposable disposables = new CompositeDisposable();
		List<ButtonBase> Buttons = new List<ButtonBase>();
		IEventAggregator eventAggregator;
		IUnityContainer container;
		VideoInfo dataProcInfo;
		LinkButtonSwitch btnSwitch;

		public void Init(SourceViewArgs args) {
			eventAggregator = container.Resolve<IEventAggregator>();

			btnSwitch = new LinkButtonSwitch(eventAggregator);
			btnSwitch.ClearSelection = () => {
				buttonsList.UnselectAll();
			};

			disposables.Add(Observable.FromEventPattern(buttonsList, "SelectionChanged")
				.Subscribe(e => {
					btnSwitch.SelectedButton = (ButtonBase)buttonsList.SelectedItem;
				}));
			disposables.Add(btnSwitch);

			dataProcInfo = new VideoInfo();

			GetSnapshot(args);

			if (args.selectedProfile == null) {
				log.WriteInfo("Selected profile == null");
				CreateEmergencyButtons(args);
			} else {
				dataProcInfo.ChanToken = args.channelDescr.videoSource.token;

				LoadButtons(args);
				disposables.Add(Observable.FromEventPattern(vsSnapshot, "MouseDown").Subscribe(ev => { GetSnapshot(args); }));
			}
			buttonsList.ItemsSource = Buttons;


		}
		void GetSnapshot(SourceViewArgs args) {
			vsSnapshot.Source = odm.ui.Resources.loading_icon.ToBitmapSource();

			ImageSource imgsrc = odm.ui.Resources.snapshot.ToBitmapSource();
			vsSnapshot.CreateBinding(Image.ToolTipProperty, LocalSnapshot.instance, x => x.loading);

			if (AppDefaults.visualSettings.Snapshot_IsEnabled) {
				disposables.Add(args.odmSession.GetSnapshot(args.selectedProfileToken)
					.ObserveOnCurrentDispatcher()
					.Subscribe(imgStream => {
						try {
							BitmapImage bitmap = new BitmapImage();
							bitmap.BeginInit();
							bitmap.CacheOption = BitmapCacheOption.OnLoad;
							bitmap.StreamSource = imgStream;
							bitmap.EndInit();
							vsSnapshot.Source = bitmap;
							vsSnapshot.CreateBinding(Image.ToolTipProperty, LocalSnapshot.instance, x => x.snapshot);
						} catch (Exception err) {
							dbg.Error(err);
							vsSnapshot.Source = imgsrc;
							vsSnapshot.CreateBinding(Image.ToolTipProperty, LocalSnapshot.instance, x => x.imagecorrupt);
						}
					}, err => {
						dbg.Error(err);
						vsSnapshot.Source = odm.ui.Resources.snapshot.ToBitmapSource();
						vsSnapshot.CreateBinding(Image.ToolTipProperty, LocalSnapshot.instance, x => x.imageloadingerror);
					}));
			} else {
				vsSnapshot.Source = odm.ui.Resources.snapshot.ToBitmapSource();
				vsSnapshot.CreateBinding(Image.ToolTipProperty, LocalSnapshot.instance, x => x.disabled);
			}
		}
		void CreateEmergencyButtons(SourceViewArgs args) {
			var curAccount = AccountManager.Instance.CurrentAccount;
			//Name = "No profile available. Video source name: " + profToken;
			////Snapshot = doGetImageSourceFromResource("odm-ui-views", "images/snapshot.png");
			//Snapshot = Resources.snapshot.ToBitmapSource();
			//snapshotToolTip = "No image available";
			Buttons.Add(new ProfilesButton(container.Resolve<EventAggregator>(), args.nvtSession, args.channelDescr.videoSource.token, null, curAccount, dataProcInfo));
		}
		void LoadButtons(SourceViewArgs args) {
			var curAccount = AccountManager.Instance.CurrentAccount;
			var vsToken = args.channelDescr.videoSource.token;

			if (args.selectedProfile.videoEncoderConfiguration != null) {
				Buttons.Add(new LiveVideoButton(container.Resolve<EventAggregator>(), args.nvtSession, vsToken, args.selectedProfile, curAccount, dataProcInfo));
				Buttons.Add(new VideoStreamingButton(container.Resolve<EventAggregator>(), args.nvtSession, vsToken, args.selectedProfile, curAccount, dataProcInfo));
			}

			var imgCaps = args.capabilities.imaging;
			if (imgCaps != null && !String.IsNullOrEmpty(imgCaps.xAddr)) {
				Buttons.Add(new ImagingButton(container.Resolve<EventAggregator>(), args.nvtSession, vsToken, args.selectedProfile, curAccount, dataProcInfo));
			}

			if (args.selectedProfile.videoAnalyticsConfiguration != null && args.capabilities.analytics != null && args.capabilities.analytics.analyticsModuleSupport)
				Buttons.Add(new AnalyticsButton(container.Resolve<EventAggregator>(), args.nvtSession, vsToken, args.selectedProfile, curAccount, dataProcInfo));
			if (args.selectedProfile.videoAnalyticsConfiguration != null && args.capabilities.analytics != null && args.capabilities.analytics.ruleSupport)
				Buttons.Add(new RulesButton(container.Resolve<EventAggregator>(), args.nvtSession, vsToken, args.selectedProfile, curAccount, dataProcInfo));
			if (args.selectedProfile.metadataConfiguration != null)
				Buttons.Add(new MetadataButton(container.Resolve<EventAggregator>(), args.nvtSession, vsToken, args.selectedProfile, curAccount, dataProcInfo));

			if (args.selectedProfile.videoEncoderConfiguration != null) {
				if (args.selectedProfile.ptzConfiguration != null)
					Buttons.Add(new PTZButton(container.Resolve<EventAggregator>(), args.nvtSession, vsToken, args.selectedProfile, curAccount, dataProcInfo));
			}
			Buttons.Add(new ProfilesButton(container.Resolve<EventAggregator>(), args.nvtSession, vsToken, args.selectedProfile, curAccount, dataProcInfo));

			//Buttons.Add(new UITestButton(container.Resolve<EventAggregator>(), args.nvtSession, videoSource.token, profile, curAccount, dataProcInfo));
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}

	public class SourceViewArgs {
		public String selectedProfileToken { get; set; }
		public Profile selectedProfile { get; set; }
		public INvtSession nvtSession { get; set; }
		public OdmSession odmSession { get; set; }
		public Capabilities capabilities { get; set; }
		public ChannelDescription channelDescr { get; set; }
	}
}
