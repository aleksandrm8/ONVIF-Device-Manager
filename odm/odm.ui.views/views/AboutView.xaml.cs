using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using utils;
using System.Windows.Documents;
using System.Diagnostics;
using Microsoft.FSharp.Core;
using Microsoft.Practices.Prism.Commands;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class AboutView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Unit> Show(IUnityContainer container) {
			return container.StartViewActivity<Unit>(context => {
				var view = new AboutView(context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private IActivityContext<Unit> context;
		private bool completed = false;
		private CompositeDisposable disposables = new CompositeDisposable();

		public LocalTitles Titles { get { return LocalTitles.instance; } }
		public CommonApplicationStrings strings { get { return CommonApplicationStrings.instance; } }
		public LocalButtons buttons { get { return LocalButtons.instance; } }


		public AboutView(IActivityContext<Unit> context) {
			this.context = context;
			InitializeComponent();

			closeButton.CreateBinding(Button.ContentProperty, buttons, x => x.close);
			groupBox1.CreateBinding(GroupBox.HeaderProperty, strings, x => x.contactInfo);
			commonCaption.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutCommon);
			russCaption.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutRus);
			belarusCaption.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutBelarus);
			russCaptionPhone.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutRusPhone);
			belarusCaptionPhone.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutBelarusPhone);
			this.CreateBinding(NavigationContext.TitleProperty, Titles, x => x.about);

			closeButton.Command = new DelegateCommand(
				() => Success(),
				() => true
			);

		}

		private void HandleRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
			var hl = sender as Hyperlink;
			if (hl != null) {
				string navigateUri = hl.NavigateUri.ToString();
				// if the URI somehow came from an untrusted source, make sure to
				// validate it before calling Process.Start(), e.g. check to see
				// the scheme is HTTP, etc.
				Process.Start(new ProcessStartInfo(navigateUri));
				e.Handled = true;
			}
		}

		private void CompleteWith(Action cont) {
			Dispatcher.BeginInvoke(() => {
				if (!completed) {
					completed = true;
					cont();
					OnCompleted();
					disposables.Dispose();
				}
			});
		}

		protected virtual void OnCompleted() {
			//activity has been completed
		}

		public void Success() {
			CompleteWith(() => {
				context.Success(null);
			});
		}

		public void Dispose() {
			CompleteWith(() => {
				context.Success(null);
			});
		}

	}
}
