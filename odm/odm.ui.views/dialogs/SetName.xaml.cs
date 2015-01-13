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

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
    public partial class SetName : DialogWindow {
        public SetName(SetNameViewModel viewModel) {
			InitializeComponent();

            this.DataContext = viewModel;

            Closing += new System.ComponentModel.CancelEventHandler(Upgrade_Closing);
		}

        void CloseBtn() {
            //(this.DataContext as UpgradeViewModel).Dispose();
        }
        void Upgrade_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            CloseBtn();
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            Close();
        }
	}
}
