using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.activities;
using odm.ui.core;
using odm.ui.views;
using utils;

namespace odm.ui {
	/// <summary>
	/// Interaction logic for ToolBarView.xaml
	/// </summary>
	public partial class BackgroundTasksView : UserControl, IDisposable {
		public static RoutedCommand CloseCommand = new RoutedCommand("Close", typeof(BackgroundTasksView));
		public static RoutedCommand RemoveTaskCommand = new RoutedCommand("RemoveTask", typeof(BackgroundTasksView));
		public static RoutedCommand CancelTaskCommand = new RoutedCommand("CancelTask", typeof(BackgroundTasksView));
		public static RoutedCommand CancelAllRunningTasksCommand = new RoutedCommand("CancelAllRunningTasks", typeof(BackgroundTasksView));
		public static RoutedCommand RemoveAllFinishedTasksCommand = new RoutedCommand("RemoveAllFinishedTasks", typeof(BackgroundTasksView));
		public static RoutedCommand AddUpgradeBatchCommand = new RoutedCommand("AddUpgradeBatch", typeof(BackgroundTasksView));
		public static RoutedCommand AddRestoreBatchCommand = new RoutedCommand("AddRestoreBatch", typeof(BackgroundTasksView));

		#region Activity definition
		public static FSharpAsync<Unit> Show(IUnityContainer container) {
			return container.StartViewActivity<Unit>(context => {
				var view = new BackgroundTasksView(context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private IActivityContext<Unit> context;
		private bool completed = false;
		private CompositeDisposable disposables = new CompositeDisposable();

		public BackgroundTasksView(IActivityContext<Unit> context) {
			this.context = context;
			InitializeComponent();

			Localization();
			IEventAggregator eventAggregator = null;
			try {
				eventAggregator = context.container.Resolve<IEventAggregator>();
			} catch (Exception err) {
				dbg.Error(err);
			}

			CommandBindings.Add(
				new CommandBinding(
					CloseCommand,
					(s, a) => {
						//view.Dispose();
						Success();
					}
				)
			);
			CommandBindings.Add(
				new CommandBinding(
					RemoveTaskCommand,
					(s, a) => {
						var backgroundTask = a.Parameter as IBackgroundTask;
						if (backgroundTask != null) {
							BackgroundTaskManager.tasks.Remove(backgroundTask);
							backgroundTask.Dispose();
						}
					}
				)
			);
			CommandBindings.Add(
				new CommandBinding(
					CancelTaskCommand,
					(s, a) => {
						var backgroundTask = a.Parameter as IBackgroundTask;
						if (backgroundTask != null) {
							backgroundTask.Dispose();
						}
					}
				)
			);
			CommandBindings.Add(
				new CommandBinding(
					AddUpgradeBatchCommand,
					(s, a) => {
						var evarg = new DeviceLinkEventArgs();
						evarg.currentAccount = AccountManager.Instance.CurrentAccount;
						evarg.session = null;
						eventAggregator.GetEvent<UpgradeButchClick>().Publish(true);
					}
				)
			);
			CommandBindings.Add(
				new CommandBinding(
					AddRestoreBatchCommand,
					(s, a) => {
						var evarg = new DeviceLinkEventArgs();
						evarg.currentAccount = AccountManager.Instance.CurrentAccount;
						evarg.session = null;
						eventAggregator.GetEvent<RestoreButchClick>().Publish(true);
					}
				)
			);
			CommandBindings.Add(
				new CommandBinding(
					CancelAllRunningTasksCommand,
					(s, a) => {
						foreach (var t in BackgroundTaskManager.tasks) {
							t.Dispose();
						}
					}
				)
			);
			CommandBindings.Add(
				new CommandBinding(
					RemoveAllFinishedTasksCommand,
					(s, a) => {
						var tasks = BackgroundTaskManager.tasks.Where(t => t.state != BackgroundTaskState.InProgress).ToList();
						foreach (var t in tasks) {
							try {
								BackgroundTaskManager.tasks.Remove(t);
								t.Dispose();
							} catch (Exception err) {
								dbg.Error(err);
							}
						}
					}
				)
			);

		}

		public LocalBackgroundTasks Strings { get { return LocalBackgroundTasks.instance; } }
		public LocalButtons ButtonStrings { get { return LocalButtons.instance; } }
		public LocalTitles Titles { get { return LocalTitles.instance; } }

		void Localization() {
			closeButton.CreateBinding(Button.ContentProperty, ButtonStrings, x => x.close);
			cancelButton.CreateBinding(Button.ContentProperty, Strings, x => x.cancellAllCaption);
			cleanButton.CreateBinding(Button.ContentProperty, Strings, x => x.removeFinishedCaption);
			addUpgradeBatchButton.CreateBinding(Button.ToolTipProperty, Titles, t => t.batchUpgrade);
			addRestoreBatchButton.CreateBinding(Button.ToolTipProperty, Titles, t => t.batchRestore);

			this.CreateBinding(NavigationContext.TitleProperty, Titles, x => x.backgroundTasks);
		}

		private void HandleRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
			var hl = sender as Hyperlink;
			if (hl != null) {
				string navigateUri = hl.NavigateUri.ToString();
				// if the URI somehow came from an untrusted source, make sure to
				// validate it before calling Process.Start(), e.g. check to see
				// the scheme is HTTP, etc.
				Process.Start(new ProcessStartInfo(navigateUri));
				e.Handled = true;
			}
		}

		private void CompleteWith(Action cont) {
			Dispatcher.BeginInvoke(() => {
				if (!completed) {
					completed = true;
					cont();
					OnCompleted();
					disposables.Dispose();
				}
			});
		}

		protected virtual void OnCompleted() {
			//activity has been completed
		}
		public void Success() {
			CompleteWith(() => {
				context.Success(null);
			});
		}

		public void Dispose() {
			CompleteWith(() => {
				context.Success(null);
			});
		}

	}
}
