namespace odm.ui.activities
    open System
    open System.IO
    open System.Linq
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Windows
    open System.Windows.Threading

    open Microsoft.Win32
    open Microsoft.Practices.Unity

    open odm.onvif
    open odm.core
    open odm.infra
    //open odm.models
    open utils
    open utils.fsharp

    type Progress() = class
        static member Show(container:IUnityContainer, message:string) = async{
            let disp = Application.Current.Dispatcher
            //do! Async.Sleep(200)
            return! disp.InvokeAsync(fun ()->
                //if (!disp.IsDisposed) {
                let view = new ProgressView(message)
                //disp.Add(presenter.ShowView(view));
                let presenter = container.Resolve<IViewPresenter>()
                presenter.ShowView(view)
                //}
            )
        }
    end

//     type ErrorInfo() = class
//        static member Show(ctx:IUnityContainer, message:string) = async{
//            let disp = Application.Current.Dispatcher
//            //do! Async.Sleep(200)
//            return! disp.InvokeAsync(fun ()->
//                //if (!disp.IsDisposed) {
//                let presenter = ctx.Resolve<IViewPresenter>()
//                let view = new ProgressView(message)
//                //disp.Add(presenter.ShowView(view));
//                presenter.ShowView(view)
//                //}
//            )
//        }
//    end