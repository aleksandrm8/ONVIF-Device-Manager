using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Subjects;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;


namespace utils {
	public class XmlSerializerNamespaceResolver : IXmlNamespaceResolver {
		Dictionary<string, string> m_namespaces = new Dictionary<string,string>();
		
		public XmlSerializerNamespaceResolver(XmlSerializerNamespaces serializerNamespaces) {
			serializerNamespaces.ToArray().ForEach(qns => {
				if (!qns.IsEmpty) {
					m_namespaces.Add(qns.Name, qns.Namespace);
				}
			});
		}

		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope) {
			return m_namespaces;
		}

		public string LookupNamespace(string prefix) {
			string ns;
			m_namespaces.TryGetValue(prefix, out ns);
			return ns;
		}

		public string LookupPrefix(string namespaceName) {
			return m_namespaces
				.Where(x => x.Value == namespaceName)
				.Select(x => x.Key)
				.FirstOrDefault();
		}
	}

	[Serializable]
	[XmlRoot("log-message")]
	public class LogMessage {

		[XmlElement("id")]
		public int id;

		[XmlElement("source")]
		public string source;

		[XmlElement("message")]
		public string message;

		[XmlElement("event-type")]
		public TraceEventType eventType;

		[XmlElement("date-time")]
		public DateTime dateTime;
		
		[XmlIgnore]
		public TraceEventCache eventCache;

		public string EvalXPath(string xpath, XmlSerializerNamespaces namespaces) {
			var nsResolver = new XmlSerializerNamespaceResolver(namespaces);
			var expr = XPathExpression.Compile(xpath, nsResolver);
			switch (xpath) {
				case "/log-message/id":
					return id.ToString(NumberFormatInfo.InvariantInfo);
				case "/log-message/source":
					return source;
				case "/log-message/event-type":
					return eventType.ToString();
				case "/log-message/message":
					return message;
			}
			throw new Exception("failed to evaluate xpath expression");
		}
	}

	public class ObservableTraceListener : TraceListener {
		//TODO: use weak reference
		protected static Dictionary<string, Subject<LogMessage>> s_sinks = new Dictionary<string, Subject<LogMessage>>();
		protected static Subject<LogMessage> s_unnamedSink = new Subject<LogMessage>();
		protected Subject<LogMessage> m_subj = null;

		public ObservableTraceListener() {
			m_subj = s_unnamedSink;
		}

		public ObservableTraceListener(string sinkName) {
			if (String.IsNullOrWhiteSpace(sinkName)) {
				m_subj = s_unnamedSink;
			} else {
				lock (s_sinks) {
					if (!s_sinks.TryGetValue(sinkName, out m_subj)) {
						m_subj = new Subject<LogMessage>();
						s_sinks.Add(sinkName, m_subj);
					}
				}
			}
		}

		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id) {
			this.TraceEvent(eventCache, source, eventType, id, string.Empty);
		}

		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message) {
			if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null)) {
				var logMsg = new LogMessage() {
					eventCache = eventCache,
					source = source,
					eventType = eventType,
					message = message,
					id = id
				};
				m_subj.OnNext(logMsg);
			}
		}

		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args) {
			if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null)) {
				var logMsg = new LogMessage() {
					eventCache = eventCache,
					source = source,
					eventType = eventType,
					message = String.Format(CultureInfo.InvariantCulture, format, args),
					id = id
				};
				m_subj.OnNext(logMsg);
			}
		}

		public static IObservable<LogMessage> GetLogMessages() {
			return s_unnamedSink;
		}

		public static IObservable<LogMessage> GetLogMessages(string sinkName) {
			if (String.IsNullOrWhiteSpace(sinkName)) {
				return GetLogMessages();
			}
			Subject<LogMessage> subj = null;
			lock (s_sinks) {
				if (!s_sinks.TryGetValue(sinkName, out subj)) {
					subj = new Subject<LogMessage>();
					s_sinks.Add(sinkName, subj);
				}
			}
			return subj;
		}

		public override void Write(string message) {
			var evtCache = new TraceEventCache();
			TraceEvent(evtCache, null, TraceEventType.Information, 0, message);
		}

		public override void WriteLine(string message) {
			var evtCache = new TraceEventCache();
			TraceEvent(evtCache, null, TraceEventType.Information, 0, message);
		}
	}
}
