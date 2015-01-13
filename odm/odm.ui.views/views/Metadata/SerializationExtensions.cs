using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using utils;

namespace odm.ui.views
{
    public static class SerializationExtensionsEx
    {
        
        public static bool TryParseInvariant(this string str, out System.Windows.Rect rect)
        {
            try 
            {
                str = str.Replace("(", "").Replace(")", "").Trim();
                string[] parts = str.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 4)
                {
                    double x1, x2, y1, y2;
                    if (true
                        && parts[0].TryParseInvariant(out x1)
                        && parts[1].TryParseInvariant(out y1)
                        && parts[2].TryParseInvariant(out x2)
                        && parts[3].TryParseInvariant(out y2))
                    {
                        rect = new System.Windows.Rect(new System.Windows.Point(x1, y1), new System.Windows.Point(x2, y2));
                        return true;
                    }
                }
            }
            catch (Exception)
            {}

            rect = System.Windows.Rect.Empty;
            return false;
        }

        public static bool TryParseInvariant(this string str, out System.Windows.Point point)
        {
            try
            {
                str = str.Replace("(", "").Replace(")", "").Trim();
                string[] parts = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    double x, y;
                    if (true
                        && parts[0].TryParseInvariant(out x)
                        && parts[1].TryParseInvariant(out y))
                    {
                        point = new System.Windows.Point(x, y);
                        return true;
                    }
                }
            }
            catch (Exception)
            { }

            point = new System.Windows.Point();
            return false;
        }
    }
}
