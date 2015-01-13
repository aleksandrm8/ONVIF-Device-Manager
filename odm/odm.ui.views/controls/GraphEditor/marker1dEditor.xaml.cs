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
using System.ComponentModel;
using odm.ui.views.CustomAnalytics;

namespace odm.ui.controls.GraphEditor {
	/// <summary>
	/// Interaction logic for marker1dEditor.xaml
	/// </summary>
    public partial class marker1dEditor : UserControl, IMarkerEditor {
		public marker1dEditor() {
			InitializeComponent();

            ScaleFactor = 1.0f;

			group = new TransformGroup();
			trans = new TranslateTransform();

			group.Children.Add(trans);

			marker.RenderTransform = group;
			pointerUp.RenderTransform = group;
			pointerDown.RenderTransform = group;

            pointerUp.Cursor = Cursors.SizeNS;
            pointerDown.Cursor = Cursors.SizeNS;
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
		double _markerWidth = 40;
        double markerWidth {
            get {
                return _markerWidth * ScaleFactor;
            }
        }
		private TranslateTransform trans;
		private TransformGroup group;
		private Point oldPoint;
		double pointerUpOffsetLeft;
		double pointerDownOffsetLeft;
		double pointerUpOffsetTop;
		double pointerDownOffsetTop;
		Size bountRct;
        Action bottomChanged;

        UnitedMarker uniMarker;

        public double ScaleFactor { get; set; }

        public void Refresh(double scalefactor) {
            ScaleFactor = scalefactor;
            Display();
        }

        void Display() {
            uniMarker.FlashFor2D();

            marker.RadiusX = 4 * ScaleFactor;
            marker.RadiusY = 4 * ScaleFactor;

            pointerUp.Width = markerPointerR * 2;
            pointerUp.Height = markerPointerR * 2;
            pointerDown.Width = markerPointerR * 2;
            pointerDown.Height = markerPointerR * 2;

            marker.StrokeThickness = markerPointerR / 2;

            Canvas.SetLeft(pointerUp, uniMarker.Top.X - markerPointerR);
            Canvas.SetTop(pointerUp, uniMarker.Top.Y - markerPointerR);
            Canvas.SetLeft(pointerDown, uniMarker.Bottom.X - markerPointerR);
            Canvas.SetTop(pointerDown, uniMarker.Bottom.Y - markerPointerR);

            Canvas.SetLeft(marker, uniMarker.Top.X - markerWidth / 2);
            Canvas.SetTop(marker, uniMarker.Top.Y);

            marker.Width = markerWidth;
            marker.Height = Math.Abs(uniMarker.Bottom.Y - uniMarker.Top.Y);

            bottomChanged();
        }

		public void Init(UnitedMarker unitedMarker, Size boundRect, Action bottomChanged) {
            this.bottomChanged = bottomChanged;
            this.uniMarker = unitedMarker;
            bountRct = boundRect;

            CheckAndCorrect(uniMarker);

            Display();
		}

        void CheckAndCorrect(UnitedMarker unitedMarker) {
            double topx = unitedMarker.Top.X;
            double topy = unitedMarker.Top.Y;
            double bottomx = unitedMarker.Bottom.X;
            double bottomy = unitedMarker.Bottom.Y;

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

            unitedMarker.Top = new OPoint() { X = topx, Y = topy};
            unitedMarker.Bottom = new OPoint() { X = bottomx, Y = bottomy };
        }

        void marker_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);
                
                double tX = pointerUpOffsetLeft + (newPoint.X - oldPoint.X);
                double tY = pointerUpOffsetTop + (newPoint.Y - oldPoint.Y);

                tX = tX < 0 ? 0 : tX;
                tY = tY < 0 ? 0 : tY;
                tX = tX >= bountRct.Width ? bountRct.Width - 1 : tX;
                tY = tY >= bountRct.Height - markerPointerR ? bountRct.Height - markerPointerR : tY;

                uniMarker.Top = new OPoint() { X = tX, Y = tY };

                double bX = pointerDownOffsetLeft + (newPoint.X - oldPoint.X);
                double bY = pointerDownOffsetTop + (newPoint.Y - oldPoint.Y);

                bX = bX < 0 ? 0 : bX;
                bY = bY < markerPointerR ? markerPointerR : bY;
                bX = bX >= bountRct.Width ? bountRct.Width - 1 : bX;
                bY = bY >= bountRct.Height ? bountRct.Height - 1 : bY;

                uniMarker.Bottom = new OPoint() { X = bX, Y = bY };

                Display();
			}
		}
		void marker_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(marker);

			if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 1) {
				oldPoint = e.GetPosition(this);
                //origOffsetLeft = Canvas.GetLeft(marker);
                //origOffsetTop = Canvas.GetTop(marker);

				pointerUpOffsetLeft = Canvas.GetLeft(pointerUp) + markerPointerR;
                pointerUpOffsetTop = Canvas.GetTop(pointerUp) + markerPointerR;
                pointerDownOffsetLeft = Canvas.GetLeft(pointerDown) + markerPointerR;
                pointerDownOffsetTop = Canvas.GetTop(pointerDown) + markerPointerR;
			}
		}
		void pointerDown_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

                double Y;

                Y = newPoint.Y >= bountRct.Height ? bountRct.Height : newPoint.Y;
                Y = Y <= uniMarker.Top.Y + markerPointerR ? uniMarker.Top.Y + markerPointerR : Y;

                uniMarker.Bottom = new OPoint() { Y = Y, X = uniMarker.Bottom.X };
                Display();
			}
		}
		void pointerDown_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(pointerDown);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);

				pointerDownOffsetTop = Canvas.GetTop(pointerDown);
			}
		}
		void pointerUp_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

                double Y;

                Y = newPoint.Y < 0 ? 0 : newPoint.Y;
                Y = Y >= uniMarker.Bottom.Y - markerPointerR ? uniMarker.Bottom.Y - markerPointerR : Y;

                uniMarker.Top = new OPoint(){Y = Y, X = uniMarker.Bottom.X};
                Display();
			}
		}
		void pointerUp_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(pointerUp);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);

				pointerUpOffsetTop = Canvas.GetTop(pointerUp);
			}
		}
		void MouseUpHandler(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(null);
		}
    }
}
