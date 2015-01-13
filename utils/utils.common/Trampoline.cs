using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace utils {
	public class Trampoline_ {
		bool acquired = false;
		Queue<Action> queue = new Queue<Action>();
		void ProcessQueue() {
			Action act;
			while (true) {
				lock (queue) {
					if (queue.Count <= 0) {
						acquired = false;
						return;
					}
					act = queue.Dequeue();
				}
				try {
					act();
				} catch (Exception err) {
					dbg.Error(err);
				}
			}
		}
		public void Invoke(Action act) {
			if (act == null) {
				return;
			}
			var acquirer = false;
			lock (queue) {
				if (!acquired) {
					acquired = true;
					acquirer = true;
				} else {
					queue.Enqueue(act);
				}
			}
			if (acquirer) {
				try {
					act();
				} catch (Exception err) {
					dbg.Error(err);
				}
				act = null;
				ProcessQueue();
			}
		}
	}
}
