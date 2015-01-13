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
using System.Reactive.Disposables;
using Microsoft.Practices.Unity;
using Microsoft.FSharp.Control;
using utils;
using odm.infra;
using Microsoft.Practices.Prism.Commands;
using onvif.services;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for ReceiversModifyView.xaml
	/// </summary>
	public partial class ReceiversModifyView : UserControl {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ReceiversModifyView(model, context);

				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion
		public ReceiversModifyView() {
			InitializeComponent();
		}

		CompositeDisposable disposables = new CompositeDisposable();

		public void Init(Model model) {
			InitializeComponent();

			OnCompleted += () => {
				disposables.Dispose();
			};

			var ApplyCommand = new DelegateCommand(
				() => {
					 Success(new Result.Apply(model));
				},
				() => true
			);
			applyButton.Command = ApplyCommand;
			
			var CancelCommand = new DelegateCommand(
				() => Success(new Result.Cancel(model)),
				() => true
			);
			cancelButton.Command = CancelCommand;

			if (model.receiver == null) {
				model.receiver = new Receiver();
				model.receiver.configuration = new ReceiverConfiguration();
				model.receiver.configuration.streamSetup = new StreamSetup() {
					transport = new Transport()
				};
			}

			valueMediaUri.Text = model.receiver.configuration.mediaUri;
			valueMediaUri.SetUpdateTrigger(
				TextBox.TextProperty,
				(string v) => model.receiver.configuration.mediaUri = v
			);

			valueMode.Items.Add(ReceiverMode.alwaysConnect);
			valueMode.Items.Add(ReceiverMode.autoConnect);
			valueMode.Items.Add(ReceiverMode.neverConnect);
			//valueMode.Items.Add(ReceiverMode.Unknown);
			valueMode.SelectedValue = model.receiver.configuration.mode;
			valueMode.SetUpdateTrigger(
				ComboBox.SelectedValueProperty,
				(ReceiverMode v) => model.receiver.configuration.mode = v
			);

			valueStreamType.Items.Add(StreamType.rtpMulticast);
			valueStreamType.Items.Add(StreamType.rtpUnicast);
			valueStreamType.SelectedValue = model.receiver.configuration.streamSetup.stream;
			valueStreamType.SetUpdateTrigger(
				ComboBox.SelectedValueProperty, 
				(StreamType v) => model.receiver.configuration.streamSetup.stream = v
			);

			valueToken.Text = model.receiver.token;
			valueToken.SetUpdateTrigger(
				TextBlock.TextProperty,
				(string v) => model.receiver.token = v
			);

			valueTransport.Items.Add(TransportProtocol.http);
			valueTransport.Items.Add(TransportProtocol.rtsp);
			valueTransport.Items.Add(TransportProtocol.tcp);
			valueTransport.Items.Add(TransportProtocol.udp);
			valueTransport.SelectedValue = model.receiver.configuration.streamSetup.transport.protocol;
			valueTransport.SetUpdateTrigger(
				ComboBox.SelectedValueProperty, 
				(TransportProtocol v) => model.receiver.configuration.streamSetup.transport.protocol = v
			);
		}
	}
}
