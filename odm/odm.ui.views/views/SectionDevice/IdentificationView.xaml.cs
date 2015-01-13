/*
* File name   : IdentificationView.xaml.cs
* Description : Device Identification view page
*
* (c) Copyright SYNESIS 2010
* 
* Modification History:
* Date        Name        Description
* 13/07/10    A.Vrana	   Initial implementation
*/

using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using utils;
using System;

namespace odm.ui.activities {
	public partial class IdentificationView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new IdentificationView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		#region Binding
		void BindModel(Model model) {
			applyButton.CreateBinding(Button.ContentProperty, ButtonsStrings, x => x.save);
			applyButton.Command = ApplyCommand;

			cancelButton.CreateBinding(Button.ContentProperty, ButtonsStrings, x => x.cancel);
			cancelButton.Command = CancelCommand;

			nameCaption.CreateBinding(Label.ContentProperty, Strings, x => x.name);
			nameValue.CreateBinding(
				TextBox.TextProperty, model,
				x => x.name,
				(m, v) => m.name = v
			);

			locationCaption.CreateBinding(Label.ContentProperty, Strings, x => x.location);
			locationValue.CreateBinding(
				TextBox.TextProperty, model,
				x => x.location,
				(m, v) => m.location = v
			);

			modelCaption.CreateBinding(Label.ContentProperty, Strings, x => x.model);
			modelValue.Text = model.model;

			firmwareCaption.CreateBinding(Label.ContentProperty, Strings, x => x.firmware);
			firmwareValue.Text = model.firmware;

			hardwareCaption.CreateBinding(Label.ContentProperty, Strings, x => x.hardware);
			hardwareValue.Text = model.hardware;

			ipAddressCaption.CreateBinding(Label.ContentProperty, Strings, x => x.ipAddress);
			ipAddressValue.Text = model.ip;

			macCaption.CreateBinding(Label.ContentProperty, Strings, x => x.macAddress);
			macValue.Text = model.mac;

			manufacturerCaption.CreateBinding(Label.ContentProperty, Strings, x => x.manufacturer);
			manufacturerValue.Text = model.manufacturer;

			serialCaption.CreateBinding(Label.ContentProperty, Strings, x => x.deviceID);
			serialValue.Text = model.serial;

			onvifVersionCaption.CreateBinding(Label.ContentProperty, Strings, x => x.version);
			onvifVersionValue.Text = model.onvifVersion !=null ? model.onvifVersion.ToString() : "unknown";

			var uri = activityContext.container.Resolve<Uri>();
			onvifUriValue.Text = uri.AbsoluteUri;
		}
		#endregion Binding

		private CompositeDisposable disposables = new CompositeDisposable();
		public Model model;

		public LinkButtonsStrings Titles {
			get {
				return LinkButtonsStrings.instance;
			}
		}
		public SaveCancelStrings ButtonsStrings {
			get {
				return SaveCancelStrings.instance;
			}
		}
		public PropertyIdentificationStrings Strings {
			get {
				return PropertyIdentificationStrings.instance;
			}
		}
		public ICommand CancelCommand {
			get;
			private set;
		}

		
		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.DataContext = model;
			this.model = model;

			var applyCommand = new DelegateCommand(
				() =>Success(new Result.Apply(model)),
				() => true
			);
			ApplyCommand = applyCommand;

			//var closeCommand = new DelegateCommand(
			//    () => Success(new Result.Close()),
			//    () => true
			//);
			//CloseCommand = closeCommand;

			CancelCommand = new DelegateCommand(
				() => model.RevertChanges(),
				() => true
			);

			InitializeComponent();

			BindModel(model);
		}

		public void Dispose() {
			Cancel();
		}

		void IDisposable.Dispose() {
			Cancel();
		}
	}
}
