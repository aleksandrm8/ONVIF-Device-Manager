using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace utils {
	public class ColorizedConsoleTraceListener : ConsoleTraceListener {
		ConsoleColor SwitchConsoleColor(TraceEventType eventType) {
			var originColor = Console.ForegroundColor;
			if ((eventType & (TraceEventType.Critical | TraceEventType.Error)) != 0) {
				Console.ForegroundColor = ConsoleColor.Red;
				//Console.Beep();
			} else if ((eventType & (TraceEventType.Warning)) != 0) {
				Console.ForegroundColor = ConsoleColor.Yellow;
			} else if ((eventType & (TraceEventType.Information)) != 0) {
				Console.ForegroundColor = ConsoleColor.Gray;
			}
			return originColor;
		}
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data) {
			var originColor = SwitchConsoleColor(eventType);
			base.TraceData(eventCache, source, eventType, id, data);
			Flush();
			Console.ForegroundColor = originColor;
		}
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data) {
			var originColor = SwitchConsoleColor(eventType);
			base.TraceData(eventCache, source, eventType, id, data);
			Flush();
			Console.ForegroundColor = originColor;
		}
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id) {
			var originColor = SwitchConsoleColor(eventType);
			base.TraceEvent(eventCache, source, eventType, id);
			Flush();
			Console.ForegroundColor = originColor;
		}
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args) {
			var originColor = SwitchConsoleColor(eventType);
			base.TraceEvent(eventCache, source, eventType, id, format, args);
			Flush();
			Console.ForegroundColor = originColor;
		}
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message) {
			var originColor = SwitchConsoleColor(eventType);
			base.TraceEvent(eventCache, source, eventType, id, message);
			Flush();
			Console.ForegroundColor = originColor;
		}
	}
}
