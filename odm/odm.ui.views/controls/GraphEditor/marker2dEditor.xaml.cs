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
using utils;
using odm.ui.views.CustomAnalytics;

namespace odm.ui.controls.GraphEditor {
	/// <summary>
	/// Interaction logic for marker2dEditor.xaml
	/// </summary>
    public partial class marker2dEditor : UserControl, IMarkerEditor {
		public marker2dEditor() {
			InitializeComponent();

            ScaleFactor = 1.0f;

			group = new TransformGroup();
			trans = new TranslateTransform();

			group.Children.Add(trans);

			marker.RenderTransform = group;
			pointerUp.RenderTransform = group;
			pointerDown.RenderTransform = group;

            pointerUp.Cursor = Cursors.SizeNWSE;
            pointerDown.Cursor = Cursors.SizeNWSE;
            marker.Cursor = Cursors.SizeAll;

			marker.MouseDown += new MouseButtonEventHandler(marker_MouseDown);
			marker.MouseMove += new MouseEventHandler(marker_MouseMove);
			marker.MouseUp += new MouseButtonEventHandler(MouseUpHandler);

			pointerUp.MouseDown += new MouseButtonEventHandler(pointerUp_MouseDown);
			pointerUp.MouseMove += new MouseEventHandler(pointerUp_MouseMove);
			pointerUp.MouseUp += new MouseButtonEventHandler(MouseUpHandler);
			pointerDown.MouseDown += new MouseButtonEventHandler(pointerDown_MouseDown);
			pointerDown.MouseMove += new MouseEventHandler(pointerDown_MouseMove);
			pointerDown.MouseUp += new MouseButtonEventHandler(MouseUpHandler);
		}
		
        double _markerPointerR = 5;
        double markerPointerR {
            get {
                double val = int.MaxValue;
                try {
                    val = _markerPointerR * ScaleFactor;
                    if (val > int.MaxValue)
                        val = int.MaxValue;
                } catch (Exception err) {
                    dbg.Error(err);
                    return (double)int.MaxValue;
                }
                return val;
            }
        }
		private TranslateTransform trans;
		private TransformGroup group;
		private Point oldPoint;
		double origOffsetLeft;
		double origOffsetTop;
		double pointerUpOffsetLeft;
		double pointerDownOffsetLeft;
		double pointerUpOffsetTop;
		double pointerDownOffsetTop;
        Size bountRct;
        Action bottomChanged;
        Action<int, bool> ScaleInfo;

        UnitedMarker uniMarker;
        UnitedMarkerCalibration uniCalibration;

        public double ScaleFactor { get; set; }

		//Check if ratio correct
		void CheckScale(double dW, double dH) {
            if (uniMarker == null) return;
            if (uniCalibration == null) return;
            double height = uniCalibration.PhysicalHeight == 0 ? double.MinValue : (double)uniCalibration.PhysicalHeight;
            var ret = dW < ((double)uniCalibration.PhysicalWidth / height) * dH;
            ScaleInfo(uniMarker.ID, ret);
		}
		bool NeedCorrection() {
            if (uniMarker == null) return false;
            if (uniCalibration == null) return false;

			double wdth = 1;
			double hght = 1;
			if (!double.IsNaN(marker.Width))
				wdth = marker.Width;
			if (!double.IsNaN(marker.Height))
				hght = marker.Height;

            bool ret;
            if(uniCalibration.PhysicalHeight == 0)
                ret = wdth < (uniCalibration.PhysicalWidth / Double.MinValue) * hght;
            else
                ret = wdth < (uniCalibration.PhysicalWidth / uniCalibration.PhysicalHeight) * hght;
            return false;
		}
        public void Refresh(double scalefactor) {
            ScaleFactor = scalefactor;
            Display();
            //Refresh();
        }

        void Display() {
            uniMarker.FlashFor1D();

            pointerUp.Width = markerPointerR * 2;
            pointerUp.Height = markerPointerR * 2;
            pointerDown.Width = markerPointerR * 2;
            pointerDown.Height = markerPointerR * 2;

            marker.StrokeThickness = markerPointerR / 2;
            Refresh();

            bottomChanged();
            CheckScale(marker.Width, marker.Height);
        }

