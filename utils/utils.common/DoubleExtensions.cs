using System;

namespace utils {
	public static class DoubleExtensions {
		//TODO: comments is needed
		public static int ToInt32(this double value) {
			if (value >= 0.0) {
				if (value < 2147483647.5) {
					int result = (int)value;
					double diff = value - (double)result;
					if (diff >= 0.5) {
						result++;
					}
					return result;
				}
			} else {
				if (value > -2147483648.5) {
					int result = (int)value;
					double diff = value - (double)result;
					if (diff <= -0.5) {
						result--;
					}
					return result;
				}
			}
			throw new OverflowException("failed to cast double to int");
		}
	}
}
