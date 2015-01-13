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
using odm.ui.controls;
using utils;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for WebPageView.xaml
	/// </summary>
	public partial class WebPageView : BasePropertyControl, IDisposable {
		public WebPageView() {
			InitializeComponent();
			DataContext = this;
			webContent.SourceUpdated += (s, a) => {
				dbg.Info(a.Source.ToString());
			};
			//webContent.CreateBinding(WebBrowser.So .Cr .Source
			Init();
		}
		public LocalTitles Titles { get { return LocalTitles.instance; } }
		public void NavigateTo(Uri url){
			webContent.Navigate(url);
		}
		
		void Init() {
			navigateLeft.Content = "<";
			navigateRight.Content = ">";
			webContent.Navigated += (o, a) => {
				var doc = (mshtml.HTMLDocument)webContent.Document;
				valueUri.Text = doc.url;
				navigateLeft.IsEnabled = webContent.CanGoBack;
				navigateRight.IsEnabled = webContent.CanGoForward;
				//state = loading;
			};
			webContent.LoadCompleted+=new LoadCompletedEventHandler((o,e)=>{
				var doc = (mshtml.HTMLDocument)webContent.Document;
				
				navigateLeft.IsEnabled = webContent.CanGoBack;
				navigateRight.IsEnabled = webContent.CanGoForward;
				//state = completed;
			});
			
 			navigateLeft.Click+=new RoutedEventHandler((o,e)=>{
				if (webContent.CanGoBack) {
					webContent.GoBack();
				}
			});
			navigateRight.Click += new RoutedEventHandler((o, e) => {
				if (webContent.CanGoForward) {
					webContent.GoForward();
				}
			});
		}

		public void Dispose() {
			webContent.Dispose();
		}
	}
}