		public void Refresh() {
            if (uniMarker == null) return;
            if (uniCalibration == null) return;

            Canvas.SetLeft(number, (uniMarker.TopLeft.X + uniMarker.BottomRight.X) / 2 - number.DesiredSize.Width * ScaleFactor / 2);
            Canvas.SetTop(number, (uniMarker.TopLeft.Y + uniMarker.BottomRight.Y) / 2 - number.DesiredSize.Height * ScaleFactor / 2);
            number.RenderTransform = new ScaleTransform(ScaleFactor, ScaleFactor);

            marker.RadiusX = 4 * ScaleFactor;
            marker.RadiusY = 4 * ScaleFactor;

            Canvas.SetLeft(pointerUp, uniMarker.TopLeft.X - markerPointerR);
            Canvas.SetTop(pointerUp, uniMarker.TopLeft.Y - markerPointerR);
            Canvas.SetLeft(pointerDown, uniMarker.BottomRight.X - markerPointerR);
            Canvas.SetTop(pointerDown, uniMarker.BottomRight.Y - markerPointerR);

            Canvas.SetLeft(marker, uniMarker.TopLeft.X);
            Canvas.SetTop(marker, uniMarker.TopLeft.Y);

            marker.Width = Math.Abs(uniMarker.BottomRight.X - uniMarker.TopLeft.X);
            marker.Height = Math.Abs(uniMarker.BottomRight.Y - uniMarker.TopLeft.Y);

			//Get possible correct width hight
            double phisRatio;
            if(uniCalibration.PhysicalHeight == 0)
                phisRatio = uniCalibration.PhysicalWidth/Double.MinValue;
            else
                phisRatio = uniCalibration.PhysicalWidth / uniCalibration.PhysicalHeight;
			double desiredWidth = marker.Height * phisRatio;
			double desiredHeight;
			if (!NeedCorrection()) { return; }
			if (desiredWidth > bountRct.Width) {
				desiredWidth = bountRct.Width;
                if(uniCalibration.PhysicalWidth == 0)
                    desiredHeight = (desiredWidth / Double.MinValue) * uniCalibration.PhysicalHeight;
                else
                    desiredHeight = (desiredWidth / uniCalibration.PhysicalWidth) * uniCalibration.PhysicalHeight;
				EnlargeWidth(desiredWidth);
				ReduceHeigth(desiredHeight);
			} else {
				EnlargeWidth(desiredWidth);
			}
		}
		void ReduceHeigth(double deltah) {
			Canvas.SetTop(marker, Canvas.GetTop(marker) + marker.Height - deltah/2);
			marker.Height = deltah;

			Canvas.SetTop(pointerUp, Canvas.GetTop(marker) - markerPointerR);
			Canvas.SetTop(pointerDown, Canvas.GetTop(pointerUp) + marker.Height);
		}
		void EnlargeWidth(double deltaw) {
			Canvas.SetLeft(marker, Canvas.GetLeft(marker) + marker.Width / 2 - deltaw / 2);
			marker.Width = deltaw;

			Canvas.SetLeft(pointerUp, Canvas.GetLeft(marker) - markerPointerR);
			Canvas.SetLeft(pointerDown, Canvas.GetLeft(pointerUp) + marker.Width);
		}
        public void Init(UnitedMarkerCalibration uniCalibration, UnitedMarker unitedMarker, Size boundRect, Action bottomChanged, Action<int, bool> ScaleInfo) {
            this.bottomChanged = bottomChanged;
            this.ScaleInfo = ScaleInfo;
            uniMarker = unitedMarker;
            this.uniCalibration = uniCalibration;
            bountRct = boundRect;

            numtext.Text = uniMarker.ID.ToString();
            CheckAndCorrect(uniMarker);

			Display();
		}

