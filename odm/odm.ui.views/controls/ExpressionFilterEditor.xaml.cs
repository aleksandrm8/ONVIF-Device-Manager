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
using utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for ExpressionFilterEditor.xaml
	/// </summary>
	public partial class ExpressionFilterEditor : UserControl, INotifyPropertyChanged, IExpressionEditor {
		public ExpressionFilterEditor() {
			InitializeComponent();
		}
		public ExpressionFilterEditor(ExpressionArguments arguments, Action<FilterExpression> addFilterExpression) {
			InitializeComponent();
			this.arguments = arguments;
			this.addFilterExpression = addFilterExpression;

			Localization();
			Init();
		}

		Action<FilterExpression> addFilterExpression;
		ExpressionArguments arguments;
		public ObservableCollection<KeyValuePair<string, string>> DialectsDictionary { get; private set; }
		public ObservableCollection<PrefixSpacePair> PrefixList { get; private set; }
		public ObservableCollection<TypeDialectPair> Dialects { get; set; }

		string expressionValue = "";
		public string ExpressionValue {
			get { return expressionValue; }
			set {
				expressionValue = value;
				if (expressionValue != "")
					btnAddFilter.IsEnabled = true;
				else
					btnAddFilter.IsEnabled = false;
				NotifyPropertyChanged("ExpressionValue");
			}
		}

		void Localization() {
			groupAddFilter.CreateBinding(GroupBox.HeaderProperty, LocalMetadata.instance, x => x.expressionSettings);
			groupAddition.CreateBinding(GroupBox.HeaderProperty, LocalMetadata.instance, x => x.additionalSet);
			captionExpressionType.CreateBinding(Label.ContentProperty, LocalMetadata.instance, x => x.expressionType);
			captionExpression.CreateBinding(Label.ContentProperty, LocalMetadata.instance, x => x.expression);
			captionPrefList.CreateBinding(TextBlock.TextProperty, LocalMetadata.instance, x => x.prefixList);
			captionTopicSet.CreateBinding(TextBlock.TextProperty, LocalMetadata.instance, x => x.topicSets);
			btnAddFilter.CreateBinding(Button.ContentProperty, LocalMetadata.instance, x => x.addFilter);
		}
		void Init() {
			Dialects = new ObservableCollection<TypeDialectPair>();
			PrefixList = new ObservableCollection<PrefixSpacePair>();

			arguments.namespaces.ForEach(itm => {
				PrefixList.Add(new PrefixSpacePair() { Prefix = itm.Key, Space = itm.Value });
			});
			Dialects.Add(DefaultDialects.dialecttopic1);
			Dialects.Add(DefaultDialects.dialecttopic2);
			Dialects.Add(DefaultDialects.dialectcontent1);

			valuePrefixList.ItemsSource = PrefixList;
			valueExpressionType.ItemsSource = Dialects;

			FillXmlDocument();

			valueExpression.CreateBinding(TextBox.TextProperty, this, x => x.ExpressionValue, (m, v) => { m.ExpressionValue = v; });

			btnAddFilter.Click+=new RoutedEventHandler((o,e)=>{
				if (addFilterExpression != null)
					addFilterExpression(CreateFilter());
			});
		}
		FilterExpression CreateFilter() {
			FilterExpression fExpr = null;

			if (valueExpressionType.SelectedItem == null)
				return null;
			var typeDialect = (TypeDialectPair)valueExpressionType.SelectedItem;
			

			XmlSerializerNamespaces nspaces = new XmlSerializerNamespaces();
			PrefixList.ForEach(p => {
				nspaces.Add(p.Prefix, p.Space);
			});

			switch (typeDialect.tp) {
				case FilterExpression.ftype.CONTENT:
					var confiltr = new global::onvif.services.MessageContentFilter();
					confiltr.expression = ExpressionValue;
					confiltr.dialect = typeDialect.dialect;
					confiltr.namespaces = nspaces;
					fExpr = ContentFilterExpression.CreateFilter(confiltr);
					break;
				default:
					var topfiltr = new global::onvif.services.TopicExpressionFilter();
					topfiltr.expression = ExpressionValue;
					topfiltr.dialect = typeDialect.dialect;
					topfiltr.namespaces = nspaces;
					fExpr = TopicFilterExpression.CreateFilter(topfiltr);
					break;
			}

			return fExpr;
		}
		public void SelectFilter(FilterExpression filter) {
			valueExpression.Text = filter.Value;

			Dialects.ForEach(itm => {
				if (itm.dialect == filter.Dialect)
					valueExpressionType.SelectedItem = itm;
			});
		}
		void FillXmlDocument() {
			if (arguments.topicSet == null)
				return;

			XmlDocument xmlDoc = new XmlDocument();

			xmlDoc.AppendChild(xmlDoc.CreateElement("root"));

			arguments.topicSet.Any.ForEach(xelem => {
				xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xelem, true));
			});

			if (xmlUnit == null) {
				xmlUnit = new XmlUnit(xmlDoc);
			} else {
				xmlUnit.InitXmlDoc(xmlDoc);
			}
		}
		public XmlUnit xmlUnit { get { return (XmlUnit)GetValue(xmlUnitProperty); } set { SetValue(xmlUnitProperty, value); } }
		public static readonly DependencyProperty xmlUnitProperty = DependencyProperty.Register("xmlUnit", typeof(XmlUnit), typeof(ExpressionFilterEditor));

		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
