using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Linq.Expressions;
using utils;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;

namespace odm {
	public static class ImageConversion {
		[System.Runtime.InteropServices.DllImport("gdi32")]
		public static extern int DeleteObject(IntPtr hObject);
		public static ImageSource ToImageSource(System.Drawing.Image img) {
			return ToImageSource(new System.Drawing.Bitmap(img));
		}
		public static ImageSource ToImageSource(System.Drawing.Bitmap bitmap) {
			var hbitmap = bitmap.GetHbitmap();
			try {
				var imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

				return imageSource;
			} finally {
				DeleteObject(hbitmap);
			}
		}
	}
}
