using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using odm.core;
using odm.ui;
using odm.ui.activities;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.viewModels;
using odm.ui.views;
using onvif.services;
using utils;
using System.Diagnostics;
using System.Linq;
using Microsoft.FSharp.Core;
using System.Collections.Generic;
using Microsoft.FSharp.Control;

namespace odm.controllers {
	public class ContentController {
		private IEventAggregator eventAggregator;
		public IEventAggregator EventAggregator {
			get {
				return eventAggregator;
			}
		}
		private IUnityContainer container;
		private IRegionManager regionManager;

		public ContentController(IUnityContainer container) {
			this.container = container;
			regionManager = container.Resolve<IRegionManager>();
			eventAggregator = container.Resolve<IEventAggregator>();

			Subscribe();
		}
		void Subscribe() {
			eventAggregator.GetEvent<Refresh>().Subscribe(Refresh, false);
			eventAggregator.GetEvent<ReleaseUI>().Subscribe(ReleaseUI, false); //only released ui

			eventAggregator.GetEvent<AboutClick>().Subscribe(AboutClick, false);

			eventAggregator.GetEvent<UpgradeStartedEvent>().Subscribe(UpgradeStartedEvent, false);

			eventAggregator.GetEvent<BackgroundTasksClick>().Subscribe(BackgroundTasksStartup, false);
			eventAggregator.GetEvent<AppSettingsClick>().Subscribe(AppSettingsStartup, false);
			eventAggregator.GetEvent<UpgradeButchReady>().Subscribe(UpgradeBatchStartup, false);
			eventAggregator.GetEvent<RestoreButchReady>().Subscribe(RestoreBatchStartup, false);
			eventAggregator.GetEvent<DeviceSelectedEvent>().Subscribe(OnSelectedDeviceChanged, false);

			eventAggregator.GetEvent<IdentificationClick>().Subscribe(IdentificationClick, false);
			eventAggregator.GetEvent<DateTimeClick>().Subscribe(DateTimeClick, false);
			eventAggregator.GetEvent<MaintenanceClick>().Subscribe(MaintenanceClick, false);
			eventAggregator.GetEvent<SystemLogClick>().Subscribe(SystemLogClick, false);
			eventAggregator.GetEvent<DigitalIOClick>().Subscribe(DigitalIOClick, false);
			eventAggregator.GetEvent<ActionsClick>().Subscribe(ActionsClick, false);
			eventAggregator.GetEvent<ActionTriggersClick>().Subscribe(ActionTriggersClick, false);
			eventAggregator.GetEvent<NetworkClick>().Subscribe(NetworkClick, false);
			//eventAggregator.GetEvent<XMLExplorerClick>().Subscribe(XmlExplorerClick, false);
			eventAggregator.GetEvent<UserManagerClick>().Subscribe(UserManagerClick, false);
			eventAggregator.GetEvent<SecurityClick>().Subscribe(SequrityClick, false);
			eventAggregator.GetEvent<WebPageClick>().Subscribe(WebPageClick, false);
			eventAggregator.GetEvent<DeviceEventsClick>().Subscribe(DeviceEventsClick, false);
			eventAggregator.GetEvent<ReceiversClick>().Subscribe(ReceiversClick, false);

			eventAggregator.GetEvent<AnalyticsClick>().Subscribe(AnalyticsClick, false);
			eventAggregator.GetEvent<RulesClick>().Subscribe(RulesClick, false);
			eventAggregator.GetEvent<ProfileChangedPreviewEvent>().Subscribe(ProfileChangedPreviewEvent, false);
			eventAggregator.GetEvent<ProfilesClick>().Subscribe(ProfilesClick, false);
			eventAggregator.GetEvent<PTZClick>().Subscribe(PTZClick, false);
			eventAggregator.GetEvent<LiveVideoClick>().Subscribe(LiveVideoClick, false);
			eventAggregator.GetEvent<ImagingClick>().Subscribe(ImagingClick, false);
			eventAggregator.GetEvent<VideoStreamingClick>().Subscribe(VideoStreamingClick, false);
			eventAggregator.GetEvent<MetadataClick>().Subscribe(MetadataClick, false);
			eventAggregator.GetEvent<UITestClick>().Subscribe(UITestClick, false);

			eventAggregator.GetEvent<NVALiveVideoClick>().Subscribe(NVALiveVideoClick, false);
			eventAggregator.GetEvent<NVAControlsClick>().Subscribe(NVAControlsClick, false);
			eventAggregator.GetEvent<NVAAnalyticsClick>().Subscribe(NVAAnalyticsClick, false);
			eventAggregator.GetEvent<NVAInputsClick>().Subscribe(NVAInputsClick, false);
			eventAggregator.GetEvent<NVAMetadataClick>().Subscribe(NVAMetadataClick, false);
			eventAggregator.GetEvent<NVASettingsClick>().Subscribe(NVASettingsClick, false);
			eventAggregator.GetEvent<ControlChangedPreviewEvent>().Subscribe(ControlChangedPreviewEvent, false);
		}
		void Refresh(bool res) {
			ReleaseUI();
		}
		void ReleaseUI(bool res) {
			ReleaseViewModels(RegionNames.reg_property);
		}

