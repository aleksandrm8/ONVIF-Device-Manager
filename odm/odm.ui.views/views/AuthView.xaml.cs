using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using utils;
using odm.ui.core;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using odm.ui.controls;

namespace odm.ui.views
{
    /// <summary>
    /// Interaction logic for AuthView.xaml
    /// </summary>
    public partial class AuthView : UserControl
    {

        IEventAggregator eventAggregator;

        public AuthView(IUnityContainer container)
        {
            eventAggregator = container.Resolve<IEventAggregator>();
            
            InitializeComponent();

            Init();
        }

        #region Dependency Properties

        

        public bool Authorized
        {
            get { return (bool)GetValue(AuthorizedProperty); }
            set { SetValue(AuthorizedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Authorized.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthorizedProperty =
            DependencyProperty.Register("Authorized", typeof(bool), typeof(AuthView), new PropertyMetadata(false, (s,e) => 
                {
                    var auth = (AuthView)s;
                    if (true.Equals(e.NewValue))
                    {
                        auth.panelEdit.Visibility = Visibility.Collapsed;
                        auth.panelView.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        auth.panelEdit.Visibility = Visibility.Visible;
                        auth.panelView.Visibility = Visibility.Collapsed;
                    }
                }));


        #endregion Dependency Properties

        void Init()
        {
            btLogin.Command = new DelegateCommand(new Action(btLogin_Click));
            btLogout.Command = new DelegateCommand(new Action(btLogout_Click));
            username.KeyDown += (s, e) => { if (e.Key == Key.Enter) btLogin_Click(); };
            password.KeyDown += (s, e) => { if (e.Key == Key.Enter) btLogin_Click(); };
            this.Loaded += AuthView_Loaded;

            AccountManager.Instance.CurrentAccountChanged += delegate { Update(); };
        }

        void Update()
        {
            Authorized = AccountManager.Instance.Autorized;
            var account = AccountManager.Instance.CurrentAccount;
            username.Text = account.Name;
            password.Password = account.Password;
            loggedUsername.Text = account.Name;
        }

        void AuthView_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        void btLogin_Click()
        {
            try
            {
                AccountManager.Instance.SetCurrentAccount(new Account() { Name=username.Text, Password=password.Password }, remember.IsChecked == true);
                eventAggregator.GetEvent<Refresh>().Publish(true);
            }
            catch (Exception err)
            {
                dbg.Error(err);
            }
        }

        void btLogout_Click()
        {
            try
            {
                var last = AccountManager.Instance.CurrentAccount;
                AccountManager.Instance.SetCurrentAccount(Account.Anonymous, true);
                //username.Text = last.Name;
                //password.Password = last.Password;

                eventAggregator.GetEvent<Refresh>().Publish(true);
            }
            catch (Exception err)
            {
                dbg.Error(err);
            }
        }
    }
}
