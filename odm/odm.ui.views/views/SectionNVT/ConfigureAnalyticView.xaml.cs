using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.core;
using odm.infra;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views.CommonAnalytics;
using odm.ui.views.CustomAnalytics;
using odm.ui.views.CustomAppro;
using utils;
using onvif.services;
using System.Collections.Generic;


namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for ConfigureAnalyticModuleView.xaml
	/// </summary>
	public partial class ConfigureAnalyticView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ConfigureAnalyticView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private CompositeDisposable disposables = new CompositeDisposable();
		//XmlParserFactory xparser;

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }

		public class ModuleDescriptor {
			public ModuleDescriptor(global::onvif.services.Config config,
				 global::onvif.services.ConfigDescription configDescription,
				 global::System.Xml.Schema.XmlSchemaSet schema) {
				this.schema = schema;
				this.config = config;
				this.configDescription = configDescription;

			}
			public global::onvif.services.Config config { get; set; }
			public global::onvif.services.ConfigDescription configDescription { get; set; }
			public global::System.Xml.Schema.XmlSchemaSet schema { get; set; }
		}

		public class AnalyticsVideoDescriptor {
			public IVideoInfo videoInfo { get; set; }
			public Size videoSourceResolution { get; set; }
			//public string profileToken { get; set; }
		}
		void StartConfiguring(UIElement conf) {
			content.Children.Clear();
			content.Children.Add(conf);
		}
		void RunCommonAnalytics(Model model) {
			CommonAnalytics customview = new CommonAnalytics();
			customview.Init(new ModuleDescriptor(model.config, model.configDescription, model.schemes));
			StartConfiguring(customview);
		}
		void RunIncotexAnalytics(StreamInfoArgs args, Model model) {
			AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
			ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

			ApproAnalyticsConfigView customview = new ApproAnalyticsConfigView();
			bool retres = customview.Init(activityContext.container, args, moduleDescriptor);
			if (!retres) {
				Error(new Exception("Module is in unproper state. Please delete and create new one."));
			}
			Apply = customview.Apply;

			StartConfiguring(customview);
			controlDisposable = customview;
		}
		void RunMagicBoxTripWireRule(StreamInfoArgs args, Model model) {
			AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
			ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

			SynesisTripWireRuleView customview = new SynesisTripWireRuleView();
			bool retres = customview.Init(activityContext.container, args, moduleDescriptor);
			if (!retres) {
				Error(new Exception("Module is in unproper state. Please delete and create new one."));
			}
			Apply = customview.Apply;

			StartConfiguring(customview);
			controlDisposable = customview;
		}
		void RunMagicBoxRegionRule(StreamInfoArgs args, Model model) {
			AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
			ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

			SynesisRegionRuleView customview = new SynesisRegionRuleView();
			bool retres = customview.Init(activityContext.container, args, moduleDescriptor);
			if (!retres) {
				Error(new Exception("Module is in unproper state. Please delete and create new one."));
			}
			Apply = customview.Apply;

			StartConfiguring(customview);
			controlDisposable = customview;
		}
		void RunMagicBoxAnnotation(StreamInfoArgs args, Model model) {
			AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
			ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

			SynesisAnnotationView customview = new SynesisAnnotationView();
			bool retres = customview.Init(activityContext.container, args, moduleDescriptor);
			if (!retres) {
				Error(new Exception("Module is in unproper state. Please delete and create new one."));
			}
			Apply = customview.Apply;

			StartConfiguring(customview);
			controlDisposable = customview;
		}
		void RunMagicBoxAnalytics(StreamInfoArgs args, Model model) {
			//var isession = activityContext.container.Resolve<INvtSession>();
			AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
			ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

			SynesisAnalyticsConfigView customview = new SynesisAnalyticsConfigView();
			bool retres = customview.Init(activityContext.container, args, moduleDescriptor);
			if (!retres) {
				Error(new Exception("Module is in unproper state. Please delete and create new one."));
			}
			Apply = customview.Apply;

			StartConfiguring(customview);
			controlDisposable = customview;
		}
		void BindModel(Model model) {
			//"http://www.synesis.ru/onvif/magicbox_video_analytics"
			XmlQualifiedName xMagicboxAnalytics = new XmlQualifiedName("AnalyticsModule", "http://www.synesis.ru/onvif/VideoAnalytics");
			XmlQualifiedName xKipodAnalytics = new XmlQualifiedName("Surveillance", "http://www.synesis.ru/onvif/VideoAnalytics");
			XmlQualifiedName xMagicboxAnnotation = new XmlQualifiedName("AnnotationModule", "http://www.synesis.ru/onvif/VideoAnalytics");
			XmlQualifiedName xMagicboxRegionRule = new XmlQualifiedName("RegionRule", "http://www.synesis.ru/onvif/VideoAnalytics");
			XmlQualifiedName xMagicboxWireRule = new XmlQualifiedName("TripWireRule", "http://www.synesis.ru/onvif/VideoAnalytics");

			XmlQualifiedName xIncotexApproAnalytics = new XmlQualifiedName("ApproMotionDetector", "http://www.incotex.ru/onvif/ApproMotionDetector");
			XmlQualifiedName xSynesisApproAnalytics = new XmlQualifiedName("ApproMotionDetector", "http://www.synesis.ru/onvif/ApproMotionDetector");

			try {
				if (!AppDefaults.visualSettings.CustomAnalytics_IsEnabled) {
					RunCommonAnalytics(model);
					return;
				}

				Dictionary<XmlQualifiedName, Action<StreamInfoArgs, Model>> AnalyticsFactory = new Dictionary<XmlQualifiedName, Action<StreamInfoArgs, Model>>();
				AnalyticsFactory.Add(xMagicboxAnalytics, RunMagicBoxAnalytics);
				AnalyticsFactory.Add(xKipodAnalytics, RunMagicBoxAnalytics);
				AnalyticsFactory.Add(xMagicboxAnnotation, RunMagicBoxAnnotation);
				AnalyticsFactory.Add(xMagicboxRegionRule, RunMagicBoxRegionRule);
				AnalyticsFactory.Add(xMagicboxWireRule, RunMagicBoxTripWireRule);
				AnalyticsFactory.Add(xIncotexApproAnalytics, RunIncotexAnalytics);
				AnalyticsFactory.Add(xSynesisApproAnalytics, RunIncotexAnalytics);


				var getsreamInfo = activityContext.container.Resolve<IStreamInfoHelper>();

				StartConfiguring(new ProgressView("Loading"));

				disposables.Add(getsreamInfo.GetFunction()()
					.ObserveOnCurrentDispatcher()
					.Subscribe(unit => {
						var infoArgs = getsreamInfo.GetInfoArgs();

						if (AnalyticsFactory.ContainsKey(model.configDescription.name)) {
							AnalyticsFactory[model.configDescription.name](infoArgs, model);
						} else {
							RunCommonAnalytics(model);
						}
					}, err => {
						Error(err);
					}));

			} catch (Exception err) {
				Error(err);
			}

		}

		public Action Apply;
		IDisposable controlDisposable;
		//TODO: Stub fix for #225 Remove this with plugin functionality
		ILastChangedModule last;
		//

		void ReleaseAll() {
			if (controlDisposable != null)
				controlDisposable.Dispose();
		}

		private void Init(Model model) {

			OnCompleted += () => {
				disposables.Dispose();
				ReleaseAll();
			};

			this.DataContext = model;

			InitializeComponent();
			BindModel(model);

			var abortCommand = new DelegateCommand(
				 () => {
					 //TODO: Stub fix for #225 Remove this with plugin functionality
					 last = activityContext.container.Resolve<ILastChangedModule>();
					 last.module = null;
					 //
					 Success(new Result.Abort());
				 },
				 () => true
			);
			AbortCommand = abortCommand;

			var applyCommand = new DelegateCommand(
				 () => {
					 if (Apply != null)
						 Apply();
					 Success(new Result.Apply(model));
				 },
				 () => true
			);
			ApplyCommand = applyCommand;

			btnApply.Command = ApplyCommand;
			btnAbort.Command = AbortCommand;
		}

		public void Dispose() {
			Cancel();
		}
	}
}
