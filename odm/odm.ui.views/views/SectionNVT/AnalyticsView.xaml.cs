using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using onvif.services;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for PropertyVideoStreaming.xaml
	/// </summary>
	public partial class AnalyticsView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new AnalyticsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }
		public PropertyVideoStreamingStrings Strings { get { return PropertyVideoStreamingStrings.instance; } }
		public AnalyticsStrings AnalyticsStrings { get { return AnalyticsStrings.instance; } }

		private CompositeDisposable disposables = new CompositeDisposable();
		private CompositeDisposable uiSubscribtions = new CompositeDisposable();

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
				uiSubscribtions.Dispose();
			};
			this.DataContext = model;

			var type = activityContext.container.Resolve<AnalyticType>();

			switch (type) {
				case AnalyticType.MODULE:
				if (model.modules != null && model.modules.Count() != 0) {
					//Array.Sort(model.modules, new ReverseComparer());
				}
				break;
				case AnalyticType.RULE:
				if (model.rules != null && model.rules.Count() != 0) {
					//Array.Sort(model.rules, new ReverseComparer());
				}

				break;
			}

			var closeCommand = new DelegateCommand(
				() => Success(new Result.Close()),
				() => true
			);
			CloseCommand = closeCommand;

			var createModuleCommand = new DelegateCommand(
				() => Success(new Result.CreateModule()),
				() => true
			);
			CreateModuleCommand = createModuleCommand;

			var deleteModuleCommand = new DelegateCommand(
				() => Success(new Result.DeleteModule(SelectedModule.name)),
				() => { return (SelectedModule != null); }
			);
			DeleteModuleCommand = deleteModuleCommand;

			var configureModuleCommand = new DelegateCommand(
				() => {
					//TODO: Stub fix for #225 Remove this with plugin functionality
					if (last != null)
						last.module = SelectedModule;
					//
					Success(new Result.ConfigureModule(SelectedModule));
				},
				() => { return (SelectedModule != null); }
			);
			ConfigureModuleCommand = configureModuleCommand;

			uiSubscribtions.Add(this.GetPropertyChangedEvents(
				AnalyticsView.SelectedModuleProperty
			).Subscribe(x => {
				deleteModuleCommand.RaiseCanExecuteChanged();
				configureModuleCommand.RaiseCanExecuteChanged();
			}));

			var createRuleCommand = new DelegateCommand(
				() => Success(new Result.CreateRule()),
				() => true
			);
			CreateRuleCommand = createRuleCommand;

			var deleteRuleCommand = new DelegateCommand(
				() => Success(new Result.DeleteRule(SelectedRule.name)),
				() => { return (SelectedRule != null); }
			);
			DeleteRuleCommand = deleteRuleCommand;

			var configureRuleCommand = new DelegateCommand(
				() => Success(new Result.ConfigureRule(SelectedRule)),
				() => { return (SelectedRule != null); }
			);
			ConfigureRuleCommand = configureRuleCommand;

			uiSubscribtions.Add(this.GetPropertyChangedEvents(
				AnalyticsView.SelectedRuleProperty
			).Subscribe(x => {
				deleteRuleCommand.RaiseCanExecuteChanged();
				configureRuleCommand.RaiseCanExecuteChanged();
			}));

			InitializeComponent();

			BindModel(model, type);
		}

		void BindModel(Model model, AnalyticType type) {
			if (type == AnalyticType.RULE) {
				groupModules.Visibility = System.Windows.Visibility.Collapsed;
			}
			if (type == AnalyticType.MODULE) {
				groupRules.Visibility = System.Windows.Visibility.Collapsed;
			}

			//TODO: Stub fix for #225 Remove this with plugin functionality
			last = activityContext.container.Resolve<ILastChangedModule>();
			if (last.module != null) {
				//CompleteWith(() => Success(new Result.ConfigureModule(last.module)));
				Success(new Result.ConfigureModule(last.module));
			}
			//

			modulesList.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(modulesList_MouseDoubleClick);
			rulesList.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(rulesList_MouseDoubleClick);
		}

		#region Binding
		private string GetCfgDisplayName(ConfigurationEntity cfg) {
			if (cfg == null) {
				return null;
			}
			if (cfg.name == null) {
				return cfg.token;
			}
			return cfg.name;
		}
		

        void rulesList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (SelectedRule != null) {
                //CompleteWith(() => Success(new Result.ConfigureRule(SelectedRule)));
				Success(new Result.ConfigureRule(SelectedRule));
            }
        }

        void modulesList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (SelectedModule != null) {
				//TODO: Stub fix for #225 Remove this with plugin functionality
                last.module = SelectedModule;
				//
                //CompleteWith(() => Success(new Result.ConfigureModule(SelectedModule)));
				Success(new Result.ConfigureModule(SelectedModule));
            }
        }
		//TODO: Stub fix for #225 Remove this with plugin functionality
		ILastChangedModule last;
		//
		#endregion Binding

		public class ReverseComparer : IComparer<Config> {
			public int Compare(Config x, Config y) {
				int typecomp = x.type.Name.CompareTo(y.type.Name);
				if (typecomp == 0)
					return x.name.CompareTo(y.name);
				return typecomp;
			}
		}
        public enum AnalyticType {
            RULE,
            MODULE,
			ALL
        }
		
		public Config SelectedModule {
			get { return (Config)GetValue(SelectedModuleProperty); }
			set { SetValue(SelectedModuleProperty, value); }
		}
		public static readonly DependencyProperty SelectedModuleProperty = DependencyProperty.Register("SelectedModule", typeof(Config), typeof(AnalyticsView));

		public Config SelectedRule {
			get { return (Config)GetValue(SelectedRuleProperty); }
			set { SetValue(SelectedRuleProperty, value); }
		}
		public static readonly DependencyProperty SelectedRuleProperty = DependencyProperty.Register("SelectedRule", typeof(Config), typeof(AnalyticsView));

		public void Dispose() {
			Cancel();
		}

	}
	//TODO: Stub fix for #225 Remove this with plugin functionality
	public interface ILastChangedModule {
		string Tag { get; set; }
		Config module { get; set; }
	}
	public class LastChangedModule: ILastChangedModule {
		public LastChangedModule() {
			Tag = "";
		}
		public string Tag { get; set; }
		public Config module { get; set; }
	}
	//
}
