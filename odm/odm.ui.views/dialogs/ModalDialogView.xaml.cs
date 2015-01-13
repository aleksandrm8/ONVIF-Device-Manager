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

using odm.ui.controls;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class ModalDialogView : DialogWindow {
		public ModalDialogView() {
			InitializeComponent();

			//this.CreateBinding(DialogWindow.HeaderProperty, LocalDevice.instance, x => x.information);
		}
	}
}
