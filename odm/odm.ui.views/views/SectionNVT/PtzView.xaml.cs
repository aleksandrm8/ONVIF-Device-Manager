using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.ui.viewModels;
using odm.ui.core;
using odm.player;
using utils;
using odm.ui.controls;
using Microsoft.Practices.Prism.Commands;
using onvif.services;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using odm.core;
using odm.infra;
using System.Windows.Controls.Primitives;
using System.Timers;
using System.Linq;
using odm.onvif;
using System.Globalization;
using System.Diagnostics;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for PTZView.xaml
	/// </summary>
	public partial class PtzView : UserControl, IDisposable, IPlaybackController, INotifyPropertyChanged {
		#region Activity definition

		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new PtzView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}

		#endregion

		enum PTZMoveModes {
			Absolute,
			Relative,
			Continuous
		}

		Model model;
		PTZMoveModes moveMode = (PTZMoveModes)(-1);
		//ObservableCollection<PTZNode> Nodes;
		public ObservableCollection<PTZPreset> Presets { get; private set; }

		public readonly Dispatcher dispatch = Dispatcher.CurrentDispatcher;
		private CompositeDisposable disposables = new CompositeDisposable();
		public CompositeDisposable subscription = new CompositeDisposable();
		public INvtSession CurrentSession;
		public Account CurrentAccount;
		public String ChannelToken;
		public string profileToken;
		PtzVec<RelMov> relMov;
		PtzVec<ContMov> contMov;
		PtzVec<AbsMov> absMov;

		void SetMoveMode(PTZMoveModes mode, bool set = true) {
			if ((this.moveMode != mode) ^ set) {
				return;
			}

			bool absOn = (mode == PTZMoveModes.Absolute) && set;
			absPanTiltControls.Visibility =
				absZoomControls.Visibility =
					absoluteMovePanel.Visibility = absOn ? Visibility.Visible : Visibility.Collapsed;
			tglbtnAbsoluteMove.IsChecked = absOn;

			bool relOn = (mode == PTZMoveModes.Relative) && set;
			relPanTiltControls.Visibility =
				relZoomControls.Visibility =
					relativeMovePanel.Visibility = relOn ? Visibility.Visible : Visibility.Collapsed;
			tglbtnRelativeMove.IsChecked = relOn;

			bool contOn = (mode == PTZMoveModes.Continuous) && set;
			contPanTiltControls.Visibility =
				contZoomControls.Visibility =
					continuousMovePanel.Visibility = contOn ? Visibility.Visible : Visibility.Collapsed;
			tglbtnContinuousMove.IsChecked = contOn;

			this.moveMode = set ? mode : (PTZMoveModes)(-1);
		}

		private void Init(Model model) {
			this.model = model;

			OnCompleted += () => {
				//TODO: release player host
				disposables.Dispose();
				subscription.Dispose();
			};
			this.DataContext = this;
			InitializeComponent();

			Presets = new ObservableCollection<PTZPreset>();

			var ptzInfo = activityContext.container.Resolve<IPtzInfo>();
			CurrentSession = activityContext.container.Resolve<INvtSession>();
			profileToken = ptzInfo.ProfileToken;

			VideoStartup();

			InitData();
			BindData();
			Localize();

		}

		public LocalPTZ Strings { get { return LocalPTZ.instance; } }
		void Localize() {
			//TODO localize!!!
		}

		#region Spaces

		public struct PtzSpacesConfig {
			public Space2DDescription absPanTiltPosition;
			public Space1DDescription absZoomPosition;
			public Space2DDescription relPanTiltTranslation;
			public Space1DDescription relZoomTranslation;
			public Space2DDescription contPanTiltVelocity;
			public Space1DDescription contZoomVelocity;
			public Space1DDescription absRelPanTiltSpeed;
			public Space1DDescription absRelZoomSpeed;
			public static T GetDefaultSpace<T>(T[] supported, Func<T, string> getUri, string def) where T : class {
				if (supported == null || supported.Length == 0 || String.IsNullOrEmpty(def)) {
					return null;
				}
				return supported.FirstOrDefault(s => getUri(s) == def);
			}
			public void Setup(PTZConfiguration config, PTZSpaces spaces) {
				absPanTiltPosition = GetDefaultSpace(
					spaces.absolutePanTiltPositionSpace, s => s.uri,
					config.defaultAbsolutePantTiltPositionSpace
				);

				absZoomPosition = GetDefaultSpace(
					spaces.absoluteZoomPositionSpace, s => s.uri,
					config.defaultAbsoluteZoomPositionSpace
				);

				relPanTiltTranslation = GetDefaultSpace(
					spaces.relativePanTiltTranslationSpace, s => s.uri,
					config.defaultRelativePanTiltTranslationSpace
				);

				relZoomTranslation = GetDefaultSpace(
					spaces.relativeZoomTranslationSpace, s => s.uri,
					config.defaultRelativeZoomTranslationSpace
				);

				contPanTiltVelocity = GetDefaultSpace(
					spaces.continuousPanTiltVelocitySpace, s => s.uri,
					config.defaultContinuousPanTiltVelocitySpace
				);

				contZoomVelocity = GetDefaultSpace(
					spaces.continuousZoomVelocitySpace, s => s.uri,
					config.defaultContinuousZoomVelocitySpace
				);

				absRelPanTiltSpeed = GetDefaultSpace(
					spaces.panTiltSpeedSpace, s => s.uri,
					config.IfNotNull(c => c.defaultPTZSpeed.IfNotNull(s => s.panTilt.IfNotNull(v => v.space)))
				);

				absRelZoomSpeed = GetDefaultSpace(
					spaces.zoomSpeedSpace, s => s.uri,
					config.IfNotNull(c => c.defaultPTZSpeed.IfNotNull(s => s.zoom.IfNotNull(v => v.space)))
				);
			}
		}
		PtzSpacesConfig ptzSpacesConfig;
		PTZConfiguration ptzConfig;
		#endregion Spaces

		void InitData() {
			//InitDefaultPTZSpaces();

			//Nodes.Clear();
			Presets.Clear();
			model.presets.ForEach(x => { Presets.Add(x); });
			valuePresetName.CreateBinding(TextBox.TextProperty, this, x => x.PresetName, (m, v) => { m.PresetName = v; });
			valuePresetsList.ItemsSource = Presets;
			valuePresetsList.CreateBinding(ListBox.SelectedItemProperty, this, x => x.SelectedPreset, (m, v) => m.SelectedPreset = v);

			var node = model.node;
			if (node == null) {
				return;
			}
			var supportedPtzSpaces = node.supportedPTZSpaces;
			if (supportedPtzSpaces == null) {
				return;
			}
			ptzConfig = model.profile.ptzConfiguration;
			if (ptzConfig == null) {
				return;
			}
			ptzSpacesConfig.Setup(ptzConfig, supportedPtzSpaces);
		}
		
		void BindData() {
			//CommonData
			//valuePresetName.CreateBinding(TextBox.TextProperty, this, x => x.PresetName, (m, v) => { m.PresetName = v; });
			//valuePresetsList.ItemsSource = Presets;
			//valuePresetsList.CreateBinding(ListBox.SelectedItemProperty, this, x => x.SelectedPreset, (m, v) => m.SelectedPreset = v);
			//ReloadPresets();

			captionErrorMessage.CreateBinding(TextBlock.TextProperty, this, x => x.ErrorMessage);

			// setup controls for absolute movements

			//var absPanTiltPositon = model.status.IfNotNull(s => s.position.IfNotNull(p => p.panTilt));
			//var absZoomPositon = model.status.IfNotNull(s => s.position.IfNotNull(p => p.zoom));
			
			relMov = Ptz.Vec(ax => RelMov.Setup(ax, this));
			contMov = Ptz.Vec(ax => ContMov.Setup(ax, this));
			absMov = Ptz.Vec(ax => AbsMov.Setup(ax, this));
			var hasRelativeMovements = relMov.ToSeq().Any(x => x.value != null);
			var hasContinuousMovements = contMov.ToSeq().Any(x => x.value != null);
			var hasAbsoluteMovements = absMov.ToSeq().Any(x => x.value != null);
			
			tglbtnAbsoluteMove.Visibility = hasAbsoluteMovements ? Visibility.Visible : Visibility.Collapsed;
			tglbtnRelativeMove.Visibility = hasRelativeMovements ? Visibility.Visible : Visibility.Collapsed;
			tglbtnContinuousMove.Visibility = hasContinuousMovements ? Visibility.Visible : Visibility.Collapsed;

			if (hasContinuousMovements) {
				SetMoveMode(PTZMoveModes.Continuous);
			} else if (hasRelativeMovements) {
				SetMoveMode(PTZMoveModes.Relative);
			} else if (hasAbsoluteMovements) {
				SetMoveMode(PTZMoveModes.Absolute);
			}


			//Buttons
			tglbtnAbsoluteMove.Checked += (s, e) => this.SetMoveMode(PTZMoveModes.Absolute, tglbtnAbsoluteMove.IsChecked == true);
			tglbtnRelativeMove.Checked += (s, e) => this.SetMoveMode(PTZMoveModes.Relative, tglbtnRelativeMove.IsChecked == true);
			tglbtnContinuousMove.Checked += (s, e) => this.SetMoveMode(PTZMoveModes.Continuous, tglbtnContinuousMove.IsChecked == true);


			btnContUp.PreviewMouseDown += ContinuousUp_MouseDown;
			btnContUp.PreviewMouseUp += ContinuousUp_MouseUp;
			btnContDown.PreviewMouseDown += ContinuousDown_MouseDown;
			btnContDown.PreviewMouseUp += ContinuousDown_MouseUp;
			btnContLeft.PreviewMouseDown += ContinuousLeft_MouseDown;
			btnContLeft.PreviewMouseUp += ContinuousLeft_MouseUp;
			btnContRight.PreviewMouseDown += ContinuousRight_MouseDown;
			btnContRight.PreviewMouseUp += ContinuousRight_MouseUp;
			btnContZoomMinus.PreviewMouseDown += ContinuousZoomMinus_MouseDown;
			btnContZoomMinus.PreviewMouseUp += ContinuousZoomMinus_MouseUp;
			btnContZoomPlus.PreviewMouseDown += ContinuousZoomPlus_MouseDown;
			btnContZoomPlus.PreviewMouseUp += ContinuousZoomPlus_MouseUp;


			btnRelUp.Click += RelUp_Click;
			btnRelRight.Click += RelRight_Click;
			btnRelDown.Click += RelDown_Click;
			btnRelLeft.Click += RelLeft_Click;
			btnRelZoomMinus.Click += RelZoomMinus_Click;
			btnRelZoomPlus.Click += RelZoomPlus_Click;

			btnAbsMove.Click += AbsoluteMove_Click;

			btnApplySettings.Click += new RoutedEventHandler(ApplySettings);
			btnDelete.Click += new RoutedEventHandler(Delete);
			btnGoHome.Click += new RoutedEventHandler(GoHome);
			btnGoTo.Click += new RoutedEventHandler(GoTo);
			btnSetHome.Click += new RoutedEventHandler(SetHome);
			btnSetPreset.Click += new RoutedEventHandler(SetPreset);
		}

		#region ShowError

		Timer errorTmr = new Timer(5000);
		void errorTmr_Elapsed(object sender, ElapsedEventArgs e) {
			dispatch.BeginInvoke(() => {
				ErrorMessage = "";
			});
		}
		void SetErrorMessage(string text) {
			if (ErrorMessage == "") {
				ErrorMessage = text;
			} else {
				ErrorMessage = ErrorMessage + System.Environment.NewLine + text;
			}

			errorTmr.Interval = 5000;

			errorTmr.AutoReset = false;
			errorTmr.Enabled = true;

			errorTmr.Elapsed -= errorTmr_Elapsed;
			errorTmr.Elapsed += new ElapsedEventHandler(errorTmr_Elapsed);
		}

		string errorMessage = "";
		public string ErrorMessage {
			get { return errorMessage; }
			set {
				errorMessage = value;
				NotifyPropertyChanged("ErrorMessage");
			}
		}

		#endregion ShowError


		#region Presets

		string presetName = "";
		public string PresetName {
			get {
				return presetName;
			}
			set {
				presetName = value;
				NotifyPropertyChanged("PresetName");
			}
		}
		PTZPreset selectedPreset = null;
		public PTZPreset SelectedPreset {
			get {
				return selectedPreset;
			}
			set {
				selectedPreset = value;
				NotifyPropertyChanged("SelectedPreset");
			}
		}

		private void ApplySettings(object sender, RoutedEventArgs e) {
			throw new NotImplementedException();
		}

		void Delete(object sender, RoutedEventArgs e) {
			if (SelectedPreset == null)
				return;
			try {
				subscription.Add(CurrentSession.RemovePreset(profileToken, SelectedPreset.token)
					 .ObserveOnCurrentDispatcher()
					 .Subscribe(presetTok => {
						 ReloadPresets();
					 }, err => {
						 SetErrorMessage(err.Message);
						 dbg.Error(err);
					 }));
			} catch (Exception err) {
				dbg.Error(err);
				//SetErrorMessage(err.Message);
			}
		}
		void GoHome(object sender, RoutedEventArgs e) {
			// This operation moves the PTZ unit to its home position. 
			// If the speed parameter is omitted, the default speed of the corresponding PTZ configuration shall be used. 
			// The speed parameter can only be specified when speed spaces are available for the PTZ node.
			// The command is non-blocking and can be interrupted by other move commands.

			//PTZSpeed speed = new PTZSpeed() {
			//	panTilt = new Vector2D() {
			//		x = defPanTiltSpeedSpace == null ? 0 : defPanTiltSpeedSpace.xRange.max,
			//		y = defPanTiltSpeedSpace == null ? 0 : defPanTiltSpeedSpace.xRange.max
			//	},
			//	zoom = new Vector1D() {
			//		x = defZoomSpeedSpace == null ? 0 : defZoomSpeedSpace.xRange.max
			//	}
			//};
			try {
				// TODO: get rid of memory leak
				subscription.Add(CurrentSession.GotoHomePosition(profileToken, null).ObserveOnCurrentDispatcher().Subscribe(unit => { }, err => { dbg.Error(err); }));
			} catch (Exception err) {
				dbg.Error(err);
				SetErrorMessage(err.Message);
			}
		}

		void GoTo(object sender, RoutedEventArgs e) {
			if (SelectedPreset == null) {
				return;
			}
			// The GotoPreset operation recalls a previously set preset. 
			// If the speed parameter is omitted, the default speed of the corresponding PTZ configuration shall be used. 
			// The speed parameter can only be specified when speed spaces are available for the PTZ node. 
			// The GotoPreset command is a non-blocking operation and can be interrupted by other move commands.

			//PTZSpeed speed = new PTZSpeed() {
			//	panTilt = new Vector2D() {
			//		x = defPanTiltSpeedSpace == null ? 0 : defPanTiltSpeedSpace.xRange.max,
			//		y = defPanTiltSpeedSpace == null ? 0 : defPanTiltSpeedSpace.xRange.max
			//	},
			//	zoom = new Vector1D() {
			//		x = defZoomSpeedSpace == null ? 0 : defZoomSpeedSpace.xRange.max
			//	}
			//};

			try {
				// TODO: get rid of memory leak
				subscription.Add(CurrentSession.GotoPreset(profileToken, SelectedPreset.token, null)
					 .ObserveOnCurrentDispatcher()
					 .Subscribe(unit => {
					 }, err => {
						 SetErrorMessage(err.Message);
					 }));
			} catch (Exception err) {
				SetErrorMessage(err.Message);
				dbg.Error(err);
			}
		}

		void SetHome(object sender, RoutedEventArgs e) {
			subscription.Add(CurrentSession.SetHomePosition(profileToken)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(
				 unit => { },
				 err => {
					 SetErrorMessage(err.Message);
					 dbg.Error(err);
				 }));
		}
		void ReloadPresets() {
			Presets.Clear();
			subscription.Add(CurrentSession.GetPresets(profileToken)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(presets => {
					 presets.ForEach(pres => {
						 Presets.Add(pres);
					 });
				 }, err => {
					 dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}
		void SetPreset(object sender, RoutedEventArgs e) {
			string defName = "Preset" + System.DateTime.Now.Ticks.ToString();
			if (PresetName != "") {
				defName = PresetName;
			}
			subscription.Add(CurrentSession.SetPreset(profileToken, defName, null)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(presetTok => {
					 //model.presets.Append(new PTZPreset() { token = presetTok });
					 ReloadPresets();
				 }, err => {
					 dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}

		#endregion Presets

		

		#region Absolute Move

		public void MoveAbsolute(PTZVector position, PTZSpeed speed) {
			CurrentSession
				.AbsoluteMove(profileToken, position, speed)
				.ObserveOnCurrentDispatcher()
				.Subscribe(unit => {}, err => SetErrorMessage(err.Message));
		}
		public void MoveAbsolute(Vector2D panTilt, Vector2D speed) {
			MoveAbsolute(
				new PTZVector() { panTilt = panTilt, zoom=null},
				speed != null ? new PTZSpeed(){panTilt = speed} : null
			);
		}
		public void MoveAbsolute(Vector1D zoom, Vector1D speed) {
			MoveAbsolute(
				new PTZVector() { panTilt = null, zoom = zoom },
				speed != null ? new PTZSpeed(){zoom = speed } : null
			);
		}

		Vector2D GetAbsPanTiltSpeed() {
			var pan = absMov.pan.IfNotNull(p=>p.speed);
			var tilt = absMov.tilt.IfNotNull(p => p.speed);
			switch ((pan != null ? 2 : 0) | (tilt != null ? 1 : 0)) {
				case 0: // both not supported 
					return null;
				case 1: // only pan not supported
					if (tilt.val == tilt.def) {
						return null;
					} else {
						return new Vector2D() { x = float.NaN, y = tilt.val };
					}
				case 2: // only tilt not supported
					if (pan.val == pan.def) {
						return null;
					} else {
						return new Vector2D() { x = pan.val, y = float.NaN };
					}
				case 3: // both supported
					if (pan.val == pan.def && tilt.val == tilt.def) {
						return null;
					} else {
						return new Vector2D() { x = pan.val, y = tilt.val };
					}
				default: // impossible
					return null;
			}
		}

		Vector1D GetAbsZoomSpeed() {
			var zoom = absMov.zoom.IfNotNull(p => p.speed);
			if (zoom == null || zoom.val == zoom.def) {
				// zoom not supported or defaults
				return null;
			}
			return new Vector1D() {
				x = zoom.val,
				space = null
			};
		}

		PTZSpeed GetAbsPtzSpeed() {
			var panTiltSpeed = GetAbsPanTiltSpeed();
			var zoomSpeed = GetAbsZoomSpeed();
			if (panTiltSpeed == null && zoomSpeed == null) {
				return null;
			}
			return new PTZSpeed() {
				panTilt = panTiltSpeed,
				zoom = zoomSpeed
			};
		}

		Vector2D GetAbsPanTiltPosition() {
			var pan = absMov.pan;
			var tilt = absMov.tilt;
			if (pan == null && tilt == null) {
				// both pan & tilt positions are not supported 
				return null;
			}
			// if one of pan or tilt isn't supported we will issue NAN as it value
			return new Vector2D() {
				x = pan != null ? pan.pos : float.NaN,
				y = tilt != null ? tilt.pos : float.NaN,
				space = null
			};
		}

		Vector1D GetAbsZoomPosition() {
			var zoom = absMov.zoom;
			if (zoom == null) {
				// zoom position is not supported 
				return null;
			}
			return new Vector1D() {
				x = zoom.pos,
				space = null
			};
		}

		PTZVector GetAbsPtzPosition() {
			var panTilt = GetAbsPanTiltPosition();
			var zoom = GetAbsZoomPosition();
			if (panTilt == null && zoom == null) {
				return null;
			}
			return new PTZVector() {
				panTilt = panTilt,
				zoom = zoom
			};
		}

		void AbsoluteMove_Click(object sender, RoutedEventArgs e) {
			var speed = GetAbsPtzSpeed();
			var position = GetAbsPtzPosition();
			MoveAbsolute(position, speed);
		}

		#endregion Absolute Move

		#region Relative Move

		public void MoveRelative(PTZVector translation, PTZSpeed speed) {
			CurrentSession
				.RelativeMove(profileToken, translation, speed)
				.ObserveOnCurrentDispatcher()
				.Subscribe(
					unit => {}, 
					err => {
						//dbg.Error(err);
						SetErrorMessage(err.Message);
					}
				);
		}
		
		Vector2D GetRelPanTiltTranslation(int panDir, int tiltDir) {
			var pan = relMov.pan;
			var tilt = relMov.tilt;
			if (pan == null && tilt == null) {
				return null;
			}
			return new Vector2D() {
				x = pan != null ? (panDir == 0 ? pan.origin : pan.GetVal(panDir<0)) : float.NaN,
				y = tilt != null ? (tiltDir == 0 ? tilt.origin : tilt.GetVal(tiltDir<0)) : float.NaN,
				space = null
			};
		}

		Vector1D GetRelZoomTranslation(int zoomDir) {
			var zoom = relMov.zoom;
			if (zoom == null) {
				return null;
			}
			return new Vector1D() {
				x = (zoomDir == 0 ? zoom.origin : zoom.GetVal(zoomDir < 0)),
				space = null
			};
		}

		PTZVector GetRelPtzTranslation(int panDir, int tiltDir, int zoomDir) {
			var panTilt = GetRelPanTiltTranslation(panDir, tiltDir);
			var zoom = GetRelZoomTranslation(zoomDir);
			if (panTilt == null && zoom == null) {
				return null;
			}
			return new PTZVector() {
				panTilt = panTilt,
				zoom = zoom
			};
		}


		Vector2D GetRelPanTiltSpeed() {
			var pan = relMov.pan.IfNotNull(p => p.speed);
			var tilt = relMov.tilt.IfNotNull(p => p.speed);
			switch ((pan != null ? 2 : 0) | (tilt != null ? 1 : 0)) {
				case 0: // both not supported 
					return null;
				case 1: // only pan not supported
					if (tilt.val == tilt.def) {
						return null;
					} else {
						return new Vector2D() { x = float.NaN, y = tilt.val };
					}
				case 2: // only tilt not supported
					if (pan.val == pan.def) {
						return null;
					} else {
						return new Vector2D() { x = pan.val, y = float.NaN };
					}
				case 3: // both supported
					if (pan.val == pan.def && tilt.val == tilt.def) {
						return null;
					} else {
						return new Vector2D() { x = pan.val, y = tilt.val };
					}
				default: // impossible
					return null;
			}
		}

		Vector1D GetRelZoomSpeed() {
			var zoom = relMov.zoom.IfNotNull(p => p.speed);
			if (zoom == null || zoom.val == zoom.def) {
				// zoom not supported or defaults
				return null;
			}
			return new Vector1D() {
				x = zoom.val,
				space = null
			};
		}

		void RelUp_Click(object sender, RoutedEventArgs e) {
			var panTilt = GetRelPanTiltTranslation(0, 1);
			if (panTilt != null) {
				var speed = GetRelPanTiltSpeed();
				MoveRelative(
					new PTZVector() { panTilt = panTilt}, 
					speed != null ? new PTZSpeed() { panTilt = speed } : null
				);
			}
		}

		void RelDown_Click(object sender, RoutedEventArgs e) {
			var panTilt = GetRelPanTiltTranslation(0, -1);
			if (panTilt != null) {
				var speed = GetRelPanTiltSpeed();
				MoveRelative(
					new PTZVector() { panTilt = panTilt },
					speed != null ? new PTZSpeed() { panTilt = speed } : null
				);
			}
		}

		void RelRight_Click(object sender, RoutedEventArgs e) {
			var panTilt = GetRelPanTiltTranslation(1, 0);
			if (panTilt != null) {
				var speed = GetRelPanTiltSpeed();
				MoveRelative(
					new PTZVector() { panTilt = panTilt },
					speed != null ? new PTZSpeed() { panTilt = speed } : null
				);
			}
		}

		void RelLeft_Click(object sender, RoutedEventArgs e) {
			var panTilt = GetRelPanTiltTranslation(-1, 0);
			if (panTilt != null) {
				var speed = GetRelPanTiltSpeed();
				MoveRelative(
					new PTZVector() { panTilt = panTilt },
					speed != null ? new PTZSpeed() { panTilt = speed } : null
				);
			}
		}

		void RelZoomPlus_Click(object sender, RoutedEventArgs e) {
			var zoom = GetRelZoomTranslation(1);
			if (zoom != null) {
				var speed = GetRelZoomSpeed();
				MoveRelative(
					new PTZVector() { zoom = zoom },
					speed != null ? new PTZSpeed() { zoom = speed } : null
				);
			}
		}


		void RelZoomMinus_Click(object sender, RoutedEventArgs e) {
			var zoom = GetRelZoomTranslation(-1);
			if (zoom != null) {
				var speed = GetRelZoomSpeed();
				MoveRelative(
					new PTZVector() { zoom = zoom },
					speed != null ? new PTZSpeed() { zoom = speed } : null
				);
			}
		}


		#endregion Relative Move


		#region Continuous Move

		public void MoveContinuous(Vector2D panTilt, Vector1D zoom) {
			CurrentSession
				.ContinuousMove(
					profileToken, 
					new PTZSpeed() { panTilt = panTilt, zoom = zoom }, 
					null
				)
				.ObserveOnCurrentDispatcher()
				.Subscribe(
					unit => { },
					err => {
						//dbg.Error(err);
						SetErrorMessage(err.Message);
					}
				);
		}

		public void StopZoom() {
			subscription.Add(
				CurrentSession.Stop(profileToken, false, true)
			.ObserveOnCurrentDispatcher()
			.Subscribe(unit => {
			}, err => {
				dbg.Error(err);
				SetErrorMessage(err.Message);
			}));
		}

		public void StopPanTilt() {
			subscription.Add(
				CurrentSession.Stop(profileToken, true, false)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(unit => {
				 }, err => {
					 //dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}

		Vector2D GetContPanTiltVelocity(int panDir, int tiltDir) {
			var pan = contMov.pan;
			var tilt = contMov.tilt;
			if (pan == null && tilt == null) {
				return null;
			}
			return new Vector2D() {
				x = pan != null ? (panDir == 0 ? pan.origin : pan.GetVal(panDir < 0)) : float.NaN,
				y = tilt != null ? (tiltDir == 0 ? tilt.origin : tilt.GetVal(tiltDir < 0)) : float.NaN,
				space = null
			};
		}

		Vector1D GetContZoomVelocity(int zoomDir) {
			var zoom = contMov.zoom;
			if (zoom == null) {
				return null;
			}
			return new Vector1D() {
				x = (zoomDir == 0 ? zoom.origin : zoom.GetVal(zoomDir < 0)),
				space = null
			};
		}

		void ContinuousUp_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopPanTilt();
		}
		void ContinuousUp_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var panTilt = GetContPanTiltVelocity(0, 1);
			if (panTilt != null) {
				MoveContinuous(panTilt, null);
			}
		}

		void ContinuousDown_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopPanTilt();
		}
		void ContinuousDown_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var panTilt = GetContPanTiltVelocity(0, -1);
			if (panTilt != null) {
				MoveContinuous(panTilt, null);
			}
		}

		void ContinuousRight_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopPanTilt();
		}
		void ContinuousRight_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var panTilt = GetContPanTiltVelocity(1, 0);
			if (panTilt != null) {
				MoveContinuous(panTilt, null);
			}
		}

		void ContinuousLeft_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopPanTilt();
		}
		void ContinuousLeft_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var panTilt = GetContPanTiltVelocity(-1, 0);
			if (panTilt != null) {
				MoveContinuous(panTilt, null);
			}
		}

		void ContinuousZoomPlus_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopZoom();
		}
		void ContinuousZoomPlus_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var zoom = GetContZoomVelocity(1);
			if (zoom != null) {
				MoveContinuous(null, zoom);
			}
		}

		void ContinuousZoomMinus_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopZoom();
		}
		void ContinuousZoomMinus_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var zoom = GetContZoomVelocity(-1);
			if (zoom != null) {
				MoveContinuous(null, zoom);
			}
		}

		#endregion Continuous Move


		#region VideoPlayback

		IPlaybackSession playbackSession;
		void VideoStartup() {
			var playerAct = activityContext.container.Resolve<IVideoPlayerActivity>();

			var playerModel = new VideoPlayerActivityModel(
				streamSetup: new StreamSetup() {
					stream = StreamType.rtpUnicast,
					transport = new Transport() {
						protocol = AppDefaults.visualSettings.Transport_Type,
						tunnel = null
					}
				},
				profile: model.profile,
				showStreamUrl: false
			);

			disposables.Add(
				  activityContext.container.RunChildActivity(player, playerModel, (c, m) => playerAct.Run(c, m))
			);
		}

		public void Dispose() {
			Cancel();
		}

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Shutdown() {
		}
		#endregion VideoPlayback


		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}

	public interface IPtzInfo {
		string ProfileToken { get; set; }
	}
	public class PtzInfo : IPtzInfo {
		public string ProfileToken { get; set; }
	}
}
