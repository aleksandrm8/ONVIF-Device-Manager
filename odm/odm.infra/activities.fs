namespace odm.infra
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Linq
    open System.Net
    open System.Reactive.Disposables
    open System.Runtime.CompilerServices
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity

    open utils
    open utils.fsharp

    type IActivityContext<'TResult> = interface
        abstract container: IUnityContainer
        abstract Success: result:'TResult -> unit
        abstract Error: error:Exception -> unit
        abstract Cancel: unit -> unit
        abstract RegisterCancellationCallback: callback:Action->IDisposable
    end
    
    [<Extension>]
    type ActivityExtensions private() = class
        [<Extension>]
        static member StartViewActivity<'TResult>(container: IUnityContainer, callback: Action<IActivityContext<'TResult>>)= async{
            let! ct = Async.CancellationToken
            return! Async.FromContinuations(fun (success, error, cancel) ->
                let context = {
                    new IActivityContext<'TResult> with
                        member this.container = container
                        member this.Success(result:'TResult) = success(result)
                        member this.Error(err:Exception) = error(err)
                        member this.Cancel() = cancel(new OperationCanceledException())
                        member this.RegisterCancellationCallback(callback: Action) = 
                            ct.Register(callback) :> IDisposable
                }
                let disp = Application.Current.Dispatcher
                Async.StartWithContinuations(
                    disp.InvokeAsync(
                        (fun() -> callback.Invoke(context))
                    ),
                    (fun()->()), 
                    error, 
                    cancel,
                    ct
                )
            )
        }
    end
