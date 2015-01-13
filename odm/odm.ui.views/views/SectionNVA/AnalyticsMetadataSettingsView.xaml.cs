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
	/// Interaction logic for AnalyticsMetadataSettingsView.xaml
	/// </summary>
	public partial class AnalyticsMetadataSettingsView : UserControl, INotifyPropertyChanged, IDisposable, IPlaybackController {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new AnalyticsMetadataSettingsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		//private CompositeDisposable disposables = new CompositeDisposable();  
		public LocalMetadata strings { get { return LocalMetadata.instance; } }
		public SaveCancelStrings SaveCanel { get { return SaveCancelStrings.instance; } }
		public CompositeDisposable subscription = new CompositeDisposable();

		private CompositeDisposable disposables = new CompositeDisposable();

		Model model;

		Dictionary<string, string> namespaces = new Dictionary<string, string>();
		private bool TryAddExploredNamespace(string prefix, string ns) {
			string existingNs;
			if (namespaces.TryGetValue(prefix, out existingNs)) {
				return existingNs == ns;
			}
			namespaces.Add(prefix, ns);
			return true;
		}
		private void ExploreNamespaces(IEnumerable<XmlElement> xmlElements) {
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
					while (!TryAddExploredNamespace(prefix, ns)) {
						prefix = prefixBase + i;
					}
				}
				if (e.ChildNodes != null) {
					ExploreNamespaces(
						e.ChildNodes
							.OfType<XmlNode>()
							.Where(x => x.NodeType == XmlNodeType.Element)
							.OfType<XmlElement>()
					);
				}
			}
		}

		private void Init(Model model) {
			this.model = model;

			InitializeComponent();

			//TODO: possible incorrect behaviour when multiple instances of odm are running
			if (AppDefaults.visualSettings.EventsCollect_IsEnabled) {
				var fi = AppDefaults.MetadataFileInfo;
				if (fi.Exists) {
					fi.Delete();
				}
			}

			//commands
			OnCompleted += () => {
				disposables.Dispose();
				subscription.Dispose();
			};
			ApplyCmd = new DelegateCommand(
				() => {
					FillModel();
					Success(new Result.Apply(model));
				}, () => true
			);

			//Start meta stream
			dispatch = Dispatcher.CurrentDispatcher;
			MetaData = new ObservableCollection<MetadataUnit>();

			Reload(activityContext.container.Resolve<INvtSession>());

		}

		void FillModel() {

		}

		public Dispatcher dispatch { get; private set; }

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
			DependencyProperty.Register("ApplyCmd", typeof(DelegateCommand), typeof(AnalyticsMetadataSettingsView));

		public MetadataUnit SelectedMeta {
			get { return (MetadataUnit)GetValue(SelectedMetaProperty); }
			set { SetValue(SelectedMetaProperty, value); }
		}
		public static readonly DependencyProperty SelectedMetaProperty =
			DependencyProperty.Register("SelectedMeta", typeof(MetadataUnit), typeof(AnalyticsMetadataSettingsView));

		public MetadataUnit LastMeta {
			get { return (MetadataUnit)GetValue(LastMetaProperty); }
			set { SetValue(LastMetaProperty, value); }
		}
		public static readonly DependencyProperty LastMetaProperty =
			DependencyProperty.Register("LastMeta", typeof(MetadataUnit), typeof(AnalyticsMetadataSettingsView));

		public ObservableCollection<MetadataUnit> MetaData {
			get { return (ObservableCollection<MetadataUnit>)GetValue(MetaDataProperty); }
			set { SetValue(MetaDataProperty, value); }
		}
		public static readonly DependencyProperty MetaDataProperty =
				DependencyProperty.Register("MetaData", typeof(ObservableCollection<MetadataUnit>), typeof(AnalyticsMetadataSettingsView));


		void Reload(INvtSession session) {
			var vs = AppDefaults.visualSettings;

			//vidBuff = new VideoBuffer(resolution.Width, resolution.Height);

			var streamSetup = new StreamSetup() {
				transport = new Transport() {
					protocol = AppDefaults.visualSettings.Transport_Type
				}
			};

			//TODO: provide a way of cancelation
			//VideoInfo.MediaUri = model.uri;
			//VideoStartup(VideoInfo);
			VideoStartup();

			//subscription.Add(session.GetStreamUri(strSetup, profile.token)
			//	.ObserveOnCurrentDispatcher()
			//	.Subscribe(uri => {
			//		VideoInfo.MediaUri = uri.Uri;
			//		VideoStartup(VideoInfo);
			//	}, err => {
			//	}));
		}

		IPlayer playerEngine;
		IPlaybackSession playbackSession;
		void VideoStartup() {
			playerEngine = new HostedPlayer();

			var account = AccountManager.Instance.CurrentAccount;
			UserNameToken usToken = null;
			if (!account.IsAnonymous) {
				usToken = new UserNameToken(account.Name, account.Password);
			}
			playerEngine.SetMetadataReciever(new MetadataFramer((stream) => {
				using (Disposable.Create(() => stream.Dispose())) {
					var xml = new XmlDocument();
					xml.Load(stream);

					try {
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
			switch (vs.Transport_Type) {
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

			MediaStreamInfo mstreamInfo = new MediaStreamInfo(model.uri, medtranp, usToken);
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


}
