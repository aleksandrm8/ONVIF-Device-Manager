namespace utils
    open System
    open System.Collections.Generic
    open System.Threading
    open System.Threading.Tasks
    open System.Reactive.Disposables
    open System.Reactive.Concurrency
    //open utils

    type Attempt<'T> = 
        |Success of 'T
        |Error of Exception

    type Trampoline() = class
        let mutable acquired = false
        let queue = new Queue<(unit->unit)>()

        let rec ProcessQueue() = 
            let cont = lock queue (fun()->
                if queue.Count > 0 then
                    let act = queue.Dequeue()
                    (fun()->
                        try act() with err -> dbg.Error(err)
                        true
                    )
                else
                    acquired <- false
                    (fun()->false)
            )
            if cont() then  ProcessQueue()

        member this.Drop (act) =
            let acquirer = lock queue (fun()->
                queue.Enqueue(act)
                let acquirer = not acquired
                acquired <- true
                acquirer
            )
            if acquirer then ProcessQueue()
    end

//    type TrampolineScheduler() = class
//        let tramp = new Trampoline()
//        
//        interface IScheduler with
//            member s.Schedule(act:Action) = 
//                let disp = new BooleanDisposable()
//                tramp.Drop(fun()->
//                    if not disp.IsDisposed then act.Invoke()
//                )
//                disp :> IDisposable
//            member s.Schedule(act:Action, span:TimeSpan) = 
//                let disp = new MutableDisposable()
//                tramp.Drop(fun()->
//                    disp.Disposable<-Scheduler.ThreadPool.Schedule((fun()->
//                        tramp.Drop(fun()->
//                            disp.Disposable<- (s:>IScheduler).Schedule(act)
//                        )
//                    ),span)
//                )
//                disp :> IDisposable
//            member s.Now = Scheduler.ThreadPool.Now
//        end        
//    end