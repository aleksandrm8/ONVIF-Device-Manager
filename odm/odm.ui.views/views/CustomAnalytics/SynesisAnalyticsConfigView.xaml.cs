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
using utils;
using onvif.services;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Xml;
using odm.ui.controls;
using odm.player;
using System.Reactive.Disposables;
using odm.ui.core;
using odm.ui.activities;
using System.IO;
using System.Xml.Schema;

namespace odm.ui.views.CustomAnalytics {
	/// <summary>
	/// Interaction logic for SynesisAnalyticsConfigView.xaml
	/// </summary>
	public partial class SynesisAnalyticsConfigView : UserControl, IDisposable, IPlaybackController {
		public SynesisAnalyticsConfigView() {
			InitializeComponent();

			disposables = new CompositeDisposable();
			player = new Border();
			player.Margin = new Thickness(0);
			player.Background = Brushes.Black;

			//tabTampering.CreateBinding(TamperingDetectorsView.isCameraObstructedEnabledProperty, tabObjectTracker, x=>
		}

		Border player;

		public string title = "Analytics module configuration";
		odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr;
		odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr;
		SynesisAnalyticsModel model;
		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }

		Size videoSourceSize;
		Size videoEncoderSize;

		public class SynesisAnalyticsModel : INotifyPropertyChanged {
			public SynesisAnalyticsModel(XmlSchemaSet schema) {

				{
					int min, max;
					var typeName = new XmlQualifiedName("DisplacementSensitivityValue", "http://www.synesis.ru/onvif/VideoAnalytics");
					if (schema.TryGetIntLimitsOfGlobalType(typeName, out min, out max)) {
						this.DisplacementSensitivityValueMin = min;
						this.DisplacementSensitivityValueMax = max;
					}
				}

				{
					int min, max;
					var typeName = new XmlQualifiedName("ContrastSensitivityValue", "http://www.synesis.ru/onvif/VideoAnalytics");
					if (schema.TryGetIntLimitsOfGlobalType(typeName, out min, out max)) {
						this.ContrastSensitivityValueMin = min;
						this.ContrastSensitivityValueMax = max;
					}
				}
			}

			bool useObjectTracker = false;
			public bool UseObjectTracker {
				get {
					return useObjectTracker;
				}
				set {
					useObjectTracker = value;
					NotifyPropertyChanged("UseObjectTracker");
				}
			}
			public const float ObjectAreaValueMin = 0.0f;
			public const float ObjectAreaValueMax = 100.0f;
			float maxObjectArea;
			public float MaxObjectArea {
				get { return maxObjectArea; }
				set {
					maxObjectArea = value;
					NotifyPropertyChanged("MaxObjectArea");
				}
			}
			float minObjectArea;
			public float MinObjectArea {
				get { return minObjectArea; }
				set {
					minObjectArea = value;
					NotifyPropertyChanged("MinObjectArea");
				}
			}

