using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {

	public class PosixTzReader {
		public class ParseException : Exception {
			ParseException(string message)
				: base(message) {
			}
			public static ParseException Create(string parsedString, int errorPosition) {
				return new ParseException(String.Format(
					"error of parsing '{0}' at position {1}",
					parsedString, errorPosition
				));
			}
		}

		string str;
		int pos = 0;
		bool whiteSpacesNotAllowed = false;

		public PosixTzReader(string str) {
			if (str == null) {
				throw new ArgumentException("str");
			}
			this.str = str;
		}

		public void RaiseParseError() {
			throw ParseException.Create(str, pos);
		}

		public bool AtEnd() {
			return pos >= str.Length;
		}

		public void SkipWhiteSpaces() {
			if (whiteSpacesNotAllowed) {
				return;
			}
			while (pos < str.Length && Char.IsWhiteSpace(str[pos])) {
				++pos;
			}
		}

		//public T Eval<T>(Func<T> func) {
		//    return func();
		//}

		public bool ReadDigit(ref int dig) {
			if (AtEnd()) {
				return false;
			}
			var c = str[pos];
			if (c < '0' || c > '9') {
				return false;
			}
			++pos;
			dig = c - '0';
			return true;
		}

		public bool ReadNum(ref int num) {
			var dig = 0;
			var saved_pos = pos;
			var res = 0;
			while (ReadDigit(ref dig)) {
				checked {
					res = res * 10 + dig;
				}
			}
			if (saved_pos == pos) {
				return false;
			}
			num = res;
			return true;
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

		public bool ReadQuotedName(ref string name) {
			if (!ReadSpecChar('<')) {
				return false;
			}
			int name_pos = pos;
			while (true) {
				if (AtEnd()) {
					//quated name should end with '>'
					RaiseParseError();
				}
				var c = str[pos];
				++pos;
				if (c == '>') {
					break;
				}
				if (!Char.IsLetterOrDigit(c) && c != '+' && c != '-') {
					//All characters between these quoting characters shall be 
					//alphanumeric characters from the portable character set 
					//in the current locale, the <plus-sign> ( '+' ) character, 
					//or the minus-sign ( '-' ) character
					RaiseParseError();
				}
			}
			name = str.Substring(name_pos, pos - name_pos - 1);
			return true;
		}

		public bool ReadUnquotedName(ref string name) {
			int name_pos = pos;
			//all characters shall be alphabetic characters from the portable 
			//character set in the current locale.
			while (!AtEnd() && Char.IsLetter(str, pos)) {
				++pos;
			};
			if (pos != name_pos) {
				name = str.Substring(name_pos, pos - name_pos);
				return true;
			}
			return false;
		}
		public bool ReadName(ref string name) {
			return ReadQuotedName(ref name) || ReadUnquotedName(ref name);
		}

		//hh[:mm[:ss]]
		public PosixTz.TimeUnit ReadTime() {
			int saved_pos;

			//parse hour part
			var hours = 0;
			if (!ReadNum(ref hours)) {
				//no hours present
				return null;
			}

			if (hours > 24) {
				//the hour shall be between zero and 24
				RaiseParseError();
			}

			//parse minute part
			var minutes = 0;
			saved_pos = pos;
			SkipWhiteSpaces();
			if (!ReadSpecChar(':')) {
				//no minutes present
				pos = saved_pos;
				return new PosixTz.TimeUnit(hours, 0, 0);
			}
			SkipWhiteSpaces();
			if (!ReadNum(ref minutes) || minutes > 59) {
				//if present shall be between zero and 59
				RaiseParseError();
			}

			//parse second part
			var seconds = 0;
			saved_pos = pos;
			SkipWhiteSpaces();
			if (!ReadSpecChar(':')) {
				//no seconds present
				pos = saved_pos;
				return new PosixTz.TimeUnit(hours, minutes, 0);
			}
			SkipWhiteSpaces();
			if (!ReadNum(ref seconds) || seconds > 59) {
				//if present shall be between zero and 59
				RaiseParseError();
			}
			return new PosixTz.TimeUnit(hours, minutes, seconds);
		}

		//[+/-]time 
		//Indicates the value added to the local time to arrive at Coordinated Universal Time
		public bool ReadTimeOffset(ref int offset) {
			int saved_pos = pos;
			var is_negative = false;

			//If preceded by a '-' , the timezone shall be east of the Prime Meridian; 
			//otherwise, it shall be west (which may be indicated by an optional 
			//preceding '+' )
			if (ReadSpecChar('-')) {
				is_negative = true;
			} else {
				ReadSpecChar('+');
			}
			SkipWhiteSpaces();
			var time = ReadTime();
			if (time == null) {
				pos = saved_pos;
				return false;
			}
			offset = time.ToOffset(is_negative);
			return true;
		}

		public PosixTz.TimeUnit ReadRuleTime() {
			int saved_pos = pos;
			SkipWhiteSpaces();
			if (!ReadSpecChar('/')) {
				pos = saved_pos;
				return null;
			}
			SkipWhiteSpaces();
			var time = ReadTime();
			if (time != null) {
				return time;
			}
			pos = saved_pos;
			return null;
		}

		//Jn (the Julian day n)
		public bool ReadFixedDateRule(ref PosixTz.DstRule rule) {
			int saved_pos = pos;
			if (!ReadSpecChar('J')) {
				return false;
			}
			SkipWhiteSpaces();
			int day = 0;
			if (!ReadNum(ref day) || day < 1 || day > 365) {
				//1 <= n <= 365
				RaiseParseError();
			};
			var time = ReadRuleTime();
			rule = new PosixTz.DstRule.FixedDateRule(day, time);
			return true;
		}

		//n (the zero-based Julian day)
		public bool ReadDayOfYearRule(ref PosixTz.DstRule rule) {
			int day = 0;
			if (!ReadNum(ref day)) {
				return false;
			}
			if (day > 365) {
				//0 <= n <= 365
				RaiseParseError();
			};
			var time = ReadRuleTime();
			rule = new PosixTz.DstRule.DayOfYearRule(day, time);
			return true;
		}

		//Mm.n.d (the d'th day of week n of month m of the year)
		public bool ReadDayOfWeekRule(ref PosixTz.DstRule rule) {
			int saved_pos = pos;
			if (!ReadSpecChar('M')) {
				return false;
			}
			SkipWhiteSpaces();
			int month = 0;
			if (!ReadNum(ref month) || month < 1 || month > 12) {
				RaiseParseError();
			};

			SkipWhiteSpaces();
			if (!ReadSpecChar('.')) {
				//week is missing
				RaiseParseError();
			}

			SkipWhiteSpaces();
			int week = 0;
			if (!ReadNum(ref week) || week < 1 || week > 5) {
				//1 <= n <= 5
				RaiseParseError();
			};

			SkipWhiteSpaces();
			if (!ReadSpecChar('.')) {
				//day is missing
				RaiseParseError();
			}

			SkipWhiteSpaces();
			int day = 0;
			if (!ReadNum(ref day) || day > 6) {
				//0 <= d <= 6
				RaiseParseError();
			};
			var time = ReadRuleTime();
			rule = new PosixTz.DstRule.DayOfWeekRule(month, week, day, time);
			return true;
		}

		public bool ReadRule(ref PosixTz.DstRule rule) {
			return ReadFixedDateRule(ref rule) || ReadDayOfYearRule(ref rule) || ReadDayOfWeekRule(ref rule);
		}

		//std offset [dst[offset],start[/time],end[/time]]
		public PosixTz ReadExpandedTimeZone() {
			SkipWhiteSpaces();
			string std_name = null;
			if (!ReadName(ref std_name) || std_name.Length < 3) {
				//std is required and should be no less than three characters
				RaiseParseError();
			}

			SkipWhiteSpaces();
			int std_ofs = 0;
			if (!ReadTimeOffset(ref std_ofs)) {
				//The offset following std shall be required. 
				RaiseParseError();
			}

			SkipWhiteSpaces();
			if (AtEnd()) {
				//if dst is missing, then the alternative time does not apply in this locale. 
				return new PosixTz(std_name, std_ofs, null);
			}

			//parse dst clause
			string dst_name = null;
			if (!ReadName(ref dst_name)) {
				//dst clause should begin with dst name which should be no less than three characters
				RaiseParseError();
			}

			SkipWhiteSpaces();
			int dst_offset = 0;
			if (!ReadTimeOffset(ref dst_offset)) {
				//if no offset follows dst, the alternative time is assumed to be one hour ahead 
				//of standard time.
				dst_offset = PosixTz.GetDefaultDstOffset(std_ofs);
			}

			SkipWhiteSpaces();
			if (!ReadSpecChar(',', ';')) {
				//no rules is specified
				RaiseParseError();
			}

			SkipWhiteSpaces();
			PosixTz.DstRule start = null;
			if (!ReadRule(ref start)) {
				//start rule is not specified
				RaiseParseError();
			}

			SkipWhiteSpaces();
			if (!ReadSpecChar(',', ';')) {
				//end rule is not specified
				RaiseParseError();
			}

			SkipWhiteSpaces();
			PosixTz.DstRule end = null;
			if (!ReadRule(ref end)) {
				//end rule is not specified
				RaiseParseError();
			}

			var dst = new PosixTz.Dst(dst_name, dst_offset, start, end);

			SkipWhiteSpaces();
			if (!AtEnd()) {
				//unexpected 
				RaiseParseError();
			}

			return new PosixTz(std_name, std_ofs, dst);
		}

	}

}
