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
using System.IO;
using utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for ExpressionDefaultEditor.xaml
	/// </summary>
	public partial class ExpressionDefaultEditor : UserControl, INotifyPropertyChanged, IExpressionEditor {
		public ExpressionDefaultEditor() {  //<- only for designer
			InitializeComponent();
		}
		public ExpressionDefaultEditor(ExpressionArguments arguments, Action<FilterExpression> addFilterExpression) {
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
			captionExpressionDialect.CreateBinding(Label.ContentProperty, LocalMetadata.instance, x => x.expressionDialect);
			captionPrefList.CreateBinding(TextBlock.TextProperty, LocalMetadata.instance, x => x.prefixList);
			captionTopicSet.CreateBinding(TextBlock.TextProperty, LocalMetadata.instance, x => x.topicSets);
			btnAddFilter.CreateBinding(Button.ContentProperty, LocalMetadata.instance, x => x.addFilter);
			btnAddPrefix .CreateBinding(Button.ContentProperty, LocalButtons.instance, x => x.add);
			btnDeletePrefix.CreateBinding(Button.ContentProperty, LocalButtons.instance, x => x.delete);
		}
		void Init() {
			DialectsDictionary = new ObservableCollection<KeyValuePair<string, string>>();
			PrefixList = new ObservableCollection<PrefixSpacePair>();

			arguments.namespaces.ForEach(itm => {
				PrefixList.Add(new PrefixSpacePair() { Prefix = itm.Key, Space = itm.Value });
			});

			valueExpressionDialect.ItemsSource = DialectsDictionary;

			valueExpressionType.ItemsSource = FilterExpression.filterTypeNames;
			valueExpressionType.SelectionChanged+=new SelectionChangedEventHandler((o,e)=>{
				InitExpressionDialect();
				valueExpressionDialect.SelectedIndex = 0;
			});
			InitExpressionDialect();
			valueExpressionDialect.SelectedIndex = 0;
			InitPrefixList();
			//valuePrefixList;

			valueExpression.CreateBinding(TextBox.TextProperty, this, x=>x.ExpressionValue, (m,v)=>{m.ExpressionValue = v;});

			FillXmlDocument();

			btnAddFilter.Click+=new RoutedEventHandler((o,e)=>{
				if (addFilterExpression != null)
					addFilterExpression(CreateFilter());
			});

			btnAddPrefix.Click+=new RoutedEventHandler((o,e)=>{
				PrefixList.Add(new PrefixSpacePair() { Prefix="", Space = ""});
			});
			btnDeletePrefix.Click += new RoutedEventHandler((o, e) => {
				if (valuePrefixList.SelectedItem != null) {
					var itm = (PrefixSpacePair)valuePrefixList.SelectedItem;
					PrefixList.Remove(itm);
				}
			});
		}

		FilterExpression CreateFilter() {
			FilterExpression fExpr = null;

			if (valueExpressionType.SelectedItem == null)
				return null;
			var tp = ((KeyValuePair<odm.ui.controls.FilterExpression.ftype, string>)valueExpressionType.SelectedItem).Key;
			
			XmlSerializerNamespaces nspaces = new XmlSerializerNamespaces();
			PrefixList.ForEach(p => {
				nspaces.Add(p.Prefix, p.Space);
			});
			
			switch (tp) { 
				case FilterExpression.ftype.CONTENT:
					var confiltr = new global::onvif.services.MessageContentFilter();
					confiltr.expression = ExpressionValue;
					if(valueExpressionDialect.SelectedValue!=null)
						confiltr.dialect = ((KeyValuePair<string, string>)valueExpressionDialect.SelectedValue).Key;
					confiltr.namespaces = nspaces;
					fExpr = ContentFilterExpression.CreateFilter(confiltr);
					break;
				default:
					var topfiltr = new global::onvif.services.TopicExpressionFilter();
					topfiltr.expression = ExpressionValue;
					if (valueExpressionDialect.SelectedValue != null)
						topfiltr.dialect = ((KeyValuePair<string,string>)valueExpressionDialect.SelectedValue).Key;
					topfiltr.namespaces = nspaces;
					fExpr = TopicFilterExpression.CreateFilter(topfiltr);
					break;
			}

			return fExpr;
		}

		#region expression_selected
		public void SelectFilter(FilterExpression filter) {
			if (filter == null)
				return;
			//fill type and dialect
			FillExpressionTypeDialect(filter);
			
			//fill expression
			ExpressionValue = filter.Value;
			//fill prefix list
			PrefixList.Clear();

			var arr = filter.Namespaces.ToArray();
			arr.ForEach(it=>{
				PrefixList.Add(new PrefixSpacePair() { Prefix = it.Name, Space = it.Namespace });
			});
		}
		void FillExpressionTypeDialect(FilterExpression filter) {
			var tp = filter.FilterType;
			DialectsDictionary.Clear();

			FilterExpression.filterTypeNames.ForEach(x => {
				if (x.Key == tp) {
					valueExpressionType.SelectedItem = x;
				}
			});

			if (valueExpressionType.SelectedItem != null) {
				fillDialectByType(filter.FilterType);
				if (filter.Dialect != null) {
					DialectsDictionary.ForEach(d => {
						if (d.Key == filter.Dialect)
							valueExpressionDialect.SelectedItem = d;
					});
				}
			}	
		} 
		#endregion

		#region init_form
		void InitPrefixList() {
			valuePrefixList.ItemsSource = PrefixList;
		}

		void InitExpressionDialect() {
			if (valueExpressionType.SelectedItem == null)
				return;
			var tp = ((KeyValuePair<odm.ui.controls.FilterExpression.ftype, string>)valueExpressionType.SelectedItem).Key;
			DialectsDictionary.Clear();

			fillDialectByType(tp);
		}
		void fillDialectByType(odm.ui.controls.FilterExpression.ftype tp) {
			DialectsDictionary.Clear();
			switch (tp) {
				case FilterExpression.ftype.CONTENT:
					arguments.messageContentFilterDialects.ForEach(item => {
						string name = item;
						try {
							name = item.Split('/').Last();
						} catch (Exception err) {
							dbg.Error(err);
						}
						DialectsDictionary.Add(new KeyValuePair<string, string>(item, name));
					});
					break;
				default:
					arguments.topicExpressionDialects.ForEach(item => {
						string name = item;
						try {
							name = item.Split('/').Last();
						} catch (Exception err) {
							dbg.Error(err);
						}
						DialectsDictionary.Add(new KeyValuePair<string, string>(item, name));
					});
					break;
			}
		}
		#endregion
		

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
		public static readonly DependencyProperty xmlUnitProperty = DependencyProperty.Register("xmlUnit", typeof(XmlUnit), typeof(ExpressionDefaultEditor));

		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
