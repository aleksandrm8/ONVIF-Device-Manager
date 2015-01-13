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
    public enum TamperingDetectorAlarms
    {
        SignalLoss,
        ImageTooNoisy, 
        ImageTooDark,
        ImageTooBright,
        ImageTooBlurry,
        CameraRedirected,
        CameraObstructed
    }

    public class TamperingDetectorAlarmSnapshot : VAEntitySnapshot
    {
        public TamperingDetectorAlarms Type { get; set; }
        public bool State { get; set; }
    
        public override VAEntity Create()
        {
            return new TamperingDetectorAlarm(this.Type);
        }
    }

    public class TamperingDetectorAlarm : VAAlarm
    {
        public TamperingDetectorAlarms Type { get; private set; }

        public TamperingDetectorAlarm(TamperingDetectorAlarms type)
        {
            this.Type = type;
        }

        public override string ToString()
        {
            return string.Format("{0}", this.Type);
        }

        public override bool Fit(VAEntitySnapshot snapshot)
        {
            return snapshot is TamperingDetectorAlarmSnapshot
                && ((TamperingDetectorAlarmSnapshot)snapshot).Type == this.Type;
        }

        public override void Update(VAEntitySnapshot snapshot1, Func<double,double> scaleX, Func<double,double> scaleY)
        {
            if (!Fit(snapshot1))
                throw new InvalidOperationException();

            TamperingDetectorAlarmSnapshot snapshot = (TamperingDetectorAlarmSnapshot)snapshot1;

            this.State = snapshot.State;
            FireUpdated();
        }
    }

    public class TamperingDetectorAlarmMetadataProcessor : BaseNotificationMessageProcessor<TamperingDetectorAlarmSnapshot>
    {
        public TamperingDetectorAlarmMetadataProcessor(string videoSourceToken, string videoAnalyticsConfToken, Action<TamperingDetectorAlarmSnapshot> initialized, Action<TamperingDetectorAlarmSnapshot> changed, Action<TamperingDetectorAlarmSnapshot> deleted)
            : base(videoSourceToken, null, videoAnalyticsConfToken, initialized, changed, deleted)
        {
        }

        readonly static Dictionary<string, TamperingDetectorAlarms> topics = new Dictionary<string, TamperingDetectorAlarms>();
        static TamperingDetectorAlarmMetadataProcessor()
        {
            topics.Add("tns1:VideoSource/tnsx:SignalLoss", TamperingDetectorAlarms.SignalLoss);
            topics.Add("tns1:VideoSource/tnsx:SignalTooNoisy", TamperingDetectorAlarms.ImageTooNoisy);
            topics.Add("tns1:VideoSource/tnsx:ImageTooDark", TamperingDetectorAlarms.ImageTooDark);
            topics.Add("tns1:VideoSource/tnsx:ImageTooBright", TamperingDetectorAlarms.ImageTooBright);
            topics.Add("tns1:VideoSource/tnsx:ImageTooBlurry", TamperingDetectorAlarms.ImageTooBlurry);
            topics.Add("tns1:VideoSource/tnsx:CameraRedirected", TamperingDetectorAlarms.CameraRedirected);
            topics.Add("tns1:VideoSource/tnsx:CameraObstructed", TamperingDetectorAlarms.CameraObstructed);
        }

        protected override bool VerifyTopic(TopicExpressionType topic)
        {
            if (@"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete".Equals(topic.Dialect))
            {
                var innerTopic = topic.Any;
                //TODO : resolve namespace prefixes!
                if (innerTopic != null && innerTopic.Length == 1)
                {
                    var topicText = innerTopic[0].InnerText;
                    if (topics.Any( (p)=> p.Key == topicText))
                        return true;
                }
            }
            return false;
        }

        protected override TamperingDetectorAlarmSnapshot Parse(TopicExpressionType topic, Message message)
        {
            if (message.source == null || message.source.simpleItem == null)
                throw new InvalidOperationException();

            string topicText = topic.Any[0].InnerText;
            TamperingDetectorAlarms type = topics[topicText];

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
            
            var snapshot = new TamperingDetectorAlarmSnapshot
                {
                    Type = type,
                    State = state
                };
            return snapshot;
        }
    }
}
