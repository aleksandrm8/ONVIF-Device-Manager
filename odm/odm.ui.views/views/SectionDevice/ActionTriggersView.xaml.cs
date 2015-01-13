using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.ui.activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
using utils;
using odm.infra;
using Microsoft.Practices.Prism.Commands;

namespace odm.ui.activities
{
    /// <summary>
    /// Interaction logic for ActionTriggersView.xaml
    /// </summary>
    public partial class ActionTriggersView : UserControl, IDisposable
    {
        #region Activity definition
        public static FSharpAsync<Result> Show(IUnityContainer container, Model model)
        {
            return container.StartViewActivity<Result>(context =>
            {
                var view = new ActionTriggersView(model, context);

                var presenter = container.Resolve<IViewPresenter>();
                presenter.ShowView(view);
            });
        }
        #endregion

        CompositeDisposable disposables = new CompositeDisposable();

        public ActionTriggersView()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            disposables.Dispose();
        }



        public void Init(Model model)
        {
            InitializeComponent();

            try
            {
                OnCompleted += () =>
                {
                    disposables.Dispose();
                };

                /*disposables.Add(Observable.FromEventPattern<MouseWheelEventArgs>(scroll, "PreviewMouseWheel")
                    .Subscribe(evarg =>
                    {
                        scroll.ScrollToVerticalOffset(-1 * evarg.EventArgs.Delta);
                    })
                );*/

                listTriggers.ItemsSource = model.triggers;

                listTriggers.CreateBinding(ListBox.SelectedValueProperty, model, x => x.selection, (m, o) =>
                {
                    m.selection = o;
                });

                var createTriggerCommand = new DelegateCommand(
                    () => Success(new Result.Create(model)),
                    () => true
                );
                createTriggerButton.Command = createTriggerCommand;

                var deleteTriggerCommand = new DelegateCommand(
                    () => Success(new Result.Delete(model)),
                    () => model.selection != null
                );
                deleteTriggerButton.Command = deleteTriggerCommand;

                var modifyTriggerCommand = new DelegateCommand(
                    () => Success(new Result.Modify(model)),
                    () => model.selection != null
                );
                modifyTriggerButton.Command = modifyTriggerCommand;

                disposables.Add(
                    model
                        .GetPropertyChangedEvents(m => m.selection)
                        .Subscribe(v =>
                        {
                            modifyTriggerCommand.RaiseCanExecuteChanged();
                            deleteTriggerCommand.RaiseCanExecuteChanged();
                        })
                );
            }
            catch (Exception err)
            {
                dbg.Error(err);
            }

            Localization();
        }

        public LocalButtons ButtonsStrings { get { return LocalButtons.instance; } }
        void Localization() {
            createTriggerButton.CreateBinding(Button.ContentProperty, ButtonsStrings, m => m.create);
            modifyTriggerButton.CreateBinding(Button.ContentProperty, ButtonsStrings, m => m.modify);
            deleteTriggerButton.CreateBinding(Button.ContentProperty, ButtonsStrings, m => m.delete);
        }
    }
}
