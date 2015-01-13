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
    public partial class SetAnalyticsName : DialogWindow {
        public SetAnalyticsName() {
			InitializeComponent();
		}

        void CloseBtn() {
        }
	}
}
