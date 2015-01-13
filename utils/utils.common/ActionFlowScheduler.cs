using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace utils {

	public class ActionFlowScheduler : IScheduler {
		protected ActionFlow m_actionFlow;

		public ActionFlowScheduler(ActionFlow actionFlow) {
			if (actionFlow == null) {
				throw new ArgumentNullException("actionFlow");
			}
			m_actionFlow = actionFlow;
		}

		public DateTimeOffset Now {
			get {
				return DateTime.Now;
			}
		}

		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action) {
			SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
			var timer = new Timer(t => {
				m_actionFlow.Invoke(() => {
					if (!disposable.IsDisposed) {
						disposable.Disposable = action(this, state);
					}
				});
			});
			timer.Change(dueTime, TimeSpan.FromMilliseconds(-1));
			return Disposable.Create(() => {
				disposable.Dispose();
				timer.Dispose();
			});	
		}

		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action) {
			SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
			m_actionFlow.Invoke(()=>{
				if (!disposable.IsDisposed) {
					disposable.Disposable = action(this, state);
				}
			});
			return disposable;
		}


		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action) {
			throw new NotImplementedException();
		}

	}
}


