using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using utils;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace odm.ui.controls {

	public class CustomWindow : Window {

		public static readonly DependencyProperty IsNonClientProperty = DependencyProperty.RegisterAttached("IsNonClient", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		
		public static readonly DependencyProperty IsLeftResizerProperty = DependencyProperty.RegisterAttached("IsLeftResizer", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		public static readonly DependencyProperty IsRightResizerProperty = DependencyProperty.RegisterAttached("IsRightResizer", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		public static readonly DependencyProperty IsTopResizerProperty = DependencyProperty.RegisterAttached("IsTopResizer", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		public static readonly DependencyProperty IsTopLeftResizerProperty = DependencyProperty.RegisterAttached("IsTopLeftResizer", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		public static readonly DependencyProperty IsTopRightResizerProperty = DependencyProperty.RegisterAttached("IsTopRightResizer", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		public static readonly DependencyProperty IsBottomResizerProperty = DependencyProperty.RegisterAttached("IsBottomResizer", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		public static readonly DependencyProperty IsBottomLeftResizerProperty = DependencyProperty.RegisterAttached("IsBottomLeftResizer", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		public static readonly DependencyProperty IsBottomRightResizerProperty = DependencyProperty.RegisterAttached("IsBottomRightResizer", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(false) {Inherits = true});
		
		public static readonly RoutedCommand CloseCommand = new RoutedCommand("Close", typeof(CustomWindow));
		public static readonly RoutedCommand MaximizeCommand = new RoutedCommand("Maximize", typeof(CustomWindow));
		public static readonly RoutedCommand MinimizeCommand = new RoutedCommand("Minimize", typeof(CustomWindow));
		
		static CustomWindow() {

			CommandManager.RegisterClassCommandBinding(
				typeof(CustomWindow), 
				new CommandBinding(
					CloseCommand, 
					(s, a) => {
						var wnd = s as CustomWindow;
						if (wnd != null) {
							wnd.Close();
						}
					}
				)
			);

			CommandManager.RegisterClassCommandBinding(
				typeof(CustomWindow), 
				new CommandBinding(
					MaximizeCommand, 
					(s, a) => {
						var wnd = s as CustomWindow;
						if (wnd != null) {
							if (wnd.WindowState == WindowState.Maximized) {
								wnd.WindowState = WindowState.Normal;
							} else {
								wnd.WindowState = WindowState.Maximized;
							}
						}
					}
				)
			);

			CommandManager.RegisterClassCommandBinding(
				typeof(CustomWindow), 
				new CommandBinding(
					MinimizeCommand, 
					(s, e) => {
						var wnd = s as CustomWindow;
						if (wnd != null) {
							wnd.WindowState = WindowState.Minimized;
						}
					}
				)
			);

		}

		public CustomWindow() {
			MouseLeftButtonDown += MouseLeftButtonDown_Handler;
			this.SourceInitialized += (s,e)=>{
				m_hwndSource = PresentationSource.FromVisual((Visual)s) as HwndSource;
			};
		}

		private HwndSource m_hwndSource = null;

		private ResizeDirection? GetResizer(UIElement e) {
			if (GetIsLeftResizer(e)) {
				return ResizeDirection.Left;
			} else if (GetIsRightResizer(e)) {
				return ResizeDirection.Right;
			} else if (GetIsTopResizer(e)) {
				return ResizeDirection.Top;
			} else if (GetIsTopLeftResizer(e)) {
				return ResizeDirection.TopLeft;
			} else if (GetIsTopRightResizer(e)) {
				return ResizeDirection.TopRight;
			} else if (GetIsBottomResizer(e)) {
				return ResizeDirection.Bottom;
			} else if (GetIsBottomLeftResizer(e)) {
				return ResizeDirection.BottomLeft;
			} else if (GetIsBottomRightResizer(e)) {
				return ResizeDirection.BottomRight;
			}

			return null;
		}
		private void MouseLeftButtonDown_Handler(object sender, MouseButtonEventArgs e) {
			dbg.Assert(e != null);
			dbg.Assert(e.OriginalSource != null);
			UIElement d = e.OriginalSource as UIElement;
			dbg.Assert(d != null);

			if (GetIsNonClient(d)) {
				if (e.ClickCount == 2) {
					if (this.WindowState == System.Windows.WindowState.Normal) {
						this.WindowState = System.Windows.WindowState.Maximized;
					} else {
						this.WindowState = System.Windows.WindowState.Normal;
					}
				} else {
					DragMove();
				}				
				e.Handled = true;
				return;
			}
			var resizer = GetResizer(d);
			if(resizer.HasValue){
				ResizeWindow(resizer.Value);
				e.Handled = true;
				return;
			}
		}

		public enum ResizeDirection:int {
			Left = 1,
			Right = 2,
			Top = 3,
			TopLeft = 4,
			TopRight = 5,
			Bottom = 6,
			BottomLeft = 7,
			BottomRight = 8,
		}

		private const int SC_SIZE = 0xF000;
		private const int WM_SYSCOMMAND = 0x112;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		public void ResizeWindow(ResizeDirection direction) {
			if (m_hwndSource != null) {
				SendMessage(m_hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(SC_SIZE + direction), IntPtr.Zero);
			}
		}
		
		public static bool GetIsNonClient(DependencyObject element) {
			return (bool)element.GetValue(IsNonClientProperty);
		}
		public static void SetIsNonClient(DependencyObject element, Boolean value) {
			element.SetValue(IsNonClientProperty, value);
		}

		public static bool GetIsLeftResizer(DependencyObject element) {
			return (bool)element.GetValue(IsLeftResizerProperty);
		}
		public static void SetIsLeftResizer(DependencyObject element, Boolean value) {
			element.SetValue(IsLeftResizerProperty, value);
		}

		public static bool GetIsRightResizer(DependencyObject element) {
			return (bool)element.GetValue(IsRightResizerProperty);
		}
		public static void SetIsRightResizer(DependencyObject element, Boolean value) {
			element.SetValue(IsRightResizerProperty, value);
		}

		public static bool GetIsTopResizer(DependencyObject element) {
			return (bool)element.GetValue(IsTopResizerProperty);
		}
		public static void SetIsTopResizer(DependencyObject element, Boolean value) {
			element.SetValue(IsTopResizerProperty, value);
		}

		public static bool GetIsTopLeftResizer(DependencyObject element) {
			return (bool)element.GetValue(IsTopLeftResizerProperty);
		}
		public static void SetIsTopLeftResizer(DependencyObject element, Boolean value) {
			element.SetValue(IsTopLeftResizerProperty, value);
		}

		public static bool GetIsTopRightResizer(DependencyObject element) {
			return (bool)element.GetValue(IsTopRightResizerProperty);
		}
		public static void SetIsTopRightResizer(DependencyObject element, Boolean value) {
			element.SetValue(IsTopRightResizerProperty, value);
		}

		public static bool GetIsBottomResizer(DependencyObject element) {
			return (bool)element.GetValue(IsBottomResizerProperty);
		}
		public static void SetIsBottomResizer(DependencyObject element, Boolean value) {
			element.SetValue(IsBottomResizerProperty, value);
		}

		public static bool GetIsBottomLeftResizer(DependencyObject element) {
			return (bool)element.GetValue(IsBottomLeftResizerProperty);
		}
		public static void SetIsBottomLeftResizer(DependencyObject element, Boolean value) {
			element.SetValue(IsBottomLeftResizerProperty, value);
		}

		public static bool GetIsBottomRightResizer(DependencyObject element) {
			return (bool)element.GetValue(IsBottomRightResizerProperty);
		}
		public static void SetIsBottomRightResizer(DependencyObject element, Boolean value) {
			element.SetValue(IsBottomRightResizerProperty, value);
		}
	}
}
