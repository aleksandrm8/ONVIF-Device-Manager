using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using odm.ui.viewModels;
using utils;
using odm.ui.activities;
using odm.infra;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for DeviceVew.xaml
	/// </summary>
	public partial class DeviceView : UserControl, IDisposable {
		protected CompositeDisposable disposables = new CompositeDisposable();
		public DeviceView(IUnityContainer container, DeviceViewModel vm) {
			this.DataContext = vm;
			InitializeComponent();
			var actView = new SerialDisposable();
			disposables.Add(
				vm.GetPropertyChangedEvents(m => m.Current)
				.ObserveOnDispatcher()
				.Subscribe(state => {
					switch (state) {
						case DeviceViewModel.States.Loading:
							content.Content = new ProgressView(LocalDevice.instance.loading);
							break;
						case DeviceViewModel.States.Error:
							content.Content = new ErrorView(vm.ErrorMessage);
							break;
						default:
							actView.Disposable = null;
							content.Content = deviceView;
							break;
					};
				})
			);
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing) {
			if (!disposed) {
				if (disposing) {
				}
				disposed = true;
			}
			disposables.Dispose();
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~DeviceView() {
			Dispose(false);
		}
	}

}
