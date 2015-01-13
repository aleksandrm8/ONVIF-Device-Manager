using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using odm.ui.core;
using Microsoft.Practices.Unity;
using odm.ui.controls;
using utils;
using Microsoft.Windows.Controls;
using odm.player;
using System.Reactive.Disposables;
using odm.ui.activities;
using onvif.services;

namespace odm.ui.views.CustomAnalytics {
    /// <summary>
    /// Interaction logic for ObjectTrackerView.xaml
    /// </summary>
    public partial class ObjectTrackerView : UserControl , IDisposable {
        public ObjectTrackerView() {
            InitializeComponent();
			//disposables = new CompositeDisposable();
            Loaded += new RoutedEventHandler(ObjectTrackerView_Loaded);
        }
        bool isReady = false;
        bool isLoaded = false;
        void ObjectTrackerView_Loaded(object sender, RoutedEventArgs e) {
            if (isReady && !isLoaded) {
                InitRegionEditor();
            }

            isLoaded = true;
        }

        odm.ui.views.CustomAnalytics.SynesisAnalyticsConfigView.SynesisAnalyticsModel model;

        void GetRose() {
            model.UserRegion.Rose.Down = rose.btnDown;
            model.UserRegion.Rose.DownLeft = rose.btnDownLeft;
            model.UserRegion.Rose.DownRight = rose.btnDownRight;
            model.UserRegion.Rose.Left = rose.btnLeft;
            model.UserRegion.Rose.Right = rose.btnRight;
            model.UserRegion.Rose.Up = rose.btnUp;
            model.UserRegion.Rose.UpLeft = rose.btnUpLeft;
            model.UserRegion.Rose.UpRight = rose.btnUpRight;
        }

        public void Apply() {
            var plist = regeditor.GetRegion();
            List<synesis.Point> synesisPoints = new List<synesis.Point>();
            plist.ForEach(point => {
                var spoint = new synesis.Point();
                spoint.X = (int)point.X;
                spoint.Y = (int)point.Y;
                synesisPoints.Add(spoint);
            });
            model.UserRegion.Points = synesisPoints.ToArray();
            GetRose();
        }
		
		public void SetPlayer(Border player) {
			playerHolder.Child = player;
		}
		
        public void Init(IUnityContainer container, odm.ui.views.CustomAnalytics.SynesisAnalyticsConfigView.SynesisAnalyticsModel model, IVideoInfo videoInfo) {//, string profToken) {
            this.model = model;
            this.container = container;
            this.videoInfo = videoInfo;
		
			//VideoStartup(videoInfo, profToken);
            BindData();
            if (isLoaded) {
                InitRegionEditor();
            }
            isReady = true;
        }

        IUnityContainer container;
        IVideoInfo videoInfo;
        public PropertyObjectTrackerStrings Strings { get { return PropertyObjectTrackerStrings.instance; } }

        void BindData() {
            useTraker.CreateBinding(CheckBox.IsCheckedProperty, model, x => {
                    IsModuleEnabled = x.UseObjectTracker;
                    return x.UseObjectTracker;
                }, (m, v) => {
                m.UseObjectTracker = v;
                IsModuleEnabled = v;
            });

            //numContrastSensitivity.ValueType = typeof(int);
            numContrastSensitivity.Minimum = model.ContrastSensitivityValueMin;
            numContrastSensitivity.Maximum = model.ContrastSensitivityValueMax;
            numContrastSensitivity.CreateBinding(Slider.ValueProperty, model, x => x.ContrastSensivity, (m, v) => {
                m.ContrastSensivity = v;
            });

            //numDisplacement.ValueType = typeof(int);
            numDisplacement.Minimum = model.DisplacementSensitivityValueMin;
            numDisplacement.Maximum = model.DisplacementSensitivityValueMax;
            numDisplacement.CreateBinding(Slider.ValueProperty, model, x => x.DisplacementSensivity, (m, v) => {
                m.DisplacementSensivity = v;
            });

            numAreaMax.Increment = 0.1;
            numAreaMax.FormatString = "F2";
            numAreaMax.Maximum = SynesisAnalyticsConfigView.SynesisAnalyticsModel.ObjectAreaValueMax;

            numAreaMax.CreateBinding(DoubleUpDown.ValueProperty, model, x => x.MaxObjectArea, (m, v) => {
                m.MaxObjectArea = v;
            });
            numAreaMax.CreateBinding(DoubleUpDown.MinimumProperty, model, x => x.MinObjectArea);

            numAreaMin.Increment = 0.1;
            numAreaMin.FormatString = "F2";
            numAreaMin.Minimum = SynesisAnalyticsConfigView.SynesisAnalyticsModel.ObjectAreaValueMin;
            numAreaMin.CreateBinding(DoubleUpDown.ValueProperty, model, x => x.MinObjectArea, (m, v) => {
                m.MinObjectArea = v;
            });
            numAreaMin.CreateBinding(DoubleUpDown.MaximumProperty, model, x => x.MaxObjectArea);
            
            numSpeedMax.Increment = 0.1;
            numSpeedMax.FormatString = "F2";
            numSpeedMax.Minimum = SynesisAnalyticsConfigView.SynesisAnalyticsModel.ObjectSpeedValueMin;
            numSpeedMax.Maximum = SynesisAnalyticsConfigView.SynesisAnalyticsModel.ObjectSpeedValueMax;
            numSpeedMax.CreateBinding(DoubleUpDown.ValueProperty, model, x => x.MaxObjectSpeed, (m, v) => {
                m.MaxObjectSpeed = v;
            });
            
            //System.Windows.Controls.Primitives.ToggleButton tb = new System.Windows.Controls.Primitives.ToggleButton();
            
            //tb.Visibility = System.Windows.Visibility.Collapsed

            numStabilization.Minimum = SynesisAnalyticsConfigView.SynesisAnalyticsModel.StabilizationTimeValueMin;
            numStabilization.Maximum = SynesisAnalyticsConfigView.SynesisAnalyticsModel.StabilizationTimeValueMax;
            numStabilization.CreateBinding(IntegerUpDown.ValueProperty, model, x => x.StabilizationTime, (m, v) => {
                m.StabilizationTime = v;
            });

            rose.btnDown = model.UserRegion.Rose.Down;
            rose.btnDownLeft = model.UserRegion.Rose.DownLeft;
            rose.btnDownRight = model.UserRegion.Rose.DownRight;
            rose.btnLeft = model.UserRegion.Rose.Left;
            rose.btnRight = model.UserRegion.Rose.Right;
            rose.btnUp = model.UserRegion.Rose.Up;
            rose.btnUpLeft = model.UserRegion.Rose.UpLeft;
            rose.btnUpRight = model.UserRegion.Rose.UpRight;
        }
        
        void InitRegionEditor() {
            List<Point> plist = new List<Point>();
            model.UserRegion.Points.ForEach(x => {
                plist.Add(new Point(x.X, x.Y));
            });
            
            regeditor.Init(plist, videoInfo.Resolution);
        }

		public void Dispose() {
			//disposables.Dispose();
			//TODO: release player host
		}

        public bool IsModuleEnabled {
            get { return (bool)GetValue(IsModuleEnabledProperty); }
            set { SetValue(IsModuleEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsModuleEnabledProperty =
            DependencyProperty.Register("IsModuleEnabled", typeof(bool), typeof(ObjectTrackerView));
	}
}
