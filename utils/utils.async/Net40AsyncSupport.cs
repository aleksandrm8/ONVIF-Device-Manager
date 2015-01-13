#if NET40
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
namespace System.Runtime.CompilerServices {
	public interface INotifyCompletion {
		void OnCompleted(Action continuation);
	}
	public interface ICriticalNotifyCompletion : INotifyCompletion {
		void UnsafeOnCompleted(Action continuation);
	}
	public interface IAsyncStateMachine {
		void MoveNext();
		void SetStateMachine(IAsyncStateMachine stateMachine);
	}
	public struct AsyncVoidMethodBuilder {
		public static AsyncVoidMethodBuilder Create() {
			return new AsyncVoidMethodBuilder();
		}

		public void SetException(Exception exception) {
			throw exception;
		}

		public void SetResult() {
		}

		public void SetStateMachine(IAsyncStateMachine stateMachine) {
			// Should not get called as we don't implement the optimization that this method is used for.
			throw new NotImplementedException();
		}

		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine {
			stateMachine.MoveNext();
		}

		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : INotifyCompletion
			where TStateMachine : IAsyncStateMachine {
			awaiter.OnCompleted(stateMachine.MoveNext);
		}

		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : ICriticalNotifyCompletion
			where TStateMachine : IAsyncStateMachine {
			awaiter.OnCompleted(stateMachine.MoveNext);
		}
	}
	public struct AsyncTaskMethodBuilder {
		TaskCompletionSource<object> tcs;

		public Task Task { get { return tcs.Task; } }

		public static AsyncTaskMethodBuilder Create() {
			AsyncTaskMethodBuilder b;
			b.tcs = new TaskCompletionSource<object>();
			return b;
		}

		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine {
			stateMachine.MoveNext();
		}

		public void SetStateMachine(IAsyncStateMachine stateMachine) {
			// Should not get called as we don't implement the optimization that this method is used for.
			throw new NotImplementedException();
		}

		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : INotifyCompletion
			where TStateMachine : IAsyncStateMachine {
			awaiter.OnCompleted(stateMachine.MoveNext);
		}

		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : ICriticalNotifyCompletion
			where TStateMachine : IAsyncStateMachine {
			awaiter.OnCompleted(stateMachine.MoveNext);
		}

		public void SetResult() {
			tcs.SetResult(null);
		}

		public void SetException(Exception exception) {
			tcs.SetException(exception);
		}
	}

	public struct AsyncTaskMethodBuilder<T> {
		TaskCompletionSource<T> tcs;

		public Task<T> Task { get { return tcs.Task; } }

		public static AsyncTaskMethodBuilder<T> Create() {
			AsyncTaskMethodBuilder<T> b;
			b.tcs = new TaskCompletionSource<T>();
			return b;
		}

		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine {
			stateMachine.MoveNext();
		}

		public void SetStateMachine(IAsyncStateMachine stateMachine) {
			// Should not get called as we don't implement the optimization that this method is used for.
			throw new NotImplementedException();
		}

		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : INotifyCompletion
			where TStateMachine : IAsyncStateMachine {
			awaiter.OnCompleted(stateMachine.MoveNext);
		}

		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : ICriticalNotifyCompletion
			where TStateMachine : IAsyncStateMachine {
			AwaitOnCompleted(ref awaiter, ref stateMachine);
		}

		public void SetResult(T result) {
			tcs.SetResult(result);
		}

		public void SetException(Exception exception) {
			tcs.SetException(exception);
		}
	}
}

namespace System.Threading.Tasks {
	public static class TaskExtensions {
		public static TaskAwaiter GetAwaiter(this Task task) {
			return new TaskAwaiter(task);
		}

		public static TaskAwaiter<T> GetAwaiter<T>(this Task<T> task) {
			return new TaskAwaiter<T>(task);
		}
	}

	public struct TaskAwaiter : INotifyCompletion {
		readonly Task task;

		public TaskAwaiter(Task task) {
			this.task = task;
		}

	
		public bool IsCompleted {
			get { return task.IsCompleted; }
		}

		public void OnCompleted(Action continuation) {
			TaskScheduler scheduler;
			if (SynchronizationContext.Current == null) {
				scheduler = TaskScheduler.Default;
			} else {
				scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			}
			task.ContinueWith(t=>continuation(), scheduler);
		}

		public void GetResult() {
			try {
				task.Wait();
			} catch (AggregateException ex) {
				throw ex.InnerExceptions[0];
			}
		}
	}

	public struct TaskAwaiter<T> : INotifyCompletion {
		readonly Task<T> task;

		public TaskAwaiter(Task<T> task) {
			this.task = task;
		}

		public bool IsCompleted {
			get { return task.IsCompleted; }
		}

		public void OnCompleted(Action continuation) {
			TaskScheduler scheduler;
			if (SynchronizationContext.Current == null) {
				scheduler = TaskScheduler.Default;
			} else {
				scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			}
			task.ContinueWith(t => continuation(), scheduler);
		}

		public T GetResult() {
			try {
				return task.Result;
			} catch (AggregateException ex) {
				throw ex.InnerExceptions[0];
			}
		}
	}
}
#endif