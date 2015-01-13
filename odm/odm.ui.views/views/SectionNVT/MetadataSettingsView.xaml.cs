using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Xsl;

using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

using odm.core;
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views;
using onvif.services;
using utils;
using System.ComponentModel;
using System.Threading.Tasks;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for MetadataSettingsView.xaml
	/// </summary>
	public partial class MetadataSettingsView : UserControl, INotifyPropertyChanged, IDisposable, IPlaybackController {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new MetadataSettingsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		//private CompositeDisposable disposables = new CompositeDisposable();  
		public LocalMetadata strings { get { return LocalMetadata.instance; } }
		public SaveCancelStrings SaveCanel { get { return SaveCancelStrings.instance; } }
		public CompositeDisposable subscription = new CompositeDisposable();
		public IVideoInfo VideoInfo { get; protected set; }
		Profile profile;

		private CompositeDisposable disposables = new CompositeDisposable();

		Model model;
		
		Dictionary<string, string> namespaces = new Dictionary<string, string>();
		private bool TryAddExploredNamespace(string prefix, string ns) {
			string existingNs;
			if(namespaces.TryGetValue(prefix, out existingNs)){
				return existingNs==ns;
			}
			namespaces.Add(prefix, ns);
			return true;
		}
		private void ExploreNamespaces(IEnumerable<XmlElement> xmlElements){
			foreach (var e in xmlElements) {
				var ns = e.NamespaceURI;
				if (!String.IsNullOrEmpty(ns)) {
					var prefix = e.Prefix;
					if (String.IsNullOrEmpty(prefix)) {
						prefix = e.GetPrefixOfNamespace(ns);
						if (String.IsNullOrEmpty(prefix)) {
							prefix = "ns";
						}
					}
					var i = 0;
					var prefixBase = prefix;
					while (!TryAddExploredNamespace(prefix,ns)) {
						prefix = prefixBase+i;
					}
				}
				if(e.ChildNodes!=null){
					ExploreNamespaces(
						e.ChildNodes
							.OfType<XmlNode>()
							.Where(x=>x.NodeType == XmlNodeType.Element)
							.OfType<XmlElement>()
					);
				}
			}
		}

		private void Init(Model model) {
			this.DataContext = model;
			this.model = model;
			if (model.topicSet != null && model.topicSet.Any != null) {
				ExploreNamespaces(model.topicSet.Any);
			}
			
			InitializeComponent();

			//TODO: possible incorrect behaviour when multiple instances of odm are running
			if (AppDefaults.visualSettings.EventsCollect_IsEnabled) {
				var fi = AppDefaults.MetadataFileInfo;
				if (fi.Exists) {
					fi.Delete();
				}
			}

			ExpressionArguments args = new ExpressionArguments(
				model.messageContentFilterDialects, model.messageContentFilters,
				model.topicExpressionDialects, model.topicExpressionFilters, model.topicSet, namespaces);
			expressionFilters.Init(args);

			//commands
			OnCompleted += () => {
				disposables.Dispose();
				subscription.Dispose();
			};
			ApplyCmd = new DelegateCommand(
				() => {
					FillModel();
					Success(new Result.Apply(model)); 
				},() => true
			);

			//Start meta stream
			profile = activityContext.container.Resolve<Profile>();
			dispatch = Dispatcher.CurrentDispatcher;
			MetaData = new ObservableCollection<MetadataUnit>();

			VideoInfo = activityContext.container.Resolve<IVideoInfo>();
			VideoInfo.Resolution = new Size(800, 600);
			Reload(activityContext.container.Resolve<INvtSession>());

			includeAnalitycs.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.includeAnalitycs, (m, v) => { m.includeAnalitycs = v; });
			includePtzPosition.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.includePtzPosition, (m, v) => { m.includePtzPosition = v; });
			includePtzStatus.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.includePtzStatus, (m, v) => { m.includePtzStatus = v; });
			expressionFilters.CreateBinding(ExpressionFilterControl.IsIncludeEventsProperty, model, x => { return x.includeEvents; }, (m, v) => { m.includeEvents = v; });
			//includeEvents.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.includeEvents, (m, v) => { m.includeEvents = v; });

			includePtzPosition.CreateBinding(CheckBox.VisibilityProperty, model, x => x.isPtzPositionSupported ? Visibility.Visible : Visibility.Collapsed);
			includePtzStatus.CreateBinding(CheckBox.VisibilityProperty, model, x => x.isPtzStatusSupported ? Visibility.Visible : Visibility.Collapsed);
		}

		void FillModel() {
			List<MessageContentFilter> contArr = new List<MessageContentFilter>();
			List<TopicExpressionFilter> topicArr = new List<TopicExpressionFilter>();

			expressionFilters.filtersList.Where(f => f.FilterType == FilterExpression.ftype.CONTENT).ForEach(filtr => {
				contArr.Add(new MessageContentFilter() { dialect = filtr.Dialect, expression = filtr.Value, namespaces = filtr.Namespaces });
			});
			expressionFilters.filtersList.Where(f => f.FilterType == FilterExpression.ftype.TOPIC).ForEach(filtr => {
				topicArr.Add(new TopicExpressionFilter() { dialect = filtr.Dialect, expression = filtr.Value, namespaces = filtr.Namespaces });
			});

			model.messageContentFilters = contArr.ToArray();
			model.topicExpressionFilters = topicArr.ToArray();
		}

		public Dispatcher dispatch { get; private set; }
		/*void MetadataReceived(DeviceMetadataArgs evarg) {
			dispatch.Invoke((ThreadStart)delegate {
				if (evarg.vsToken == VideoInfo.ChanToken) {
					var meta = new MetadataUnit(evarg.xml);
					this.MetaData.ShiftPush(meta, 1000);
					LastMeta = meta;
				}
			});
		}*/

		public string MetaPath {
			get {
				return AppDefaults.MetadataFolderPath;
			}
		}

		public DelegateCommand ApplyCmd {
			get { return (DelegateCommand)GetValue(ApplyCmdProperty); }
			set { SetValue(ApplyCmdProperty, value); }
		}
		public static readonly DependencyProperty ApplyCmdProperty =
			DependencyProperty.Register("ApplyCmd", typeof(DelegateCommand), typeof(MetadataSettingsView));

		public MetadataUnit SelectedMeta {
			get { return (MetadataUnit)GetValue(SelectedMetaProperty); }
			set { SetValue(SelectedMetaProperty, value); }
		}
		public static readonly DependencyProperty SelectedMetaProperty =
			DependencyProperty.Register("SelectedMeta", typeof(MetadataUnit), typeof(MetadataSettingsView));

		public MetadataUnit LastMeta {
			get { return (MetadataUnit)GetValue(LastMetaProperty); }
			set { SetValue(LastMetaProperty, value); }
		}
		public static readonly DependencyProperty LastMetaProperty =
			DependencyProperty.Register("LastMeta", typeof(MetadataUnit), typeof(MetadataSettingsView));

		public ObservableCollection<MetadataUnit> MetaData {
			get { return (ObservableCollection<MetadataUnit>)GetValue(MetaDataProperty); }
			set { SetValue(MetaDataProperty, value); }
		}
		public static readonly DependencyProperty MetaDataProperty =
			DependencyProperty.Register("MetaData", typeof(ObservableCollection<MetadataUnit>), typeof(MetadataSettingsView));

		//public ObservableCollection<MetadataUnit> MetaData { get; set; }

		void Reload(INvtSession session) {
			var vs = AppDefaults.visualSettings;

			StreamSetup strSetup = new StreamSetup() {
				stream = StreamType.rtpUnicast,
				transport = new Transport(){
					protocol = vs.Transport_Type,
					tunnel = null
				}
			};

			//TODO: provide a way of cancelation
			//try {
			//	var streamInfo = await session.GetStreamUri(strSetup, profile.token);
			//	VideoInfo.MediaUri = streamInfo.uri;
			//	VideoStartup(VideoInfo);
			//} catch (Exception err) {
			//	dbg.Error(err);
			//	throw;
			//}

			subscription.Add(
				session.GetStreamUri(strSetup, profile.token)
				.ObserveOnCurrentDispatcher()
				.Subscribe(
					uri => {
						VideoInfo.MediaUri = uri.uri;
						VideoStartup(VideoInfo);
					}, 
					err => {
					}
				)
			);
		}

		IPlayer playerEngine;
		IPlaybackSession playbackSession;
		void VideoStartup(IVideoInfo iVideo) {
			playerEngine = new HostedPlayer();
			
			var account = AccountManager.Instance.CurrentAccount;
			UserNameToken usToken = null;
			if (!account.IsAnonymous) {
				usToken = new UserNameToken(account.Name, account.Password);
			}
			playerEngine.SetMetadataReciever(new MetadataFramer((stream) => {
				using (Disposable.Create(() => stream.Dispose())) {
					var xml = new XmlDocument();
					try {
						xml.Load(stream);
						if (xml.DocumentElement != null && xml.DocumentElement.HasChildNodes) {
							
							//TODO: possible incorrect behaviour when multiple instances of odm are running
							if (AppDefaults.visualSettings.EventsCollect_IsEnabled) {
								try {
									using (var sw = AppDefaults.MetadataFileInfo.AppendText()) {
										using (var xw = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = true, CloseOutput = false })) {
											xml.DocumentElement.WriteTo(xw);
										}
										sw.WriteLine();
										sw.WriteLine("<!--------------------------------------------------------------------------------!>");
									}
								} catch {
									//swallow error
								}
							}

							MetadataReceived(xml);
						}
					} catch (Exception err) {

						//TODO: possible incorrect behaviour when multiple instances of odm are running
						if (AppDefaults.visualSettings.EventsCollect_IsEnabled) {
							try {
								using (var sw = AppDefaults.MetadataFileInfo.AppendText()) {
									sw.WriteLine("<!---------------------------------------------------------------------------------");
									sw.WriteLine("ERROR: {0}", err.Message);
									sw.WriteLine("---------------------------------------------------------------------------------!>");
								}
							} catch {
								//swallow error
								dbg.Error(err);
							}
						}

						dbg.Error(err);
					}
				}
			}));
			var vs = AppDefaults.visualSettings;
			MediaStreamInfo.Transport medtranp = MediaStreamInfo.Transport.Tcp;
			switch(vs.Transport_Type){
				case TransportProtocol.http:
					medtranp = MediaStreamInfo.Transport.Http;
					break;
				case TransportProtocol.rtsp:
					medtranp = MediaStreamInfo.Transport.Tcp;
					break;
				case TransportProtocol.tcp:
					medtranp = MediaStreamInfo.Transport.Tcp;
					break;
				case TransportProtocol.udp:
					medtranp = MediaStreamInfo.Transport.Udp;
					break;
			}
			
			MediaStreamInfo mstreamInfo = new MediaStreamInfo(iVideo.MediaUri, medtranp, usToken);
			playerEngine.Play(mstreamInfo, this);

			disposables.Add(playerEngine);
		}
		void MetadataReceived(XmlDocument xml) {
			dispatch.Invoke((ThreadStart)delegate {
				var meta = new MetadataUnit(xml);
				this.MetaData.ShiftPush(meta, 1000);
				LastMeta = meta;
			});
		}

		public void Dispose() {
			Cancel();
		}

		public void Shutdown() {
		}

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class MetadataUnit : DependencyObject {
		public MetadataUnit(XmlDocument value) {
			Value = value;
			InitTextView();
		}

		void InitTextView() {
			TextView = "";
			Value.DocumentElement.ChildNodes.ForEach(chn => {
				var el = chn as System.Xml.XmlElement;
				if (el != null) {
					TextView += "meta type: " + el.LocalName + "\n";
				}
			});

			var strs = Value.InnerText.Split(':');
			strs.ForEach((string str) => {
				TextView += str + ": ";
			});
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
						using (var xsltStream = EmbeddedResource.xml2html_XmlToHtml10Basic_xslt.GetStream()) {
							//using (var xmlReader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + @"/xml2html/XmlToHtml10Basic.xslt", xmlReaderSettings)) {
							using (var xmlReader = XmlReader.Create(xsltStream, xmlReaderSettings)) {
								xslt.Load(xmlReader, xsltSettings, new XmlUrlResolver());
								xmlReader.Close();
							}
						}
						s_xml2html = xslt;
					}
				}
				return s_xml2html;
			}
		}

		string ConvertToString(XmlDocument doc) {
			try {
				var html = new StringBuilder();
				var writer = new StringWriter(html);
				xml2html.Transform(doc, null, writer);
				return html.ToString();
			} catch (Exception err) {
				return err.Message;
			}
		}

		public string TextView {
			get { return (string)GetValue(TextViewProperty); }
			set { SetValue(TextViewProperty, value); }
		}
		public static readonly DependencyProperty TextViewProperty =
			DependencyProperty.Register("TextView", typeof(string), typeof(MetadataUnit), new UIPropertyMetadata());

		public string html {
			get { return (string)GetValue(htmlProperty); }
			set { SetValue(htmlProperty, value); }
		}
		public static readonly DependencyProperty htmlProperty =
			DependencyProperty.Register("html", typeof(string), typeof(MetadataUnit));

		public XmlDocument Value {
			get { return (XmlDocument)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(XmlDocument), typeof(MetadataUnit), new PropertyMetadata(null, (obj, evarg) => {
				if (((XmlDocument)evarg.NewValue) != null) {
					((MetadataUnit)obj).html = ((MetadataUnit)obj).ConvertToString((XmlDocument)evarg.NewValue);
				}
			}));
	}
}
