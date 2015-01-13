using onvif.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace odm.ui.activities {

	public partial class PtzView {
		class ContMov {
			public readonly Range<float> rng;
			public readonly float origin;
			public float level { get; private set; }
			ContMov(float min, float max) {
				rng = new Range<float>(min, max);
				origin = 0f;
				if (min >= 0f || max <= 0f) {
					origin = rng.Coerce(min / 2f + max / 2f);
				}
				level = 50f;
			}
			public float GetVal(bool inv) {
				var k = inv ? (rng.min - origin) : (rng.max - origin);
				return rng.Coerce(level / 100f * k + origin);
			}

			public static PtzVec<FloatRange> GetVelRanges(PtzView view) {
				FloatRange pan, tilt, zoom;

				var panTiltSpace = view.ptzSpacesConfig.contPanTiltVelocity;
				var zoomSpace = view.ptzSpacesConfig.contZoomVelocity;

				if (panTiltSpace != null) {
					pan = panTiltSpace.xRange; tilt = panTiltSpace.yRange;
				} else {
					pan = tilt = null;
				}
				zoom = zoomSpace != null ? zoomSpace.xRange : null;
				return Ptz.Vec(pan, tilt, zoom);
			}

			public static PtzVec<Slider> GetLvlSliders(PtzView view) {
				return Ptz.Vec(
					pan: view.sliderContPanVelocity,
					tilt: view.sliderContTiltVelocity,
					zoom: view.sliderContZoomVelocity
				);
			}

			public static ContMov Setup(PtzAxis ax, PtzView view) {
				ContMov res = null;
				var lvlSlider = GetLvlSliders(view)[ax];
				do {
					var range = GetVelRanges(view)[ax];
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
					res = new ContMov(min, max);
					if (lvlSlider != null) {
						lvlSlider.Minimum = 0d;
						lvlSlider.Maximum = 100d;
						lvlSlider.SmallChange = 1d;
						lvlSlider.LargeChange = 10d;
						lvlSlider.Value = res.level;
						lvlSlider.Visibility = Visibility.Visible;
						if (min != max) {
							lvlSlider.IsEnabled = true;
							lvlSlider.ValueChanged += (s, a) => {
								res.level = (float)a.NewValue;
							};
						} else {
							lvlSlider.IsEnabled = false;
						}
					}
					return res;
				} while (false);

				if (lvlSlider != null) {
					lvlSlider.Visibility = Visibility.Collapsed;
				}

				return res;
			}

		}

	}
}