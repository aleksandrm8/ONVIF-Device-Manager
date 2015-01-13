using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Interop;

using Microsoft.Practices.Prism.Commands;

using utils;

namespace odm.ui.controls {

	public class DialogWindow : Window {

		public static readonly DependencyProperty IsNonClientProperty = DependencyProperty.RegisterAttached("IsNonClient", typeof(bool), typeof(DialogWindow), new FrameworkPropertyMetadata(false) { Inherits = true });
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(DialogWindow));
		public static readonly RoutedCommand CloseCommand = new RoutedCommand("Close", typeof(DialogWindow));
		
		static DialogWindow() {
			CommandManager.RegisterClassCommandBinding(
				typeof(DialogWindow),
				new CommandBinding(
					CloseCommand,
					(s, a) => {
						var wnd = s as DialogWindow;
						if (wnd != null) {
							wnd.Close();
						}
					}
				)
			);
		}

		public DialogWindow() {
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs evt) {
			UIElement el = evt==null?null:evt.OriginalSource as UIElement;
			if (el != null && GetIsNonClient(el)) {
				if (evt.ClickCount > 1) {
					//Close();
				} else {
					DragMove();
				}
				evt.Handled = true;
				return;
			}
			base.OnMouseLeftButtonDown(evt);
		}

		public string Header {
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		public static bool GetIsNonClient(DependencyObject element) {
			return (bool)element.GetValue(IsNonClientProperty);
		}
		public static void SetIsNonClient(DependencyObject element, Boolean value) {
			element.SetValue(IsNonClientProperty, value);
		}
	}
}
