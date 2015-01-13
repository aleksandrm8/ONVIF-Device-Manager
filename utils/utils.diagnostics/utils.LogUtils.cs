using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;


namespace utils {

	public interface ILogUtils {
		TraceListenerCollection listeners {get;}
		void WriteEvent(string evtMsg, string evtSrc, TraceEventType evtType);
		void Flush();
		void Close();
	}

	public class LogUtilsBase : ILogUtils {

		//--------------------------------------------------
		// public section
		//--------------------------------------------------

		public virtual Guid activityId {
			get {
				return Guid.Empty;
			}
		}

		//--------------------------------------------------
		// ILogUtils implementation
		//--------------------------------------------------

		public TraceListenerCollection listeners {
			get {
				return Trace.Listeners;
			}
		}
		
		public void WriteEvent(string message, string source, TraceEventType type) {
			var ctx = BeforeWriteEvent();
			try {
				if (source == null) {
					source = s_processFileName;
				}
				var evtId = Interlocked.Increment(ref _evtId);
				var evtCache = new TraceEventCache();
				foreach (TraceListener l in listeners) {
					lock (l) {
						try {
							l.TraceEvent(evtCache, source, type, evtId, message);
							if (Trace.AutoFlush) {
								l.Flush();
							}
						} catch {
							//swallow error
							Debugger.Break();
						}
					}
				}
			} finally {
				AfterWriteEvent(ctx);
			}
		}

		public void Flush() {
			foreach (TraceListener l in listeners) {
				lock (l) {
					l.Flush();
				}
			}
		}
		public void Close() {
			foreach (TraceListener l in listeners) {
				lock (l) {
					l.Close();
				}
			}
		}
		
		//--------------------------------------------------
		// protected section
		//--------------------------------------------------

		protected static string s_processFileNameField = null;
		protected static string s_processFileName {
			get {
				if (s_processFileNameField == null) {
					s_processFileNameField = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
				}
				return s_processFileNameField;
			}
		}

		protected virtual Object BeforeWriteEvent() {
			var oldActivityId = Trace.CorrelationManager.ActivityId;
			if (oldActivityId != activityId) {
				Trace.CorrelationManager.ActivityId = activityId;
			}
			return oldActivityId;
		}

		protected virtual void AfterWriteEvent(Object context) {
			var oldActivityId = (Guid)context;
			if (oldActivityId != activityId) {
				Trace.CorrelationManager.ActivityId = activityId;
			}
		}

		//--------------------------------------------------
		// private section
		//--------------------------------------------------

		private int _evtId = 0;
	}


	public class LogUtilsDefault : LogUtilsBase {

		//--------------------------------------------------
		// public section
		//--------------------------------------------------
		
		public LogUtilsDefault() {
			_activityId = Guid.NewGuid();
		}

		public LogUtilsDefault(Guid activityId) {
			this._activityId = activityId;
		}

		//--------------------------------------------------
		// protected section
		//--------------------------------------------------

		protected Guid _activityId;
	}

	public class log {

		//--------------------------------------------------
		// public section
		//--------------------------------------------------
		
		static log() {
			Init(new LogUtilsDefault());
		}

		[Conditional("TRACE")]
		public static void Init(ILogUtils impl) {
			//lock (_staticLock) {
			log._impl = impl;
			//}
		}

		[Conditional("TRACE")]
		public static void WriteEvent(string evtMsg, string evtSrc, TraceEventType evtType) {
			_impl.WriteEvent(evtMsg, evtSrc, evtType);
		}

		[Conditional("TRACE")]
		public static void WriteError(string errMsg, string source) {
			WriteEvent(errMsg, source, TraceEventType.Error);
		}

		[Conditional("TRACE")]
		public static void WriteError(string errMsg) {
			WriteEvent(errMsg, null, TraceEventType.Error);
		}

		[Conditional("TRACE")]
		public static void WriteError(Exception err) {

            WriteEvent(GetInner(err), null, TraceEventType.Error);
		}
        static string GetInner(Exception err) {
            string result = err.ToString();
            if (err.InnerException != null) {
                result += Environment.NewLine + GetInner(err.InnerException);
            }
            return result;
        }
		
		[Conditional("TRACE")]
		public static void WriteWarning(string warnMsg, string source) {
			WriteEvent(warnMsg, source, TraceEventType.Warning);
		}

		[Conditional("TRACE")]
		public static void WriteWarning(string warnMsg) {
			WriteEvent(warnMsg, null, TraceEventType.Warning);
		}

		[Conditional("TRACE")]
		public static void WriteInfo(string infoMsg, string source) {
			WriteEvent(infoMsg, source, TraceEventType.Information);
		}

		[Conditional("TRACE")]
		public static void WriteInfo(string infoMsg) {
			WriteEvent(infoMsg, null, TraceEventType.Information);
		}

		[Conditional("TRACE")]
		public static void Flush() {
			_impl.Flush();
		}

		[Conditional("TRACE")]
		public static void Close() {
			_impl.Close();
		}

		//--------------------------------------------------
		// private section
		//--------------------------------------------------

		//private static object _staticLock = new Object();
		private static ILogUtils _impl;

	}
}
