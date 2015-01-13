using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for DeviceCard.xaml
	/// </summary>
	public class DeviceCard : ContentControl {

		public static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register("DeviceName", typeof(string), typeof(DeviceCard));
		public static readonly DependencyProperty FirmwareProperty = DependencyProperty.Register("Firmware", typeof(string), typeof(DeviceCard));
		public static readonly DependencyProperty AddressProperty = DependencyProperty.Register("Address", typeof(string), typeof(DeviceCard));

		public DeviceCard() {
			InitializeComponent();
		}

		bool m_contentLoaded = false;
		public void InitializeComponent() {
			if (!m_contentLoaded) {
				m_contentLoaded = true;
				Uri resourceLocator = new Uri("/odm;component/controls/DeviceCard.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		public string DeviceName{
			get {
				return (string)GetValue(DeviceNameProperty);
			}
			set {
				SetValue(DeviceNameProperty, value);
			}
		}
		public string Address {
			get {
				return (string)GetValue(AddressProperty);
			}
			set {
				SetValue(AddressProperty, value);
			}
		}
		public string Firmware {
			get {
				return (string)GetValue(FirmwareProperty);
			}
			set {
				SetValue(FirmwareProperty, value);
			}
		}
	}
}
