using System;
using System.Reactive.Disposables;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using odm.infra;
using odm.ui.views;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for UserManagementView.xaml
	/// </summary>
	public partial class UserManagementView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new UserManagementView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private CompositeDisposable disposables = new CompositeDisposable();
		public LocalButtons SaveCancel { get { return LocalButtons.instance; } }
		public LocalUserManagement Strings { get { return LocalUserManagement.instance; } }
		public LocalTitles Titles { get { return LocalTitles.instance; } }

		void Localization() {
			createUserButton.CreateBinding(Button.ContentProperty, Strings, x => x.createUserButton);
			deleteUserButton.CreateBinding(Button.ContentProperty, Strings, x => x.deleteUserButton);
			downloadPoliciesButton.CreateBinding(Button.ContentProperty, Strings, x => x.downloadPoliciesButton);
			editPoliciesButton.CreateBinding(Button.ContentProperty, Strings, x => x.editPoliciesButton);
			modifyUserButton.CreateBinding(Button.ContentProperty, Strings, x => x.modifyUserButton);
			uploadPoliciesButton.CreateBinding(Button.ContentProperty, Strings, x => x.uploadPoliciesButton);

			usersCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.usersCaption);
			policiesCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.policiesCaption);
		}

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.DataContext = model;

			var userManagementEventArgs = activityContext.container.Resolve<IUserManagementEventArgs>();

			var createUserCommand = new DelegateCommand(
				() => Success(new Result.CreateUser(model)),
				() => true
			);
			var deleteUserCommand = new DelegateCommand(
				() => Success(new Result.DeleteUser(model)),
				() => model.selection != null
			);
			var modifyUserCommand = new DelegateCommand(
				() => Success(new Result.ModifyUser(model)),
				() => model.selection != null
			);
			disposables.Add(
				model
					.GetPropertyChangedEvents(m => m.selection)
					.Subscribe(v => {
						modifyUserCommand.RaiseCanExecuteChanged();
						deleteUserCommand.RaiseCanExecuteChanged();
					})
			);
			var downloadPoliciesCommand = new DelegateCommand(
				() => {
					var dlg = new SaveFileDialog();
					dlg.FileName = userManagementEventArgs.Manufacturer + "-" + userManagementEventArgs.DeviceModel + "-policies.txt";
					dlg.Filter = "Text files (*.txt) |*.txt|All files (*.*)|*.*";
					if (dlg.ShowDialog() == true) {
						Success(new Result.DownloadPolicy(model, dlg.FileName));
					}
				},
				() => true
			);
			var uploadPoliciesCommand = new DelegateCommand(
				() => {
					var dlg = new OpenFileDialog();
					if (dlg.ShowDialog() == true) {
						Success(new Result.UploadPolicy(model, dlg.FileName));
					}
				},
				() => true
			);
			InitializeComponent();

			Localization();

			usersList.ItemsSource = model.users;
			usersList.CreateBinding(
				ListBox.SelectedValueProperty, model,
				m => m.selection,
				(m, v) => m.selection = v
			);
			createUserButton.Command = createUserCommand;
			modifyUserButton.Command = modifyUserCommand;
			deleteUserButton.Command = deleteUserCommand;
			uploadPoliciesButton.Command = uploadPoliciesCommand;
			downloadPoliciesButton.Command = downloadPoliciesCommand;
		}

		public void Dispose() {
			Cancel();
		}

		private void usersList_SelectionChanged(object sender, SelectionChangedEventArgs e) {

		}

	}
}
