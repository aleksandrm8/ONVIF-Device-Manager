using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;

namespace utils {
	public static class ObjectExtensions {
		public static object Convert(this object val, Type type) {
			dbg.Assert(type != null);

			Type valType = val.GetType();
			if (valType == type) {
				return val;
			}

			TypeConverter conv = TypeDescriptor.GetConverter(type);
			if (conv.CanConvertFrom(valType)) {
				//return conv.ConvertFrom(val);
				return conv.ConvertFrom(null, CultureInfo.InvariantCulture, val);
			}

			conv = TypeDescriptor.GetConverter(valType);

			if (!conv.CanConvertTo(type)) {
				throw new InvalidOperationException("failed to convert");
			}

			//return conv.ConvertTo(val, type);
			return conv.ConvertTo(null, CultureInfo.InvariantCulture, val, type);
		}

		public static TType Convert<TType>(this object val) {
			if (val == null) {
				return default(TType);
			}
			return (TType)val.Convert(typeof(TType));
		}

		public static string ToHtmlText(this object val) {
			if (val == null) {
				return null;
			}
			return HttpUtility.HtmlEncode(val.ToString());
		}

		public static string ToJson(this object val) {
			var ser = new JavaScriptSerializer();
			return ser.Serialize(val);
		}

		public static void ToJson(this object val, StringBuilder builder) {
			var ser = new JavaScriptSerializer();
			ser.Serialize(val, builder);
		}

		public static T FromJson<T>(this string input) {
			var ser = new JavaScriptSerializer();
			return ser.Deserialize<T>(input);
		}
	}
}
