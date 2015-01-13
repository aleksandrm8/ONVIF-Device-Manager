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
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;
using utils;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using System.Reactive.Disposables;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for EngineCotrolInputModifyView.xaml
	/// </summary>
	public partial class EngineControlInputModifyView : UserControl {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineControlInputModifyView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion Activity definition

		CompositeDisposable disposables = new CompositeDisposable();

		public void Init(Model model) {
			InitializeComponent();
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
