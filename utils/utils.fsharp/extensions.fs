module utils.fsharp
    open System
    open System.Collections
    open System.Collections.Generic
    open System.Linq
    open System.Reactive.Concurrency
    open System.Reactive.Disposables
    open System.Threading
    open System.Threading.Tasks
    //open utils
    
    let inline CastAs(o:obj) = 
        match o with
        | :? ^T as res -> res
        | _ -> null
    
    let inline CreateCompletionPoint f = 
        let handler = ref (Some f)
        fun h ->
            match Interlocked.Exchange<_>(handler, None) with
            | Some f -> f h
            | None -> ()
    
    let inline IsNull (value:^a when ^a: not struct and ^a:null) = obj.ReferenceEquals(value, null)
    let inline NotNull (value:^a when ^a:not struct and ^a:null) = not(value |> IsNull)
    let inline IfNotNull c v = if NotNull(v) then c(v) else null
    
    let inline Suppress exclusion substitute value  = 
        if value<>exclusion then value else substitute

    let inline SuppressNull substitute value = 
        if IsNull(value) then substitute else value
    

    type ObservedEvent<'T> = 
    | Notification of 'T
    | Completion
    | Failure of Exception

//    let inline Trigger f = 
//        let handler = ref (Some f)
//        fun arg ->
//            match Interlocked.Exchange<_>(handler, None) with
//            | Some h -> h arg
//            | None -> ()
    
    ///<summary>Math extensions</summary>
    type Math = class
        static member Coerce min max value = 
            if value < min then min
            elif value > max then max
            else value
    end

    ///<summary></summary>
    type AsyncObserver<'T> = ('T->unit)*(Exception->unit)
    
    [<AllowNullLiteral>]
    type IAsyncObserver<'T> = interface
        abstract OnSuccess: result:'T -> unit
        abstract OnError: error:Exception -> unit
        abstract OnCancel: error:OperationCanceledException -> unit
    end

    type IAsyncSink<'T> = interface
        abstract Success: result:'T -> bool
        abstract Error: error:Exception -> bool
        abstract Cancel: error:OperationCanceledException -> bool
    end

    type internal AsyncResult<'T> = 
        | Succeeded of 'T
        | Failed of Exception
        | Canceled of OperationCanceledException

    type internal AsyncGate<'T> = 
        | Completed of AsyncResult<'T>
        | Subscribed of IAsyncObserver<'T>
        | Started
        | Notified
    
    type internal MemoizedAsyncState<'TResult> =
        |Completed of 'TResult
        |Failed of Exception
        |Canceled of OperationCanceledException
        |Processing of LinkedList<IAsyncSink<'TResult>>
        |Idle
    
    type CancelationPoint = 
        | Canceled
        | Subscribed of (unit->unit)
        | Initialized

    ///<summary>Async extensions</summary>
    type global.Microsoft.FSharp.Control.Async with
        //static new() = ()
        //static let inline a = ()
        static member inline Map cont comp = async{
            let! res = comp
            return cont(res)
        }

        static member inline Zip(c1,c2) = async{
            let! r1 = c1
            let! r2 = c2
            return (r1, r2)
        }

        static member inline Zip(c1,c2,c3) = async{
            let! r1 = c1
            let! r2 = c2
            let! r3 = c3
            return (r1, r2, r3)
        }

        static member inline Zip(c1,c2,c3,c4) = async{
            let! r1 = c1
            let! r2 = c2
            let! r3 = c3
            let! r4 = c4
            return (r1, r2, r3, r4)
        }

        static member inline Zip(c1,c2,c3,c4,c5) = async{
            let! r1 = c1
            let! r2 = c2
            let! r3 = c3
            let! r4 = c4
            let! r5 = c5
            return (r1, r2, r3, r4, r5)
        }

        static member inline Zip(c1,c2,c3,c4,c5,c6) = async{
            let! r1 = c1
            let! r2 = c2
            let! r3 = c3
            let! r4 = c4
            let! r5 = c5
            let! r6 = c6
            return (r1, r2, r3, r4, r5, r6)
        }

        static member inline Zip(c1,c2,c3,c4,c5,c6,c7) = async{
            let! r1 = c1
            let! r2 = c2
            let! r3 = c3
            let! r4 = c4
            let! r5 = c5
            let! r6 = c6
            let! r7 = c7
            return (r1, r2, r3, r4, r5, r6, r7)
        }

        static member inline Zip(c1,c2,c3,c4,c5,c6,c7,c8) = async{
            let! r1 = c1
            let! r2 = c2
            let! r3 = c3
            let! r4 = c4
            let! r5 = c5
            let! r6 = c6
            let! r7 = c7
            let! r8 = c8
            return (r1, r2, r3, r4, r5, r6, r7, r8)
        }

        static member inline Zip(c1,c2,c3,c4,c5,c6,c7,c8,c9) = async{
            let! r1 = c1
            let! r2 = c2
            let! r3 = c3
            let! r4 = c4
            let! r5 = c5
            let! r6 = c6
            let! r7 = c7
            let! r8 = c8
            let! r9 = c9
            return (r1, r2, r3, r4, r5, r6, r7, r8, r9)
        }

        ///<summary></summary>
        static member inline SleepEx(milliseconds:int) = async{
            if milliseconds > 0 then
                let disp = new SerialDisposable()
                use! ch = Async.OnCancel(fun()->disp.Dispose())
                do! Async.FromContinuations(fun (success, error, cancel) ->
                    let timerSubscription = new SerialDisposable()
                    let CompleteWith = CreateCompletionPoint(fun cont ->
                        timerSubscription.Dispose()
                        cont()
                    )

                    disp.Disposable <- Disposable.Create(fun()->
                        CompleteWith (fun ()-> cancel(new OperationCanceledException()))
                    )
                    let tmr = new Timer(
                        callback = (fun state -> CompleteWith(success)), 
                        state = null, dueTime = milliseconds, period = Timeout.Infinite
                    )
                    if tmr |> IsNull then
                        CompleteWith(fun ()->error(new Exception("failed to create timer")))
                    else
                        timerSubscription.Disposable <- Disposable.Create(fun()->
                            try tmr.Dispose() with _ -> ()
                        )
                )
            else
                return ()
        }

        static member StartChildEx (comp:Async<'TRes>):Async<Async<'TRes>> = async{
            let! ct = Async.CancellationToken
            
            let gate = ref AsyncGate.Started
            let CompleteWith(result:AsyncResult<'T>, callbacks:IAsyncObserver<'T>) =
                let notify() = 
                    match result with
                        | AsyncResult.Succeeded v -> callbacks.OnSuccess(v)
                        | AsyncResult.Failed e -> callbacks.OnError(e)
                        | AsyncResult.Canceled e -> callbacks.OnCancel(e)
                match Interlocked.Exchange(gate, Notified) with 
                    | Notified -> ()
                    | _ -> notify()

            let ProcessResults (result:AsyncResult<'TRes>) =
                let t = Interlocked.CompareExchange<AsyncGate<'TRes>>(gate, AsyncGate.Completed(result), AsyncGate.Started)
                match t with
                | AsyncGate.Subscribed callbacks -> 
                    CompleteWith(result, callbacks)
                | _ -> ()
            let Subscribe (success, error, cancel) = 
                let callbacks = {
                    new IAsyncObserver<'TRes> with
                        member this.OnSuccess v = success v
                        member this.OnError e = error e
                        member this.OnCancel e = cancel e
                }
                let t = Interlocked.CompareExchange<AsyncGate<'TRes>>(gate, AsyncGate.Subscribed(callbacks), AsyncGate.Started)
                match t with
                | AsyncGate.Completed result -> 
                    CompleteWith(result, callbacks)
                | _ -> ()

            Async.StartWithContinuations(
                computation = comp,
                continuation = (fun v -> ProcessResults(AsyncResult.Succeeded(v))),
                exceptionContinuation = (fun e -> ProcessResults(AsyncResult.Failed(e))),
                cancellationContinuation = (fun e -> ProcessResults(AsyncResult.Canceled(e))),
                cancellationToken = ct
            )
            return Async.FromContinuations( fun (success, error, cancel) ->
                Subscribe(success, error, cancel)
            )
        }
    
        static member inline CreateWithCancellation(factory) = async{
            let! ct = Async.CancellationToken
            return! Async.FromContinuations(fun cb ->
                let callbacks = ref <| Some cb
                let inline CompleteWith cont = fun result ->
                    match Interlocked.Exchange<_>(callbacks, None) with
                    | Some cb ->
                        cont cb result
                        true
                    | _ -> 
                        false
                let sink = {
                    new IAsyncSink<_> with
                        member this.Success v = CompleteWith (fun (sc, ec, cc) -> sc) v
                        member this.Error e = CompleteWith (fun (sc, ec, cc) -> ec) e
                        member this.Cancel e = CompleteWith (fun (sc, ec, cc) -> cc) e
                }
                factory(sink, ct)
            )
        }

        static member inline CreateWithCancellation1(factory) = async{
            let! ct = Async.CancellationToken
            return! Async.FromContinuations(fun (success, error, cancel) ->
                let callbacks = ref <| Some (success, error, cancel)
                let inline CompleteWith cont = (fun result ->
                    match Interlocked.Exchange<_>(callbacks, None) with
                    | Some cb  ->
                        cont cb result
                        true
                    | _ -> 
                        false
                )
                factory(
                    (fun v -> CompleteWith(fun (sc, ec, cc) -> sc) v ),
                    (fun e -> CompleteWith(fun (sc, ec, cc) -> ec) e ),
                    (fun e -> CompleteWith(fun (sc, ec, cc) -> cc) e ),
                    ct
                )
            )
        }

        ///<summary></summary>
        static member inline CreateWithAbortHandler1(factory) = async{
            let! ct = Async.CancellationToken
            return! Async.FromContinuations(fun (success, error, cancel)->
                let cp = ref (CancelationPoint.Initialized)
                let cr = ct.Register(fun () ->
                    match Interlocked.Exchange(cp, CancelationPoint.Canceled) with
                    | CancelationPoint.Subscribed ch -> ch()
                    | _->()
                )
                let callbacks = ref <| Some (success, error, cancel)
                let inline CompleteWith cont = fun result ->
                    match Interlocked.Exchange<_>(callbacks, None) with
                    | Some cb ->
                        cr.Dispose()
                        cont cb result
                        true
                    | _ -> 
                        false
                let success = CompleteWith (fun (sc, ec, cc) -> sc)
                let error = CompleteWith (fun (sc, ec, cc) -> ec)
                let cancel = CompleteWith (fun (sc, ec, cc) -> cc)
                
                let abort = factory (success, error, cancel)
                let ch () = 
                    if cancel(new OperationCanceledException()) then
                        try abort() with e -> dbg.Error(e)
                
                match Interlocked.CompareExchange(cp, CancelationPoint.Subscribed(ch), CancelationPoint.Initialized) with
                |CancelationPoint.Canceled -> ch()
                |CancelationPoint.Subscribed _ -> dbg.Error("failed to subscribe to cancelation point")
                |_ -> ()
            )
        }

        ///<summary></summary>
        static member inline CreateWithDisposable1(factory:_->IDisposable) = async{
            let! ct = Async.CancellationToken
            return! Async.FromContinuations(fun (success, error, cancel)->
                let cp = ref (CancelationPoint.Initialized)
                let cr = ct.Register(fun () ->
                    match Interlocked.Exchange(cp, CancelationPoint.Canceled) with
                    | CancelationPoint.Subscribed ch -> ch()
                    | _->()
                )
                let callbacks = ref <| Some (success, error, cancel)
                let inline CompleteWith cont = fun result ->
                    match Interlocked.Exchange<_>(callbacks, None) with
                    | Some cb ->
                        cr.Dispose()
                        cont cb result
                        true
                    | _ -> 
                        false
                let success = CompleteWith (fun (sc, ec, cc) -> sc)
                let error = CompleteWith (fun (sc, ec, cc) -> ec)
                let cancel = CompleteWith (fun (sc, ec, cc) -> cc)
                
                let disp = factory (
                    success,
                    error,
                    cancel
                )

                let ch () = 
                    if cancel (new OperationCanceledException()) then
                        try disp.Dispose() with e -> dbg.Error(e)
                
                match Interlocked.CompareExchange(cp, CancelationPoint.Subscribed(ch), CancelationPoint.Initialized) with
                |CancelationPoint.Canceled -> ch()
                |CancelationPoint.Subscribed _ -> dbg.Error("failed to subscribe to cancelation point")
                |_ -> ()
            )
        }
    
        ///<summary></summary>
        static member inline CreateWithDisposable(factory:_->IDisposable) = async{
            let! ct = Async.CancellationToken
            return! Async.FromContinuations(fun (success, error, cancel)->
                let cp = ref (CancelationPoint.Initialized)
                let cr = ct.Register(fun () ->
                    match Interlocked.Exchange(cp, CancelationPoint.Canceled) with
                    | CancelationPoint.Subscribed ch -> ch()
                    | _->()
                )
                let callbacks = ref <| Some (success, error, cancel)
                let inline CompleteWith cont = fun result ->
                    match Interlocked.Exchange<_>(callbacks, None) with
                    | Some cb ->
                        cr.Dispose()
                        cont cb result
                        true
                    | _ -> 
                        false
                
                let disp = factory {
                    new IAsyncSink<_> with
                        member this.Success v = CompleteWith (fun (sc, ec, cc) -> sc) v
                        member this.Error e = CompleteWith (fun (sc, ec, cc) -> ec) e
                        member this.Cancel e = CompleteWith (fun (sc, ec, cc) -> cc) e
                }

                let ch () = 
                    if CompleteWith (fun (sc, ec, cc) -> cc) (new OperationCanceledException()) then
                        try disp.Dispose() with e -> dbg.Error(e)
                
                match Interlocked.CompareExchange(cp, CancelationPoint.Subscribed(ch), CancelationPoint.Initialized) with
                |CancelationPoint.Canceled -> ch()
                |CancelationPoint.Subscribed _ -> dbg.Error("failed to subscribe to cancelation point")
                |_ -> ()
            )
        }

        ///<summary></summary>
        static member inline CreateWithAbortHandler(factory:_->Action) = Async.CreateWithDisposable(fun sink ->
            match factory(sink) with
            | null -> null
            | ch -> {new IDisposable with member d.Dispose() = ch.Invoke()}
        )

        ///<summary></summary>
        static member StartChilds(c1, c2) = Async.Zip(
            Async.StartChildEx(c1),
            Async.StartChildEx(c2)
        )

        ///<summary></summary>
        static member StartChilds(c1, c2, c3) = Async.Zip(
            Async.StartChildEx(c1),
            Async.StartChildEx(c2),
            Async.StartChildEx(c3)
        )

        ///<summary></summary>
        static member StartChilds(c1, c2, c3, c4) = Async.Zip(
            Async.StartChildEx(c1),
            Async.StartChildEx(c2),
            Async.StartChildEx(c3),
            Async.StartChildEx(c4)
        )

        ///<summary></summary>
        static member StartChilds(c1, c2, c3, c4, c5) = Async.Zip(
            Async.StartChildEx(c1),
            Async.StartChildEx(c2),
            Async.StartChildEx(c3),
            Async.StartChildEx(c4),
            Async.StartChildEx(c5)
        )

        ///<summary></summary>
        static member StartChilds(c1, c2, c3, c4, c5, c6) = Async.Zip(
            Async.StartChildEx(c1),
            Async.StartChildEx(c2),
            Async.StartChildEx(c3),
            Async.StartChildEx(c4),
            Async.StartChildEx(c5),
            Async.StartChildEx(c6)
        )

        ///<summary></summary>
        static member inline StartChilds(c1, c2, c3, c4, c5, c6, c7) = Async.Zip(
            Async.StartChildEx(c1),
            Async.StartChildEx(c2),
            Async.StartChildEx(c3),
            Async.StartChildEx(c4),
            Async.StartChildEx(c5),
            Async.StartChildEx(c6),
            Async.StartChildEx(c7)
        )

        ///<summary></summary>
        static member inline StartChilds(c1, c2, c3, c4, c5, c6, c7, c8) = Async.Zip(
            Async.StartChildEx(c1),
            Async.StartChildEx(c2),
            Async.StartChildEx(c3),
            Async.StartChildEx(c4),
            Async.StartChildEx(c5),
            Async.StartChildEx(c6),
            Async.StartChildEx(c7),
            Async.StartChildEx(c8)
        )

        ///<summary></summary>
        static member inline StartChilds(c1, c2, c3, c4, c5, c6, c7, c8, c9) = Async.Zip(
            Async.StartChildEx(c1),
            Async.StartChildEx(c2),
            Async.StartChildEx(c3),
            Async.StartChildEx(c4),
            Async.StartChildEx(c5),
            Async.StartChildEx(c6),
            Async.StartChildEx(c7),
            Async.StartChildEx(c8),
            Async.StartChildEx(c9)
        )

        static member inline Parallel(comp1, comp2) = async{
            let! comp = Async.StartChilds(comp1, comp2)
            return! comp |> Async.Zip 
        }

        ///<summary></summary>
        static member inline Parallel(comp1, comp2, comp3) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3)
            return! comp |> Async.Zip 
        }
    
        ///<summary></summary>
        static member inline Parallel(comp1, comp2, comp3, comp4) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4)
            return! comp |> Async.Zip 
        }
    
        ///<summary></summary>
        static member inline Parallel(comp1, comp2, comp3, comp4, comp5) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5)
            return! comp |> Async.Zip 
        }
    
        ///<summary></summary>
        static member inline Parallel(comp1, comp2, comp3, comp4, comp5, comp6) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5, comp6)
            return! comp |> Async.Zip 
        }
    
        ///<summary></summary>
        static member inline Parallel(comp1, comp2, comp3, comp4, comp5, comp6, comp7) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5, comp6, comp7)
            return! comp |> Async.Zip 
        }
    
        ///<summary></summary>
        static member inline Parallel(comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8)
            return! comp |> Async.Zip 
        }
        
        ///<summary></summary>
        static member inline Parallel(comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8, comp9) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8, comp9)
            return! comp |> Async.Zip 
        }

        ///<summary></summary>
        static member Memoize(comp:Async<'T>):Async<'T> = 
            let tramp = new Trampoline()
            let state = ref MemoizedAsyncState<'T>.Idle
            let Subscribe (observers:LinkedList<IAsyncSink<'T>>) (observer:IAsyncSink<'T>) =
                let node = observers.AddLast(observer)
                Disposable.Create(fun()->
                    tramp.Drop(fun()->
                        observers.Remove(node)
                    )
                )
            let StartComp() =
                let subscribers = new LinkedList<IAsyncSink<'T>>()
                let CompCompleted(result: 'T) = tramp.Drop(fun()->
                    state := Completed(result)
                    for cb in subscribers do
                        cb.Success(result) |> ignore
                    subscribers.Clear()
                )
                let CompFailed(error: Exception) = tramp.Drop(fun()->
                    state := Failed(error)
                    for cb in subscribers do
                        cb.Error(error) |> ignore
                    subscribers.Clear()
                )
                let CompCanceled(error: OperationCanceledException) = tramp.Drop(fun()->
                    state := Failed(error)
                    for cb in subscribers do
                        cb.Cancel(error) |> ignore
                    subscribers.Clear()
                )
                Async.StartWithContinuations(
                    comp,
                    CompCompleted,
                    CompFailed,
                    CompCanceled
                )
                state := Processing(subscribers)
                subscribers
            
            Async.CreateWithDisposable(fun callbacks ->
                let disp = new SingleAssignmentDisposable()
                tramp.Drop(fun()->
                    match !state with
                    |MemoizedAsyncState.Idle ->
                        let subsribers = StartComp()
                        disp.Disposable <- callbacks |> Subscribe subsribers
                    |MemoizedAsyncState.Processing subsribers ->
                        disp.Disposable <- callbacks |> Subscribe subsribers
                    |MemoizedAsyncState.Completed v ->
                        callbacks.Success(v) |> ignore
                    |MemoizedAsyncState.Canceled e ->
                        callbacks.Cancel(e) |> ignore
                    |MemoizedAsyncState.Failed e ->
                        callbacks.Error(e) |> ignore
                )
                disp :> IDisposable
            )

        static member Race(comps:seq<Async<'TRes>>) = async{
            let! ct = Async.CancellationToken
            return! Async.FromContinuations(fun (success, error, cancel) ->
                let cts = new CancellationTokenSource()
                let completed = ref false
                let tramp = new Trampoline()
                let cs = new SerialDisposable()

                let CompleteWith cont = 
                    if not(!completed) then
                        completed := true
                        cont()
                        cts.Cancel()
                        cs.Dispose()

                cs.Disposable <- ct.Register(fun ()->
                    tramp.Drop(fun ()-> 
                        CompleteWith(fun ()-> cancel (new OperationCanceledException()))
                    )
                )

                let runningTasks = ref 0
                let last_error = ref None
                tramp.Drop(fun ()->
                    for comp in comps do
                        runningTasks := !runningTasks + 1
                        Async.StartWithContinuations(
                            comp,
                            (fun v -> tramp.Drop( fun ()->
                                runningTasks := !runningTasks - 1
                                CompleteWith(fun()->
                                    success (Some v)
                                )
                            )),
                            (fun e -> tramp.Drop( fun ()-> 
                                runningTasks := !runningTasks - 1
                                if !runningTasks = 0 then
                                    CompleteWith(fun()-> error e)
                                else
                                    last_error := Some e
                            )),
                            (fun e -> tramp.Drop( fun ()->
                                runningTasks := !runningTasks - 1
                                if !runningTasks = 0 then
                                    CompleteWith(fun()-> error e)
                                else
                                    last_error := Some (e:>Exception)
                            )),
                            cts.Token
                        )
                    if !runningTasks = 0 then
                        CompleteWith(fun()-> success None)
                )
//                let itor = comps.GetEnumerator()
//                let rec loop () = 
//                    tramp.Drop(fun ()->
//                        if itor.MoveNext() && not(!completed) then
//                            runningTasks := !runningTasks + 1
//                            Async.StartWithContinuations(
//                                itor.Current,
//                                (fun v -> tramp.Drop( fun ()->
//                                    runningTasks := !runningTasks - 1
//                                    CompleteWith(fun()->
//                                        success (Some v)
//                                    )
//                                )),
//                                (fun e -> tramp.Drop( fun ()-> 
//                                    runningTasks := !runningTasks - 1
//                                    if !runningTasks = 0 then
//                                        CompleteWith(fun()-> error e)
//                                    else
//                                        last_error := Some e
//                                )),
//                                (fun e -> tramp.Drop( fun ()->
//                                    runningTasks := !runningTasks - 1
//                                    if !runningTasks = 0 then
//                                        CompleteWith(fun()-> error e)
//                                    else
//                                        last_error := Some (e:>Exception)
//                                )),
//                                cts.Token
//                            )
//                            loop()
//                        else
//                            itor.Dispose()
//                    )
//                loop()
//                tramp.Drop(fun()->
//                    runningTasks := !runningTasks - 1
//                    if !runningTasks = 0 then
//                        match !last_error with
//                        | None -> 
//                            CompleteWith(fun()->
//                                success None
//                            )
//                        | Some e -> 
//                            CompleteWith(fun()->
//                                error e
//                            )
//                )
            )
        }

//        Async.FromContinuations(fun (success, error, cancel) ->
//            
//            let attempts = ref 1
//            let completed = ref false
//                let tramp = new Trampoline()
//                for comp in comps do 
//                    tramp.Drop(fun()->
//                        attempts:= !attempts+1
//                        //Interlocked.Increment(attempts)
//                        let session = this.CreateSession(uri)
//                        let dev = session :> IDeviceAsync
//                        Async.StartWithContinuations(
//                            dev.GetCapabilities(),
//                            (fun v-> tramp.Drop(fun ()->
//                                if not !completed then
//                                    completed:=true
//                                    try success session with err->Assert.FailWithError err
//                                    cts.Cancel()
//                            )),
//                            (fun e->tramp.Drop(fun()->
//                                attempts:= !attempts - 1 
//                                if !attempts = 0 && not !completed then
//                                    completed:=true
//                                    try error e with err->Assert.FailWithError err
//                            )),
//                            (fun c->tramp.Drop(fun()->
//                                attempts:= !attempts - 1
//                                if !attempts = 0 && not !completed then
//                                    completed:=true
//                                    try cancel c with err->Assert.FailWithError err
//                            )),
//                            cts.Token
//                        )
//                    )
//                tramp.Drop(fun()->
//                    attempts:= !attempts - 1
//                    if !attempts = 0 && not !completed then
//                        completed:=true
//                        try 
//                            error <| new Exception("failed to create session") 
//                        with 
//                            err->Assert.FailWithError err
//                )
//            )
//            let IsLinkLocalAddress(uri:Uri) = 
//                let ipAddr = 
//                    match uri.HostNameType with
//                    |UriHostNameType.IPv4 -> IPAddress.Parse(uri.Host)
//                    |UriHostNameType.IPv6 -> IPAddress.Parse(uri.Host)
//                    |_->null
//                if ipAddr |> IsNull then
//                    false
//                else
//                    let addrBytes = ipAddr.GetAddressBytes()
//                    ipAddr.IsIPv6LinkLocal || 
//                    (ipAddr.AddressFamily = AddressFamily.InterNetwork && addrBytes.[0] = 169uy && addrBytes.[1] = 254uy)
//            return! TryCreateSession(uris)
//            Async.Race

    end


    type AsyncSemaphore(initialCnt) = class
        let gate = new obj()
        let list = new LinkedList<unit->bool>()
        let cnt = ref initialCnt
        member this.Wait(wasAcquired) = Async.CreateWithDisposable(fun sink ->
            let cont = lock gate (fun () -> 
                if !cnt > 0 then
                    cnt := !cnt - 1
                    fun() -> 
                        if sink.Success() then
                            wasAcquired := true
                        else
                            this.Release()
                        Disposable.Empty
                else
                    let node = list.AddLast(fun () ->
                        if sink.Success() then
                            wasAcquired := true
                            true
                        else
                            false
                    )
                    fun() -> 
                        Disposable.Create(fun () ->
                            lock gate ( fun () ->
                                if node.List |> NotNull then
                                    list.Remove(node)
                            )
                        )
            )
            cont()
        )
        member this.Release() = 
            let first = lock gate (fun() ->
                let first = list.First
                if first |> NotNull then
                    list.RemoveFirst()
                else
                    cnt := !cnt + 1
                first
            )
            if first |> NotNull then
                let success = first.Value
                if not(success()) then
                    this.Release()
    end

    ///<summary>IScheduler extensions</summary>
    type IScheduler with
        
        ///<summary></summary>
        member this.InvokeAsync f = async{
            let disp = new SingleAssignmentDisposable()
            use! onCancel = Async.OnCancel(fun ()->
                disp.Dispose()
            )
            return! Async.FromContinuations(fun (success, error, cancel)->
                let CompleteWith = CreateCompletionPoint(fun cont ->
                    cont()
                )
                let d = new SingleAssignmentDisposable()
                disp.Disposable <- Disposable.Create(fun()->
                    d.Dispose()
                    CompleteWith (fun ()-> cancel (new OperationCanceledException()))
                )
                d.Disposable <- this.Schedule(fun()->
                    CompleteWith (fun ()->
                        let cont = 
                            try
                                let res = f()
                                (fun ()-> success res)
                            with
                                err->(fun()->error err)
                        cont()
                    )
                )
            )
        }
    end
    
    type System.Windows.Threading.Dispatcher with
        member this.InvokeAsync f = async{
            let disp = new SerialDisposable()
            use! cancellation = Async.OnCancel(fun()->
                disp.Dispose()
            )
            return! Async.FromContinuations(fun (success, error, cancel) ->
                let CompleteWith = CreateCompletionPoint(fun cont -> cont())
                let op = this.BeginInvoke(new Action(fun()->
                    CompleteWith(fun()->
                        let cont = 
                            try 
                                let result = f()
                                (fun ()-> success result)
                            with e -> 
                                (fun ()-> error e)
                        cont()
                    )
                ))
                if op |> NotNull then
                    disp.Disposable <- Disposable.Create(fun()->
                        CompleteWith(fun()-> 
                            cancel(new OperationCanceledException())
                            try op.Abort() |> ignore with e -> dbg.Error(e)
                        )
                    )
                else
                    CompleteWith(fun()-> error(new Exception("failed to dispatch action")))
            )
        }
    end

    type IObservable<'T> with
        member o.GetEventAsync() = Async.CreateWithDisposable(fun sink ->
            let subscription = new SerialDisposable()
            let CompleteWith evt =
                if sink.Success(evt) then
                    subscription.Dispose()

            subscription.Disposable <- o.Subscribe(
                (fun v-> 
                    CompleteWith (ObservedEvent.Notification(v))
                ),
                (fun err-> 
                    CompleteWith (ObservedEvent.Failure(err))
                ),
                (fun ()-> 
                    CompleteWith (ObservedEvent.Completion)
                )
            )
            {new IDisposable with
                member d.Dispose() = 
                    subscription.Dispose()
            }
        )
    end
