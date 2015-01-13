using System;
using System.Reactive.Disposables;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.infra;
using Microsoft.Practices.Prism.Commands;
using System.Collections.Generic;
using Org.BouncyCastle.X509;
using onvif.services;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for CertificatesUploadView.xaml
	/// </summary>
	public partial class CertificateUploadView : UserControl, IDisposable {
		
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new CertificateUploadView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion
		
		private CompositeDisposable disposables = new CompositeDisposable();
		public LocalButtons ButtonsLocales { get { return LocalButtons.instance; } }
		public LocalSequrity Strings { get { return LocalSequrity.instance; } }

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			CancelCommand = new DelegateCommand(
				() => {
					Success(new Result.Cancel());
				},
				() => true
			);
			UploadCommand = new DelegateCommand(
				() => {
					model.certificate.certificateID = certificateNameValue.Text;
					Success(new Result.Upload());
				},
				() => true
			);

			InitializeComponent();

			certificateDetails.Text = CertificateToString(model.certificate);
			certificateNameValue.Text = CertificateNum(model.certificate);

			certificateNameCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.enterName);
			btnCancel.CreateBinding(Button.ContentProperty, ButtonsLocales, s => s.cancel);
			btnUpload.CreateBinding(Button.ContentProperty, Strings, s => s.uploadCertificate);
			captionDetails.CreateBinding(TextBlock.TextProperty, Strings, s => s.details);
		}
		public string CertificateNum(Certificate cert) {
			var certParser = new X509CertificateParser();
			var x509 = certParser.ReadCertificate(cert.certificate.data);
			return x509.SerialNumber.ToString();
		}
		public string CertificateToString(Certificate cert) {
			var certParser = new X509CertificateParser();
			var x509 = certParser.ReadCertificate(cert.certificate.data);
			return x509.ToString();
		}
		public void Dispose() {
			Cancel();
		}
	}
}
