using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls;
using odm.player;
using odm.ui.activities;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;
using utils;

namespace odm.ui.views.CustomAnalytics {
    /// <summary>
    /// Interaction logic for DepthCalibrationView.xaml
    /// </summary>
    public partial class DepthCalibrationView : UserControl, IDisposable {
        public DepthCalibrationView() {
			//disposables = new CompositeDisposable();

            matrixValueCollection = new ObservableCollection<MatrixValue>();

            Btn1D = new DelegateCommand(() => {
                SwitchTo1D();
            });
            Btn2D = new DelegateCommand(() => {
                SwitchTo2D();
            });

            InitializeComponent();

            CollapseInfoPanel();

            BtnPauseResume.CreateBinding(Button.ContentProperty, ButtonStrings, x => x.pause);

            deptheditor.SetNotifiers(DispalyDisplacementError, DisplayMarkerScaleError, DisplayMarkerWhidthError);
        }

		//static internal Uri doGetImageSourceFromResource(string psAssemblyName, string psResourceName) {
		//    Uri oUri = new Uri("pack://application:,,,/" + psAssemblyName + ";component/" + psResourceName, UriKind.RelativeOrAbsolute);
		//    return oUri;
		//}
		void player_MouseWheel(object sender, MouseWheelEventArgs e) {
			if (e.Delta > 0) {
				if (Double.IsNaN(viewPortContainer.Height)) {
					scroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
					scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

					viewPortContainer.Height = viewPortContainer.ActualHeight;
					viewPortContainer.Width = viewPortContainer.ActualWidth;
				}
				viewPortContainer.Height += 20;
				viewPortContainer.Width += 20;

				double actualH = scroller.ActualHeight; 
				double actualW = scroller.ActualWidth;

				Point mousePointer = e.GetPosition(scroller);

				if (mousePointer.X < actualW/2) {
				}else{
					scroller.LineRight();
				}
				if (mousePointer.Y < actualH / 2) {
				} else {
					scroller.LineDown();
				}

			} else {
				double actualH = scroller.ActualHeight;
				if (actualH <= viewPortContainer.ActualHeight) {
					viewPortContainer.Height = Double.NaN;
					viewPortContainer.Width = Double.NaN;

					scroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
					scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
				} else {
					viewPortContainer.Height -= 20;
					viewPortContainer.Width -= 20;

					double actualW = scroller.ActualWidth;

					Point mousePointer = e.GetPosition(scroller);

					if (mousePointer.X < actualW / 2) {
						scroller.LineUp();
					} else {
					}
					if (mousePointer.Y < actualH / 2) {
						scroller.LineLeft();
					} else {
					}
				}
			}

			e.Handled = true;
		}

        odm.ui.views.CustomAnalytics.SynesisAnalyticsConfigView.SynesisAnalyticsModel model;
        public PropertyDepthCalibrationStrings Strings {get { return PropertyDepthCalibrationStrings.instance; } }
        public SaveCancelStrings ButtonStrings { get { return SaveCancelStrings.instance; } }
        IUnityContainer container;
        IVideoInfo videoInfo;
        
        enum MarkerMode { 
            M1D,
            M2D
        }
        //Default marker mode - 1d (height merker)
        MarkerMode markerMode = MarkerMode.M1D;

        public ObservableCollection<MatrixValue> matrixValueCollection { get; set; }
        public UnitedMarkerCalibration calibrationMarkers { get; set; }

