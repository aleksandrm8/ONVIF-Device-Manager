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
using Microsoft.Practices.Unity;
using odm.ui.controls;
using System.Reactive.Disposables;
using Microsoft.FSharp.Control;
using odm.infra;
using odm.player;
using odm.ui.core;
using onvif.services;
using Microsoft.Practices.Prism.Commands;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for EngineAnalyticsView.xaml
	/// </summary>
	public partial class EngineAnalyticsView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineAnalyticsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		public static void Start(IUnityContainer container, ContentColumn content) {//(Model model, IUnityContainer container, ContentColumn content) {
			//var view = new EngineAnalyticsView(model, container);
			var view = new EngineAnalyticsView(container);

			content.Content = view;
		}
		#endregion

		public EngineAnalyticsView(IUnityContainer container) {
			InitializeComponent();
		}

		CompositeDisposable disposables = new CompositeDisposable();

		enum AnalyticType {
			RULE,
			MODULE
		}

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			
			InitializeComponent();

			//var type = activityContext.container.Resolve<AnalyticType>();

		//    switch (type) {
		//        case AnalyticType.MODULE:
		//            if (model.modules != null && model.modules.Count() != 0) {
		//                Array.Sort(model.modules, new ReverseComparer());
		//            }
		//            break;
		//        case AnalyticType.RULE:
		//            if (model.rules != null && model.rules.Count() != 0) {
		//                Array.Sort(model.rules, new ReverseComparer());
		//            }

		//            break;
		//    }


			CreateModuleCommand = new DelegateCommand(
				() => Success(new Result.CreateModule()),
				() => true
			);
			btnCreateModule.Command = CreateModuleCommand;

			DeleteModuleCommand = new DelegateCommand(
				() => 
					Success(new Result.DeleteModule("")),//SelectedModule.Name)),
				() => { return true;}//SelectedModule != null); }
			);
			btnDeleteModule.Command = DeleteModuleCommand;

		//    var configureModuleCommand = new DelegateCommand(
		//        () => {
		//            //TODO: Stub fix for #225 Remove this with plugin functionality
		//            if (last != null)
		//                last.module = SelectedModule;
		//            //
		//            Success(new Result.ConfigureModule(SelectedModule));
		//        },
		//        () => { return (SelectedModule != null); }
		//    );
		//    ConfigureModuleCommand = configureModuleCommand;

			//disposables.Add(this.GetPropertyChangedEvents(
			//    EngineAnalyticsView.SelectedModuleProperty
			//).Subscribe(x => {
			//    deleteModuleCommand.RaiseCanExecuteChanged();
			//    configureModuleCommand.RaiseCanExecuteChanged();
			//}));

			CreateRuleCommand = new DelegateCommand(
				() => Success(new Result.CreateRule()),
				() => true
			);
			btnCreateRule.Command = CreateRuleCommand;

			DeleteRuleCommand = new DelegateCommand(
				() => 
					Success(new Result.DeleteRule("")), //selectedrule.name)),
				() => { return true;}// (selectedrule != null); }
			);
			btnDeleteRule.Command = DeleteRuleCommand;

			//ConfigureRuleCommand = new DelegateCommand(
			//    () => Success(new Result.ConfigureRule(SelectedRule)),
			//    () => { return (SelectedRule != null); }
			//);
			btnConfigureRule.Command = ConfigureRuleCommand;

		//    uiSubscribtions.Add(this.GetPropertyChangedEvents(
		//        AnalyticsView.SelectedRuleProperty
		//    ).Subscribe(x => {
		//        deleteRuleCommand.RaiseCanExecuteChanged();
		//        configureRuleCommand.RaiseCanExecuteChanged();
		//    }));


		//    BindModel(model, type);
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
