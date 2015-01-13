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
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using utils;
using System.Reactive.Disposables;
using onvif.services;
using System.Reactive.Linq;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for ReceiversView.xaml
	/// </summary>
	public partial class ReceiversView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ReceiversView(model, context);

				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion
		CompositeDisposable disposables = new CompositeDisposable();

		public void Init(Model model) {
			InitializeComponent();

			try {
				OnCompleted += () => {
					disposables.Dispose();
				};

				disposables.Add(Observable.FromEventPattern<MouseWheelEventArgs>(scroll, "PreviewMouseWheel")
					.Subscribe(evarg => {
						scroll.ScrollToVerticalOffset(-1 * evarg.EventArgs.Delta);
					})
				);

				listReceivers.ItemsSource = model.receivers;

				listReceivers.CreateBinding(ListBox.SelectedValueProperty, model, x => x.selection, (m, o) => {
					m.selection = o;
				});

				var createReceiverCommand = new DelegateCommand(
					() => Success(new Result.Create(model)),
					() => true
				);
				createReceiverButton.Command = createReceiverCommand;

				var deleteReceiverCommand = new DelegateCommand(
					() => Success(new Result.Delete(model)),
					() => model.selection != null
				);
				deleteReceiverButton.Command = deleteReceiverCommand;

				var modifyReceiverCommand = new DelegateCommand(
					() => Success(new Result.Modify(model)),
					() => model.selection != null
				);
				modifyReceiverButton.Command = modifyReceiverCommand;

				disposables.Add(
					model
						.GetPropertyChangedEvents(m => m.selection)
						.Subscribe(v => {
							modifyReceiverCommand.RaiseCanExecuteChanged();
							deleteReceiverCommand.RaiseCanExecuteChanged();
						})
				);
			} catch(Exception err) {
				dbg.Error(err);
			}
		}

		public void Dispose() {
			disposables.Dispose();	
		}
	}
	public class ReceiverStatus {
		public ReceiverStatus(Receiver rec) {
			
		}
	}
}
