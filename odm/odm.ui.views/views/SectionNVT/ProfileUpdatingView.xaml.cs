using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using onvif.services;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for PropertyVideoStreaming.xaml
	/// </summary>
	public partial class ProfileUpdatingView : UserControl {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ProfileUpdatingView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }
		public LocalProfile Strings { get { return LocalProfile.instance; } }


		#region Binding
		private string GetCfgDisplayName(IConfigurationEntity cfg) {
			if (cfg == null) {
				return null;
			}
			if (cfg.name == null) {
				return cfg.token;
			}
			return cfg.name;
		}
		void BindModel(Model model) {
			//this.CreateBinding(IsModifiedProperty, model, x => x.isModified);

			//this.valueAecfg.IsEnabled = model.audioEncCfgs.Length > 0; 
			this.valueAecfg.CreateBinding(TextBlock.TextProperty, model,
				x => GetCfgDisplayName(x.audioEncCfg)
			);
			this.btnAecfg.IsEnabled = model.audioEncCfgs.Length > 0;
			this.chbIsAecfg.IsEnabled = model.audioEncCfgs.Length > 0;
			this.chbIsAecfg.CreateBinding(CheckBox.IsCheckedProperty, model,
				x => x.isAudioEncCfgEnabled,
				(m, v) => m.isAudioEncCfgEnabled = v
			);

			this.valueMetacfg.CreateBinding(TextBlock.TextProperty, model,
				x => GetCfgDisplayName(x.metaCfg)
			);
			this.btnMetacfg.IsEnabled = model.metaCfgs.Length > 0;
			this.chbIsMetacfg.IsEnabled = model.metaCfgs.Length > 0;
			this.chbIsMetacfg.CreateBinding(CheckBox.IsCheckedProperty, model,
				x => x.isMetaCfgEnabled,
				(m, v) => m.isMetaCfgEnabled = v
			);

			this.valuePtzcfg.CreateBinding(TextBlock.TextProperty, model,
				x => GetCfgDisplayName(x.ptzCfg)
			);
			this.btnPtzcfg.IsEnabled = model.ptzCfgs.Length > 0;
			this.chbIsPtzcfg.IsEnabled = model.ptzCfgs.Length > 0;
			this.chbIsPtzcfg.CreateBinding(CheckBox.IsCheckedProperty, model,
				x => x.isPtzCfgEnabled,
				(m, v) => m.isPtzCfgEnabled = v
			);

			this.valueVacfg.CreateBinding(TextBlock.TextProperty, model,
				x => GetCfgDisplayName(x.analyticsCfg)
			);
			this.btnVacfg.IsEnabled = model.analyticsCfgs.Length > 0;
			this.chbIsVacfg.IsEnabled = model.analyticsCfgs.Length > 0;
			this.chbIsVacfg.CreateBinding(CheckBox.IsCheckedProperty, model,
				x => x.isAnalyticsCfgEnabled,
				(m, v) => m.isAnalyticsCfgEnabled = v
			);

			this.valueVecfg.CreateBinding(TextBlock.TextProperty, model,
				x => GetCfgDisplayName(x.videoEncCfg)
			);
			this.btnVecfg.IsEnabled = model.videoEncCfgs.Length > 0;
			this.chbIsVecfg.IsEnabled = model.videoEncCfgs.Length > 0;
			this.chbIsVecfg.CreateBinding(CheckBox.IsCheckedProperty, model,
				x => x.isVideoEncCfgEnabled,
				(m, v) => m.isVideoEncCfgEnabled = v
			);

		}
		void Localization() {
			captionVecfg.CreateBinding(TextBlock.TextProperty, Strings, s => s.captionVecfg);
			captionAecfg.CreateBinding(TextBlock.TextProperty, Strings, s => s.captionAecfg);
			captionVacfg.CreateBinding(TextBlock.TextProperty, Strings, s => s.captionVacfg);
			captionPtzcfg.CreateBinding(TextBlock.TextProperty, Strings, s => s.captionPtzcfg);
			captionMetacfg.CreateBinding(TextBlock.TextProperty, Strings, s => s.captionMetacfg);
		}
		//public bool IsModified {
		//    get { return (bool)GetValue(IsModifiedProperty); }
		//    set { SetValue(IsModifiedProperty, value); }
		//}
		//public static readonly DependencyProperty IsModifiedProperty =
		//    DependencyProperty.Register("IsModified", typeof(bool), typeof(CreateProfileView));

		#endregion Binding

		private void Init(Model model) {
			this.DataContext = model;

			var selectVideoEncCfgCommand = new DelegateCommand(
				() => Success(new Result.SelectVideoEncCfg(model)),
				() => true
			);
			SelectVideoEncCfgCommand = selectVideoEncCfgCommand;

			var selectAudioEncCfgCommand = new DelegateCommand(
				() => Success(new Result.SelectAudioEncCfg(model)),
				() => true
			);
			SelectAudioEncCfgCommand = selectAudioEncCfgCommand;

			var selectAnalitycsCfgCommand = new DelegateCommand(
				() => Success(new Result.SelectAnalyticsCfg(model)),
				() => true
			);
			SelectAnalyticsCfgCommand = selectAnalitycsCfgCommand;

			var selectPtzCfgCommand = new DelegateCommand(
				() => Success(new Result.SelectPtzCfg(model)),
				() => true
			);
			SelectPtzCfgCommand = selectPtzCfgCommand;

			var selectMetaCfgCommand = new DelegateCommand(
				() => Success(new Result.SelectMetaCfg(model)),
				() => true
			);
			SelectMetaCfgCommand = selectMetaCfgCommand;

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

			InitializeComponent();

			BindModel(model);
			Localization();
		}
		public void Dispose() {
			Cancel();
		}

	}
}
