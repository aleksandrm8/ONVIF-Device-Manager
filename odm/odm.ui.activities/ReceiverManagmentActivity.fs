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
    //open ProfileDescription
    open odm.ui
    
    type ReceiverManagementActivityResult =
        |Refresh
        |Close
    
    type ReceiverManagementActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)

        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        let dev = session :> IDeviceAsync
        let recv = session :> IReceiverAsync

        let load() = async{
            //let! caps = dev.GetCapabilities()
            let! receivers = recv.GetReceivers()
            
            return new ReceiversView.Model(
                receivers = receivers,
                selection = null
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
                        let! res = ReceiversView.Show(ctx, model)

                        return! res.Handle(
                            create = (fun m ->async{
                                //create new receiver
                                let! r = ReceiversModifyView.Show(
                                    ctx, 
                                    new ReceiversModifyView.Model()
                                )
                                return r.Handle(
                                    apply = (fun m -> this.CreateReceiver(m) ),
                                    close = (fun m -> this.Main()),
                                    cancel = (fun m -> this.Main())
                                )
                            }),
                            delete = (fun m -> async{
                                //delete selected receiver
                                return this.DeleteReceiver(m.selection.token)
                            }),
                            modify = (fun m -> async{
                                //modify selected receiver
                                let! r = ReceiversModifyView.Show(
                                    ctx, 
                                    new ReceiversModifyView.Model(
                                        receiver = m.selection
                                    )
                                )
                                return r.Handle(
                                    apply = (fun m -> this.ModifyReceiver(m)),
                                    close = (fun m -> this.Main()),
                                    cancel = (fun m -> this.Main())
                                )
                            }),
//                            select = (fun item -> async{
//                                //activate selected profile
//                                let prof = GetProfileFromItem(item)
//                                if prof<>null then
//                                    return this.Complete(ProfileManagementActivityResult.Select(prof.token))
//                                else
//                                    do! show_error(new Exception(LocalProfile.instance.selectFail))
//                                    return! show()
//                            }),
                            close = (fun m -> async{
                                return this.Complete(ReceiverManagementActivityResult.Close)
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
        member private this.CreateReceiver(model) = async{
            try
                use! progress = Progress.Show(ctx, "creating new receiver...")
                do! recv.CreateReceiver(model.receiver.configuration) |> Async.Ignore
            with err -> 
                dbg.Error(err)
                do! ErrorView.Show(ctx, err) |> Async.Ignore
            return! this.Main()
        }

        member private this.ModifyReceiver(model) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, "configuring receiver...")
                        do! recv.ConfigureReceiver(model.receiver.token, model.receiver.configuration)
                    }
                    return this.Main()
                with err -> 
                    do! show_error(err)
                    return this.Main()
                }
            return! cont
        }

        member private this.DeleteReceiver(receiverToDelete) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, "deleting receiver...")
                        do! recv.DeleteReceiver(receiverToDelete)
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
            let act = new ReceiverManagementActivity(ctx)
            act.Main()
    end
