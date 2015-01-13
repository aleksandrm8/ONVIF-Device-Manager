using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using odm.ui.controls;
using utils;

namespace odm.ui {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : CustomWindow, INotifyPropertyChanged {
		public MainWindow() {
			InitializeComponent();

            devlistCaption.CreateBinding(TextBlock.TextProperty, LocalTitles.instance, s => s.devicesList);

			this.CommandBindings.Add(
				new CommandBinding(
					DeviceListView.HideCommand,
					(s, a) => {
						devlist.Visibility = Visibility.Collapsed;
						deviceListButton.Visibility = Visibility.Visible;
					}
				)
			);
			deviceListButton.Command = new DelegateCommand(
				() => {
					devlist.Visibility = Visibility.Visible;
					deviceListButton.Visibility = Visibility.Collapsed;
				}
			);


			//AppDefaults.InitConfigs();

			InitPosition();

			SizeChanged += new SizeChangedEventHandler((obj, evargs) => {
				var mvs = AppDefaults.visualSettings;
				mvs.WndState = this.WindowState;

				if (this.WindowState == System.Windows.WindowState.Maximized) {
					AppDefaults.UpdateVisualSettings(mvs);
					return;
				}

				if (this.WindowState == System.Windows.WindowState.Minimized) {
					AppDefaults.UpdateVisualSettings(mvs);
					return;
				}

				mvs.WndSize = new Rect(mvs.WndSize.X, mvs.WndSize.Y, evargs.NewSize.Width, evargs.NewSize.Height);
				AppDefaults.UpdateVisualSettings(mvs);
			});
			LocationChanged += new EventHandler((obj, evargs) => {
				var vs = AppDefaults.visualSettings;
				vs.WndState = this.WindowState;
				if (this.WindowState == System.Windows.WindowState.Maximized) {
					AppDefaults.UpdateVisualSettings(vs);
					return;
				}
				if (this.WindowState == System.Windows.WindowState.Minimized) {
					AppDefaults.UpdateVisualSettings(vs);
					return;
				}

				vs.WndSize = new Rect(this.Left, this.Top, vs.WndSize.Width, vs.WndSize.Height);
				AppDefaults.UpdateVisualSettings(vs);
			});
		}

		void InitPosition() {
			var vis = AppDefaults.visualSettings;

			this.Width = vis.WndSize.Width;
			this.Height = vis.WndSize.Height;

			this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
			this.Left = vis.WndSize.X;
			this.Top = vis.WndSize.Y;

			this.WindowState = vis.WndState;
		}

		public void Init(MainWindowViewModel vm) {
			this.DataContext = vm;
		}

		public static DependencyProperty ToolBarProperty = DependencyProperty.RegisterAttached("ToolBar", typeof(FrameworkElement), typeof(Control));
		public static FrameworkElement GetToolBar(DependencyObject ToolBar) {
			return (FrameworkElement)ToolBar.GetValue(ToolBarProperty);
		}
		public static void SetToolBar(DependencyObject ToolBar, Boolean value) {
			ToolBar.SetValue(ToolBarProperty, value);
		}
		public FrameworkElement ToolBar {
			get {return (FrameworkElement)GetValue(ToolBarProperty);}
			set {SetValue(ToolBarProperty, value);}
		}

		public void NotifyPropertyChanged(string prop) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(prop));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
