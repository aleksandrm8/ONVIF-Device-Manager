using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public static class PosixTzExtensions {
		public static DateTime ToSystemDateTime(this PosixTz.TimeUnit time) {
			return new DateTime(1, 1, 1, time.hours, time.minutes, time.seconds);
		}
		public static TimeZoneInfo.TransitionTime ToSystemTransitionTime(this PosixTz.DstRule rule, int year) {
			return rule.Match<TimeZoneInfo.TransitionTime>(
				fixedDate => {
					var date = fixedDate.GetDate();
					return TimeZoneInfo.TransitionTime.CreateFixedDateRule(
						fixedDate.time.ToSystemDateTime(),
						date.month + 1, date.day + 1
					);
				},
				dayOfYear => {
					var date = dayOfYear.GetDate(year);
					return TimeZoneInfo.TransitionTime.CreateFixedDateRule(
						dayOfYear.time.ToSystemDateTime(),
						date.month + 1, date.day + 1
					);
				},
				dayOfWeek => TimeZoneInfo.TransitionTime.CreateFloatingDateRule(
					dayOfWeek.time.ToSystemDateTime(),
					dayOfWeek.month, dayOfWeek.week, (DayOfWeek)dayOfWeek.day
				)
			);
		}
		public static PosixTz.DstRule GetPosixRuleFromTransitionTime(TimeZoneInfo.TransitionTime trasitionTime) {
			var time = new PosixTz.TimeUnit(
				trasitionTime.TimeOfDay.Hour,
				trasitionTime.TimeOfDay.Minute,
				trasitionTime.TimeOfDay.Second
			);
			if (trasitionTime.IsFixedDateRule) {
				return new PosixTz.DstRule.FixedDateRule(
					trasitionTime.Month - 1,
					trasitionTime.Day - 1,
					time
				);
			}
			return new PosixTz.DstRule.DayOfWeekRule(
				trasitionTime.Month,
				trasitionTime.Week,
				(int)trasitionTime.DayOfWeek,
				time
			);
		}

		public static TimeZoneInfo ToSystemTimeZone(this PosixTz posixTz, int year, bool isDaylightSavingTime = true) {
			if ((object)posixTz == null) {
				return null;
			}
			var id = posixTz.name;
			if (id.Length > 32) {
				id = id.Substring(0, 32);
			}
			var baseOffset = TimeSpan.FromSeconds(-posixTz.offset);
			if ((object)posixTz.dst == null || !isDaylightSavingTime) {
				return TimeZoneInfo.CreateCustomTimeZone(
					id, baseOffset, posixTz.Format(), posixTz.name
				);
			}
			var daylightDelta = TimeSpan.FromSeconds(
				posixTz.offset - posixTz.dst.offset
			);
			var start = posixTz.dst.start.ToSystemTransitionTime(year);
			var end = posixTz.dst.end.ToSystemTransitionTime(year);

			var adjustmentRule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(
				new DateTime(year, 1, 1), new DateTime(year, 12, 31), daylightDelta, start, end
			);
			return TimeZoneInfo.CreateCustomTimeZone(
				id, baseOffset, posixTz.Format(), posixTz.name, posixTz.dst.name, new[] { adjustmentRule }
			);
		}

		public static DateTime ConvertUtcTimeToLocal(this PosixTz posixTz, DateTime time, bool isDaylightSavingTime = true) {
			if ((object)posixTz == null) {
				throw new ArgumentNullException("posixTz");
			}
			var tz = posixTz.ToSystemTimeZone(time.Year, isDaylightSavingTime);
			return TimeZoneInfo.ConvertTimeFromUtc(time, tz);
		}

		public static DateTime ConvertLocalTimeToUtc(this PosixTz posixTz, DateTime time, bool isDaylightSavingTime = true) {
			if ((object)posixTz == null) {
				throw new ArgumentNullException("posixTz");
			}
			var tz = posixTz.ToSystemTimeZone(time.Year, isDaylightSavingTime);
			return TimeZoneInfo.ConvertTimeToUtc(time, tz);
		}

	}
}
