using System;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using utils;

namespace odm.ui.activities {
	public partial class InfoView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, string message) {
			var model = new Model(message);
			return container.StartViewActivity<Result>(context => {
				var view = new InfoView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private void Init(Model model) {
			InitializeComponent();
			okButton.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.close);
			okButton.Command = new DelegateCommand(
				() => Success(new Result.Ok()),
				() => true
			);
			message.Text = model.message;
		}

		public void Dispose() {
			Cancel();
		}
	}
}
