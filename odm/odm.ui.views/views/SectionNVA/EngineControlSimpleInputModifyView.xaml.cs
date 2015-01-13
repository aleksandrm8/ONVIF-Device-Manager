using System;
using System.Collections.Generic;
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
using odm.core;
using odm.infra;
using odm.ui.controls;
using odm.ui.core;
using odm.player;
using onvif.services;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using System.Reactive.Disposables;
using utils;
using Microsoft.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Controls.Primitives;


namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for EngineControlSimpleInputModifyView.xaml
	/// </summary>
	public partial class EngineControlSimpleInputModifyView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineControlSimpleInputModifyView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		CompositeDisposable disposables = new CompositeDisposable();

		public void Init(Model model) {
			InitializeComponent();
			
			if (model.input == null) return;
			if (model.input.videoInput == null)
				model.input.videoInput = new VideoEncoderConfiguration();
			if (model.input.videoInput.resolution == null)
				model.input.videoInput.resolution = new VideoResolution() { width = 720, height = 576 };

			if (model.receiver == null) return;
			if (model.receiver.configuration == null)
				model.receiver.configuration = new ReceiverConfiguration();
			if (model.receiver.configuration.streamSetup == null)
				model.receiver.configuration.streamSetup = new StreamSetup();
			if (model.receiver.configuration.streamSetup.transport == null) {
				model.receiver.configuration.streamSetup.transport = new Transport();
				model.receiver.configuration.streamSetup.transport.protocol = TransportProtocol.udp;
			}

			valueEnableControl.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.isEnable, (m, v) => { m.isEnable = v; });

			valueWidth.Value = model.input.videoInput.resolution.width;
			valueHeight.Value = model.input.videoInput.resolution.height;
			//valueWidth.CreateBinding(IntegerUpDown.ValueProperty, model.input.VideoInput.Resolution, m => m.width, (m, v) => {
			//	m.width = v;
			//});
			//valueHeight.CreateBinding(IntegerUpDown.ValueProperty, model.input.VideoInput.Resolution, m => m.height, (m, v) => {
			//	m.height = v;
			//});

			valueReceivers.ItemsSource = model.receivers;
			valueReceivers.CreateBinding(ComboBox.SelectedItemProperty, model, x => x.receiver, (m, v) => { 
				m.receiver = v; 
			});

			valueTransport.CreateBinding(TextBlock.TextProperty, model, x => x.receiver.configuration.streamSetup.transport.protocol);
			valueMode.CreateBinding(TextBlock.TextProperty, model, x => x.receiver.configuration.mode);

			valueUri.CreateBinding(TextBlock.TextProperty, model, m => m.receiver.configuration.mediaUri);

			ApplyCommand = new DelegateCommand(
				() => {
					try {
						model.input.videoInput.resolution.width = (int)valueWidth.Value;
						model.input.videoInput.resolution.height = (int)valueHeight.Value;
						Success(new Result.Apply(model));
					} catch (Exception err) {
						Error(err);
					}
				},
				() => true
			);
			btnApply.Command = ApplyCommand;
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
