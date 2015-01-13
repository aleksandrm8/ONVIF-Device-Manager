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

using odm.utils;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for SetNtpServer.xaml
	/// </summary>
	public partial class SetNtpServer : Window {
		public SetNtpServer() {
			InitializeComponent();
			Localization();

			setButton.Click += new RoutedEventHandler(setButton_Click);
		}

		void setButton_Click(object sender, RoutedEventArgs e) {
			if (setServer != null)
				setServer(ntpAddress.Text);
			this.Close();
		}

		public Action<string> setServer { get; set; }

		void Localization() {
			this.Title = PropertyTimeZoneStrings.instance.ntpServerSetupTitle;
			setButton.CreateBinding(Button.ContentProperty, PropertyTimeZoneStrings.instance, x => x.ntpServerSetupSet);
		}
	}
}
