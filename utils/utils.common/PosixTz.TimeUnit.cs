using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	
	public partial class PosixTz {

		public class TimeUnit: IEquatable<TimeUnit> {

			public readonly int hours;
			public readonly int minutes;
			public readonly int seconds;
			
			public TimeUnit(int hours, int minutes, int seconds) {
				this.hours = hours;
				this.minutes = minutes;
				this.seconds = seconds;
			}
			
			public int ToOffset(bool negative = false) {
				var offset = seconds + TAI.SecondsPerMinute * minutes + TAI.SecondsPerHour * hours;
				if (negative) {
					return -offset;
				}
				return offset;
			}
			
			public override string ToString() {
				return new StringBuilder()
					.Append(hours.ToString("00")).Append(":")
					.Append(minutes.ToString("00")).Append(":")
					.Append(seconds.ToString("00")).ToString();
			}

			public string Format() {
				var writer = new PosixTzWriter();
				writer.WriteTime(this);
				return writer.ToString();
			}

			public static TimeUnit Parse(string str) {
				var reader = new PosixTzReader(str);
				reader.SkipWhiteSpaces();
				TimeUnit time = reader.ReadTime();
				if (time == null) {
					reader.RaiseParseError();
				}
				reader.SkipWhiteSpaces();
				if (!reader.AtEnd()) {
					reader.RaiseParseError();
				}
				return time;
			}

			public override bool Equals(object obj) {
				return Equals(obj as TimeUnit);
			}
			public bool Equals(TimeUnit other) {
				return
					!Object.ReferenceEquals(other, null) &&
					other.hours == hours &&
					other.minutes == minutes &&
					other.seconds == seconds;
			}
			public override int GetHashCode() {
				return HashCode.Combine(hours, minutes, seconds);
			}
			public static bool operator ==(TimeUnit left, TimeUnit right) {
				if (Object.ReferenceEquals(left, right)) {
					return true;
				}
				return !Object.ReferenceEquals(left, null) && left.Equals(right);
			}
			public static bool operator !=(TimeUnit left, TimeUnit right) {
				return !(left == right);
			}
			

		}
	}
}
