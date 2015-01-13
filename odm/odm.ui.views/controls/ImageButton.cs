using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace odm.ui {
	public class ImageButton : ButtonBase {
		public static DependencyProperty imageSourceProperty = DependencyProperty.Register("imageSource", typeof(ImageSource), typeof(ImageButton));
		public ImageSource imageSource {
			get { return (ImageSource)GetValue(imageSourceProperty); }
			set { SetValue(imageSourceProperty, value); }
		}
		public ImageButton() {
		}
	}
}