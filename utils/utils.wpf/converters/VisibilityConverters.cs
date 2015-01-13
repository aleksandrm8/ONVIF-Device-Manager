using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using utils;

namespace utils
{
    [ValueConversion(typeof(bool), typeof(Visibility), ParameterType  = typeof(bool))]
    public class HiddenVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            bool revert = false;
            if (parameter != null && !parameter.TryParseInvariant(out revert))
                throw new ArgumentException("parameter");

            
            if (value is bool)
            {
                bool vis = (bool)value;
                vis ^= revert;
                return vis ? Visibility.Visible : Visibility.Hidden;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility), ParameterType = typeof(bool))]
    public class CollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            bool revert = false;
            if (parameter != null && !parameter.TryParseInvariant(out revert))
                throw new ArgumentException("parameter");
            
            
            if (value is bool)
            {
                bool vis = (bool)value;
                vis ^= revert;
                return vis ? Visibility.Visible : Visibility.Collapsed;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
