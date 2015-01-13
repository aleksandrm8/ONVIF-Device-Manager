using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace odm.ui.dialogs {
    public class SetNameViewModel :DependencyObject {
        public SetNameViewModel() {
            Name = "";
            ButtonName = "Set";
            NameCaption = "Set Name";
        }

        public string NameCaption {
            get { return (string)GetValue(NameCaptionProperty); }
            set { SetValue(NameCaptionProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NameCaption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameCaptionProperty =
            DependencyProperty.Register("NameCaption", typeof(string), typeof(SetNameViewModel));

        public string Name {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(SetNameViewModel));

        public string ButtonName {
            get { return (string)GetValue(ButtonNameProperty); }
            set { SetValue(ButtonNameProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ButtonName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonNameProperty =
            DependencyProperty.Register("ButtonName", typeof(string), typeof(SetNameViewModel), new UIPropertyMetadata(""));
    }
}