        #region Notifications
        //Errors notifications
        void DispalyDisplacementError(bool visible) {
            if (visible) {
                infoDisplacement.Visibility = System.Windows.Visibility.Visible;
            } else {
                infoDisplacement.Visibility = System.Windows.Visibility.Collapsed;
            }
            CheckInfoPanel();
        }
        void DisplayMarkerScaleError(int markerNum, bool visible) {
            if (markerNum == 1) {
                DisplayMarker(infoMarker1, visible);
            } else if (markerNum == 2) {
                DisplayMarker(infoMarker2, visible);
            }
        }
        void DisplayMarkerWhidthError(bool visible) {
            if (visible)
                infoMarkerWhidth.Visibility = System.Windows.Visibility.Visible;
            else
                infoMarkerWhidth.Visibility = System.Windows.Visibility.Collapsed;
            CheckInfoPanel();
        }
        void DisplayMarker(FrameworkElement markerinfo, bool visible) {
            if (visible)
                markerinfo.Visibility = System.Windows.Visibility.Visible;
            else
                markerinfo.Visibility = System.Windows.Visibility.Collapsed;
            CheckInfoPanel();
        }
        void CollapseInfoPanel() {
            infoDisplacement.Visibility = System.Windows.Visibility.Collapsed;
            infoMarker1.Visibility = System.Windows.Visibility.Collapsed;
            infoMarker2.Visibility = System.Windows.Visibility.Collapsed;
            infoMarkerWhidth.Visibility = System.Windows.Visibility.Collapsed;
            infoPanel.Visibility = System.Windows.Visibility.Collapsed;
        }
        void CheckInfoPanel() {
            if (infoDisplacement.Visibility == System.Windows.Visibility.Visible ||
                infoMarker1.Visibility == System.Windows.Visibility.Visible ||
                infoMarker2.Visibility == System.Windows.Visibility.Visible ||
                infoMarkerWhidth.Visibility == System.Windows.Visibility.Visible) {
                infoPanel.Visibility = System.Windows.Visibility.Visible;
            } else {
                infoPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        } 
        #endregion

        //Change markers 1d/2d 
        void SwitchTo1D() {
            Marker1DSpecific = System.Windows.Visibility.Visible;
            Marker2DSpecific = System.Windows.Visibility.Collapsed;
            
            markerMode = MarkerMode.M1D;
            CollapseInfoPanel();
            deptheditor.SwitchTo1DMode();
        }
        void SwitchTo2D() {
            Marker1DSpecific = System.Windows.Visibility.Collapsed;
            Marker2DSpecific = System.Windows.Visibility.Visible;

            markerMode = MarkerMode.M2D;
            CollapseInfoPanel();
            deptheditor.SwitchTo2DMode();
        }

		void InitViewPort() {
			playerHolder.MouseWheel += new MouseWheelEventHandler(player_MouseWheel);
			playerHolder.PreviewMouseWheel += new MouseWheelEventHandler((obj, evargs) => { });
			scroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
			scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
			try {
				//Stream CursorStream = Application.GetResourceStream(doGetImageSourceFromResource("odm-ui-views", "images/wheel_zoom.cur")).Stream;
				//Stream CursorStream = new MemoryStream(odm.ui.Resources.wheel_zoom);
				//playerHolder.Cursor = new Cursor(CursorStream);

			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		//Border player;
		public void SetPlayer(Border player) {
			//this.player = player;
			playerHolder.Child = player;
		}
        public void Init(IUnityContainer container, odm.ui.views.CustomAnalytics.SynesisAnalyticsConfigView.SynesisAnalyticsModel model, IVideoInfo videoInfo){//, string profToken) {
            this.model = model;
            this.container = container;
            this.videoInfo = videoInfo;
			
            calibrationMarkers = new UnitedMarkerCalibration(model.Markers);

			InitViewPort();
            FillData();
            InitDepthEditor();
        }
        
        void FillData() {
            valueFocalLength.Increment = 0.1;
            valueFocalLength.FormatString = "F2";
            valueFocalLength.Minimum = 0;
            valueFocalLength.CreateBinding(DoubleUpDown.ValueProperty, calibrationMarkers, x => x.FocalLength, (m, v) => {
                m.FocalLength = v;
            });

            this.CreateBinding(DepthCalibrationView.PhysicalHeightProperty, calibrationMarkers, x => x.PhysicalHeight, (m, v) => {
                m.PhysicalHeight = v;
            });
            this.CreateBinding(DepthCalibrationView.PhysicalWidthProperty, calibrationMarkers, x => x.PhysicalWidth, (m, v) => {
                m.PhysicalWidth = v;
            });

            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item1));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item116));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item118));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item12));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item123));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item125));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item127));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item13));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item132));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item136));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item14));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item15));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item16));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item18));
            matrixValueCollection.Add(new MatrixValue(synesis.MatrixFormat.Item23));

            var r = matrixValueCollection.Where(x => x.mformat == calibrationMarkers.MatrixFormat.mformat).FirstOrDefault();
            valueMatrixFormat.CreateBinding(ComboBox.SelectedItemProperty, calibrationMarkers, x => x.MatrixFormat, (m, v) => {
                m.MatrixFormat = v;
            });
            valueMatrixFormat.SelectedItem = r;
        }

        public void Apply() {
            GetData();
        }
        
        void GetData() {
            if (markerMode == MarkerMode.M1D) {
                //1d
                synesis.HeightMarkerCalibration hmarker = new synesis.HeightMarkerCalibration();
                hmarker.MatrixFormat = calibrationMarkers.MatrixFormat.mformat;
                hmarker.FocalLength = calibrationMarkers.FocalLength;
                List<synesis.HeightMarker> hmarkerslist = new List<synesis.HeightMarker>();
                synesis.HeightMarker hmark = new synesis.HeightMarker();
                List<synesis.SurfaceNormal> snormals = new List<synesis.SurfaceNormal>();
                
                synesis.SurfaceNormal normal = new synesis.SurfaceNormal();
                normal.Point = new synesis.Point() { 
                    X = (int)calibrationMarkers.UnitedMarker1.Top.X, 
                    Y = (int)calibrationMarkers.UnitedMarker1.Top.Y };
                normal.Height = (int)Math.Abs(calibrationMarkers.UnitedMarker1.Bottom.Y - calibrationMarkers.UnitedMarker1.Top.Y);
                snormals.Add(normal);
                
                normal = new synesis.SurfaceNormal();
                normal.Point = new synesis.Point() {
                    X = (int)calibrationMarkers.UnitedMarker2.Top.X,
                    Y = (int)calibrationMarkers.UnitedMarker2.Top.Y
                };
                normal.Height = (int)Math.Abs(calibrationMarkers.UnitedMarker2.Bottom.Y - calibrationMarkers.UnitedMarker2.Top.Y);
                snormals.Add(normal);

                hmark.SurfaceNormals = snormals.ToArray();

                hmark.Height = calibrationMarkers.PhysicalHeight;

                hmarkerslist.Add(hmark);
                
                hmarker.HeightMarkers = hmarkerslist.ToArray();
                
                model.Markers.Item = hmarker;
            } else {
                synesis.CombinedMarkerCalibration cmarker = new synesis.CombinedMarkerCalibration();

                List<synesis.CombinedMarker> cmarkerslist = new List<synesis.CombinedMarker>();
                synesis.CombinedMarker cmark = new synesis.CombinedMarker();
                List<synesis.Rect> srects = new List<synesis.Rect>();

                synesis.Rect rect = new synesis.Rect();
                rect.LeftTop = new synesis.Point() {
                    X = (int)calibrationMarkers.UnitedMarker1.TopLeft.X,
                    Y = (int)calibrationMarkers.UnitedMarker1.TopLeft.Y
                };
                rect.RightBottom = new synesis.Point() {
                    X = (int)calibrationMarkers.UnitedMarker1.BottomRight.X,
                    Y = (int)calibrationMarkers.UnitedMarker1.BottomRight.Y
                };
                srects.Add(rect);

                rect = new synesis.Rect();
                rect.LeftTop = new synesis.Point() {
                    X = (int)calibrationMarkers.UnitedMarker2.TopLeft.X,
                    Y = (int)calibrationMarkers.UnitedMarker2.TopLeft.Y
                };
                rect.RightBottom = new synesis.Point() {
                    X = (int)calibrationMarkers.UnitedMarker2.BottomRight.X,
                    Y = (int)calibrationMarkers.UnitedMarker2.BottomRight.Y
                };
                srects.Add(rect);

                cmark.Rectangles = srects.ToArray();

                cmark.Height = calibrationMarkers.PhysicalHeight;
                cmark.Width = calibrationMarkers.PhysicalWidth;

                cmarkerslist.Add(cmark);

                cmarker.CombinedMarkers = cmarkerslist.ToArray();

                model.Markers.Item = cmarker;
            }
        }

        void InitDepthEditor() {
            deptheditor.Init(videoInfo.Resolution, calibrationMarkers);

            var hmarkerCalibration = model.Markers.Item as synesis.HeightMarkerCalibration;
            if (hmarkerCalibration != null) {
                //1d
                Marker1DSpecific = System.Windows.Visibility.Visible;
                Marker2DSpecific = System.Windows.Visibility.Collapsed;

                markerMode = MarkerMode.M1D;
                deptheditor.SwitchTo1DMode();
                btn1D.IsChecked = true;
            } else { 
                //2d
                Marker1DSpecific = System.Windows.Visibility.Collapsed;
                Marker2DSpecific = System.Windows.Visibility.Visible;

                markerMode = MarkerMode.M2D;
                deptheditor.SwitchTo2DMode();
                btn2D.IsChecked = true;
            }
        }

		public void Dispose() {
			//disposables.Dispose();
			//TODO: release player host
		}

        public int PhysicalWidth {
            get { return (int)GetValue(PhysicalWidthProperty); }
            set { SetValue(PhysicalWidthProperty, value); }
        }
        public static readonly DependencyProperty PhysicalWidthProperty =
            DependencyProperty.Register("PhysicalWidth", typeof(int), typeof(DepthCalibrationView), new PropertyMetadata((obj, ev) => {
                var ctrl = (DepthCalibrationView)obj;
                if (ctrl.deptheditor != null)
                    ctrl.deptheditor.Refresh();
            }));

        public int PhysicalHeight {
            get { return (int)GetValue(PhysicalHeightProperty); }
            set { SetValue(PhysicalHeightProperty, value); }
        }
        public static readonly DependencyProperty PhysicalHeightProperty =
            DependencyProperty.Register("PhysicalHeight", typeof(int), typeof(DepthCalibrationView), new PropertyMetadata((obj, ev) => {
                var ctrl = (DepthCalibrationView)obj;
                if (ctrl.deptheditor != null)
                    ctrl.deptheditor.Refresh();
            }));

        public Visibility Marker2DSpecific {
            get { return (Visibility)GetValue(Marker2DSpecificProperty); }
            set { SetValue(Marker2DSpecificProperty, value); }
        }
        public static readonly DependencyProperty Marker2DSpecificProperty =
            DependencyProperty.Register("Marker2DSpecific", typeof(Visibility), typeof(DepthCalibrationView));

        public Visibility Marker1DSpecific {
            get { return (Visibility)GetValue(Marker1DSpecificProperty); }
            set { SetValue(Marker1DSpecificProperty, value); }
        }
        public static readonly DependencyProperty Marker1DSpecificProperty =
            DependencyProperty.Register("Marker1DSpecific", typeof(Visibility), typeof(DepthCalibrationView));

        public DelegateCommand Btn1D {
            get { return (DelegateCommand)GetValue(Btn1DProperty); }
            set { SetValue(Btn1DProperty, value); }
        }
        public static readonly DependencyProperty Btn1DProperty =
            DependencyProperty.Register("Btn1D", typeof(DelegateCommand), typeof(DepthCalibrationView));

        public DelegateCommand Btn2D {
            get { return (DelegateCommand)GetValue(Btn2DProperty); }
            set { SetValue(Btn2DProperty, value); }
        }
        public static readonly DependencyProperty Btn2DProperty =
            DependencyProperty.Register("Btn2D", typeof(DelegateCommand), typeof(DepthCalibrationView));

        bool isPaused = false;
        private void Button_Click(object sender, RoutedEventArgs e) {
            if (isPaused) {
                isPaused = false;
                BtnPauseResume.CreateBinding(Button.ContentProperty, ButtonStrings, x => x.pause);
            } else {
                isPaused = true;
                BtnPauseResume.CreateBinding(Button.ContentProperty, ButtonStrings, x => x.resume);
            }
        }
	}
    public class MatrixValue {
        public MatrixValue() {
        }
        Dictionary<synesis.MatrixFormat, string> matrixTable = new Dictionary<synesis.MatrixFormat, string>(){
            {synesis.MatrixFormat.Item1, "1"},
            {synesis.MatrixFormat.Item116, "1/1,6"},
            {synesis.MatrixFormat.Item118, "1/1,8"},
            {synesis.MatrixFormat.Item12, "1/2"},
            {synesis.MatrixFormat.Item123, "1/2,3"},
            {synesis.MatrixFormat.Item125, "1/2,5"},
            {synesis.MatrixFormat.Item127, "1/2,7"},
            {synesis.MatrixFormat.Item13, "1/3"},
            {synesis.MatrixFormat.Item132, "1/3,2"},
            {synesis.MatrixFormat.Item136, "1/3,6"},
            {synesis.MatrixFormat.Item14, "1/4"},
            {synesis.MatrixFormat.Item15, "1/5"},
            {synesis.MatrixFormat.Item16, "1/6"},
            {synesis.MatrixFormat.Item18, "1/8"},
            {synesis.MatrixFormat.Item23, "2/3"}
        };
        public MatrixValue(synesis.MatrixFormat mformat) {
            this.mformat = mformat;
            this.name = matrixTable[mformat];
        }
        public synesis.MatrixFormat mformat { get; set; }
        public string name { get; set; }
    }
    //this class used to combine data for all markers types
    public class OPoint: INotifyPropertyChanged {

        public void SetSynesisPoint(synesis.Point spt) {
            point = new Point(spt.X, spt.Y);
        }
        public synesis.Point GetSynesisPoint() {
            return new synesis.Point() { X = (int)x, Y = (int)y };
        }
        public Point point {
            get { return new Point(x, y); }
            set {
                X = value.X;
                Y = value.Y;
            }
        }
        double x;
        double y;
        public double X {
            get { return x;}
            set {
                x = value;
                NotifyPropertyChanged("X");
            }
        }
        public double Y {
            get { return y; }
            set {
                y = value;
                NotifyPropertyChanged("Y");
            }
        }
        private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class UnitedMarker : INotifyPropertyChanged {
        protected UnitedMarker() {

        }
        public UnitedMarker(int ID) {
            this.ID = ID;
        }
        public UnitedMarker(synesis.Rect cmarker, int ID) {
            this.ID = ID;
            TopLeft = new OPoint() { X = cmarker.LeftTop.X, Y = cmarker.LeftTop.Y };
            BottomRight = new OPoint() { X = cmarker.RightBottom.X, Y = cmarker.RightBottom.Y };

            FlashFor1D();
        }
        public UnitedMarker(synesis.SurfaceNormal hmarker, int ID) {
            this.ID = ID;

            Top = new OPoint() { X = hmarker.Point.X, Y = hmarker.Point.Y };
            Bottom = new OPoint() { X = top.X, Y = top.Y + hmarker.Height };

            FlashFor2D();
        }

        public int ID{get; protected set;}

        public void FlashFor1D() {
            bottom.X = (bottomright.X + topleft.X) / 2;
            top.X = bottom.X;

            top.Y = topleft.Y;
            bottom.Y = bottomright.Y;
        }
        public void FlashFor2D() {
            double deltaX = topleft.X - bottomright.X;
            if (deltaX == 0) deltaX = 40;
            bottomright.X = bottom.X + deltaX/2;
            topleft.X = top.X - deltaX/2;

            bottomright.Y = bottom.Y;
            topleft.Y = top.Y;
        }

        OPoint bottomright = new OPoint();
        public OPoint BottomRight {
            get { return bottomright; }
            set {
                bottomright = value;
                NotifyPropertyChanged("BottomRight");
            }
        }        
        OPoint topleft = new OPoint();
        public OPoint TopLeft {
            get { return topleft; }
            set {
                topleft = value;
                NotifyPropertyChanged("TopLeft");
            }
        }

        OPoint bottom = new OPoint();
        public OPoint Bottom {
            get { return bottom; }
            set {
                bottom = value;
                NotifyPropertyChanged("Bottom");
            }
        }
        OPoint top = new OPoint();
        public OPoint Top {
            get { return top; }
            set {
                top = value;
                NotifyPropertyChanged("Top");
            }
        }
        
        private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class UnitedMarkerCalibration:INotifyPropertyChanged {
        private UnitedMarkerCalibration() {

        }
        public UnitedMarkerCalibration(synesis.MarkerCalibration mk) {
            var retHm = mk.Item as synesis.HeightMarkerCalibration;
            if (retHm != null) {
                Init(retHm);
                return;
            }
            var retCm = mk.Item as synesis.CombinedMarkerCalibration;
            if (retCm != null) {
                Init(retCm);
                return;
            }
        }
        void Init(synesis.HeightMarkerCalibration hCalibration) {
            FocalLength = hCalibration.FocalLength;
            MatrixFormat = new MatrixValue(hCalibration.MatrixFormat);

            if (hCalibration.HeightMarkers.Count() != 1)
                return;

            physicalHeight = hCalibration.HeightMarkers[0].Height;
            physicalWidth = physicalHeight;

            if (hCalibration.HeightMarkers[0].SurfaceNormals.Count() != 2)
                return;
            unitedMarker1 = new UnitedMarker(hCalibration.HeightMarkers[0].SurfaceNormals[0], 1);
            unitedMarker2 = new UnitedMarker(hCalibration.HeightMarkers[0].SurfaceNormals[1], 2);
        }
        public UnitedMarkerCalibration(synesis.HeightMarkerCalibration hCalibration) {
            Init(hCalibration);
        }
        void Init(synesis.CombinedMarkerCalibration cCalibration) {
            matrixFormat = new MatrixValue(synesis.MatrixFormat.Item1);
            
            if (cCalibration.CombinedMarkers.Count() != 1)
                return;

            physicalHeight = cCalibration.CombinedMarkers[0].Height;
            physicalWidth = cCalibration.CombinedMarkers[0].Width;

            if (cCalibration.CombinedMarkers[0].Rectangles.Count() != 2)
                return;
            unitedMarker1 = new UnitedMarker(cCalibration.CombinedMarkers[0].Rectangles[0], 1);
            unitedMarker2 = new UnitedMarker(cCalibration.CombinedMarkers[0].Rectangles[1], 2);
        }
        public UnitedMarkerCalibration(synesis.CombinedMarkerCalibration cCalibration) {
            Init(cCalibration);
        }
        float focalLength = 0.0f;
        public float FocalLength{
            get { return focalLength; }
            set {
                focalLength = value;
                NotifyPropertyChanged("FocalLength");
            }
        }
        MatrixValue matrixFormat = new MatrixValue(synesis.MatrixFormat.Item1);
        public MatrixValue MatrixFormat {
            get {
                return matrixFormat;
            }
            set {
                matrixFormat = value;
                NotifyPropertyChanged("MatrixFormat");
            }
        }
        int physicalHeight = 0;
        public int PhysicalHeight {
            get { return physicalHeight; }
            set {
                physicalHeight = value;
                NotifyPropertyChanged("PhysicalHeight");
            }
        }
        int physicalWidth = 0;
        public int PhysicalWidth {
            get { return physicalWidth; }
            set {
                physicalWidth = value;
                NotifyPropertyChanged("PhysicalWidth");
            }
        }

        UnitedMarker unitedMarker1 = new UnitedMarker(1);
        public UnitedMarker UnitedMarker1 {
            get {return unitedMarker1;}
            set {
                unitedMarker1 = value;
                NotifyPropertyChanged("UnitedMarker1");
            }
        }
        UnitedMarker unitedMarker2 = new UnitedMarker(2);
        public UnitedMarker UnitedMarker2 {
            get { return unitedMarker2; }
            set {
                unitedMarker2 = value;
                NotifyPropertyChanged("UnitedMarker2");
            }
        }

		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
