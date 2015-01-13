using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace utils {
	public class Awaiter<TResult> : IAwaiter<TResult>, IAwaitable<TResult> {
		#region State
		abstract class State {
			private State() { }
#if NET45
			public abstract T Match<T>(
				Func<T> started,
				Func<Queue<Action>, T> subscribed,
				Func<TResult, T> succeeded,
				Func<ExceptionDispatchInfo, T> failed
			);
			public abstract void Match(
				Action started,
				Action<Queue<Action>> subscribed,
				Action<TResult> succeeded,
				Action<ExceptionDispatchInfo> failed
			);
#else
			public abstract T Match<T>(
				Func<T> started,
				Func<Queue<Action>, T> subscribed,
				Func<TResult, T> succeeded,
				Func<Exception, T> failed
			);
			public abstract void Match(
				Action started,
				Action<Queue<Action>> subscribed,
				Action<TResult> succeeded,
				Action<Exception> failed
			);
#endif
			public sealed class Strated : State {
				public Strated() { }
#if NET45
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<TResult, T> succeeded, Func<ExceptionDispatchInfo, T> failed) {
					return started();
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action<TResult> succeeded, Action<ExceptionDispatchInfo> failed) {
					started();
				}
#else
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<TResult, T> succeeded, Func<Exception, T> failed) {
					return started();
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action<TResult> succeeded, Action<Exception> failed) {
					started();
				}
#endif
			}
			public sealed class Subscribed : State {
				private Queue<Action> awaiters = new Queue<Action>();
				public Subscribed(Action continuation) {
					awaiters.Enqueue(continuation);
				}
#if NET45
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<TResult, T> succeeded, Func<ExceptionDispatchInfo, T> failed) {
					return subscribed(awaiters);
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action<TResult> succeeded, Action<ExceptionDispatchInfo> failed) {
					subscribed(awaiters);
				}
#else
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<TResult, T> succeeded, Func<Exception, T> failed) {
					return subscribed(awaiters);
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action<TResult> succeeded, Action<Exception> failed) {
					subscribed(awaiters);
				}
#endif
			}
			public sealed class Succeeded : State {
				private TResult result;
				public Succeeded(TResult result) {
					this.result = result;
				}
#if NET45
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<TResult, T> succeeded, Func<ExceptionDispatchInfo, T> failed) {
					return succeeded(result);
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action<TResult> succeeded, Action<ExceptionDispatchInfo> failed) {
					succeeded(result);
				}
#else
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<TResult, T> succeeded, Func<Exception, T> failed) {
					return succeeded(result);
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action<TResult> succeeded, Action<Exception> failed) {
					succeeded(result);
				}

#endif
			}

			public sealed class Failed : State {
#if NET45
				private ExceptionDispatchInfo error;
				public Failed(ExceptionDispatchInfo error) {
					this.error = error;
				}
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<TResult, T> succeeded, Func<ExceptionDispatchInfo, T> failed) {
					return failed(error);
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action<TResult> succeeded, Action<ExceptionDispatchInfo> failed) {
					failed(error);
				}
#else
				private Exception error;
				public Failed(Exception error) {
					this.error = error;
				}
				public override T Match<T>(Func<T> started, Func<Queue<Action>, T> subscribed, Func<TResult, T> succeeded, Func<Exception, T> failed) {
					return failed(error);
				}
				public override void Match(Action started, Action<Queue<Action>> subscribed, Action<TResult> succeeded, Action<Exception> failed) {
					failed(error);
				}
#endif
				
			}
		}
		#endregion
		State state = new State.Strated();

		public bool IsCompleted {
			get {
				return state.Match(
					started: () => false,
					subscribed: awaiters => false,
					succeeded: result => true,
					failed: error => true
				);
			}
		}

		public TResult GetResult() {
			return state.Match(
				started: () => { throw new InvalidOperationException(); },
				subscribed: awaiters => { throw new InvalidOperationException(); },
				succeeded: result => result,
				failed: exInfo => {
#if NET45
					exInfo.Throw();
					return default(TResult);
#else
					throw new Exception("/"+exInfo.Message, exInfo);
#endif
				}
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
				succeeded: result => {
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
				succeeded: result => false,
				failed: error => false
			);
		}

		public bool CompleteWithSuccess(TResult result) {
			return CompleteWith(new State.Succeeded(result));
		}

		public bool CompleteWithError(Exception error) {
#if NET45
			return CompleteWith(new State.Failed(ExceptionDispatchInfo.Capture(error)));
#else
			return CompleteWith(new State.Failed(error));
#endif
		}

		public bool Cancel() {
#if NET45
			return CompleteWith(new State.Failed(ExceptionDispatchInfo.Capture(new OperationCanceledException())));
#else
			return CompleteWith(new State.Failed(new OperationCanceledException()));
#endif

		}
		public IAwaiter<TResult> GetAwaiter() {
			return this;
		}
	}

}
