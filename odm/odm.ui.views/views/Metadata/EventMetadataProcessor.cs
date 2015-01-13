using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using odm.player;
using onvif.services;
using utils;

namespace odm.ui.views
{
    public interface INotificationMessageProcessor
    {
        void Process(NotificationMessageHolderType message);
    }

    public class EventMetadataProcessor
    {
        public readonly IList<INotificationMessageProcessor> Processors = new List<INotificationMessageProcessor>();
        
        
        public EventMetadataProcessor ()
        {
            
        }

        public void Process(EventStream eventStream)
        {
            try
            {
                var messages = ExtractNotificationMessages(eventStream);
                foreach (var message in messages)
                    foreach (var p in Processors)
                        try
                        {
                            p.Process(message);
                        }
                        catch (Exception ex) 
                        {
                            Trace.WriteLine(string.Format("metadata processor {0} failed for message {1}: {2}", p.GetType(), message.Message.InnerText,ex));
                            //dbg.Error(ex);
                        }
            }
            catch (Exception ex)
            {
                dbg.Error(ex);
            }
        }

		  private NotificationMessageHolderType[] ExtractNotificationMessages(EventStream ev) {
			  var messages = new List<NotificationMessageHolderType>();
			  if (ev == null) {
				  return new NotificationMessageHolderType[0];
			  }
			  var items = ev.items;
			  if (items == null) {
				  return new NotificationMessageHolderType[0];
			  }
			  return items.OfType<NotificationMessageHolderType>().ToArray();
		  }

    }
}
