using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using odm.player;
using onvif.services;
using utils;

namespace odm.ui.views
{
    public class ObjectMotionMetadataProcessor : BaseNotificationMessageProcessor<VAObjectSnapshot>
    {
        public ObjectMotionMetadataProcessor(string videoSourceToken, string videoAnalyticsConfToken, Action<VAObjectSnapshot> initialized, Action<VAObjectSnapshot> changed, Action<VAObjectSnapshot> deleted)
            : base(videoSourceToken, null, videoAnalyticsConfToken, initialized, changed, deleted)
        {
        }

        protected override bool VerifyTopic(TopicExpressionType topic)
        {
            if (@"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete".Equals(topic.Dialect))
            {
                var innerTopic = topic.Any;
                //TODO : resolve namespace prefixes!
                if (innerTopic != null && innerTopic.Length == 1 && "tns1:VideoAnalytics/tnsx:ObjectMotion".Equals(innerTopic[0].InnerText))
                    return true;
            }
            return false;
        }

        protected override VAObjectSnapshot Parse(TopicExpressionType topic, Message message)
        {
            if (message.key == null || message.key.simpleItem == null || message.key.simpleItem.Length != 1)
                throw new InvalidOperationException();
            var key = message.key.simpleItem[0];

            if (key == null || key.name != "ObjectId")
                throw new InvalidOperationException();
            string objectId = key.value;

            System.Windows.Rect rect = System.Windows.Rect.Empty;
            System.Windows.Point currPos = new System.Windows.Point();
            System.Windows.Point startPos = new System.Windows.Point();

            var data = message.data;
            if (data.simpleItem != null)
            {
                foreach (var si in data.simpleItem)
                {
                    if (si.name == "BoundingBox")
                    {
                        si.value.TryParseInvariant(out rect);
                    }
                    else if (si.name == "CurrentPosition")
                    {
                        si.value.TryParseInvariant(out currPos);
                    }
                    else if (si.name == "StartPosition")
                    {
                        si.value.TryParseInvariant(out startPos);
                    }
                }
            }
            if (data.elementItem != null)
            {
                foreach (var ei in data.elementItem)
                {
                    //TODO remove
                    if (ei.name == "BoundingBox")
                    {
                        var rectangle = ei.any.Deserialize<Rectangle>();
                        rect = new Rect(new Point(rectangle.left, rectangle.bottom), new Point(rectangle.right, rectangle.top));
                    }
                }
            }


            var snapshot = new VAObjectSnapshot
                {
                    Id = objectId,
                    BoundingBox = rect,
                    CurrentPosition = currPos,
                    StartPosition = startPos,
                };
            return snapshot;
        }
    }
}
