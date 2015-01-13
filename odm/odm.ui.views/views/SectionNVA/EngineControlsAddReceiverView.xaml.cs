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
using utils;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for EngineControlsInputModifyView.xaml
	/// </summary>
	public partial class EngineControlsAddReceiverView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineControlsAddReceiverView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion Activity definition

		CompositeDisposable disposables = new CompositeDisposable();
		//Model model;

		public EngineControlsAddReceiverView() {
			InitializeComponent();
		}

		public void Init(Model model) {
			//inputsList = new List<InputReceiverPair>();

			//OnCompleted += () => {
			//    disposables.Dispose();
			//};
			//this.model = model;

			//InitializeComponent();

			//int count = 0;
			//if (model.inputs != null)
			//    count = model.inputs.Count();
			//if (model.receivers != null)
			//    count = count > model.receivers.Count() ? count : model.receivers.Count();

			//for (int i = 0; i < count; i++) {
			//    Receiver rec = null;
			//    AnalyticsEngineInput inp = null;

			//    if (model.receivers != null && model.receivers.Count() > i)
			//        rec = model.receivers[i];

			//    if (model.inputs != null && model.inputs.Count() > i)
			//        inp = model.inputs[i];

			//    inputsList.Add(new InputReceiverPair(rec, inp));
			//}
			//inputs.ItemsSource = inputsList;

			//FinishCommand = new DelegateCommand(
			//    () => Success(new Result.Finish(model)),
			//    () => true
			//);
			//btnFinish.Command = FinishCommand;

			//ModifyCommand = new DelegateCommand(
			//    () => Success(new Result.Modify(null)),
			//    () => true
			//);
			//btnModify.Command = ModifyCommand;

			//AbortCommand = new DelegateCommand(
			//    () => Success(new Result.Abort()),
			//    () => true
			//);
			//btnAbort.Command = AbortCommand;

		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
