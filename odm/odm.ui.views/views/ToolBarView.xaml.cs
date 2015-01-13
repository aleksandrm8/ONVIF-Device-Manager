using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using odm.ui.activities;
using odm.ui.core;
using odm.ui.views;
using utils;

namespace odm.ui {
	/// <summary>
	/// Interaction logic for ToolBarView.xaml
	/// </summary>
	public partial class ToolBarView : UserControl {
		public static readonly RoutedCommand AppSettingsCommand = new RoutedCommand("AppSettings", typeof(ToolBarView));
		public static readonly RoutedCommand SecuritySettingsCommand = new RoutedCommand("SecuritySettings", typeof(ToolBarView));
		public static readonly RoutedCommand AboutCommand = new RoutedCommand("About", typeof(ToolBarView));
		public static readonly RoutedCommand BackgroundTasksCommand = new RoutedCommand("BackgroundTasks", typeof(ToolBarView));
		public static readonly RoutedCommand AccountSettingsCommand = new RoutedCommand("AccountSettings", typeof(ToolBarView));
		public ToolBarView(IUnityContainer container) {

			eventAggregator = container.Resolve<IEventAggregator>();

			this.CommandBindings.Add(
				new CommandBinding(
					ToolBarView.AppSettingsCommand,
					(s, a) => {
						var evarg = new DeviceLinkEventArgs();
						evarg.currentAccount = AccountManager.Instance.CurrentAccount;
						evarg.session = null;
						eventAggregator.GetEvent<AppSettingsClick>().Publish(true);
					}
				)
			);
            this.CommandBindings.Add(
                new CommandBinding(
                    ToolBarView.AccountSettingsCommand,
                    (s, a) => {
                        var evarg = new DeviceLinkEventArgs();
                        evarg.currentAccount = AccountManager.Instance.CurrentAccount;
                        evarg.session = null;
                        eventAggregator.GetEvent<AccountManagerClick>().Publish(evarg);
                    }
                )
            );
            
			this.CommandBindings.Add(
				new CommandBinding(
					ToolBarView.BackgroundTasksCommand,
					(s, a) => {
                        var evarg = new DeviceLinkEventArgs();
                        evarg.currentAccount = AccountManager.Instance.CurrentAccount;
                        evarg.session = null;
                        eventAggregator.GetEvent<BackgroundTasksClick>().Publish(true);
					}
				)
			);

			this.CommandBindings.Add(
				 new CommandBinding(
					  ToolBarView.AboutCommand,
					  (s, a) => {
						  var evarg = new DeviceLinkEventArgs();
						  evarg.currentAccount = AccountManager.Instance.CurrentAccount;
						  evarg.session = null;
						  eventAggregator.GetEvent<AboutClick>().Publish(evarg);
					  }
				 )
			);
            
            
			InitializeComponent();
            auth.Content = new AuthView(container); // TODO how to pass the container?
		}
        void InitLocalization() {
            IEnumerable<odm.localization.Language> langs = odm.localization.Language.AvailableLanguages;
            odm.ui.controls.ListItem<odm.localization.Language>[] list = langs.Select(x => odm.ui.controls.ListItem.Wrap(x, y => y.DisplayName)).Where(u => u.Unwrap().iso3 != null).ToArray();

            var defItem = list.Where(x => x.Unwrap().iso3 == odm.ui.Properties.Settings.Default.DefaultLocaleIso3).FirstOrDefault();

            if (defItem == null) {
                defItem = odm.ui.controls.ListItem.Wrap(odm.localization.Language.Default, x => "english");
            }

            odm.localization.Language.Current = defItem.Unwrap();
        }
        
		public LocalDevice Strings { get { return LocalDevice.instance; } }
		public LocalTitles Titles { get { return LocalTitles.instance; } }
		Account anonymous;
		bool isNotApply;
		bool IsNotApply {
			get { 
				return isNotApply; 
			}
			set {
				isNotApply = value;
			}
		}
		IEventAggregator eventAggregator;
		
		
		/*void OnAccountApply() {
			if (CurrentAccount == anonymous) {
				AccountManager.SetCurrent(new DefAccountDescriptor());
			} else {
				AccountManager.SetCurrent(CurrentAccount);
			}
			//AccountManager.SetCurrent(CurrentAccount);
			eventAggregator.GetEvent<Refresh>().Publish(true);
		}*/

		#region Commands
		void InitCommands() {
			/*ApplyClick = new DelegateCommand(() => {
				OnAccountApply();
			});*/
		}

		
		public static readonly DependencyProperty ApplyClickProperty =
			 DependencyProperty.Register("ApplyClick", typeof(DelegateCommand), typeof(ToolBarView));

		#endregion
	}
}
