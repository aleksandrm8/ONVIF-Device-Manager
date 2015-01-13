using System;
using System.Text;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using odm.infra;
using odm.ui.views;
using utils;

namespace odm.ui.activities
{
    public partial class MaintenanceView : UserControl, IDisposable
    {

        #region Activity definition
        public static FSharpAsync<Result> Show(IUnityContainer container, Model model)
        {
            return container.StartViewActivity<Result>(context =>
            {
                var view = new MaintenanceView(model, context);
                var presenter = container.Resolve<IViewPresenter>();
                presenter.ShowView(view);
            });
        }
        #endregion

        protected LocalMaintenance Strings { get { return LocalMaintenance.instance; } }

        private void Init(Model model)
        {
            var maintenanceLinkEventArgs = activityContext.container.Resolve<IMaintenanceLinkEventArgs>();
            BackupCommand = new DelegateCommand(
                () =>
                {
                    var dlg = new SaveFileDialog();

                    dlg.Filter = "Backup files (*.backup)|*.backup|Zip archives (*.zip)|*.zip";
                    dlg.Title = Strings.savebuckup;
                    dlg.FileName = CreateBackupFileName(maintenanceLinkEventArgs.Manufacturer, maintenanceLinkEventArgs.DeviceModel, model.firmwareVersion);
                    dlg.AddExtension = true;
                    //dlg.InitialDirectory = Directory.GetCurrentDirectory();
                    if (dlg.ShowDialog() == true)
                    {
                        Success(new Result.Backup(dlg.FileName));
                    }
                },
                () => model != null
                    && model.capabilities != null
                    && model.capabilities.device != null
                    && model.capabilities.device.system != null
                    && model.capabilities.device.system.systemBackup
            );

            RestoreCommand = new DelegateCommand(
                () =>
                {
                    var dlg = new OpenFileDialog();
                    dlg.Filter = "Backup files (*.backup)|*.backup|Zip archives (*.zip)|*.zip";
                    dlg.Title = Strings.loadbuckup;
                    //dlg.InitialDirectory = Directory.GetCurrentDirectory();
                    if (dlg.ShowDialog() == true)
                    {
                        Success(new Result.Restore(dlg.FileName));
                    }

                },
                () => model != null
                    && model.capabilities != null
                    && model.capabilities.device != null
                    && model.capabilities.device.system != null
                    && model.capabilities.device.system.systemBackup
            );

            SoftResetCommand = new DelegateCommand(
                () => Success(new Result.SoftReset()),
                () => true
            );

            HardResetCommand = new DelegateCommand(
                () => Success(new Result.HardReset()),
                () => true
            );

            RebootCommand = new DelegateCommand(
                () => Success(new Result.Reboot()),
                () => true
            );

            var hasSystemCapabilities =
                model.capabilities != null &&
                model.capabilities.device != null &&
                model.capabilities.device.system != null;

            var systemCapabilities = hasSystemCapabilities ? model.capabilities.device.system : null;

            UpgradeCommand = new DelegateCommand(
                () =>
                {
                    var dlg = new OpenFileDialog();
                    dlg.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                    dlg.Title = Strings.selectfirmware;
                    //dlg.InitialDirectory = Directory.GetCurrentDirectory();
                    if (dlg.ShowDialog() == true)
                    {
                        Success(new Result.Upgrade(dlg.FileName));
                    }
                },
                () => model != null && model.isFirmwareUpgradeSupported
            );

            InitializeComponent();

            BindModel(model);
            Localization();

        }

        private static string CreateBackupFileName(string manufacturer, string deviceModel, string firmwareVersion)
        {
            StringBuilder name = new StringBuilder();
            if (!string.IsNullOrEmpty(manufacturer))
                name.Append(manufacturer.Replace('.', '-') + "-");
            if (!string.IsNullOrEmpty(deviceModel))
                name.Append(deviceModel.Replace('.', '-') + "-");
            if (!string.IsNullOrEmpty(firmwareVersion))
                name.Append(firmwareVersion.Replace('.', '-') + "-");
            name.Append("backup");

            return name.ToString();
        }

        #region Binding
        void Localization()
        {
            configurationCaption.CreateBinding(Label.ContentProperty, Strings, s => s.configurationCaption);
            diagnosticsCaption.CreateBinding(Label.ContentProperty, Strings, s => s.diagnosticsCaption);
            hardResetCaption.CreateBinding(Label.ContentProperty, Strings, s => s.hardResetCaption);
            resetCaption.CreateBinding(Label.ContentProperty, Strings, s => s.resetCaption);
            softResetCaption.CreateBinding(Label.ContentProperty, Strings, s => s.softResetCaption);
            upgradeCaption.CreateBinding(Label.ContentProperty, Strings, s => s.upgradeCaption);
            upgradeUnsupportedTxt.CreateBinding(TextBlock.TextProperty, Strings, s => s.unsupported);

            backupBtn.CreateBinding(Button.ContentProperty, Strings, x => x.btnBackup);
            restoreBtn.CreateBinding(Button.ContentProperty, Strings, x => x.btnRestore);
            softResetBtn.CreateBinding(Button.ContentProperty, Strings, x => x.btnSoftReset);
            hardResetBtn.CreateBinding(Button.ContentProperty, Strings, x => x.btnHardReset);

            softResetBtn.CreateBinding(Button.ToolTipProperty, Strings, x => x.btnSoftResetTooltip);
            hardResetBtn.CreateBinding(Button.ToolTipProperty, Strings, x => x.btnHardResetTooltip);

            rebootBtn.CreateBinding(Button.ContentProperty, Strings, x => x.btnReboot);
            upgradeBtn.CreateBinding(Button.ContentProperty, Strings, x => x.btnUpgrate);
        }
        void BindModel(Model model)
        {
            backupBtn.Command = BackupCommand;
            restoreBtn.Command = RestoreCommand;

            backupUnsupportedTxt.Visibility =
                model.isFirmwareUpgradeSupported ?
                System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

            softResetBtn.Command = SoftResetCommand;
            hardResetBtn.Command = HardResetCommand;
            rebootBtn.Command = RebootCommand;
            upgradeBtn.Command = UpgradeCommand;

            upgradeUnsupportedTxt.Visibility =
                model.isFirmwareUpgradeSupported ?
                System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

            fimwareTxt.Text = model.firmwareVersion;
        }
        #endregion Binding

        public void Dispose()
        {
            Cancel();
        }

    }
}
