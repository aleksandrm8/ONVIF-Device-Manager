//module VideoSettingsActivity
namespace odm.ui.activities
    open System
    open System.Linq
    //open System.Disposables
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.core
    open odm.infra
    open utils
    ////open odm.models
    open utils.fsharp
    open ProfileDescription
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs

    type CreateProfileActivityResult =
    |Created of Profile
    |Aborted

    type CreateProfileActivity(ctx:IUnityContainer, vsToken:string) = class
        do if vsToken |> IsNull then raise( new ArgumentNullException("vsToken") )
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        let load() = async{
            let! vscs = async{
                try
                    let! vscs = session.GetVideoSourceConfigurations()
                    if vscs |> NotNull then
                        return vscs |> Seq.filter (fun vsc->vsc.sourceToken = vsToken) |> Seq.toArray
                    else
                        return [||]
                with err ->
                    dbg.Error(err)
                    return [||]
            }
            let! ascs = async{
                try
                    let! ascs = session.GetAudioSourceConfigurations()
                    return ascs |> SuppressNull [||]
                with err ->
                    dbg.Error(err)
                    return [||]
            }
            let model = new ProfileCreationView.Model(
                videoSrcCfgs = vscs,
                audioSrcCfgs = ascs
            )
            
            //model.videoSrcCfgs <- vscs |> List.toArray
            model.origin.isVideoSrcCfgEnabled <- model.videoSrcCfgs.Length > 0
            if model.videoSrcCfgs.Length > 0 then
                model.origin.videoSrcCfg <- model.videoSrcCfgs.[0]
            
            //model.audioSrcCfgs <- ascs
            model.origin.isAudioSrcCfgEnabled <- false
            if ascs.Length > 0 then
                model.origin.audioSrcCfg <- ascs.[0]

            model.origin.profName <- "new profile"
            model.origin.profToken <- null
            
            model.RevertChanges();
            return model
        }

        let create(model:ProfileCreationView.Model) = async{
            if model.profName |> IsNull then failwith "invalid profile name"
            if IsNull(model.videoSrcCfg) || IsNull(model.videoSrcCfg.token) then failwith "invalid video source configuration"

            let profToken = model.profToken
                
            let! profile = session.CreateProfile(model.profName, profToken)
            
            if model.isVideoSrcCfgEnabled && NotNull(model.videoSrcCfg) then
                do! session.AddVideoSourceConfiguration(profile.token, model.videoSrcCfg.token)
                profile.videoSourceConfiguration <- model.videoSrcCfg

            if model.isAudioSrcCfgEnabled && NotNull(model.audioSrcCfg) then
                do! session.AddAudioSourceConfiguration(profile.token, model.audioSrcCfg.token)
                profile.audioSourceConfiguration <- model.audioSrcCfg

            return profile
        }

        member private this.Main() = async{
            let! cont = async{
                try
                    let! model = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! load()
                    }
                    return this.ShowForm(model)
                with err->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = ProfileCreationView.Show(ctx, model)
                    return res.Handle(
                        finish = (fun model->this.Finish(model)),
                        abort = (fun model->this.Complete(CreateProfileActivityResult.Aborted)),
                        selectVideoSrcCfg = (fun model->this.SelectVsc(model)),
                        selectAudioSrcCfg = (fun model->this.SelectAsc(model)),
                        configure = (fun model->this.Configure(model))
                    )
                with err->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.Finish(model) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalProfile.instance.creatingProfile)
                    let! profile = create(model)
                    let result = CreateProfileActivityResult.Created(profile)
                    return this.Complete(result)
                with err->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.Configure(model) = async{
            let! cont = async{
                try
                    let! profile = async{
                        use! progress = Progress.Show(ctx, LocalProfile.instance.creatingProfile)
                        return! create(model)
                    }
                    return! async{
                        let! wasAborted = ConfigureProfileActivity.Run(ctx, profile)
                        if wasAborted then
                            do! session.DeleteProfile(profile.token)
                            return this.ShowForm(model)
                        else
                            let result = CreateProfileActivityResult.Created(profile)
                            return this.Complete(result)
                    }
                with err->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.SelectVsc(model) = async{
            let! cont = async{
                try
                    let! videoSrcs = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! session.GetVideoSources()
                    }
                    let items = Seq.toList(seq{
                        for i in model.videoSrcCfgs do
                            let name = i.GetName()
                            let details = 
                                let vs = videoSrcs |> Seq.tryFind (fun vs->vs.token = i.sourceToken)
                                match vs with
                                |Some vs -> GetVscDetails(i,Seq.singleton vs)
                                |None -> GetVscDetails(i,null)
                            
                            yield (i, new ItemSelectorView.Item(name, details |> List.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable))
                    })
                    let m = new ItemSelectorView.Model(
                        items = (
                            items |> List.map (fun (x,y) -> y) |> List.toArray
                        ),
                        flags = (
                            ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                        )
                    )
                    match items |> List.tryFind (fun (x,y) -> x = model.videoSrcCfg) with
                    | Some (cfg, item) -> m.origin.selection<-item
                    | None -> m.origin.selection<-null
                    m.RevertChanges()
                    let! res = ItemSelectorView.Show(ctx, m)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match items |> List.tryFind (fun (x,y) -> y = item) with
                        | Some (cfg,item) -> model.videoSrcCfg <- cfg
                        | None -> ()
                    return this.ShowForm(model)
                with err ->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.SelectAsc(model) = async{
            let! cont = async{
                try
                    let! audioSrcs = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! session.GetAudioSources()
                    }
                    let items = Seq.toList(seq{
                        for i in model.audioSrcCfgs do
                            let name = i.GetName()
                            let details = seq{
                                let createProp(name, value) = new ItemSelectorView.ItemProp(name, value)
                                        
                                yield createProp("name", i.name)
                                yield createProp("token", i.token)
                                yield createProp("source token", i.sourceToken)
                                match audioSrcs |> Seq.tryFind (fun x-> x.token = i.sourceToken) with
                                | Some audioSrc ->
                                    yield createProp("channels", audioSrc.channels.ToString())                                                    
                                | None -> ()
                            }
                            yield (i, new ItemSelectorView.Item(name, details |> Seq.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable))
                    })
                    let m = new ItemSelectorView.Model(
                        items = (
                            items |> List.map (fun (x,y) -> y) |> List.toArray
                        ),
                        flags = (
                            ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                        )
                    )
                    match items |> List.tryFind (fun (x,y) -> x = model.audioSrcCfg) with
                    | Some (cfg, item) -> m.origin.selection<-item
                    | None -> m.origin.selection<-null
                    m.RevertChanges()
                    let! res = ItemSelectorView.Show(ctx, m)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match items |> List.tryFind (fun (x,y) -> y = item) with
                        | Some (cfg,item) -> model.audioSrcCfg <- cfg
                        | None -> ()
                    return this.ShowForm(model)
                with err ->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx:IUnityContainer, vsToken:string) = 
            let act = new CreateProfileActivity(ctx, vsToken)
            act.Main()
    end