		public void FireAccountStartupEvent() {
		}
		void AboutClick(DeviceLinkEventArgs evarg) {
			var ctx = new ModalDialogContext();
			AboutView
				.Show(ctx)
				.Subscribe(
					result => {
						ctx.Dispose();
					},
					error => {
						ctx.Dispose();
						dbg.Error(error);
					}
				);
		}
		void BackgroundTasksStartup(bool smth) {
			var ctx = new ModalDialogContext();
			IUnityContainer container = (IUnityContainer)ctx;
			container.RegisterInstance<IEventAggregator>(eventAggregator);
			BackgroundTasksView
				.Show(ctx)
				.Subscribe(
					result => {
						ctx.Dispose();
					},
					error => {
						ctx.Dispose();
						dbg.Error(error);
					}
				);
		}

		void RestoreBatchStartup(BatchTaskEventArgs evargs) {
			var ctx = new ModalDialogContext();
			ctx.RegisterInstance<BatchTaskEventArgs>(evargs);
			RestoreBatchTaskView
				.Show(ctx)
				.Subscribe(
					result => {
						ctx.Dispose();
					},
					error => {
						ctx.Dispose();
						dbg.Error(error);
					}
				);
		}

		void UpgradeBatchStartup(BatchTaskEventArgs evargs) {
			var ctx = new ModalDialogContext();
			ctx.RegisterInstance<BatchTaskEventArgs>(evargs);
			UpgradeBatchTaskView
				.Show(ctx)
				.Subscribe(
					result => {
						ctx.Dispose();
					},
					error => {
						ctx.Dispose();
						dbg.Error(error);
					}
				);
		}
		void AppSettingsStartup(bool smth) {
			var ctx = new ModalDialogContext();
			AppSettingsView
				.Show(ctx)
				.Subscribe(
					result => {
						ctx.Dispose();
					},
					error => {
						ctx.Dispose();
						dbg.Error(error);
					}
				);
		}
		void ProfileChangedPreviewEvent(ProfileChangedEventArgs evargs) {
			ReleasePropertyUI();
			eventAggregator.GetEvent<ProfileChangedEvent>().Publish(evargs);
		}
		void ReleasePropertyUI() {
			ReleaseViewModels(RegionNames.reg_property);
		}
		void ReleaseUI() {
			ReleaseViewModels(RegionNames.reg_device);
			ReleaseViewModels(RegionNames.reg_property);
		}

		void HideView(string region, FrameworkElement view) {
			try {
				var regions = regionManager.Regions[region];
				regionManager.Regions[region].Remove(view);

			} catch (Exception err) {
				dbg.Error(err);
			}
		}