			public const float ObjectSpeedValueMin = 0.0f;
			public const float ObjectSpeedValueMax = 100.0f;
			float maxObjectSpeed;
			public float MaxObjectSpeed {
				get { return maxObjectSpeed; }
				set {
					maxObjectSpeed = value;
					NotifyPropertyChanged("MaxObjectSpeed");
				}
			}
			public int DisplacementSensitivityValueMin { get; private set; }
			public int DisplacementSensitivityValueMax { get; private set; }
			int displacementSensivity;
			public int DisplacementSensivity {
				get { return displacementSensivity; }
				set {
					displacementSensivity = value;
					NotifyPropertyChanged("DisplacementSensivity");
				}
			}
			public const int StabilizationTimeValueMin = 40;
			public const int StabilizationTimeValueMax = 10000;
			int stabilizationTime;
			public int StabilizationTime {
				get { return stabilizationTime; }
				set {
					stabilizationTime = value;
					NotifyPropertyChanged("StabilizationTime");
				}
			}
			bool useAntishaker;
			public bool UseAntishaker {
				get { return useAntishaker; }
				set {
					useAntishaker = value;
					NotifyPropertyChanged("UseAntishaker");
				}
			}
			bool cameraRedirected;
			public bool CameraRedirected {
				get { return cameraRedirected; }
				set {
					cameraRedirected = value;
					NotifyPropertyChanged("CameraRedirected");
				}
			}
			bool shiftOutputPicture;
			public bool ShiftOutputPicture {
				get { return shiftOutputPicture; }
				set {
					shiftOutputPicture = value;
					NotifyPropertyChanged("ShiftOutputPicture");
				}
			}
			bool cameraObstructed;
			public bool CameraObstructed {
				get { return cameraObstructed; }
				set {
					cameraObstructed = value;
					NotifyPropertyChanged("CameraObstructed");
				}
			}
			public int ContrastSensitivityValueMin { get; private set; }
			public int ContrastSensitivityValueMax { get; private set; }
			int contrastSensivity;
			public int ContrastSensivity {
				get { return contrastSensivity; }
				set {
					contrastSensivity = value;
					NotifyPropertyChanged("ContrastSensivity");
				}
			}
			bool imageTooDark;
			public bool ImageTooDark {
				get { return imageTooDark; }
				set {
					imageTooDark = value;
					NotifyPropertyChanged("ImageTooDark");
				}
			}
			bool imageTooBright;
			public bool ImageTooBright {
				get { return imageTooBright; }
				set {
					imageTooBright = value;
					NotifyPropertyChanged("ImageTooBright");
				}
			}
			bool imageTooNoisy;
			public bool ImageTooNoisy {
				get { return imageTooNoisy; }
				set {
					imageTooNoisy = value;
					NotifyPropertyChanged("ImageTooNoisy");
				}
			}
			bool imageTooBlurry;
			public bool ImageTooBlurry {
				get { return imageTooBlurry; }
				set {
					imageTooBlurry = value;
					NotifyPropertyChanged("ImageTooBlurry");
				}
			}

			public synesis.AntishakerCrop AntishakerCrop { get; set; }
			public synesis.MarkerCalibration Markers { get; set; }
			public synesis.UserRegion UserRegion { get; set; }

			private void NotifyPropertyChanged(String info) {
				var prop_changed = this.PropertyChanged;
				if (prop_changed != null) {
					prop_changed(this, new PropertyChangedEventArgs(info));
				}
			}
			public event PropertyChangedEventHandler PropertyChanged;
		}

