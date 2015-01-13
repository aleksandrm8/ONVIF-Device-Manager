using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Microsoft.Practices.Unity;
using Microsoft.FSharp.Control;
using Unit = Microsoft.FSharp.Core.Unit;

using odm.core;
using odm.infra;
using odm.ui.views;
using odm.ui.viewModels;
using odm.controllers;

namespace odm.ui.activities {
	public class InfoActivity : IActivity<Unit> {
		IUnityContainer container;
		IViewPresenter presenter;
		IScheduler scheduler;
		string message;

        public InfoActivity(IUnityContainer container, string message) {
			this.container = container;
			this.presenter = container.Resolve<IViewPresenter>();
			this.scheduler = new DispatcherScheduler(Application.Current.Dispatcher);
			this.message = message;
		}

		public static FSharpAsync<Unit> Show(IUnityContainer container, string text) {
			return Apm.Defer(() => {
				return new InfoActivity(container, text).Run();
			}, new DispatcherScheduler(Application.Current.Dispatcher));
        }
		public FSharpAsync<Unit> Run() {
			return Apm.Create<Unit>((success, error) => {
				var disp = new SingleAssignmentDisposable();
				Action<Action> CompleteWith = cont => {
					cont();
					disp.Dispose();
				};

				var model = new InfoViewModel(message);
                model.close = () => { success(null); };
				var view = new InfoView(model);
                disp.Disposable = presenter.ShowView(view);
				return disp;
            });
        }
    }
}
