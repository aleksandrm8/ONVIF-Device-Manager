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
	/// Interaction logic for RegionEditor.xaml
	/// </summary>
	public partial class RegionEditor : UserControl {
		public RegionEditor() {
			InitializeComponent();

			group = new TransformGroup();
			trans = new TranslateTransform();
			group.Children.Add(trans);
            Loaded += new RoutedEventHandler(RegionEditor_Loaded);

            viewBox.SizeChanged += new SizeChangedEventHandler(viewBox_SizeChanged);
		}

		List<Ellipse> elipsesList = new List<Ellipse>();
		List<Point> pointsList = new List<Point>();
		List<Line> linesList = new List<Line>();

        double _markerPointerR = 5;
        double markerPointerR { 
            get {
                double val = int.MaxValue;
                try {
                    val = _markerPointerR * ScaleFactor;
                    if (val > int.MaxValue)
                        val = int.MaxValue;
                }catch(Exception err){
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
		Rect bountRct;
		int maxPoints = 10;
		int minPoints = 3;
        public double ScaleFactor { get; set; }

        void viewBox_SizeChanged(object sender, SizeChangedEventArgs e) {
            ScaleFactor = bountRct.Width / e.NewSize.Width;
            Refresh();
        }
        void RefreshLines() {
            region.Points.Clear();
            linesList.ForEach(x => {
                mcanvas.Children.Remove(x);
            });
            elipsesList.ForEach(x => {
                mcanvas.Children.Remove(x);
            });
            linesList.Clear();
            pointsList.Clear();

            elipsesList.ForEach(x => {
                Point pt = new Point(Canvas.GetLeft(x) + markerPointerR, Canvas.GetTop(x) + markerPointerR);
                region.Points.Add(pt);
                pointsList.Add(pt);
            });

            linesList = CreateLinesList(pointsList);
            linesList.ForEach(x => { mcanvas.Children.Add(x); });
            elipsesList.ForEach(x => { mcanvas.Children.Add(x); });
        }
        public void Refresh() {
            region.Points.Clear();
            region.Points = new PointCollection(pointsList);

            //Refresh lines
            linesList.ForEach(x => {
                mcanvas.Children.Remove(x);
            });
            linesList.Clear();

            linesList = CreateLinesList(pointsList);
            linesList.ForEach(x => { mcanvas.Children.Add(x); });

            //refresh elipses
            elipsesList.ForEach(x => {
                mcanvas.Children.Remove(x);
            });
            elipsesList.Clear();

            pointsList.ForEach(x => {
                var ell = CresteElipse(x);
                elipsesList.Add(ell);
                mcanvas.Children.Add(ell);
            });
        }

        void RegionEditor_Loaded(object sender, RoutedEventArgs e) {
            if (isinit && !isloaded) {
                Display(tmp_inplst, tmp_bound);
            }
            isloaded = true;
        }
        void Display(List<Point> inplst, Size bound) {
            mcanvas.Width = bound.Width;
            mcanvas.Height = bound.Height;

            //fix unproper values if neaded
            List<Point> plst = new List<Point>();
            inplst.ForEach(x => {
                if (x.X < 0) x.X = 0;
                if (x.Y < 0) x.Y = 0;
                if (x.X >= bountRct.Width) x.X = bountRct.Width - 1;
                if (x.Y >= bountRct.Height) x.Y = bountRct.Height - 1;
                plst.Add(x);
            });

            pointsList.Clear();
            pointsList.AddRange(plst);

            Refresh();
        }
        List<Point> tmp_inplst;
        Size tmp_bound;
        bool isloaded = false;
        bool isinit = false;
        public void Init(List<Point> inplst, Size bound, int maxPoints) {
            this.maxPoints = maxPoints;
            Init(inplst, bound);
        }
        public void Init(List<Point> inplst, Size bound) {
            bountRct = new Rect(0, 0, bound.Width, bound.Height);
            if (!isloaded) {
                tmp_bound = bound;
                tmp_inplst = inplst;
            } else {
                Display(inplst, bound);
            }
            isinit = true;
		}

		void PointAdded() {
			region.Points.Clear();
			linesList.ForEach(x => {
				mcanvas.Children.Remove(x);
			});
			elipsesList.ForEach(x => {
				mcanvas.Children.Remove(x);
			});
			linesList.Clear();
			elipsesList.Clear();

			linesList = CreateLinesList(pointsList);
			linesList.ForEach(x => { mcanvas.Children.Add(x); });
			pointsList.ForEach(x => {
				region.Points.Add(x);
			});
			pointsList.ForEach(x => {
				var ell = CresteElipse(x);
				elipsesList.Add(ell);
				mcanvas.Children.Add(ell);
			});
		}
		
		List<Line> CreateLinesList(List<Point> plst) {
			List<Line> linesLst = new List<Line>();
			for (int x = 1; x < plst.Count; x++) {
				if (x == plst.Count - 1) {
					linesLst.Add(CreateLine(plst[x - 1], plst[x]));
					linesLst.Add(CreateLine(plst[x],plst[0]));
				} else
					linesLst.Add(CreateLine(plst[x - 1], plst[x]));
			}
			return linesLst;
		}
		Line CreateLine(Point p1, Point p2) {
			Line ln = new Line();

            ln.Cursor = Cursors.Cross;

			ln.Style = (Style)FindResource("RegionLineStyle");
            ln.StrokeThickness = markerPointerR / 2;
			ln.X1 = p1.X;
			ln.X2 = p2.X;
			ln.Y1 = p1.Y;
			ln.Y2 = p2.Y;
			ln.RenderTransform = group;
			ln.MouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => {
				if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2) {
					if (pointsList.Count >= maxPoints)
						return;
                    Point nPoint = e.GetPosition((IInputElement)sender);

					Point lpt = new Point(((Line)sender).X2, ((Line)sender).Y2);
					Point tmp = pointsList.Where(x => x.X == lpt.X && x.Y == lpt.Y ).FirstOrDefault();
					int ind = pointsList.IndexOf(tmp);
					pointsList.Insert(ind, nPoint);
					PointAdded();
				}
			});
			return ln;
		}
        
        
		Ellipse CresteElipse(Point pt) {
			Ellipse ell = new Ellipse();

            ell.Cursor = Cursors.Hand;

			ell.Style = (Style)FindResource("TrackerPointer");
			//Marker position
            Canvas.SetTop(ell, pt.Y - markerPointerR);
            Canvas.SetLeft(ell, pt.X - markerPointerR);

            ell.Width = markerPointerR * 2;
            ell.Height = markerPointerR * 2;
			ell.RenderTransform = group;
			ell.Focusable = true;
			ell.Tag = pt;

			ell.MouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => { 
				var ret = e.MouseDevice.Capture((Ellipse)sender, CaptureMode.Element);
				if (e.LeftButton == MouseButtonState.Pressed) {
					oldPoint = e.GetPosition(this);
					origOffsetLeft = Canvas.GetLeft((Ellipse)sender);
					origOffsetTop = Canvas.GetTop((Ellipse)sender);
				}
				if (e.RightButton == MouseButtonState.Pressed && e.ClickCount ==2) {
					if (pointsList.Count <= minPoints)
						return;
					Ellipse elps = (Ellipse)sender;
					pointsList.Remove((Point)elps.Tag);
					PointAdded();
				}
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
                    Canvas.SetLeft(ellipse, X-markerPointerR);
                    Canvas.SetTop(ellipse, Y-markerPointerR);


                    pointsList.Clear();
                    elipsesList.ForEach(x => {
                        pointsList.Add(new Point(X, Y));
                    });
                    
					RefreshLines();
                    //Refresh();
				}
			});
			

			return ell;
		}

		public List<Point> GetRegion() {
			return pointsList;
		}
	}
}

