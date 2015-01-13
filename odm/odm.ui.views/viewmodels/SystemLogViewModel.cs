using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.core;
using odm.infra;
using odm.ui.controls;
using odm.ui.core;
using utils;
using onvif.services;
using Microsoft.Win32;
using System.IO;
using System;
using Microsoft.Practices.Prism.Events;
using odm.ui.views;

namespace odm.ui.viewModels {
    public class SystemLogViewModel:ViewModelDeviceBase {
        public SystemLogViewModel(IUnityContainer container):base(container) {
            SysTypes = new ObservableCollection<SysLogType>();
            SysTypes.Add(DefaultSysLogType);
            SysTypes.Add(new SysLogType(SystemLogType.access));

			eventAggregator = container.Resolve<EventAggregator>();
        }
		public LocalSystemLog Strings { get { return LocalSystemLog.instance; } }
		public LocalDevice AppStrings { get { return LocalDevice.instance; } }
		public LocalButtons ButtonStrings { get { return LocalButtons.instance; } }

        public ObservableCollection<SysLogType> SysTypes { get; set; }
        public static readonly SysLogType DefaultSysLogType = new SysLogType(SystemLogType.system);
		EventAggregator eventAggregator;
		SysLogDescriptor logDescr;

		public void Init(INvtSession session, Account account, SysLogDescriptor slogdescr) {
            logDescr = slogdescr;
            base.Init(session, account);
		}
		void InitData() {
			SystemLogString = logDescr.SystemLog;
			LogInfo = logDescr.Info;

			OnAttach.RaiseCanExecuteChanged();
			OnSave.RaiseCanExecuteChanged();
		}

        public override void Load(INvtSession session, Account account) {
            CurrentSession = session;

            //show default log at startup
            subscription.Add(CurrentSession
                .GetSystemLog(SelectedType.type)
                .ObserveOnCurrentDispatcher()
                .Subscribe(syslog =>
                {
                    try
                    {
                        if (syslog == null)
                            return;

                        logDescr.FillData(syslog, SelectedType);
                    }
                    catch (Exception err)
                    {
                        dbg.Error(err);
                    }
                    finally
                    {
                        if (logDescr != null && logDescr.IsReceived)
                            InitData();
                        Current = States.Common;
                    }
                }, err =>
                {
                    dbg.Error(err);
                    Current = States.Common;
                }));

        }

        void GetLogString() {
            Current = States.Loading;
			SystemLogString = null;
            subscription.Add(CurrentSession
                .GetSystemLog(SelectedType.type)
                .ObserveOnCurrentDispatcher()
                .Subscribe(syslog => {
					try {
                        if (syslog == null)
                            throw new InvalidOperationException(LocalSystemLog.instance.syslogNotFound);

						logDescr.FillData(syslog, SelectedType);
						InitData();
						Current = States.Common;
					} catch (Exception err) {
						dbg.Error(err);
						ErrorMessage = err.Message;
						ErrorBtnClick = new DelegateCommand(() => {
							Current = States.Common;
						});
						Current = States.Error;
					}
                }, err => {
					dbg.Error(err);
					ErrorMessage = err.Message;
                    ErrorBtnClick = new DelegateCommand(() => {
                        Current = States.Common;
                    });

                    Current = States.Error;
                }));
        }

        #region Commands
        public override void InitCommands() {
            base.InitCommands();

			btnErrorClose = new DelegateCommand(() => {
				Current = States.Common;
			});

            OnGet = new DelegateCommand(() => {
                GetLogString();
            });

			OnSave = new DelegateCommand(() => {
				var dlg = new SaveFileDialog();
				dlg.Filter = "Text files (*.txt)|*.txt|All files(*.*)|*.*";
				dlg.Title = Strings.titleSaveLog;
				dlg.FileName = logDescr.SysLogFileName;
				//dlg.InitialDirectory = Directory.GetCurrentDirectory();
				if (dlg.ShowDialog() == true) {
					using (var fstream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write, FileShare.Read)) {
						var data = logDescr.SystemLog.ToUtf8();
						fstream.Write(data, 0, data.Length);
						fstream.Flush();
						fstream.Close();
					}
				}
			}, () => {
				if (logDescr != null && logDescr.SystemLog != "")
					return true;
				else
					return false;
			});

			OnAttach = new DelegateCommand(() => {
				if(logDescr != null && logDescr.Attachment != null){
					var hasAttachment =
							logDescr.Attachment != null &&
							logDescr.Attachment.Include != null &&
							logDescr.Attachment.Include.Length > 0;
					var attachment = hasAttachment ? logDescr.Attachment : null;
					if (hasAttachment) {
						var dlg = new SaveFileDialog();
						dlg.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
						dlg.Title = Strings.titleSaveAttach;
						dlg.FileName = logDescr.AttachmentFileName;
						//dlg.InitialDirectory = Directory.GetCurrentDirectory();
						if (dlg.ShowDialog() == true) {
							using (var fstream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write, FileShare.Read)) {
//								var data = logDescr.Attachment.Include;//syslog.String.ToUtf8();
								fstream.Write(logDescr.Attachment.Include, 0, logDescr.Attachment.Include.Length);
								fstream.Flush();
								fstream.Close();
							}
						}
					}
				}
			}, () => {
 				if(logDescr != null && logDescr.Attachment != null)
					return true;
				else
					return false;
			});
        }

		public DelegateCommand btnErrorClose {get { return (DelegateCommand)GetValue(btnErrorCloseProperty); }set { SetValue(btnErrorCloseProperty, value); }}
		public static readonly DependencyProperty btnErrorCloseProperty = DependencyProperty.Register("btnErrorClose", typeof(DelegateCommand), typeof(SystemLogViewModel));

        public ICommand OnGet {get { return (ICommand)GetValue(OnGetProperty); }set { SetValue(OnGetProperty, value); }}
        public static readonly DependencyProperty OnGetProperty = DependencyProperty.Register("OnGet", typeof(ICommand), typeof(SystemLogViewModel));

		public DelegateCommand OnSave {get { return (DelegateCommand)GetValue(OnSaveProperty); }set { SetValue(OnSaveProperty, value); }}
		public static readonly DependencyProperty OnSaveProperty = DependencyProperty.Register("OnSave", typeof(DelegateCommand), typeof(SystemLogViewModel));

		public DelegateCommand OnAttach { get { return (DelegateCommand)GetValue(OnAttachProperty); } set { SetValue(OnAttachProperty, value); } }
		public static readonly DependencyProperty OnAttachProperty = DependencyProperty.Register("OnAttach", typeof(DelegateCommand), typeof(SystemLogViewModel));

        #endregion Commands
		public string LogInfo {get { return (string)GetValue(LogInfoProperty); }set { SetValue(LogInfoProperty, value); }}
		public static readonly DependencyProperty LogInfoProperty = DependencyProperty.Register("LogInfo", typeof(string), typeof(SystemLogViewModel));

        public SysLogType SelectedType {get { return (SysLogType)GetValue(SelectedTypeProperty); }set { SetValue(SelectedTypeProperty, value); }}
        public static readonly DependencyProperty SelectedTypeProperty = DependencyProperty.Register("SelectedType", typeof(SysLogType), typeof(SystemLogViewModel), new PropertyMetadata(DefaultSysLogType));

        public string SystemLogString {get { return (string)GetValue(SystemLogStringProperty); }set { SetValue(SystemLogStringProperty, value); }}
        public static readonly DependencyProperty SystemLogStringProperty = DependencyProperty.Register("SystemLogString", typeof(string), typeof(SystemLogViewModel));

    }

}
