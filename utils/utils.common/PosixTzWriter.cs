using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public class PosixTzWriter {
		StringBuilder sb = new StringBuilder();

		public void WriteTime(PosixTz.TimeUnit time) {
			sb.Append(time.hours);
			if (time.minutes == 0 && time.seconds == 0) {
				return;
			}
			sb.Append(":");
			sb.Append(time.minutes);
			if (time.seconds == 0) {
				return;
			}
			sb.Append(":");
			sb.Append(time.seconds);
		}

		public void WriteTimeOffset(int offset) {
			if (offset < 0) {
				sb.Append("-");
				offset = -offset;
			}
			var seconds = offset % TAI.SecondsPerMinute;
			var minutes = (offset / TAI.SecondsPerMinute)%TAI.MinutesPerHour;
			var hours = offset / TAI.SecondsPerHour;
			WriteTime(new PosixTz.TimeUnit(hours, minutes, seconds));
		}

		public void WriteRule(PosixTz.DstRule rule){
			rule.Match(
				fixedDateRule => {
					sb.Append("J");
					sb.Append(fixedDateRule.day);
				},
				dayOfYearRule => {
					sb.Append(dayOfYearRule.day);
				},
				dayOfWeekRule => {
					sb.Append("M");
					sb.Append(dayOfWeekRule.month);
					sb.Append(".");
					sb.Append(dayOfWeekRule.week);
					sb.Append(".");
					sb.Append(dayOfWeekRule.day);
				}
			);
			if (rule.time != PosixTz.DEFAULT_RULE_TIME) {
				sb.Append("/");
				WriteTime(rule.time);
			}
		}
		public static StringBuilder NormalizeName(string name, out bool quoted) {
			if (String.IsNullOrEmpty(name)) {
				quoted = false;
				return new StringBuilder(PosixTz.DEFAULT_STD_NAME);
			}
			var nb = new StringBuilder();
			var q = false;
			foreach (var c in name) {
				if (nb.Length >= PosixTz.TZNAME_MAX) {
					break;
				} else if (Char.IsDigit(c) || c == '+' || c == '-') {
					q = true;
				} else if (!Char.IsLetter(c)) {
					continue;
				}
				nb.Append(c);
			}
			if (nb.Length < 3) {
				quoted = false;
				return new StringBuilder(PosixTz.DEFAULT_STD_NAME);
			}
			quoted = q;
			return nb;
		}
		public static string NormalizeName(string name) {
			bool quoted;
			var nb = NormalizeName(name, out quoted);
			return nb.ToString();
		}

		public void WriteName(string name) {
			bool quoted;
			var nb = NormalizeName(name, out quoted);
			if (quoted) {
				sb.Append("<");
				sb.Append(nb);
				sb.Append(">");
			} else {
				sb.Append(nb);
			}
		}

		public void WriteTimeZone(PosixTz tz) {
			WriteName(tz.name);
			WriteTimeOffset(tz.offset);
			var dst = tz.dst;
			if (dst != null) {
				WriteName(dst.name);
				if (dst.offset != PosixTz.GetDefaultDstOffset(tz.offset)) {
					WriteTimeOffset(dst.offset);
				}
				sb.Append(",");
				WriteRule(dst.start);
				sb.Append(",");
				WriteRule(dst.end);
			}
		}

		public override string ToString() {
			return sb.ToString();
		}
	}
}
