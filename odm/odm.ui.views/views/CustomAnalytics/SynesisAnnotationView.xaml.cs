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
using Microsoft.Practices.Unity;
using utils;
using System.ComponentModel;
using onvif.services;
using odm.ui.core;
using odm.ui.controls;
using odm.player;
using System.Reactive.Disposables;
using odm.ui.activities;

namespace odm.ui.views.CustomAnalytics {
    /// <summary>
    /// Interaction logic for SynesisAnnotationView.xaml
    /// </summary>
    public partial class SynesisAnnotationView : UserControl, IDisposable, IPlaybackController {
        public SynesisAnnotationView() {
            InitializeComponent();
			disposables = new CompositeDisposable();
        }
        public void Apply() {
            try {
                GetData(modulDescr.config.parameters.simpleItem, model);
            } catch (Exception err) {
                dbg.Error(err);
            }
        }

        public LocalDisplayAnnotation Strings { get { return LocalDisplayAnnotation.instance; } }

        public class SynesisAnalyticsModel : INotifyPropertyChanged {
            bool enableMovingRects;
            public bool EnableMovingRects {
                get {
                    return enableMovingRects;
                }
                set {
                    enableMovingRects = value;
                    NotifyPropertyChanged("EnableMovingRects");
                }
            }
            bool enableSpeed;
            public bool EnableSpeed {
                get {
                    return enableSpeed;
                }
                set {
                    enableSpeed = value;
                    NotifyPropertyChanged("EnableSpeed");
                }
            }
            bool enableTimestamp;
            public bool EnableTimestamp {
                get {
                    return enableTimestamp;
                }
                set {
                    enableTimestamp = value;
                    NotifyPropertyChanged("EnableTimestamp");
                }
            }
            bool enableSystemInformation;
            public bool EnableSystemInformation {
                get {
                    return enableSystemInformation;
                }
                set {
                    enableSystemInformation = value;
                    NotifyPropertyChanged("EnableSystemInformation");
                }
            }
            bool enableChannelName;
            public bool EnableChannelName {
                get {
                    return enableChannelName;
                }
                set {
                    enableChannelName = value;
                    NotifyPropertyChanged("EnableChannelName");
                }
            }
            bool enableTracking;
            public bool EnableTracking {
                get {
                    return enableTracking;
                }
                set {
                    enableTracking = value;
                    NotifyPropertyChanged("EnableTracking");
                }
            }
            bool enableUserRegion;
            public bool EnableUserRegion {
                get {
                    return enableUserRegion;
                }
                set {
                    enableUserRegion = value;
                    NotifyPropertyChanged("EnableUserRegion");
                }
            }
            bool enableRules;
            public bool EnableRules {
                get {
                    return enableRules;
                }
                set {
                    enableRules = value;
                    NotifyPropertyChanged("EnableRules");
                }
            }
            bool enableCalibrationResults;
            public bool EnableCalibrationResults {
                get {
                    return enableCalibrationResults;
                }
                set {
                    enableCalibrationResults = value;
                    NotifyPropertyChanged("EnableCalibrationResults");
                }
            }
			private void NotifyPropertyChanged(String info) {
				var prop_changed = this.PropertyChanged;
				if (prop_changed != null) {
					prop_changed(this, new PropertyChangedEventArgs(info));
				}
			}
            public event PropertyChangedEventHandler PropertyChanged;
        }
        void GetData(ItemList.SimpleItem[] simpleItems, SynesisAnalyticsModel model) {
            simpleItems.ForEach(simple => {
                switch (simple.name) {
                    case "EnableMovingRects":
                        simple.value = DataConverter.BoolToString(model.EnableMovingRects);
                        break;
                    case "EnableSpeed":
                        simple.value = DataConverter.BoolToString(model.EnableSpeed);
                        break;
                    case "EnableTimestamp":
                        simple.value = DataConverter.BoolToString(model.EnableTimestamp);
                        break;
                    case "EnableSystemInformation":
                        simple.value = DataConverter.BoolToString(model.EnableSystemInformation);
                        break;
                    case "EnableChannelName":
                        simple.value = DataConverter.BoolToString(model.EnableChannelName);
                        break;
                    case "EnableTracking":
                        simple.value = DataConverter.BoolToString(model.EnableTracking);
                        break;
                    case "EnableUserRegion":
                        simple.value = DataConverter.BoolToString(model.EnableUserRegion);
                        break;
                    case "EnableRules":
                        simple.value = DataConverter.BoolToString(model.EnableRules); 
                        break;
                    case "EnableCalibrationResults":
                        simple.value = DataConverter.BoolToString(model.EnableCalibrationResults);
                        break;
                }
            });
        }
        void FillData(ItemList.SimpleItem[] simpleItems, SynesisAnalyticsModel model) {
            simpleItems.ForEach(simple => {
                switch (simple.name) {
                    case "EnableMovingRects":
                        model.EnableMovingRects = DataConverter.StringToBool(simple.value);
                        break;
                    case "EnableSpeed":
                        model.EnableSpeed = DataConverter.StringToBool(simple.value);
                        break;
                    case "EnableTimestamp":
                        model.EnableTimestamp = DataConverter.StringToBool(simple.value);
                        break;
                    case "EnableSystemInformation":
                        model.EnableSystemInformation = DataConverter.StringToBool(simple.value);
                        break;
                    case "EnableChannelName":
                        model.EnableChannelName = DataConverter.StringToBool(simple.value);
                        break;
                    case "EnableTracking":
                        model.EnableTracking = DataConverter.StringToBool(simple.value);
                        break;
                    case "EnableUserRegion":
                        model.EnableUserRegion = DataConverter.StringToBool(simple.value);
                        break;
                    case "EnableRules":
                        model.EnableRules = DataConverter.StringToBool(simple.value); 
                        break;
                    case "EnableCalibrationResults":
                        model.EnableCalibrationResults = DataConverter.StringToBool(simple.value);
                        break;
                }
            });
        }
        void BindData() {
            EnableCalibrationResults.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableCalibrationResults, (m, v) => { m.EnableCalibrationResults = v; });
            EnableChannelName.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableChannelName, (m, v) => { m.EnableChannelName = v; });
            EnableMovingRects.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableMovingRects, (m, v) => { m.EnableMovingRects = v; });
            EnableRules.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableRules, (m, v) => { m.EnableRules = v; });
            EnableSpeed.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableSpeed, (m, v) => { m.EnableSpeed = v; });
            EnableSystemInformation.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableSystemInformation, (m, v) => { m.EnableSystemInformation = v; });
            EnableTimestamp.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableTimestamp, (m, v) => { m.EnableTimestamp = v; });
            EnableTracking.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableTracking, (m, v) => { m.EnableTracking = v; });
            EnableUserRegion.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableUserRegion, (m, v) => { m.EnableUserRegion = v; });
        }

        public string title = "Annotation module configuration";
        SynesisAnalyticsModel model;
        odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr;
        odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr;
        IUnityContainer container;
        IVideoInfo videoInfo;
        
        public bool Init(IUnityContainer container, StreamInfoArgs args, odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr) {
			this.modulDescr = modulDescr;
			this.container = container;

			this.videoDescr = new ConfigureAnalyticView.AnalyticsVideoDescriptor() {
				videoInfo = new VideoInfo() { MediaUri = args.streamUri, Resolution = new Size() {  Width = args.sourceResolution.Width, Height = args.sourceResolution.Height } },
				videoSourceResolution = new Size() { Width = args.encoderResolution.Width, Height = args.encoderResolution.Height } 
			};


            model = new SynesisAnalyticsModel();
            
            FillData(modulDescr.config.parameters.simpleItem, model);
            BindData();
			VideoStartup(args);
            return true;
        }

		IPlaybackSession playbackSession;
		VideoBuffer vidBuff;
		CompositeDisposable disposables;
		void VideoStartup(StreamInfoArgs args) {//, string profToken) {
			vidBuff = new VideoBuffer((int)args.sourceResolution.Width, (int)args.sourceResolution.Height);

			//var playerAct = container.Resolve<IVideoPlayerActivity>();
			////profileToken: profToken,
			//var model = new VideoPlayerActivityModel(
			//    showStreamUrl: false,
			//    streamSetup: new StreamSetup() {
			//        Stream = StreamType.RTPUnicast,
			//        Transport = new Transport() {
			//            Protocol = AppDefaults.visualSettings.Transport_Type,
			//            Tunnel = null
			//        }
			//    }
			//);

			//disposables.Add(
			//    container.RunChildActivity(player, model, (c, m) => playerAct.Run(c, m))
			//);
			VideoPlayerView playview = new VideoPlayerView();
			disposables.Add(playview);
			
			player.Child = playview;

			playview.Init(new VideoPlayerView.Model(
				streamSetup: args.streamSetup,
				mediaUri: new MediaUri() {
					uri = args.streamUri
				},
				encoderResolution: new VideoResolution() {
					height = (int)args.sourceResolution.Height,
					width = (int)args.sourceResolution.Width
				},
				isUriEnabled: false,
				metadataReceiver: null
			));
		}

		public void Dispose() {
			if (vidBuff != null) {
				vidBuff.Dispose();
			}
			disposables.Dispose();
			//TODO: release player host
		}

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Shutdown() {
		}
	}
}
