using System;
using System.Collections.Generic;
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
	/// Interaction logic for HeaderedColumn.xaml
	/// </summary>
	public partial class DeviceChannelControl : ContentControl {
		public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DeviceChannelControl));
		public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(FrameworkElement), typeof(DeviceChannelControl));

		public DeviceChannelControl() {
			//this.InitializeComponent();
		}
		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}
		public FrameworkElement Header {
			get { return (FrameworkElement)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}
	}
}