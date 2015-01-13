using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace utils {
	public interface IDisposable<T> : IDisposable {
		T value { get; }
	}
	public class DisposableExt {
		private class AnonymousDisposableWithFinalizer<T> : IDisposable<T> {
			private Action<bool> disposeAction = null;
			public T value { get; private set; }
			public AnonymousDisposableWithFinalizer(T value, Action<bool> disposeAction) {
				this.value = value;
				this.disposeAction = disposeAction;
			}
			public virtual void Dispose(bool disposing) {
				var act = Interlocked.Exchange(ref disposeAction, null);
				if (act != null) {
					value = default(T);
					act(disposing);
				}
			}
			public void Dispose() {
				Dispose(true);
			}
			~AnonymousDisposableWithFinalizer() {
				Dispose(false);
			}
		}

		private class AnonymousDisposable<T> : IDisposable<T> {
			private Action disposeAction = null;
			public T value { get; private set; }
			public AnonymousDisposable(T value, Action disposeAction) {
				this.value = value;
				this.disposeAction = disposeAction;
			}
			public void Dispose() {
				var act = Interlocked.Exchange(ref disposeAction, null);
				if (act != null) {
					value = default(T);
					act();
				}
			}
		}

		public static IDisposable<T> CreateWithFinalizer<T>(T value, Action<bool> disposeAction) {
			return new AnonymousDisposableWithFinalizer<T>(value, disposeAction);
		}
		public static IDisposable<T> Create<T>(T value, Action disposeAction) {
			return new AnonymousDisposable<T>(value, disposeAction);
		}
	}

	public sealed class SerialDisposable<T>:IDisposable where T:IDisposable{
		private T m_instance = default(T);
		private object sync = new object();
		public bool isDisposed {get; private set;}
		public T instance {
			get {
				return m_instance;
			}
			set {
				IDisposable toDispose = null;
				lock (sync) {
					if (isDisposed) {
						toDispose = value;
					} else {
						toDispose = m_instance;
						m_instance = value;
					}
				}
				if (toDispose != null) {
					toDispose.Dispose();
				}
			}
		}

		public void Dispose() {
			IDisposable toDispose = null;
			lock (sync) {
				isDisposed = true;
				toDispose = m_instance;
				m_instance = default(T);
			}
			if (toDispose != null) {
				toDispose.Dispose();
			}
		}
	}
}
