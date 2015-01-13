using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
//using System.Drawing;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace utils {
	public class FuncBindableSource<TSrc, TProp> : INotifyPropertyChanged, IDisposable
		where TSrc : INotifyPropertyChanged {

		Action<TProp> m_setter = null;
		Func<TProp> m_getter = null;
		SerialDisposable subscription = new SerialDisposable();

		public FuncBindableSource(TSrc src, Func<TSrc, TProp> getter, Action<TSrc, TProp> setter = null) {
			if (src == null) {
				throw new ArgumentNullException("src");
			}
			if (getter == null) {
				throw new ArgumentNullException("getter");
			}
			m_getter = () => {
				return getter(src);
			};
			if (setter != null) {
				m_setter = (val) => setter(src, val);
			}
			src.PropertyChanged += OnSourcePropertyChanged;
			subscription.Disposable = Disposable.Create(() => {
				src.PropertyChanged -= OnSourcePropertyChanged;
			});
		}

		void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs args) {
			m_isCached = false;
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs("value"));
			}
		}

		bool m_isCached = false;
		TProp m_cachedValue;
		public TProp value {
			get {
				if (!m_isCached) {
					m_cachedValue = m_getter();
					m_isCached = true;
				}
				return m_cachedValue;
			}
			set {
				if (m_setter != null) {
					m_setter(value);
				}
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public void Dispose() {
			subscription.Dispose();
		}
	}
	public static class WpfExtensions {

		[DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hObject);

		/// <summary>
		/// get casted data context of element
		/// </summary>
		/// <typeparam name="T">reference type which data context is casted to</typeparam>
		/// <param name="element"><see cref="System.Windows.FrameworkElement"/> whose data context we want to get</param>
		/// <returns>casted data context or null if element is null or casting failed</returns>
		public static T GetDataContext<T>(this FrameworkElement element) where T : class {
			if (element == null) {
				return default(T);
			}
			return element.DataContext as T;
		}

		/// <summary>
		/// get casted data context of element
		/// </summary>
		/// <typeparam name="T">value type which data context is casted to</typeparam>
		/// <param name="element"><see cref="System.Windows.FrameworkElement"/> whose data context we want to get</param>
		/// <returns>casted data context or default(T) if element is null or casting failed</returns>
		public static T GetDataContextAsValue<T>(this FrameworkElement element) where T : struct {
			if (element == null) {
				return default(T);
			}
			return (element.DataContext as T?).GetValueOrDefault();
		}

		/// <summary> 
		/// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>. 
		/// </summary> 
		/// <param name="source">Bitmap to convert</param> 
		/// <returns>Converted BitmapSource</returns> 
		public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap source) {
			var hBitmap = source.GetHbitmap();
			try {
				return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
			} finally {
				DeleteObject(hBitmap);
			}
		}

		public static T? GetQueryOrNull<T>(this BitmapMetadata metadata, string query) where T : struct {
			if (!metadata.ContainsQuery(query)) {
				return null;
			}
			return metadata.GetQuery(query) as T?;
		}

		public static T GetQueryOrDefault<T>(this BitmapMetadata metadata, string query, T defaultValue) where T : struct {
			if (!metadata.ContainsQuery(query)) {
				return defaultValue;
			}
			return (metadata.GetQuery(query) as T?).GetValueOrDefault(defaultValue);
		}

		public static T GetQuery<T>(this BitmapMetadata metadata, string query) where T : struct {
			return (T)metadata.GetQuery(query);
		}

		public static IDisposable CreateBinding<TControl, TSource, TSourceProperty>(this TControl control, DependencyProperty dp, TSource source, Func<TSource, TSourceProperty> getter)
			where TControl : DependencyObject
			where TSource : INotifyPropertyChanged {

			var funSrc = new FuncBindableSource<TSource, TSourceProperty>(source, getter);
			Binding binding = new Binding("value");
			binding.Mode = BindingMode.OneWay;
			binding.Source = funSrc;

			var bindExpr = BindingOperations.SetBinding(control, dp, binding);
			return Disposable.Create(() => {
				BindingOperations.ClearBinding(control, dp);
				funSrc.Dispose();
			});
		}

		public static IDisposable SetUpdateTrigger<TControl, TProp>(this TControl control, DependencyProperty dp, Action<TProp> setter) where TControl : DependencyObject {
			var dpDesc = DependencyPropertyDescriptor.FromProperty(dp, control.GetType());
			EventHandler onValueChanged = delegate {
				if (dpDesc != null) {
					setter((TProp)dpDesc.GetValue(control));
				}
			};
			dpDesc.AddValueChanged(control, onValueChanged);
			return Disposable.Create(() => {
				if (dpDesc != null) {
					dpDesc.RemoveValueChanged(control, onValueChanged);
					dpDesc = null;
					onValueChanged = null;
				}
			});
		}

		public static IDisposable CreateBinding<TControl, TSource, TSourceProperty>(this TControl control
				, DependencyProperty dp
				, TSource source
				, Func<TSource, TSourceProperty> getter
				, Action<TSource, TSourceProperty> setter)
			where TControl : DependencyObject
			where TSource : INotifyPropertyChanged {

			var funSrc = new FuncBindableSource<TSource, TSourceProperty>(source, getter, setter);
			Binding binding = new Binding("value");
			binding.Mode = BindingMode.TwoWay;
			binding.Source = funSrc;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			var bindExpr = BindingOperations.SetBinding(control, dp, binding);
			return Disposable.Create(() => {
				BindingOperations.ClearBinding(control, dp);
				funSrc.Dispose();
			});
		}

		public static void ClearBinding<TControl>(this TControl ctrl, DependencyProperty dp) where TControl : DependencyObject {
			BindingOperations.ClearBinding(ctrl, dp);
		}

		public static void ClearAll<TControl>(this TControl ctrl) where TControl : DependencyObject {
			BindingOperations.ClearAllBindings(ctrl);
		}

		public static IObservable<object> GetPropertyChangedEvents<TControl>(this TControl ctrl, DependencyProperty dp) where TControl : DependencyObject {
			var dpd = DependencyPropertyDescriptor.FromProperty(dp, typeof(TControl));

			return Observable.Create<object>(observer => {
				EventHandler eh = (s, e) => {
					var val = ctrl.GetValue(dp);
					observer.OnNext(val);
				};
				dpd.AddValueChanged(ctrl, eh);
				return Disposable.Create(() => {
					dpd.RemoveValueChanged(ctrl, eh);
				});
			});
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListItem<T> {
		public T value;
		string name;
		public ListItem(T t, string name) {
			value = t;
			this.name = name;
		}
		public override string ToString() {
			return name;
		}
		public override bool Equals(object obj) {
			if (obj.GetType() == typeof(ListItem<T>)) {
				return value.Equals(((ListItem<T>)obj).value);
			} else
				return false;
		}
		public override int GetHashCode() {
			return value.GetHashCode();
		}
	}
	public static class ListItemEx {
		public static ListItem<T> GetAsListItem<T>(this T t, string name = null) {
			return new ListItem<T>(t, name);
		}
		public static ListItem<T> TryAsListItem<T>(this T t) {
			if (t is ListItem<T>) {
				return t as ListItem<T>;
			} else return null;
		}

		public static void SelectListItem<T>(this Selector ctrl, T value) {
			if (ctrl.HasItems) {
				if (ctrl.Items.Contains(value.GetAsListItem<T>())) {
					ctrl.SelectedIndex = ctrl.Items.IndexOf(value.GetAsListItem<T>());
				}
			}
		}
		public static ListItem<T> SelectedListItem<T>(this Selector ctrl) {
			if (ctrl.HasItems && ctrl.SelectedItem != null) {
				return (ListItem<T>)ctrl.SelectedItem;
			} else return null;
		}


		public static bool Contains(this GeometryDrawing gd, Point hp) {
			var geo = gd.Geometry;
			if (!gd.Bounds.Contains(hp)) {
				return false;
			}
			return (gd.Brush != null && geo.FillContains(hp)) || geo.StrokeContains(gd.Pen, hp);
		}

		public static void DrawLine(this DrawingContext dc, Pen pen, double x1, double y1, double x2, double y2) {
			dc.DrawLine(pen, new Point(x1, y1), new Point(x2,y2));
		}
		public static void Arrange(this UIElement element, double x, double y, double width, double height) {
			element.Arrange(new Rect(x, y, width, height));
		}
	}
}
