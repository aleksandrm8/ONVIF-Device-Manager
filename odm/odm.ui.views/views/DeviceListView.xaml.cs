using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using odm.ui.viewModels;
using utils;
using System.Windows.Media;

namespace odm.ui.controls {

	public partial class DeviceListView : UserControl {
		public static readonly RoutedCommand HideCommand = new RoutedCommand("Hide", typeof(DeviceListView));
        public LocalDeviceList Strings { get { return LocalDeviceList.instance;} }
		DeviceListViewModel viewModel;
		public DeviceListView(DeviceListViewModel viewModel) {
			this.viewModel = viewModel;
			this.DataContext = viewModel;
			InitializeComponent();
			//hideButton.Command = new DelegateCommand(
			//   () => this.Visibility = Visibility.Collapsed,
			//   () => true
			//);


			btnresetFilter.Click+=new RoutedEventHandler((o,e)=>{
				valueFilter.Text = "";
			});
			
			Loaded += (s,a)=>{
                deviceList.CreateBinding(DeviceListControl.FirmwareCaptionProperty, Strings, x => x.firmware);
                deviceList.CreateBinding(DeviceListControl.LocationCaptionProperty, Strings, x => x.location);
                deviceList.CreateBinding(DeviceListControl.AddressCaptionProperty, Strings, x => x.address);
				viewModel.LoadDevices();
			};
		}
	}
}
