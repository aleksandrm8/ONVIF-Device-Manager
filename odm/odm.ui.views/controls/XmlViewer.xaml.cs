
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Xsl;
using odm.models;
using System.IO;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for XmlViewer.xaml
	/// </summary>
	public partial class XmlViewer : UserControl {
		public XmlViewer() {
			InitializeComponent();
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
        //public void InitControls(DumpModel devModel) {
        //    //devModel.xmlDump
        //    var html = new StringBuilder();
        //    var writer = new StringWriter(html);
        //    xml2html.Transform(devModel.xmlDump, null, writer);
		
        //    htmlView.NavigateToString(html.ToString());
        //}

	}

	public class XmlUnit : DependencyObject {
		public XmlUnit(XmlDocument value) {
			this.Value = value;
		}
		public void InitXmlDoc(XmlDocument value) {
			this.Value = value;
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
						using (var xsltStream = EmbeddedResource.xml2html_XmlToHtml10Basic_xslt.GetStream()) {
							//using (var xmlReader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + @"/xml2html/XmlToHtml10Basic.xslt", xmlReaderSettings)) {
							using (var xmlReader = XmlReader.Create(xsltStream, xmlReaderSettings)) {
								xslt.Load(xmlReader, xsltSettings, new XmlUrlResolver());
								xmlReader.Close();
							}
						}
						s_xml2html = xslt;
					}
				}
				return s_xml2html;
			}
		}

		string ConvertToString(XmlDocument doc) {
			try {
				var html = new StringBuilder();
				var writer = new StringWriter(html);
				xml2html.Transform(doc, null, writer);
				return html.ToString();
			} catch (Exception err) {
				return err.Message;
			}
		}

		public XmlDocument Value { get { return (XmlDocument)GetValue(ValueProperty); } set { SetValue(ValueProperty, value); } }
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(XmlDocument), typeof(XmlUnit), new PropertyMetadata(null, (obj, evarg) => {
				if (((XmlDocument)evarg.NewValue) != null) {
					((XmlUnit)obj).html = ((XmlUnit)obj).ConvertToString((XmlDocument)evarg.NewValue);
				}
			}));
		public string html { get { return (string)GetValue(htmlProperty); } set { SetValue(htmlProperty, value); } }
		public static readonly DependencyProperty htmlProperty = DependencyProperty.Register("html", typeof(string), typeof(XmlUnit));
	}
}
