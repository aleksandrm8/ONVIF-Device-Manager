using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.activities;
using onvif.services;
using utils;

namespace odm.ui {
	/// <summary>
	/// Interaction logic for ToolBarView.xaml
	/// </summary>
	public partial class AppSettingsView : UserControl, IDisposable {
		public static readonly RoutedCommand ApplyCommand = new RoutedCommand("ApplyCommand", typeof(AppSettingsView));
		public static readonly RoutedCommand CancelCommand = new RoutedCommand("CancelCommand", typeof(AppSettingsView));

		#region Activity definition
		public static FSharpAsync<Unit> Show(IUnityContainer container) {
			return container.StartViewActivity<Unit>(context => {
				var view = new AppSettingsView(context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private IActivityContext<Unit> context;
		private bool completed = false;
		private CompositeDisposable disposables = new CompositeDisposable();

		public AppSettingsView(IActivityContext<Unit> context) {
			this.context = context;
			LocalesCollection = new ObservableCollection<odm.localization.Language>();

			this.CommandBindings.Add(
				new CommandBinding(
					AppSettingsView.ApplyCommand,
					(s, a) => {
						odm.localization.Language.Current = (localization.Language)langValue.SelectedItem;
						odm.ui.Properties.Settings.Default.DefaultLocaleIso3 = odm.localization.Language.Current.iso3;
						odm.ui.Properties.Settings.Default.Save();

						var vs = AppDefaults.visualSettings;

						vs.Events_IsEnabled = enableEventValue.IsChecked.Value;
						vs.EventsCollect_IsEnabled = collectEventsValue.IsChecked.Value;
						vs.Snapshot_IsEnabled = enableSnapshotValue.IsChecked.Value;
						vs.CustomAnalytics_IsEnabled = enableCustomAnalyticsValuse.IsChecked.Value;
						vs.ui_video_rendering_fps = videoRenderingFpsValue.Value ?? 30;
						vs.Event_Subscription_Type = ((KeyValuePair<VisualSettings.EventType, string>)eventType.SelectedValue).Key;
						vs.Base_Subscription_Port = basePortValue.Value.Value;
						vs.ShowVideoPlaybackStatistics = showVideoPlaybackStatisticsValue.IsChecked == true;
						vs.Transport_Type = ((KeyValuePair<TransportProtocol, string>)transportTypes.SelectedValue).Key;
						vs.OpenInExternalWebBrowser = webValue.IsChecked.Value;
                        vs.EnableGraphicAnnotation = enableGraphicAnnotationValue.IsChecked.Value;

						vs.UseOnlyCommonFilterView = enableOnlyDefValue.IsChecked.Value;

						AppDefaults.UpdateVisualSettings(vs);


						Success();
					}
				)
			);

			this.CommandBindings.Add(
				new CommandBinding(
					AppSettingsView.CancelCommand,
					(s, a) => {

						Success();
					}
				)
			);

			InitializeComponent();

			BindData();
			LocaLization();
		}

		public ObservableCollection<odm.localization.Language> LocalesCollection { get; set; }
		public LocalAppSettings Strings { get { return LocalAppSettings.instance; } }
		public LocalButtons ButtonStrings { get { return LocalButtons.instance; } }
		public LocalTitles Titles { get { return LocalTitles.instance; } }
		public Dictionary<odm.ui.VisualSettings.EventType, string> eventTypesDict = new Dictionary<VisualSettings.EventType, string>();
		public Dictionary<odm.ui.VisualSettings.TlsMode, string> tlsModesDict = new Dictionary<odm.ui.VisualSettings.TlsMode, string>();
		public Dictionary<TransportProtocol, string> transportModesDict = new Dictionary<TransportProtocol, string>();

		void BindData() {
			var vs = AppDefaults.visualSettings;

			//transportCaption;
            transportModesDict.Add(TransportProtocol.http, "HTTP");
            transportModesDict.Add(TransportProtocol.rtsp, "TCP");
            transportModesDict.Add(TransportProtocol.udp, "UDP");

			transportTypes.ItemsSource = transportModesDict;
			transportTypes.SelectedValue = transportModesDict.SingleOrDefault(x => x.Key == vs.Transport_Type);

			//tlsModeValue

			eventTypesDict.Add(VisualSettings.EventType.ONLY_PULL, Strings.eventOnlyPullPoint);
			eventTypesDict.Add(VisualSettings.EventType.ONLY_BASE, Strings.eventOnlyBase);
			eventTypesDict.Add(VisualSettings.EventType.TRY_PULL, Strings.eventTryPullPoint);

			eventType.ItemsSource = eventTypesDict;
			eventType.SelectedValue = eventTypesDict.SingleOrDefault(x => x.Key == vs.Event_Subscription_Type);

			eventType.SelectionChanged += new SelectionChangedEventHandler((obj, evargs) => {
				if (((KeyValuePair<VisualSettings.EventType, string>)eventType.SelectedValue).Key == VisualSettings.EventType.ONLY_PULL) {
					basePortValue.IsEnabled = false;
					basePortCaption.IsEnabled = false;
				} else {
					basePortValue.IsEnabled = true;
					basePortCaption.IsEnabled = true;
				}
			});

			basePortValue.Value = vs.Base_Subscription_Port;

			webValue.IsChecked = vs.OpenInExternalWebBrowser;
            enableGraphicAnnotationValue.IsChecked = vs.EnableGraphicAnnotation;
            
			enableEventValue.IsChecked = vs.Events_IsEnabled;
			collectEventsValue.IsChecked = vs.EventsCollect_IsEnabled;
			enableSnapshotValue.IsChecked = vs.Snapshot_IsEnabled;
			enableCustomAnalyticsValuse.IsChecked = vs.CustomAnalytics_IsEnabled;

			int maxDefault = 100;
			videoRenderingFpsValue.Minimum = 1;
			videoRenderingFpsValue.Maximum = maxDefault;
			int fps = (AppDefaults.visualSettings.ui_video_rendering_fps <= 0 || AppDefaults.visualSettings.ui_video_rendering_fps > maxDefault)
						?
						maxDefault : AppDefaults.visualSettings.ui_video_rendering_fps;
			videoRenderingFpsValue.Value = fps;

			showVideoPlaybackStatisticsValue.IsChecked = vs.ShowVideoPlaybackStatistics;

			enableOnlyDefValue.IsChecked = vs.UseOnlyCommonFilterView;
		}

		void LocaLization() {
			IEnumerable<odm.localization.Language> langs = odm.localization.Language.AvailableLanguages;
			odm.ui.controls.ListItem<odm.localization.Language>[] list = langs.Select(x => odm.ui.controls.ListItem.Wrap(x, y => y.DisplayName)).Where(u => u.Unwrap().iso3 != null).ToArray();

			var defItem = list.Where(x => x.Unwrap().iso3 == odm.ui.Properties.Settings.Default.DefaultLocaleIso3).FirstOrDefault();

			list.ForEach(x => langValue.Items.Add(x.Unwrap()));

			if (defItem == null) {
				defItem = odm.ui.controls.ListItem.Wrap(odm.localization.Language.Default, x => "english");
				LocalesCollection.Add(defItem.Unwrap());
			}

			langValue.SelectedItem = defItem.Unwrap();

			basePortCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.basePortCaption);
			eventTypeCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.eventTypeCaption);

			webCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.webbrowser);

			collectEventsCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.collectEventsCaption);
			enableCustomAnalyticsCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.enableCustomAnalyticsCaption);
			enableEventCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.enableEventCaption);
            enableGraphicAnnotationCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.enableGraphicAnnotationCaption);
			enableSnapshotCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.enableSnapshotCaption);
			langCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.langCaption);
			videoRenderingFpsCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.videoRenderingFpsCaption);
            enableOnlyDefMode.CreateBinding(TextBlock.TextProperty, Strings, x => x.enableOnlyDefModeCaption);
			showVideoPlaybackStatisticsCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.showVideoPlaybackStatisticsCaption);
			eventCaption.CreateBinding(GroupBox.HeaderProperty, Strings, x => x.eventCaption);
			transportCaption.CreateBinding(TextBlock.TextProperty, Strings, x=>x.videotransport);


			applyButton.CreateBinding(Button.ContentProperty, ButtonStrings, x => x.apply);
			cancelButton.CreateBinding(Button.ContentProperty, ButtonStrings, x => x.cancel);

			uiCaption.CreateBinding(GroupBox.HeaderProperty, Strings, x => x.uiCaption);
			communicationCaption.CreateBinding(GroupBox.HeaderProperty, Strings, x => x.communicationCaption);

			this.CreateBinding(NavigationContext.TitleProperty, Titles, x => x.appsettings);
		}

		private void HandleRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
			var hl = sender as Hyperlink;
			if (hl != null) {
				string navigateUri = hl.NavigateUri.ToString();
				// if the URI somehow came from an untrusted source, make sure to
				// validate it before calling Process.Start(), e.g. check to see
				// the scheme is HTTP, etc.
				Process.Start(new ProcessStartInfo(navigateUri));
				e.Handled = true;
			}
		}

		private void CompleteWith(Action cont) {
			Dispatcher.BeginInvoke(() => {
				if (!completed) {
					completed = true;
					cont();
					OnCompleted();
					disposables.Dispose();
				}
			});
		}

		protected virtual void OnCompleted() {
			//activity has been completed
		}
		public void Success() {
			CompleteWith(() => {
				context.Success(null);
			});
		}

		public void Dispose() {
			CompleteWith(() => {
				context.Success(null);
			});
		}
	}
}
