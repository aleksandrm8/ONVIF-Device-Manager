using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using onvif.services;
using utils;

namespace odm.ui.views
{
    public abstract class BaseNotificationMessageProcessor<TSnapshot> : INotificationMessageProcessor where TSnapshot: VAEntitySnapshot
    {
        readonly string videoSourceToken;
        readonly string videoSourceConfToken;
        readonly string videoAnalyticsConfToken;

        readonly Action<TSnapshot> changed;
        readonly Action<TSnapshot> initialized;
        readonly Action<TSnapshot> deleted;

        public BaseNotificationMessageProcessor(string videoSourceToken, string videoSourceConfToken, string videoAnalyticsConfToken, Action<TSnapshot> initialized, Action<TSnapshot> changed, Action<TSnapshot> deleted)
        {
            this.videoSourceToken = videoSourceToken;
            this.videoSourceConfToken = videoSourceConfToken;
            this.videoAnalyticsConfToken = videoAnalyticsConfToken;


            this.initialized = initialized;
            this.changed = changed;
            this.deleted = deleted;
        }

        protected abstract bool VerifyTopic(TopicExpressionType topic);

        public void Process(NotificationMessageHolderType nm)
        {
            var topic = nm.Topic;
            if (topic != null)
            {
                if (VerifyTopic(topic))
                    Parse(topic, nm.Message);
            }
        }

        protected abstract TSnapshot Parse(TopicExpressionType topic, Message message);

        private void Parse(TopicExpressionType topic, XmlElement msg)
        {
            var message = msg.Deserialize<Message>();

            var propertyOperation = message.propertyOperation;

            if (message.source != null && message.source.simpleItem != null)
            {
                foreach (var simpleItem in message.source.simpleItem)
                {
                    if (simpleItem.name == "VideoSourceToken" && videoSourceToken != null && simpleItem.value != videoSourceToken)
                        return;
                    if (simpleItem.name == "VideoSourceConfigurationToken" && videoSourceConfToken != null && simpleItem.value != videoSourceConfToken)
                        return;
                    if (simpleItem.name == "VideoAnalyticsConfigurationToken" && videoAnalyticsConfToken != null && simpleItem.value != videoAnalyticsConfToken)
                        return;
                }
            }

            if (message.data == null)
                return;

            var snapshot = Parse(topic, message);
            snapshot.Time = message.utcTime;


            if (this.initialized != null && propertyOperation == PropertyOperation.initialized)
                this.initialized(snapshot);
            if (this.changed != null && propertyOperation == PropertyOperation.changed)
                this.changed(snapshot);
            if (this.deleted != null && propertyOperation == PropertyOperation.deleted)
                this.deleted(snapshot);
            
        } 
    }
}
