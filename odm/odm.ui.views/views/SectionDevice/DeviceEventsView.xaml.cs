using odm.ui.viewModels;
using System.Linq;
using utils;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using odm.ui.views;
using System.Collections.ObjectModel;
using System.Windows;
using odm.core;
using odm.ui.core;
using System.Linq.Expressions;
using System.ComponentModel;
using System;
using odm.infra;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Serialization;
using onvif.services;

namespace odm.ui.controls {
	public partial class DeviceEventsView : BasePropertyControl, INotifyPropertyChanged {
		public DeviceEventsView(IUnityContainer container) {
			InitializeComponent();

			this.container = container;
            this.DataContext = this;

            Loaded += new System.Windows.RoutedEventHandler(DeviceEventsView_Loaded);
		}
		EventAggregator eventAggregator;
		IUnityContainer container;
		INvtSession session; 
		Account account;

		IDisposable eventPropSubscription;

		public LocalDevice AppStrings { get { return LocalDevice.instance; } }
		public LocalMetadata strings { get { return LocalMetadata.instance; } }
		public LocalButtons SaveCanel { get { return LocalButtons.instance; } }

		Dictionary<string, string> namespaces = new Dictionary<string, string>();
		private bool TryAddExploredNamespace(string prefix, string ns) {
			string existingNs;
			if (namespaces.TryGetValue(prefix, out existingNs)) {
				return existingNs == ns;
			}
			namespaces.Add(prefix, ns);
			return true;
		}
		private void ExploreNamespaces(IEnumerable<XmlElement> xmlElements) {
			foreach (var e in xmlElements) {
				var ns = e.NamespaceURI;
				if (!String.IsNullOrEmpty(ns)) {
					var prefix = e.Prefix;
					if (String.IsNullOrEmpty(prefix)) {
						prefix = e.GetPrefixOfNamespace(ns);
						if (String.IsNullOrEmpty(prefix)) {
							prefix = "ns";
						}
					}
					var i = 0;
					var prefixBase = prefix;
					while (!TryAddExploredNamespace(prefix, ns)) {
						prefix = prefixBase + i;
					}
				}
				if (e.ChildNodes != null) {
					ExploreNamespaces(
						e.ChildNodes
							.OfType<XmlNode>()
							.Where(x => x.NodeType == XmlNodeType.Element)
							.OfType<XmlElement>()
					);
				}
			}
		}
		public ObservableCollection<FilterExpression> filters { get; set; }
		DeviceEventsEventArgs evarg;
		public void Init(DeviceEventsEventArgs evarg) {
			this.session = evarg.session;
			this.account = evarg.currentAccount;
			this.filters = evarg.filters;
			this.evarg = evarg;

			eventAggregator = container.Resolve<EventAggregator>();

			this.CreateBinding(StateCommonProperty, this, x => {
				return x.Current == States.Common ? Visibility.Visible : Visibility.Collapsed;
			});
			this.CreateBinding(StateLoadingProperty, this, x => {
				return x.Current == States.Loading ? Visibility.Visible : Visibility.Collapsed;
			});
			this.CreateBinding(StateErrorProperty, this, x => {
				return x.Current == States.Error ? Visibility.Visible : Visibility.Collapsed;
			});

			Current = States.Loading;
			eventPropSubscription = session.GetEventProperties()
				.ObserveOnCurrentDispatcher()
				.Subscribe(responce => {
					Current = States.Common;

					if (responce.TopicSet != null && responce.TopicSet.Any != null) {
						ExploreNamespaces(responce.TopicSet.Any);
					}

					List<MessageContentFilter> contList = new List<MessageContentFilter>();
					List<TopicExpressionFilter> topList = new List<TopicExpressionFilter>();

					evarg.filters.Where(f => f.FilterType == FilterExpression.ftype.CONTENT).ForEach(x => { 
						var fex = (FilterExpression)x;
						contList.Add(new MessageContentFilter() { dialect = fex.Dialect, expression = fex.Value, namespaces = fex.Namespaces });
					});
					evarg.filters.Where(f => f.FilterType == FilterExpression.ftype.TOPIC).ForEach(x => {
						var fex = (FilterExpression)x;
						topList.Add(new TopicExpressionFilter() { dialect = fex.Dialect, expression = fex.Value, namespaces = fex.Namespaces });
					});

					ExpressionArguments args = new ExpressionArguments(
					responce.MessageContentFilterDialect, contList.ToArray(),
					responce.TopicExpressionDialect, topList.ToArray(), 
					responce.TopicSet, namespaces);
					
					expressionFilters.Init(args, false);
				}, err => {
					Current = States.Error;
					ErrorMessage = err.Message;
					dbg.Error(err);
			});

			btnApplyFilters.Click+=new RoutedEventHandler((o,e)=>{
				// resubscribe to events.
				evarg.filters.Clear();
				expressionFilters.filtersList.ForEach(item => {
					evarg.filters.Add(item);
				});
				eventAggregator.GetEvent<AddEventsFilterClick>().Publish(evarg);
			});
			//Filter settings
			//ExpressionArguments args = new ExpressionArguments(
			//    model.messageContentFilterDialects, model.messageContentFilters,
			//    model.topicExpressionDialects, model.topicExpressionFilters, model.topicSet, namespaces);
			//expressionFilters.Init(args);
		}

