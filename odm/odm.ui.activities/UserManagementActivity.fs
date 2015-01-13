namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.IO
    open System.Linq
    open System.Net
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.ui
    open odm.core
    open odm.infra
    open utils
    //open odm.models
    open utils.fsharp

    type UserManagementActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        
        let show_error(err:Exception) = async{
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        let load() = async{
            let! users = dev.GetUsers()
            let model = new UserManagementView.Model(
                users = users
            )
            model.selection <- users |> IfNotNull(fun x->x.FirstOrDefault())
            model.AcceptChanges()
            return model
        }
        
        ///<summary></summary>
        member private this.Main() = async{
            let! cont = async{
                try
                    let! model = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! load()
                    }
                    return this.ShowForm(model)
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = UserManagementView.Show(ctx, model)
                    return res.Handle(
                        createUser = (fun model-> this.CreateUser(model)),
                        deleteUser = (fun model-> this.DeleteUser(model)),
                        modifyUser = (fun model-> this.ModifyUser(model)),
                        uploadPolicy = (fun model fileName-> this.UploadPolicy(model, fileName)),
                        downloadPolicy = (fun model fileName-> this.DownloadPolicy(model, fileName)),
                        close = (fun (model)->this.Complete())
                    )
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        member private this.CreateUser(model) = async{
            let! cont = async{
                try
                    let creationModel = new UserCreationView.Model(
                        defaultUserName = "user",
                        defaultPassword = null,
                        defaultUserLevel = UserLevel.administrator,
                        existingUsers = (
                            if model.users |> NotNull then 
                                model.users |> Seq.map(fun u-> u.username) |> Seq.toArray
                            else
                                null
                        )
                    )
                    let! res = UserCreationView.Show(ctx, creationModel)
                    return! res.Handle(
                        apply = (fun username password userLevel-> async{
                            use! progress = Progress.Show(ctx, LocalDevice.instance.creating)
                            let user = new User()
                            user.username <- username
                            user.password <- password
                            user.userLevel <- userLevel
                            do! dev.CreateUsers([|user|])
                            return async{
                                do! InfoView.Show(ctx, LocalUserManagement.instance.createSuccess) |> Async.Ignore
                                return! this.Main()
                            }
                        }),
                        cancel = (fun ()-> async{
                            return this.ShowForm(model)
                        })
                    )
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.ModifyUser(model) = async{
            let! cont = async{
                try
                    let updatingModel = new UserUpdatingView.Model(
                        name = model.selection.username
                    )
                    updatingModel.level <- model.selection.userLevel
                    updatingModel.password <- 
                        if String.IsNullOrEmpty(model.selection.password) then
                            null
                        else
                            model.selection.password
                    updatingModel.AcceptChanges()
                    let! res = UserUpdatingView.Show(ctx, updatingModel)
                    return! res.Handle(
                        apply = (fun (model)-> async{
                            use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                            let user = new User()
                            user.username <- model.name
                            user.password <- model.password
                            user.userLevel <- model.level
                            do! dev.SetUser([|user|])
                            return async{
                                do! InfoView.Show(ctx, LocalUserManagement.instance.changeSuccess) |> Async.Ignore
                                return! this.Main()
                            }
                        }),
                        cancel = (fun ()-> async{
                            return this.ShowForm(model)
                        })
                    )
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.DeleteUser(model) = async{
            let! cont = async{
                try
                    if model.selection |> NotNull then
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        do! dev.DeleteUsers([|model.selection.username|])
                        return async{
                            do! InfoView.Show(ctx, LocalUserManagement.instance.deleteSuccess) |> Async.Ignore
                            return! this.Main()
                        }
                    else
                        return this.ShowForm(model)
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.UploadPolicy(model, fileName) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalDevice.instance.uploading)
                    use fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
                    let! bytes = fstream.AsyncRead(int(fstream.Length))
                    let data = new BinaryData()
                    data.data <- bytes
                    do! dev.SetAccessPolicy(data)
                    return this.ShowForm(model)
//                    let! res = OpenFileActivity.Run("Policy files (*.pan)|*.pan")
//                    return!
//                        match res with
//                        |OpenFileActivityResult.Selected fileName -> 
//                            async{
//                                use! progress = ProgressActivity.Show(ctx, "uploading...")
//                                use fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
//                                let! bytes = fstream.AsyncRead(int(fstream.Length))
//                                let data = new BinaryData()
//                                data.Data <- bytes
//                                do! dev.SetAccessPolicy(data)
//                                return this.ShowForm(model)
//                            }
//                        |OpenFileActivityResult.Canceled -> 
//                            async{
//                                return this.ShowForm(model)
//                            }
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.DownloadPolicy(model, fileName) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalDevice.instance.downloading)
                    let! data = dev.GetAccessPolicy()
                    use fstream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read)
                    if NotNull(data) && NotNull(data.data) then
                        do! fstream.AsyncWrite(data.data)
                    return this.ShowForm(model)
//                    let! res = SaveFileActivity.Run("Policy files|*.pan")
//                    return!
//                        match res with
//                        |SaveFileActivityResult.Selected fileName -> 
//                            async{
//                                use! progress = ProgressActivity.Show(ctx, "downloading...")
//                                let! data = dev.GetAccessPolicy()
//                                if data<>null && data.Data <> null then
//                                    use fstream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read)
//                                    do! fstream.AsyncWrite(data.Data)
//                                return this.ShowForm(model)
//                            }
//                        |SaveFileActivityResult.Canceled ->  
//                            async{
//                                return this.ShowForm(model)
//                            }
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.Complete(res) = async{
            return res
        }
        

        static member Run(ctx:IUnityContainer) = 
            let act = new UserManagementActivity(ctx)
            act.Main()
    end
