namespace utils
    open System
    open System.Collections.Generic
    open System.Linq
    open System.Threading
    open System.Threading.Tasks
    //open System.Windows.Threading
    open System.Runtime.CompilerServices
    open System.Reactive.Concurrency
    open System.Reactive.Disposables
    //open utils
    open utils.fsharp

    type AsyncState<'T> = 
        |Success of 'T
        |Error of Exception
        |Cancel
        |Processing
        |Idle
    

    [<Extension>]
    type Apm private() = class
      
//        static member CreateAnonymousObserver<'T>(onSuccess:Action<'T>, onError:Action<Exception>, onCancel:Action): IAsyncObserver<'T> = {
//            new IAsyncObserver<'T> with
//                member this.OnSuccess(result) =  onSuccess.Invoke(result)
//                member this.OnError(err) = onError.Invoke(err)
//                member this.OnCancel() = onCancel.Invoke()
//            end
//        }
//        
//        static member CreateAnonymousObserver(callback:Action<Func<'T>>): IAsyncObserver<'T> = {
//            new IAsyncObserver<'T> with
//                member this.OnSuccess(result) =  callback.Invoke(fun () -> result)
//                member this.OnError(err) = callback.Invoke(fun ()-> raise err)
//                member this.OnCancel() = callback.Invoke(fun ()-> raise <| new OperationCanceledException())
//            end
//        }
//
//        static member CreateScheduledObserver(observer:IAsyncObserver<'T>, scheduler: IScheduler): IAsyncObserver<'T> = {
//            new IAsyncObserver<'T> with
//                member this.OnSuccess(result) = 
//                    scheduler.Schedule(fun ()-> observer.OnSuccess(result)) |> ignore
//                member this.OnError(err) = 
//                    scheduler.Schedule(fun ()-> observer.OnError(err)) |> ignore
//                member this.OnCancel() = 
//                    scheduler.Schedule(fun ()-> observer.OnCancel()) |> ignore
//            end
//        }

        static member Create<'T>(factory: Func<IAsyncSink<'T>, IDisposable>): Async<'T> = 
            Async.CreateWithDisposable(fun sink -> factory.Invoke sink)

        static member Create<'T>(factory: Func<Action<'T>,Action<Exception>,Action, IEnumerable<Async<unit>>>): Async<'T> =
            async{
                return! Async.FromContinuations(fun (success, error, cancel)->
                    let cts = new CancellationTokenSource()
                    
                    let CompleteWith = CreateCompletionPoint (fun cont ->
                        cts.Cancel()
                        cont()
                    )

                    let steps = 
                        factory.Invoke(
                            (fun v->CompleteWith(fun()->success v)), 
                            (fun e->CompleteWith(fun()->error e)), 
                            (fun()->CompleteWith(fun()->cancel(new OperationCanceledException()))) 
                        )
                    let comp = async{
                        for x in steps do
                            do! x
                    }
                    Async.StartWithContinuations(comp,
                        (fun ()->()),
                        (fun e->CompleteWith(fun()->error e)),
                        (fun c->CompleteWith(fun()->error c)),
                        cts.Token
                    )
                )
            }
        static member Defer<'T>(factory:Func<Async<'T>>):Async<'T> = 
            async{
                return! factory.Invoke()
            }
            
        static member Defer<'T>(factory:Func<Async<'T>>, scheduler:IScheduler):Async<'T> = 
            async{
                let! comp = scheduler.InvokeAsync(fun ()->
                    factory.Invoke()
                )
                return! comp
            }

        static member Return<'T>(v: 'T): Async<'T> = 
            Async.FromContinuations(fun (success, error, cancel)->
                success v
            )
        
        static member Empty(): Async<unit> = 
            Async.FromContinuations(fun (success, error, cancel)->
                success()
            )
        
        static member Error<'T>(err: Exception): Async<'T> = 
            Async.FromContinuations(fun (success, error, cancel)->
                error err
            )
        
        static member Canceled<'T>(): Async<'T> = 
            Async.FromContinuations(fun (success, error, cancel)->
                cancel (new OperationCanceledException())
            )

        static member Do(act:Action): Async<unit> = 
            async{
                return act.Invoke()
            }

        static member Do<'T>(act:Action, scheduler:IScheduler): Async<unit> = 
            scheduler.InvokeAsync(fun ()->
                act.Invoke()
            )

        static member Do<'T>(func: Func<'T>):Async<'T> = 
            async{
                return func.Invoke()
            }
        
        static member Do<'T>(func: Func<'T>, scheduler:IScheduler):Async<'T> = 
            scheduler.InvokeAsync(fun ()->
                func.Invoke()
            )

        static member Sleep(span:TimeSpan):Async<unit> = 
            Async.SleepEx(span.Milliseconds)

        static member Iterate(enumerable:IEnumerable<Async<unit>>): Async<unit> = 
            async{
                for x in enumerable do
                    do! x
            }
           
            
