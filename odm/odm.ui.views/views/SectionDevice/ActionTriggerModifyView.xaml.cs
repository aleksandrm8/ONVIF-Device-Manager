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
using System.Reactive.Disposables;
using Microsoft.Practices.Unity;
using Microsoft.FSharp.Control;
using utils;
using odm.infra;
using Microsoft.Practices.Prism.Commands;
using onvif.services;
using System.Xml;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Controls.Primitives;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for ActionTriggerModifyView.xaml
	/// </summary>
	public partial class ActionTriggerModifyView : UserControl {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ActionTriggerModifyView(model, context);

				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion


		public ActionTriggerModifyView() {
			InitializeComponent();
		}

		CompositeDisposable disposables = new CompositeDisposable();

        public void Init(Model model) {
            InitializeComponent();

            OnCompleted += () => {
                disposables.Dispose();
            };

            var availableActions = new ObservableCollection<Action1>();
            var includedActions = new ObservableCollection<Action1>();

            var applyCommand = new DelegateCommand(
                () => {
                    model.trigger.Configuration.TopicExpression = GetTopicExression();
                    if (string.IsNullOrEmpty(model.trigger.Configuration.TopicExpression.Any.First().InnerText))
                        model.trigger.Configuration.TopicExpression = null;
                    if (string.IsNullOrEmpty(model.trigger.Configuration.ContentExpression.Any.First().InnerText))
                        model.trigger.Configuration.ContentExpression = null;
                    model.trigger.Configuration.ActionToken = includedActions.Select(a => a.Token).ToArray();

                    Success(new Result.Apply(model));
                },
                () => true
            );
            applyButton.Command = applyCommand;

            var cancelCommand = new DelegateCommand(
                () => Success(new Result.Cancel(model)),
                () => true
            );
            cancelButton.Command = cancelCommand;

            FixModel(model);

            { // token
                valueToken.CreateBinding(TextBlock.TextProperty, model.trigger, x => x.Token);
                if (string.IsNullOrEmpty(model.trigger.Token)) {
                    captionToken.Visibility = Visibility.Collapsed;
                    valueToken.Visibility = Visibility.Collapsed;
                }
            }

            { // topic filter
                var concreteSetTopics = GetConcreteSetTopics(model.topicSet);
                concreteSetTopics.Insert(0, string.Empty);

                var topicExpr = model.trigger.Configuration.TopicExpression.Any.First().InnerText;
                var topicExprParts = topicExpr.Split(new char[] { '|' });
                foreach (var part in topicExprParts) {
                    var control = CreateTopicExprControl(concreteSetTopics, part);
                    valuesTopicExpr.Items.Add(control);
                }

                var addTopicExprPartCommand = new DelegateCommand(
                    executeMethod: () => {
                        var control = CreateTopicExprControl(concreteSetTopics, string.Empty);
                        valuesTopicExpr.Items.Add(control);
                    },
                    canExecuteMethod: () => valuesTopicExpr.Items.Count <= 32
                );
                addTopicExprPartButton.Command = addTopicExprPartCommand;
            }

            { // content filter
                valueContentExpr.CreateBinding(TextBox.TextProperty, model.trigger.Configuration.ContentExpression
                    , m => m.Any.First().InnerText
                    , (m, v) => m.Any = new XmlNode[] { new XmlDocument().CreateTextNode(v) });
            }

            { // actions
                var addActionCommand = new DelegateCommand(
                () => {
                    var actions = (listAvailableActions.SelectedItems ?? new ArrayList()).Select(i => (Action1)i).ToList();
                    availableActions.RemoveRange(actions);
                    includedActions.AddRange(actions);
                },
                () => (listAvailableActions.SelectedItems ?? new ArrayList()).Count > 0
            );
                addActionButton.Command = addActionCommand;

                var removeActionCommand = new DelegateCommand(
                    () => {
                        var actions = (listIncludedActions.SelectedItems ?? new ArrayList()).Select(i => (Action1)i).ToList();
                        includedActions.RemoveRange(actions);
                        availableActions.AddRange(actions);
                    },
                    () => (listIncludedActions.SelectedItems ?? new ArrayList()).Count > 0
                );
                removeActionButton.Command = removeActionCommand;

                includedActions.AddRange(model.trigger.Configuration.ActionToken.Select(token => model.actions.First(a => a.Token == token)).ToList());
                availableActions.AddRange(model.actions.Except(includedActions).ToList());
                listAvailableActions.ItemsSource = availableActions;
                listIncludedActions.ItemsSource = includedActions;

                listIncludedActions.SelectionChanged += delegate { removeActionCommand.RaiseCanExecuteChanged(); };
                listAvailableActions.SelectionChanged += delegate { addActionCommand.RaiseCanExecuteChanged(); };
            }

            Localization();
        }

        Selector CreateTopicExprControl(IEnumerable<string> topics, string topic) {
            return new ComboBox() {
                ItemsSource = topics,
                SelectedItem = topic.Trim(),
                IsReadOnly = true,
                IsEditable = false
            };
        }

        TopicExpressionType GetTopicExression() {
            var parts = (from Selector ctrl in valuesTopicExpr.Items
                        where ctrl.SelectedItem != string.Empty
                        select (string)ctrl.SelectedItem).ToArray();
            
            string dialect = @"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
            if (parts.Length == 1 && !parts.First().EndsWith("//."))
                dialect = @"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete";

            var expr = string.Join("|", parts);

            var topicExpr = new TopicExpressionType() {
                Dialect = dialect,
                Any = new XmlNode[] { new XmlDocument().CreateTextNode(expr) }
            };
            return topicExpr;
        }
            

        LocalButtons ButtonsStrings { get { return LocalButtons.instance; } }
        ActionEngineStrings Strings { get { return ActionEngineStrings.instance; } }
        void Localization() {
            applyButton.CreateBinding(Button.ContentProperty, ButtonsStrings, m => m.apply);
            cancelButton.CreateBinding(Button.ContentProperty, ButtonsStrings, m => m.cancel);

            captionToken.CreateBinding(TextBlock.TextProperty, Strings, m => m.triggerToken);
            captionTopicExpr.CreateBinding(TextBlock.TextProperty, Strings, m => m.triggerTopicExpression);
            addTopicExprPartButton.CreateBinding(Button.ContentProperty, Strings, m => m.addTriggerTopicExprPart);
            addTopicExprPartButton.CreateBinding(Button.ToolTipProperty, Strings, m => m.addTriggerTopicExprPartTooltip);
            captionContentExpr.CreateBinding(TextBlock.TextProperty, Strings, m => m.triggerContentExpression);
            captionAvailableActions.CreateBinding(TextBlock.TextProperty, Strings, m => m.triggerAvailableActions);
            captionIncludedActions.CreateBinding(TextBlock.TextProperty, Strings, m => m.triggerIncludedActions);
            addActionButton.CreateBinding(Button.ContentProperty, Strings, m => m.triggerAddAction);
            removeActionButton.CreateBinding(Button.ContentProperty, Strings, m => m.triggerRemoveAction);
        }

        private void FixModel(Model model) {
            model.actions = model.actions ?? new Action1[0];
            foreach (var action in model.actions) {
                action.Configuration = action.Configuration ?? new ActionConfiguration();
                action.Configuration.Parameters = action.Configuration.Parameters ?? new ItemList();
                action.Configuration.Parameters.simpleItem = action.Configuration.Parameters.simpleItem ?? new ItemList.SimpleItem[0];
                action.Configuration.Parameters.elementItem = action.Configuration.Parameters.elementItem ?? new ItemList.ElementItem[0];
            }

            model.trigger = model.trigger ?? new ActionTrigger();
            model.trigger.Configuration = model.trigger.Configuration ?? new ActionTriggerConfiguration();
            model.trigger.Configuration.TopicExpression = model.trigger.Configuration.TopicExpression ?? new TopicExpressionType();
            model.trigger.Configuration.TopicExpression.Dialect = model.trigger.Configuration.TopicExpression.Dialect ?? @"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
            model.trigger.Configuration.TopicExpression.Any = model.trigger.Configuration.TopicExpression.Any ?? new XmlNode[] { new XmlDocument().CreateTextNode(string.Empty) };
            model.trigger.Configuration.ContentExpression = model.trigger.Configuration.ContentExpression ?? new QueryExpressionType2();
            model.trigger.Configuration.ContentExpression.Dialect = model.trigger.Configuration.ContentExpression.Dialect ?? @"http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter";
            model.trigger.Configuration.ContentExpression.Any = model.trigger.Configuration.ContentExpression.Any ?? new XmlNode[] { new XmlDocument().CreateTextNode(string.Empty) };
            model.trigger.Configuration.ActionToken = model.trigger.Configuration.ActionToken ?? new string[0];

            model.topicSet = model.topicSet ?? new TopicSetType();
            model.topicSet.Any = model.topicSet.Any ?? new XmlElement[0];
        }

        #region GetTopics

        //TODO refactor: move to the onvif.session project
        private static IList<String> GetConcreteTopics(TopicSetType topicSet) {
            var topics = new List<string>();
            var roots = topicSet.Any ?? new XmlElement[0];
            foreach (var root in roots) {
                string path = root.Name;
                ParseConcreteTopicElement(root, path, topics);
            }
            return topics;
        }
        private static void ParseConcreteTopicElement(XmlElement element, string basePath, IList<string> topics) { 
            element.ChildNodes.ForEachElement( elem => {
                string path = basePath + '/' + elem.Name;
                if (elem.Attributes.Any( (attr) => attr.NamespaceURI == @"http://docs.oasis-open.org/wsn/t-1" && attr.LocalName == "topic" && attr.Value == "true"))
                    topics.Add(path);
                else
                    ParseConcreteTopicElement(elem, path, topics);
            });
        }

        private static IList<String> GetConcreteSetTopics(TopicSetType topicSet) {
            var topics = new List<string>();
            var roots = topicSet.Any ?? new XmlElement[0];
            foreach (var root in roots) {
                string path = root.Name;
                ParseConcreteSetTopicElement(root, path, topics);
            }
            return topics;
        }
        private static void ParseConcreteSetTopicElement(XmlElement element, string basePath, IList<string> topics) {
            topics.Add(basePath + "//.");
            element.ChildNodes.ForEachElement(elem => {
                string path = basePath + '/' + elem.Name;
                if (elem.Attributes.Any((attr) => attr.NamespaceURI == @"http://docs.oasis-open.org/wsn/t-1" && attr.LocalName == "topic" && attr.Value == "true")) {
                    topics.Add(path);
                }
                else {
                    topics.Add(path + "//.");
                    ParseConcreteSetTopicElement(elem, path, topics);
                }
            });
        }

        #endregion GetTopics
    }

    internal class TopicDialectConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (@"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete".Equals(value))
                value = "Concrete";
            else if (@"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet".Equals(value))
                value = "ConcreteSet";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if ("Concrete".Equals(value))
                value = @"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete";
            else if ("ConcreteSet".Equals(value))
                value = @"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
            return value;
        }
    }
}
