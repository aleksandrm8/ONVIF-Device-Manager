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

namespace odm.ui.views
{
    /// <summary>
    /// Interaction logic for VAAlarmView.xaml
    /// </summary>
    public partial class VAAlarmView : UserControl
    {
        public VAAlarmView()
        {
            InitializeComponent();



            this.DataContextChanged += (s, e) => 
                {
                    var newAlarm = e.NewValue as VAAlarm;
                    if (newAlarm != null)
                    {
                        newAlarm.Updated += model_Updated;
                        model_Updated(newAlarm, EventArgs.Empty);
                    }

                    var oldAlarm = e.OldValue as VAAlarm;
                    if (oldAlarm != null)
                    {
                        oldAlarm.Updated -= model_Updated;
                    }
                };

            
        }

        void model_Updated(object sender, EventArgs e)
        {
            var model = (VAAlarm)sender;
            UpdateName(model);
            UpdateState(model);
        }

        void UpdateState(VAAlarm model)
        {
            this.Visibility = model.State ? Visibility.Visible : Visibility.Collapsed;
        }
        void UpdateName(VAAlarm model)
        {
            desc.Text = model.ToString();
        }
        
        
    }
}
