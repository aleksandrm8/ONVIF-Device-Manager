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
    /// Interaction logic for TripWireEditor.xaml
    /// </summary>
    public partial class TripWireEditor : UserControl {
        public TripWireEditor() {
            InitializeComponent();
            Loaded += new RoutedEventHandler(RegionEditor_Loaded);
            viewBox.SizeChanged+=new SizeChangedEventHandler(viewBox_SizeChanged);
        }

        bool isinit;
        bool isloaded;
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

        //private Point oldPoint;
        //double origOffsetLeft;
        //double origOffsetTop;
        Size bountRct;
        public double ScaleFactor { get; set; }
        public Point P1;
        public Point P2;
        Ellipse ellipse1;
        Ellipse ellipse2;
        Line line;
        public enum TripWireDirection {
            FromBoth = 1,
            FromLeft = 2,
            FromRight = 3
        }
        public TripWireDirection ruleDirection = TripWireDirection.FromBoth;

        void RegionEditor_Loaded(object sender, RoutedEventArgs e) {
            if (isinit && !isloaded) {
                Display();
            }
            isloaded = true;
        }
        void viewBox_SizeChanged(object sender, SizeChangedEventArgs e) {
            ScaleFactor = bountRct.Width / e.NewSize.Width;
            InitPromitives();
            Refresh();
        }

        Line leftLine;
        Line rightLine;

        void CreateDirectionPoints(Point p1, Point p2) {
            Point pt = new Point((P1.X + P2.X) / 2, (P1.Y + P2.Y) / 2);            

            double catetX = P1.X - P2.X;
            double catetY = P1.Y - P2.Y;
            double gipothen = Math.Sqrt(catetX * catetX + catetY * catetY);
            
            double angle;

            if (gipothen == 0)
                angle = 0;
            else {
                angle = (Math.Acos(catetX / gipothen));// * 180 / Math.PI;
            }

            Point leftPStart = new Point();
            Point rightPStart = new Point();

            Point leftP = new Point();
            Point rightP = new Point();

            if (p2.Y <= p1.Y) {
                leftPStart.X = pt.X + Math.Sin(angle) * markerPointerR;
                leftP.X = pt.X + Math.Sin(angle) * markerPointerR * 3;

                rightPStart.X = pt.X - Math.Sin(angle) * markerPointerR;
                rightP.X = pt.X - Math.Sin(angle) * markerPointerR * 3;
            } else {
                leftPStart.X = pt.X - Math.Sin(angle) * markerPointerR;
                leftP.X = pt.X - Math.Sin(angle) * markerPointerR * 3;

                rightPStart.X = pt.X + Math.Sin(angle) * markerPointerR;
                rightP.X = pt.X + Math.Sin(angle) * markerPointerR * 3;
            }
            if (p2.X >= p1.X) {
                leftPStart.Y = pt.Y - Math.Cos(angle) * markerPointerR;
                leftP.Y = pt.Y - Math.Cos(angle) * markerPointerR * 3;

                rightPStart.Y = pt.Y + Math.Cos(angle) * markerPointerR;
                rightP.Y = pt.Y + Math.Cos(angle) * markerPointerR * 3;
            } else {
                leftPStart.Y = pt.Y - Math.Cos(angle) * markerPointerR;
                leftP.Y = pt.Y - Math.Cos(angle) * markerPointerR * 3;

                rightPStart.Y = pt.Y + Math.Cos(angle) * markerPointerR;
                rightP.Y = pt.Y + Math.Cos(angle) * markerPointerR * 3;
            }

            leftLine.StrokeEndLineCap = PenLineCap.Triangle;
            leftLine.StrokeThickness = markerPointerR * 3;
            leftLine.X1 = leftPStart.X;
            leftLine.X2 = leftP.X;
            leftLine.Y1 = leftPStart.Y;
            leftLine.Y2 = leftP.Y;

            rightLine.StrokeEndLineCap = PenLineCap.Triangle;
            rightLine.StrokeThickness = markerPointerR * 3;
            rightLine.X1 = rightPStart.X;
            rightLine.X2 = rightP.X;
            rightLine.Y1 = rightPStart.Y;
            rightLine.Y2 = rightP.Y;

            switch (ruleDirection) {
                case TripWireDirection.FromRight:
                    leftLine.Stroke = new SolidColorBrush(Red);
                    rightLine.Stroke = new SolidColorBrush(tRed);
                    break;
                case TripWireDirection.FromLeft:
                    rightLine.Stroke = new SolidColorBrush(Red);
                    leftLine.Stroke = new SolidColorBrush(tRed);
                    break;
                case TripWireDirection.FromBoth:
                    leftLine.Stroke = new SolidColorBrush(Red);
                    rightLine.Stroke = new SolidColorBrush(Red);
                    break;
                default:
                    leftLine.Stroke = new SolidColorBrush(tRed);
                    rightLine.Stroke = new SolidColorBrush(tRed);
                    break;
            }

            //mcanvas.Children.Add(leftDirection);
        }
        Color Red = Color.FromRgb(0xFF, 0x00, 0);
        Color tRed = Color.FromArgb(100, 0xFF, 0x00, 0);
        Line CreateLine(Point p1, Point p2) {
            Line ln = new Line();
            ln.Style = (Style)FindResource("RegionLineStyle");
            ln.Stroke = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0));
            ln.StrokeThickness = markerPointerR / 2;
            ln.X1 = p1.X;
            ln.X2 = p2.X;
            ln.Y1 = p1.Y;
            ln.Y2 = p2.Y;

            return ln;
        }

        Ellipse CresteElipse(Point pt, int pnum) {
            Ellipse ell = new Ellipse();
            ell.Cursor = Cursors.Hand;

            ell.Style = (Style)FindResource("TrackerPointer");
            ell.Fill = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0));
            ell.Stroke = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0));
            //Marker position
            Canvas.SetTop(ell, pt.Y - markerPointerR);
            Canvas.SetLeft(ell, pt.X - markerPointerR);

            ell.Width = markerPointerR * 2;
            ell.Height = markerPointerR * 2;
            //ell.RenderTransform = group;
            ell.Focusable = true;
            ell.Tag = pt;

            ell.MouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => {
                var ret = e.MouseDevice.Capture((Ellipse)sender, CaptureMode.Element);
                //if (e.LeftButton == MouseButtonState.Pressed) {
                //    oldPoint = e.GetPosition(this);
                //    origOffsetLeft = Canvas.GetLeft((Ellipse)sender);
                //    origOffsetTop = Canvas.GetTop((Ellipse)sender);
                //}
            });
            ell.MouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => {
                e.MouseDevice.Capture(null);
            });
            ell.MouseMove += new MouseEventHandler((object sender, MouseEventArgs e) => {
                if (e.LeftButton == MouseButtonState.Pressed) {
                    Point newPoint = e.GetPosition(mcanvas);

                    double X = newPoint.X < 0 ? 0 : newPoint.X;
                    double Y = newPoint.Y < 0 ? 0 : newPoint.Y;
                    X = X >= bountRct.Width ? bountRct.Width - 1 : X;
                    Y = Y >= bountRct.Height ? bountRct.Height - 1 : Y;

                    var ellipse = (Ellipse)sender;
                    Canvas.SetLeft(ellipse, X - markerPointerR);
                    Canvas.SetTop(ellipse, Y - markerPointerR);

                    if (pnum == 1) {
                        P1.X = X;
                        P1.Y = Y;
                    } else {
                        P2.X = X;
                        P2.Y = Y;
                    }

                    Refresh();
                }
            });


            return ell;
        }

        void InitPromitives() {
            if(ellipse1 != null)
                mcanvas.Children.Remove(ellipse1);
            if(ellipse2 != null)
                mcanvas.Children.Remove(ellipse2);
            if (line != null)
                mcanvas.Children.Remove(line);

            ellipse1 = CresteElipse(P1, 1);

            ellipse2 = CresteElipse(P2, 2);

            line = CreateLine(P1, P2);

            mcanvas.Children.Add(ellipse1);
            mcanvas.Children.Add(ellipse2);
            mcanvas.Children.Add(line);
        }
        
        void Display() {
            mcanvas.Width = bountRct.Width;
            mcanvas.Height = bountRct.Height;

            mcanvas.Children.Add(leftLine);
            mcanvas.Children.Add(rightLine);

            InitPromitives();
            
            Refresh();
        }
        public void Refresh() {
            mcanvas.Children.Remove(line);
            line = CreateLine(P1, P2);
            CreateDirectionPoints(P1, P2);
            mcanvas.Children.Add(line);
        }

        public void SetDirection(TripWireDirection direction) {
            ruleDirection = direction;
            Refresh();
        }
        public void Init(Point P1, Point P2, Size bound, TripWireDirection direction) {
            ruleDirection = direction;
            this.P1 = P1;
            this.P2 = P2;

            leftLine = new Line();
            rightLine = new Line();
            leftLine.Cursor = Cursors.Hand;
            rightLine.Cursor = Cursors.Hand;
            leftLine.PreviewMouseLeftButtonDown+=new MouseButtonEventHandler((obj, evargs)=>{
                if(ruleDirection != TripWireDirection.FromRight)
                    ruleDirection = ruleDirection ^ TripWireDirection.FromRight;
                Refresh();
            });
            rightLine.PreviewMouseLeftButtonDown += new MouseButtonEventHandler((obj, evargs) => {
                if (ruleDirection != TripWireDirection.FromLeft)
                    ruleDirection = ruleDirection ^ TripWireDirection.FromLeft;
                Refresh();
            });

            bountRct = bound;
            if (isloaded) {
                Display();
            }
            isinit = true;
        }
    }
}
