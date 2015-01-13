using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views;
using onvif.services;
using utils;

namespace odm.ui.activities {
	public partial class LiveVideoView : BasePropertyControl, IDisposable, IPlaybackController {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new LiveVideoView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private CompositeDisposable disposables = new CompositeDisposable();
		private Model model;
        
		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.model = model;
			InitializeComponent();
            InitAnnоtation(model.encoderResolution);
			VideoStartup(model);
		}

        private void InitAnnоtation(VideoResolution resolution)
        {
            Func<double, double> scaleX = (x) => (1 + x) * resolution.width / 2.0;
            Func<double, double> scaleY = (y) => (1 - y) * resolution.height / 2.0;

            { // objects
                objects.Width = resolution.width;
                objects.Height = resolution.height;

                movingObjectsHolder = new VAEntitiesHolder<VAObject, VAObjectSnapshot>(scaleX, scaleY, null);

                var binding = new Binding("Objects") { ElementName = root.Name };
                objects.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            }

            { // alarms
                alarms.Width = resolution.width;
                alarms.Height = resolution.height;
                
                ((INotifyCollectionChanged)alarms.Items).CollectionChanged += (s, e) =>
                {
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        var c = alarms.ItemContainerGenerator.ContainerFromItem(e.NewItems[e.NewItems.Count - 1]);
                        if (c is FrameworkElement)
                            ((FrameworkElement)c).BringIntoView();
                    }
                };

                alarmsHolder = new VAEntitiesHolder<VAAlarm, VAEntitySnapshot>(scaleX, scaleY, null);
                var binding = new Binding("Alarms") { ElementName = root.Name };
                alarms.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            }
        }

		IPlaybackSession playbackSession;

        VAEntitiesHolder<VAObject, VAObjectSnapshot> movingObjectsHolder;
        public ObservableCollection<VAObject> Objects
        {
            get { return movingObjectsHolder.Entities; }
        }
        VAEntitiesHolder<VAAlarm, VAEntitySnapshot> alarmsHolder;
        public ObservableCollection<VAAlarm> Alarms
        {
            get { return alarmsHolder.Entities; }
        }

        void VideoStartup(Model model)
        {
            var playerAct = activityContext.container.Resolve<IVideoPlayerActivity>();


            //subscribe to metadata
            IMetadataReceiver metadataReceiver = null;
            if (AppDefaults.visualSettings.EnableGraphicAnnotation)
            {
                var eventMetadataProcessor = new EventMetadataProcessor();
                eventMetadataProcessor.Processors.Add(new ObjectMotionMetadataProcessor(model.videoSourceToken, null, movingObjectsHolder.EntityInitialized, movingObjectsHolder.EntityChanged, movingObjectsHolder.EntityDeleted));
                eventMetadataProcessor.Processors.Add(new MotionAlarmMetadataProcessor(model.videoSourceToken, null, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
                eventMetadataProcessor.Processors.Add(new RegionMotionAlarmMetadataProcessor(model.videoSourceConfToken, model.videoAnalyticsConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
                eventMetadataProcessor.Processors.Add(new LoiteringAlarmMetadataProcessor(model.videoSourceConfToken, model.videoAnalyticsConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
                eventMetadataProcessor.Processors.Add(new AbandonedItemAlarmMetadataProcessor(model.videoSourceConfToken, model.videoAnalyticsConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
                eventMetadataProcessor.Processors.Add(new TripwireAlarmMetadataProcessor(model.videoSourceConfToken, model.videoAnalyticsConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
                eventMetadataProcessor.Processors.Add(new TamperingDetectorAlarmMetadataProcessor(model.videoSourceToken, null, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
                var metadataProcessor = new MetadataProcessor(eventMetadataProcessor, null);
                metadataReceiver = new MetadataFramer(metadataProcessor.Process);
            }

            var playerModel = new VideoPlayerActivityModel(
                profileToken: model.profToken,
                showStreamUrl: false,//TODO when true, annotation is not positioned correctly
					 streamSetup: new StreamSetup() {
						 stream = StreamType.rtpUnicast,
						 transport = new Transport() {
							 protocol = AppDefaults.visualSettings.Transport_Type,
							 tunnel = null
						 }
					 },
                metadataReceiver: metadataReceiver
            );

            

            disposables.Add(
                activityContext.container.RunChildActivity(player, playerModel, (c, m) => playerAct.Run(c, m))
            );

            ShowStreamURI();
        }

        void ShowStreamURI()
        {
            var getStreamInfo = activityContext.container.Resolve<odm.ui.core.IStreamInfoHelper>();
            disposables.Add(getStreamInfo.GetFunction()()
                    .ObserveOnCurrentDispatcher()
                    .Subscribe(unit =>
                    {
                        var infoArgs = getStreamInfo.GetInfoArgs();

                        if (infoArgs != null && !string.IsNullOrEmpty(infoArgs.streamUri))
                        {
                            uriString.Text = infoArgs.streamUri;
                            uriString.Visibility = System.Windows.Visibility.Visible;
                        }
                    }, err =>
                    {
                        Error(err);
                    }));
        }
        

		public void Dispose() {
			Cancel();
		}

		public void Shutdown() {
			
		}

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}
	}
}
