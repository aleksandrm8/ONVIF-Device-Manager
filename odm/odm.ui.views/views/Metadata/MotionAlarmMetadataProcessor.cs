using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public class MotionAlarmSnapshot : VAEntitySnapshot
    {
        public bool State { get; set; }
        
        public override VAEntity Create()
        {
            return new MotionAlarm();
        }
    }

    public class MotionAlarm : VAAlarm
    {
        public MotionAlarm()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Fit(VAEntitySnapshot snapshot)
        {
            return snapshot is MotionAlarmSnapshot;
        }

        public override void Update(VAEntitySnapshot snapshot1, Func<double,double> scaleX, Func<double,double> scaleY)
        {
            if (!Fit(snapshot1))
                throw new InvalidOperationException();
 	        
            MotionAlarmSnapshot snapshot = (MotionAlarmSnapshot)snapshot1;

            this.State = snapshot.State;
            FireUpdated();
        }
    }

    public class MotionAlarmMetadataProcessor : BaseNotificationMessageProcessor<MotionAlarmSnapshot>
    {
        public MotionAlarmMetadataProcessor(string videoSourceToken, string videoAnalyticsConfToken, Action<MotionAlarmSnapshot> initialized, Action<MotionAlarmSnapshot> changed, Action<MotionAlarmSnapshot> deleted)
            : base(videoSourceToken, null, videoAnalyticsConfToken, initialized, changed, deleted)
        {
        }

        protected override bool VerifyTopic(TopicExpressionType topic)
        {
            if (@"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete".Equals(topic.Dialect))
            {
                var innerTopic = topic.Any;
                //TODO : resolve namespace prefixes!
                if (innerTopic != null && innerTopic.Length == 1 && "tns1:VideoAnalytics/tnsx:MotionAlarm".Equals(innerTopic[0].InnerText))
                    return true;
            }
            return false;
        }

        protected override MotionAlarmSnapshot Parse(TopicExpressionType topic, Message message)
        {
            bool state = false;
            
            var data = message.data;
            if (data.simpleItem != null)
            {
                foreach (var si in data.simpleItem)
                {
                    if (si.name == "State")
                    {
                        si.value.TryParseInvariant(out state);
                    }
                }
            }
            
            var snapshot = new MotionAlarmSnapshot
                {
                    State = state
                };
            return snapshot;
        }
    }
}