		void ReleaseViewModels(string region) {
			try {
				var regions = regionManager.Regions[region];
				var views = regions.Views;
				views.ForEach(x => {
					IDisposable viewmodel = (x as Control).DataContext as IDisposable;
					if (viewmodel != null)
						viewmodel.Dispose();

					var disp = (x as IDisposable);
					if (disp != null) {
						disp.Dispose();
					}

					regionManager.Regions[region].Remove(x);
				});

				Ctxdisp.Dispose();
				UIdisp.Dispose();
				subscriptions.Dispose();
				subscriptions = new CompositeDisposable();
			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		void DisplayView(object view, string viewName, string regionName) {
			try {
				IRegion region = regionManager.Regions[regionName];
				region.Add(view);
			} catch (Exception err) {
				var msg = err.Message;
				dbg.Error(err);
			}
		}
		void ShowView(object view, string viewName, string regionName) {
			try {
				IRegion region = regionManager.Regions[regionName];

				ReleaseViewModels(regionName);
				region.Add(view);

				//object existedView = region.GetView(viewName);
				//if (existedView == null) {
				//    existedView = view;
				//    region.Add(existedView, viewName);
				//} else {
				//    var exView = existedView as Control;
				//    var nView = view as Control;
				//    exView.DataContext = nView.DataContext;
				//}

				//region.Activate(existedView);

			} catch (Exception err) {
				var msg = err.Message;
				dbg.Error(err);
			}
		}

		#region NVALinks
		void ControlChangedPreviewEvent(ControlChangedEventArgs evargs) {
			ReleasePropertyUI();
			eventAggregator.GetEvent<ControlChangedEvent>().Publish(evargs);
		}
		void NVALiveVideoClick(NVALinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "NVAControlsClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.liveVideo);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			StreamInfoHelper strinfo = new StreamInfoHelper();
			strinfo.streamInfo = () => {
				return Apm.Iterate(LoadStreamInfoNVA(evarg, strinfo));
			};
			ctx.RegisterInstance<IStreamInfoHelper>(strinfo);

			Ctxdisp = ctx;

			var ssetup = new StreamSetup() {
				transport = new Transport() {
					protocol = AppDefaults.visualSettings.Transport_Type
				}
			};

			//EngineControlsView.Start(new EngineControlsView.Model(null, null), container, column);
			UIdisp = NvaLiveVideoActivity
				.Run(ctx, evarg.control.token, evarg.control.engineConfigToken, ssetup)
				.ObserveOnCurrentDispatcher()
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
		}
		void NVAControlsClick(NVALinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "NVAControlsClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.nvaControls);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			string token = null;
			if (evarg.control != null) {
				token = evarg.control.token;
			}

			UIdisp = NvaControlManagmentActivity
				.Run(ctx, evarg.engine, token)//evarg.controlToken)
				.ObserveOnCurrentDispatcher()
				.Subscribe(x => {
					if (x.IsSelect) {
						//var profTok = ((ProfileManagementActivityResult.Select)x).Item;
						//ProfileChangedEventArgs evargs = new ProfileChangedEventArgs(evarg.session, evarg.token, profTok);
						//eventAggregator.GetEvent<ProfileChangedPreviewEvent>().Publish(evargs);
						var controlTok = ((NvaControlManagmentActivityResult.Select)x).Item;
						ControlChangedEventArgs evargs = new ControlChangedEventArgs(evarg.session, evarg.engine, controlTok);
						eventAggregator.GetEvent<ControlChangedPreviewEvent>().Publish(evargs);
					} else if (x.IsRefresh) {
						//ProfileChangedEventArgs evargs = new ProfileChangedEventArgs(evarg.session, evarg.token, evarg.profileToken);
						//eventAggregator.GetEvent<ProfileChangedPreviewEvent>().Publish(evargs);
						ControlChangedEventArgs evargs = new ControlChangedEventArgs(evarg.session, evarg.engine, evarg.control.token);
						eventAggregator.GetEvent<ControlChangedPreviewEvent>().Publish(evargs);
					}
				}, err => {
					dbg.Error(err);
				});
		}
		void NVAAnalyticsClick(NVALinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "AnalyticsClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.analytics);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			//TODO: Stub fix for #225 Remove this with plugin functionality
			ILastChangedModule lmodule = new LastChangedModule();
			ctx.RegisterInstance<ILastChangedModule>(lmodule);
			//
			StreamInfoHelper strinfo = new StreamInfoHelper();
			strinfo.streamInfo = () => {
				return Apm.Iterate(LoadStreamInfoNVA(evarg, strinfo));
			};
			ctx.RegisterInstance<IStreamInfoHelper>(strinfo);

			Ctxdisp = ctx;

			//ctx.RegisterInstance<IVideoInfo>(evarg.videoInfo);
			ctx.RegisterInstance<odm.ui.activities.AnalyticsView.AnalyticType>(odm.ui.activities.AnalyticsView.AnalyticType.ALL);

			//evarg.controlToken

			string token = null;
			if (evarg.control != null)
				token = evarg.control.engineConfigToken;

			UIdisp = AnalyticsActivity
				.Run(ctx, token)//evarg.profileToken)
				.Subscribe(x => {
					//TODO: handle return from activity
				}, err => {
					dbg.Error(err);
				});

		}
		IEnumerable<FSharpAsync<Unit>> LoadStreamInfoNVA(NVALinkEventArgs args, StreamInfoHelper parent) {
			StreamInfoArgs strInfoArgs = new StreamInfoArgs(args.session);
			//strInfoArgs.encoderResolution = new Size(args.profile.VideoEncoderConfiguration.Resolution.Width, args.profile.VideoEncoderConfiguration.Resolution.Height);

			StreamSetup strsetup = new StreamSetup() {
				stream = StreamType.rtpUnicast,
				transport = new Transport() {
					protocol = AppDefaults.visualSettings.Transport_Type
				},
			};

			strInfoArgs.streamSetup = strsetup;

			yield return args.session.GetAnalyticsDeviceStreamUri(strsetup, args.control.token).Select(muri => {
				strInfoArgs.streamUri = muri;

				parent.args = strInfoArgs;
				return (Unit)null;
			});
			var inputtoken = "";
			try {
				inputtoken = args.control.inputToken.First();
			} catch (Exception err) {
				dbg.Error(err);
			}

			yield return args.session.GetAnalyticsEngineInput(inputtoken).Select(inp => {
				strInfoArgs.encoderResolution = new Size(inp.videoInput.resolution.width, inp.videoInput.resolution.height);
				strInfoArgs.sourceResolution = new Size(inp.videoInput.resolution.width, inp.videoInput.resolution.height);
				return (Unit)null;
			});
		}
		void NVAInputsClick(NVALinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "NVAInputsClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.nvaInputs);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			EngineAnalyticsView.Start(container, column);
			//UIdisp = ProfileManagementActivity
			//    .Run(ctx, evarg.profileToken, evarg.token)
			//    .ObserveOnCurrentDispatcher()
			//    .Subscribe(x => {
			//        if (x.IsSelect) {
			//            var profTok = ((ProfileManagementActivityResult.Select)x).Item;
			//            ProfileChangedEventArgs evargs = new ProfileChangedEventArgs(evarg.session, evarg.token, profTok);
			//            eventAggregator.GetEvent<ProfileChangedPreviewEvent>().Publish(evargs);
			//        } else if (x.IsRefresh) {
			//            ProfileChangedEventArgs evargs = new ProfileChangedEventArgs(evarg.session, evarg.token, evarg.profileToken);
			//            eventAggregator.GetEvent<ProfileChangedPreviewEvent>().Publish(evargs);
			//        }
			//    }, err => {
			//        dbg.Error(err);
			//    });
		}

		void NVAMetadataClick(NVALinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "MetadataClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.nvaMetadata);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			StreamInfoHelper strinfo = new StreamInfoHelper();
			strinfo.streamInfo = () => {
				return Apm.Iterate(LoadStreamInfoNVA(evarg, strinfo));
			};
			ctx.RegisterInstance<IStreamInfoHelper>(strinfo);

			Ctxdisp = ctx;

			StreamSetup ssetup = new StreamSetup() {
				transport = new Transport() {
					protocol = AppDefaults.visualSettings.Transport_Type
				}
			};

			//EngineControlsView.Start(new EngineControlsView.Model(null, null), container, column);
			UIdisp = NvaMetadataSettingsActivity
				 .Run(ctx, evarg.control.token, evarg.control.engineConfigToken, ssetup)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(x => {
				 }, err => {
					 dbg.Error(err);
				 });
		}


		void NVASettingsClick(NVALinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "NVASettingsClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.nvaSettings);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			UIdisp = NvaSimpleConfigureInputsActivity
				.Run(ctx, evarg.control)
				.ObserveOnCurrentDispatcher()
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
		}
		#endregion NVALinks

		#region ChannleLinks
		void UpgradeStartedEvent(UpgradeEventArgs evargs) {
			var view = container.Resolve<Upgrade>();
			var viewModel = view.DataContext as odm.ui.dialogs.UpgradeViewModel;
			viewModel.Init(evargs.facade, evargs.path);
			view.Owner = Application.Current.MainWindow;
			view.ShowDialog();
		}

		IDisposable Ctxdisp = Disposable.Empty;
		IDisposable UIdisp = Disposable.Empty;
		CompositeDisposable subscriptions = new CompositeDisposable();

		IUnityContainer CreateActivityContext(string regionName, INvtSession session, ContentColumn column) {
			var container = this.container.CreateChildContainer();

			//create & register view presenter
			var presenter = ViewPresenter.Create(view => {
				var region = regionManager.Regions[regionName];
				//dbg.Assert(column.Content == null);
				column.Content = view;

				return Disposable.Create(() => {
					//column.Content = null;
				});
			});
			container.RegisterInstance<IViewPresenter>(presenter);
			container.RegisterInstance<INvtSession>(session);
			container.RegisterInstance<Dispatcher>(Dispatcher.CurrentDispatcher);

			var app = Application.Current as App;
			if (app != null && app.plugins != null) {
				try {
					foreach (var p in app.plugins) {
						p.OnDeviceSettingsContextCreated(container);
					}
				} catch (Exception err) {
					dbg.Error(err);
				}
			}

			return container;
		}

		void ProfilesClick(ChannelLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "ProfilesClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.profileEditor);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			UIdisp = ProfileManagementActivity
					 .Run(ctx, evarg.profile == null ? null : evarg.profile.token, evarg.token)
				.ObserveOnCurrentDispatcher()
				.Subscribe(x => {
					if (x.IsSelect) {
						var profTok = ((ProfileManagementActivityResult.Select)x).Item;
						ProfileChangedEventArgs evargs = new ProfileChangedEventArgs(evarg.session, evarg.token, profTok);
						eventAggregator.GetEvent<ProfileChangedPreviewEvent>().Publish(evargs);
					} else if (x.IsRefresh) {
						ProfileChangedEventArgs evargs = new ProfileChangedEventArgs(evarg.session, evarg.token, evarg.profile.token);
						eventAggregator.GetEvent<ProfileChangedPreviewEvent>().Publish(evargs);
					}
				}, err => {
					dbg.Error(err);
				});
		}

		void LiveVideoClick(ChannelLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "LiveVideoClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.liveVideo);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			StreamInfoHelper strinfo = new StreamInfoHelper();
			strinfo.streamInfo = () => {
				return Apm.Iterate(LoadStreamInfoNVT(evarg, strinfo));
			};
			ctx.RegisterInstance<IStreamInfoHelper>(strinfo);

			Ctxdisp = ctx;

			ctx.RegisterInstance<IVideoInfo>(evarg.videoInfo);

			string videoSourceToken = evarg.token;
			string profToken = evarg.profile.token;

			UIdisp = LiveVideoActivity
				.Run(ctx, profToken, videoSourceToken)
				.Subscribe(x => {
					eventAggregator.GetEvent<VideoChangedEvent>().Publish(evarg);
				}, err => {
					dbg.Error(err);
				});
		}

		void PTZClick(ChannelLinkEventArgs evarg) {

			//Observable
			//    .Interval(TimeSpan.FromMilliseconds(800))
			//    .ObserveOnDispatcher()
			//    .Subscribe(l => {
			//        eventAggregator.GetEvent<VideoStreamingClick>().Publish(evarg);
			//    });

			//Observable
			//    .Interval(TimeSpan.FromMilliseconds(380))
			//    .ObserveOnDispatcher()
			//    .Subscribe(l => {
			//        eventAggregator.GetEvent<LiveVideoClick>().Publish(evarg);
			//    });

			//Observable
			//    .Interval(TimeSpan.FromMilliseconds(380))
			//    .ObserveOnDispatcher()
			//    .Subscribe(l => {
			//        var mevarg = new MetadataEventArgs();
			//        mevarg.currentAccount = evarg.currentAccount;
			//        mevarg.profile = 
			//        eventAggregator.GetEvent<MetadataClick>().Publish(evarg);
			//    });

			//Observable
			//    .Interval(TimeSpan.FromMilliseconds(50))
			//    .ObserveOnDispatcher()
			//    .Subscribe(l => {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "PTZClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.ptz);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			ctx.RegisterInstance<IVideoInfo>(evarg.videoInfo);

			PtzInfo ptzInfo = new PtzInfo() { ProfileToken = evarg.profile.token };
			ctx.RegisterInstance<IPtzInfo>(ptzInfo);

			UIdisp = PtzActivity
				.Run(ctx, evarg.profile.token)
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
			//});

		}

		void AnalyticsClick(ChannelLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "AnalyticsClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.analytics);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			//TODO: Stub fix for #225 Remove this with plugin functionality
			ILastChangedModule lmodule = new LastChangedModule();
			ctx.RegisterInstance<ILastChangedModule>(lmodule);
			//

			StreamInfoHelper strinfo = new StreamInfoHelper();
			strinfo.streamInfo = () => {
				return Apm.Iterate(LoadStreamInfoNVT(evarg, strinfo));
			};
			ctx.RegisterInstance<IStreamInfoHelper>(strinfo);

			Ctxdisp = ctx;

			ctx.RegisterInstance<IVideoInfo>(evarg.videoInfo);
			ctx.RegisterInstance<odm.ui.activities.AnalyticsView.AnalyticType>(odm.ui.activities.AnalyticsView.AnalyticType.MODULE);

			string token = null;
			if (evarg.profile.videoAnalyticsConfiguration != null)
				token = evarg.profile.videoAnalyticsConfiguration.token;
			UIdisp = AnalyticsActivity
				.Run(ctx, token)
				.Subscribe(x => {
					//TODO: handle return from activity
				}, err => {
					dbg.Error(err);
				});
		}
		IEnumerable<FSharpAsync<Unit>> LoadStreamInfoNVT(ChannelLinkEventArgs args, StreamInfoHelper parent) {
			StreamInfoArgs strInfoArgs = new StreamInfoArgs(args.session);
			strInfoArgs.encoderResolution = new Size(args.profile.videoEncoderConfiguration.resolution.width, args.profile.videoEncoderConfiguration.resolution.height);

			var strsetup = new StreamSetup() {
				stream = StreamType.rtpUnicast,
				transport = new Transport() {
					protocol = AppDefaults.visualSettings.Transport_Type
				}
			};

			strInfoArgs.streamSetup = strsetup;

			yield return args.session.GetStreamUri(strsetup, args.profile.token).Select(muri => {
				strInfoArgs.streamUri = muri.uri;

				parent.args = strInfoArgs;
				return (Unit)null;
			});

			yield return args.session.GetVideoSourceConfiguration(args.profile.videoSourceConfiguration.token).Select(conf => {
				if (conf != null && conf.bounds != null)
					strInfoArgs.sourceResolution = new Size(conf.bounds.width, conf.bounds.height);
				else {
					strInfoArgs.sourceResolution = new Size(720, 576);
				}
				return (Unit)null;
			});
		}

		void RulesClick(ChannelLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "RulesClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.ruleEngine);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			//TODO: Stub fix for #225 Remove this with plugin functionality
			ILastChangedModule lmodule = new LastChangedModule();
			ctx.RegisterInstance<ILastChangedModule>(lmodule);
			//

			StreamInfoHelper strinfo = new StreamInfoHelper();
			strinfo.streamInfo = () => {
				return Apm.Iterate(LoadStreamInfoNVT(evarg, strinfo));
			};
			ctx.RegisterInstance<IStreamInfoHelper>(strinfo);

			Ctxdisp = ctx;

			ctx.RegisterInstance<IVideoInfo>(evarg.videoInfo);
			ctx.RegisterInstance<odm.ui.activities.AnalyticsView.AnalyticType>(odm.ui.activities.AnalyticsView.AnalyticType.RULE);

			string token = null;
			if (evarg.profile.videoAnalyticsConfiguration != null)
				token = evarg.profile.videoAnalyticsConfiguration.token;
			UIdisp = AnalyticsActivity
				.Run(ctx, token)
				.Subscribe(x => {
					//TODO: handle return from activity
				}, err => {
					dbg.Error(err);
				});
		}
		void ImagingClick(ChannelLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "ImagingClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.sensorSettings);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			ctx.RegisterInstance<IVideoInfo>(evarg.videoInfo);

			UIdisp = ImagingSettingsActivity
				.Run(ctx, evarg.profile.token)
				.Subscribe(x => {
					eventAggregator.GetEvent<VideoChangedEvent>().Publish(evarg);
				}, err => {
					dbg.Error(err);
				});
		}

		void VideoStreamingClick(ChannelLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "VideoStreamingClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.videoStreaming);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			ctx.RegisterInstance<IVideoInfo>(evarg.videoInfo);

			UIdisp = VideoSettingsActivity
						.Run(ctx, evarg.profile.token)
						.Subscribe(x => {
							eventAggregator.GetEvent<VideoChangedEvent>().Publish(evarg);
						}, err => {
							dbg.Error(err);
						});
			//subscriptions.Add(evarg.session.GetScopes()
			//    .ObserveOnCurrentDispatcher()
			//    .Subscribe(scopes => {
			//        var slist = (scopes == null ? Enumerable.Empty<string>() : scopes.Select(x => x.ScopeItem)).ToList();
			//        ScopesHolder sholder = new ScopesHolder(slist.ToArray());
			//        ctx.RegisterInstance<IScopesHolder>(sholder);
			//        UIdisp = VideoSettingsActivity
			//            .Run(ctx, evarg.profileToken)
			//            .Subscribe(x => {
			//                eventAggregator.GetEvent<VideoChangedEvent>().Publish(evarg);
			//            }, err => {
			//                dbg.Error(err);
			//            });
			//    }, err => {
			//        dbg.Error(err);
			//    }));
		}
		void MetadataClick(MetadataEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "MetadataClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.metadata);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			ctx.RegisterInstance<IVideoInfo>(evarg.videoInfo);
			ctx.RegisterInstance<Profile>(evarg.profile);
			ctx.RegisterInstance<INvtSession>(evarg.session);

			UIdisp = MetadataSettingsActivity
				.Run(ctx, evarg.profile)
				.Subscribe(x => {
					//eventAggregator.GetEvent<VideoChangedEvent>().Publish(evarg);
				}, err => {
					dbg.Error(err);
				});
		}
		void UITestClick(UITestEventArgs evarg) {

			//Observable
			//    .Interval(TimeSpan.FromMilliseconds(860))
			//    .ObserveOnDispatcher()
			//    .Subscribe(l => {
			//        var args = new ChannelLinkEventArgs();
			//        args.currentAccount = evarg.currentAccount;
			//        args.profileToken = evarg.profileToken;
			//        args.session = evarg.session;
			//        args.token = evarg.token;
			//        args.videoInfo = evarg.videoInfo;
			//        eventAggregator.GetEvent<VideoStreamingClick>().Publish(args);
			//    });

			Observable
				.Interval(TimeSpan.FromMilliseconds(200))
				.ObserveOnDispatcher()
				.Subscribe(l => {
					var args = new ChannelLinkEventArgs();
					args.currentAccount = evarg.currentAccount;
					args.profile = evarg.profile;
					args.session = evarg.session;
					args.token = evarg.token;
					args.videoInfo = evarg.videoInfo;
					eventAggregator.GetEvent<LiveVideoClick>().Publish(args);
				});

			//Observable
			//    .Interval(TimeSpan.FromMilliseconds(180))
			//    .ObserveOnDispatcher()
			//    .Subscribe(l => {
			//        var args = new MetadataEventArgs();
			//        args.currentAccount = evarg.currentAccount;
			//        args.profileToken = evarg.profileToken;
			//        args.session = evarg.session;
			//        args.token = evarg.token;
			//        args.videoInfo = evarg.videoInfo;
			//        eventAggregator.GetEvent<MetadataClick>().Publish(args);
			//    });

			//Observable
			//    .Interval(TimeSpan.FromMilliseconds(50))
			//    .ObserveOnDispatcher()
			//    .Subscribe(l => {
			//        var args = new ChannelLinkEventArgs();
			//        args.currentAccount = evarg.currentAccount;
			//        args.profileToken = evarg.profileToken;
			//        args.session = evarg.session;
			//        args.token = evarg.token;
			//        args.videoInfo = evarg.videoInfo;
			//        eventAggregator.GetEvent<PTZClick>().Publish(args);
			//    });

		}
		#endregion ChannelLinks

		#region DeviceLinks
		void SequrityClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "CertificatesClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.sequrity);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			UIdisp = CertificateManagementActivity
				.Run<IUnityContainer>(ctx)
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
		}

		void UserManagerClick(UserManagementEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "UserManagerClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.usermanager);

			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			ctx.RegisterInstance<IUserManagementEventArgs>(evarg);

			Ctxdisp = ctx;

			UIdisp = UserManagementActivity
				.Run(ctx)
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
		}
		void WebPageClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			var view = container.Resolve<WebPageView>();
			view.DataContext = view;
			var baseOfDeviceUrl = new Uri(evarg.session.deviceUri.GetLeftPart(UriPartial.Authority));
			view.NavigateTo(baseOfDeviceUrl);
			ShowView(view, "deviceweb_view", RegionNames.reg_property);
		}
		void DeviceEventsClick(DeviceEventsEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			var view = container.Resolve<DeviceEventsView>();

			view.Init(evarg);
			view.BindEvents(evarg.events);

			ShowView(view, "deviceevents_view", RegionNames.reg_property);
		}
		void IdentificationClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "IdentificationClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.identificationAndStatus);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			var uri = evarg.session.deviceUri;
			ctx.RegisterInstance<Uri>(uri);

