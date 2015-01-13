using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using plugin_manager;
using utils;

namespace odm.ui {
	class OdmBootstrapper : UnityBootstrapper {
		protected override DependencyObject CreateShell() {
			MainWindow mainWnd = new MainWindow();
			//try {
			//    mainWnd = this.Container.Resolve<MainWindow>();
			//} catch (Exception err) {
			//    dbg.Error(err);
			//}
			mainWnd.Init(new MainWindowViewModel());
			return mainWnd;
		}

		protected override void ConfigureContainer() {
			base.ConfigureContainer();
			this.Container.RegisterInstance<IVideoSettingsView>(new DefaultVideoSettingsView());
		}
		protected override void InitializeShell() {
			//base.InitializeShell();
			var wnd = (MainWindow)this.Shell;
			App.Current.MainWindow = wnd;
			App.Current.MainWindow.Show();
		}
		protected override IModuleCatalog CreateModuleCatalog() {
			//return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(new Uri());
			return base.CreateModuleCatalog();
		}
		protected override void ConfigureModuleCatalog() {
			base.ConfigureModuleCatalog();
			var moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
			moduleCatalog.AddModule(typeof(MainModule));
		}


	}
}
