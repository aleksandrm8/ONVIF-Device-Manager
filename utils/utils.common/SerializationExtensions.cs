using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace utils
{
    public static class SerializationExtensions
    {
        public static bool ParseBoolInvariant(this object obj) {
            bool val;
            if (!obj.TryParseInvariant(out val))
                throw new InvalidOperationException(string.Format("Failed to parse '{0}' to {1}", obj, val.GetType()));
            return val;
        }

        public static bool TryParseInvariant(this object obj, out bool value) {
            bool retval = false;
            value = false;

            if (obj is bool) {
                value = (bool)obj;
                retval = true;
            }
            else if (obj is bool?) {
                value = ((bool?)obj) == true;
                retval = true;
            }
            else if (obj is string) {
                string str = (string)obj;
                str = str.ToLower();
                if (str == "true" || str == "1") {
                    value = true;
                    retval = true;
                }
                else if (str == "false" || str == "0") {
                    value = false;
                    retval = true;
                }
            }
            return retval;
        }

        public static double ParseInvariant(this string str) {
            double val;
            if (!str.TryParseInvariant(out val))
                throw new InvalidOperationException(string.Format("Failed to parse '{0}' to {1}", str, val.GetType()));
            return val;
        }

        public static bool TryParseInvariant(this object obj, out double val)
        {
			  try {
                  if (obj is double) {
                      val = (double)obj;
                      return true;
                  }
                  else if (obj is int) {
                      val = (int)obj;
                      return true;
                  }
                  else if (obj is string) {
                      val = XmlConvert.ToDouble((string)obj);
                      return true;
                  }
                  throw new InvalidCastException();
			  } catch {
				  val = default(double);
				  return false;
			  }
            //return double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out val);
        }

    }
}
