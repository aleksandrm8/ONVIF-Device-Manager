using onvif.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace odm.ui.activities {

	public partial class PtzView {
		class AbsRelSpeed {

			public Range<float> rng { get; private set; }
			public float val { get; private set; }
			public readonly float def;

			public AbsRelSpeed(float min, float max, float def) {
				rng = new Range<float>(min, max);
				this.def = def;
				this.val = def;
			}
			public static PtzVec<Tuple<FloatRange, float>> GetSpeedInfo(PtzView view) {
				Tuple<FloatRange, float> pan = null;
				Tuple<FloatRange, float> tilt = null;
				Tuple<FloatRange, float> zoom = null;

				var defSpeeds = view.ptzConfig.defaultPTZSpeed;
				if (defSpeeds == null) {
					return Ptz.Vec(pan, tilt, zoom);
				}

				var panTiltDef = defSpeeds.panTilt;
				if (panTiltDef != null) {
					var panTiltSpace = view.ptzSpacesConfig.absRelPanTiltSpeed;
					var rng = panTiltSpace != null ? panTiltSpace.xRange : null;
					pan = Tuple.Create(rng, panTiltDef.x);
					tilt = Tuple.Create(rng, panTiltDef.y);
				}

				var zoomDef = defSpeeds.zoom;
				if (zoomDef != null) {
					var zoomSpace = view.ptzSpacesConfig.absRelZoomSpeed;
					var rng = zoomSpace != null ? zoomSpace.xRange : null;
					zoom = Tuple.Create(rng, zoomDef.x);
				}
				return Ptz.Vec(pan, tilt, zoom);
			}

			public static AbsRelSpeed Create(PtzAxis ax, PtzView view, Slider slider) {
				AbsRelSpeed res = null;
				do {
					var spdInf = GetSpeedInfo(view)[ax];
					if (spdInf == null) {
						break;
					}
					var range = spdInf.Item1;
					if (range == null) {
						break;
					}
					var def = spdInf.Item2;
					if (range == null) {
						break;
					}
					if (float.IsNaN(def)) {
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
					if (def > max || def < min) {
						res = new AbsRelSpeed(def, def, def);
						break;
					}
					res = new AbsRelSpeed(min, max, def);
					if (slider != null) {
						slider.Minimum = min;
						slider.Maximum = max;
						slider.SmallChange = (max - min) / 100f;
						slider.LargeChange = (max - min) / 10f;
						slider.Value = def;
						slider.Visibility = Visibility.Visible;
						if (min != max) {
							slider.IsEnabled = true;
							slider.ValueChanged += (s, a) => {
								res.val = res.rng.Coerce((float)a.NewValue);
							};
						} else {
							slider.IsEnabled = false;
						}
					}
					return res;
				} while (false);

				if (slider != null) {
					slider.Visibility = Visibility.Collapsed;
				}
				return res;
			}
		}
	}
}