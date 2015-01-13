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
using odm.ui.controls;

namespace odm.ui.views.CommonAnalytics {
    /// <summary>
    /// Interaction logic for CommonAnalytics.xaml
    /// </summary>
    public partial class CommonAnalytics : UserControl {
        public CommonAnalytics() {
            InitializeComponent();
        }
        public void Init(odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor moduleDescr) {
            BindModel(moduleDescr);
        }
        
        XmlParserFactory xparser;

        void BindModel(odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor model) {
            xparser = new XmlParserFactory(model.config, model.configDescription ,model.schema);
            FillItems(xparser);
        }

        void FillItems(XmlParserFactory parser) {
            itemsPanel.Children.Clear();
            //fill simple items
            itemsPanel.Children.Add(parser.GetSimpleItemsControl());
            //fill element items
            //itemsPanel.Children.Add(parser.GetElementItemsControl());
            itemsPanel.Children.Add(parser.GetElementItemsControl());
        }

    }
}
