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
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using System.Reactive.Disposables;
using odm.core;
using odm.infra;
using odm.ui.controls;
using odm.ui.views;
using odm.ui.core;
using odm.player;
using onvif.services;
using utils;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for AnalyticsLiveVideoView.xaml
	/// </summary>
	public partial class AnalyticsLiveVideoView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new AnalyticsLiveVideoView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		CompositeDisposable disposables = new CompositeDisposable();
		VideoBuffer vidBuff;

		public void Init(Model model) {
			InitializeComponent();

			OnCompleted += () => {
				disposables.Dispose();
			};

			VideoResolution resolution = null;

			//TODO rid of these checks
			if (model.size == null) {
				resolution = new VideoResolution() { height = 576, width = 720 };
			} if (model.size.width < 1 || model.size.height < 1) {
				resolution = new VideoResolution() { height = 576, width = 720 };
			} else {
				resolution = model.size;
			}
			InitAnnоtation(resolution);
			VideoStartup(model, resolution);
		}

		VAEntitiesHolder<VAObject, VAObjectSnapshot> movingObjectsHolder;
		public ObservableCollection<VAObject> Objects {
			get { return movingObjectsHolder.Entities; }
		}
		VAEntitiesHolder<VAAlarm, VAEntitySnapshot> alarmsHolder;
		public ObservableCollection<VAAlarm> Alarms {
			get { return alarmsHolder.Entities; }
		}

		private void InitAnnоtation(VideoResolution resolution) {
			// transform from onvif 
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
				/*((INotifyCollectionChanged)alarms.Items).CollectionChanged += (s, e) => 
					 {
						  if (e.NewItems != null && e.NewItems.Count > 0)
						  {
								Dispatcher.BeginInvoke(
									 new Action(delegate { ((ListBox)alarms).ScrollIntoView(e.NewItems[e.NewItems.Count - 1]); }) 
									 , DispatcherPriority.SystemIdle); 
						  }
					 };*/
				((INotifyCollectionChanged)alarms.Items).CollectionChanged += (s, e) => {
					if (e.NewItems != null && e.NewItems.Count > 0) {
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

		void VideoStartup(Model model, VideoResolution resolution) {

			//subscribe to metadata
			IMetadataReceiver metadataReceiver = null;
			if (AppDefaults.visualSettings.EnableGraphicAnnotation) {
				string vaConfToken = model.engineConfToken;
				var eventMetadataProcessor = new EventMetadataProcessor();
				//eventMetadataProcessor.Processors.Add(new ObjectMotionMetadataProcessor(null, vaConfToken, movingObjectsHolder.EntityInitialized, movingObjectsHolder.EntityChanged, movingObjectsHolder.EntityDeleted));
				eventMetadataProcessor.Processors.Add(new MotionAlarmMetadataProcessor(null, vaConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
				eventMetadataProcessor.Processors.Add(new RegionMotionAlarmMetadataProcessor(null, vaConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
				eventMetadataProcessor.Processors.Add(new LoiteringAlarmMetadataProcessor(null, vaConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
				eventMetadataProcessor.Processors.Add(new AbandonedItemAlarmMetadataProcessor(null, vaConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
				eventMetadataProcessor.Processors.Add(new TripwireAlarmMetadataProcessor(null, vaConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
				eventMetadataProcessor.Processors.Add(new TamperingDetectorAlarmMetadataProcessor(null, vaConfToken, alarmsHolder.EntityInitialized, alarmsHolder.EntityChanged, alarmsHolder.EntityDeleted));
				var sceneMetadataProcessor = new SceneMetadataProcessor(movingObjectsHolder.EntityInitialized, movingObjectsHolder.EntityChanged, movingObjectsHolder.EntityDeleted);
				var metadataProcessor = new MetadataProcessor(eventMetadataProcessor, sceneMetadataProcessor);
				metadataReceiver = new MetadataFramer(metadataProcessor.Process);
			}

			vidBuff = new VideoBuffer(resolution.width, resolution.height);

			var streamSetup = new StreamSetup() {
				transport = new Transport() {
					protocol = AppDefaults.visualSettings.Transport_Type
				}
			};

			VideoPlayerView playview = new VideoPlayerView();
			disposables.Add(playview);

			player.Child = playview;

			playview.Init(new VideoPlayerView.Model(
				streamSetup: streamSetup,
				mediaUri: new MediaUri() { 
					uri = model.uri 
				},
				encoderResolution: new VideoResolution() {
					height = resolution.height,
					width = resolution.width
				},
				isUriEnabled: false, //TODO if true then annotation is not positioned correctly
				metadataReceiver: metadataReceiver
			));

			uriString.Visibility = System.Windows.Visibility.Visible;
			uriString.Text = model.uri;
		}

		public void Dispose() {
		}
	}
}
