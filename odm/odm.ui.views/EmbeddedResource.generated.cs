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
using System.IO;

using utils;

namespace odm.ui {
	public class EmbeddedResource {
		public static readonly EmbeddedResource xml2html_XmlToHtml10_xslt = new EmbeddedResource("odm.ui.xml2html.XmlToHtml10.xslt");
		public static readonly EmbeddedResource xml2html_XmlToHtml10Basic_xslt = new EmbeddedResource("odm.ui.xml2html.XmlToHtml10Basic.xslt");

		public Stream GetStream() {
			var asm = this.GetType().Assembly;
			return asm.GetManifestResourceStream(resourceName);
		}
		private string resourceName;
		private EmbeddedResource(string resourceName) {
			this.resourceName = resourceName;
		}
	}
}
