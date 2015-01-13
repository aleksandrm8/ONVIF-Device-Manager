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
using System.Reactive.Disposables;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for RadioExpanderControl.xaml
	/// </summary>
	public partial class RadioExpanderControl : UserControl, IDisposable {
		public RadioExpanderControl() {
			InitializeComponent();
		}
		CompositeDisposable disposables = new CompositeDisposable();

		public void AddOnvifSection(bool isActive, bool needRefresh, UserControl section){

		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