			Ctxdisp = ctx;

			UIdisp = IdentificationActivity
				.Run(ctx)
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
		}
		void ReceiversClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "ReceiversClick", RegionNames.reg_property);

			column.CreateBinding(ContentColumn.TitleProperty, LocalTitles.instance, x => x.receivers);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			var uri = evarg.session.deviceUri;
			ctx.RegisterInstance<Uri>(uri);

			Ctxdisp = ctx;

			//ReceiversView review = new ReceiversView();
			//review.Init(null);
			//ShowView(review, "receiver_view", RegionNames.reg_property);

			UIdisp = ReceiverManagementActivity
				.Run(ctx)
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
		}
		void DateTimeClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "DateTimeClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.timesettings);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			UIdisp = TimeSettingsActivity
				.Run(ctx)
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
		}
		void MaintenanceClick(MaintenanceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "MaintenanceClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.maintenance);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			ctx.RegisterInstance<IMaintenanceLinkEventArgs>(evarg);
			Ctxdisp = ctx;

			UIdisp = MaintenanceActivity
				.Run(ctx)
				.Subscribe(x => {
				}, err => {
					dbg.Error(err);
				});
		}
		void DigitalIOClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			var view = container.Resolve<DigitalIOView>();
			var viewModel = view.DataContext as DigitalIOViewModel;
			if (viewModel != null) {
				viewModel.Init(evarg.session, evarg.currentAccount);
			}
			ShowView(view, "digitalio_view", RegionNames.reg_property);
		}
		void ActionsClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "ActionsClick", RegionNames.reg_property);

			column.CreateBinding(ContentColumn.TitleProperty, LocalTitles.instance, x => x.actions);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			Ctxdisp = ctx;

			UIdisp = ActionManagementActivity
				 .Run(ctx)
				 .Subscribe(x => {
				 }, err => {
					 dbg.Error(err);
				 });
		}
		void ActionTriggersClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "ActionTriggersClick", RegionNames.reg_property);

			column.CreateBinding(ContentColumn.TitleProperty, LocalTitles.instance, x => x.triggers);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);

			Ctxdisp = ctx;

			UIdisp = ActionTriggerManagementActivity
				 .Run(ctx)
				 .Subscribe(x => {
				 }, err => {
					 dbg.Error(err);
				 });
		}
		void SystemLogClick(SysLogLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			var view = container.Resolve<SystemLogView>();
			var viewModel = view.DataContext as SystemLogViewModel;
			if (viewModel != null) {
				viewModel.Init(evarg.session, evarg.currentAccount, evarg.sysLog);
			}
			ShowView(view, "syslog_view", RegionNames.reg_property);
		}
		void NetworkClick(DeviceLinkEventArgs evarg) {
			ReleaseViewModels(RegionNames.reg_property);

			ContentColumn column = new ContentColumn();
			ShowView(column, "NetworkClick", RegionNames.reg_property);
			column.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, x => x.networkSettings);
			var ctx = CreateActivityContext(RegionNames.reg_property, evarg.session, column);
			Ctxdisp = ctx;

			UIdisp = NetworkSettingsActivity
				.Run(ctx)
				.Subscribe(x => {
					//TODO: change active profile
				}, err => {
					dbg.Error(err);
				});
		}
		//void XmlExplorerClick(DeviceLinkEventArgs evarg) {
		//    var view = container.Resolve<XMLExplorerView>();
		//    var viewModel = view.DataContext as XMLExplorerViewModel;
		//    if (viewModel != null) {
		//        viewModel.Init(evarg.session, evarg.currentAccount);
		//    }
		//    ShowView(view, "xmlexplorer_view", RegionNames.reg_property);
		//}

		#endregion DeviceLinks

		void OnSelectedDeviceChanged(DeviceSelectedEventArgs evArgs) {
			if (evArgs.devHolder == null) {
				return;
			}
			//var view = container.Resolve<DeviceView>();
			//view.Init(evArgs.devHolder, evArgs.sessionFactory);

			var view = container.Resolve<OnvifEntityPanel>();
			view.Init(evArgs.devHolder, evArgs.sessionFactory, container);

			//var viewModel = view.DataContext as DeviceViewModel;
			//if (viewModel != null) {
			//    viewModel.Init(evArgs.devHolder, evArgs.sessionFactory);
			//}
			ReleaseUI();
			ShowView(view, RegionNames.reg_entity_view, RegionNames.reg_device);
		}
	}

	public class RegionNames {
		public static string reg_property {
			get {
				return "property";
			}
		}
		public static string reg_device {
			get {
				return "device";
			}
		}
		public static string reg_entity_view {
			get {
				return "onvif_entity_view";
			}
		}
	}
}
