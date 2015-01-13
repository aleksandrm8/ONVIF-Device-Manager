using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using odm.ui.views.CustomAnalytics;
using onvif.services;
using utils;

namespace odm.ui.core {

	public interface IConfiguratorFactory {
		IConfigurator Create(XmlQualifiedName type);
	}

	public class ConfiguratorFactory : IConfiguratorFactory {
		public IConfigurator Create(XmlQualifiedName type) {
			if (new XmlQualifiedName("TripWireRule", "http://www.synesis.ru/onvif/VideoAnalytics").Equals(type))
				return new TripWireRuleConfigurator();
			else if (new XmlQualifiedName("RegionRule", "http://www.synesis.ru/onvif/VideoAnalytics").Equals(type))
				return new RegionRuleConfigurator();
			else if (new XmlQualifiedName("AnalyticsModule", "http://www.synesis.ru/onvif/VideoAnalytics").Equals(type))
				return new AnalyticsModuleConfigurator();

			return new Configurator();
		}
	}

	public interface IConfigurator {
		Config Configure(Config defaultConfig, Config[] existingConfigs);
	}

	public class Configurator : IConfigurator {
		public Config Configure(Config defaultConfig, Config[] existingConfigs) {
			existingConfigs = existingConfigs ?? new Config[0];

			if (existingConfigs.Any((cfg) => cfg.name == defaultConfig.name))
				throw new InvalidOperationException(string.Format(ExceptionStrings.instance.sErrorNameNotUnique, defaultConfig.name));

			return OnConfigure(defaultConfig, existingConfigs);
		}

		protected virtual Config OnConfigure(Config defaultConfig, Config[] existingConfigs) {
			return defaultConfig;
		}
	}

	class RegionRuleConfigurator : Configurator {
		protected override Config OnConfigure(Config defaultConfig, Config[] existingConfigs) {
			foreach (var item in defaultConfig.parameters.elementItem) {
				if (item.name == "Points") {
					const float b = 0.25f;
					item.any = new synesis.RegionPoints() {
						Points = new[]{
                           new synesis.Point(){ X=-b,Y=-b},
                           new synesis.Point(){ X=-b,Y=b},
                           new synesis.Point(){ X=b,Y=b},
                           new synesis.Point(){ X=b,Y=-b},
                        }
					}.Serialize();
				} else if (item.name == "RegionMotionAlarm") {
					item.any = new synesis.RegionMotionAlarm() { Enabled = true }.Serialize();
				} else if (item.name == "DirectionAlarm") {
					item.any = new synesis.DirectionAlarm() {
						Enabled = false,
						MinShift = 0.05f,
						Rose = new synesis.Rose() { Left = true, Right = true }
					}.Serialize();
				} else if (item.name == "LoiteringAlarm") {
					item.any = new synesis.LoiteringAlarm() { Radius = 999f, Time = 10 }.Serialize();
				} else if (item.name == "SpeedAlarm") {
					item.any = new synesis.SpeedAlarm() { Speed = 1.388888888889f, Time = 2 }.Serialize();
				}
			}
			return defaultConfig;
		}
	}

	class TripWireRuleConfigurator : Configurator {
		protected override Config OnConfigure(Config defaultConfig, Config[] existingConfigs) {
			var xmlPoints = defaultConfig.parameters.elementItem.FirstOrDefault((item) => item.name == "Points");
			if (xmlPoints != null) {
				const float b = 0.25f;
				xmlPoints.any = new synesis.TripWirePoints() {
					P1 = new synesis.Point() { X = -b, Y = -b },
					P2 = new synesis.Point() { X = b, Y = b }
				}.Serialize();
			}

			var xmlDirection = defaultConfig.parameters.simpleItem.FirstOrDefault((item) => item.name == "Direction");
			if (xmlDirection != null) {
				xmlDirection.value = "FromBoth";
			}

			return defaultConfig;
		}
	}

	class AnalyticsModuleConfigurator : Configurator {
		protected override Config OnConfigure(Config defaultConfig, Config[] existingConfigs) {
			foreach (var item in defaultConfig.parameters.elementItem) {
				if (item.name == "AntishakerCrop") {
					item.any = new synesis.AntishakerCrop() {
						XOffs = -1, YOffs = 1, CropWidth = 2, CropHeight = 2
					}.Serialize();
				} else if (item.name == "UserRegion") {
					var rose = new synesis.Rose() { Left = true, UpLeft = true, Up = true, UpRight = true, Right = true, DownRight = true, Down = true, DownLeft = true };
					var points = new [] {
                     new synesis.Point() { X=-1,Y=-1 },
                     new synesis.Point() { X=-1,Y=1},
                     new synesis.Point() { X=1,Y=1 },
                     new synesis.Point() { X=1,Y=-1 }
               };
					item.any = new synesis.UserRegion() {
						Rose = rose, Points = points
					}.Serialize();
				} else if (item.name == "MarkerCalibration") {
					var heightMarker = new synesis.HeightMarker() {
						Height = 170,
						SurfaceNormals = new synesis.SurfaceNormal[] 
                        { 
                            new synesis.SurfaceNormal() { Height=0.4f, Point = new synesis.Point() {X=-0.5f,Y=-0.25f} } ,
                            new synesis.SurfaceNormal() { Height = 0.25f, Point = new synesis.Point() { X = 0.5f, Y = 0.5f } }
                        }
					};
					var heightMarkers = new synesis.HeightMarker[] { heightMarker };
					var heightMarkerCalibration = new synesis.HeightMarkerCalibration() { FocalLength = 9, MatrixFormat = synesis.MatrixFormat.Item13, HeightMarkers = heightMarkers };
					item.any = new synesis.MarkerCalibration() { Item = heightMarkerCalibration }.Serialize();
				}
			}

			foreach (var item in defaultConfig.parameters.simpleItem) {
				switch (item.name) {
					case "UseObjectTracker":
					item.value = DataConverter.BoolToString(true); break;
					case "StabilizationTime":
					item.value = DataConverter.IntToString(1000); break;
					case "MinObjectArea":
					item.value = DataConverter.FloatToString(0.1f); break;
					case "MaxObjectArea":
					item.value = DataConverter.FloatToString(10.0f); break;
					case "MaxObjectSpeed":
					item.value = DataConverter.FloatToString(20.0f); break;
					case "DisplacementSensitivity":
					item.value = DataConverter.IntToString(3); break;
					case "UseAntishaker":
					item.value = DataConverter.BoolToString(true); break;
					case "ContrastSensitivity":
					item.value = DataConverter.IntToString(7); break;
					case "ImageTooDark":
					item.value = DataConverter.BoolToString(true); break;
					case "ImageTooBlurry":
					item.value = DataConverter.BoolToString(true); break;
					case "ImageTooBright":
					item.value = DataConverter.BoolToString(true); break;
					case "CameraRedirected":
					item.value = DataConverter.BoolToString(true); break;
					case "CameraObstructed":
					item.value = DataConverter.BoolToString(true); break;
				}
			}





			return defaultConfig;
		}
	}

}
