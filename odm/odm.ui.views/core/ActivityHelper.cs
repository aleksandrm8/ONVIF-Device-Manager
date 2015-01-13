using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using odm.ui.activities;
using odm.player;
using onvif.services;
using utils;
using System.Windows.Controls;
using System.Reactive.Disposables;
using odm.infra;
using Microsoft.FSharp.Control;
using System.Windows;

namespace odm.ui.core {

	public static class ActivityExtensions {
		static IUnityContainer CreateActivityContext(Action<FrameworkElement> setView, IUnityContainer parentContext) {
			var container = parentContext.CreateChildContainer();

			//create & register view presenter
			var presenter = ViewPresenter.Create(view => {
				setView(view);
				return Disposable.Create(() => {
					//setView(null);
				});
			});
			container.RegisterInstance<IViewPresenter>(presenter);

			return container;
		}

		static IUnityContainer CreateActivityContext(ContentControl contentControl, IUnityContainer parentContext) {
			return CreateActivityContext(view => { contentControl.Content = view; }, parentContext);
		}

		static IUnityContainer CreateActivityContext(Decorator decorator, IUnityContainer parentContext) {
			return CreateActivityContext(view => { decorator.Child = view; }, parentContext);
		}


		public static IDisposable RunChildActivity<TResult>(this IUnityContainer parentContext, ContentControl contentControl, Func<IUnityContainer, FSharpAsync<TResult>> activity) {
			var childContext = CreateActivityContext(contentControl, parentContext);
			return activity(childContext)
				.Subscribe(x => {

				}, err => {
					dbg.Error(err);
				});
		}

		public static IDisposable RunChildActivity<TModel, TResult>(this IUnityContainer parentContext, ContentControl contentControl, TModel model, Func<IUnityContainer, TModel, FSharpAsync<TResult>> activity) {
			var childContext = CreateActivityContext(contentControl, parentContext);
			return activity(childContext, model)
				.Subscribe(x => {

				}, err => {
					dbg.Error(err);
				});
		}

		public static IDisposable RunChildActivity<TArg1, TArg2, TResult>(this IUnityContainer parentContext, ContentControl contentControl, TArg1 arg1, TArg2 arg2, Func<IUnityContainer, TArg1, TArg2, FSharpAsync<TResult>> activity) {
			var childContext = CreateActivityContext(contentControl, parentContext);
			return activity(childContext, arg1, arg2)
				.Subscribe(x => {

				}, err => {
					dbg.Error(err);
				});
		}

		public static IDisposable RunChildActivity<TArg1, TArg2, TArg3, TResult>(this IUnityContainer parentContext, ContentControl contentControl, TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<IUnityContainer, TArg1, TArg2, TArg3, FSharpAsync<TResult>> activity) {
			var childContext = CreateActivityContext(contentControl, parentContext);
			return activity(childContext, arg1, arg2, arg3)
				.Subscribe(x => {

				}, err => {
					dbg.Error(err);
				});
		}

		public static IDisposable RunChildActivity<TResult>(this IUnityContainer parentContext, Decorator decorator, Func<IUnityContainer, FSharpAsync<TResult>> activity) {
			var childContext = CreateActivityContext(decorator, parentContext);
			return activity(childContext)
				.Subscribe(x => {

				}, err => {
					dbg.Error(err);
				});
		}

		public static IDisposable RunChildActivity<TModel, TResult>(this IUnityContainer parentContext, Decorator decorator, TModel model, Func<IUnityContainer, TModel, FSharpAsync<TResult>> activity) {
			var childContext = CreateActivityContext(decorator, parentContext);
			return activity(childContext, model)
				.Subscribe(x => {

				}, err => {
					dbg.Error(err);
				});
		}

		public static IDisposable RunChildActivity<TArg1, TArg2, TResult>(this IUnityContainer parentContext, Decorator decorator, TArg1 arg1, TArg2 arg2, Func<IUnityContainer, TArg1, TArg2, FSharpAsync<TResult>> activity) {
			var childContext = CreateActivityContext(decorator, parentContext);
			return activity(childContext, arg1, arg2)
				.Subscribe(x => {

				}, err => {
					dbg.Error(err);
				});
		}

		public static IDisposable RunChildActivity<TArg1, TArg2, TArg3, TResult>(this IUnityContainer parentContext, Decorator decorator, TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<IUnityContainer, TArg1, TArg2, TArg3, FSharpAsync<TResult>> activity) {
			var childContext = CreateActivityContext(decorator, parentContext);
			return activity(childContext, arg1, arg2, arg3)
				.Subscribe(x => {

				}, err => {
					dbg.Error(err);
				});
		}
	}


	public class ActivityHelper: IDisposable {
		public ActivityHelper() {
			disposables = new CompositeDisposable();
		}
		CompositeDisposable disposables;
		VideoBuffer vidBuff;

		IUnityContainer CreateActivityContext(IUnityContainer container, ContentControl UIelement) {
			var childContainer = container.CreateChildContainer();

			//create & register view presenter
			var presenter = ViewPresenter.Create(view => {
				UIelement.Content = view;
				return Disposable.Create(() => {
					//UIelement.Content = null;
				});
			});
			childContainer.RegisterInstance<IViewPresenter>(presenter);

			return childContainer;
		}

		public void VideoStartup(IVideoInfo iVideo, IUnityContainer container, ContentControl UIelement) {
			//vidBuff = new VideoBuffer((int)iVideo.Resolution.Width, (int)iVideo.Resolution.Height);

			//var newContainer = CreateActivityContext(container, UIelement);
			//var playerAct = container.Resolve<IVideoPlayerActivity>();

			//var model = new VideoPlayerActivityModel(
			//    profileToken: this.context.model.profToken,
			//    showStreamUrl: false,
			//    streamSetup: new StreamSetup() {
			//        Stream = StreamType.RTPUnicast,
			//        Transport = new Transport() {
			//            Protocol = AppDefaults.visualSettings.Transport_Type,
			//            Tunnel = null
			//        }
			//    }
			//);

			//disposables.Add(playerAct
			//    .Run(newContainer, model)
			//    .Subscribe(x => {
			//    }, err => {
			//        dbg.Error(err);
			//    }));
		}

		public void Dispose() {
			if (vidBuff != null)
				vidBuff.Dispose();
			disposables.Dispose();
		}
	}
}
