using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

using utils;

namespace odm.config {

	//<logger-config>
	//	<columns>
	//		<column name="id" xpath="/log-message/id"/>
	//		<column name="source" xpath="/log-message/source"/>
	//		<column name="eventType" xpath="/log-message/eventType"/>
	//		<column name="message" xpath="/log-message/message"/>
	//	</columns>
	//	<details>
	//		<detail name="XML" xpath="" xslt=""/>
	//		<detail name="XML" xpath="" xslt=""/>
	//		<detail name="XML" xpath="" xslt=""/>
	//	</details>			 
	//</logger-config>

	public static class LoggerConfigAPI {
		private const string c_xmlFilePath = "logger.config.xml";
		public static LoggerConfig Load(string xmlFilePath = null) {
			if (xmlFilePath == null) {
				xmlFilePath = c_xmlFilePath;
			}
			var doc = new XmlDocument();
			doc.Load(xmlFilePath);
			var nav = doc.CreateNavigator();
			var ns = nav.GetNamespacesInScope(new XmlNamespaceScope());
			return doc.Deserialize<LoggerConfig>();
		}
		public static void Save(LoggerConfig loggerConig, string xmlFilePath = null) {
			if (xmlFilePath == null) {
				xmlFilePath = c_xmlFilePath;
			}
			var doc = new XmlDocument();
			using (var w = new XmlTextWriter(xmlFilePath, Encoding.UTF8)) {
				w.Formatting = Formatting.Indented;
				var ns = new XmlSerializerNamespaces();
				ns.Add("", "");
				loggerConig.Serialize(ns).WriteTo(w);
			}
		}
	}
	
	[Serializable]
	[XmlRoot("logger-config")]
	public class LoggerConfig{
		
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces xmlns;

		[XmlArray("columns")]
		[XmlArrayItem("column")]
		public LoggerColumnConfig[] columns;

		[XmlArray("tabs")]
		[XmlArrayItem("tab")]
		public LoggerTabConfig[] tabs;		
	}
	
	[Serializable]
	[XmlRoot("column")]
	public class LoggerColumnConfig {

		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces xmlns;

		[XmlAttribute("name")]
		public string name;

		[XmlAttribute("xpath")]
		public string xpath;
	}
	
	[Serializable]
	public class LoggerTabConfig {
		
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces xmlns;

		[XmlAttribute("name")]
		public string name;

		[XmlAttribute("xpath")]
		public string xpath;

		[XmlAttribute("xslt")]
		public string xslt;
	}
}
