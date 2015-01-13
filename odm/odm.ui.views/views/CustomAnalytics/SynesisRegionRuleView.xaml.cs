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
using odm.ui.core;
using onvif.services;
using utils;
using System.ComponentModel;
using System.Xml;
using odm.ui.controls;
using odm.player;
using System.Reactive.Disposables;
using odm.ui.activities;

namespace odm.ui.views.CustomAnalytics {
	/// <summary>
	/// Interaction logic for SynesisRegionRuleView.xaml
	/// </summary>
	public partial class SynesisRegionRuleView : UserControl, IDisposable, IPlaybackController {
		public SynesisRegionRuleView() {
			InitializeComponent();
			disposables = new CompositeDisposable();
		}

		public string title = "Region rule configuration";
		IUnityContainer container;
		odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr;
		odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr;
		IVideoInfo videoInfo;
		SynesisRegionRuleModel model;
		public PropertyRuleEngineStrings Strings { get { return PropertyRuleEngineStrings.instance; } }
		public PropertyObjectTrackerStrings RoseStrings { get { return PropertyObjectTrackerStrings.instance; } }
		Size videoSourceSize;
		Size videoEncoderSize;

		public void Apply() {
			GetData();
			GetElementItems();
		}

		public class SynesisRegionRuleModel : INotifyPropertyChanged {
			public SynesisRegionRuleModel() {
			}

			public synesis.RegionPoints Points { get; set; }
			public synesis.DirectionAlarm DirectionAlarm { get; set; }
			public synesis.SpeedAlarm SpeedAlarm { get; set; }
			public synesis.LoiteringAlarm LoiteringAlarm { get; set; }
			public synesis.AbandonedItemAlarm AbandonedItemAlarm { get; set; }
			public synesis.RegionMotionAlarm RegionMotionAlarm { get; set; }

			private void NotifyPropertyChanged(String info) {
				var prop_changed = this.PropertyChanged;
				if (prop_changed != null) {
					prop_changed(this, new PropertyChangedEventArgs(info));
				}
			}
			public event PropertyChangedEventHandler PropertyChanged;
		}

		void GetElementItems() {
			modulDescr.config.parameters.elementItem.ForEach(x => {
				switch (x.name) {
					case "DirectionAlarm":
						x.any = model.DirectionAlarm.Serialize();
						break;
					case "LoiteringAlarm":
						x.any = model.LoiteringAlarm.Serialize();
						break;
					case "Points":
						ScalePointsOutput(model.Points);
						x.any = model.Points.Serialize();
						break;
					case "SpeedAlarm":
						x.any = model.SpeedAlarm.Serialize();
						break;
					case "AbandonedItemAlarm":
						x.any = model.AbandonedItemAlarm.Serialize();
						break;
					case "RegionMotionAlarm":
						x.any = model.RegionMotionAlarm.Serialize();
						break;
				}
			});
		}

		void FillElementItems(ItemList.ElementItem[] elementItems, SynesisRegionRuleModel model) {
			elementItems.ForEach(x => {
				switch (x.name) {
					case "DirectionAlarm":
						model.DirectionAlarm = x.any.Deserialize<synesis.DirectionAlarm>();
						break;
					case "LoiteringAlarm":
						model.LoiteringAlarm = x.any.Deserialize<synesis.LoiteringAlarm>();
						break;
					case "Points":
						model.Points = x.any.Deserialize<synesis.RegionPoints>();
						ScalePointsInput(model.Points);
						break;
					case "SpeedAlarm":
						model.SpeedAlarm = x.any.Deserialize<synesis.SpeedAlarm>();
						break;
					case "AbandonedItemAlarm":
						model.AbandonedItemAlarm = x.any.Deserialize<synesis.AbandonedItemAlarm>();
						break;
					case "RegionMotionAlarm":
						model.RegionMotionAlarm = x.any.Deserialize<synesis.RegionMotionAlarm>();
						break;
				}
			});
		}

