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
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using onvif.services;
using utils;
using System.Reactive.Disposables;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for EngineSettingsView.xaml
	/// </summary>
	public partial class EngineSettingsView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineSettingsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion Activity definition

		public EngineSettingsView() {
			InitializeComponent();
		}

		CompositeDisposable disposables = new CompositeDisposable();

		public void Init(Model model) {
			
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
