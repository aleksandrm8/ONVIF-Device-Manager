using System;
using System.Windows.Threading;

namespace utils {
	public static class DispatcherExtensions {
		public static void EnsureAccess(this DispatcherObject dispatcherObject, Action action) {
			var disp = dispatcherObject.Dispatcher;
			if (!disp.CheckAccess()) {
				disp.BeginInvoke(action);
			}
		}
		public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action) {
			return dispatcher.BeginInvoke(action);
		}
		public static DispatcherOperation BeginInvoke(this DispatcherFrame dispatcherFrame, Action action) {
			return dispatcherFrame.Dispatcher.BeginInvoke(action);
		}
	}
}
