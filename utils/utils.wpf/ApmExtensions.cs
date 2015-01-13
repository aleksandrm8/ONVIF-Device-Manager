using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Windows.Threading;
using Microsoft.FSharp.Control;
using System.Reactive;
using System.Reactive.Linq;

namespace utils {
	public static class ApmExtensions {
		public static FSharpAsync<T> ObserveOnCurrentDispatcher<T>(this FSharpAsync<T> comp) {
			var synCtx = SynchronizationContext.Current;
			if (synCtx == null) {
				synCtx = new TrampolineSynCtx();
			}
			var scheduler = new SynchronizationContextScheduler(synCtx);
			return comp.ObserveOn(scheduler);
		}
		public static IObservable<T> ObserveOnCurrentDispatcher<T>(this IObservable<T> comp) {
			var synCtx = SynchronizationContext.Current;
			if (synCtx == null) {
				synCtx = new TrampolineSynCtx();
			}
			var scheduler = new SynchronizationContextScheduler(synCtx);
			return comp.ObserveOn(scheduler);
		}
	}
}
