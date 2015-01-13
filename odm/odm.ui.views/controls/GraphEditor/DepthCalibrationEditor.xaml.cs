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
using System.Xml;
using System.ComponentModel;
using odm.ui.views.CustomAnalytics;
using odm.infra;

namespace odm.ui.controls.GraphEditor {
    /// <summary>
    /// Interaction logic for DepthCalibrationEditor.xaml
    /// </summary>
    public partial class DepthCalibrationEditor : UserControl {
        public DepthCalibrationEditor() {
            InitializeComponent();

            viewBox.SizeChanged += new SizeChangedEventHandler(viewBox_SizeChanged);
        }

        public void SetNotifiers(Action<bool> displacementNotify, Action<int, bool> markerNotify, Action<bool> markerWidthNotify) {
            this.displacementNotify = displacementNotify;
            this.markerNotify = markerNotify;
            this.markerWidthNotify = markerWidthNotify;
        }
        
        Action<bool> displacementNotify; 
        Action<int, bool> markerNotify;
        Action<bool> markerWidthNotify;

        List<marker1dEditor> m1EditorList = new List<marker1dEditor>();
        List<marker2dEditor> m2EditorList = new List<marker2dEditor>();

        void NotifyDisplacement(double Y1, double Y2) {
            var disp = Math.Abs(Y1 - Y2);
            if ((bound.Height / 4) > disp) {
                displacementNotify(true);
            } else {
                displacementNotify(false);
            }
        }
        void bottomChanged() {
            if (m1EditorList.Count == 2) {
                NotifyDisplacement(markerCalibration.UnitedMarker1.Bottom.Y, markerCalibration.UnitedMarker2.Bottom.Y);
            } else if (m2EditorList.Count == 2) {
                NotifyDisplacement(markerCalibration.UnitedMarker1.BottomRight.Y, markerCalibration.UnitedMarker2.BottomRight.Y);
                //check hier than wider
                var m1width = Math.Abs(markerCalibration.UnitedMarker1.TopLeft.X - markerCalibration.UnitedMarker1.BottomRight.X);
                var m2width = Math.Abs(markerCalibration.UnitedMarker2.TopLeft.X - markerCalibration.UnitedMarker2.BottomRight.X);

                if (markerCalibration.UnitedMarker1.BottomRight.Y > markerCalibration.UnitedMarker2.BottomRight.Y) {
                    //m1width
                    markerWidthNotify(m1width < m2width);
                } else {
                    //m2width
                    markerWidthNotify(m2width < m1width);
                } 
            }
        }

        void scaleInfo(int mid, bool iserror) {
            markerNotify(mid, iserror);
        }

        void viewBox_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (e.NewSize.Width == 0)
                return;
            scaleFactor = bound.Width / e.NewSize.Width;
            Refresh();
        }
        
        Size bound;
        
        double scaleFactor = 1.0f;
        public double ScaleFactor { 
            get {
                return scaleFactor;
            }
        }

        UnitedMarkerCalibration markerCalibration;

        public void Init(Size bound, UnitedMarkerCalibration markerCalibration) {
            this.bound = bound;

            this.markerCalibration = markerCalibration;

            mcanvas.Width = bound.Width;
            mcanvas.Height = bound.Height;
        }
        
        public void Refresh() {
            m1EditorList.ForEach(x => {
                var ctrl = x as IMarkerEditor;
                if (ctrl != null) {
                    ctrl.Refresh(ScaleFactor);
                }
            });
            m2EditorList.ForEach(x => {
                var ctrl = x as IMarkerEditor;
                if (ctrl != null) {
                    ctrl.Refresh(ScaleFactor);
                }
            });
        }

        public void SwitchTo1DMode() {
            mcanvas.Children.Clear();
            
            m1EditorList.Clear(); 
            m2EditorList.Clear();

            marker1dEditor meditor = new marker1dEditor();
            meditor.ScaleFactor = ScaleFactor;
            meditor.Init(markerCalibration.UnitedMarker1, bound, bottomChanged);

            mcanvas.Children.Add(meditor);
            m1EditorList.Add(meditor);

            meditor = new marker1dEditor();
            meditor.ScaleFactor = ScaleFactor;
            meditor.Init(markerCalibration.UnitedMarker2, bound, bottomChanged);

            mcanvas.Children.Add(meditor);
            m1EditorList.Add(meditor);
        }
        public void SwitchTo2DMode() {
            mcanvas.Children.Clear();

            m1EditorList.Clear();
            m2EditorList.Clear();

            marker2dEditor meditor = new marker2dEditor();
            meditor.ScaleFactor = ScaleFactor;
            meditor.Init(markerCalibration, markerCalibration.UnitedMarker1, bound, bottomChanged, scaleInfo);

            mcanvas.Children.Add(meditor);
            m2EditorList.Add(meditor);

            meditor = new marker2dEditor();
            meditor.ScaleFactor = ScaleFactor;
            meditor.Init(markerCalibration, markerCalibration.UnitedMarker2, bound, bottomChanged, scaleInfo);

            mcanvas.Children.Add(meditor);
            m2EditorList.Add(meditor);
        }
    }

    public interface IMarkerEditor { 
        void Refresh(double scaleFactor);
    }
}
