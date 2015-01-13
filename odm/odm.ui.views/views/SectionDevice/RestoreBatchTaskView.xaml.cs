using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using odm.core;
using odm.infra;
using odm.ui.activities;
using odm.ui.core;
using onvif.utils;
using utils;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for RestoreBatchTaskView.xaml
	/// </summary>
	public partial class RestoreBatchTaskView : UserControl, INotifyPropertyChanged, IDisposable {
		public static readonly RoutedCommand RestoreCommand = new RoutedCommand("RestoreCommand", typeof(RestoreBatchTaskView));
		public static readonly RoutedCommand CancelCommand = new RoutedCommand("CancelCommand", typeof(RestoreBatchTaskView));

		#region Activity definition
		public static FSharpAsync<Unit> Show(IUnityContainer container) {
			return container.StartViewActivity<Unit>(context => {
				var view = new RestoreBatchTaskView(context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		bool completed = false;
		IActivityContext<Unit> activityContext;
		INvtSession nvtSession;
		CompositeDisposable IdentitySubscriptions = new CompositeDisposable();
		public LocalTitles Titles { get { return LocalTitles.instance; } }
		public LocalBatchTasks Strings { get { return LocalBatchTasks.instance; } }
		public LocalButtons ButtonStrings { get { return LocalButtons.instance; } }

		public RestoreBatchTaskView(IActivityContext<Unit> context) {
			this.activityContext = context;

			items = new ObservableCollection<BatchItem>();
			manufacturers = new ObservableCollection<string>();
			models = new ObservableCollection<string>();
			firmwares = new ObservableCollection<string>();

			CommonList = new List<BatchItem>();
			BatchTaskEventArgs evargs = context.container.Resolve<BatchTaskEventArgs>();
			nvtSession = evargs.session;
			;

			evargs.Devices.ForEach(dev => {
				BatchItem bitem = new BatchItem(dev);
				CommonList.Add(bitem);
			});

			this.CommandBindings.Add(
				new CommandBinding(
					RestoreBatchTaskView.RestoreCommand,
					(s, a) => {
						OnRestore();
					}
				)
			);

			this.CommandBindings.Add(
				new CommandBinding(
					RestoreBatchTaskView.CancelCommand,
					(s, a) => {
						Success();
					}
				)
			);

			InitializeComponent();

			BindData();
			Localization();
		}
		void Localization() {
			this.CreateBinding(NavigationContext.TitleProperty, Titles, x => x.batchRestore);
			btnUpgrade.CreateBinding(Button.ContentProperty, Strings, s => s.batchUpgrade);
			btnCLose.CreateBinding(Button.ContentProperty, ButtonStrings, s => s.close);
			filtersCaption.CreateBinding(GroupBox.HeaderProperty, Strings, s => s.filters);
			manufacturersCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.manufacturer);
			modelsCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.model);
			firmwareCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.firmware);
		}
		void OnRestore() {
			var dlg = new OpenFileDialog();
			dlg.Filter = "Backup files (*.backup)|*.backup|Zip archives (*.zip)|*.zip";
			dlg.Title = "Open backup file";
			//dlg.InitialDirectory = Directory.GetCurrentDirectory();
			if (dlg.ShowDialog() != true)
				return;

			items.Where(item => item.IsChecked == true).ForEach(item => {
				StartRestore(item, dlg.FileName);
			});
		}

		void StartRestore(BatchItem bitem, string binPath) {
			NvtSessionFactory sessionFactory = new NvtSessionFactory(LoadCurrentAccount());

			IdentitySubscriptions.Add(sessionFactory.CreateSession(bitem.Device.Uris)
					  .ObserveOnCurrentDispatcher()
					  .Subscribe(isession => {
						  OdmSession odmsession = new OdmSession(isession);
						  var async = odmsession.RestoreSystem(binPath);

						  var taskFact = BackgroundTask.CreateForAsync(async.ObserveOnCurrentDispatcher(), bitem.Device);
						  var backgroundTask = taskFact(
									  result => CompleteWith(() => {
									  }),
									  error => CompleteWith(() => {
										  activityContext.Error(error);
									  })
						  );
						  BackgroundTaskManager.tasks.Add(backgroundTask);
					  }, err => {
						  dbg.Error(err);
					  }));
		}

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

			public static Func<Action<TResult>, Action<Exception>, IBackgroundTask> CreateForAsync<TResult>(FSharpAsync<TResult> async, DeviceDescriptionHolder dev) {
				return (onSuccess, onError) => {
					var disp = new MultipleAssignmentDisposable();
					var backgroundTask = new AnonymousBackgroundTask();
					backgroundTask.name = "Restore device " + dev.Name;
					backgroundTask.description = "Address: " + dev.Address;
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

		System.Net.NetworkCredential LoadCurrentAccount() {
			var acc = AccountManager.Instance.CurrentAccount;
			System.Net.NetworkCredential account = null;
            if (!acc.IsAnonymous)
                account = new System.Net.NetworkCredential() { UserName = acc.Name, Password = acc.Password };
			
			return account;
		}
		public class BatchItem : INotifyPropertyChanged {
			public BatchItem(DeviceDescriptionHolder dev) {
				Device = dev;
			}
			bool isChecked;
			public bool IsChecked {
				get {
					return isChecked;
				}
				set {
					isChecked = value;
					NotifyPropertyChanged("IsChecked");
				}
			}
			public DeviceDescriptionHolder Device { get; set; }
			public string Name {
				get {
					return Device.Name;
				}
				set { }
			}
			public string Manufacturer {
				get {
					return Device.Manufacturer;
				}
				set { }
			}
			public string Model {
				get {
					return Device.DeviceModel;
				}
				set { }
			}
			public string Firmware {
				get {
					return Device.Firmware;
				}
				set { }
			}
			public string Address {
				get {
					return Device.Address;
				}
				set { }
			}

			private void NotifyPropertyChanged(String info) {
				var prop_changed = this.PropertyChanged;
				if (prop_changed != null) {
					prop_changed(this, new PropertyChangedEventArgs(info));
				}
			}
			public event PropertyChangedEventHandler PropertyChanged;
		}

		List<BatchItem> CommonList { get; set; }
		public ObservableCollection<BatchItem> items { get; set; }
		public ObservableCollection<string> manufacturers { get; set; }
		public ObservableCollection<string> models { get; set; }
		public ObservableCollection<string> firmwares { get; set; }

		public string SelectedManufacturer {
			get { return (string)GetValue(SelectedManufacturerProperty); }
			set { SetValue(SelectedManufacturerProperty, value); }
		}
		public static readonly DependencyProperty SelectedManufacturerProperty =
			DependencyProperty.Register("SelectedManufacturer", typeof(string), typeof(RestoreBatchTaskView), new UIPropertyMetadata((obj, evarg) => {
				var view = (RestoreBatchTaskView)obj;
				view.RefreshModels();
			}));

		public string SelectedModel {
			get { return (string)GetValue(SelectedModelProperty); }
			set { SetValue(SelectedModelProperty, value); }
		}
		public static readonly DependencyProperty SelectedModelProperty =
			DependencyProperty.Register("SelectedModel", typeof(string), typeof(RestoreBatchTaskView), new UIPropertyMetadata((obj, evarg) => {
				var view = (RestoreBatchTaskView)obj;
				view.RefreshFirmwares();
			}));

		public string SelectedFirmware {
			get { return (string)GetValue(SelectedFirmwareProperty); }
			set { SetValue(SelectedFirmwareProperty, value); }
		}
		public static readonly DependencyProperty SelectedFirmwareProperty =
			DependencyProperty.Register("SelectedFirmware", typeof(string), typeof(RestoreBatchTaskView), new UIPropertyMetadata((obj, evarg) => {
				var view = (RestoreBatchTaskView)obj;
				view.RefreshList();
			}));

		void BindData() {
			devicesList.CreateBinding(ListView.ItemsSourceProperty, this, x => x.items, (m, v) => { m.items = v; });
			manufacturersValue.CreateBinding(ComboBox.ItemsSourceProperty, this, x => x.manufacturers, (m, v) => { m.manufacturers = v; });
			modelsValue.CreateBinding(ComboBox.ItemsSourceProperty, this, x => x.models, (m, v) => { m.models = v; });
			firmwareValue.CreateBinding(ComboBox.ItemsSourceProperty, this, x => x.firmwares, (m, v) => { m.firmwares = v; });

			InitFilter();
		}

		void RefreshModels() {
			models.Clear();
			CommonList.Where(man => man.Manufacturer == SelectedManufacturer)
				.GroupBy(x => x.Model)
				.ForEach(m => {
					models.Add(m.Key);
				});
			SelectedModel = models.FirstOrDefault();

			RefreshFirmwares();
		}
		void RefreshFirmwares() {
			firmwares.Clear();
			CommonList.Where(man => man.Manufacturer == SelectedManufacturer)
				.GroupBy(x => x.Firmware)
				.ForEach(f => {
					firmwares.Add(f.Key);
				});
			SelectedFirmware = firmwares.FirstOrDefault();
		}
		void InitFilter() {
			CommonList.GroupBy(x => x.Manufacturer).ForEach(m => {
				manufacturers.Add(m.Key);
			});
			SelectedManufacturer = manufacturers.FirstOrDefault();

			RefreshModels();
		}
		void RefreshList() {
			items.Clear();
			CommonList.ForEach(elem => elem.IsChecked = false);

			CommonList.Where(item => item.Manufacturer == SelectedManufacturer && item.Model == SelectedModel && item.Firmware == SelectedFirmware)
				.ForEach(it => {
					it.IsChecked = true;
					items.Add(it);
				});
		}
		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

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
					//disposables.Dispose();
				}
			});
		}

		protected virtual void OnCompleted() {
			//activity has been completed
		}
		public void Success() {
			CompleteWith(() => {
				activityContext.Success(null);
			});
		}

		public void Dispose() {
			CompleteWith(() => {
				activityContext.Success(null);
			});
		}

		void IDisposable.Dispose() {
			IdentitySubscriptions.Dispose();
		}
	}
}
