using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

using odm.infra;
using odm.ui.controls;

using utils;
using onvif.services;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for PropertyVideoStreaming.xaml
	/// </summary>
	public partial class AnalyticsSetNameView : UserControl {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new AnalyticsSetNameView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }
		public AnalyticsStrings Strings { get { return AnalyticsStrings.instance; } }

		private CompositeDisposable disposables = new CompositeDisposable();

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
		#endregion Binding
		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.DataContext = model;

			var abortCommand = new DelegateCommand(
				() => Success(new Result.Abort()),
				() => true
			);
			AbortCommand = abortCommand;

			configureCommand = new DelegateCommand(
				() => {
					Success(new Result.Configure(ItemName, AnalyticsType));
				},() => {
					return (ItemName != null) && (ItemName != "") && (AnalyticsType != null);
				}
			);
			ConfigureCommand = configureCommand;

			InitializeComponent();

			BindModel(model);
			Localization();
		}
		
		void BindModel(Model model) {
			if (model.types == null) {
				return;
			}
			var typeItems = model.types.Select(x => NamedValue.Create(x, x.name.Name)).ToArray();
			comboTypes.ItemsSource = typeItems;
			var defType = typeItems.FirstOrDefault();
			AnalyticsType = defType.value;
			comboTypes.SelectedValue = defType;
			comboTypes.SetUpdateTrigger(
				ComboBox.SelectedValueProperty,
				(NamedValue<ConfigDescription> v) => AnalyticsType = v != null ? v.value : null
			);
			ItemName = null;
			nameValue.Text = ItemName;
			nameValue.SetUpdateTrigger(
				TextBox.TextProperty,
				(string v) => ItemName = v
			);
		}

		void Localization() {
			setNameCaption.CreateBinding(Label.ContentProperty, Strings, s => s.setNameCaption);
			setTypeCaption.CreateBinding(Label.ContentProperty, Strings, s => s.setTypeCaption);
			btnAbort.CreateBinding(Button.ContentProperty, ButtonsStrings, s => s.cancel);
			btnConfirm.CreateBinding(Button.ContentProperty, ButtonsStrings, s => s.continue1);
		}

		DelegateCommand configureCommand;
		public ConfigDescription AnalyticsType {
			get { return (ConfigDescription)GetValue(AnalyticsTypeProperty); }
			set { SetValue(AnalyticsTypeProperty, value); }
		}
		public static readonly DependencyProperty AnalyticsTypeProperty =
			DependencyProperty.Register("AnalyticsType", typeof(ConfigDescription), typeof(AnalyticsSetNameView));

		public string ItemName {
			get { return (string)GetValue(ItemNameProperty); }
			set { SetValue(ItemNameProperty, value); }
		}
		public static readonly DependencyProperty ItemNameProperty =
			DependencyProperty.Register("ItemName", typeof(string), typeof(AnalyticsSetNameView), new PropertyMetadata((obj, evarg) => {
				var mod = (AnalyticsSetNameView)obj;
				mod.configureCommand.RaiseCanExecuteChanged();
			}));

		public void Dispose() {
			Cancel();
		}

	}
}
