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
	/// Interaction logic for EngineControlsVACSelectionView.xaml
	/// </summary>
	public partial class EngineControlsVACSelectionView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineControlsVACSelectionView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion Activity definition
		public EngineControlsVACSelectionView() {
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
			
			Localization();
		}

		void Localization() {
			captionDetails.CreateBinding(TextBlock.TextProperty, LocalEngineControlsVACSelection.instance, s=>s.details);
			captionVAC.CreateBinding(TextBlock.TextProperty, LocalEngineControlsVACSelection.instance, s => s.vac);

			btnAbort.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.abort);
			btnSelect.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.select);
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
