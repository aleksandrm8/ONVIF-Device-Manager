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
    
    type ActionManagementActivityResult =
        |Refresh
        |Close
    
    type ActionManagementActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)

        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        let actionEngine = session :> IActionEngineAsync

        let load() = async{
            let! actions, supportedActions = Async.Parallel(
                actionEngine.GetActions(),
                actionEngine.GetSupportedActions()
            )
            
            return new ActionsView.Model(
                actions = actions,
                selection = null,
                supportedActions = supportedActions
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
                        let! res = ActionsView.Show(ctx, model)

                        return! res.Handle(
                            create = (fun m ->async{
                                //create a new action
                                let! r = ActionModifyView.Show(
                                    ctx, 
                                    new ActionModifyView.Model(
                                        supportedActions = m.supportedActions
                                    )
                                )
                                return r.Handle(
                                    apply = (fun m -> this.CreateAction(m) ),
                                    close = (fun m -> this.Main()),
                                    cancel = (fun m -> this.Main())
                                )
                            }),
                            delete = (fun m -> async{
                                //delete a selected action
                                return this.DeleteAction(m.selection)
                            }),
                            modify = (fun m -> async{
                                //modify a selected action
                                let! r = ActionModifyView.Show(
                                    ctx, 
                                    new ActionModifyView.Model(
                                        action = m.selection,
                                        supportedActions = m.supportedActions
                                    )
                                )
                                return r.Handle(
                                    apply = (fun m -> this.ModifyAction(m)),
                                    close = (fun m -> this.Main()),
                                    cancel = (fun m -> this.Main())
                                )
                            }),
                            close = (fun m -> async{
                                return this.Complete(ActionManagementActivityResult.Close)
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

        member private this.CreateAction(model) = async{
            try
                use! progress = Progress.Show(ctx, "creating a new action...")
                do! actionEngine.CreateActions([| model.action.Configuration |]) |> Async.Ignore
            with err -> 
                dbg.Error(err)
                do! ErrorView.Show(ctx, err) |> Async.Ignore
            return! this.Main()
        }

        member private this.ModifyAction(model) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, "configuring the action...")
                        do! actionEngine.ModifyActions([| model.action |])
                    }
                    return this.Main()
                with err -> 
                    do! show_error(err)
                    return this.Main()
                }
            return! cont
        }

        member private this.DeleteAction(actionToDelete:Action1) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, "deleting the action...")
                        do! actionEngine.DeleteActions( [| actionToDelete.Token |])
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
            let act = new ActionManagementActivity(ctx) 
            act.Main()
    end
