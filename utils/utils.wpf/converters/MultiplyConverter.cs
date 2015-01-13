using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using utils;

namespace utils {
    [ValueConversion(typeof(double), typeof(double))]
    public class MultiplyConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var f = 1.0d;
            if (parameter != null && !parameter.TryParseInvariant(out f))
                throw new ArgumentException("parameter");

            var val = 0.0d;
            if (value != null) {
                if (value.TryParseInvariant(out val))
                    return val * f;
                else 
                    throw new ArgumentException("value"); 
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
