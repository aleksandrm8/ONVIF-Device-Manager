using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using utils;

namespace odm.ui.views
{
    public class VAObject : VAEntity
    {
        public string Id { get; private set; }

        public Rect BoundingBox { get; set; }
        public Point StartPosition { get; set; }

        readonly PointCollection trajectory = new PointCollection();
        public PointCollection Trajectory { get { return trajectory; } }

        public VAObject(string id)
        {
            this.Id = id;
        }

        public override bool Fit(VAEntitySnapshot snapshot)
        {
            return snapshot is VAObjectSnapshot 
                && ((VAObjectSnapshot)snapshot).Id == this.Id;
        }

        public override void Update(VAEntitySnapshot snapshot1, Func<double, double> scaleX, Func<double, double> scaleY)
        {
            if(!Fit(snapshot1))
                throw new InvalidOperationException();

            var snapshot = (VAObjectSnapshot)snapshot1;

            this.BoundingBox = Scale(snapshot.BoundingBox, scaleX, scaleY);

            if (this.Trajectory.Count == 0)
            {
                var sp = snapshot.StartPosition != default(Point) ? snapshot.StartPosition : snapshot.CurrentPosition;
                this.StartPosition = Scale(sp, scaleX, scaleY);
                this.Trajectory.Add(this.StartPosition);
            }
            else
            {
                const int sizeMax = 10000;
                this.Trajectory.ShiftPush(Scale(snapshot.CurrentPosition, scaleX, scaleY), sizeMax);
            }

            FireUpdated();
        }


        static System.Windows.Point Scale(System.Windows.Point p, Func<double, double> scaleX, Func<double, double> scaleY)
        {
            return new System.Windows.Point(scaleX(p.X), scaleY(p.Y));
        }

        static System.Windows.Rect Scale(System.Windows.Rect r, Func<double, double> scaleX, Func<double, double> scaleY)
        {
            var tl = Scale(r.TopLeft, scaleX, scaleY);
            var br = Scale(r.BottomRight, scaleX, scaleY);
            if (tl.Y > br.Y)
            {
                var t = tl.Y;
                tl.Y = br.Y;
                br.Y = t;
            }
            if (tl.X > br.X)
            {
                var t = tl.X;
                tl.X = br.X;
                br.X = t;
            }
            return new System.Windows.Rect(tl, br);
        }

    }
}
