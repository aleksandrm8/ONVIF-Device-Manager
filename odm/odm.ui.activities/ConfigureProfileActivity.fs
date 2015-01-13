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

    type internal SelectItemModel<'T when 'T:equality>(items, selectedEntity:option<'T>) = class
        let selectedItem = 
            match selectedEntity with
            |Some entity->
                match items |> List.tryFind(fun(e,i)->e=entity) with
                |Some (e,i) -> i
                |None -> null
            |None->null
        
        new (items, selectedEntity:'T) = SelectItemModel<'T> (items, Some selectedEntity)
        new (items) = SelectItemModel<'T> (items, None)

        member this.items:list<'T*ItemSelectorView.Item> = items
        member this.selection:ItemSelectorView.Item = selectedItem
        member this.GetEntityFromItem(item:ItemSelectorView.Item) = 
            let entity = items |> List.tryFind(fun(e, i) -> i=item)
            match entity with
            | Some (e,i) -> Some e
            | None -> None
        member this.GetEntities() = 
            items |> List.map(fun(e, i)->e) |> List.toArray
        member this.GetItems() = 
            items |> List.map(fun(e, i)->i) |> List.toArray
    end

    type ConfigureProfileActivity(ctx:IUnityContainer, profile:Profile) = class
        do if profile |> IsNull then raise( new ArgumentNullException("profile") )
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            //let! caps = session.GetCapabilities()
            let! vecs, aecs, vacs, metcs, ptzcs = Async.Parallel(
                async{
                    if profile.videoSourceConfiguration |> NotNull then
                        let! vecs = session.GetCompatibleVideoEncoderConfigurations(profile.token)
                        return vecs |> SuppressNull [||]
                    else
                        return [||]
                },
                async{
                    if profile.audioSourceConfiguration |> NotNull then
                        let! aecs = session.GetCompatibleAudioEncoderConfigurations(profile.token)
                        return aecs |> SuppressNull [||]
                    else
                        return [||]
                },
                async{
                    let! isAnalyticsSupported = facade.IsAnalyticsSupported()
                    if isAnalyticsSupported then
                        let! vacs = session.GetCompatibleVideoAnalyticsConfigurations(profile.token)
                        return vacs |> SuppressNull [||]
                    else
                        return [||]
                },
                async{
                    let! metcs = session.GetCompatibleMetadataConfigurations(profile.token)
                    return metcs |> SuppressNull [||]
                },
                async{
                    let! ptzcs = async{
                        let! isPtzSupported = facade.IsPtzSupported()
                        if not(isPtzSupported) then
                            return null
                        else
                            try
                                let ptz = session :> IPtzAsync 
                                return! ptz.GetConfigurations()
                            with err->
                                dbg.Error(err)
                                return null
                    }
                    if ptzcs |> NotNull then
                        return ptzcs
                    elif profile.ptzConfiguration |> NotNull then
                        return [|profile.ptzConfiguration|]
                    else
                        return [||]
                }
            )
            
            let model = new ProfileUpdatingView.Model(
                videoEncCfgs = vecs,
                audioEncCfgs = aecs,
                analyticsCfgs = vacs,
                metaCfgs = metcs,
                ptzCfgs = ptzcs
            )
            //model.videoEncCfgs <- vecs
            model.isVideoEncCfgEnabled <- NotNull(profile.videoEncoderConfiguration)
            model.videoEncCfg <- 
                if profile.videoEncoderConfiguration |> NotNull then
                    let cfgToken = profile.videoEncoderConfiguration.token
                    match vecs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif vecs.Length > 0 then
                    vecs.[0]
                else
                    null
            
            //model.audioEncCfgs <- aecs
            model.isAudioEncCfgEnabled <- NotNull(profile.audioEncoderConfiguration)
            model.audioEncCfg <- 
                if profile.audioEncoderConfiguration |> NotNull then
                    let cfgToken = profile.audioEncoderConfiguration.token
                    match aecs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif aecs.Length > 0 then
                    aecs.[0]
                else
                    null
            
            //model.analyticsCfgs <- vacs
            model.isAnalyticsCfgEnabled <- NotNull(profile.videoAnalyticsConfiguration)
            model.analyticsCfg <- 
                if profile.videoAnalyticsConfiguration |> NotNull then
                    let cfgToken = profile.videoAnalyticsConfiguration.token
                    match vacs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif vacs.Length > 0 then
                    vacs.[0]
                else
                    null
            
            //model.metaCfgs <- metcs
            model.isMetaCfgEnabled <- NotNull(profile.metadataConfiguration)
            model.metaCfg <- 
                if profile.metadataConfiguration |> NotNull then
                    let cfgToken = profile.metadataConfiguration.token
                    match metcs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif metcs.Length > 0 then
                    metcs.[0]
                else
                    null
            
            //model.ptzCfgs <- ptzcs
            model.isPtzCfgEnabled <- NotNull(profile.ptzConfiguration)
            model.ptzCfg <- 
                if profile.ptzConfiguration |> NotNull then
                    let cfgToken = profile.ptzConfiguration.token
                    match ptzcs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif ptzcs.Length > 0 then
                    ptzcs.[0]
                else
                    null
            
            model.AcceptChanges();
            return model
        }

        let configure(model:ProfileUpdatingView.Model) = async{

            if model.isVideoEncCfgEnabled && NotNull(model.videoEncCfg) then
                do! session.AddVideoEncoderConfiguration(profile.token, model.videoEncCfg.token)
                profile.videoEncoderConfiguration <- model.videoEncCfg
            elif not(model.isVideoEncCfgEnabled) && NotNull(profile.videoEncoderConfiguration) then
                do! session.RemoveVideoEncoderConfiguration(profile.token)
                profile.videoEncoderConfiguration <- null
            
            if model.isAudioEncCfgEnabled && NotNull(model.audioEncCfg) then
                do! session.AddAudioEncoderConfiguration(profile.token, model.audioEncCfg.token)
                profile.audioEncoderConfiguration <- model.audioEncCfg
            elif not(model.isAudioEncCfgEnabled) && NotNull(profile.audioEncoderConfiguration) then
                do! session.RemoveAudioEncoderConfiguration(profile.token)
                profile.audioEncoderConfiguration <- null
            
            if model.isAnalyticsCfgEnabled && NotNull(model.analyticsCfg) then
                do! session.AddVideoAnalyticsConfiguration(profile.token, model.analyticsCfg.token)
                profile.videoAnalyticsConfiguration <- model.analyticsCfg
            elif not(model.isAnalyticsCfgEnabled) && NotNull(profile.videoAnalyticsConfiguration) then
                do! session.RemoveVideoAnalyticsConfiguration(profile.token)
                profile.videoAnalyticsConfiguration <- null
            
            if model.isMetaCfgEnabled && NotNull(model.metaCfg) then
                do! session.AddMetadataConfiguration(profile.token, model.metaCfg.token)
                profile.metadataConfiguration <- model.metaCfg
            elif not(model.isMetaCfgEnabled) && NotNull(profile.metadataConfiguration) then
                do! session.RemoveMetadataConfiguration(profile.token)
                profile.metadataConfiguration <- null
            
            if model.isPtzCfgEnabled && NotNull(model.ptzCfg) then
                do! session.AddPTZConfiguration(profile.token, model.ptzCfg.token)
                profile.ptzConfiguration <- model.ptzCfg
            elif not(model.isPtzCfgEnabled) && NotNull(profile.ptzConfiguration) then
                do! session.RemovePTZConfiguration(profile.token)
                profile.ptzConfiguration <- null 
            
            model.AcceptChanges()
            return ()
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
                    do! show_error(err)
                    return this.Complete(true)
            }
            return! cont
        }

        ///<summary></summary>
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = ProfileUpdatingView.Show(ctx, model)
                    return res.Handle(
                        abort = (fun ()-> this.Complete(true)),
                        finish = (fun model-> this.Finish(model)),
                        selectVideoEncCfg = (fun model-> this.SelectVec(model)),
                        selectAudioEncCfg = (fun model-> this.SelectAec(model)),
                        selectMetaCfg = (fun model-> this.SelectMeta(model)),
                        selectPtzCfg = (fun model-> this.SelectPtz(model)),
                        selectAnalyticsCfg = (fun model-> this.SelectAnalytics(model))
                    )
                with err -> 
                    do! show_error err
                    return this.Complete(true)
            }
            return! cont
        }

        ///<summary></summary>
        member private this.Finish(model) = async{
            if model.isModified then
                do! async{
                    use! progress = Progress.Show(ctx, LocalDevice.instance.configuring)
                    return! configure(model)
                }
            return! this.Complete(false)
        }
        
        ///<summary></summary>
        member private this.SelectVec(model) = async{
            let! cont = async{
                try
                    let items = Seq.toList(seq{
                        for vec in model.videoEncCfgs do
                            let item = new ItemSelectorView.Item(vec.GetName(), GetVecDetails(vec) |> List.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable)
                            yield (vec, item)
                    })

                    let itemsModel = new SelectItemModel<VideoEncoderConfiguration>(items, model.videoEncCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)

                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some e -> model.videoEncCfg <- e
                        | None -> ()

                    return this.ShowForm(model)
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.SelectAec(model) = async{
            let! cont = async{
                try
                    let items = Seq.toList(seq{
                        for aec in model.audioEncCfgs do
                            let item = new ItemSelectorView.Item(aec.GetName(), GetAecDetails(aec) |> List.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable)
                            yield (aec, item)
                    })
                    let itemsModel = new SelectItemModel<AudioEncoderConfiguration>(items, model.audioEncCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some entity -> model.audioEncCfg <- entity
                        | None -> ()
                    return this.ShowForm(model)
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.SelectMeta(model) = async{
            let! cont = async{
                try
                    let items = Seq.toList(seq{
                        for meta in model.metaCfgs do
                            let item = new ItemSelectorView.Item(meta.GetName(), GetMetaDetails(meta) |> List.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable)
                            yield (meta, item)

                    })
                    let itemsModel = new SelectItemModel<MetadataConfiguration>(items, model.metaCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some entity -> model.metaCfg <- entity
                        | None -> ()
                    return this.ShowForm(model)
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        member private this.SelectPtz(model) = async{
            let! cont = async{
                try
                    let! nodes = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! session.GetNodes()
                    }
                    let items = Seq.toList(seq{
                        for ptz in model.ptzCfgs do
                            let item = new ItemSelectorView.Item(ptz.GetName(), GetPtzDetails(ptz, nodes) |> Seq.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable)
                            yield (ptz, item)
                    })
                    let itemsModel = new SelectItemModel<PTZConfiguration>(items, model.ptzCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some entity -> model.ptzCfg <- entity
                        | None -> () 
                    return this.ShowForm(model)
                with err ->
                    do! show_error err
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.SelectAnalytics(model) = async{
            let! cont = async{
                try
                    let items = Seq.toList(seq{
                        for analytics in model.analyticsCfgs do
                            let details = GetVacDetails(analytics)
                            yield (analytics, new ItemSelectorView.Item(analytics.GetName(), details |> Seq.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable))
                    })
                    let itemsModel = new SelectItemModel<VideoAnalyticsConfiguration>(items, model.analyticsCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some entity -> model.analyticsCfg <- entity
                        | None -> () 
                    return this.ShowForm(model)
                with err ->
                    do! show_error err
                    return this.ShowForm(model)
            }
            return! cont
        }
        member private this.Complete(res) = async{
            return res
        }
        static member Run(ctx, profileToken) = 
            let act = new ConfigureProfileActivity(ctx,profileToken)
            act.Main()
    end

