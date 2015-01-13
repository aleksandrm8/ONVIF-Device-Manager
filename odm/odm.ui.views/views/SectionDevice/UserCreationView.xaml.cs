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
	public partial class UserCreationView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new UserCreationView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public delegate void PropertyChanged();
		
		public PropertyChanged OnRepeatedPasswordChanged;
		private string m_repeatedPassword;
		public string repeatedPassword {
			get {return m_repeatedPassword;}
			set {
				m_repeatedPassword = value;
				if (OnRepeatedPasswordChanged != null) {
					OnRepeatedPasswordChanged();
				}
			}
		}

		public PropertyChanged OnPasswordChanged;
		private string m_password;
		public string password {
			get {return m_password;}
			set {
				m_password = value;
				if (OnPasswordChanged != null) {
					OnPasswordChanged();
				}
			}
		}

		public PropertyChanged OnUserNameChanged;
		private string m_userName;
		public string userName {
			get {return m_userName;}
			set {
				m_userName = value;
				if (OnUserNameChanged != null) {
					OnUserNameChanged();
				}
			}
		}

		public PropertyChanged OnUserLevelChanged;
		private UserLevel m_userLevel;
		public UserLevel userLevel {
			get {return m_userLevel;}
			set {
				m_userLevel = value;
				if (OnUserLevelChanged != null) {
					OnUserLevelChanged();
				}
			}
		}

        public LocalButtons SaveCancel { get { return LocalButtons.instance; } }
        public LocalUserManagement Strings { get { return LocalUserManagement.instance; } }
        public LocalTitles Titles { get { return LocalTitles.instance; } }

		private void Init(Model model) {
			this.DataContext = model;

			var applyCommand = new DelegateCommand(
				() => Success(new Result.Apply(userName, password, userLevel)),
				() => {
					if (repeatedPassword != password) {
						return false;
					}
					if (String.IsNullOrWhiteSpace(userName)) {
						return false;
					}
					if (model.existingUsers != null && model.existingUsers.Contains(userName)) {
						return false;
					}
					return true;
				}
			);
			OnRepeatedPasswordChanged += () => applyCommand.RaiseCanExecuteChanged();
			OnPasswordChanged += () => applyCommand.RaiseCanExecuteChanged();
			OnUserNameChanged += () => applyCommand.RaiseCanExecuteChanged();
			
			ApplyCommand = applyCommand;
			
			var cancelCommand = new DelegateCommand(
				() => Success(new Result.Cancel()),
				() => true
			);
			CancelCommand = cancelCommand;

			password = model.defaultPassword;
			repeatedPassword = model.defaultPassword;
			userName = model.defaultUserName;
			userLevel = model.defaultUserLevel;

			InitializeComponent();

            userNameCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.nameCaption);
			userNameValue.Text = userName;
			userNameValue.TextChanged += (s, a) => userName = userNameValue.Text;

            passwordCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.passwordCaption);
			passwordValue.Text = password;
			passwordValue.TextChanged +=
				(s, a) => password = String.IsNullOrEmpty(passwordValue.Text) ? null : passwordValue.Text;

			//repeatPasswordCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.repeatPassword);
			repeatPasswordValue.Text = repeatedPassword;
			repeatPasswordValue.TextChanged +=
				(s, a) => repeatedPassword = String.IsNullOrEmpty(repeatPasswordValue.Text) ? null : repeatPasswordValue.Text;

			roleCaption.CreateBinding(Label.ContentProperty, this.Strings, s => s.roleCaption);
			roleValue.ItemsSource = EnumHelper.GetValues<UserLevel>().Where(v=>v!= UserLevel.anonymous && v!=UserLevel.extended);
			roleValue.SelectedValue = userLevel;
			roleValue.SelectionChanged += (s, a) => userLevel = (UserLevel)roleValue.SelectedValue;

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
