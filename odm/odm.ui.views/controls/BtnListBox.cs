using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace odm.ui.controls
{
    public class BtnListBox : ListBox
    {
        public BtnListBox()
        {
            this.PreviewMouseLeftButtonDown += BtnListBox_PreviewMouseLeftButtonDown;
        }

        void BtnListBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.SelectedItem = null;
        }
    }
}
