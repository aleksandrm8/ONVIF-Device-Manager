using System;
using System.Windows;
using onvif.utils;
using utils;

namespace odm.ui.dialogs {
    public class UpgradeViewModel :DependencyObject ,IDisposable {
        public UpgradeViewModel() {
        }
        public void Init(OdmSession facade, string path) {
            Binding();

            subscriptions = facade.UpgradeFirmware(path)
                .ObserveOnCurrentDispatcher()
                .Subscribe(message => {
                    IsProgressVisible = Visibility.Hidden;
                    Message = "Upgrade completed successfully.";
                    this.CreateBinding(ButtonNameProperty, Buttons, x => x.close);
                }, err => {
					dbg.Error(err);
					IsProgressVisible = Visibility.Hidden;
                    Message = err.Message;
                    this.CreateBinding(ButtonNameProperty, Buttons, x => x.close);
                });
        }
        IDisposable subscriptions;
        public LocalButtons Buttons { get { return LocalButtons.instance; } }
        public LocalMaintenance Strings { get { return LocalMaintenance.instance; } }

        void Binding() {
            this.CreateBinding(MessageProperty, Strings, x => x.uploadingFirmware);
            this.CreateBinding(ButtonNameProperty, Buttons, x => x.cancel);
        }

        public void Dispose() {
            subscriptions.Dispose();
        }

        public Visibility IsProgressVisible {
            get { return (Visibility)GetValue(IsProgressVisibleProperty); }
            set { SetValue(IsProgressVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsProgressVisible. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsProgressVisibleProperty =
            DependencyProperty.Register("IsProgressVisible", typeof(Visibility), typeof(UpgradeViewModel), new UIPropertyMetadata(Visibility.Visible));

        public string ButtonName {
            get { return (string)GetValue(ButtonNameProperty); }
            set { SetValue(ButtonNameProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ButtonName. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonNameProperty =
            DependencyProperty.Register("ButtonName", typeof(string), typeof(UpgradeViewModel));

        public string Message {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Message. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(UpgradeViewModel));
    }
}
