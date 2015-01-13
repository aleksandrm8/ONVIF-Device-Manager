using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace utils {
    public static class DependencyObjectEx {


        #region Name


        public static string GetName(DependencyObject obj) {
            return (string)obj.GetValue(NameProperty);
        }

        public static void SetName(DependencyObject obj, string value) {
            obj.SetValue(NameProperty, value);
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached("Name", typeof(string), typeof(DependencyObjectEx), new PropertyMetadata(null));


        #endregion Name

    }
}
