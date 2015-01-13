using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using odm.controllers;
using odm.core;
using odm.ui.activities;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.links;
using odm.ui.views;
using onvif.services;
using onvif.utils;
using utils;
using System.Windows.Controls;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for DeviceVew.xaml
	/// </summary>
	public partial class SectionDevice : UserControl, IDisposable {
		#region Loader
		public static FSharpAsync<DeviceViewArgs> Load(DeviceDescriptionHolder devHolder, NvtSessionFactory sessionFactory) {
			DeviceViewArgs args = new DeviceViewArgs();
			args.deviceIconUri = devHolder.DeviceIconUri;
			args.deviceModel = devHolder.DeviceModel;
			args.manufacturer = devHolder.Manufacturer;

			return Apm.Iterate(LoadImpl(devHolder, sessionFactory, args)).Select(f => { return args; });
		}
		static IEnumerable<FSharpAsync<Unit>> LoadImpl(DeviceDescriptionHolder devHolder, NvtSessionFactory sessionFactory, DeviceViewArgs args) {
			yield return sessionFactory.CreateSession(devHolder.Uris).Select(f => { args.nvtSession = f; return (Unit)null; });
			args.odmSession = new OdmSession(args.nvtSession);
			yield return args.nvtSession.GetAllCapabilities().Select(caps => { args.capabilities = caps; return (Unit)null; });
		}
		#endregion Loader

		public SectionDevice(IUnityContainer container) {
			this.container = container;
			dispatch = Dispatcher.CurrentDispatcher;

			InitializeComponent();
		}

		#region local_vars
		Dispatcher dispatch;
		CompositeDisposable disposables = new CompositeDisposable();
		IEventAggregator eventAggregator;
		IUnityContainer container;
		OdmSession odmSession;
		List<ButtonBase> Buttons = new List<ButtonBase>();

		LinkButtonSwitch btnSwitch;
		#endregion local_vars

		public void Init(DeviceViewArgs args) {
			eventAggregator = container.Resolve<IEventAggregator>();
			filtersList = new ObservableCollection<FilterExpression>();

			btnSwitch = new LinkButtonSwitch(eventAggregator);
			btnSwitch.ClearSelection = () => {
				buttonsList.UnselectAll();
			};
			disposables.Add(Observable.FromEventPattern(buttonsList, "SelectionChanged")
			.Subscribe(e => {
				btnSwitch.SelectedButton = (ButtonBase)buttonsList.SelectedItem;
			}));
			disposables.Add(btnSwitch);

			var subcrtok = eventAggregator.GetEvent<AddEventsFilterClick>().Subscribe(arg => {
				dispatch.BeginInvoke(() => {
					ReSubscribe(args);
				});
			});
			disposables.Add(Disposable.Create(() => {
				eventAggregator.GetEvent<AddEventsFilterClick>().Unsubscribe(subcrtok);
			}));

			odmSession = new OdmSession(args.nvtSession);

			InitDeviceImage(args);
			LoadButtons(args);
			EventsSubscription(args);
		}
		#region Events
		public ObservableCollection<FilterExpression> filtersList { get; set; }
		CompositeDisposable EventSubscriptions = new CompositeDisposable();
		EventsStorage events = new EventsStorage();

		void ReSubscribe(DeviceViewArgs args) {
			if (EventSubscriptions != null && !EventSubscriptions.IsDisposed) {
				EventSubscriptions.Dispose();
				EventSubscriptions = new CompositeDisposable();
			}

			List<MessageContentFilter> contArray = new List<MessageContentFilter>();
			List<TopicExpressionFilter> topArray = new List<TopicExpressionFilter>();

			filtersList.ForEach(item => {
				if (item.FilterType == FilterExpression.ftype.CONTENT) {
					contArray.Add(new MessageContentFilter() { dialect = item.Dialect, expression = item.Value, namespaces = item.Namespaces });
				} else {
					topArray.Add(new TopicExpressionFilter() { dialect = item.Dialect, expression = item.Value, namespaces = item.Namespaces });
				}
			});

			switch (AppDefaults.visualSettings.Event_Subscription_Type) {
				case VisualSettings.EventType.ONLY_BASE:
				SubscribeBase(args.odmSession, contArray.ToArray(), topArray.ToArray());
				break;
				case VisualSettings.EventType.ONLY_PULL:
				SubscribePullPoint(args.odmSession, contArray.ToArray(), topArray.ToArray());
				break;
				case VisualSettings.EventType.TRY_PULL:
				if (args.capabilities.events != null) {
					if (args.capabilities.events.wsPullPointSupport == true) {
						SubscribePullPoint(args.odmSession, contArray.ToArray(), topArray.ToArray());
					} else {
						SubscribeBase(args.odmSession, contArray.ToArray(), topArray.ToArray());
					}
				}
				break;
			}
		}
		void SubscribePullPoint(OdmSession facade, MessageContentFilter[] contArray, TopicExpressionFilter[] topArray) {
			EventSubscriptions.Add(
					 facade.GetPullPointEvents(topArray, contArray).Subscribe(
						  onvifEvent => {
							  try {
								  dispatch.BeginInvoke(() => {
									  var evdescr = new EventDescriptor(onvifEvent);
									  events.AddEvent(evdescr);
									  eventAggregator.GetEvent<DeviceEventReceived>().Publish(new DeviceEventArgs("", evdescr));
								  });
							  } catch (Exception err) {
								  dbg.Error(err);
							  }
						  }, err => {

							  dbg.Error(err);
							  dispatch.BeginInvoke(() => {
								  var evdescr = new EventDescriptor(null);
								  evdescr.ErrorMessage = err.Message;
								  events.AddEvent(evdescr);
								  eventAggregator.GetEvent<DeviceEventReceived>().Publish(new DeviceEventArgs("", evdescr));
							  });
						  }
					 )
				);
		}
		void SubscribeBase(OdmSession facade, MessageContentFilter[] contArray, TopicExpressionFilter[] topArray) {
			EventSubscriptions.Add(
					 facade.GetBaseEvents(AppDefaults.visualSettings.Base_Subscription_Port, topArray, contArray).Subscribe(
						  onvifEvent => {
							  try {
								  dispatch.BeginInvoke(() => {
									  var evdescr = new EventDescriptor(onvifEvent);
									  events.AddEvent(evdescr);
									  eventAggregator.GetEvent<DeviceEventReceived>().Publish(new DeviceEventArgs("", evdescr));
								  });
							  } catch (Exception err) {
								  dbg.Error(err);
							  }
						  }, err => {
							  dbg.Error(err);
							  dispatch.BeginInvoke(() => {
								  var evdescr = new EventDescriptor(null);
								  evdescr.ErrorMessage = err.Message;
								  events.AddEvent(evdescr);
								  eventAggregator.GetEvent<DeviceEventReceived>().Publish(new DeviceEventArgs("", evdescr));
							  });
						  }
					 )
				);
		}
		void EventsSubscription(DeviceViewArgs args) {
			if (AppDefaults.visualSettings.Events_IsEnabled && args.capabilities.events != null) {

				//if (AppDefaults.visualSettings.DefEventFilter != "")
				//    filtersList.Add(new FilterExpression() { Value = AppDefaults.visualSettings.DefEventFilter });
				ReSubscribe(args);
			}
		}
		#endregion Events
		void InitDeviceImage(DeviceViewArgs args) {
			devImage.Source = odm.ui.Resources.loading_icon.ToBitmapSource();
			ImageSource imgsrc = odm.ui.Resources.onvif.ToBitmapSource();

			devImage.CreateBinding(Image.ToolTipProperty, LocalSnapshot.instance, s => s.loading);

			//"/odm;component/Resources/Images/onvif.png"
			//this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.loading);

			if (args.deviceIconUri != null) {
				Uri uri = new Uri(args.nvtSession.deviceUri, args.deviceIconUri);
				disposables.Add(odmSession.DownloadStream(uri, null)
					.ObserveOnCurrentDispatcher()
					.Subscribe(imgStream => {
						try {
							BitmapImage bitmap = new BitmapImage();
							bitmap.BeginInit();
							bitmap.CacheOption = BitmapCacheOption.OnLoad;
							bitmap.StreamSource = imgStream;
							bitmap.EndInit();
							devImage.Source = bitmap;
							devImage.ToolTip = "";

						} catch (Exception err) {
							dbg.Error(err);
							devImage.Source = imgsrc;
							devImage.CreateBinding(Image.ToolTipProperty, LocalSnapshot.instance, s => s.imagecorrupt);
						}
					}, err => {
						dbg.Error(err.Message);
					}));
			} else {
				devImage.Source = imgsrc;
				devImage.ToolTip = "";
			}
		}
		void LoadButtons(DeviceViewArgs args) {
			try {
				var curAccount = AccountManager.Instance.CurrentAccount;
				Buttons.Add(new IdentificationButton(eventAggregator, args.nvtSession, curAccount));
				Buttons.Add(new DateTimeButton(eventAggregator, args.nvtSession, curAccount));
				Buttons.Add(new MaintenanceButton(eventAggregator, args.nvtSession, curAccount, args.capabilities, args.deviceModel, args.manufacturer));
				var caps = args.capabilities;
				var devCaps = caps != null ? caps.device : null;

				if (devCaps != null) {
					Buttons.Add(new NetworkButton(eventAggregator, args.nvtSession, curAccount));
					Buttons.Add(new UserManagerButton(eventAggregator, args.nvtSession, curAccount, args.deviceModel, args.manufacturer));
					if (devCaps.security != null) {
						Buttons.Add(new SequrityButton(eventAggregator, args.nvtSession, curAccount));
					}
					var sysCaps = devCaps.system;
					if (sysCaps != null) {
						if (sysCaps.systemLogging) {
							SysLogDescriptor slogdescr = new SysLogDescriptor(new SysLogType(SystemLogType.system), null, "");
							Buttons.Add(new SystemLogButton(eventAggregator, args.nvtSession, curAccount, slogdescr));
						}
					}
					var ioCaps = devCaps.io;
					if (ioCaps != null){
						var hasRelays = ioCaps.relayOutputsSpecified && ioCaps.relayOutputs > 0;
						var hasInputs = ioCaps.inputConnectorsSpecified && ioCaps.inputConnectors > 0;
						if (hasRelays || hasInputs) {
							Buttons.Add(new DigitalIOButton(eventAggregator, args.nvtSession, curAccount));
						}
					}
				}

				if (caps.actionEngine != null) {
					Buttons.Add(new ActionsButton(eventAggregator, args.nvtSession, curAccount));
					Buttons.Add(new ActionTriggersButton(eventAggregator, args.nvtSession, curAccount));
				}

				Buttons.Add(new WebPageButton(eventAggregator, args.nvtSession, curAccount));

				if (AppDefaults.visualSettings.Events_IsEnabled) {
					Buttons.Add(new DeviceEventsButton(eventAggregator, filtersList, events, args.nvtSession, curAccount));
				}

				if (caps.extension != null && caps.extension.receiver != null)
					Buttons.Add(new ReceiversButton(eventAggregator, args.nvtSession, curAccount));

				//Buttons.Add(new XMLExplorerButton(eventAggregator, session, curAccount));
			} catch (Exception err) {
				dbg.Error(err);
			}

			buttonsList.ItemsSource = Buttons;
		}

		public void Dispose() {
			disposables.Dispose();
			EventSubscriptions.Dispose();
		}
	}
	public class DeviceViewArgs {
		public INvtSession nvtSession { get; set; }
		public OdmSession odmSession { get; set; }
		public Capabilities capabilities { get; set; }
		public string deviceIconUri { get; set; }
		public string deviceModel { get; set; }
		public string manufacturer { get; set; }
	}

	public class EventsStorage {

		public EventsStorage() {
			eventsCollection = new ObservableCollection<EventDescriptor>();
		}
		public void AddEvent(EventDescriptor ev) {
			eventsCollection.Add(ev);
			while (eventsCollection.Count > 1000)
				eventsCollection.RemoveAt(0);
		}
		public void Clear() {
			eventsCollection.Clear();
		}

		public ObservableCollection<EventDescriptor> eventsCollection { get; protected set; }
	}
	public class EventDescriptor {

		const string KeyValueFormat = "{0}: {1}";

		public string ErrorMessage { get; set; }
		public EventDescriptor(OnvifEvent ev) {
			_onvifEvent = ev;
			//ev.message.Source.SimpleItem
		}
		OnvifEvent _onvifEvent;

		public string PropertyOperation {
			get {
				string val = "";
				if (_onvifEvent == null || _onvifEvent.message == null)
					return val;

				return _onvifEvent.message.propertyOperation.ToString();
			}
		}
		public string ArrivalTime {
			get {
				string val = "";
				if (_onvifEvent == null || _onvifEvent.message == null)
					return val;
				try {
					val = _onvifEvent.message.utcTime.ToLongTimeString();
				} catch { }

				return val;
			}
		}
		public string Key {
			get {
				if (_onvifEvent == null || _onvifEvent.message == null || _onvifEvent.message.key == null)
					return string.Empty;

				StringBuilder sb = new StringBuilder();

				var key = _onvifEvent.message.key;
				if (key.simpleItem != null) {
					foreach (var item in key.simpleItem)
						sb.AppendLine(string.Format(KeyValueFormat, item.name, item.value));
				}

				return sb.ToString().Trim();
			}
		}
		public string Topic {
			get {
				string val = "";
				if (_onvifEvent == null)
					return val;
				try {
					StringBuilder sb = new StringBuilder();
					int c = 0;

					_onvifEvent.topic.Any.ForEach(x => {
						if (c != 0)
							sb.AppendLine();
						sb.Append(x.Value);
						c++;
					});
					_onvifEvent.topic.AnyAttr.ForEach(x => {
						if (c != 0)
							sb.AppendLine();
						sb.Append(string.Format(KeyValueFormat, x.Name, x.Value));
						c++;
					});

					val = sb.ToString();
				} catch { }

				return val;
			}
		}
		public string Details {
			get {
				string val = "";
				if (_onvifEvent == null)
					return val;
				try {
					StringBuilder sb = new StringBuilder();
					int c = 0;
					if (_onvifEvent.message.extension != null) {
						_onvifEvent.message.extension.any.ForEach(x => {
							if (c != 0)
								sb.AppendLine();
							sb.Append(string.Format(KeyValueFormat, x.Name, x.Value));
							c++;
						});
					}

					val = sb.ToString();
				} catch (Exception err) {
					dbg.Error(err);
				}

				return val;
			}
		}
		public string Source {
			get {
				if (_onvifEvent == null || _onvifEvent.message == null || _onvifEvent.message.source == null)
					return string.Empty;

				StringBuilder sb = new StringBuilder();

				var source = _onvifEvent.message.source;
				if (source.simpleItem != null) {
					foreach (var item in source.simpleItem)
						sb.AppendLine(string.Format(KeyValueFormat, item.name, item.value));
				}

				return sb.ToString().Trim();
			}
		}
		public string Data {
			get {
				if (_onvifEvent == null || _onvifEvent.message == null || _onvifEvent.message.data == null)
					return string.Empty;

				StringBuilder sb = new StringBuilder();

				var data = _onvifEvent.message.data;
				if (data.simpleItem != null) {
					foreach (var item in data.simpleItem)
						sb.AppendLine(string.Format(KeyValueFormat, item.name, item.value));
				}

				return sb.ToString().Trim();
			}
		}

		public override string ToString() {
			string log = "";
			log = Topic + Environment.NewLine +
				 Key + Environment.NewLine +
				 PropertyOperation + Environment.NewLine +
				 Data + Environment.NewLine +
				 Details + Environment.NewLine + Environment.NewLine;
			return log;
		}

		private static object gate = new object();
		private static XslCompiledTransform s_xml2html = null;
		private static XslCompiledTransform xml2html {
			get {
				lock (gate) {
					if (s_xml2html == null) {
						var xslt = new XslCompiledTransform();

						var xmlReaderSettings = new XmlReaderSettings() {
							DtdProcessing = DtdProcessing.Parse
						};
						XsltSettings xsltSettings = new XsltSettings() {
							EnableScript = false,
							EnableDocumentFunction = false
						};

						using (var xmlReader = XmlReader.Create(@"xml2html/XmlToHtml10Basic.xslt", xmlReaderSettings)) {
							xslt.Load(xmlReader, xsltSettings, new XmlUrlResolver());
							xmlReader.Close();
						}
						s_xml2html = xslt;
					}
				}
				return s_xml2html;
			}
		}
		string ConvertToString(string doc) {
			try {
				var html = new StringBuilder();
				var writer = new StringWriter(html);

				XmlDocument xdoc = new XmlDocument();
				xdoc.LoadXml(doc);

				xml2html.Transform(xdoc, null, writer);
				return html.ToString();
			} catch (Exception err) {
				return err.Message;
			}
		}
	}
}
