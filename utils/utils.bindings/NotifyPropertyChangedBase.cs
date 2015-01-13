using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Threading;

namespace utils {

	public class NotifyPropertyChangedBase : INotifyPropertyChanged {
		public NotifyPropertyChangedBase(): this(null) {
		}
		public NotifyPropertyChangedBase(IScheduler scheduler) {
			if (scheduler != null) {
				this.scheduler = scheduler;
			} else {
				this.scheduler = Scheduler.Immediate;
			}
		}
		private IScheduler scheduler;
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(String propertyName) {
			if (PropertyChanged != null) {
				scheduler.Schedule(() => {
					var prop_changed = PropertyChanged;
					if (prop_changed != null) {
						prop_changed(this, new PropertyChangedEventArgs(propertyName));
					}
				});
			}
		}
	}

	public class NotifyPropertyChangedBase<T> : INotifyPropertyChanged {
		public NotifyPropertyChangedBase():this(null) {
		}
		public NotifyPropertyChangedBase(IScheduler scheduler) {
			if (scheduler != null) {
				this.scheduler = scheduler;
			} else {
				this.scheduler = Scheduler.Immediate;
			}
		}
		private IScheduler scheduler;
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName) {
			if (PropertyChanged != null) {
				scheduler.Schedule(() => {
					var prop_changed = PropertyChanged;
					if (prop_changed != null) {
						prop_changed(this, new PropertyChangedEventArgs(propertyName));
					}
				});
			}
		}
		protected void NotifyPropertyChanged<TProperty>(Expression<Func<T, TProperty>> expression) {
			var me = expression.Body as MemberExpression;
			dbg.Assert(me != null);
			NotifyPropertyChanged(me.Member.Name);
		}
	}
}
