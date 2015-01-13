using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils
{
    public static class StringExtensions
    {
        public static string Replace(this string str, IDictionary<string, string> values)
        {
            foreach (var kv in values)
                str = str.Replace(kv.Key, kv.Value);
            return str;
        }
        /// <summary>
        /// Check if string contains valid IPV4 ip adderss
        /// like 192.168.0.1
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIPV4Valid(this string str) {
            var ipNodes = str.Split('.');
            if (ipNodes.Length != 4)
                return false;
            for (int i = 0; i < ipNodes.Length; i++) {
                int val;
                if (Int32.TryParse(ipNodes[i], out val)) {
                    if (val > 255 || val < 0)
                        return false;
                } else return false;
            }
            return true;
        }
        /// <summary>
        /// Check if string contains valid list of ip adresses.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sep">Separator for ip adresses. by default is ';'</param>
        /// <returns></returns>
        public static bool IsIPV4ListValid(this string str, char sep = ';') {
            var ret = true;
            var nodes = str.Split(sep);
            nodes.ForEach(node => {
                if (!IsIPV4Valid(node))
                    ret = false;
            });
            return ret;
        }

        /// <summary>
        /// Check if port number is valid. [0;65565]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPortNumberValid(this string str) {
            int val;
            if (Int32.TryParse(str, out val)) {
                if (val < 0 || val > 65335)
                    return false;
            } else return false;

            return true;
        }
        /// <summary>
        /// Check if string contains valid list of port numbers.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sep">Separator for port numbers. by default is ';'</param>
        /// <returns></returns>
        public static bool IsPortNumbersListValid(this string str, char sep = ';') {
            var ret = true;
            var nodes = str.Split(sep);
            nodes.ForEach(node => {
                if (!IsPortNumberValid(node))
                    ret = false;
            });
            return ret;
        }
        public static int[] ToIntArray(this string str, char sep = ';') {
            List<int> returns = new List<int>();
            var nodes = str.Split(sep);
            nodes.ForEach(node => {
                int val;
                if (int.TryParse(node, out val))
                    returns.Add(val);
            });
            return returns.ToArray(); ;
        }
        /// <summary>
        /// Combines several strings in one with separator.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="sep">separator value/ by default = ';'</param>
        /// <returns></returns>
        public static string Combine(this string[] arr, char sep = ';') {
            var val = "";
            arr.ForEach(ip => {
                val += (val == "" ? "" : sep + " ") + ip;
            });
            return val;
        }
    }
}
