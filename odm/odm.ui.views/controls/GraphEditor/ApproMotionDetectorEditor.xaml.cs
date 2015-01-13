using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace odm.ui.controls.GraphEditor {
	/// <summary>
	/// Interaction logic for ApproMotionDetectorEditor.xaml
	/// </summary>
	public partial class ApproMotionDetectorEditor : UserControl {
		public ApproMotionDetectorEditor() {
			InitializeComponent();

            p001.Cursor = Cursors.Hand;
            p010.Cursor = Cursors.Hand;
            p100.Cursor = Cursors.Hand;
            p002.Cursor = Cursors.Hand;
            p020.Cursor = Cursors.Hand;
            p200.Cursor = Cursors.Hand;
            p004.Cursor = Cursors.Hand;
            p040.Cursor = Cursors.Hand;
            p400.Cursor = Cursors.Hand;
            p008.Cursor = Cursors.Hand;
            p080.Cursor = Cursors.Hand;
            p800.Cursor = Cursors.Hand;
		}

		public int MaskedValue { 
			get {
                int value = 0;
                value = p001.IsChecked.Value ? (value | 0x1) : (value & int.MaxValue - 0x1);
                value = p010.IsChecked.Value ? (value | 0x10) : (value & int.MaxValue - 0x10);
                value = p100.IsChecked.Value ? (value | 0x100) : (value & int.MaxValue - 0x100);

                value = p002.IsChecked.Value ? (value | 0x2) : (value & int.MaxValue - 0x2);
                value = p020.IsChecked.Value ? (value | 0x20) : (value & int.MaxValue - 0x20);
                value = p200.IsChecked.Value ? (value | 0x200) : (value & int.MaxValue - 0x200);

                value = p004.IsChecked.Value ? (value | 0x4) : (value & int.MaxValue - 0x4);
                value = p040.IsChecked.Value ? (value | 0x40) : (value & int.MaxValue - 0x40);
                value = p400.IsChecked.Value ? (value | 0x400) : (value & int.MaxValue - 0x400);

                value = p008.IsChecked.Value ? (value | 0x8) : (value & int.MaxValue - 0x8);
                value = p080.IsChecked.Value ? (value | 0x80) : (value & int.MaxValue - 0x80);
                value = p800.IsChecked.Value ? (value | 0x800) : (value & int.MaxValue - 0x800);
                return value;
			}
			set {
				p001.IsChecked = ((value & 0x1) == 0x1);
				p010.IsChecked = ((value & 0x10) == 0x10);
				p100.IsChecked = ((value & 0x100) == 0x100);

				p002.IsChecked = ((value & 0x2) == 0x2);
				p020.IsChecked = ((value & 0x20) == 0x20);
				p200.IsChecked = ((value & 0x200) == 0x200);

				p004.IsChecked = ((value & 0x4) == 0x4);
				p040.IsChecked = ((value & 0x40) == 0x40);
				p400.IsChecked = ((value & 0x400) == 0x400);

				p008.IsChecked = ((value & 0x8) == 0x8);
				p080.IsChecked = ((value & 0x80) == 0x80);
				p800.IsChecked = ((value & 0x800) == 0x800);

			}
		}
	}
}
