using System;
using System.Collections.Generic;
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

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for HeaderedColumn.xaml
	/// </summary>
    public partial class ContentColumn : ContentControl {
		public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ContentColumn));
		public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(FrameworkElement), typeof(ContentColumn));
		public static DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(FrameworkElement), typeof(ContentColumn));
		public static DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(FrameworkElement), typeof(ContentColumn));
		public static DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(FrameworkElement), typeof(ContentColumn));
		public ContentColumn() {
			//this.InitializeComponent();
		}
		public string Title {
			get {
				return (string)GetValue(TitleProperty);
			}
			set {
				SetValue(TitleProperty, value);
			}
		}
		public FrameworkElement Header {
			get {
				return (FrameworkElement)GetValue(HeaderProperty);
			}
			set {
				SetValue(HeaderProperty, value);
			}
		}
		public FrameworkElement Footer {
			get {
				return (FrameworkElement)GetValue(FooterProperty);
			}
			set {
				SetValue(FooterProperty, value);
			}
		}
		public FrameworkElement Progress {
			get {
				return (FrameworkElement)GetValue(ProgressProperty);
			}
			set {
				SetValue(ProgressProperty, value);
			}
		}
		public FrameworkElement Error {
			get {
				return (FrameworkElement)GetValue(ErrorProperty);
			}
			set {
				SetValue(ErrorProperty, value);
			}
		}		
	}
}