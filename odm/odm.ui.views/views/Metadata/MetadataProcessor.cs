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

namespace odm.ui.views {

	public class MetadataProcessor {
		readonly EventMetadataProcessor eventMetadataProcessor;
		readonly SceneMetadataProcessor sceneMetadataProcessor;


		public MetadataProcessor(EventMetadataProcessor eventMetadataProcessor, SceneMetadataProcessor sceneMetadataProcessor) {
			this.eventMetadataProcessor = eventMetadataProcessor;
			this.sceneMetadataProcessor = sceneMetadataProcessor;
		}

		public void Process(MetadataStream metadata) {
			try {
				List<EventStream> events;
				List<VideoAnalyticsStream> analytics;
				ExtractStreams(metadata, out events, out analytics);

				if (eventMetadataProcessor != null)
					events.ForEach(es => eventMetadataProcessor.Process(es));
				if (sceneMetadataProcessor != null)
					analytics.ForEach(vas => sceneMetadataProcessor.Process(vas));
			} catch (Exception ex) {
				dbg.Error(ex);
			}
		}

		public void Process(Stream stream) {
			using (Disposable.Create(() => stream.Dispose())) {
				try {
					/*using (var sr = new StreamReader(stream)) {
						 var msg = sr.ReadToEnd();*/

					//TODO hot fix
					//msg = msg.Replace(new Dictionary<string, string> { { "<tt::", "<tt:" }, { "</tt::", "</tt:" } });

					//TODO workaround: a message comes with additional binary data before xml in the payload
					/*{
						 int index = msg.IndexOf("<?xml", 0);
						 msg = msg.Substring(index);
					}*/

					//TODO workaround #2: a message contains ElementItem called "BoundingBox" which has an incorrect format
					/*{
						 //if (msg.IndexOf("<tt:ElementName ") >= 0)
						 //    ;

						 const string startTag = "<tt:ElementItem Name=\"BoundingBox\">";
						 int startTagIndex = msg.IndexOf(startTag);
						 if (startTagIndex >= 0)
						 {
							  const string rectStartTag = "<tt:Rectangle ";
							  const string rectEndTag = " />";

							  msg = msg.Insert(startTagIndex + startTag.Length, rectStartTag);
							  const string endTag = "</tt:ElementItem>";
							  int contentStartIndex = startTagIndex + startTag.Length + rectStartTag.Length;
							  int endTagIndex = msg.IndexOf(endTag, contentStartIndex);
							  int contentEndIndex = endTagIndex;
							  msg = msg.Insert(endTagIndex, " />");

							  string content = msg.Substring(contentStartIndex, contentEndIndex - contentStartIndex);
							  msg = msg.Remove(contentStartIndex, contentEndIndex - contentStartIndex);

							  content = content.Replace("<left>", " left=\"");
							  content = content.Replace("</left>", "\" ");

							  content = content.Replace("<top>", " top=\"");
							  content = content.Replace("</top>", "\" ");

							  content = content.Replace("<right>", " right=\"");
							  content = content.Replace("</right>", "\" ");

							  content = content.Replace("<bottom>", " bottom=\"");
							  content = content.Replace("</bottom>", "\" ");

							  msg = msg.Insert(contentStartIndex, content);
						 }
					}*/

					/*Trace.WriteLine("\n===========================\n" + msg);

					using (var stream1 = new StringReader(msg)) {
						 var metadata = (MetadataStream)new XmlSerializer(typeof(MetadataStream)).Deserialize(stream1);
			  */

					var metadata = (MetadataStream)new XmlSerializer(typeof(MetadataStream)).Deserialize(stream);
					Process(metadata);
					/*}
			  }*/
				} catch (Exception ex) {
					Trace.WriteLine(ex.ToString());
					dbg.Error(ex);
				}
			}
		}

		private void ExtractStreams(MetadataStream metadata, out List<EventStream> events, out List<VideoAnalyticsStream> analytics) {
			events = new List<EventStream>();
			analytics = new List<VideoAnalyticsStream>();

			var items = metadata.items;
			if (items != null) {
				foreach (var item in items) {
					if (item is EventStream) {
						var ev = (EventStream)item;
						events.Add(ev);
					} else if (item is VideoAnalyticsStream) {
						var va = (VideoAnalyticsStream)item;
						analytics.Add(va);
					}
				}
			}
		}


	}
}
