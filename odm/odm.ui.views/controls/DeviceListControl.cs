using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using odm.core;
using odm.models;
using odm.ui.core;
using utils;
using System.Windows.Data;

namespace odm.ui.controls {

	public class DeviceListControl : Control {
		public static readonly DependencyProperty devicesProperty = DependencyProperty.Register("devices", typeof(ObservableCollection<DeviceDescriptionHolder>), typeof(DeviceListControl));
		public ObservableCollection<DeviceDescriptionHolder> devices {
			get { return (ObservableCollection<DeviceDescriptionHolder>)GetValue(devicesProperty); }
			set { SetValue(devicesProperty, value); }
		}

		public static readonly DependencyProperty selectedDeviceProperty = DependencyProperty.Register("selectedDevice", typeof(DeviceDescriptionHolder), typeof(DeviceListControl));
		public DeviceDescriptionHolder selectedDevice {
			get { return (DeviceDescriptionHolder)GetValue(selectedDeviceProperty); }
			set { SetValue(selectedDeviceProperty, value); }
		}
		
		static DeviceListControl() {
		}

        public static readonly DependencyProperty FirmwareCaptionProperty = DependencyProperty.Register("FirmwareCaption", typeof(string), typeof(DeviceListControl));
        public string FirmwareCaption {
            get { return (string)GetValue(FirmwareCaptionProperty); }
            set { SetValue(FirmwareCaptionProperty, value); }
        }
        public static readonly DependencyProperty AddressCaptionProperty = DependencyProperty.Register("AddressCaption", typeof(string), typeof(DeviceListControl));
        public string AddressCaption {
            get { return (string)GetValue(AddressCaptionProperty); }
            set { SetValue(AddressCaptionProperty, value); }
        }
        public static readonly DependencyProperty LocationCaptionProperty = DependencyProperty.Register("LocationCaption", typeof(string), typeof(DeviceListControl));
        public string LocationCaption {
            get { return (string)GetValue(LocationCaptionProperty); }
            set { SetValue(LocationCaptionProperty, value); }
        }
	}
}
