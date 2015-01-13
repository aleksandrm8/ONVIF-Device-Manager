using System.Windows.Controls;
using odm.ui.viewModels;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.ui.activities;
using Microsoft.FSharp.Core;
using System.Reactive.Disposables;
using System;
using odm.infra;
using System.Reactive.Concurrency;
using System.Windows;
using System.Reactive.Linq;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class ProgressView : UserControl {

		//#region Activity definition
		//public static FSharpAsync<IDisposable> Show(IUnityContainer container, string message, bool isCalncellabe=false) {
		//    return Apm.Do<IDisposable>(
		//        () => {
		//            var scheduler = new DispatcherScheduler(Application.Current.Dispatcher);
		//            var disp = new CompositeDisposable();
		//            disp.Add(scheduler.Schedule(
		//                TimeSpan.FromSeconds(0.1),
		//                ()=>{
		//                    if (!disp.IsDisposed) {
		//                        var presenter = container.Resolve<IViewPresenter>();
		//                        var view = new ProgressView(container, message);
		//                        //disp.Add(presenter.ShowView(view));
		//                        presenter.ShowView(view);
		//                    }
		//                }
		//            ));
		//            return disp;
		//        }
		//    );
		//}
		//#endregion


		public LocalDevice Strings { get { return LocalDevice.instance; } }

		private void Init(Model model) {
			InitializeComponent();
			messageBlock.Text = model.text;
		}
		public ProgressView(string message){
			Init(new Model(null, message, false));
		}
	}
}
