using System;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace utils {
	
	public class dbg{

		//--------------------------------------------------
		// public section
		//--------------------------------------------------

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Warning(string errMsg) {
			_WarningInternal(2, errMsg, null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Warning(string errMsg, string errSrc) {
			_WarningInternal(2, errMsg, errSrc);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Error(string errMsg) {
			_ErrorInternal(2, errMsg, null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Error(string errMsg, string errSrc) {
			_ErrorInternal(2, errMsg, errSrc);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Error(Exception exception) {
			_ErrorInternal(2, exception.Message, exception.Source);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Error(Exception exception, string errSrc) {
			_ErrorInternal(2, exception.Message, errSrc);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Assert(bool expr, string errMsg, string category) {
			_AssertInternal(2, expr, errMsg, category);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Assert(bool expr, string errMsg) {
			_AssertInternal(2, expr, errMsg, null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Assert(bool expr) {
			_AssertInternal(2, expr, null, null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Info(string errMsg, string errSrc) {
			_InfoInternal(2, errMsg, errSrc);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Info(string errMsg) {
			_InfoInternal(2, errMsg, null);
		}

		public class AssertionFailureException : Exception {
		}

		[DebuggerHidden]
		[Conditional("DEBUG")]
		public static void Break() {
			if (Debugger.IsAttached) {
				Debugger.Break();
			}
		}

		[DebuggerHidden]
		[Conditional("DEBUG")]
		public static void BreakIf(bool Condition) {
			if (Debugger.IsAttached && Condition) {
				Debugger.Break();
			}
		}

		//--------------------------------------------------
		// protected section
		//--------------------------------------------------
		protected static string s_processFileNameField = null;
		protected static string s_processFileName {
			get{
				if (s_processFileNameField == null) {
					s_processFileNameField = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
				}
				return s_processFileNameField;
			}
		}
			
		protected static string _ResolveSourceIfNone(StackFrame sf, string src) {
			if (src == null) {
				//return sf.GetMethod().DeclaringType.Namespace;
				//return sf.GetMethod().DeclaringType.FullName;
				//return Process.GetCurrentProcess().ProcessName;
				return s_processFileName;
			}
			return src;
		}

		[DebuggerHidden]
		protected static void _AssertInternal(int framesToSkip, bool expr, string errMsg, string src) {
			if (expr) {
				return;
			}

			StackTrace stack = new StackTrace(framesToSkip, true);
			var evtSb = new StringBuilder();
			evtSb.Append("ASSERT FAILED:");
			if (!String.IsNullOrEmpty(errMsg)) {
				evtSb.Append(" \"");
				evtSb.Append(errMsg);
				evtSb.Append("\"");
			}
			evtSb.Append(" at ");
			var sf = stack.GetFrame(0);
			AppendStackFrame(evtSb, sf);
			src = _ResolveSourceIfNone(sf, src);
			log.WriteEvent(evtSb.ToString(), src, TraceEventType.Critical);
			log.Flush();
			Break();
		}

		[DebuggerHidden]
		protected static void _ErrorInternal(int framesToSkip, string errMsg, string src) {

			StackTrace stack = new StackTrace(framesToSkip, true);
			var evtSb = new StringBuilder();
			if (!String.IsNullOrEmpty(errMsg)) {
				evtSb.Append(" \"");
				evtSb.Append(errMsg);
				evtSb.Append("\"");
			}
			evtSb.Append(" at ");
			var sf = stack.GetFrame(0);
			AppendStackFrame(evtSb, sf);
			src = _ResolveSourceIfNone(sf, src);
			log.WriteEvent(evtSb.ToString(), src, TraceEventType.Error);
			log.Flush();
			Break();
		}

		protected static void _WarningInternal(int framesToSkip, string errMsg, string src) {

			StackTrace stack = new StackTrace(framesToSkip, true);
			var evtSb = new StringBuilder();
			//if (!String.IsNullOrEmpty(errMsg)) {
			//    evtSb.Append(" \"");
			evtSb.Append(errMsg);
			//    evtSb.Append("\"");
			//}
			//evtSb.Append(" at ");
			var sf = stack.GetFrame(0);
			//AppendStackFrame(evtSb, sf);
			src = _ResolveSourceIfNone(sf, src);
			log.WriteEvent(evtSb.ToString(), src, TraceEventType.Warning);
			log.Flush();
			//Break();
		}

		protected static void _InfoInternal(int framesToSkip, string errMsg, string src) {

			StackTrace stack = new StackTrace(framesToSkip, true);
			var evtSb = new StringBuilder();
			//if (!String.IsNullOrEmpty(errMsg)) {
			//    evtSb.Append(" \"");
			    evtSb.Append(errMsg);
			//    evtSb.Append("\"");
			//}
			//evtSb.Append(" at ");
			var sf = stack.GetFrame(0);
			//AppendStackFrame(evtSb, sf);
			src = _ResolveSourceIfNone(sf, src);
			log.WriteEvent(evtSb.ToString(), src, TraceEventType.Information);
			log.Flush();
			//Break();
		}

		private static void AppendStackFrame(StringBuilder sb, StackFrame sf) {
			sb.Append(sf.GetMethod());
			var fileName = sf.GetFileName();
			if (!String.IsNullOrEmpty(fileName)) {
				sb.Append(" in ");
				sb.Append(sf.GetFileName());
				var lineNumber = sf.GetFileLineNumber();
				if (lineNumber > 0) {
					sb.Append(": line ");
					sb.Append(lineNumber);
				}
			}
		}

	}
}
