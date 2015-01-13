using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace odm.ui.controls {
	public class BasePropertyControl :UserControl{
		void Localization() { }
		public virtual void ReleaseUnmanaged() { }
		public virtual void ReleaseAll() {
		}
		public Action<Exception, string> onBindingError { get; set; }
		public Action<string> onVideoInitializationError { get; set; }

		protected virtual void VideoOperationError(string message) {
			if (onVideoInitializationError != null) {
				onVideoInitializationError(message);
			}
		}
		protected virtual void BindingError(Exception err, string message) {
			if (onBindingError != null) {
				onBindingError(err, message);
			}
		}
	}
}
