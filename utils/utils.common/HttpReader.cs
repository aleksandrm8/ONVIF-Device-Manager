using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public class HttpReader {
		string str;
		int pos = 0;

		public HttpReader(string str) {
			this.str = str ?? String.Empty;
		}

		public bool AtEnd() {
			return pos >= str.Length;
		}

		public void SkipWhiteSpaces() {
			while (pos < str.Length && Char.IsWhiteSpace(str[pos])) {
				++pos;
			}
		}

		public bool ReadSpecChar(char c) {
			if (AtEnd() || c != str[pos]) {
				return false;
			}
			++pos;
			return true;
		}

		public bool ReadSpecChar(char c1, char c2) {
			return ReadSpecChar(c1) || ReadSpecChar(c2);
		}

		public bool ReadSpecString(string token) {
			var origin_pos = pos;
			var i = 0;
			var len = token != null ? token.Length : 0;
			while (i < len) {
				if (AtEnd()) {
					pos = origin_pos;
					return false;
				}
				if (token[i] != str[pos]) {
					pos = origin_pos;
					return false;
				}
				++pos;
			}
			return true;
		}

		public bool ReadSpecString(string token, bool ignoreCase) {
			if (!ignoreCase) {
				return ReadSpecString(token);
			}
			var origin_pos = pos;
			var i = 0;
			var len = token != null ? token.Length : 0;
			while (i < len) {
				if (AtEnd()) {
					pos = origin_pos;
					return false;
				}
				if (Char.ToLower(token[i]) != Char.ToLower(str[pos])) {
					pos = origin_pos;
					return false;
				}
				++pos;
			}
			return true;
		}

		public char[] ts_specials = new char[]{
			'(', ')', '<', '>', '@',',', ';', ':', '\\', '"', '/', '[', ']', '?', '=', '{', '}', ' ', '\t'
		};
		
		public bool IsLinearWhiteSpace(){
			return IsEndOfLine() && (str[pos + 2] == ' ' || str[pos + 2] == '\t');
		}

		//
		public bool IsEndOfLine() {
			return str[pos] == '\r' && str[pos + 1] == '\n';
		}

		public bool IsSpecialChar() {
			return ts_specials.Contains(str[pos]);
		}

		public bool IsControlChar() {
			return Char.IsControl(str, pos);
		}

		public bool ReadQuotedString(ref string value) {
			var origin_pos = pos;
			if (!ReadSpecChar('"')) {
				return false;
			}
			int start_pos = pos;
			while (true) {
				if (AtEnd()) {
					pos = origin_pos;
					return false;
				}
				var c = str[pos];
				if (c == '"') {
					value = str.Substring(start_pos, pos - start_pos);
					++pos;
					return true;
				}
				if (Char.IsControl(str, pos) || IsSpecialChar()) {
					pos = origin_pos;
					return false;
				}
				++pos;
			}
		}

		public bool ReadToken(ref string value) {
			int start_pos = pos;
			//1*<any CHAR except CTLs or tspecials>
			while (!AtEnd() && !IsControlChar() && !IsSpecialChar()) {
				++pos;
			};
			if (pos != start_pos) {
				value = str.Substring(start_pos, pos - start_pos);
				return true;
			}
			return false;
		}

	}
}
