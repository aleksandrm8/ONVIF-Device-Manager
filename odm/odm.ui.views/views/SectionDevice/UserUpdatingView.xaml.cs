using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using onvif.services;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for UserCreationView.xaml
	/// </summary>
	public partial class UserUpdatingView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new UserUpdatingView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public delegate void PropertyChanged();

		public PropertyChanged OnRepeatedPasswordChanged;
		private string m_repeatedPassword;
		public string repeatedPassword {
			get { return m_repeatedPassword; }
			set {
				m_repeatedPassword = value;
				if (OnRepeatedPasswordChanged != null) {
					OnRepeatedPasswordChanged();
				}
			}
		}

		public LocalButtons SaveCancel { get { return LocalButtons.instance; } }
		public LocalUserManagement Strings { get { return LocalUserManagement.instance; } }
		public LocalTitles Titles { get { return LocalTitles.instance; } }

		private void Init(Model model) {
			this.DataContext = model;

			var applyCommand = new DelegateCommand(
				() => Success(new Result.Apply(model)),
				() => {
					if (repeatedPassword != model.password) {
						return false;
					}
					return true;
				}
			);
			OnRepeatedPasswordChanged += () => applyCommand.RaiseCanExecuteChanged();

			ApplyCommand = applyCommand;

			var cancelCommand = new DelegateCommand(
				() => Success(new Result.Cancel()),
				() => true
			);
			CancelCommand = cancelCommand;

			repeatedPassword = model.password;

			InitializeComponent();

			userNameCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.nameCaption);
			userNameValue.CreateBinding(TextBox.TextProperty, model, m => m.name);

			passwordCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.passwordCaption);
			repeatPasswordCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.repeatPasswordCaption);
			passwordValue.CreateBinding(
				TextBox.TextProperty, model,
				m => m.password,
				(m, v) => {
					m.password = String.IsNullOrEmpty(v) ? null : v;
					applyCommand.RaiseCanExecuteChanged();
				}
			);

			//repeatPasswordCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.repeatPassword);
			repeatPasswordValue.Text = repeatedPassword;
			repeatPasswordValue.TextChanged += (s, a) => {
				repeatedPassword = String.IsNullOrEmpty(repeatPasswordValue.Text) ? null : repeatPasswordValue.Text;
			};

			roleCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.roleCaption);
			roleValue.ItemsSource = EnumHelper.GetValues<UserLevel>().Where(v => v != UserLevel.anonymous && v != UserLevel.extended);
			roleValue.CreateBinding(ComboBox.SelectedValueProperty, model, m => m.level, (m, v) => m.level = v);

			applyButton.Command = ApplyCommand;
			applyButton.CreateBinding(Button.ContentProperty, SaveCancel, s => s.apply);

			cancelButton.Command = CancelCommand;
			cancelButton.CreateBinding(Button.ContentProperty, SaveCancel, s => s.cancel);

		}

		public void Dispose() {
			Cancel();
		}

	}
}
