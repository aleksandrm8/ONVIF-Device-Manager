using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views;
using utils;

namespace odm.ui.viewModels {
    public class ToolBarViewModel: DependencyObject {
        public ToolBarViewModel(IEventAggregator eventAggregator) {
            this.eventAggregator = eventAggregator;
            LocalesCollection = new ObservableCollection<odm.localization.Language>();
            Accounts = new ObservableCollection<Account>();

            eventAggregator.GetEvent<InitAccounts>().Subscribe(ret => {
                InitAccounts();
            });

            InitCommands();
        }
        
        public DeviceControlStrings Strings { get { return DeviceControlStrings.instance; } }
        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }

        public void Init() {
            InitAccounts();
            InitLocalization();
        }

        Account anonymous;
        void InitAccounts() {
            IsNotApply = true;
            Accounts.Clear();

            anonymous = new Account();
            anonymous.Name = "<Anonymous>";
            Accounts.Add(anonymous);
            AccountManager.Load().ForEach(x => Accounts.Add(x));

            try {
                var curaccount = AccountManager.CurrentAccount;
                if (curaccount == null)
                    CurrentAccount = anonymous;
                else {
                    Accounts.ForEach(x => {
                        if (x.Name == curaccount.Name && x.Password == curaccount.Password)
                            CurrentAccount = x;
                    });
                }
            } catch (Exception err) {
                dbg.Error(err);
            }
        }
        bool IsNotApply = true;
        void OnAccountApply() {
            if (CurrentAccount == anonymous) {
                AccountManager.SetCurrent(anonymous, false);
            } else {
                AccountManager.SetCurrent(CurrentAccount, true);
            }

            eventAggregator.GetEvent<Refresh>().Publish(true);
        }

        IEventAggregator eventAggregator;
        public ObservableCollection<odm.localization.Language> LocalesCollection { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }

        void InitLocalization() {
            IEnumerable<odm.localization.Language> langs = odm.localization.Language.AvailableLanguages;
            odm.ui.controls.ListItem<odm.localization.Language>[] list = langs.Select(x => odm.ui.controls.ListItem.Wrap(x, y => y.DisplayName)).Where(u => u.Unwrap().iso3 != null).ToArray();

            var defItem = list.Where(x => x.Unwrap().iso3 == odm.ui.Properties.Settings.Default.DefaultLocaleIso3).FirstOrDefault();

            list.ForEach(x => LocalesCollection.Add(x.Unwrap()));

            if (defItem == null) {
                defItem = odm.ui.controls.ListItem.Wrap(odm.localization.Language.Default, x => "english");
                LocalesCollection.Add(defItem.Unwrap());
            }
            SelectedLocale = defItem.Unwrap();
        }

        #region Commands
        void InitCommands() {
            OnAboutClick = new DelegateCommand(() => {
                var evarg = new DeviceLinkEventArgs();
                evarg.currentAccount = AccountManager.CurrentAccount;
                evarg.session = null;
                eventAggregator.GetEvent<AboutClick>().Publish(evarg);
            });
            AccountClick = new DelegateCommand(() => {
                var evarg = new DeviceLinkEventArgs();
                evarg.currentAccount = AccountManager.CurrentAccount;
                evarg.session = null;
                eventAggregator.GetEvent<AccountManagerClick>().Publish(evarg);
            });
            ApplyClick = new DelegateCommand(() => {
                OnAccountApply();
            });
            PropertiesClick = new DelegateCommand(() => {
                eventAggregator.GetEvent<PropertiesClick>().Publish(true);
            });
        }

        public DelegateCommand PropertiesClick {
            get { return (DelegateCommand)GetValue(PropertiesClickProperty); }
            set { SetValue(PropertiesClickProperty, value); }
        }
        public static readonly DependencyProperty PropertiesClickProperty =
            DependencyProperty.Register("PropertiesClick", typeof(DelegateCommand), typeof(ToolBarViewModel));
        
        public DelegateCommand OnAboutClick {
            get { return (DelegateCommand)GetValue(OnAboutClickProperty); }
            set { SetValue(OnAboutClickProperty, value); }
        }
        public static readonly DependencyProperty OnAboutClickProperty =
            DependencyProperty.Register("OnAboutClick", typeof(DelegateCommand), typeof(ToolBarViewModel));

        public DelegateCommand ApplyClick {
            get { return (DelegateCommand)GetValue(ApplyClickProperty); }
            set { SetValue(ApplyClickProperty, value); }
        }
        public static readonly DependencyProperty ApplyClickProperty =
            DependencyProperty.Register("ApplyClick", typeof(DelegateCommand), typeof(ToolBarViewModel));

        public DelegateCommand AccountClick {
            get { return (DelegateCommand)GetValue(AccountClickProperty); }
            set { SetValue(AccountClickProperty, value); }
        }
        public static readonly DependencyProperty AccountClickProperty =
            DependencyProperty.Register("AccountClick", typeof(DelegateCommand), typeof(ToolBarViewModel));

        #endregion



        public Account CurrentAccount {
            get { return (Account)GetValue(CurrentAccountProperty); }
            set { SetValue(CurrentAccountProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CurrentAccount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentAccountProperty =
            DependencyProperty.Register("CurrentAccount", typeof(Account), typeof(ToolBarViewModel), new PropertyMetadata(null, (obj, evarg) => {
                var vm = ((ToolBarViewModel)obj);
                if (!vm.IsNotApply)
                    vm.OnAccountApply();
                vm.IsNotApply = false;
            }));

        

        //void Localization() {
        //    var ver = Assembly.GetExecutingAssembly().GetName().Version;
        //    this.Title = String.Format("{0} v{1}.{2}.{3}", CommonApplicationStrings.Instance.applicationName, ver.Major, ver.Minor, ver.Build);
        //}
        public odm.localization.Language SelectedLocale {
            get { return (odm.localization.Language)GetValue(SelectedLocaleProperty); }
            set { SetValue(SelectedLocaleProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SelectedLocale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLocaleProperty =
            DependencyProperty.Register("SelectedLocale", typeof(odm.localization.Language), typeof(ToolBarViewModel), new PropertyMetadata(null,(obj, ev)=>{
                ToolBarViewModel tb = (ToolBarViewModel)obj;
                odm.localization.Language selection = (odm.localization.Language)ev.NewValue;
                if (selection == null) {
                    odm.localization.Language.Current = null;
                } else {
                    odm.localization.Language.Current = selection;
                    odm.ui.Properties.Settings.Default.DefaultLocaleIso3 = odm.localization.Language.Current.iso3;
                    odm.ui.Properties.Settings.Default.Save();
                }
                //        Localization();
            }));

    }
}
