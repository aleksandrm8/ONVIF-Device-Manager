using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace utils {
	public static class BitmapImageEx {
		public enum ImageType {
			PNG,
			JPG,
			BMP
		}

		public static BitmapImage Crop(this BitmapImage bmp, Int32Rect rct) {
			CroppedBitmap croppedBmp = new CroppedBitmap(bmp, rct);

			BmpBitmapEncoder encoder = new BmpBitmapEncoder();
			MemoryStream memoryStream = new MemoryStream();
			BitmapImage bImg = new BitmapImage();

			encoder.Frames.Add(BitmapFrame.Create(croppedBmp));
			encoder.Save(memoryStream);

			bImg.BeginInit();
			bImg.CacheOption = BitmapCacheOption.OnLoad;
			bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
			bImg.EndInit();

			memoryStream.Close();

			return bImg;
		}
		public static BitmapImage CreateThumbnail(this BitmapImage bmp, double scale = 10) {
			if (scale < 1) {
				return bmp.Clone();
			}

			BmpBitmapEncoder encoder = new BmpBitmapEncoder();
			MemoryStream memoryStream = new MemoryStream();
			BitmapImage bImg = new BitmapImage();

			encoder.Frames.Add(BitmapFrame.Create(bmp));
			encoder.Save(memoryStream);

			bImg.BeginInit();
			bImg.CacheOption = BitmapCacheOption.OnLoad;
			bImg.DecodePixelHeight = (int)(((double)bmp.PixelHeight) / scale);
			bImg.DecodePixelWidth = (int)(((double)bmp.PixelWidth) / scale);

			bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
			bImg.EndInit();

			memoryStream.Close();

			return bImg;
		}
		public static void ToFile(this BitmapImage bmp, string path, ImageType imgType = ImageType.BMP) {
			FileInfo finfo = new FileInfo(path);
			BitmapEncoder encoder = null;

			switch (imgType) {
				case ImageType.JPG:
					encoder = new JpegBitmapEncoder();
					break;
				case ImageType.PNG:
					encoder = new PngBitmapEncoder();
					break;
				default:
					encoder = new BmpBitmapEncoder();
					break;
			}
			encoder.Frames.Add(BitmapFrame.Create(bmp));

			using (var filestream = finfo.Create())
				encoder.Save(filestream);
		}
		public static void FromFile(this BitmapImage bmp, string path) {
			FileInfo finfo = new FileInfo(path);
			if (!finfo.Exists)
				throw new FileNotFoundException();


			using (
				FileStream stream = finfo.Open(FileMode.Open, FileAccess.Read)) {
				bmp.BeginInit();
				bmp.CacheOption = BitmapCacheOption.OnLoad;
				bmp.StreamSource = stream;
				bmp.EndInit();
			}
		}
		public static void FromStream(this  BitmapImage bmp, Stream src) {
			bmp.BeginInit();
			bmp.CacheOption = BitmapCacheOption.OnLoad;
			bmp.StreamSource = src;
			bmp.EndInit();
		}
	}
}
