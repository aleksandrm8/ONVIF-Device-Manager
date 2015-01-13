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
using odm.ui.views;
using System.Globalization;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for ActionModifyView.xaml
	/// </summary>
	public partial class ActionModifyView : UserControl {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ActionModifyView(model, context);

				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion


		public ActionModifyView() {
			InitializeComponent();
		}

		CompositeDisposable disposables = new CompositeDisposable();

		public void Init(Model model) {
			InitializeComponent();

			OnCompleted += () => {
				disposables.Dispose();
			};

			var ApplyCommand = new DelegateCommand(
				() => {
                    GetSimpleItems(model);
			        Success(new Result.Apply(model));
				},
				() => true
			);
			applyButton.Command = ApplyCommand;
			
			var CancelCommand = new DelegateCommand(
				() => Success(new Result.Cancel(model)),
				() => true
			);
			cancelButton.Command = CancelCommand;

			if (model.action == null) 
                valueType.IsEnabled = true;

            FixModel(model);


            valueType.ItemsSource = from type in model.supportedActions.ActionDescription select type.Name;
            valueType.CreateBinding(ComboBox.SelectedValueProperty, model.action.Configuration, x => x.Type, (m, o) => {
                m.Type = o;
                FillSimpleItems(model);
            });

			valueToken.CreateBinding(TextBlock.TextProperty, model.action, x => x.Token);
            if (string.IsNullOrEmpty(model.action.Token)) {
                captionToken.Visibility = Visibility.Collapsed;
                valueToken.Visibility = Visibility.Collapsed;
            }
            
            valueName.CreateBinding(TextBox.TextProperty, model.action, x=>x.Configuration.Name, (m,o) => {
                m.Configuration.Name = o;
            });

		    //bind to simple items
            FillSimpleItems(model);

            Localization();
		}


        public LocalButtons ButtonsStrings { get { return LocalButtons.instance; } }
        public ActionEngineStrings Strings { get { return ActionEngineStrings.instance; } }
        void Localization() {
            applyButton.CreateBinding(Button.ContentProperty, ButtonsStrings, m => m.apply);
            cancelButton.CreateBinding(Button.ContentProperty, ButtonsStrings, m => m.cancel);

            captionToken.CreateBinding(TextBlock.TextProperty, Strings, m => m.actionToken);
            captionName.CreateBinding(TextBlock.TextProperty, Strings, m => m.actionName);
            captionType.CreateBinding(TextBlock.TextProperty, Strings, m => m.actionType);
            simpleItemsGroup.CreateBinding(GroupBox.HeaderProperty, Strings, m => m.actionSimpleItems);
        }

        private void FixModel(Model model) {
            model.supportedActions = model.supportedActions ?? new SupportedActions();
            model.supportedActions.ActionContentSchemaLocation = model.supportedActions.ActionContentSchemaLocation ?? new string[0];
            model.supportedActions.ActionDescription = model.supportedActions.ActionDescription ?? new ActionConfigDescription[0];
            foreach (var actionDesc in model.supportedActions.ActionDescription) {
                actionDesc.ParameterDescription = actionDesc.ParameterDescription ?? new ItemListDescription();
                actionDesc.ParameterDescription.simpleItemDescription = actionDesc.ParameterDescription.simpleItemDescription ?? new ItemListDescription.SimpleItemDescription[0];
                actionDesc.ParameterDescription.elementItemDescription = actionDesc.ParameterDescription.elementItemDescription ?? new ItemListDescription.ElementItemDescription[0];
            }

            model.action = model.action ?? new Action1();
            model.action.Configuration = model.action.Configuration ?? new ActionConfiguration();
            model.action.Configuration.Type = model.action.Configuration.Type ?? model.supportedActions.ActionDescription.First().Name;
            model.action.Configuration.Parameters = model.action.Configuration.Parameters ?? new ItemList();
            model.action.Configuration.Parameters.simpleItem = model.action.Configuration.Parameters.simpleItem ?? new ItemList.SimpleItem[0];
            model.action.Configuration.Parameters.elementItem = model.action.Configuration.Parameters.elementItem ?? new ItemList.ElementItem[0];

        }

        private void FillSimpleItems(Model model) {
            var actionDesc = model.supportedActions.ActionDescription.First(ad => ad.Name == model.action.Configuration.Type);

            var simpleItemDescs = actionDesc.ParameterDescription.simpleItemDescription.ToDictionary(sid => sid.name);
            var defaultValues = simpleItemDescs.Values.ToDictionary(sid => sid.name, sid => XmlExtensions.ConvertXSValue(sid.type, null));
            simpleItemsValue.InitValues(defaultValues);

            
            var newValues = model.action.Configuration.Parameters.simpleItem.ToDictionary(
					si => si.name,
					si => XmlExtensions.ConvertXSValue(simpleItemDescs[si.name].type, si.value)
				);
            simpleItemsValue.SetValues(newValues);

            model.action.Configuration.Parameters.simpleItem = simpleItemDescs.Values.Select(sid => new ItemList.SimpleItem() { name = sid.name } ).ToArray();
        }

        private void GetSimpleItems(Model model) {
            var values = simpleItemsValue.GetValues();
            if (model.action.Configuration.Parameters != null && model.action.Configuration.Parameters.simpleItem != null) {
                foreach (var si in model.action.Configuration.Parameters.simpleItem) {
                    object value;
                    if (values.TryGetValue(si.name, out value))
                        si.value = XmlExtensions.ConvertBackXSValue(value);
                }
            }
        }

        

        
	}
}