		public void BindEvents(EventsStorage events) {
			events.eventsCollection.ForEach(x => {
				if (x.ErrorMessage != null) {
					ErrorMsg = x.ErrorMessage;
					ErrorVisibility = Visibility.Visible;
					EventsVisibility = Visibility.Visible;
				}
			});
			this.events = events.eventsCollection;
		}

		void DeviceEventsView_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            colArrivalTime.CreateBinding(GridViewColumn.HeaderProperty, Strings, s => s.colArrivalTime);
            colSource.CreateBinding(GridViewColumn.HeaderProperty, Strings, s => s.colSource);
            colKey.CreateBinding(GridViewColumn.HeaderProperty, Strings, s => s.colKey);
            colData.CreateBinding(GridViewColumn.HeaderProperty, Strings, s => s.colData);
            colDetails.CreateBinding(GridViewColumn.HeaderProperty, Strings, s => s.colDetails);
            colMessage.CreateBinding(GridViewColumn.HeaderProperty, Strings, s => s.colMessage);
            colProperty.CreateBinding(GridViewColumn.HeaderProperty, Strings, s => s.colProperty);
            colTopic.CreateBinding(GridViewColumn.HeaderProperty, Strings, s => s.colTopic);

			eventAggregator.GetEvent<DeviceEventReceived>().Subscribe(eargs => {
				LastEvent = eargs.data;
			});
        }
        public LocalTitles TitleStrings { get { return LocalTitles.instance; } }
        public LocalEvents Strings { get { return LocalEvents.instance; } }


		public Visibility EventsVisibility {get { return (Visibility)GetValue(EventsVisibilityProperty); }set { SetValue(EventsVisibilityProperty, value); }}
		public static readonly DependencyProperty EventsVisibilityProperty = DependencyProperty.Register("EventsVisibility", typeof(Visibility), typeof(DeviceEventsView), new UIPropertyMetadata(Visibility.Visible));

		public Visibility ErrorVisibility { get { return (Visibility)GetValue(ErrorVisibilityProperty); } set { SetValue(ErrorVisibilityProperty, value); }}
		public static readonly DependencyProperty ErrorVisibilityProperty = DependencyProperty.Register("ErrorVisibility", typeof(Visibility), typeof(DeviceEventsView), new UIPropertyMetadata(Visibility.Collapsed));

		public string ErrorMsg {get { return (string)GetValue(ErrorMsgProperty); }set { SetValue(ErrorMsgProperty, value); }}
		public static readonly DependencyProperty ErrorMsgProperty = DependencyProperty.Register("ErrorMsg", typeof(string), typeof(DeviceEventsView));

		public odm.ui.views.EventDescriptor LastEvent { get { return (odm.ui.views.EventDescriptor)GetValue(LastEventProperty); } set { SetValue(LastEventProperty, value); } }
		public static readonly DependencyProperty LastEventProperty = DependencyProperty.Register("LastEvent", typeof(odm.ui.views.EventDescriptor), typeof(DeviceEventsView));

		public ObservableCollection<odm.ui.views.EventDescriptor> events { get; set; }

		#region States
		public enum States {
			Loading,
			Common,
			Error
		}
		States current;
		public States Current {
			get {
				return current;
			}
			set {
				current = value;
				OnPropertyChanged(() => Current);
			}
		}
		public virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyEvaluator) {
			var lambda = propertyEvaluator as LambdaExpression;
			var member = lambda.Body as MemberExpression;
			var handler = PropertyChanged;
			if (handler != null) {
				handler(this, new PropertyChangedEventArgs(member.Member.Name));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		#region StatesProperties
		public Visibility StateCommon {
			get { return (Visibility)GetValue(StateCommonProperty); }
			set { SetValue(StateCommonProperty, value); }
		}
		// Using a DependencyProperty as the backing store for StateCommon.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StateCommonProperty =
			DependencyProperty.Register("StateCommon", typeof(Visibility), typeof(DeviceEventsView), new PropertyMetadata(Visibility.Collapsed));

		public Visibility StateError {
			get { return (Visibility)GetValue(StateErrorProperty); }
			set { SetValue(StateErrorProperty, value); }
		}
		// Using a DependencyProperty as the backing store for StateError.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StateErrorProperty =
			DependencyProperty.Register("StateError", typeof(Visibility), typeof(DeviceEventsView), new PropertyMetadata(Visibility.Collapsed));

		public Visibility StateLoading {
			get { return (Visibility)GetValue(StateLoadingProperty); }
			set { SetValue(StateLoadingProperty, value); }
		}
		// Using a DependencyProperty as the backing store for StateLoading.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StateLoadingProperty =
			DependencyProperty.Register("StateLoading", typeof(Visibility), typeof(DeviceEventsView), new PropertyMetadata(Visibility.Collapsed));

		public string ErrorMessage {
			get { return (string)GetValue(ErrorMessageProperty); }
			set { SetValue(ErrorMessageProperty, value); }
		}
		// Using a DependencyProperty as the backing store for ErrorMessage.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ErrorMessageProperty =
			DependencyProperty.Register("ErrorMessage", typeof(string), typeof(DeviceEventsView));

		#endregion StatesProperties

	}
}
