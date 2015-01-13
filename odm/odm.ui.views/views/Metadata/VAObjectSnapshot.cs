using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using onvif.services;

namespace odm.ui.views
{
    [Serializable]
    public class VAObjectSnapshot : VAEntitySnapshot
    {
        public string Id {get; set;}
        public Rect BoundingBox { get; set; }
        public Point CurrentPosition { get; set; }
        public Point StartPosition { get; set; }

        public override VAEntity Create()
        {
            return new VAObject(this.Id);
        }
    }

}
