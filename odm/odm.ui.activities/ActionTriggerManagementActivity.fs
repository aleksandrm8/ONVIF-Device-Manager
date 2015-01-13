namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Linq
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    //open System.Disposables
    
    open Microsoft.Practices.Unity

    //open odm.models
    open odm.onvif
    open odm.core
    open odm.infra

    open onvif.services
    open onvif.utils

    open utils
    open utils.fsharp
    open odm.ui
    
    type ActionTriggerManagementActivityResult =
        |Refresh
        |Close
    
    type ActionTriggerManagementActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)

        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        let eventEngine = session :> IEventAsync
        let actionEngine = session :> IActionEngineAsync

        let load() = async{
            let! actionTriggers, actions, eventProperties = Async.Parallel(
                actionEngine.GetActionTriggers(),
                actionEngine.GetActions(),
                eventEngine.GetEventProperties()
            )

            return new ActionTriggersView.Model(
                triggers = actionTriggers,
                selection = null,
                actions = actions,
                topicSet = eventProperties.TopicSet
            )
        }
        member private this.Main() = async{
            let! cont = async{
                try
                    let! model = async{
                        use! progress = Progress.Show(ctx, "loading...")
                        return! load()
                    }
                    return this.ShowForm(model)
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let rec show()  = async{
                        let! res = ActionTriggersView.Show(ctx, model)

                        return! res.Handle(
                            create = (fun m ->async{
                                //create a new action trigger
                                let! r = ActionTriggerModifyView.Show(
                                    ctx, 
                                    new ActionTriggerModifyView.Model(
                                        actions = m.actions,
                                        topicSet = m.topicSet
                                    )
                                )
                                return r.Handle(
                                    apply = (fun m -> this.CreateActionTrigger(m) ),
                                    close = (fun m -> this.Main()),
                                    cancel = (fun m -> this.Main())
                                )
                            }),
                            delete = (fun m -> async{
                                //delete a selected action trigger
                                return this.DeleteActionTrigger(m.selection)
                            }),
                            modify = (fun m -> async{
                                //modify a selected action trigger
                                let! r = ActionTriggerModifyView.Show(
                                    ctx, 
                                    new ActionTriggerModifyView.Model(
                                        trigger = m.selection,
                                        actions = m.actions,
                                        topicSet = m.topicSet
                                    )
                                )
                                return r.Handle(
                                    apply = (fun m -> this.ModifyActionTrigger(m)),
                                    close = (fun m -> this.Main()),
                                    cancel = (fun m -> this.Main())
                                )
                            }),
                            close = (fun m -> async{
                                return this.Complete(ActionTriggerManagementActivityResult.Close)
                            })
                        )
                    }
                    return! show()

                with err ->
                   do! show_error(err)
                   return this.ShowForm(model)
            }
            return! cont
        }

        member private this.CreateActionTrigger(model) = async{
            try
                use! progress = Progress.Show(ctx, "creating a new action trigger...")
                do! actionEngine.CreateActionTriggers([| model.trigger.Configuration |]) |> Async.Ignore
            with err -> 
                dbg.Error(err)
                do! ErrorView.Show(ctx, err) |> Async.Ignore
            return! this.Main()
        }

        member private this.ModifyActionTrigger(model) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, "configuring the action trigger...")
                        do! actionEngine.ModifyActionTriggers([| model.trigger |])
                    }
                    return this.Main()
                with err -> 
                    do! show_error(err)
                    return this.Main()
                }
            return! cont
        }

        member private this.DeleteActionTrigger(actionTrigger:ActionTrigger) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, "deleting the action trigger...")
                        do! actionEngine.DeleteActionTriggers( [| actionTrigger.Token |])
                    }
                    return this.Main()
                with err -> 
                    do! show_error(err)
                    return this.Main()
                }
            return! cont
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx) = 
            let act = new ActionTriggerManagementActivity(ctx) 
            act.Main()
    end
