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
using Microsoft.FSharp.Control;
using odm.core;
using onvif.utils;
using onvif.services;
using Microsoft.FSharp.Core;
using utils;
using System.Reactive.Disposables;
using odm.ui.activities;
using odm.ui.controls;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;

namespace odm.ui.views.SectionNVA {
	/// <summary>
	/// Interaction logic for SectionAnalytics.xaml
	/// </summary>
	public partial class SectionAnalytics : UserControl, IDisposable {

		public SectionAnalytics(IUnityContainer container) {
			InitializeComponent();

			this.container = container;
			eventAggregator = container.Resolve<IEventAggregator>();
		}

		CompositeDisposable disposables = new CompositeDisposable();
		//Dictionary<string, DeviceEngineControl> engineControls = new Dictionary<string, DeviceEngineControl>();
		List<KeyValuePair<string, DeviceEngineControl>> engineControls = new List<KeyValuePair<string, DeviceEngineControl>>();
		IUnityContainer container;
		IEventAggregator eventAggregator;

		public void Init(AnalyticsArgs args) {

			args.Engines.ForEach(engine => {
				LoadEngine(engine, args);
			});
		}

		void InitEngineControl(DeviceEngineControl engineControl, AnalyticsEngine engine, AnalyticsArgs args, string ctrltoken = null) {
			//try to remove and clear all needed data
			if (engineControl.Content is IDisposable) {
				var disp = engineControl.Content as IDisposable;
				//try to remove content from disposables collection
				if (disposables.Contains(disp))
					disposables.Remove(disp);
				//dispose existing control
				disp.Dispose();
			}

			//Begin load channels section
			disposables.Add(EnginesView.Load(engine, args.capabilities, args.nvtSession, args.odmSession, ctrltoken)
				.ObserveOnCurrentDispatcher()
				.Subscribe(ctrlArgs => {
					if (ctrlArgs.selectedEngineControl != null)
						engineControl.Title = ctrlArgs.engine.name + ": " + ctrlArgs.selectedEngineControl.name;
					else
						engineControl.Title = ctrlArgs.engine.name;

					EnginesView enginesView = new EnginesView(container);
					disposables.Add(enginesView);

					enginesView.Init(ctrlArgs);
					engineControl.Content = enginesView;
				}, err => {

					ErrorView errorView = new ErrorView(err);
					disposables.Add(errorView);

					engineControl.Content = errorView;
				}
			));
		}
		void ShowLoadingProgress(DeviceEngineControl engineControl, string title) {
			ProgressView progress = new ProgressView("Loading ..");
			engineControl.Content = progress;
			engineControl.Title = title;
		}
		void LoadEngine(AnalyticsEngine engine, AnalyticsArgs args) {
			try {
				//Create engine control
				DeviceEngineControl engineControl = new DeviceEngineControl();

				//add control to controls dictionary
				//if (engine.token == null) {
				//    throw new ArgumentNullException("Analytics engine token can not be null");
				//}
				engineControls.Add(new KeyValuePair<string, DeviceEngineControl>(engine.token, engineControl));

				//Display progress bar
				ShowLoadingProgress(engineControl, engine.token);

				//add control to parent UI panel
				parent.Children.Add(engineControl);

				InitEngineControl(engineControl, engine, args);

				//subscribe to control changed event
				var subsToken = eventAggregator.GetEvent<ControlChangedEvent>().Subscribe(evargs => {
					if (evargs.engine.token == engine.token) {
						//reload channel with new profile
						InitEngineControl(engineControl, engine, args, evargs.controlToken);
					}
				}, false);
				disposables.Add(Disposable.Create(() => {
					eventAggregator.GetEvent<ControlChangedEvent>().Unsubscribe(subsToken);
				}));
			} catch (Exception err) {
				ErrorView errorView = new ErrorView(err);
				disposables.Add(errorView);
				parent.Children.Add(errorView); 
			}
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}

	public class AnalyticsArgs {
		public INvtSession nvtSession { get; set; }
		public OdmSession odmSession { get; set; }
		public Capabilities capabilities { get; set; }
		public AnalyticsEngine[] Engines { get; set; }
	}
}
