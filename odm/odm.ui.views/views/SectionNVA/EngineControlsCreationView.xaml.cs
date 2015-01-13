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
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Commands;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for EngineControlsCreationView.xaml
	/// </summary>
	public partial class EngineControlsCreationView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineControlsCreationView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion Activity definition

		public EngineControlsCreationView() {
			InitializeComponent();
		}

		CompositeDisposable disposables = new CompositeDisposable();
		Model model;

		public void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.model = model;
			InitializeComponent();

			if (model.name == null)
				model.name = "control_name";
			valueControlName.CreateBinding(TextBox.TextProperty, model, x => x.name, (m, o) => {
				m.name = o;
			});
			valueVACToken.CreateBinding(TextBox.TextProperty, model, x => x.vacToken);

			FinishCommand = new DelegateCommand(
				() => Success(new Result.Finish(model)),
				() => true
			);
			btnFinish.Command = FinishCommand;

			ConfigureInputsCommand = new DelegateCommand(
				() => Success(new Result.ConfigureInputs(model)),
				() => true
			);
			btnConfigure.Command = ConfigureInputsCommand;

			ConfigureVacCommand = new DelegateCommand(
				() => Success(new Result.ConfigureVac(model)),
				() => true
			);
			btnSelectVAC.Command = ConfigureVacCommand;

			AbortCommand = new DelegateCommand(
				() => Success(new Result.Abort()),
				() => true
			);
			btnAbort.Command = AbortCommand;
			
			Localization();
		}

		void Localization() {
			captionControlName.CreateBinding(TextBlock.TextProperty, LocalEngineControlsUpdating.instance, s => s.controlname);
			captionVAC.CreateBinding(TextBlock.TextProperty, LocalEngineControlsUpdating.instance, s => s.vac);

			btnAbort.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.abort);
			btnConfigure.CreateBinding(Button.ContentProperty, LocalEngineControlsUpdating.instance, s => s.configure);
			btnFinish.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.finish);
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