        void CheckAndCorrect(UnitedMarker unitedMarker) {
            double topx = uniMarker.TopLeft.X < unitedMarker.BottomRight.X ? uniMarker.TopLeft.X : unitedMarker.BottomRight.X;
            double bottomx = uniMarker.TopLeft.X < unitedMarker.BottomRight.X ? unitedMarker.BottomRight.X : uniMarker.TopLeft.X;

            double topy = uniMarker.TopLeft.Y < unitedMarker.BottomRight.Y ? uniMarker.TopLeft.Y : unitedMarker.BottomRight.Y;
            double bottomy = uniMarker.TopLeft.Y < unitedMarker.BottomRight.Y ? unitedMarker.BottomRight.Y : uniMarker.TopLeft.Y;

            if (topx < 0)
                topx = 0;
            if (topy < 0)
                topy = 0;

            if (topx >= bountRct.Width) topx = bountRct.Width - 1;
            if (topy >= bountRct.Height) topy = bountRct.Height - 1;

            if (bottomx < 0) bottomx = 0;
            if (bottomy < 0) bottomy = 0;

            if (bottomx >= bountRct.Width) bottomx = bountRct.Width - 1;
            if (bottomy >= bountRct.Height) bottomy = bountRct.Height - 1;

            unitedMarker.TopLeft = new OPoint() { X = topx, Y = topy };
            unitedMarker.BottomRight = new OPoint() { X = bottomx, Y = bottomy };
        }

		void marker_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

                double tX = pointerUpOffsetLeft + (newPoint.X - oldPoint.X);
                double tY = pointerUpOffsetTop + (newPoint.Y - oldPoint.Y);

                tX = tX < 0 ? 0 : tX;
                tY = tY < 0 ? 0 : tY;
                tX = tX >= bountRct.Width - markerPointerR ? bountRct.Width - markerPointerR : tX;
                tY = tY >= bountRct.Height - markerPointerR ? bountRct.Height - markerPointerR : tY;

                uniMarker.TopLeft = new OPoint() { X = tX, Y = tY };

                double bX = pointerDownOffsetLeft + (newPoint.X - oldPoint.X);
                double bY = pointerDownOffsetTop + (newPoint.Y - oldPoint.Y);

                bX = bX < markerPointerR ? markerPointerR : bX;
                bY = bY < markerPointerR ? markerPointerR : bY;
                bX = bX >= bountRct.Width ? bountRct.Width - 1 : bX;
                bY = bY >= bountRct.Height ? bountRct.Height - 1 : bY;

                uniMarker.BottomRight = new OPoint() { X = bX, Y = bY };

                Display();
			}
		}
		
		void marker_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(marker);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);
				origOffsetLeft = Canvas.GetLeft(marker);
				origOffsetTop = Canvas.GetTop(marker);

                pointerUpOffsetLeft = Canvas.GetLeft(pointerUp) + markerPointerR;
                pointerUpOffsetTop = Canvas.GetTop(pointerUp) + markerPointerR;
                pointerDownOffsetLeft = Canvas.GetLeft(pointerDown) + markerPointerR;
                pointerDownOffsetTop = Canvas.GetTop(pointerDown) + markerPointerR;

			}
		}
		void pointerDown_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

                double X;
                double Y;

                Y = newPoint.Y >= bountRct.Height ? bountRct.Height - 1 : newPoint.Y;
                X = newPoint.X >= bountRct.Width ? bountRct.Width - 1 : newPoint.X;

                Y = Y <= uniMarker.TopLeft.Y + markerPointerR ? uniMarker.TopLeft.Y + markerPointerR : Y;
                X = X <= uniMarker.TopLeft.X + markerPointerR ? uniMarker.TopLeft.X + markerPointerR : X;

                uniMarker.BottomRight = new OPoint() { Y = Y, X = X };

                Display();
			}
		}
		void pointerDown_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(pointerDown);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);

				pointerDownOffsetTop = Canvas.GetTop(pointerDown);
				pointerDownOffsetLeft = Canvas.GetLeft(pointerDown);
			}
		}
		void pointerUp_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

                double X;
                double Y;

                Y = newPoint.Y < 0 ? 0 : newPoint.Y;
                X = newPoint.X < 0 ? 0 : newPoint.X;

                Y = Y >= uniMarker.BottomRight.Y - markerPointerR ? uniMarker.BottomRight.Y - markerPointerR : Y;
                X = X >= uniMarker.BottomRight.X - markerPointerR ? uniMarker.BottomRight.X - markerPointerR : X;

                uniMarker.TopLeft = new OPoint() { Y = Y, X = X };
                Display();
			}
		}
		void pointerUp_MouseDown(object sender, MouseEventArgs e) {
			e.MouseDevice.Capture(pointerUp);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);

				pointerUpOffsetTop = Canvas.GetTop(pointerUp);
				pointerUpOffsetLeft = Canvas.GetLeft(pointerUp);
			}
		}
		void MouseUpHandler(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(null);
            Display();
		}
    }
}
