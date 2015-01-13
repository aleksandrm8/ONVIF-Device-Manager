using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using odm.controllers;
using odm.ui.activities;
using odm.ui.controls;
using odm.ui.views;
//using incotext;


namespace odm.ui {
	class MainModule : IModule {
		private readonly IUnityContainer container;
		private readonly IRegionManager regionManager;
		private ContentController contentController;
		//[Dependency]
		//public IRegionViewRegistry regionViewRegistry{get;set;}

		public MainModule(IUnityContainer container) {
			this.container = container;
			regionManager = container.Resolve<IRegionManager>();
			container.RegisterInstance<MainModule>(this);
		}

		public void Initialize() {
			container.RegisterInstance<MainModule>(new MainModule(container));

			contentController = container.Resolve<ContentController>();

			//InitContainerFake();
			InitContainer();

			//PluginManager pmanager = new PluginManager();
            //pmanager.RegisterVideoSettingsPlugin(new IncotexPlugin());
            //container.RegisterInstance<IPluginManager>(pmanager);
			container.RegisterInstance<IVideoPlayerActivity>(VideoPlayerActivity.Create());
			
			//TODO: Synesis specific mode to be removed in plugins
			container.RegisterInstance<odm.ui.core.IConfiguratorFactory>(new odm.ui.core.ConfiguratorFactory());

			regionManager.RegisterViewWithRegion("deviceslist", typeof(DeviceListView));
			regionManager.RegisterViewWithRegion("mainframe", typeof(DeviceExplorerView));
			regionManager.RegisterViewWithRegion("toolbar", typeof(ToolBarView));
		}
		void InitContainerFake() {

		}
		void InitContainer() {
		}

		~MainModule() {
			Dispose(false);
		}
		protected virtual void Dispose(bool disposing) {
		}
	}

}