		void GetSimpleItems() {
			modulDescr.config.parameters.simpleItem.ForEach(x => {
				switch (x.name) {
					case "StabilizationTime":
						x.value = XmlConvert.ToString(model.StabilizationTime);
						break;
					case "ShiftOutputPicture":
						x.value = XmlConvert.ToString(model.ShiftOutputPicture);
						break;
					case "UseObjectTracker":
						x.value = XmlConvert.ToString(model.UseObjectTracker);
						break;
					case "MaxObjectArea":
						x.value = XmlConvert.ToString(model.MaxObjectArea);
						break;
					case "MinObjectArea":
						x.value = XmlConvert.ToString(model.MinObjectArea);
						break;
					case "MaxObjectSpeed":
						x.value = XmlConvert.ToString(model.MaxObjectSpeed);
						break;
					case "DisplacementSensitivity":
						x.value = XmlConvert.ToString(model.DisplacementSensivity);
						break;
					case "CameraObstructed":
						x.value = XmlConvert.ToString(model.CameraObstructed);
						break;
					case "UseAntishaker":
						x.value = XmlConvert.ToString(model.UseAntishaker);
						break;
					case "CameraRedirected":
						x.value = XmlConvert.ToString(model.CameraRedirected);
						break;
					case "ContrastSensitivity":
						x.value = XmlConvert.ToString(model.ContrastSensivity);
						break;
					case "ImageTooDark":
						x.value = XmlConvert.ToString(model.ImageTooDark);
						break;
					case "ImageTooBlurry":
						x.value = XmlConvert.ToString(model.ImageTooBlurry);
						break;
					case "ImageTooBright":
						x.value = XmlConvert.ToString(model.ImageTooBright);
						break;
					case "ImageTooNoisy":
						x.value = XmlConvert.ToString(model.ImageTooNoisy);
						break;
				}
			});
		}
		void GetElementItems() {
			modulDescr.config.parameters.elementItem.ForEach(x => {
				switch (x.name) {
					case "AntishakerCrop":
						ScaleAntishakerCropOutput(model.AntishakerCrop);
						x.any = model.AntishakerCrop.Serialize();
						break;
					case "MarkerCalibration":
						ScaleMarkersOutput(model.Markers);
						x.any = model.Markers.Serialize();
						break;
					case "UserRegion":
						ScaleUserRegionOutput(model.UserRegion);
						x.any = model.UserRegion.Serialize();
						break;
				}
			});
		}
		void FillSimpleItems(ItemList.SimpleItem[] simpleItems, SynesisAnalyticsModel model) {
			simpleItems.ForEach(x => {
				switch (x.name) {
					case "StabilizationTime":
						model.StabilizationTime = DataConverter.StringToInt(x.value);
						break;
					case "ShiftOutputPicture":
						model.ShiftOutputPicture = DataConverter.StringToBool(x.value);
						break;
					case "UseObjectTracker":
						model.UseObjectTracker = DataConverter.StringToBool(x.value);
						break;
					case "MaxObjectArea":
						model.MaxObjectArea = DataConverter.StringToFloat(x.value);
						break;
					case "MinObjectArea":
						model.MinObjectArea = DataConverter.StringToFloat(x.value);
						break;
					case "MaxObjectSpeed":
						model.MaxObjectSpeed = DataConverter.StringToFloat(x.value);
						break;
					case "DisplacementSensitivity":
						model.DisplacementSensivity = DataConverter.StringToInt(x.value);
						break;
					case "CameraObstructed":
						model.CameraObstructed = DataConverter.StringToBool(x.value);
						break;
					case "UseAntishaker":
						model.UseAntishaker = DataConverter.StringToBool(x.value);
						break;
					case "CameraRedirected":
						model.CameraRedirected = DataConverter.StringToBool(x.value);
						break;
					case "ContrastSensitivity":
						model.ContrastSensivity = DataConverter.StringToInt(x.value);
						break;
					case "ImageTooDark":
						model.ImageTooDark = DataConverter.StringToBool(x.value);
						break;
					case "ImageTooBlurry":
						model.ImageTooBlurry = DataConverter.StringToBool(x.value);
						break;
					case "ImageTooBright":
						model.ImageTooBright = DataConverter.StringToBool(x.value);
						break;
					case "ImageTooNoisy":
						model.ImageTooNoisy = DataConverter.StringToBool(x.value);
						break;
				}
			});
		}

		void FillElementItems(ItemList.ElementItem[] elementItems, SynesisAnalyticsModel model) {
			elementItems.ForEach(x => {
				switch (x.name) {
					case "AntishakerCrop":
						model.AntishakerCrop = x.any.Deserialize<synesis.AntishakerCrop>();
						ScaleAntishakerCropInput(model.AntishakerCrop);
						break;
					case "MarkerCalibration":
						model.Markers = x.any.Deserialize<synesis.MarkerCalibration>();
						ScaleMarkersInput(model.Markers);
						break;
					case "UserRegion":
						model.UserRegion = x.any.Deserialize<synesis.UserRegion>();
						ScaleUserRegionInput(model.UserRegion);
						break;
				}
			});
		}

