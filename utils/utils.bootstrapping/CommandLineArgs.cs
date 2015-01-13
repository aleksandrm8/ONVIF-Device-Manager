using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace utils {

	public class CommandLineArgs : Dictionary<String, List<String>> {

		public int GetParamAsInt(string paramName) {
			List<string> val = null;
			if (!TryGetValue(paramName, out val) || val == null || val.Count == 0) {
				throw new Exception(String.Format("parameter {0} is not specified", paramName));
			}
			if (val.Count > 1) {
				throw new Exception(String.Format("parameter {0} is specified more than one time", paramName));
			}
			try {
				return int.Parse(val.First());
			} catch {
				throw new Exception(String.Format("parameter {0} is not valid", paramName));
			}
		}

		public string GetParamAsString(string paramName) {
			List<string> val = null;
			if (!TryGetValue(paramName, out val) || val == null || val.Count == 0) {
				throw new Exception(String.Format("parameter {0} is not specified", paramName));
			}
			if (val.Count > 1) {
				throw new Exception(String.Format("parameter {0} is specified more than once", paramName));
			}
			return val.First();
		}

		public String[] Format() {
			var args = new List<String>();
			foreach (var arg in this) {
				if (arg.Value == null || arg.Value.Count == 0) {
					args.Add(arg.Key);
				} else {
					foreach (var val in arg.Value) {
						args.Add(String.Format("/{0}:{1}",arg.Key, Uri.EscapeDataString(val)));
					}
				}
			}
			return args.ToArray();
		}

		public static CommandLineArgs Parse(String[] args) {
			var commandLineArgs = new CommandLineArgs();
			if (args.Length == 0) {
				return commandLineArgs;
			}

			String pattern = @"^/(?<argname>[A-Za-z0-9_\-.%]+):(?<argvalue>.+)$";
			foreach (string x in args) {
				Match match = Regex.Match(x, pattern);

				if (!match.Success) {
					throw new Exception("failed to parse command line");
				}
				String argname = match.Groups["argname"].Value.ToLower();
				List<String> values = null;
				if (!commandLineArgs.TryGetValue(argname, out values)) {
					values = new List<String>();
					commandLineArgs.Add(argname, values);
				}
				//var s = match.Groups["argvalue"].Value;
				values.Add(Uri.UnescapeDataString(match.Groups["argvalue"].Value));
			}
			return commandLineArgs;
		}
	}
}
