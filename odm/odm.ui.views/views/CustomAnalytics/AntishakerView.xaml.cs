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
using odm.ui.core;
using Microsoft.Practices.Unity;
using System.Xml;
using utils;
using odm.ui.controls;
using odm.player;
using System.Reactive.Disposables;
using odm.ui.activities;
using onvif.services;

namespace odm.ui.views.CustomAnalytics {
	/// <summary>
	/// Interaction logic for AntishakerView.xaml
	/// </summary>
	public partial class AntishakerView : UserControl, IDisposable {
		public AntishakerView() {
			InitializeComponent();
		}
		odm.ui.views.CustomAnalytics.SynesisAnalyticsConfigView.SynesisAnalyticsModel model;
		IUnityContainer container;
		public PropertyAntishakerStrings Strings { get { return PropertyAntishakerStrings.instance; } }
		IVideoInfo videoInfo;

		public void SetPlayer(Border player) {
			//this.player = player;
			playerHolder.Child = player;
		}
		public void Init(IUnityContainer container, odm.ui.views.CustomAnalytics.SynesisAnalyticsConfigView.SynesisAnalyticsModel model, IVideoInfo videoInfo){//, string profToken) {
			this.model = model;
			this.videoInfo = videoInfo;
			this.container = container;

			BindData();
			InitRectangle();
		}
		void BindData() {
			useAntishaker.CreateBinding(CheckBox.IsCheckedProperty, model, x => {
				IsModuleEnabled = x.UseAntishaker;
				return x.UseAntishaker;
			}, (m, v) => {
				m.UseAntishaker = v;
				IsModuleEnabled = m.UseAntishaker;
			});
            rectEditor.RectangleChanged += rectEditor_RectangleChanged;

			//enableDisplacement.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.CameraRedirected, (m, v) => {
			//    m.CameraRedirected = v;
			//});
			enableOutputPictureShift.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.ShiftOutputPicture, (m, v) => {
				m.ShiftOutputPicture = v;
			});
		}

        void rectEditor_RectangleChanged(object sender, EventArgs e)
        {
            model.UseAntishaker = true;
        }

		Rect FromSynesisAntishaker(synesis.AntishakerCrop rct) {
			Rect srct = new Rect();
			srct.X = rct.XOffs;
			srct.Y = rct.YOffs;
			srct.Width = rct.CropWidth;
			srct.Height = rct.CropHeight;
			if (srct.Width == 0)
				srct.Width = 10;
			if (srct.Height == 0)
				srct.Height = 10;
			if (srct.X == 0)
				srct.X = 1;
			if (srct.Y == 0)
				srct.Y = 1;
			return srct;
		}
		synesis.AntishakerCrop ToSynesisAntishaker(Rect rect) {
			synesis.AntishakerCrop crop = new synesis.AntishakerCrop();
			crop.XOffs = (int)rect.X > 0 ? (int)rect.X : 0;
			crop.YOffs = (int)rect.Y > 0 ? (int)rect.Y : 0;
			crop.CropWidth = (int)rect.Width;
			crop.CropHeight = (int)rect.Height;
			return crop;
		}
		public void Apply() {
			GetData();
		}
		public void GetData() {
			var rect = new Rect(rectEditor.Top.X, rectEditor.Top.Y, rectEditor.Bottom.X - rectEditor.Top.X, rectEditor.Bottom.Y - rectEditor.Top.Y);
			model.AntishakerCrop = ToSynesisAntishaker(rect);
		}
		void InitRectangle() {
			Rect r = FromSynesisAntishaker(model.AntishakerCrop);
			rectEditor.Init(r.TopLeft, r.BottomRight, videoInfo.Resolution);
		}
		
		public void Dispose() {
            rectEditor.RectangleChanged -= rectEditor_RectangleChanged;
			//disposables.Dispose();
		}

		public bool IsModuleEnabled {
			get { return (bool)GetValue(IsModuleEnabledProperty); }
			set { SetValue(IsModuleEnabledProperty, value); }
		}
		public static readonly DependencyProperty IsModuleEnabledProperty =
			DependencyProperty.Register("IsModuleEnabled", typeof(bool), typeof(AntishakerView));
	}
}
