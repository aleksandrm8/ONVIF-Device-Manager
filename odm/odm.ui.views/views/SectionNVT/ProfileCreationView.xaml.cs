using System.Windows;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using utils;

namespace odm.ui.activities {
	public partial class ProfileCreationView : UserControl {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ProfileCreationView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		public LocalButtons ButtonsStrings { get { return LocalButtons.instance; } }
        public LocalProfile Strings { get { return LocalProfile.instance; } }

		//private CompositeDisposable disposables = new CompositeDisposable();

        #region Binding
        void Localization() {
            captionVideoSourceConfig.CreateBinding(TextBlock.TextProperty, Strings, s => s.captionVideoSourceConfig);
            captionAscfg.CreateBinding(TextBlock.TextProperty, Strings, s => s.captionAscfg);
            captionProfileName.CreateBinding(TextBlock.TextProperty, Strings, s => s.captionProfileName);
            btnFinish.CreateBinding(Button.ContentProperty, ButtonsStrings, s => s.apply);
            btnAbort.CreateBinding(Button.ContentProperty, ButtonsStrings, s => s.cancel);
            btnConfigure.CreateBinding(Button.ContentProperty, ButtonsStrings, s => s.edit);
        }
        void BindModel(Model model) {
            this.CreateBinding(IsModifiedProperty, model, x => x.isModified);

			this.btnSelectVscfg.IsEnabled = model.videoSrcCfgs.Length > 0;
			this.chbIsVscfg.IsEnabled = true;
			this.chbIsVscfg.CreateBinding(
				CheckBox.IsCheckedProperty, model,
				x => x.isVideoSrcCfgEnabled,
				(m, v) => m.isVideoSrcCfgEnabled = v
			);
			this.valueVscfgToken.CreateBinding(
				TextBox.TextProperty, model, 
				x => {
					if(x.videoSrcCfg == null){return null;}
					if (x.videoSrcCfg.name == null) { return x.videoSrcCfg.token; }
					return x.videoSrcCfg.name;
				}
			);
            //this.CreateBinding(ProfTokenProperty, model, x => x.profToken, (m, v) => {
            //    m.profToken = v;
            //});
            this.valueProfileName.CreateBinding(TextBox.TextProperty, model, x => x.profName, (m, v) => {
                m.profName = v;
            });

            //this.CreateBinding(IsVideoSrcCfgEnabledProperty, model, x => x.isVideoSrcCfgEnabled, (m, v) => {
            //    m.isVideoSrcCfgEnabled = v;
            //});
			this.btnSelectAscfg.IsEnabled = model.audioSrcCfgs.Length > 0;
			this.chbIsAscfg.IsEnabled = model.audioSrcCfgs.Length > 0; 
			this.chbIsAscfg.CreateBinding(
				CheckBox.IsCheckedProperty, model, 
				x => x.isAudioSrcCfgEnabled,
				(m, v) => m.isAudioSrcCfgEnabled = v
			);
			this.valueAscfg.CreateBinding(
				TextBox.TextProperty, model,
				x => {
					if (x.audioSrcCfg == null) { return null; }
					if (x.audioSrcCfg.name == null) { return x.audioSrcCfg.token; }
					return x.audioSrcCfg.name;
				}
			);
        }

        public bool IsModified {
            get { return (bool)GetValue(IsModifiedProperty); }
            set { SetValue(IsModifiedProperty, value); }
        }
        public static readonly DependencyProperty IsModifiedProperty =
			DependencyProperty.Register("IsModified", typeof(bool), typeof(ProfileCreationView));
        
        #endregion Binding

		private void Init(Model model) {
			this.DataContext = this;
			var finishCommand = new DelegateCommand(
				() => Success(new Result.Finish(model)),
				() => true
			);
			FinishCommand = finishCommand;

			var abortCommand = new DelegateCommand(
				() => Success(new Result.Abort()),
				() => true
			);
			AbortCommand = abortCommand;

			var configureCommand = new DelegateCommand(
				() => Success(new Result.Configure(model)),
				() => true
			);
			ConfigureCommand = configureCommand;

			var selectVideoSrcCfgCommand = new DelegateCommand(
				() => Success(new Result.SelectVideoSrcCfg(model)),
				() => true
			);
			SelectVideoSrcCfgCommand = selectVideoSrcCfgCommand;

			var selectAudioSrcCfgCommand = new DelegateCommand(
				() => Success(new Result.SelectAudioSrcCfg(model)),
				() => true
			);
			SelectAudioSrcCfgCommand = selectAudioSrcCfgCommand;

			if (model != null) {
				//disposables.Add(
				//    context.model
				//        .GetPropertyChangedEvents(x => x.isModified)
				//        .Subscribe(x => {
				//            applyCmd.RaiseCanExecuteChanged();
				//            cancelCmd.RaiseCanExecuteChanged();
				//        })
				//);
				//disposables.Add(
				//    context.model
				//        .GetPropertyChangedEvents(x => x.dhcp)
				//        .Where(dhcp=>!dhcp)
				//        .Subscribe(dhcp => {
				//            context.model.useDnsFromDhcp = dhcp;
				//            context.model.useNtpFromDhcp = dhcp;
				//        })
				//);
			}
			
			InitializeComponent();
			BindModel(model);
			Localization();
		}
		public void Dispose() {
			Cancel();
		}
	}

}
