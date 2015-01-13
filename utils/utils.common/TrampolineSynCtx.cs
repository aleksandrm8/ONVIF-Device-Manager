using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace utils {
	//TODO: current implementation is vulnerable to out-of-band exceptions
	public class TrampolineSynCtx : SynchronizationContext {
		public class UnhandledException : Exception {
			public UnhandledException(Exception error): base("trampoline unhandled exception", error) {
			}
		}
		private bool processing = false;
		Queue<Action> queue = new Queue<Action>();

		public override void Send(SendOrPostCallback callback, object state) {
			log.WriteError("TrampolineSynCtx::Send is deprecated");
			callback(state);
		}

		private void ProcessQueue() {
			
			while (true) {
				Action act;
				lock (queue) {
					if (queue.Count <= 0) {
						processing = false;
						return;
					}
					act = queue.Dequeue();
				}
				act();
			}
		}

		public override void Post(SendOrPostCallback callback, object state) {
			var synctx = SynchronizationContext.Current;
			
			ContextCallback wcallback = _s=>{
				if (synctx == null) {
					SynchronizationContext.SetSynchronizationContext(this);
				} else if (synctx != this) {
					log.WriteError("possibilty of execution flow disorder");
				}
				try{
					callback(_s);
				}catch(Exception err){
					//swallow error
					log.WriteError(new UnhandledException(err));
					dbg.Break();
				}
			};

			var ectx = ExecutionContext.Capture();
			Action act = () => {
				if (ectx != null) {
					ExecutionContext.Run(ectx, wcallback, state);
					ectx.Dispose();
					ectx = null;
				} else {
					//flow suppressed
					log.WriteError("flow was suppressed");
					var origin = SynchronizationContext.Current;
					SynchronizationContext.SetSynchronizationContext(synctx);
					wcallback(state);
					SynchronizationContext.SetSynchronizationContext(origin);
				}
			};

			lock (queue) {
				queue.Enqueue(act);
				if (processing) {
					return;
				}
				processing = true;
			}
			ProcessQueue();
		}
		public override SynchronizationContext CreateCopy() {
			return this;
		}
	}
}
