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
using System.ComponentModel;
using System.Collections.ObjectModel;
using utils;

namespace odm.ui.views {
    /// <summary>
    /// Interaction logic for BatchTasksView.xaml
    /// </summary>
    public partial class BatchTasksView : UserControl, INotifyPropertyChanged {
        public BatchTasksView() {
            InitializeComponent();
            items = new ObservableCollection<BatchItem>();

            BindData();
        }
        public class BatchItem:INotifyPropertyChanged {
            bool isEnabled = true;
            public bool IsEnabled {
                get {
                    return isEnabled;
                }
                set {
                    isEnabled = value;
                    NotifyPropertyChanged("IsEnabled");
                }
            }
            bool isChecked;
            public bool IsChecked {
                get {
                    return isChecked;
                }
                set {
                    isChecked = value;
                    NotifyPropertyChanged("IsChecked");
                }
            }

            public string Name { get; set; }
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public string Firmware { get; set; }
            public string Hardware { get; set; }

            private void NotifyPropertyChanged(String info) {
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        public ObservableCollection<BatchItem> items { get; set; }
        
        void BindData() {
            fillFakeData();
            devicesList.CreateBinding(ListView.ItemsSourceProperty, this, x => x.items);
        }
        void fillFakeData() {
            BatchItem bitem = new BatchItem() { IsChecked = false, Firmware = "0,6,3445", Manufacturer = "Synesis", Model = "DK-455", Name = "Name#1" };
            items.Add(bitem);
            bitem = new BatchItem() { IsChecked = false, Firmware = "0,6,3445", Manufacturer = "Synesis", Model = "DK-455", Name = "Name#1" };
            items.Add(bitem);
            bitem = new BatchItem() { IsEnabled = false, IsChecked = false, Firmware = "5.20", Manufacturer = "Axis", Model = "P3301", Name = "Axis camera" };
            items.Add(bitem);
            bitem = new BatchItem() { IsChecked = false, Firmware = "0,6,3415", Manufacturer = "Synesis", Model = "DK-255", Name = "Name#2" };
            items.Add(bitem);
            bitem = new BatchItem() { IsChecked = false, Firmware = "0,6,3446", Manufacturer = "Synesis", Model = "DK-255", Name = "Name#3" };
            items.Add(bitem);
            bitem = new BatchItem() { IsEnabled = false, IsChecked = false, Firmware = "0,6,3446", Manufacturer = "Synesis", Model = "DK-455", Name = "Name#4" };
            items.Add(bitem);
            bitem = new BatchItem() { IsChecked = false, Firmware = "0,6,2445", Manufacturer = "Synesis", Model = "DK-455", Name = "Name#5" };
            items.Add(bitem);
            bitem = new BatchItem() { IsChecked = false, Firmware = "0,6,3745", Manufacturer = "Synesis", Model = "DK-455", Name = "Name#6" };
            items.Add(bitem);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
