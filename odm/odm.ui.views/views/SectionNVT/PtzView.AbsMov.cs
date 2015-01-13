using onvif.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace odm.ui.activities {

	public partial class PtzView {

		class AbsMov {
			public readonly Range<float> rng;
			public float pos { get; private set; }
			public readonly AbsRelSpeed speed;
			AbsMov(float min, float max, AbsRelSpeed speed) {
				rng = new Range<float>(min, max);
				pos = 0f;
				if (min >= 0f || max <= 0f) {
					pos = rng.Coerce(min / 2f + max / 2f);
				}
				this.speed = speed;
			}

			public static PtzVec<FloatRange> GetPosRanges(PtzView view) {
				FloatRange pan, tilt, zoom;

				var panTiltSpace = view.ptzSpacesConfig.absPanTiltPosition;
				var zoomSpace = view.ptzSpacesConfig.absZoomPosition;

				if (panTiltSpace != null) {
					pan = panTiltSpace.xRange; tilt = panTiltSpace.yRange;
				} else {
					pan = tilt = null;
				}
				zoom = zoomSpace != null ? zoomSpace.xRange : null;
				return Ptz.Vec(pan, tilt, zoom);
			}

			public static PtzVec<Slider> GetPosSliders(PtzView view) {
				return Ptz.Vec(
					pan: view.sliderAbsPanValue,
					tilt: view.sliderAbsTiltValue,
					zoom: view.sliderAbsZoomValue
				);
			}
			public static PtzVec<Slider> GetSpeedSliders(PtzView view) {
				return Ptz.Vec(
					pan: view.sliderAbsPanSpeed,
					tilt: view.sliderAbsTiltSpeed,
					zoom: view.sliderAbsZoomSpeed
				);
			}

			public static AbsMov Setup(PtzAxis ax, PtzView view) {
				AbsMov res = null;
				var posSlider = GetPosSliders(view)[ax];
				var speedSlider = GetSpeedSliders(view)[ax];
				do {
					var range = GetPosRanges(view)[ax];
					if (range == null) {
						break;
					}
					var min = range.min;
					var max = range.max;
					if (float.IsNaN(min) || float.IsNaN(max) || min > max) {
						break;
					}
					if (float.IsNegativeInfinity(min)) {
						min = float.MinValue;
					}
					if (float.IsPositiveInfinity(max)) {
						max = float.MaxValue;
					}
					if (posSlider == null) {
						res = new AbsMov(min, max, AbsRelSpeed.Create(ax, view, null));
						break;
					}
					res = new AbsMov(min, max, AbsRelSpeed.Create(ax, view, speedSlider));
					posSlider.Minimum = min;
					posSlider.Maximum = max;
					posSlider.SmallChange = (max-min)/100f;
					posSlider.LargeChange = (max-min)/10f;
					posSlider.Value = res.pos;
					posSlider.Visibility = Visibility.Visible;
					if (min != max) {
						posSlider.IsEnabled = true;
						posSlider.ValueChanged += (s, a) => {
							res.pos = res.rng.Coerce((float)a.NewValue);
						};
					} else {
						posSlider.IsEnabled = false;
					}
					return res;
				} while (false);

				if (posSlider != null) {
					posSlider.Visibility = Visibility.Collapsed;
				}
				if (speedSlider != null) {
					speedSlider.Visibility = Visibility.Collapsed;
				}

				return res;
			}

		}

	}
}