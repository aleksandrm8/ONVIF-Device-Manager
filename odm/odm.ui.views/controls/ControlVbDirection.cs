using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace odm.ui.controls {
    public class ControlVbDirection: Button {
        public enum Orientations {
            Top,
            Left,
            Right,
            Bottom
        };

        public Orientations Orientation {
            get { return (Orientations)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientations), typeof(ControlVbDirection), new UIPropertyMetadata(Orientations.Top));

        
    }
}