//        static member Iterate<'T>(enumFact:Func<Action<Func<'T>>,IEnumerable<IAsync<Unit>>>):IAsync<'T> = 
//            failwith "not implemented"
        
        static member FromBeginEnd(beginFun:Func<AsyncCallback, obj, IAsyncResult>, endFun:Action<IAsyncResult>): Func<Async<unit>> = 
            let bf(a,b) = beginFun.Invoke(a,b)
            let ef(a) = endFun.Invoke(a)
            new Func<Async<unit>>(fun () -> Async.FromBeginEnd(bf, ef))
        
        static member FromBeginEnd<'TArg1>(beginFun:Func<'TArg1, AsyncCallback, obj, IAsyncResult>, endFun:Action<IAsyncResult>): Func<'TArg1, Async<unit>> = 
            let bf(a,b,c) = beginFun.Invoke(a,b,c)
            let ef(a) = endFun.Invoke(a)
            new Func<'TArg1, Async<unit>>(fun arg1 -> Async.FromBeginEnd(arg1, bf, ef))

        static member FromBeginEnd<'TArg1, 'TArg2>(beginFun:Func<'TArg1, 'TArg2, AsyncCallback, obj, IAsyncResult>, endFun:Action<IAsyncResult>): Func<'TArg1, 'TArg2, Async<unit>> = 
            let bf(a,b,c,d) = beginFun.Invoke(a,b,c,d)
            let ef(a) = endFun.Invoke(a)
            new Func<'TArg1,'TArg2,Async<unit>>(fun arg1 arg2 -> Async.FromBeginEnd(arg1, arg2, bf, ef))
        
        static member FromBeginEnd<'TArg1, 'TArg2, 'TArg3>(beginFun:Func<'TArg1, 'TArg2, 'TArg3, AsyncCallback, obj, IAsyncResult>, endFun:Action<IAsyncResult>): Func<'TArg1, 'TArg2, 'TArg3, Async<unit>> = 
            let bf(a,b,c,d,e) = beginFun.Invoke(a,b,c,d,e)
            let ef(a) = endFun.Invoke(a)
            new Func<'TArg1,'TArg2,'TArg3,Async<unit>>(fun arg1 arg2 arg3 -> Async.FromBeginEnd(arg1, arg2, arg3, bf, ef))

        static member FromBeginEnd<'TResult>(beginFun:Func<AsyncCallback, obj, IAsyncResult>, endFun:Func<IAsyncResult,'TResult>): Func<Async<'TResult>> = 
            let bf(a,b) = beginFun.Invoke(a,b)
            let ef(a) = endFun.Invoke(a)
            new Func<Async<'TResult>>(fun () -> Async.FromBeginEnd(bf, ef))
        
        static member FromBeginEnd<'TArg1,'TResult>(beginFun:Func<'TArg1, AsyncCallback, obj, IAsyncResult>, endFun:Func<IAsyncResult,'TResult>): Func<'TArg1, Async<'TResult>> = 
            let bf(a,b,c) = beginFun.Invoke(a,b,c)
            let ef(a) = endFun.Invoke(a)
            new Func<'TArg1, Async<'TResult>>(fun arg1 -> Async.FromBeginEnd(arg1, bf, ef))
           
        static member FromBeginEnd<'TArg1,'TArg2,'TResult>(beginFun:Func<'TArg1, 'TArg2, AsyncCallback, obj, IAsyncResult>, endFun:Func<IAsyncResult,'TResult>): Func<'TArg1, 'TArg2, Async<'TResult>> = 
            let bf(a,b,c,d) = beginFun.Invoke(a,b,c,d)
            let ef(a) = endFun.Invoke(a)
            new Func<'TArg1,'TArg2,Async<'TResult>>(fun arg1 arg2 -> Async.FromBeginEnd(arg1, arg2, bf, ef))

        static member FromBeginEnd<'TArg1, 'TArg2, 'TArg3,'TResult>(beginFun:Func<'TArg1, 'TArg2, 'TArg3, AsyncCallback, obj, IAsyncResult>, endFun:Func<IAsyncResult,'TResult>): Func<'TArg1, 'TArg2, 'TArg3, Async<'TResult>> = 
            let bf(a,b,c,d,e) = beginFun.Invoke(a,b,c,d,e)
            let ef(a) = endFun.Invoke(a)
            new Func<'TArg1,'TArg2,'TArg3,Async<'TResult>>(fun arg1 arg2 arg3 -> Async.FromBeginEnd(arg1, arg2, arg3, bf, ef))


        [<Extension>]
        static member Invoke<'T>(source:Async<'T>, onSuccess:Action<'T>, onError:Action<Exception>, onCancel:Action): IDisposable = 
            let cts = new CancellationTokenSource()
            let disp = new CancellationDisposable(cts)
            Async.StartWithContinuations(source,
                (fun v->onSuccess.Invoke(v)),
                (fun e->onError.Invoke(e)),
                (fun c->onCancel.Invoke()),
                cts.Token
            )
            disp :> IDisposable

        [<Extension>]
        static member RunSynchronously<'T>(source:Async<'T>): 'T = 
            Async.RunSynchronously(source)

