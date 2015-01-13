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

namespace odm.ui.controls.GraphEditor {
	/// <summary>
	/// Interaction logic for RectangleEditor.xaml
	/// </summary>
    public partial class RectangleEditor : UserControl {

        public event EventHandler RectangleChanged = delegate { };

        public RectangleEditor() {
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

            viewBox.SizeChanged += new SizeChangedEventHandler(viewBox_SizeChanged);
            Loaded += new RoutedEventHandler(RectangleEditor_Loaded);
        }

        void RectangleEditor_Loaded(object sender, RoutedEventArgs e) {
            if (isInited) {
                mcanvas.Width = bountRct.Width;
                mcanvas.Height = bountRct.Height;
                Display();
            }
            isLoaded = true;
        }

        void viewBox_SizeChanged(object sender, SizeChangedEventArgs e) {
            double v = e.NewSize.Width;
            if(v == 0)
                v = 0.00000000001;
            ScaleFactor = bountRct.Width / v;
            if (IsLoaded && isInited) {
                Display();
            }
        }

        Point top;
        Point bottom;
        public Point Top {
            get {
                return top;
            }
        }
        public Point Bottom {
            get {
                return bottom;
            }
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

        public double ScaleFactor { get; set; }

        public Size physicalSize;
        public Size PhysicalSize {
            get { return physicalSize; }
            set {
                physicalSize = value;
                if (physicalSize.Height == 0)
                    physicalSize.Height = 1;
                if (physicalSize.Width == 0)
                    physicalSize.Width = 1;
                Refresh();
            }
        }
        //Check if ratio correct
        bool IsUnCorrect(double dW, double dH) {
            return dW < (physicalSize.Width / physicalSize.Height) * dH;
        }
        bool NeedCorrection() {
            //double wdth = 1;
            //double hght = 1;
            //if (!double.IsNaN(marker.Width))
            //    wdth = marker.Width;
            //if (!double.IsNaN(marker.Height))
            //    hght = marker.Height;

            //bool ret = wdth < (physicalSize.Width / physicalSize.Height) * hght;
            return false;
        }
        public void Refresh(double scalefactor) {
            ScaleFactor = scalefactor;
            Display();
            //Refresh();
        }

        void Display() {
            marker.RadiusX = 4 * ScaleFactor;
            marker.RadiusY = 4 * ScaleFactor;

            pointerUp.Width = markerPointerR * 2;
            pointerUp.Height = markerPointerR * 2;
            pointerDown.Width = markerPointerR * 2;
            pointerDown.Height = markerPointerR * 2;

            marker.StrokeThickness = markerPointerR / 2;
            Refresh();
        }

        public void Refresh() {
            Canvas.SetLeft(pointerUp, top.X - markerPointerR);
            Canvas.SetTop(pointerUp, top.Y - markerPointerR);
            Canvas.SetLeft(pointerDown, bottom.X - markerPointerR);
            Canvas.SetTop(pointerDown, bottom.Y - markerPointerR);

            Canvas.SetLeft(marker, top.X);
            Canvas.SetTop(marker, top.Y);

            marker.Width = Math.Abs(bottom.X - top.X);
            marker.Height = Math.Abs(bottom.Y - top.Y);

            //Get possible correct width hight
            //double phisRatio = physicalSize.Width / physicalSize.Height;
            //double desiredWidth = marker.Height * phisRatio;
            //double desiredHeight;
            ////if (!NeedCorrection()) { return; }
            //if (desiredWidth > bountRct.Width) {
            //    desiredWidth = bountRct.Width;
            //    desiredHeight = (desiredWidth / physicalSize.Width) * physicalSize.Height;
            //    EnlargeWidth(desiredWidth);
            //    ReduceHeigth(desiredHeight);
            //} else {
            //    EnlargeWidth(desiredWidth);
            //}
        }
        void ReduceHeigth(double deltah) {
            Canvas.SetTop(marker, Canvas.GetTop(marker) + marker.Height - deltah / 2);
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
        bool isInited = false;
        bool isLoaded = false;
        void OnLoad() {
            mcanvas.Width = bountRct.Width;
            mcanvas.Height = bountRct.Height;

            PhysicalSize = Size.Empty;
            
            Display();
        }
        public void Init(Point p1, Point p2, Size boundRect) {
            bountRct = boundRect;
            if (p1.Y < p2.Y) {
                top.Y = p1.Y;
                bottom.Y = p2.Y;
            } else {
                top.Y = p2.Y;
                bottom.Y = p1.Y;
            }
            if (p1.X < p2.X) {
                top.X = p1.X;
                bottom.X = p2.X;
            } else {
                top.X = p2.X;
                bottom.X = p1.X;
            }

            if (top.X < 0) top.X = 0;
            if (top.Y < 0) top.Y = 0;
            if (top.X >= boundRect.Width) top.X = boundRect.Width - 1;
            if (top.Y >= boundRect.Height) top.Y = boundRect.Height - 1;
            if (bottom.X < 0) bottom.X = 0;
            if (bottom.Y < 0) bottom.Y = 0;
            if (bottom.X >= boundRect.Width) bottom.X = boundRect.Width - 1;
            if (bottom.Y >= boundRect.Height) bottom.Y = boundRect.Height - 1;

            if (isLoaded) {
                mcanvas.Width = bountRct.Width;
                mcanvas.Height = bountRct.Height;
                Display();
            }
            isInited = true;
        }

        void marker_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                Point newPoint = e.GetPosition((IInputElement)mcanvas);

                double tX = pointerUpOffsetLeft + (newPoint.X - oldPoint.X);
                double tY = pointerUpOffsetTop + (newPoint.Y - oldPoint.Y);

                tX = tX < 0 ? 0 : tX;
                tY = tY < 0 ? 0 : tY;
                tX = tX >= bountRct.Width - markerPointerR ? bountRct.Width - markerPointerR : tX;
                tY = tY >= bountRct.Height - markerPointerR ? bountRct.Height - markerPointerR : tY;

                top.X = tX;
                top.Y = tY;

                double bX = pointerDownOffsetLeft + (newPoint.X - oldPoint.X);
                double bY = pointerDownOffsetTop + (newPoint.Y - oldPoint.Y);

                bX = bX < markerPointerR ? markerPointerR : bX;
                bY = bY < markerPointerR ? markerPointerR : bY;
                bX = bX >= bountRct.Width ? bountRct.Width - 1 : bX;
                bY = bY >= bountRct.Height ? bountRct.Height - 1 : bY;

                bottom.X = bX;
                bottom.Y = bY;

                Display();

                RectangleChanged(this, EventArgs.Empty);
            }
        }

