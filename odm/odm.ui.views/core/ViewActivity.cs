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

using odm.infra;


namespace odm.ui.activities {
	
	//public interface IActivityContext<TResult> {
	//    IUnityContainer container { get; }
	//    void Success(TResult result);
	//    void Error(Exception error);
	//    void Cancel();
	//    IDisposable RegisterCancellationCallback(Action callback);
	//}
	
	//public static class ViewActivity {
	//    private class ActivityContext<TModel, TResult> : IActivityContext<TModel, TResult> {
	//        private TModel m_model;
	//        private Action<TResult> m_success;
	//        private Action<Exception> m_error;
	//        private IUnityContainer m_container;

	//        public ActivityContext(IUnityContainer container, TModel model, Action<TResult> success, Action<Exception> error) {
	//            m_container = container;
	//            m_model = model;
	//            m_success = success;
	//            m_error = error;
	//        }

	//        public TModel model { get { return m_model; } }
	//        public void Success(TResult result) { m_success(result); }
	//        public void Error(Exception error) { m_error(error); }
	//        public IUnityContainer container { get { return m_container; } }
	//    }
	//    public static FSharpAsync<TResult> Create<TModel, TResult>(IUnityContainer container, TModel model, Func<IActivityContext<TModel, TResult>, IDisposable> showView) {
	//        var scheduler = new DispatcherScheduler(Application.Current.Dispatcher);
	//        return Apm.Defer(() => {
	//            return Apm.Create<TResult>((success, error) => {
	//                var disp = new SingleAssignmentDisposable();
	//                var context = new ActivityContext<TModel, TResult>(
	//                    container, model, 
	//                    res=>{
	//                        disp.Dispose();
	//                        success(res);
	//                    }, 
	//                    err=>{
	//                        disp.Dispose();
	//                        error(err);
	//                    }
	//                );
	//                disp.Disposable = showView(context);
	//                return disp;
	//            });
	//        }, scheduler);
	//    }
	//}
}
