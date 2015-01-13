using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using utils;

namespace odm.ui.views.CustomAnalytics {
    public class DataConverter {
        public static int StringToInt(string value) {
            int x = 0;
            try {
                x = XmlConvert.ToInt32(value);
            } catch (Exception err) {
                dbg.Error(err);
            }
            return x;
        }
        public static string IntToString(int value) {
            string x = "";
            try {
                x = XmlConvert.ToString(value);
            } catch (Exception err) {
                dbg.Error(err);
            }
            return x;
        }
        public static float StringToFloat(string value) {
            double x = 0;
            try {
                x = XmlConvert.ToDouble(value);
            } catch (Exception err) {
                dbg.Error(err);
            }
            return (float)x;
        }
        public static double StringToDouble(string value) {
            double x = 0;
            try {
                x = XmlConvert.ToDouble(value);
            } catch (Exception err) {
                dbg.Error(err);
            }
            return x;
        }
        public static string FloatToString(float value) {
            string x = "";
            try {
                x = XmlConvert.ToString(value);
            } catch (Exception err) {
                dbg.Error(err);
            }
            return x;
        }
        public static string DoubleToString(double value) {
            string x = "";
            try {
                x = XmlConvert.ToString(value);
            } catch (Exception err) {
                dbg.Error(err);
            }
            return x;
        }
        public static bool StringToBool(string value) {
            bool res = false;
            try {
                res = XmlConvert.ToBoolean(value);
            } catch (Exception err) {
                dbg.Error(err);
            }

            return res;
        }
        public static string BoolToString(bool value) {
            string x = "false";
            try {
                x = XmlConvert.ToString(value);
            } catch (Exception err) {
                dbg.Error(err);
            }
            return x;
        }

        public static Point SynesisToPoint(synesis.Point pt) {
            return new Point(pt.X, pt.Y);
        }
        public static synesis.Point PointToSynesis(Point pt) {
            return new synesis.Point() { X = (int)pt.X, Y = (int)pt.Y};
        }

        public static void ScalePointOutput(synesis.Point point, Size videoSourceSize, Size videoEncoderSize) {
            double valueX = point.X;
            double valueY = point.Y;
            //convert ftrom video sourve resolution to encoder resolution
            double scalex = videoSourceSize.Width / (videoEncoderSize.Width == 0 ? 1 : videoEncoderSize.Width);
            double scaley = videoSourceSize.Height / (videoEncoderSize.Height == 0 ? 1 : videoEncoderSize.Height);
            valueX = valueX * scalex;
            valueY = valueY * scaley;

            //scale from visible to [-1;1]
            double heightValue = videoSourceSize.Height - 1;
            heightValue = heightValue == 0 ? 1 : heightValue;
            double widthValue = videoSourceSize.Width - 1;
            widthValue = widthValue == 0 ? 1 : widthValue;

            point.X = (float)(((valueX * 2) / widthValue) - 1);
            point.Y = (float)((((heightValue - valueY) * 2) / heightValue) - 1);
        }
        public static void ScalePointInput(synesis.Point point, Size videoSourceSize, Size videoEncoderSize) {
            //scale from [-1;1] to visible dimensions
            double valueX = (videoSourceSize.Width / 2) * (point.X + 1);
            double valueY = videoSourceSize.Height - (videoSourceSize.Height / 2) * (point.Y + 1);

            //convert ftrom video sourve resolution to encoder resolution
            double scalex = videoEncoderSize.Width / (videoSourceSize.Width == 0 ? 1 : videoSourceSize.Width);
            double scaley = videoEncoderSize.Height / (videoSourceSize.Height == 0 ? 1 : videoSourceSize.Height);
            valueX = valueX * scalex;
            valueY = valueY * scaley;
            point.X = (float)valueX;
            point.Y = (float)valueY;
        }
    }
}
