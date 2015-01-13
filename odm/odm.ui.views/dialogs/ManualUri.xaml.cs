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
using System.Windows.Shapes;
using System.Diagnostics;

using Microsoft.Practices.Prism.Commands;

using odm.ui.controls;
using utils;
using odm.ui.dialogs;
using odm.ui.core;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
    public partial class ManualUri : DialogWindow {
		public enum ManualType {ADD, EDIT };
		
		public ManualUri(string operation, ManualType mtype, string deviceUri) {
			InitializeComponent();
			operationName = operation;
            this.DataContext = this;

			if (mtype == ManualType.ADD) {
				devUri = "http://192.168.0.1/onvif/device_service";
			} else {
				devUri = deviceUri;
			}
			ButtonName = LocalButtons.instance.apply;

            Closing += new System.ComponentModel.CancelEventHandler(Upgrade_Closing);
		}

		public string operationName { get; set; }
		public string ButtonName { get; set; }

		public LocalButtons Buttons { get { return LocalButtons.instance; } }

		public string devUri {
			get { return (string)GetValue(devUriProperty); }
			set { SetValue(devUriProperty, value); }
		}
		public static readonly DependencyProperty devUriProperty = DependencyProperty.Register("devUri", typeof(string), typeof(ManualUri), new PropertyMetadata((o, evarg) => {
			var instance = o as ManualUri;
			if (instance != null) {
				instance.btnApply.IsEnabled = Uri.IsWellFormedUriString((string)evarg.NewValue, UriKind.RelativeOrAbsolute);
				if (instance.btnApply.IsEnabled) {
					instance.valueErrorInfo.Visibility = Visibility.Collapsed;
				} else {
					instance.valueErrorInfo.Visibility = Visibility.Visible;
				}
			}
		}));

        void CloseBtn() {
			//devUri = "";
            //(this.DataContext as UpgradeViewModel).Dispose();
        }
        void Upgrade_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            CloseBtn();
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = true;
			this.Close();
        }

		private void btnCancel_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = false;
			this.Close();
		}
	}
}
