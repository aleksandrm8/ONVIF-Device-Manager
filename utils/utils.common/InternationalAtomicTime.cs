using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public static class TAI {
		
		public static class Calendar {
			
			public struct Date {
				public int year;
				public int month;
				public int day;
			}
			
			public static readonly int[] daysToMonth365 = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
			public static readonly int[] daysToMonth366 = { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335 };
			
			public static int GetJulianDayFromJulianDate(int year, int month, int day) {
				long a = (14 - month) / 12;
				long y = year + 4800 - a;
				long m = month + 12 * a - 3;
				return (int)(day + (153 * m + 2) / 5 + 365 * y + y / 4 - 32083);
			}
			
			public static Date GetJulianDateFromJulianDay(int jdn) {
				long c = jdn + 32082;
				long d = (4 * c + 3) / 1461;
				long e = c - (1461 * d) / 4;
				long m = (5 * e + 2) / 153;
				long day = e - (153 * m + 2) / 5 + 1;
				long month = m + 3 - 12 * (m / 10);
				long year = d - 4800 + m / 10;
				return new Date {
					year = (int)year,
					month = (int)month,
					day = (int)day
				};
			}
		}

		public const int SecondsPerMinute = 60;
		public const int MinutesPerHour = 60;
		public const int HoursPerDay = 24;
		public const int SecondsPerHour = MinutesPerHour * SecondsPerMinute;
		public const int SecondsPerDay = SecondsPerHour * HoursPerDay;
	}
}
