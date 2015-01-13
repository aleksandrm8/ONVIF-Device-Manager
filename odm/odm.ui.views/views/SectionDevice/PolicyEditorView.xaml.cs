using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using odm.ui.controls;
using onvif.services;
using utils;


namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for PolicyEditorView.xaml
	/// </summary>
	public partial class PolicyEditorView : UserControl {
		
		//#region Activity definition
		//public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
		//    return ViewActivity.Create<Model, Result>(container, model, context => {
		//        var presenter = container.Resolve<IViewPresenter>();
		//        var view = new UserManagementView(context);
		//        var disp = presenter.ShowView(view);
		//        return Disposable.Create(() => {
		//            disp.Dispose();
		//            view.Dispose();
		//        });
		//    });
		//}
		//#endregion

		//private IActivityContext<Model, Result> context;
		//private bool completed = false;
		//private CompositeDisposable disposables = new CompositeDisposable();
		//public SaveCancelStrings SaveCancel { get { return SaveCancelStrings.instance; } }
		//public UserManagementStrings Strings { get { return UserManagementStrings.instance; } }
		//public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }

		//public PolicyEditorView(IActivityContext<Model, Result> context) {

        public LocalButtons Buttons { get { return LocalButtons.instance; } }

        void Localization() {
            saveButton.CreateBinding(Button.ContentProperty, Buttons, s => s.apply);
            closeButton.CreateBinding(Button.ContentProperty, Buttons, s => s.close);
        }
        public PolicyEditorView() {
			//var model = context.model;
			//this.context = context;
			//this.DataContext = model;
			InitializeComponent();

            Localization();
		}

		//public void CompleteWith(Action cont) {
		//    if (!completed) {
		//        cont();
		//        completed = true;
		//        OnCompleted();
		//        disposables.Dispose();
		//    }
		//}

		//public void OnCompleted() {
		//    //activity has been completed
		//}
		//public void OnCancel() {
		//    //activity has been canceled
		//}
		//public void Success(Result result) {
		//    CompleteWith(
		//        () => context.Success(result)
		//    );
		//}
		//public void Error(Exception error) {
		//    CompleteWith(
		//        () => context.Error(error)
		//    );
		//}
		//public void Dispose() {
		//    CompleteWith(() => OnCancel());
		//}

	}
}
