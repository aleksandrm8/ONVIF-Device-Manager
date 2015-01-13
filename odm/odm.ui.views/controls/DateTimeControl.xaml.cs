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
using odm.ui.activities;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.ComponentModel;
using utils;
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for DateTimeControl.xaml
	/// </summary>
	public partial class DateTimeControl : UserControl, IDisposable {
		public DateTimeControl() {
			InitializeComponent();
			Invalidate();
		}

		Func<DateTime> getDateTime = null;
		SerialDisposable timerSubscription = new SerialDisposable();
		
		public void Init(Func<DateTime> getDateTime) {
			this.getDateTime = getDateTime;
			timerSubscription.Disposable = null;
			Invalidate();
			if (getDateTime != null && !timerSubscription.IsDisposed) {
				timerSubscription.Disposable = new Timer(
					(state) => {
						Dispatcher.BeginInvoke(() => {
							Invalidate();
						});
					}, null, 500, 500
				);
			}
		}
		public void Invalidate() {
			var dateTime = default(DateTime);
			if (getDateTime != null) {
				dateTime = getDateTime();
			}
			valueTime.Text = dateTime.ToLongTimeString();
			valueDate.Text = dateTime.ToShortDateString();
				
			if (dateTime.Kind != DateTimeKind.Utc) {
				valueCaption.Text = LocalTimeZone.instance.captionLocal;
				valueCaption.FontWeight = FontWeights.Normal;
				valueCaption.Foreground = Brushes.DarkGray;
					
			} else {
				valueCaption.Text = LocalTimeZone.instance.captionUtc;
				valueCaption.FontWeight = FontWeights.Bold;
				valueCaption.Foreground = Brushes.DarkRed;
			}
			
		}

		public void Dispose() {
			timerSubscription.Disposable = null;
			getDateTime = null;
			Invalidate();
		}
	}
}
