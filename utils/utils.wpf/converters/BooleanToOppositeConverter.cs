using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace utils
{
    
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanToOppositeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;
            return DependencyProperty.UnsetValue;
        }
    }
}