		void ScalePointsInput(synesis.RegionPoints points) {
			points.Points.ForEach(x => {
				DataConverter.ScalePointInput(x, videoSourceSize, videoEncoderSize);
			});
		}
		void ScalePointsOutput(synesis.RegionPoints points) {
			points.Points.ForEach(x => {
				DataConverter.ScalePointOutput(x, videoSourceSize, videoEncoderSize);
			});
		}
		void InitRegionEditor() {
			List<Point> plist = new List<Point>();
			model.Points.Points.ForEach(x => {
				plist.Add(new Point(x.X, x.Y));
			});

			regeditor.Init(plist, videoInfo.Resolution, 20);
		}
		void GetData() {
			try {
				GetRose();

				model.SpeedAlarm.Time = (int)valueRunningTime.Value;
				model.RegionMotionAlarm.Enabled = enableMoving.IsChecked.Value;
				model.LoiteringAlarm.Time = (int)valueLoiteringTime.Value;
				model.AbandonedItemAlarm.Enabled = enableAbandoning.IsChecked.Value;
				model.SpeedAlarm.Enabled = enableRunning.IsChecked.Value;
				model.LoiteringAlarm.Enabled = enableLoitering.IsChecked.Value;
				model.DirectionAlarm.Enabled = enableDirectionMoving.IsChecked.Value;

				model.DirectionAlarm.MinShift = (float)valueDirectionMoving.Value;
				model.LoiteringAlarm.Radius = (float)valueLoiteringRadius.Value;

				//valueLoiteringTime.Value = model.LoiteringRule.Time;

				model.SpeedAlarm.Speed = (float)valueRunningSpeed.Value * 1000f / 3600f; // to m/s

				var plist = regeditor.GetRegion();
				List<synesis.Point> synesisPoints = new List<synesis.Point>();
				plist.ForEach(point => {
					var spoint = new synesis.Point();
					spoint.X = (int)point.X;
					spoint.Y = (int)point.Y;
					synesisPoints.Add(spoint);
				});
				model.Points.Points = synesisPoints.ToArray();
				GetRose();

			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		void GetRose() {
			model.DirectionAlarm.Rose.Down = directionRose.btnDown;
			model.DirectionAlarm.Rose.DownLeft = directionRose.btnDownLeft;
			model.DirectionAlarm.Rose.DownRight = directionRose.btnDownRight;
			model.DirectionAlarm.Rose.Left = directionRose.btnLeft;
			model.DirectionAlarm.Rose.Right = directionRose.btnRight;
			model.DirectionAlarm.Rose.Up = directionRose.btnUp;
			model.DirectionAlarm.Rose.UpLeft = directionRose.btnUpLeft;
			model.DirectionAlarm.Rose.UpRight = directionRose.btnUpRight;
		}
		void BindData() {
			InitRegionEditor();

			try {
				enableRunning.IsChecked = model.SpeedAlarm.Enabled;
				enableLoitering.IsChecked = model.LoiteringAlarm.Enabled;
				enableDirectionMoving.IsChecked = model.DirectionAlarm.Enabled;
				enableAbandoning.IsChecked = model.AbandonedItemAlarm.Enabled;
				enableMoving.IsChecked = model.RegionMotionAlarm.Enabled;

				valueDirectionMoving.Increment = 0.1;
				valueDirectionMoving.FormatString = "F2";
				valueDirectionMoving.Value = model.DirectionAlarm.MinShift;

				valueLoiteringRadius.Increment = 0.1;
				valueLoiteringRadius.FormatString = "F2";
				valueLoiteringRadius.Minimum = 0;
				valueLoiteringRadius.Value = model.LoiteringAlarm.Radius;

				valueLoiteringTime.Minimum = 0;
				valueLoiteringTime.Value = model.LoiteringAlarm.Time;

				valueRunningSpeed.Increment = 0.1;
				valueRunningSpeed.FormatString = "F2";
				valueRunningSpeed.Minimum = 0;
				valueRunningSpeed.Value = model.SpeedAlarm.Speed * 3600f / 1000f; // to km/h

				valueRunningTime.Value = model.SpeedAlarm.Time;

				directionRose.btnDown = model.DirectionAlarm.Rose.Down;
				directionRose.btnDownLeft = model.DirectionAlarm.Rose.DownLeft;
				directionRose.btnDownRight = model.DirectionAlarm.Rose.DownRight;
				directionRose.btnLeft = model.DirectionAlarm.Rose.Left;
				directionRose.btnRight = model.DirectionAlarm.Rose.Right;
				directionRose.btnUp = model.DirectionAlarm.Rose.Up;
				directionRose.btnUpLeft = model.DirectionAlarm.Rose.UpLeft;
				directionRose.btnUpRight = model.DirectionAlarm.Rose.UpRight;

			} catch (Exception err) {
				dbg.Error(err);
			}
		}

		public bool Init(IUnityContainer container, StreamInfoArgs args, odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr) {
			this.modulDescr = modulDescr;
			this.container = container;

			this.videoDescr = new ConfigureAnalyticView.AnalyticsVideoDescriptor() {
				videoInfo = new VideoInfo() { MediaUri = args.streamUri, Resolution = new Size() { Width = args.sourceResolution.Width, Height = args.sourceResolution.Height } },
				videoSourceResolution = new Size() { Width = args.encoderResolution.Width, Height = args.encoderResolution.Height }
			};

			this.videoInfo = videoDescr.videoInfo;

			videoSourceSize = new Size(videoDescr.videoSourceResolution.Width, videoDescr.videoSourceResolution.Height);
			videoEncoderSize = new Size(videoDescr.videoInfo.Resolution.Width, videoDescr.videoInfo.Resolution.Height);

			model = new SynesisRegionRuleModel();

			try {
				FillElementItems(modulDescr.config.parameters.elementItem, model);
			} catch (Exception err) {
				dbg.Error(err);
				return false;
			}
			try {
				BindData();
				VideoStartup(args);
			} catch (Exception err) {
				dbg.Error(err);
				return false;
			}
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
