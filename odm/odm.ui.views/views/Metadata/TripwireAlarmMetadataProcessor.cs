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

    public class TripwireAlarmSnapshot : VAEntitySnapshot
    {
        public bool HasCrossed { get; set; }
        public string Rule { get; set; }
        public string ObjectId { get; set; }

        public override VAEntity Create()
        {
            return new TripwireAlarm(this.Rule, this.ObjectId);
        }
    }

    public class TripwireAlarm : VAAlarm
    {
        public string Rule {get; private set;}
        public string ObjectId { get; private set; }
        public bool HasCrossed { get; set; }

        public override bool State 
        { 
            get { return this.HasCrossed; }
            set { this.HasCrossed = value; }
        }

        public TripwireAlarm(string rule, string objectId)
        {
            this.Rule = rule;
            this.ObjectId = objectId;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}, {2}", Name, Rule, ObjectId);
        }
        

        public override bool Fit(VAEntitySnapshot snapshot)
        {
            return snapshot is TripwireAlarmSnapshot 
                && ((TripwireAlarmSnapshot)snapshot).Rule == this.Rule
                && ((TripwireAlarmSnapshot)snapshot).ObjectId == this.ObjectId;
        }

        public override void Update(VAEntitySnapshot snapshot1, Func<double,double> scaleX, Func<double,double> scaleY)
        {
            if (!Fit(snapshot1))
                throw new InvalidOperationException();
 	        
            TripwireAlarmSnapshot snapshot = (TripwireAlarmSnapshot)snapshot1;

            this.HasCrossed = snapshot.HasCrossed;
            FireUpdated();
        }
    }

    public class TripwireAlarmMetadataProcessor : BaseNotificationMessageProcessor<TripwireAlarmSnapshot>
    {
        public TripwireAlarmMetadataProcessor(string videoSourceConfToken, string videoAnalyticsConfToken, Action<TripwireAlarmSnapshot> initialized, Action<TripwireAlarmSnapshot> changed, Action<TripwireAlarmSnapshot> deleted)
            : base(null, videoSourceConfToken, videoAnalyticsConfToken, initialized, changed, deleted)
        {
        }

        protected override bool VerifyTopic(TopicExpressionType topic)
        {
            if (@"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete".Equals(topic.Dialect))
            {
                var innerTopic = topic.Any;
                //TODO : resolve namespace prefixes!
                if (innerTopic != null && innerTopic.Length == 1 && "tns1:RuleEngine/LineDetector/tnsx:Object".Equals(innerTopic[0].InnerText))
                    return true;
            }
            return false;
        }

        protected override TripwireAlarmSnapshot Parse(TopicExpressionType topic, Message message)
        {
            if (message.source == null || message.source.simpleItem == null)
                throw new InvalidOperationException();
            string rule = null;
            foreach (var s in message.source.simpleItem)
                if (s.name == "Rule") rule = s.value;
            if (string.IsNullOrEmpty(rule))
                throw new InvalidOperationException();

            if (message.key == null || message.key.simpleItem == null || message.key.simpleItem.Length != 1)
                throw new InvalidOperationException();
            var key = message.key.simpleItem[0];

            
            if (key == null || key.name != "ObjectId")
                throw new InvalidOperationException();
            string objectId = key.value;

            bool hasCrossed = false;
            
            var data = message.data;
            if (data.simpleItem != null)
            {
                foreach (var si in data.simpleItem)
                {
                    if (si.name == "State")
                    {
                        si.value.TryParseInvariant(out hasCrossed);
                    }
                    else if (si.name == "HasCrossed")
                    {
                        si.value.TryParseInvariant(out hasCrossed);
                    }
                }
            }
            
            var snapshot = new TripwireAlarmSnapshot
                {
                    HasCrossed = hasCrossed,
                    Rule = rule,
                    ObjectId = objectId
                };
            return snapshot;
        }
    }
}
