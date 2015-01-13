using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace utils {
    public static class BringOnTopBehavior {



        public static bool GetActivate(DependencyObject obj) {
            return (bool)obj.GetValue(ActivateProperty);
        }

        public static void SetActivate(DependencyObject obj, bool value) {
            obj.SetValue(ActivateProperty, value);
        }

        // Using a DependencyProperty as the backing store for Activate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivateProperty =
            DependencyProperty.RegisterAttached("Activate", typeof(bool), typeof(BringOnTopBehavior), new PropertyMetadata(false, OnActivateChanged));

        private static void OnActivateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var v = d as UIElement;
            if (v == null) {
                dbg.Break();
                return;
            }

            if (true.Equals(e.NewValue)) {
                v.AddHandler(Mouse.MouseDownEvent, new MouseButtonEventHandler(v_MouseDown), true);
            }
            else {
                v.RemoveHandler(Mouse.MouseDownEvent, new MouseButtonEventHandler(v_MouseDown));
            }
        }

        static void v_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var v = sender as UIElement;
            if (v == null) {
                dbg.Break();
                return;
            }

            var p = (Panel)LogicalTreeHelper.GetParent(v);

            var children = p.Children.OfType<UIElement>();

            var max = children.Max(c => Panel.GetZIndex(c));
            
            Panel.SetZIndex(v, max + 1);
            
            var min = children.Min(c => Panel.GetZIndex(c));

            foreach (var child in children)
                Panel.SetZIndex(child, Panel.GetZIndex(child) - min);
            
        }
    }
}