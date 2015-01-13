using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace odm.ui.activities {
	public interface INavigationContext {
		IObservable<String> title { get; }
	}
	public class NavigationContext : INavigationContext {
		
		public static string GetTitle(DependencyObject obj) {
			return (string)obj.GetValue(TitleProperty);
		}

		public static void SetTitle(DependencyObject obj, string value) {
			obj.SetValue(TitleProperty, value);
		}

		public static readonly DependencyProperty TitleProperty;
		static NavigationContext(){
			TitleProperty = DependencyProperty.RegisterAttached(
				"Title", typeof(string), typeof(NavigationContext), 
				new FrameworkPropertyMetadata(null)
			);
		}



		public IObservable<string> title { get; set; }
	}
}
