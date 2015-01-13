using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using onvif.services;

namespace odm.ui.activities {
	public struct Range<T> {
		public readonly T min;
		public readonly T max;
		public Range(T min, T max) {
			this.min = min; this.max = max;
		}
	}
	public static class RangeEtensions {
		public static float Coerce(this Range<float> rng, float val) {
			if (val < rng.min) {
				return rng.min;
			}
			if (val > rng.max) {
				return rng.max;
			}
			return val;
		}
	}
	public partial class PtzView {
		public enum PtzMovType {
			abs, rel, cont
		}
		public enum PtzSpaceType {
			pos, speed, vel, trans
		}
		public enum PtzAxis {
			pan, tilt, zoom
		}
		public struct PtzAxisVal<T> {
			public readonly PtzAxis axis;
			public readonly T value;
			public PtzAxisVal(PtzAxis axis, T value) {
				this.axis = axis; this.value = value;
			}
		}

		public struct PtzVec<T> {
			public T pan;
			public T tilt;
			public T zoom;
			public T this[PtzAxis c] {
				get {
					switch (c) {
						case PtzAxis.pan: return pan;
						case PtzAxis.tilt: return tilt;
						case PtzAxis.zoom: return zoom;
						default: return default(T);
					}
				}
				set {
					switch (c) {
						case PtzAxis.pan: pan = value; break;
						case PtzAxis.tilt: tilt = value; break;
						case PtzAxis.zoom: zoom = value; break;
					}
				}
			}
			public PtzVec(T defVal) {
				pan = defVal;
				tilt = defVal;
				zoom = defVal;
			}
			public PtzVec(Func<PtzAxis, T> fact) {
				pan = fact(PtzAxis.pan);
				tilt = fact(PtzAxis.tilt);
				zoom = fact(PtzAxis.zoom);
			}
			public PtzVec(T pan, T tilt, T zoom) {
				this.pan = pan;
				this.tilt = tilt;
				this.zoom = zoom;
			}
			public PtzVec(IEnumerable<PtzAxisVal<T>> seq) {
				var tmp = default(PtzVec<T>);
				if (seq != null) {
					foreach (var av in seq) {
						tmp[av.axis] = av.value;
					}
				}
				this = tmp;
			}
			public IEnumerable<PtzAxisVal<T>> ToSeq() {
				yield return new PtzAxisVal<T>(PtzAxis.pan, pan);
				yield return new PtzAxisVal<T>(PtzAxis.tilt, tilt);
				yield return new PtzAxisVal<T>(PtzAxis.zoom, zoom);
			}
		}

		static class Ptz {
			public static PtzVec<T> VecFromSeq<T>(IEnumerable<PtzAxisVal<T>> seq) {
				return new PtzVec<T>(seq);
			}
			//public static PtzVec<T> ToVec<T>(this IEnumerable<PtzAxisVal<T>> seq) {
			//	return new PtzVec<T>(seq);
			//}
			public static PtzAxisVal<T> AxisVal<T>(PtzAxis axis, T value) {
				return new PtzAxisVal<T>(axis, value);
			}
			public static PtzVec<T> Vec<T>(T pan, T tilt, T zoom) {
				return new PtzVec<T>(pan, tilt, zoom);
			}
			public static PtzVec<T> Vec<T>(Func<PtzAxis, T> fact) {
				return new PtzVec<T>(fact);
			}
			public static IEnumerable<PtzAxis> axes {
				get {
					yield return PtzAxis.pan;
					yield return PtzAxis.tilt;
					yield return PtzAxis.zoom;
				}
			}
		}
	}
}
