using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

using odm.controllers;
using odm.ui.viewModels;
using odm.ui.views;
using utils;

namespace odm.ui {
	public class MainWindowViewModel : DependencyObject {
		public MainWindowViewModel() {
			var ver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
			this.CreateBinding(TitleProperty, odm.ui.controls.CommonApplicationStrings.instance, x => {
				return
					String.Format("{0} v{1}.{2}.{3}", x.applicationName, ver.Major, ver.Minor, ver.Build);
			});
		}
		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}
		// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(MainWindowViewModel));

	}
}
