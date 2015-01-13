using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Documents;
using System.Collections;
using utils;

namespace utils
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool revert = false;
            if (parameter != null && !parameter.TryParseInvariant(out revert))
                throw new ArgumentException("parameter");

            return revert ? value != null : value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
