using onvif.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace odm.ui.activities {

	public partial class PtzView {

		class RelMov {
			public readonly Range<float> rng;
			public readonly float origin;
			public float step { get; private set; }
			public readonly AbsRelSpeed speed;
			RelMov(float min, float max, AbsRelSpeed speed) {
				rng = new Range<float>(min, max);
				origin = 0f;
				if (min >= 0f || max <= 0f) {
					origin = rng.Coerce(min / 2f + max / 2f);
				}
				step = 10f;
				this.speed = speed;
			}
			public float GetVal(bool inv) {
				var k = inv ? (rng.min - origin) : (rng.max - origin);
				return rng.Coerce(step / 100f * k + origin);
			}

			public static PtzVec<FloatRange> GetValueRanges(PtzView view) {
				FloatRange pan, tilt, zoom;

				var panTiltSpace = view.ptzSpacesConfig.relPanTiltTranslation;
				var zoomSpace = view.ptzSpacesConfig.relZoomTranslation;

				if (panTiltSpace != null) {
					pan = panTiltSpace.xRange; tilt = panTiltSpace.yRange;
				} else {
					pan = tilt = null;
				}
				zoom = zoomSpace != null ? zoomSpace.xRange : null;
				return Ptz.Vec(pan, tilt, zoom);
			}

			public static PtzVec<Slider> GetStepSliders(PtzView view) {
				return Ptz.Vec(
					pan: view.sliderRelPanValue,
					tilt: view.sliderRelTiltValue,
					zoom: view.sliderRelZoomValue
				);
			}
			public static PtzVec<Slider> GetSpeedSliders(PtzView view) {
				return Ptz.Vec(
					pan: view.sliderRelPanSpeed,
					tilt: view.sliderRelTiltSpeed,
					zoom: view.sliderRelZoomSpeed
				);
			}

			public static RelMov Setup(PtzAxis ax, PtzView view) {
				RelMov res = null;
				var stepSlider = GetStepSliders(view)[ax];
				var sppedSlider = GetSpeedSliders(view)[ax];
				do {
					var range = GetValueRanges(view)[ax];
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
					if (stepSlider == null) {
						res = new RelMov(min, max, AbsRelSpeed.Create(ax, view, null));
						break;
					}
					res = new RelMov(min, max, AbsRelSpeed.Create(ax, view, sppedSlider));
					stepSlider.Minimum = 0d;
					stepSlider.Maximum = 100d;
					stepSlider.SmallChange = 1d;
					stepSlider.LargeChange = 10d;
					stepSlider.Value = res.step;
					stepSlider.Visibility = Visibility.Visible;
					if (min != max) {
						stepSlider.IsEnabled = true;
						stepSlider.ValueChanged += (s, a) => {
							res.step = (float)a.NewValue;
						};
					} else {
						stepSlider.IsEnabled = false;
					}
					return res;
				} while(false);

				if (stepSlider != null) {
					stepSlider.Visibility = Visibility.Collapsed;
				}
				if (sppedSlider != null) {
					sppedSlider.Visibility = Visibility.Collapsed;
				}

				return res;
			}
			
		}

	}
}