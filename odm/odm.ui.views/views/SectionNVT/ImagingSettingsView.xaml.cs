using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls;
using odm.core;
using odm.infra;
using odm.onvif;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;
using utils;

namespace odm.ui.activities {

	/// <summary>
	/// Interaction logic for PTZView.xaml
	/// </summary>
	public partial class ImagingSettingsView : UserControl, IDisposable, IPlaybackController {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ImagingSettingsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		enum FocusSettingsStatus { Waiting, Success, Error }

		public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }
		CompositeDisposable disposables = new CompositeDisposable();
		LocalImaging Strings { get { return LocalImaging.instance; } }
		LinkButtonsStrings LinkStrings { get { return LinkButtonsStrings.instance; } }
		Model model;
		IImagingAsync session;
		IUnityContainer container;
		IPlaybackSession playbackSession;
		IVideoInfo videoInfo;

		void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.model = model;
			this.DataContext = model;

			var applyCmd = new DelegateCommand(
			() => Success(new Result.Apply(model)),
				 () => {
					 //if (context.model != null) {
					 //    return context.model.isModified;
					 //}
					 return true;
				 }
			);
			ApplyCommand = applyCmd;

			var revertCmd = new DelegateCommand(
				() => {
					if (model != null) {
						model.RevertChanges();
					}
				},
				() => {
					//if (context.model != null) {
					//    return !context.model.isModified;
					//}
					return true;
				}
			);

			InitializeComponent();

			this.container = activityContext.container;
			this.videoInfo = container.Resolve<IVideoInfo>();
			this.session = (IImagingAsync)activityContext.container.Resolve<INvtSession>();

			BindFocusMoveData();
			BindImagingData();
			Localization();

			var vInfo = container.Resolve<IVideoInfo>();

