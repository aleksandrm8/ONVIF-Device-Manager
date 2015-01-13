using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Text.RegularExpressions;

namespace odm.ui.controls {
    public class ControlNumericTextBox:TextBox {
        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e) {
            
            base.OnPreviewTextInput(e);
        }
        protected override void OnTextInput(System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[0-9]"); 
            if (!regex.Match(e.Text).Success)
                return;
            int result;
            if (this.Text != "") {
                bool expr = Int32.TryParse(this.Text, out result) && result <= MaxValue && result >= MinValue;
                if (expr)
                    base.OnTextInput(e);
            } else {
                base.OnTextInput(e);
            }
        }
        public int MaxValue {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(ControlNumericTextBox), new UIPropertyMetadata(1000));

        public int MinValue {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(ControlNumericTextBox), new UIPropertyMetadata(0));
        
        //public string Text {
        //    get { return (string)GetValue(TextProperty); }
        //    set { SetValue(TextProperty, value); }
        //}
        //// Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty TextProperty =
        //    DependencyProperty.Register("Text", typeof(string), typeof(ControlNumericTextBox), new UIPropertyMetadata(""));
    }
}
