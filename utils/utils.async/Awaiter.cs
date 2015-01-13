using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace utils {
	public class Awaiter : IAwaiter, IAwaitable {
		#region State
		abstract class State {
			private State() { }
			public abstract T Match<T>(
				Func<T> started,
				Func<Queue<Action>, T> subscribed,
				Func<T> succeeded,
				Func<Exception, T> failed
			);
			public abstract void Match(
				Action started,
				Action<Queue<Action>> subscribed,
				Action succeeded,
				Action<Exception> failed
			);
			public sealed class Strated : State {
				public Strated() { }
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<T> succeeded, Func<Exception, T> failed) {
					return started();
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action succeeded, Action<Exception> failed) {
					started();
				}
			}
			public sealed class Subscribed : State {
				private Queue<Action> awaiters = new Queue<Action>();
				public Subscribed(Action continuation) {
					awaiters.Enqueue(continuation);
				}
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<T> succeeded, Func<Exception, T> failed) {
					return subscribed(awaiters);
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action succeeded, Action<Exception> failed) {
					subscribed(awaiters);
				}
			}
			public sealed class Succeeded : State {
				public Succeeded() {
				}
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<T> succeeded, Func<Exception, T> failed) {
					return succeeded();
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action succeeded, Action<Exception> failed) {
					succeeded();
				}
			}
			public sealed class Failed : State {
				private Exception error;
				public Failed(Exception error) {
					this.error = error;
				}
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<T> succeeded, Func<Exception, T> failed) {
					return failed(error);
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action succeeded, Action<Exception> failed) {
					failed(error);
				}
			}
		}
		#endregion


		public Awaiter() {}
		void release(object owner) {
			if (owner != null)
				if (owner is IDisposable)
					(owner as IDisposable).Dispose();
			Cancel();
		}
		public Awaiter(CancellationToken ct, object owner) {
			if (ct.IsCancellationRequested) {
				release(owner);
			} else {
				ct.Register(() => {
					release(owner);
				});
			}
		}

		State state = new State.Strated();

		public bool IsCompleted {
			get {
				return state.Match(
					started: () => false,
					subscribed: awaiters => false,
					succeeded: () => true,
					failed: error => true
				);
			}
		}

		public void GetResult() {
			state.Match(
				started: () => { throw new InvalidOperationException(); },
				subscribed: awaiters => { throw new InvalidOperationException(); },
				succeeded: () => { },
				failed: error => { throw error; }
			);
		}

		public void OnCompleted(Action continuation) {
			state.Match(
				started: () => {
					state = new State.Subscribed(continuation);
				},
				subscribed: awaiters => {
					awaiters.Enqueue(continuation);
				},
				succeeded: () => {
					continuation();
				},
				failed: error => {
					continuation();
				}
			);
		}

		bool CompleteWith(State completedState) {
			return state.Match(
				started: () => {
					state = completedState;
					return true;
				},
				subscribed: awaiters => {
					state = completedState;
					while (awaiters.Count > 0) {
						var continuation = awaiters.Dequeue();
						continuation();
					}
					return true;
				},
				succeeded: () => false,
				failed: error => false
			);
		}

		public bool CompleteWithSuccess() {
			return CompleteWith(new State.Succeeded());
		}

		public bool CompleteWithError(Exception error) {
			return CompleteWith(new State.Failed(error));
		}

		public bool Cancel() {
			return CompleteWith(new State.Failed(new OperationCanceledException()));
		}
		public IAwaiter GetAwaiter() {
			return this;
		}
	}

}