		void ScaleAntishakerCropOutput(synesis.AntishakerCrop crop) {
			double valueX = crop.XOffs;
			double valueY = crop.YOffs;
			double width = crop.CropWidth;
			double height = crop.CropHeight;
			//convert from video sourve resolution to encoder resolution
			double scalex = videoDescr.videoSourceResolution.Width / videoDescr.videoInfo.Resolution.Width;
			double scaley = videoDescr.videoSourceResolution.Height / videoDescr.videoInfo.Resolution.Height;
			valueX = valueX * scalex;
			valueY = valueY * scaley;
			height = height * scaley;
			width = width * scalex;
			//scale from visible to [-1;1]
			double heightValue = videoDescr.videoSourceResolution.Height - 1;
			double widthValue = videoDescr.videoSourceResolution.Width - 1;
			crop.XOffs = (float)(((valueX * 2) / widthValue) - 1);
			crop.YOffs = (float)((((heightValue - valueY) * 2) / heightValue) - 1);
			crop.CropWidth = (float)((width * 2) / widthValue);
			crop.CropHeight = (float)((height * 2) / heightValue);
		}
		void ScaleAntishakerCropInput(synesis.AntishakerCrop crop) {
			//scale from [-1;1] to visible dimensions
			double valueX = (videoDescr.videoSourceResolution.Width / 2) * (crop.XOffs + 1);
			double valueY = videoDescr.videoSourceResolution.Height - (videoDescr.videoSourceResolution.Height / 2) * (crop.YOffs + 1);
			double width = (videoDescr.videoSourceResolution.Width / 2) * (crop.CropWidth);
			double height = (videoDescr.videoSourceResolution.Height / 2) * (crop.CropHeight);
			//convert ftrom video sourve resolution to encoder resolution
			double scalex = videoDescr.videoInfo.Resolution.Width / (videoDescr.videoSourceResolution.Width == 0 ? 1 : videoDescr.videoSourceResolution.Width);
			double scaley = videoDescr.videoInfo.Resolution.Height / (videoDescr.videoSourceResolution.Height == 0 ? 1 : videoDescr.videoSourceResolution.Height);
			valueX = valueX * scalex;
			valueY = valueY * scaley;
			height = height * scaley;
			width = width * scalex;
			crop.XOffs = (float)valueX;
			crop.YOffs = (float)valueY;
			crop.CropHeight = (float)height;
			crop.CropWidth = (float)width;
		}
		void ScaleMarkersOutput(synesis.MarkerCalibration markers) {
			var cmarkerCalibration = markers.Item as synesis.CombinedMarkerCalibration;
			if (cmarkerCalibration != null) {
				//2d
				cmarkerCalibration.CombinedMarkers.ForEach(cm => {
					cm.Rectangles.ForEach(rect => {
						ScaleRectOutput(rect);
					});
				});
			} else {
				//1d
				var hmarkerCalibration = markers.Item as synesis.HeightMarkerCalibration;
				hmarkerCalibration.HeightMarkers.ForEach(hm => {
					hm.SurfaceNormals.ForEach(sn => {
						DataConverter.ScalePointOutput(sn.Point, videoSourceSize, videoEncoderSize);
						sn.Height = ScaleHeigthOutput(sn.Height);
					});
				});
			}
		}
		void ScaleMarkersInput(synesis.MarkerCalibration markers) {
			var cmarkerCalibration = markers.Item as synesis.CombinedMarkerCalibration;
			if (cmarkerCalibration != null) {
				//2d
				cmarkerCalibration.CombinedMarkers.ForEach(cm => {
					cm.Rectangles.ForEach(rect => {
						ScaleRectInput(rect);
					});
				});
			} else {
				//1d
				var hmarkerCalibration = markers.Item as synesis.HeightMarkerCalibration;
				hmarkerCalibration.HeightMarkers.ForEach(hm => {
					hm.SurfaceNormals.ForEach(sn => {
						DataConverter.ScalePointInput(sn.Point, videoSourceSize, videoEncoderSize);
						sn.Height = ScaleHeigthInput(sn.Height);
					});
				});
			}
		}
		void ScaleRectOutput(synesis.Rect rect) {
			DataConverter.ScalePointOutput(rect.LeftTop, videoSourceSize, videoEncoderSize);
			DataConverter.ScalePointOutput(rect.RightBottom, videoSourceSize, videoEncoderSize);
		}
		void ScaleRectInput(synesis.Rect rect) {
			DataConverter.ScalePointInput(rect.LeftTop, videoSourceSize, videoEncoderSize);
			DataConverter.ScalePointInput(rect.RightBottom, videoSourceSize, videoEncoderSize);
		}
		float ScaleHeigthOutput(float dval) {
			var val = (double)dval;
			double scaley = videoDescr.videoSourceResolution.Height / videoDescr.videoInfo.Resolution.Height;
			val = val * scaley;
			//scale from visible to [-1;1]
			double heightValue = videoDescr.videoSourceResolution.Height;
			return (float)((val * 2) / heightValue);
		}
		float ScaleHeigthInput(float dval) {
			//scale from [-1;1] to visible dimensions
			var val = (videoDescr.videoSourceResolution.Height / 2) * (dval);

			double scaley = videoDescr.videoInfo.Resolution.Height / videoDescr.videoSourceResolution.Height;
			val = val * scaley;
			return (float)val;
		}
		void ScaleUserRegionInput(synesis.UserRegion uregion) {
			uregion.Points.ForEach(x => {
				DataConverter.ScalePointInput(x, videoSourceSize, videoEncoderSize);
			});
		}
		void ScaleUserRegionOutput(synesis.UserRegion uregion) {
			uregion.Points.ForEach(x => {
				DataConverter.ScalePointOutput(x, videoSourceSize, videoEncoderSize);
			});
		}
		public void Apply() {
			try {
				pageObjectTracker.Apply();
				pageAntishaker.Apply();
				pageDepthCalibration.Apply();
				pageTampering.Apply();

				GetSimpleItems();
				GetElementItems();
			} catch (Exception err) {
				dbg.Error(err);
			}
		}

