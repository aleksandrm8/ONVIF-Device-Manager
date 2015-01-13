using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace utils {
	public static class SynchronizationContextExtensions {
		public static void Post(this SynchronizationContext synctx, Action act){
			synctx.Post(_ => act(), null);
		}
		public static void Post<TArg>(this SynchronizationContext synctx, Action<TArg> act, TArg arg) {
			synctx.Post(_ => act(arg), null);
		}
		public static void Post<TArg1, TArg2>(this SynchronizationContext synctx, Action<TArg1, TArg2> act, TArg1 arg1, TArg2 arg2) {
			synctx.Post(_ => act(arg1, arg2), null);
		}
		public static void Post<TArg1, TArg2, TArg3>(this SynchronizationContext synctx, Action<TArg1, TArg2, TArg3> act, TArg1 arg1, TArg2 arg2, TArg3 arg3) {
			synctx.Post(_ => act(arg1, arg2, arg3), null);
		}
		public static Task<TResult> SendAsync<TResult>(this SynchronizationContext synctx, Func<TResult> func) {
			var taskSource = new TaskCompletionSource<TResult>();
			synctx.Post(_ => {
				try {
					var result = func();
					taskSource.TrySetResult(result);
				} catch (Exception error) {
					taskSource.TrySetException(error);
				}
			}, null);
			return taskSource.Task;
		}
		public static Task SendAsync(this SynchronizationContext synctx, Action act) {
			var taskSource = new TaskCompletionSource<object>();
			synctx.Post(_ => {
				try {
					act();
					taskSource.TrySetResult(null);
				} catch (Exception error) {
					taskSource.TrySetException(error);
				}
			}, null);
			return taskSource.Task;
		}
	}
}
