using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	
	public partial class PosixTz {

		public abstract class DstRule : IEquatable<DstRule> {

			public readonly TimeUnit time;

			private DstRule(TimeUnit time) {
				//default time, if it is not given, shall be 02:00:00.
				this.time = time ?? DEFAULT_RULE_TIME;
			}

			public abstract void Match(
				Action<FixedDateRule> fixedDateHandler,
				Action<DayOfYearRule> dayOfYearHandler,
				Action<DayOfWeekRule> dayOfWeekHandler
			);

			public T Match<T>(
				Func<FixedDateRule, T> fixedDateHandler,
				Func<DayOfYearRule, T> dayOfYearHandler,
				Func<DayOfWeekRule, T> dayOfWeekHandler
			){
				T r = default(T);
				Match(
					x => { r = fixedDateHandler(x); },
					x => { r = dayOfYearHandler(x); },
					x => { r = dayOfWeekHandler(x); }
				);
				return r;
			}

			public string Format() {
				var writer = new PosixTzWriter();
				writer.WriteRule(this);
				return writer.ToString();
			}

			public static DstRule Parse(string str) {
				var reader = new PosixTzReader(str);
				reader.SkipWhiteSpaces();
				DstRule rule = null;
				if(!reader.ReadRule(ref rule)){
					reader.RaiseParseError();
				}
				reader.SkipWhiteSpaces();
				if(!reader.AtEnd()){
					reader.RaiseParseError();
				}
				return rule;
			}
			public virtual bool Equals(DstRule other) {
				return Equals((object)other);
			}
			public static bool operator ==(DstRule left, DstRule right) {
				if (Object.ReferenceEquals(left, right)) {
					return true;
				}
				return !Object.ReferenceEquals(left, null) && left.Equals(right);
			}
			public static bool operator !=(DstRule left, DstRule right) {
				return !(left == right);
			}
			public class FixedDateRule : DstRule, IEquatable<FixedDateRule> {
				public struct MonthAndDay {
					//zero-based month
					public int month;
					//zero-based day of month
					public int day;
				}
				//day of year in range 1..365, leap days aren't counted
				public readonly int day;
				public MonthAndDay GetDate() {
					var d = (day + 305) % 365;
					var m = (d * 5 + 2) / 153;
					return new MonthAndDay() {
						month = (m + 2) % 12,
						day = d - (153 * m + 2) / 5
					};
				}
				public FixedDateRule(int day, TimeUnit time = null): base(time) {
					this.day = day;
				}
				public FixedDateRule(int month, int dayOfMonth, TimeUnit time = null): base(time) {
					var m = (month + 10) % 12;
					this.day = ((153 * m + 2) / 5 + dayOfMonth + 59) % 365 + 1;
				}
				public override void Match(
					Action<FixedDateRule> fixedDateHandler,
					Action<DayOfYearRule> dayOfYearHandler,
					Action<DayOfWeekRule> dayOfWeekHandler
				) { fixedDateHandler(this); }

				public override bool Equals(object obj) {
					return Equals(obj as FixedDateRule);
				}
				public override bool Equals(DstRule other) {
					return Equals(other as FixedDateRule);
				}
				public bool Equals(FixedDateRule other) {
					return
						!Object.ReferenceEquals(other, null) &&
						time == other.time && day == other.day;
				}
				public override int GetHashCode() {
					return HashCode.Combine(time.GetHashCode(), day);
				}
			}

			public class DayOfYearRule : DstRule, IEquatable<DayOfYearRule> {
				public struct MonthAndDay {
					//zero-based month
					public int month;
					//zero-based day of month
					public int day;
				}
				//day of year in range 0..365, leap days are counted
				public readonly int day;
				public MonthAndDay GetDate(int year) {
					var daysPerYear = year % 4 == 0 && (year % 100 != 0 || year % 400 == 0) ? 366 : 365;//DateTime.IsLeapYear(year) ? 366 : 365;
					var d = (day + 306) % daysPerYear;
					var m = (d * 5 + 2) / 153;
					return new MonthAndDay() {
						month = (m + 2) % 12,
						day = d - (153 * m + 2) / 5
					};
				}
				public DayOfYearRule(int day, TimeUnit time = null): base(time) {
					this.day = day;
				}
				public override void Match(
					Action<FixedDateRule> fixedDateHandler,
					Action<DayOfYearRule> dayOfYearHandler,
					Action<DayOfWeekRule> dayOfWeekHandler
				) { dayOfYearHandler(this); }

				public override bool Equals(object obj) {
					return Equals(obj as DayOfYearRule);
				}
				public override bool Equals(DstRule other) {
					return Equals(other as DayOfYearRule);
				}
				public bool Equals(DayOfYearRule other) {
					return
						!Object.ReferenceEquals(other, null) &&
						time == other.time && day == other.day;
				}
				public override int GetHashCode() {
					return HashCode.Combine(
						day, time.GetHashCode()
					);
				}
			}

			public class DayOfWeekRule : DstRule, IEquatable<DayOfWeekRule> {
				public readonly int month;
				public readonly int week;
				public readonly int day;
				public DayOfWeekRule(int month, int week, int day, TimeUnit time = null): base(time) {
					this.month = month;
					this.week = week;
					this.day = day;
				}
				public override void Match(
					Action<FixedDateRule> fixedDateHandler,
					Action<DayOfYearRule> dayOfYearHandler,
					Action<DayOfWeekRule> dayOfWeekHandler
				) { dayOfWeekHandler(this); }

				public override bool Equals(object obj) {
					return Equals(obj as DayOfWeekRule);
				}
				public override bool Equals(DstRule other) {
					return Equals(other as DayOfWeekRule);
				}
				public bool Equals(DayOfWeekRule other) {
					return 
						!Object.ReferenceEquals(other, null) &&
						time == other.time &&
						month == other.month &&
						week == other.week &&
						day == other.day;
				}
				public override int GetHashCode() {
					return HashCode.Combine(
						time.GetHashCode(),
						month, week, day
					);
				}

			}
			
		}
		
	}
}
