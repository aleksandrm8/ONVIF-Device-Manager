using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public static class EnumHelper {
		public static T[] GetValues<T>() where T : struct {
			return (T[])Enum.GetValues(typeof(T));
		}
		public static bool IsOneOf<T>(this T comp, params T[] values) where T : struct {
			return values.Contains(comp);
		}
		public static T Parse<T>(string value) where T : struct {
			return (T)Enum.Parse(typeof(T), value);
		}
	}
}
