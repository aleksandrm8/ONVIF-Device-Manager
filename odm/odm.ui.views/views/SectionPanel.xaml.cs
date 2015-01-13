using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using utils;
using odm.core;
using onvif.utils;
using onvif.services;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using odm.ui.activities;
using odm.ui.controls;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using odm.ui.views.SectionNVA;
using odm.ui.views.SectionNVT;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for SectionPanel.xaml
	/// </summary>
	public partial class SectionPanel : UserControl, IDisposable {
		public SectionPanel() {
			InitializeComponent();
		}
		protected CompositeDisposable disposables = new CompositeDisposable();

		public void Init(bool isActive, Func<IDisposable> activate, Action deactivate) {
			Activate = activate;
			Deactivate = deactivate;
			header.IsChecked = isActive;

			disposables.Add(Observable.FromEventPattern(header, "Checked").Subscribe(ev => {
				Deactivate();
				SetState();
			}));
			//disposables.Add(Observable.FromEventPattern(header, "Unchecked").Subscribe(ev => {
			//    Deactivate();
			//    SetState();
			//}));
			SetState();
		}
		void SetState() {
			if (header.IsChecked.Value) {
				header.IsEnabled = false;
				refreshPanel.IsEnabled = true;
				refreshPanel.Visibility = System.Windows.Visibility.Visible;

				content.Visibility = System.Windows.Visibility.Visible;
				if(Activate != null)
					disposables.Add(Activate());
			} else {
				refreshPanel.IsEnabled = false;
				refreshPanel.Visibility = System.Windows.Visibility.Collapsed;
				header.IsEnabled = true;
				content.Visibility = System.Windows.Visibility.Collapsed;
				//Deactivate();
			}
		}
		//public void Activate() {
		//    header.IsChecked = true;
		//    header.IsEnabled = !header.IsChecked.Value;
		//}
		public Func<IDisposable> Activate { get; protected set; }
		public Action Deactivate { get; protected set; }
		public void Hide() {
			header.IsChecked = false;
			header.IsEnabled = !header.IsChecked.Value;

			SetState();
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}

	public class SourceSectionPanel : SectionPanel, IDisposable {
		#region Loader
		public static FSharpAsync<SourcesArgs> Load(INvtSession nvtSession, OdmSession odmSession, Capabilities capabilities) {
			SourcesArgs args = new SourcesArgs();
			args.odmSession = odmSession;
			args.capabilities = capabilities;
			args.nvtSession = nvtSession;

			return Apm.Iterate(LoadImpl(args)).Select(f => { return args; });
		}
		static IEnumerable<FSharpAsync<Unit>> LoadImpl(SourcesArgs args) {
			yield return args.odmSession.GetChannelDescriptions().Select(f => { args.channels = f; return (Unit)null; });
		}
		#endregion Loader
		public SourceSectionPanel(DeviceViewArgs args, IUnityContainer container) {
			eventAggregator = container.Resolve<IEventAggregator>();
			disposables.Add(Observable.FromEventPattern(btnRefresh, "Click").Subscribe(ev => {
				eventAggregator.GetEvent<ReleaseUI>().Publish(false);
				loadingDisp.Dispose();
				loadingDisp = new CompositeDisposable();

				CreateView(args, container);
			}));
		}
		IUnityContainer container;
		IEventAggregator eventAggregator;
		CompositeDisposable loadingDisp = new CompositeDisposable();

		public void CreateView(DeviceViewArgs args, IUnityContainer container) {
			this.container = container;
			ProgressView progress = new ProgressView("Loading ..");
			if (this.content.Content is IDisposable) {
				var dis = this.content.Content as IDisposable;
				dis.Dispose();
			}
			this.content.Content = progress;

			disposables.Add(Load(args.nvtSession, args.odmSession, args.capabilities)
			.ObserveOnCurrentDispatcher()
			.Subscribe(channelsArgs => {
				SectionSources sourcesView = new SectionSources(container);
				disposables.Add(sourcesView);
				sourcesView.Init(channelsArgs);
				this.content.Content = sourcesView;
			}, err => {
				ErrorView errorView = new ErrorView(err);
				disposables.Add(errorView);

				this.content.Content = errorView;
			}));
		}

		void IDisposable.Dispose() {
			loadingDisp.Dispose();
		}
	}

	public class AnalyticsSectionPanel : SectionPanel, IDisposable {
		#region Loader
		public static FSharpAsync<AnalyticsArgs> Load(INvtSession nvtSession, OdmSession odmSession, Capabilities capabilities) {
			AnalyticsArgs args = new AnalyticsArgs();
			args.odmSession = odmSession;
			args.capabilities = capabilities;
			args.nvtSession = nvtSession;

			return Apm.Iterate(LoadImpl(args)).Select(f => { return args; });
		}
		static IEnumerable<FSharpAsync<Unit>> LoadImpl(AnalyticsArgs args) {
			yield return args.nvtSession.GetAnalyticsEngines().Select(engines => { args.Engines = engines; return (Unit)null; });
		}
		#endregion Loader

		IUnityContainer container;
		IEventAggregator eventAggregator;
		CompositeDisposable loadingDisp = new CompositeDisposable();

		public AnalyticsSectionPanel(DeviceViewArgs args, IUnityContainer container) {
			eventAggregator = container.Resolve<IEventAggregator>();
			disposables.Add(Observable.FromEventPattern(btnRefresh, "Click").Subscribe(ev => {
				eventAggregator.GetEvent<ReleaseUI>().Publish(false);

				loadingDisp.Dispose();
				loadingDisp = new CompositeDisposable();

				CreateView(args, container);
			}));
		}

		public void CreateView(DeviceViewArgs args, IUnityContainer container) {
			this.container = container;
			ProgressView progress = new ProgressView("Loading ..");
			if (this.content.Content is IDisposable) {
				var dis = this.content.Content as IDisposable;
				dis.Dispose();
			}
			this.content.Content = progress;

			loadingDisp.Add(Load(args.nvtSession, args.odmSession, args.capabilities)
				.ObserveOnCurrentDispatcher()
				.Subscribe(analyticsArgs => {
					SectionAnalytics analyticsView = new SectionAnalytics(container);
					disposables.Add(analyticsView);
					analyticsView.Init(analyticsArgs);
					this.content.Content = analyticsView;
				}, err => {
					ErrorView errorView = new ErrorView(err);
					disposables.Add(errorView);

					this.content.Content = errorView;
				}
			));
		}

		void IDisposable.Dispose() {
			loadingDisp.Dispose();
		}
	}
}
