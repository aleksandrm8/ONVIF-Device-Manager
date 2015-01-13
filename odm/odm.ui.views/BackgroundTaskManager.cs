using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using utils;

namespace odm.ui {
	public enum BackgroundTaskState {
		Completed,
		Canceled,
		InProgress,
		Failed
	}
	public interface IBackgroundTask : INotifyPropertyChanged, IDisposable {
		BackgroundTaskState state { get; }
		string description { get; }
		string name { get; }
	}
	static public class BackgroundTaskManager {
		public static void AddTask(IBackgroundTask task){
			throw new NotImplementedException();
		}
		public static IEnumerable<IBackgroundTask> GetTasks() {
			throw new NotImplementedException();
		}
		private static ObservableCollection<IBackgroundTask> m_tasks = new ObservableCollection<IBackgroundTask>();
		public static ObservableCollection<IBackgroundTask> tasks {
			get {return m_tasks;}
		}
	}
}
