using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Windows;
using System.Xml.Xsl;
using System.IO;

namespace odm.ui.controls {
	public class ControlXMLView : ContentControl {
		public ControlXMLView() {

		}

		private static object gate = new object();
		private static XslCompiledTransform s_xml2html = null;
		private static XslCompiledTransform xml2html {
			get {
				lock (gate) {
					if (s_xml2html == null) {
						var xslt = new XslCompiledTransform();

						var xmlReaderSettings = new XmlReaderSettings() {
							DtdProcessing = DtdProcessing.Parse
						};
						XsltSettings xsltSettings = new XsltSettings() {
							EnableScript = false,
							EnableDocumentFunction = false
						};

						using (var xmlReader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + @"xml2html/XmlToHtml10Basic.xslt", xmlReaderSettings)) {
							xslt.Load(xmlReader, xsltSettings, new XmlUrlResolver());
							xmlReader.Close();
						}
						s_xml2html = xslt;
					}
				}
				return s_xml2html;
			}
		}
		public string ConvertToString(XmlDocument doc) {
			var html = new StringBuilder();
			var writer = new StringWriter(html);
			xml2html.Transform(doc, null, writer);
		
			return html.ToString();
		}
		public string StringInfo {
			get { return (string)GetValue(StringInfoProperty); }
			set { SetValue(StringInfoProperty, value); }
		}

		// Using a DependencyProperty as the backing store for StringInfo.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StringInfoProperty =
			DependencyProperty.Register("StringInfo", typeof(string), typeof(ControlXMLView));

		
		public XmlDocument XmlInfo {
			get { return (XmlDocument)GetValue(XmlInfoProperty); }
			set { SetValue(XmlInfoProperty, value); }
		}
		// Using a DependencyProperty as the backing store for XmlInfo.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty XmlInfoProperty =
			DependencyProperty.Register("XmlInfo", typeof(XmlDocument), typeof(ControlXMLView), new UIPropertyMetadata((obj, ev) => {
				((ControlXMLView)obj).StringInfo = ((ControlXMLView)obj).ConvertToString((XmlDocument)ev.NewValue);
			}));
	}
	public class WebBrowserHelper {
		public static readonly DependencyProperty BodyProperty =
			DependencyProperty.RegisterAttached("Body", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnBodyChanged));

		public static string GetBody(DependencyObject dependencyObject) {
			return (string)dependencyObject.GetValue(BodyProperty);
		}

		public static void SetBody(DependencyObject dependencyObject, string body) {
			dependencyObject.SetValue(BodyProperty, body);
		}

		private static void OnBodyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var webBrowser = (WebBrowser)d;
			webBrowser.NavigateToString((string)e.NewValue);
		}
	}
}
