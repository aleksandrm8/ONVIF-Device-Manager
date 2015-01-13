using System;
using System.Reactive.Disposables;
using System.Windows;
using utils;
using System.Reactive.Concurrency;
using System.Reactive.Threading;

namespace odm.ui.activities {
	public interface IViewPresenter {
		IDisposable ShowView(FrameworkElement view);
	}
	public static class ViewPresenter {
		private class DelegatePresenter : IViewPresenter {
			Func<FrameworkElement, IDisposable> showView;
			IScheduler scheduler;
			public DelegatePresenter(Func<FrameworkElement, IDisposable> showView, IScheduler scheduler) {
				if (showView == null) {
					throw new ArgumentNullException("showView");
				} 
				this.showView = showView;
				this.scheduler = scheduler;
			}
			public IDisposable ShowView(FrameworkElement view) {
				var disp = new CompositeDisposable();
				disp.Add(scheduler.Schedule(() => {
					disp.Add(showView(view));
				}));
				return Disposable.Create(() => {
					scheduler.Schedule(() => {
						disp.Dispose();
					});
				});
			}
		}
		public static IViewPresenter Create(Func<FrameworkElement, IDisposable> showView){
			var scheduler = new DispatcherScheduler(Application.Current.Dispatcher);
			return new DelegatePresenter(showView, scheduler);
		}
		public static IViewPresenter Defer(Func<IViewPresenter> factory) {
			var scheduler = new DispatcherScheduler(Application.Current.Dispatcher);
			return new DelegatePresenter(view => factory().ShowView(view), scheduler);
		}
	}
	
}
