using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace odm.ui.controls.GraphEditor {
	public class DrawingConverter {
		public static System.Drawing.Point PointWpfToForm(System.Windows.Point inPoint) {
			return new System.Drawing.Point((int)inPoint.X, (int)inPoint.Y);
		}
		public static System.Windows.Point PointFormToWpf(System.Drawing.Point inPoint) {
			return new System.Windows.Point(inPoint.X, inPoint.Y);
		}
		public static System.Drawing.Rectangle RectToRectangle(System.Windows.Rect inRect) {
			return new System.Drawing.Rectangle(PointWpfToForm(inRect.TopLeft), new System.Drawing.Size((int)inRect.Width,(int)inRect.Height));
		}
		public static System.Windows.Rect RectangleToRect(System.Drawing.Rectangle inRect) {
			return new System.Windows.Rect(PointFormToWpf(inRect.Location), new System.Windows.Point(inRect.Right, inRect.Bottom));
		}
	}
}