			VideoStartup(model.profToken);
		}
		
		void Localization() {
			//TODO 
			//imagingSettingsPanel.CreateBinding(GroupBox.HeaderProperty, Strings, s => s.imagingSettings);
			captionAutoFocusMode.CreateBinding(Label.ContentProperty, Strings, s => s.autoFocusMode);
			captionWhiteBalanceMode.CreateBinding(Label.ContentProperty, Strings, s => s.whitemode);
			captionBrightness.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			//captionCb.CreateBinding(Label.ContentProperty, Strings, s => s.whiteBalanceCb);
			//captionCompensation.CreateBinding(Label.ContentProperty, Strings, s => s);
			//captionCompensationMode.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			captionContrast.CreateBinding(Label.ContentProperty, Strings, s => s.contrast);
			//captionCr.CreateBinding(Label.ContentProperty, Strings, s => s.whiteBalanceCr);
			//captionExposureGain.CreateBinding(Label.ContentProperty, Strings, s => s.gain);
			//captionExposureIris.CreateBinding(Label.ContentProperty, Strings, s => s.iris);
			//captionExposureMode.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			//captionExposurePriority.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			//captionExposureTime.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			captionSaturation.CreateBinding(Label.ContentProperty, Strings, s => s.saturation);
			captionSharpness.CreateBinding(Label.ContentProperty, Strings, s => s.sharpness);
			//captionWideDynamicRange.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);

			focusSettingsPanel.CreateBinding(GroupBox.HeaderProperty, Strings, s => s.focusSettings);
			focusSettingsStatus.CreateBinding(Label.ContentProperty, Strings, s => s.status);
			focusSettingsStatusSuccess.CreateBinding(TextBlock.TextProperty, Strings, s => s.statusSuccess);
			focusSettingsStatusWaiting.CreateBinding(TextBlock.TextProperty, Strings, s => s.statusWaiting);

			captionPositionFocusMove.CreateBinding(Label.ContentProperty, Strings, s => s.focusPosition);
		}

		void BindImagingData() {
			if (model == null) {
				throw new InvalidOperationException("model is null");
			}

			var settings = model.settings;
			if (settings == null) {
				return;
				//throw new InvalidOperationException("model.settings == null");
			}

			var options = model.options;
			//if (options == null) {
			//	throw new InvalidOperationException("model.options == null");
			//}

			var focusSettings = settings.focus;
			if (focusSettings != null) {
				var focusOptions = options != null ? options.focus : null;
				if (focusOptions == null) {
					focusOptions = new FocusOptions20() {
						autoFocusModes = new AutoFocusMode[] { focusSettings.autoFocusMode },
						defaultSpeed = focusSettings.defaultSpeedSpecified ? new FloatRange() {
							min = focusSettings.defaultSpeed, max = focusSettings.defaultSpeed
						} : null
					};
				}
				InitDropDownList(
					valueAutoFocusMode,
					focusSettings.autoFocusMode /*value*/, focusOptions.autoFocusModes /*options*/,
					v => focusSettings.autoFocusMode = v /*updater*/
				);
				//TODO: no default speed settings
			}

			if (settings.brightnessSpecified) {
				InitSlider(
					valueBrightness, captionBrightness,
					settings.brightness /*value*/, options != null ? options.brightness : null /*range*/,
					v => settings.brightness = v /*updater*/
				);
			}

			if (settings.colorSaturationSpecified) {
				InitSlider(
					valueSaturation, captionSaturation,
					settings.colorSaturation /*value*/, options != null ? options.colorSaturation : null /*range*/,
					v => settings.colorSaturation = v /*updater*/
				);
			}

			if (settings.contrastSpecified) {
				InitSlider(
					valueContrast, captionContrast,
					settings.contrast /*value*/, options != null ? options.contrast : null /*range*/,
					v => settings.contrast = v /*updater*/
				);
			}

			if (settings.sharpnessSpecified) {
				InitSlider(
					valueSharpness, captionSharpness,
					settings.sharpness /*value*/, options != null ? options.sharpness : null /*range*/,
					v => settings.sharpness = v /*updater*/
				);
			}

			var wbSettings = settings.whiteBalance;
			if (wbSettings != null) {
				var wbOptions = options != null ? options.whiteBalance : null;
				if (wbOptions == null) {
					wbOptions = new WhiteBalanceOptions20() {
						mode = new WhiteBalanceMode[] { wbSettings.mode }
					};
				}
				InitDropDownList(
					valueWhiteBalanceMode,
					wbSettings.mode /*value*/, wbOptions.mode /*options*/,
					v => {
						wbSettings.mode = v;
					} /*updater*/
				);
				if (wbSettings.cbGainSpecified) {
					var slider = AddSlider(
						Strings.whiteBalanceCb,
						wbSettings.cbGain /*value*/, wbOptions.ybGain /*range*/,
						v => wbSettings.cbGain = v /*updater*/
					);
					valueWhiteBalanceMode.SelectionChanged += (s, a) => {
						slider.Visibility = wbSettings.mode != WhiteBalanceMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = wbSettings.mode != WhiteBalanceMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
				if (wbSettings.crGainSpecified) {
					var slider = AddSlider(
						Strings.whiteBalanceCr,
						wbSettings.crGain /*value*/, wbOptions.yrGain /*range*/,
						v => wbSettings.crGain = v /*updater*/
					);
					valueWhiteBalanceMode.SelectionChanged += (s, a) => {
						slider.Visibility = wbSettings.mode != WhiteBalanceMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = wbSettings.mode != WhiteBalanceMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
			}

			var blcSettings = settings.backlightCompensation;
			if (blcSettings != null) {
				var blcOptions = options != null ? options.backlightCompensation : null;
				if (blcOptions == null) {
					blcOptions = new BacklightCompensationOptions20() {
						mode = new BacklightCompensationMode[] { blcSettings.mode }
					};
				}
				var modeCombo = AddDropDownList(
					Strings.backlightMode,
					blcSettings.mode /*value*/, blcOptions.mode /*options*/,
					v => {
						blcSettings.mode = v;
					} /*updater*/
				);
				if (blcSettings.levelSpecified) {
					var slider = AddSlider(
						Strings.backlightLevel,
						blcSettings.level /*value*/, blcOptions.level /*range*/,
						v => blcSettings.level = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = blcSettings.mode == BacklightCompensationMode.on ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = blcSettings.mode == BacklightCompensationMode.on ? Visibility.Visible : Visibility.Collapsed;
				}
			}

			var wdrSettings = settings.wideDynamicRange;
			if (wdrSettings != null) {
				var wdrOptions = options != null ? options.wideDynamicRange : null;
				if (wdrOptions == null) {
					wdrOptions = new WideDynamicRangeOptions20() {
						mode = new WideDynamicMode[] { wdrSettings.mode }
					};
				}
				var modeCombo = AddDropDownList(
					"Wide dynamic range mode",
					wdrSettings.mode /*value*/, wdrOptions.mode /*options*/,
					v => {
						wdrSettings.mode = v;
					} /*updater*/
				);
				if (wdrSettings.levelSpecified) {
					var slider = AddSlider(
						"Wide dynamic range level",
						wdrSettings.level /*value*/, wdrOptions.level /*range*/,
						v => wdrSettings.level = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = wdrSettings.mode == WideDynamicMode.on ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = wdrSettings.mode == WideDynamicMode.on ? Visibility.Visible : Visibility.Collapsed;
				}
			}

			var expSettings = settings.exposure;
			if (expSettings != null) {
				var expOptions = options != null ? options.exposure : null;
				if (expOptions == null) {
					expOptions = new ExposureOptions20() {
						mode = new ExposureMode[] { expSettings.mode }
					};
				}
				var modeCombo = AddDropDownList(
					"Exposure mode",
					expSettings.mode /*value*/, expOptions.mode /*options*/,
					v => {
						expSettings.mode = v;
					} /*updater*/
				);
				if (expSettings.gainSpecified) {
					var slider = AddSlider(
						"Exposure gain",
						expSettings.gain /*value*/, expOptions.gain /*range*/,
						v => expSettings.gain = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode != ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode != ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
				if (expSettings.minGainSpecified) {
					var slider = AddSlider(
						"Exposure min gain",
						expSettings.minGain /*value*/, expOptions.minGain /*range*/,
						v => expSettings.minGain = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
				if (expSettings.maxGainSpecified) {
					var slider = AddSlider(
						"Exposure max gain",
						expSettings.maxGain /*value*/, expOptions.maxGain /*range*/,
						v => expSettings.maxGain = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}

				if (expSettings.exposureTimeSpecified) {
					var slider = AddSlider(
						"Exposure time",
						expSettings.exposureTime /*value*/, expOptions.exposureTime /*range*/,
						v => expSettings.exposureTime = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode != ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode != ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
				if (expSettings.minExposureTimeSpecified) {
					var slider = AddSlider(
						"Exposure min time",
						expSettings.minExposureTime /*value*/, expOptions.minExposureTime /*range*/,
						v => expSettings.minExposureTime = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
				if (expSettings.maxExposureTimeSpecified) {
					var slider = AddSlider(
						"Exposure max time",
						expSettings.maxExposureTime /*value*/, expOptions.maxExposureTime /*range*/,
						v => expSettings.maxExposureTime = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}

				if (expSettings.irisSpecified) {
					var slider = AddSlider(
						"Exposure time",
						expSettings.iris /*value*/, expOptions.iris /*range*/,
						v => expSettings.iris = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode != ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode != ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
				if (expSettings.minIrisSpecified) {
					var slider = AddSlider(
						"Exposure min iris",
						expSettings.minIris /*value*/, expOptions.minIris /*range*/,
						v => expSettings.minIris = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
				if (expSettings.maxIrisSpecified) {
					var slider = AddSlider(
						"Exposure max iris",
						expSettings.maxIris /*value*/, expOptions.maxIris /*range*/,
						v => expSettings.maxIris = v /*updater*/
					);
					modeCombo.SelectionChanged += (s, a) => {
						slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
					};
					slider.Visibility = expSettings.mode == ExposureMode.auto ? Visibility.Visible : Visibility.Collapsed;
				}
			}

			if (settings.irCutFilterSpecified) {
				var modes = options != null ? options.irCutFilterModes : null;
				if (modes == null) {
					modes = new IrCutFilterMode[0];
				}
				AddDropDownList(
					"Infrared cutoff filter settings",
					settings.irCutFilter /*value*/, options.irCutFilterModes /*options*/,
					v => {
						settings.irCutFilter = v;
					} /*updater*/
				);
			}
		}

		Slider AddSlider(string caption, float value, FloatRange range, Action<float> updater) {
			var row = imagingSettings.RowDefinitions.Count;
			var slider = new Slider() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			var label = new Label() {
				Content = caption,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left
			};
			var text = new TextBlock() {
				Text = value.ToString("F2"),
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(2,0,2,0)
			};
			slider.ValueChanged += (s,a)=>{
				text.Text = slider.Value.ToString("F2");
			};
			InitSlider(slider, label, value, range, updater);
			Grid.SetRow(label, row);
			Grid.SetColumn(label, 0);
			Grid.SetRow(text, row);
			Grid.SetColumn(text, 1);
			Grid.SetRow(slider, row);
			Grid.SetColumn(slider, 2);
			imagingSettings.RowDefinitions.Add(new RowDefinition());
			imagingSettings.Children.Add(slider);
			imagingSettings.Children.Add(label);
			imagingSettings.Children.Add(text);
			return slider;
		}

		ComboBox AddDropDownList<TVal>(string caption, TVal value, TVal[] options, Action<TVal> updater) {
			var row = imagingSettings.RowDefinitions.Count;
			var combo = new ComboBox() {
				SelectedValue = value,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left
			};
			var label = new Label() {
				Content = caption,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left
			};
			InitDropDownList(combo, value, options, updater);
			Grid.SetRow(label, row);
			Grid.SetColumn(label, 0);
			Grid.SetRow(combo, row);
			Grid.SetColumn(combo, 2);
			imagingSettings.RowDefinitions.Add(new RowDefinition());
			imagingSettings.Children.Add(combo);
			imagingSettings.Children.Add(label);
			return combo;
		}

		void InitSlider(Slider valueControl, Label captionControl, float value, FloatRange range, Action<float> updater) {
			double min;
			double max;
			if (range != null) {
				min = range.min;
				min = value < min ? value:min;
				max = range.max;
				max = value > max ? value:max;
			}else{
				min = value;
				max = value;
			}
			captionControl.Visibility = Visibility.Visible;
			valueControl.Minimum = min;
			valueControl.Maximum = max;
			valueControl.Value = value;
			valueControl.TickFrequency = 10.0;
			valueControl.SetUpdateTrigger(Slider.ValueProperty, (double v) => updater((float)v));
			valueControl.Visibility = Visibility.Visible;
			
		}

		void InitDropDownList<TVal>(ComboBox valueControl, TVal value, TVal[] options, Action<TVal> updater) {
				if (valueControl == null) {
					//throw new ArgumentNullException("valueControl");
					return;
				}
				try {
					var currentValue = value;
					if (options == null) {
						options = new TVal[] { currentValue };
					} else if (!options.Contains(currentValue)) {
						options = options.Append(currentValue).ToArray();
					}
					valueControl.ItemsSource = options;
					valueControl.SelectedValue = currentValue;
					if (options.Length > 1) {
						valueControl.SelectionChanged += (s, a) => {
							try {
								updater((TVal)valueControl.SelectedValue);
							} catch {
								//swallow error
							}
						};
						valueControl.IsEnabled = true;
					} else {
						valueControl.IsEnabled = false;
					}
					valueControl.Visibility = Visibility.Visible;
					valueControl.IsEditable = false;
						
				} catch {
					//swallow error
				}
		}

		void SwallowExceptions(Action block) {
			try {
				block();
			} catch (Exception err) {
				dbg.Error(err);
			}
		}

		#region Focus Settings

		void SetFocusSettingsStatus(FocusSettingsStatus status, string msg = null) {
			focusSettingsStatusSuccess.Visibility = Visibility.Collapsed;
			focusSettingsStatusWaiting.Visibility = Visibility.Collapsed;
			focusSettingsStatusError.Visibility = Visibility.Collapsed;

			if (status == FocusSettingsStatus.Success) {
				focusSettingsStatusSuccess.Visibility = Visibility.Visible;
			} else if (status == FocusSettingsStatus.Waiting) {
				focusSettingsStatusWaiting.Visibility = Visibility.Visible;
			} else if (status == FocusSettingsStatus.Error) {
				focusSettingsStatusError.Visibility = Visibility.Visible;
				focusSettingsStatusError.Text = msg ?? "Unknown error";
			}
		}

		void SendAbsoluteFocusPosition(float position) {
			Debug.Assert(model.moveOptions.absolute != null);
			Debug.Assert(model.moveOptions.absolute.position != null);
			Debug.Assert(model.moveOptions.absolute.position.min <= position && position <= model.moveOptions.absolute.position.max);

			SetFocusSettingsStatus(FocusSettingsStatus.Waiting);
			panelPositionFocusMove.IsEnabled = false;

			var absolute = new AbsoluteFocus() { position = position };
			if (model.moveOptions.absolute.speed != null) {
				absolute.speed = model.moveOptions.absolute.speed.max;
				absolute.speedSpecified = true;
			}
			var focus = new FocusMove() { absolute = absolute };

			disposables.Add(session.Move(model.sourceToken, focus).ObserveOnCurrentDispatcher().Subscribe(
				 u => { ReceiveFocusPosition(); },
				 err => { SetFocusSettingsStatus(FocusSettingsStatus.Error, err.Message); panelPositionFocusMove.IsEnabled = true; }));
		}

		void WaitForPositioningCompleted(ImagingStatus20 status, float prevPosition) {
			if (status.focusStatus20 != null) {
				if (status.focusStatus20.position == prevPosition) {
					valuePositionFocusMove.Value = status.focusStatus20.position;
					SetFocusSettingsStatus(FocusSettingsStatus.Success);
					panelPositionFocusMove.IsEnabled = true;
				} else {
					disposables.Add(session.GetStatus(model.sourceToken).ObserveOnCurrentDispatcher().Subscribe(
						 st => { WaitForPositioningCompleted(st, status.focusStatus20.position); },
						 err => { SetFocusSettingsStatus(FocusSettingsStatus.Error, err.Message); panelPositionFocusMove.IsEnabled = true; }
						 ));
				}
			} else {
				SetFocusSettingsStatus(FocusSettingsStatus.Error, "Response is empty");
				panelPositionFocusMove.IsEnabled = true;
			}
		}

		void ReceiveFocusPosition() {
			SetFocusSettingsStatus(FocusSettingsStatus.Waiting);
			panelPositionFocusMove.IsEnabled = false;

			disposables.Add(session.GetStatus(model.sourceToken).ObserveOnCurrentDispatcher().Subscribe(
				 st => { WaitForPositioningCompleted(st, -1); },
				 err => { SetFocusSettingsStatus(FocusSettingsStatus.Error, err.Message); panelPositionFocusMove.IsEnabled = true; }
				 ));
		}

		void SendDistanceFocusMove(float distance) {
			SetFocusSettingsStatus(FocusSettingsStatus.Waiting);
			panelPositionFocusMove.IsEnabled = false;

			var relative = new RelativeFocus() { distance = distance };
			if (model.moveOptions.relative.speed != null) {
				relative.speed = model.moveOptions.relative.speed.max;
				relative.speedSpecified = true;
			}
			var focus = new FocusMove() { relative = relative };

			//disposables += (IDisposable)null;

			disposables.Add(session.Move(model.sourceToken, focus).ObserveOnCurrentDispatcher().Subscribe(
				 u => { ReceiveFocusPosition(); },
				 err => { 
					 SetFocusSettingsStatus(FocusSettingsStatus.Error, err.Message); 
					 panelPositionFocusMove.IsEnabled = true; }
			));
		}

		void BindAbsolutePositionFocusMove() {
			var absolute = model.moveOptions.absolute;
			if (absolute != null && absolute.position != null) {
				var position = absolute.position;
				if (position.min < position.max) {
					valuePositionFocusMove.Minimum = absolute.position.min;
					valuePositionFocusMove.Maximum = absolute.position.max;
					valuePositionFocusMove.PreviewMouseLeftButtonUp += delegate {
						if (valuePositionFocusMove.IsMouseCaptureWithin)
							SendAbsoluteFocusPosition((float)valuePositionFocusMove.Value);
					};
					valuePositionFocusMove.Visibility = Visibility.Visible;
				}
			}
		}

		void BindRelativePositionFocusMove() {
			var relative = model.moveOptions.relative;
			if (relative != null && relative.distance != null) {
				var distance = relative.distance;
				if (distance.min < distance.max) {
					float delta = (distance.max - distance.min) / 100.0f;

					relativeLeftFocusMove.Click += delegate { SendDistanceFocusMove(-delta); };
					relativeRightFocusMove.Click += delegate { SendDistanceFocusMove(delta); };
					relativeLeftFocusMove.Visibility = Visibility.Visible;
					relativeRightFocusMove.Visibility = Visibility.Visible;
				}
			}
		}

		void BindFocusMoveData() {
			if (session == null) {
				throw new InvalidOperationException("session is null");
			}
			if (model == null) {
				throw new InvalidOperationException("model is null");
			}
			//if (model.moveOptions == null) {
			//	return;
			//	//throw new InvalidOperationException("model.moveOptions is null");
			//}

			if (model.settings == null || model.settings.focus == null || model.settings.focus.autoFocusMode != AutoFocusMode.manual) {
				return;
			}

			if (model.moveOptions == null && model.moveOptions.absolute == null && model.moveOptions.relative == null && model.moveOptions.continuous == null) {
				return;
			}

			focusSettingsPanel.Visibility = Visibility.Visible;

			SwallowExceptions(delegate { BindAbsolutePositionFocusMove(); });
			SwallowExceptions(delegate { BindRelativePositionFocusMove(); });
			//TODO BindContinuousPositionFocusMove

			ReceiveFocusPosition();
		}

		#endregion Focus Settings


		
		void VideoStartup(string token) {
			var playerAct = activityContext.container.Resolve<IVideoPlayerActivity>();

			var playerModel = new VideoPlayerActivityModel(
				profileToken: model.profToken,
				showStreamUrl: true,
				streamSetup: new StreamSetup() {
					stream = StreamType.rtpUnicast,
					transport = new Transport() {
						protocol = AppDefaults.visualSettings.Transport_Type,
						tunnel = null
					}
				}
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

	}
}
