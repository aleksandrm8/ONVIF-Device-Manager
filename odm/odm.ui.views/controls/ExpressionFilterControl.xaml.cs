using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;

using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

using odm.core;
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views;
using onvif.services;
using utils;
using System.Windows.Input;
using System.ComponentModel;
using System.Xml.Linq;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for ExpressionFilterControl.xaml
	/// </summary>
	public partial class ExpressionFilterControl : UserControl {
		public ExpressionFilterControl() {
			InitializeComponent();
			Localization();
		}
		
		void Localization() {
			btnDeleteFilter.CreateBinding(Button.ContentProperty, LocalButtons.instance, x => x.delete);
			groupFilters.CreateBinding(GroupBox.HeaderProperty, LocalMetadata.instance, x => x.filter);
			includeEvents.CreateBinding(CheckBox.ContentProperty, LocalMetadata.instance, x => x.includeEvents);
		}
		
		public ObservableCollection<FilterExpression> filtersList { get; set; }
		ExpressionArguments arguments;

		IExpressionEditor exprEditor = null;

		public void Init(ExpressionArguments arguments, bool IsIncludeEvents = true) {
			this.arguments = arguments;
			if (IsIncludeEvents) {
				includeEvents.Visibility = System.Windows.Visibility.Visible;
			}
			//panelFilterEditor.Children.Add(new ExpressionFilterEditor());
			if (AppDefaults.visualSettings.UseOnlyCommonFilterView)
				exprEditor = new ExpressionDefaultEditor(arguments, AddFilter);
			else { 
				exprEditor = new ExpressionFilterEditor(arguments, AddFilter);
			}
			panelFilterEditor.Children.Add((UserControl)exprEditor);


			filtersList = new ObservableCollection<FilterExpression>();

			//buttons initialization
			//Delete filter button
			btnDeleteFilter.IsEnabled = valueFilters.SelectedItem == null ? false : true;
			//Current filter list
			valueFilters.ItemsSource = filtersList;
			valueFilters.SelectionChanged += new SelectionChangedEventHandler((o, e) => {
				if (valueFilters.SelectedItem == null)
					btnDeleteFilter.IsEnabled = false;
				else {
					btnDeleteFilter.IsEnabled = true;
					FilterSelected((FilterExpression)valueFilters.SelectedItem);
				}
			});

			btnDeleteFilter.Click += new RoutedEventHandler((o, e) => {
				if (valueFilters.SelectedItem == null)
					return;
				FilterExpression sitem = valueFilters.SelectedItem as FilterExpression;
				if (sitem != null) {
					filtersList.Remove(sitem);
				}
			});

			//fill startup filters
			arguments.messageContentFilters.ForEach(f => {
				filtersList.Add(ContentFilterExpression.CreateFilter(f));
			});
			arguments.topicExpressionFilters.ForEach(f => {
				filtersList.Add(TopicFilterExpression.CreateFilter(f));
			});
		}
		void SelectUiElement(FilterExpression filterExpression) {
			if (AppDefaults.visualSettings.UseOnlyCommonFilterView) {
				exprEditor = new ExpressionDefaultEditor(arguments, AddFilter);
				panelFilterEditor.Children.Clear();
				panelFilterEditor.Children.Add((UserControl)exprEditor);
			} else {
				if (filterExpression.Dialect != null &&
					filterExpression.Dialect != DefaultDialects.dialecttopic1.dialect &&
					filterExpression.Dialect != DefaultDialects.dialecttopic2.dialect &&
					filterExpression.Dialect != DefaultDialects.dialectcontent1.dialect) {
					exprEditor = new ExpressionDefaultEditor(arguments, AddFilter);
					panelFilterEditor.Children.Clear();
					panelFilterEditor.Children.Add((UserControl)exprEditor);
				} else {
					exprEditor = new ExpressionFilterEditor(arguments, AddFilter);
					panelFilterEditor.Children.Clear();
					panelFilterEditor.Children.Add((UserControl)exprEditor);
				}
			}

		}
		void AddFilter(FilterExpression filter) {
			filtersList.Add(filter);
		}

		void FilterSelected(FilterExpression filterExpression) {
			SelectUiElement(filterExpression);
			if (exprEditor != null)
				exprEditor.SelectFilter(filterExpression);
			else {
				dbg.Error("Interface not initialized!");
			}
		}

		public bool IsIncludeEvents {
			get { 
				return (bool)GetValue(IsIncludeEventsProperty); }
			set {
				SetValue(IsIncludeEventsProperty, value); 
			}}
		public static readonly DependencyProperty IsIncludeEventsProperty = DependencyProperty.Register("IsIncludeEvents", typeof(bool), typeof(ExpressionFilterControl), new PropertyMetadata(true));

	}
	public interface IExpressionEditor {
		void SelectFilter(FilterExpression filter);
	}
	public class PrefixSpacePair : DependencyObject {
		public string Space { get { return (string)GetValue(SpaceProperty); } set { SetValue(SpaceProperty, value); } }
		public static readonly DependencyProperty SpaceProperty = DependencyProperty.Register("Space", typeof(string), typeof(PrefixSpacePair), new UIPropertyMetadata(""));

		public string Prefix { get { return (string)GetValue(PrefixProperty); } set { SetValue(PrefixProperty, value); } }
		public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register("Prefix", typeof(string), typeof(PrefixSpacePair), new UIPropertyMetadata(""));
	}

	public class ExpressionArguments {
		public ExpressionArguments(string[] messageContentFilterDialects,
			MessageContentFilter[] messageContentFilters,
			string[] topicExpressionDialects,
			TopicExpressionFilter[] topicExpressionFilters,
			TopicSetType topicSet, Dictionary<string, string> namespaces) {

			this.messageContentFilterDialects = messageContentFilterDialects;
			this.messageContentFilters = messageContentFilters;
			this.topicExpressionDialects = topicExpressionDialects;
			this.topicExpressionFilters = topicExpressionFilters;
			this.topicSet = topicSet;
			this.namespaces = namespaces;

		}
		public string[] messageContentFilterDialects;
		public MessageContentFilter[] messageContentFilters;
		public string[] topicExpressionDialects;
		public TopicExpressionFilter[] topicExpressionFilters;
		public TopicSetType topicSet;
		public Dictionary<string, string> namespaces;
	}

	public class TopicFilterExpression : FilterExpression {
		protected TopicFilterExpression() { }

		public static TopicFilterExpression CreateFilter(TopicExpressionFilter filtr) {
			return new TopicFilterExpression() {
				currentType = ftype.TOPIC,
				Value = filtr.expression,
				Dialect = filtr.dialect,
				Namespaces = filtr.namespaces
			};
		}
	}
	public class ContentFilterExpression : FilterExpression {
		protected ContentFilterExpression() { }

		public static ContentFilterExpression CreateFilter(MessageContentFilter filtr) {

			return new ContentFilterExpression() {
				currentType = ftype.CONTENT,
				Value = filtr.expression,
				Dialect = filtr.dialect,
				Namespaces = filtr.namespaces
			};
		}
	}
	public class FilterExpression : DependencyObject {
		public enum ftype {
			TOPIC,
			CONTENT
		};
		public static Dictionary<ftype, string> filterTypeNames = new Dictionary<ftype, string>() { { ftype.CONTENT, "Content" }, { ftype.TOPIC, "Topic" } };
		protected ftype currentType;
		public ftype FilterType { get { return currentType; } }
		public string FilterTypeName {
			get {
				return filterTypeNames[currentType];
			}
		}
		protected FilterExpression() {
		}

		public XmlSerializerNamespaces Namespaces { get; set; }

		public string DisplayName {
			get {
				string val = "";

				val += FilterTypeName + ": " + Value;

				return val;
			}
		}

		public string Dialect { get { return (string)GetValue(DialectProperty); } set { SetValue(DialectProperty, value); } }
		public static readonly DependencyProperty DialectProperty = DependencyProperty.Register("Dialect", typeof(string), typeof(FilterType));

		public string Value { get { return (string)GetValue(ValueProperty); } set { SetValue(ValueProperty, value); } }
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(FilterType));

		//List<string> prefixList = new List<string>();
		//public List<string> PrefixList { get { return prefixList; } }
		//public void AddInfo(string info) {
		//    PrefixList.Add(info);
		//}
	}

	public class DefaultDialects{
		static private readonly string dialect1 = "http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete";
		static private readonly string dialect2 = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
		static private readonly string dialect3 = "http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter";
		public static TypeDialectPair dialecttopic1 = new TypeDialectPair(FilterExpression.ftype.TOPIC, dialect1);
		public static TypeDialectPair dialecttopic2 = new TypeDialectPair(FilterExpression.ftype.TOPIC, dialect2);
		public static TypeDialectPair dialectcontent1 = new TypeDialectPair(FilterExpression.ftype.CONTENT, dialect3);
	}
	public class TypeDialectPair : DependencyObject {
		public TypeDialectPair(odm.ui.controls.FilterExpression.ftype tp, string dialect) {
			this.tp = tp;
			this.dialect = dialect;

			Name = FilterExpression.filterTypeNames[tp] + "  " + dialectName;
		}
		public odm.ui.controls.FilterExpression.ftype tp { get; private set; }
		public string dialect { get; private set; }
		public string dialectName { 
			get {
				string ret = dialect;
				try {
					var arr = ret.Split('/');
					var cnt = arr.Length;
					if (cnt != 0)
						ret = arr[cnt - 1];
				} catch (Exception err) { 
					dbg.Error(err); 
				}

				return ret;
			} 
		}
		public string Name { get { return (string)GetValue(NameProperty); } set { SetValue(NameProperty, value); } }
		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(TypeDialectPair), new UIPropertyMetadata(""));
	}
}