		IPlaybackSession playbackSession;
		VideoBuffer vidBuff;
		CompositeDisposable disposables;
		void VideoStartup(StreamInfoArgs args) {//, string profToken) {
			vidBuff = new VideoBuffer((int)args.sourceResolution.Width, (int)args.sourceResolution.Height);

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

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Shutdown() {
		}

		IUnityContainer container;

		public bool Init(IUnityContainer container, StreamInfoArgs args, odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr) {
			this.modulDescr = modulDescr;
			this.container = container;

			try {
				this.videoDescr = new ConfigureAnalyticView.AnalyticsVideoDescriptor() {
					videoInfo = new VideoInfo() { MediaUri = args.streamUri, Resolution = new Size() { Width = args.sourceResolution.Width, Height = args.sourceResolution.Height } },
					videoSourceResolution = new Size() { Width = args.encoderResolution.Width, Height = args.encoderResolution.Height }
				};

				VideoStartup(args);//, videoDescr.profileToken);

				videoSourceSize = new Size(videoDescr.videoSourceResolution.Width, videoDescr.videoSourceResolution.Height);
				videoEncoderSize = new Size(videoDescr.videoInfo.Resolution.Width, videoDescr.videoInfo.Resolution.Height);

				model = new SynesisAnalyticsModel(modulDescr.schema);

			} catch (Exception err) {
				return false;
			}

			try {
				FillSimpleItems(modulDescr.config.parameters.simpleItem, model);
			} catch (Exception err) {
				dbg.Error(err);
				return false;
			}
			try {
				FillElementItems(modulDescr.config.parameters.elementItem, model);
			} catch (Exception err) {
				dbg.Error(err);
				return false;
			}
			try {
				pageAntishaker.Init(container, model, videoDescr.videoInfo);//, videoDescr.profileToken);
			} catch (Exception err) {
				dbg.Error(err);
				return false;
			}
			try {
				pageDepthCalibration.Init(container, model, videoDescr.videoInfo);//, videoDescr.profileToken);
			} catch (Exception err) {
				dbg.Error(err);
				return false;
			}
			try {
				pageObjectTracker.Init(container, model, videoDescr.videoInfo);//, videoDescr.profileToken);
			} catch (Exception err) {
				dbg.Error(err);
				return false;
			}
			try {
				pageTampering.Init(container, model, videoDescr.videoInfo);//, videoDescr.profileToken);
			} catch (Exception err) {
				dbg.Error(err);
				return false;
			}

			analyticsTabCtrl.RequestBringIntoView += new RequestBringIntoViewEventHandler((sender, evargs) => {
				var tab = ((System.Windows.Controls.Primitives.Selector)(sender)).SelectedItem;
				var tctrl = tab as TabItem;
				if (tctrl.Name == tabAntishaker.Name) {
					pageAntishaker.SetPlayer(null);
					pageDepthCalibration.SetPlayer(null);
					pageObjectTracker.SetPlayer(null);
					pageTampering.SetPlayer(null);

					pageAntishaker.SetPlayer(player);
				} else if (tctrl.Name == tabDepthCalibration.Name) {
					pageAntishaker.SetPlayer(null);
					pageDepthCalibration.SetPlayer(null);
					pageObjectTracker.SetPlayer(null);
					pageTampering.SetPlayer(null);

					pageDepthCalibration.SetPlayer(player);

				} else if (tctrl.Name == tabObjectTracker.Name) {
					pageAntishaker.SetPlayer(null);
					pageDepthCalibration.SetPlayer(null);
					pageObjectTracker.SetPlayer(null);
					pageTampering.SetPlayer(null);

					pageObjectTracker.SetPlayer(player);

				} else if (tctrl.Name == tabTampering.Name) {
					pageAntishaker.SetPlayer(null);
					pageDepthCalibration.SetPlayer(null);
					pageObjectTracker.SetPlayer(null);
					pageTampering.SetPlayer(null);

					pageTampering.SetPlayer(player);

				}
			});

			//TODO: Stub fix for #225 Remove this with plugin functionality
			last = container.Resolve<odm.ui.activities.ILastChangedModule>();
			analyticsTabCtrl.SelectionChanged += new SelectionChangedEventHandler((obj, arg) => {
				var selection = analyticsTabCtrl.SelectedItem as TabItem;
				var antishaker = selection.Content as AntishakerView;
				if (antishaker != null) {
					last.Tag = "pageAntishaker";
				}
				var calibration = selection.Content as DepthCalibrationView;
				if (calibration != null) {
					last.Tag = "pageDepthCalibration";
				}
				var objecttracker = selection.Content as ObjectTrackerView;
				if (objecttracker != null) {
					last.Tag = "pageObjectTracker";
				}
				var tampering = selection.Content as TamperingDetectorsView;
				if (tampering != null) {
					last.Tag = "pageTampering";
				}
			});
			if (last.Tag != "") {
				switch (last.Tag) {
					case "pageAntishaker":
						analyticsTabCtrl.SelectedItem = tabAntishaker;

						pageDepthCalibration.SetPlayer(null);
						pageTampering.SetPlayer(null);
						pageObjectTracker.SetPlayer(null);

						pageAntishaker.SetPlayer(player);
						break;
					case "pageDepthCalibration":
						analyticsTabCtrl.SelectedItem = tabDepthCalibration;

						pageTampering.SetPlayer(null);
						pageAntishaker.SetPlayer(null);
						pageObjectTracker.SetPlayer(null);

						pageDepthCalibration.SetPlayer(player);
						break;
					case "pageObjectTracker":
						analyticsTabCtrl.SelectedItem = tabObjectTracker;

						pageDepthCalibration.SetPlayer(null);
						pageTampering.SetPlayer(null);
						pageAntishaker.SetPlayer(null);

						pageObjectTracker.SetPlayer(player);

						break;
					case "pageTampering":
						analyticsTabCtrl.SelectedItem = tabTampering;

						pageDepthCalibration.SetPlayer(null);
						pageObjectTracker.SetPlayer(null);
						pageAntishaker.SetPlayer(null);

						pageTampering.SetPlayer(player);
						break;
				}
			} else {
				pageDepthCalibration.SetPlayer(null);
				pageTampering.SetPlayer(null);
				pageAntishaker.SetPlayer(null);
				pageObjectTracker.SetPlayer(player);
			}
			//

			return true;
		}

		//TODO: Stub fix for #225 Remove this with plugin functionality
		odm.ui.activities.ILastChangedModule last;
		//
		public void Dispose() {
			pageAntishaker.Dispose();
			pageDepthCalibration.Dispose();
			pageObjectTracker.Dispose();
			pageTampering.Dispose();

			if (vidBuff != null) {
				vidBuff.Dispose();
			}
			disposables.Dispose();
		}
	}
}