//        [<Extension>]
//        static member Invoke<'T>(source:IAsync<'T>, callback:Action<Func<'T>>): IDisposable = 
//            source.Invoke(Apm.CreateAnonymousObserver(callback))

        [<Extension>]
        static member StartImmediate<'T>(source:Async<'T>, onSuccess:Action<'T>, onError:Action<Exception>, ct:CancellationToken) = 
            Async.StartWithContinuations(source,
                (fun v->onSuccess.Invoke(v)),
                (fun e->onError.Invoke(e)),
                (fun e->onError.Invoke(e)),
                ct
            )


        [<Extension>]
        static member Subscribe<'T>(source:Async<'T>, onSuccess:Action<'T>, onError:Action<Exception>): IDisposable = 
            let gate = new obj()
            let wrapped_observer = ref <| Some {
                new IAsyncObserver<'T> with
                    member o.OnSuccess(result) = onSuccess.Invoke(result)
                    member o.OnError(err) = onError.Invoke(err)
                    member o.OnCancel(err) = onError.Invoke(err)
                end
            }
            let complete_with cont = 
                lock gate (fun () ->
                    match !wrapped_observer with
                    | Some observer -> cont(observer)
                    | None -> ()
                    wrapped_observer := None
                )
                

            let cts = new CancellationTokenSource()
            let disp = new CancellationDisposable(cts)
            Async.StartWithContinuations(source,
                (fun v->complete_with(fun observer -> observer.OnSuccess v)),
                (fun e->complete_with(fun observer -> observer.OnError e)),
                (fun e->complete_with(fun observer -> observer.OnCancel e)),
                cts.Token
            )
            Disposable.Create(fun()->
                lock gate (fun () ->
                    wrapped_observer:=None
                )
                disp.Dispose()
            )

