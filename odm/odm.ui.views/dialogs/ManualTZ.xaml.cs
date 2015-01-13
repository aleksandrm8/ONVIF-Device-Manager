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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Globalization;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
    public partial class ManualTZ : DialogWindow, INotifyPropertyChanged {
		public ManualTZ(string posixTZ) {
			InitializeComponent();
            this.DataContext = this;
			btnApply.Content = LocalButtons.instance.apply;

			valueTZ.CreateBinding(TextBox.TextProperty, this, t => t.posixTZ,
				(m, o) => {
					m.posixTZ = o;
					posix = PosixTz.TryParse(o);
					ValidateUI();
				}
			);
			this.posixTZ = posixTZ;
			Loaded+=new RoutedEventHandler((o,e)=>{ValidateUI();});
            Closing += new System.ComponentModel.CancelEventHandler(Upgrade_Closing);
		}
		void ValidateUI() {
			var pos = PosixTz.TryParse(posixTZ);
			if (pos == null) {
				borderTZ.BorderBrush = Brushes.Red;
			} else {
				borderTZ.BorderBrush = Brushes.Transparent;
			}
		}
		public PosixTz posix { get; protected set; }
		public LocalButtons Buttons { get { return LocalButtons.instance; } }

		string _posixTZ;
		public string posixTZ {
			get {
				return _posixTZ;
			}
			set {
				_posixTZ = value;
				NotifyPropertyChanged("posixTZ");
			}
		}
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

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String info) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
	}
}
