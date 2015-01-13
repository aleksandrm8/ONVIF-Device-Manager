using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;
using odm.controllers;
using odm.extensibility;
using utils;

namespace odm.ui {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		public App() {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
		}

        [HandleProcessCorruptedStateExceptionsAttribute]
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.WriteError("Unhandled exception has occurred: " + e.ExceptionObject);
        }


		public void ChangeTheme(string _theme) {
			
		}

		[ImportMany(typeof(IPlugin))]
		public IEnumerable<IPlugin> plugins;

		void LoadPlugins() {
			try{
				var pluginsPath = Utils.MapPath("~/plugins");
				if (!Directory.Exists(pluginsPath)) {
					plugins = Enumerable.Empty<IPlugin>();
					return;
				}
				var catalog = new DirectoryCatalog(pluginsPath);
				var container = new CompositionContainer(catalog);
				container.SatisfyImportsOnce(this);
			} catch (Exception err) {
				//swallow error
				log.WriteError(String.Format("error during loading plugins: {0}", err.Message));
				dbg.Break();
				plugins = Enumerable.Empty<IPlugin>();
			}
			foreach (var p in plugins) {
				try {
					p.Init();
				} catch (Exception err) {
					//swallow error
					log.WriteError(String.Format("error during plugin initialization: {0}", err.Message));
					dbg.Break();
				}
			}
		}

		protected override void OnStartup(StartupEventArgs e) {
			//some devices don't understand http header "Expect: 100-Continue"
			ServicePointManager.Expect100Continue = false;
			//accept any certificate for tls connections
			ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, policyErrors) => {
				return true;
			};

            LoadCurrentLanguage();

			base.OnStartup(e);
			OdmBootstrapper bootstrapper = new OdmBootstrapper();
			LoadPlugins();
            bootstrapper.Run();
		}

        void LoadCurrentLanguage() {
            try {
                odm.localization.Language.Current = (from lang in odm.localization.Language.AvailableLanguages
                                                     where lang.iso3 == odm.ui.Properties.Settings.Default.DefaultLocaleIso3
                                                     select lang).FirstOrDefault() ?? odm.localization.Language.Default;
            }
            catch (Exception err) {
                log.WriteError(err);
            }
        }
	}
}
