using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Controls;
using System.Xml;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using utils;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

namespace odm.ui.activities {
	public partial class ErrorView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Exception error) {
			var model = new Model(error);
			return container.StartViewActivity<Result>(context => {
				var view = new ErrorView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public Model model;

		public ErrorView(Exception error) {
			Init(new Model(error));
		}

		private void Init(Model model) {
			this.model = model;
			this.DataContext = model;

			InitializeComponent();

			Localization();

			okButton.Command = new DelegateCommand(
				() => Success(new Result.Ok()),
				() => true
			);

			detailsButton.Click += (o, e) => {
				if (valueDetails.Visibility != System.Windows.Visibility.Visible) {
					valueDetails.Visibility = System.Windows.Visibility.Visible;
					detailsButton.ClearBinding(Button.ContentProperty);
					detailsButton.Content = "Hide details";
				} else {
					valueDetails.Visibility = System.Windows.Visibility.Collapsed;
					detailsButton.ClearBinding(Button.ContentProperty);
					detailsButton.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.details);
				}

			};
			if (model.error is WebException) {
			}
			var err = CorrectError(model.error);
			if (err is FaultException) {
				var faulrExc = (err as FaultException);
				var msgFault = faulrExc.CreateMessageFault();
				if (msgFault.HasDetail) {
					try {
						var sb = new StringBuilder();
						var reader = msgFault.GetReaderAtDetailContents();
						do {
							sb.Append(reader.Value);
						} while (reader.Read());
						var det = sb.ToString();
						if (!String.IsNullOrEmpty(det)) {
							valueDetails.Text = det; // det.InnerText;
							detailsButton.IsEnabled = true;
						}
					} catch {
						//swallow error
						//no details will be shown, if something went wrong
					}
				}
			}

			message.Text = err.Message;
		}
		//SerialDisposable
		void Localization() {
			detailsButton.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.details);
			okButton.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.close);
		}
		//protected string GetErrorMessage(Exception error) {
		//    var fault = error as FaultException;
		//    if (fault != null) {
		//        var msg = fault.CreateMessageFault();
		//        var list = new List<string>();
		//        var reason = msg.Reason.ToString().Trim(' ', '.');
		//        if (!String.IsNullOrEmpty(reason)) {
		//            list.Add(reason);
		//        }
		//        if (msg.HasDetail) {
		//            var details = msg

		//        }
		//    }
		//}

		protected Exception CorrectError(Exception error) {
			var protoException = error as ProtocolException;
			if (protoException == null) {
				return error;
			}
			var webException = error.InnerException as WebException;
			if (webException == null) {
				return error;
			}
			var response = webException.Response as HttpWebResponse;
			if (response == null) {
				return error;
			}
			try {
				using (var stream = response.GetResponseStream()) {
					using (var xmlReader = XmlReader.Create(stream)) {
						var msg = Message.CreateMessage(xmlReader, int.MaxValue, MessageVersion.Soap12WSAddressing10);
						if (msg.IsFault) {
							var fault = MessageFault.CreateFault(msg, Int16.MaxValue);
							return new FaultException(fault);
						}
					}
				}
			} catch {
				return error;
			}

			return error;
		}

		public void Dispose() {
			Cancel();
		}
	}
}
