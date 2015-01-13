using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.controllers;
using odm.core;
using odm.ui.controls;
using odm.ui.core;
using utils;

namespace odm.ui.viewModels {
    public abstract class ViewModelDeviceBase : ViewModelBase {
        public ViewModelDeviceBase(IUnityContainer container)
            : base(container) {
        }
        public virtual void Init(INvtSession session, Account account) {
            base.CurrentSession = session;
            Load(session, account);
        }
        public abstract void Load(INvtSession session, Account account);
    }
    public abstract class ViewModelChannelBase : ViewModelBase {
        public ViewModelChannelBase(IUnityContainer container):base(container) {

        }
        public virtual void Init(INvtSession session, String chanToken, string profileToken, Account account, IVideoInfo videoInfo) {
            base.CurrentSession = session;
            this.ChannelToken = chanToken;
            this.profileToken = profileToken;
            CurrentAccount = account;
            VideoInfo = videoInfo;
            Load(session, chanToken, profileToken, account, videoInfo);
        }
        public override void Dispose() {
            //StopPlayback();
            base.Dispose();
        }
        //public void StopPlayback() {
        //    DataProcessInfo dataProcInfo = (DataProcessInfo)VideoInfo;
        //    try {
        //        if (dataProcInfo == null)
        //            return;

        //        dataProcInfo.Stop();
                
        //    } catch (Exception err) {
        //        dbg.Error(err);
        //        //dataProcInfo.ReleaseAll();
        //    }
        //}
        //public void StartPlayback() {
        //    //var dataProcInfo = VideoInfo;

        //    //try {
        //    //    if (dataProcInfo == null)
        //    //        return;

        //    //    dataProcInfo.Start();

        //    //} catch (Exception err) {
        //    //    dbg.Error(err);
        //    //    //dataProcInfo.ReleaseAll();
        //    //}
        //}
        public IVideoInfo VideoInfo { get; protected set; }
        public String ChannelToken;
		public string profileToken;
		public abstract void Load(INvtSession session, String chanToken, string profileToken, Account account, IVideoInfo videoInfo);
    }
	public abstract class ViewModelBase :DependencyObject, IDisposable, INotifyPropertyChanged{
        public ViewModelBase(IUnityContainer container) {
            this.container = container;
            dispatch = Dispatcher.CurrentDispatcher;
            subscription = new CompositeDisposable();
            InitCommands();
            BindData();
            Current = States.Loading;
        }
        public IUnityContainer container;
        public INvtSession CurrentSession;
        public Account CurrentAccount;

        public readonly Dispatcher dispatch;
        public CompositeDisposable subscription;

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
        public InfoFormStrings InfoStrings { get { return InfoFormStrings.instance; } }
        public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }

        public virtual void BindData() {
            this.CreateBinding(StateCommonProperty, this, x => { 
				return x.Current == States.Common ? Visibility.Visible : Visibility.Collapsed; 
			});
            this.CreateBinding(StateLoadingProperty, this, x => { 
				return x.Current == States.Loading ? Visibility.Visible : Visibility.Collapsed; 
			});
            this.CreateBinding(StateErrorProperty, this, x => { 
				return x.Current == States.Error ? Visibility.Visible : Visibility.Collapsed; 
			});
        }

        public virtual void Dispose() {
            subscription.Dispose();
            subscription = new CompositeDisposable();
        }

        #region States
        public enum States {
            Loading,
            Common,
            Error
        }
        States current;
        public States Current {
            get {
                return current;
            }
            set {
                current = value;
                OnPropertyChanged(() => Current);
            }
        }
        public virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyEvaluator) {
            var lambda = propertyEvaluator as LambdaExpression;
            var member = lambda.Body as MemberExpression;
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(member.Member.Name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Commands
        public virtual void InitCommands() {
            ErrorBtnClick = new DelegateCommand(() => {
                //LoadModel(CurrentSession);
            });

        }
        public ICommand ErrorBtnClick {
            get { return (ICommand)GetValue(ErrorBtnClickProperty); }
            set { SetValue(ErrorBtnClickProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ErrorBtnClick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorBtnClickProperty =
            DependencyProperty.Register("ErrorBtnClick", typeof(ICommand), typeof(ViewModelBase));

        #endregion

        #region StatesProperties
        public Visibility StateCommon {
            get { return (Visibility)GetValue(StateCommonProperty); }
            set { SetValue(StateCommonProperty, value); }
        }
        // Using a DependencyProperty as the backing store for StateCommon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateCommonProperty =
            DependencyProperty.Register("StateCommon", typeof(Visibility), typeof(ViewModelBase), new PropertyMetadata(Visibility.Collapsed));

        public Visibility StateError {
            get { return (Visibility)GetValue(StateErrorProperty); }
            set { SetValue(StateErrorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for StateError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateErrorProperty =
            DependencyProperty.Register("StateError", typeof(Visibility), typeof(ViewModelBase), new PropertyMetadata(Visibility.Collapsed));

        public Visibility StateLoading {
            get { return (Visibility)GetValue(StateLoadingProperty); }
            set { SetValue(StateLoadingProperty, value); }
        }
        // Using a DependencyProperty as the backing store for StateLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateLoadingProperty =
            DependencyProperty.Register("StateLoading", typeof(Visibility), typeof(ViewModelBase), new PropertyMetadata(Visibility.Collapsed));

        public string ErrorMessage {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ErrorMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(ViewModelBase));

        #endregion StatesProperties
    }
	public class ChangeHolder<T> {
		public void SetBoth(T val) {
			origin = val;
			current = val;
		}
		public void SetCurrent(T val) {
			current = val;
		}
		public T RevertChanges() {
			return origin;
		}
		public bool IsModified() {
			return !origin.Equals(current);
		}
		public ChangeHolder(T val) {
			origin = val;
			current = val;
		}
		T origin;
		T current;
	}
}
