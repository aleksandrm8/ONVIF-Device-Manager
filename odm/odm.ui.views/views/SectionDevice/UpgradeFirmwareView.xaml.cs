using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using utils;

namespace odm.ui.activities {
	public static class BackgroundTask {
		private class AnonymousBackgroundTask : NotifyPropertyChangedBase<IBackgroundTask>, IBackgroundTask {

			private BackgroundTaskState m_state = BackgroundTaskState.InProgress;
			public BackgroundTaskState state {
				get { return m_state; }
				set {
					if (m_state != value) {
						m_state = value;
						NotifyPropertyChanged(t => t.state);
					}
				}
			}

			private string m_description = null;
			public string description {
				get { return m_description; }
				set {
					if (m_description != value) {
						m_description = value;
						NotifyPropertyChanged(t => t.description);
					}
				}
			}

			private string m_name = null;
			public string name {
				get { return m_name; }
				set {
					if (m_name != value) {
						m_name = value;
						NotifyPropertyChanged(t => t.name);
					}
				}
			}

			public MultipleAssignmentDisposable m_disposable = new MultipleAssignmentDisposable();
			public IDisposable disposable {
				get { return m_disposable.Disposable; }
				set { m_disposable.Disposable = value; }
			}
			public void Dispose() {
				m_disposable.Dispose();
			}
		}

		public static Func<Action<TResult>, Action<Exception>, IBackgroundTask> CreateForAsync<TResult>(FSharpAsync<TResult> async) {
			return (onSuccess, onError) => {
				var disp = new MultipleAssignmentDisposable();
				var backgroundTask = new AnonymousBackgroundTask();
				backgroundTask.name = "upgarde firmware";
				backgroundTask.state = BackgroundTaskState.InProgress;
				backgroundTask.disposable = Disposable.Create(() => {
					disp.Dispose();
					backgroundTask.state = BackgroundTaskState.Canceled;
				});
				disp.Disposable = async.Subscribe(
					res => {
						onSuccess(res);
						backgroundTask.state = BackgroundTaskState.Completed;
					},
					err => {
						onError(err);
						backgroundTask.state = BackgroundTaskState.Failed;
					}
				);
				return backgroundTask;
			};
		}
	}

	/// <summary>
	/// Interaction logic for UpgradeFirmwareView.xaml
	/// </summary>
	public partial class UpgradeFirmwareView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Func<FSharpAsync<Unit>> activity) {
			var model = new Model(activity: activity);
			return container.StartViewActivity<Result>(context => {
				var view = new UpgradeFirmwareView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private SingleAssignmentDisposable activitySubscription = new SingleAssignmentDisposable();

		public LocalButtons Buttons { get { return LocalButtons.instance; } }
		public LocalMaintenance Strings { get { return LocalMaintenance.instance; } }

		private void Init(Model model) {
			var async = model.activity();
			var taskFact = BackgroundTask.CreateForAsync(async.ObserveOnCurrentDispatcher());
			var backgroundTask = taskFact(
				result => Success(() => {
					var displayCompleteNotification = closeOnFinishCheckBox.IsChecked == false;
					return new Result.Completed(displayCompleteNotification);
				}),
				error => Error(error)
			);
			activitySubscription.Disposable = backgroundTask;
			InitializeComponent();

			NavigationContext.SetTitle(this, Strings.titleupgradingFirmware);
			cancelButton.CreateBinding(Button.ContentProperty, Buttons, s => s.cancel);
			cancelButton.Command = new DelegateCommand(
				() => Success(() => {
					activitySubscription.Dispose();
					return new Result.Canceled();
				}),
				() => true
			);

			backgroundButton.Command = new DelegateCommand(
				() => Success(() => {
					BackgroundTaskManager.tasks.Add(backgroundTask);
					return new Result.Background(activitySubscription);
				}),
				() => true
			);

			backgroundButton.CreateBinding(Button.ContentProperty, Buttons, x => x.background);
			cancelButton.CreateBinding(Button.ContentProperty, Buttons, x => x.cancel);
			message.CreateBinding(TextBlock.TextProperty, Strings, x => x.upgradingFirmware);
			closeOnFinishCheckBox.CreateBinding(CheckBox.ContentProperty, Strings, x => x.autoclose);
		}

		public void Dispose() {
			Success(() => {
				activitySubscription.Dispose();
				return new Result.Canceled();
			});
		}

	}
}
