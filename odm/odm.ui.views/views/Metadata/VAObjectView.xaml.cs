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
    /// Interaction logic for VAObjectView.xaml
    /// </summary>
    public partial class VAObjectView : Canvas
    {
        public VAObjectView()
        {
            InitializeComponent();



            this.DataContextChanged += (s, e) => 
                {
                    var newObj = e.NewValue as VAObject;
                    if (newObj != null)
                    {
                        newObj.Updated += model_Updated;
                        model_Updated(newObj, EventArgs.Empty);
                    }

                    var oldObj = e.OldValue as VAObject;
                    if (oldObj != null)
                    {
                        oldObj.Updated -= model_Updated;
                    }
                };

            
        }

        void model_Updated(object sender, EventArgs e)
        {
            var model = (VAObject)sender;
            UpdateId(model);
            UpdateBoundingBox(model);
            UpdateTrajectory(model);
        }

        void UpdateId(VAObject model)
        {
            id.Text = model.Id.ToString();
            Canvas.SetLeft(id, model.BoundingBox.Left);
            Canvas.SetTop(id, model.BoundingBox.Top - 20);
        }
        void UpdateBoundingBox(VAObject model)
        {
            boundingBox.Width = model.BoundingBox.Width;
            boundingBox.Height = model.BoundingBox.Height;
            Canvas.SetLeft(boundingBox, model.BoundingBox.Left);
            Canvas.SetTop(boundingBox, model.BoundingBox.Top);
        }
        void UpdateTrajectory(VAObject model)
        {
            trajectory.Points = model.Trajectory;
        }
        
    }
}