//        [<Extension>]
//        static member Subscribe<'T>(source:Async<'T>, callback:Action<Func<'T>>): IDisposable = 
//            ApmExt.Subscribe(source, 
//                (fun res->callback.Invoke(fun ()->res)),
//                (fun err->callback.Invoke(fun ()->raise err))
//            )

//        [<Extension>]
//        static member Delay<'T>(source: Async<'T>, span: TimeSpan, scheduler: IScheduler): IAsync<'T> = 
//            Apm.create (fun observer -> 
//                let tramp = new Trampoline()
//                let disp = new MutableDisposable()
//                tramp.Drop(fun()->
//                    disp.Disposable <- scheduler.Schedule((fun ()-> 
//                        tramp.Drop(fun()-> disp.Disposable <- source.Invoke(observer))
//                    ),span)
//                )
//                disp :> IDisposable
//            )

        [<Extension>]
        static member Delay<'T>(source: Async<'T>, span: TimeSpan): Async<'T> = 
            async{
                do! Async.SleepEx(span.Milliseconds)
                return! source
            }
        
        [<Extension>]
        static member Select<'T, 'U>(comp: Async<'T>, selector:Func<'T, 'U>): Async<'U> = 
           async{
                let! v = comp
                return selector.Invoke(v)
            }

        [<Extension>]
        static member SelectMany<'T, 'U>(comp: Async<'T> , cont:Func< Async<'T> , Async<'U> >): Async<'U> = 
           async{
                let! branch = Async.StartChildEx(comp)
                return! cont.Invoke(branch)
            }
        
        [<Extension>]
        static member SelectMany<'T, 'U, 'W>(comp:Async<'T>, cont:Func<Async<'T>,Async<'U>>, selector:Func<'T, 'U, 'W>): Async<'W> = 
            async{
                let! branch = Async.StartChildEx(comp)
                let! u = cont.Invoke(branch)
                let! t = branch
                return selector.Invoke(t,u)
            }
        
        [<Extension>]
        static member Ignore<'T>(source: Async<'T>): Async<unit> = 
            Async.Ignore source

        [<Extension>]
        static member Handle<'T>(source: Async<'T>, handler:Action<'T>): Async<unit> = 
            async{
                let! t = source
                return handler.Invoke(t)
            }
            
        [<Extension>]
        static member ObserveOn<'T>(source: Async<'T>, scheduler: IScheduler): Async<'T> = 
            let complete_with cont = 
                scheduler.Schedule(fun()->
                    cont()
                ) |> ignore
            async{
                let! ct = Async.CancellationToken
                return! Async.FromContinuations(fun (onSuccess, onError, onCancel)->
                    Async.StartWithContinuations(source,
                        (fun v->complete_with(fun()->onSuccess v)),
                        (fun e->complete_with(fun()->onError e)),
                        (fun c->complete_with(fun()->onCancel c)),
                        ct
                    )
                )
            }

        [<Extension>]
        static member StartAsTask<'T>(comp: Async<'T>): Task<'T> = 
            let cs = new TaskCompletionSource<'T>()
            Async.StartWithContinuations(
                comp,
                (fun v->cs.SetResult(v)),
                (fun e->cs.SetException(e)),
                (fun c->cs.SetCanceled())
            )
            cs.Task

        [<Extension>]
        static member StartAsTask<'T>(comp: Async<'T>, cancellationToken: CancellationToken): Task<'T> = 
            let cs = new TaskCompletionSource<'T>()
            Async.StartWithContinuations(
                comp,
                (fun v->cs.SetResult(v)),
                (fun e->cs.SetException(e)),
                (fun c->cs.SetCanceled()),
                cancellationToken
            )
            cs.Task

        [<Extension>]
        static member GetAwaiter<'T>(comp: Async<'T>): TaskAwaiter<'T> = 
            Apm.StartAsTask(comp).GetAwaiter()
           
    end