        void marker_MouseDown(object sender, MouseButtonEventArgs e) {
            e.MouseDevice.Capture(marker);

            if (e.LeftButton == MouseButtonState.Pressed) {
                oldPoint = e.GetPosition((IInputElement)mcanvas);
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
                Point newPoint = e.GetPosition((IInputElement)mcanvas);

                double X;
                double Y;

                Y = newPoint.Y >= bountRct.Height ? bountRct.Height - 1 : newPoint.Y;
                X = newPoint.X >= bountRct.Width ? bountRct.Width - 1 : newPoint.X;

                Y = Y <= top.Y + markerPointerR ? top.Y + markerPointerR : Y;
                X = X <= top.X + markerPointerR ? top.X + markerPointerR : X;

                bottom.Y = Y;
                bottom.X = X;

                Display();

                RectangleChanged(this, EventArgs.Empty);
            }
        }
        void pointerDown_MouseDown(object sender, MouseButtonEventArgs e) {
            e.MouseDevice.Capture(pointerDown);

            if (e.LeftButton == MouseButtonState.Pressed) {
                oldPoint = e.GetPosition((IInputElement)mcanvas);

                pointerDownOffsetTop = Canvas.GetTop(pointerDown);
                pointerDownOffsetLeft = Canvas.GetLeft(pointerDown);
            }
        }
        void pointerUp_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                Point newPoint = e.GetPosition((IInputElement)mcanvas);

                double X;
                double Y;

                Y = newPoint.Y < 0 ? 0 : newPoint.Y;
                X = newPoint.X < 0 ? 0 : newPoint.X;

                Y = Y >= bottom.Y - markerPointerR ? bottom.Y - markerPointerR : Y;
                X = X >= bottom.X - markerPointerR ? bottom.X - markerPointerR : X;

                top.Y = Y;
                top.X = X;

                Display();

                RectangleChanged(this, EventArgs.Empty);
            }
        }
        void pointerUp_MouseDown(object sender, MouseEventArgs e) {
            e.MouseDevice.Capture(pointerUp);

            if (e.LeftButton == MouseButtonState.Pressed) {
                oldPoint = e.GetPosition((IInputElement) mcanvas);

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
