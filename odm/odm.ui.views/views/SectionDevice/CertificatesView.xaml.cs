using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using odm.infra;
using onvif.services;
using Org.BouncyCastle.X509;
using utils;
using Org.BouncyCastle.Asn1.X509;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for CertificatesView.xaml
	/// </summary>
	public partial class CertificatesView : UserControl, IDisposable, INotifyPropertyChanged {
		
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new CertificatesView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion
		private CompositeDisposable disposables = new CompositeDisposable();

		public ObservableCollection<CertificateDescr> certificates { get; set; }

		public LocalTitles Titles { get { return LocalTitles.instance; } }
		public LocalButtons ButtonsLocales { get { return LocalButtons.instance; } }
		public LocalSequrity Strings { get { return LocalSequrity.instance; } }

		public ICommand UploadCmd { get; private set; }

		Model model;

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			certificates = new ObservableCollection<CertificateDescr>();

			DeleteCommand = new DelegateCommand(
				() => {
					var descr = (certificateslist.SelectedItem as CertificateDescr);
					if(descr != null)
						Success(new Result.Delete(model, descr.CertificateId));	
				},
				() => certificateslist.SelectedItem != null
			);

			ApplyCommand = new DelegateCommand(
				() => {
					//List<string> certifIdlst = new List<string>();
					//var cert = certificates.ToList().Where(x => x.isEnabled == true);
					//cert.ForEach(crt=>{
					//    certifIdlst.Add(crt.CertificateId);
					//});
					Success(new Result.Apply(model));
				},
				() => true
			);

			UploadCmd = new DelegateCommand(
				() => {
					//var djg = new OpenFileDialog();
					//djg.ShowDialog();
					Success(new Result.Upload(model));
				},
				() => true
			);

			InitializeComponent();

			this.model = model;

			BindData(model);
			Localization();
		}

		CertificateDescr selectedCertificate;
		public CertificateDescr SelectedCertificate {
			get {
				return selectedCertificate;
			}
			set {
				selectedCertificate = value;
				NotifyPropertyChanged("SelectedCertificate");
			}
		}

		public class CertificateDescr {
			public CertificateDescr(Certificate cert){
				this.cert = cert;
				Parse(cert);
			}
			Certificate cert;
			
			//public string ContentType {
			//    get {
			//        return cert.contentType;
			//    }
			//}
			X509Certificate x509;
			string GetSubscriber() {
				string ret = "";
				if(x509.IssuerDN == null){
					ret = "Self signed";
				}else{
					var subscr = x509.IssuerDN.GetValues(X509Name.CN);
					ret = "CN: ";
					subscr.ForEach(val => {
						ret += val.ToString() + " ";
					});
					var subscrCa = x509.IssuerDN.GetValues(X509Name.Name);
					if (subscrCa.Count != 0) {
						ret = ret + " Name: ";
						subscr.ForEach(val => {
							ret += val.ToString() + " ";
						});
					}
				}
				return ret;
			}
			string GetCertificateName() {
				//x509.SubjectDN.
				if (x509.SerialNumber == null)
					return "<None>";
				return x509.SerialNumber.ToString();
			}
			string GetValidity() {
				if (x509.NotBefore == null && x509.NotAfter == null) {
					return "forever";
				}
				string notBefore = x509.NotBefore == null ? ".." : x509.NotBefore.ToShortDateString();
				string notAfter = x509.NotAfter == null ? ".." : x509.NotAfter.ToShortDateString();
				return "" + notBefore + " - " + notAfter;
			}
			void Parse(Certificate cert) {
				var certParser = new X509CertificateParser();
				x509 = certParser.ReadCertificate(cert.data);
			}
			public override string ToString() {
				var certParser = new X509CertificateParser();
				var x509 = certParser.ReadCertificate(cert.data);
				return x509.ToString();
			}
			public bool isEnabled {
				get {
					return cert.enabled;
				}
				set {
					cert.enabled = value;
				}
			}
			public string CertificateId { get { return cert.cid; } }
			public string FromTo { get { return GetValidity(); } }
			public string CommonName { get { return GetCertificateName(); } }
			public string Subscriber { get { return GetSubscriber(); } }
		}
		void Localization() {
			uploadCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.uploadCertificate);
			btnUpload.CreateBinding(Button.ContentProperty, Strings, s => s.btnUpload);
			setStatus.CreateBinding(Button.ContentProperty, ButtonsLocales, s => s.apply);
			delete.CreateBinding(Button.ContentProperty, ButtonsLocales, s => s.delete);
		}
		void BindData(Model model) {
			model.certificates.ForEach(cert => {
				bool isEnabled = cert.enabled;
				CertificateDescr cdescr = new CertificateDescr(cert);
				certificates.Add(cdescr);
			});

			
			certificateslist.SelectionChanged+=new SelectionChangedEventHandler((obj, value)=>{
				if (certificateslist.SelectedItem != null) {
					//detailsValue.Text = certificateslist.SelectedItem.ToString();
					((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();
				}
			});
			certificateslist.CreateBinding(ListBox.ItemsSourceProperty, this, x => x.certificates);
		}


		public void Dispose() {
			Cancel();
		}

		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
