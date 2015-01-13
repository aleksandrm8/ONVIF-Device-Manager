using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace utils {

	public class ActionFlow {

		bool m_canceled = false;
		ManualResetEvent m_waitEvt = new ManualResetEvent(false);
		bool m_isProcessed = false;
		object m_gate = new object();
		Queue<Action> m_queue = new Queue<Action>();

		public void Exit() {
			Invoke(() => {
				lock (m_gate) {
					m_canceled = true;
					m_queue.Clear();
				}
			});
		}

		public bool Invoke(Action action) {
			lock (m_gate) {
				if (m_canceled) {
					return false;
				}
				m_queue.Enqueue(action);
				if (!m_isProcessed) {
					m_waitEvt.Set();
				}
			}
			return true;
		}

		public void Run() {
			m_canceled = false;
			while (!m_canceled) {
				m_waitEvt.WaitOne();
				Action action = null;
				lock (m_gate) {
					m_isProcessed = true;
					action = m_queue.Dequeue();
				}
				try {
					action();
				} catch(Exception err) {
					//TODO: handle error
					dbg.Error(err);
				}
				lock (m_gate) {
					m_isProcessed = false;
					if (m_queue.Count == 0) {
						m_waitEvt.Reset();
					}
				}
			}
		}
	}
}


