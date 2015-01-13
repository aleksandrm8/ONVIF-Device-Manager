namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Linq
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity

    //open odm.models
    open odm.onvif
    open odm.core
    open odm.infra
    open odm.ui

    open onvif.services
    open onvif.utils
    open utils
    open utils.fsharp
    open ProfileDescription
    
    type Nva = class
        
        /// <summary>returns input configurations used by specified control</summary>
        /// <param name="vand">client of video analytics for devices service</param>
        /// <param name="controlToken">token of control</param>
        static member GetControlInputConfigs(vand:IAnalyticsDeviceAsync, controlToken:string) = async{
            let! ctrl = vand.GetAnalyticsEngineControl(controlToken)
            let configTokens =  ctrl.inputToken |> SuppressNull ([||])
            if configTokens.Length > 1 then
                let! allConfigs = vand.GetAnalyticsEngineInputs() |> Async.Map (SuppressNull ([||]))
                return [|
                    for c in allConfigs do 
                        if NotNull(c) && configTokens |> Seq.exists(fun ct->ct = c.token) then 
                            yield c
                |]
            elif configTokens.Length = 1 then
                let! cfg = vand.GetAnalyticsEngineInput(configTokens.[0])
                return [|cfg|]
            else
                return [||]
        }
    end

    type ItemSelectorActivityResult =
        | Delete
        | Select
        | Create
        | Modify

    type ItemSelectorActivityTransition<'T> = 
        | Complete of 'T
        | Continue

    type ItemSelectorProcessor = interface
        abstract member OnDelete : deletingItemIndex:int -> Async<ItemSelectorActivityTransition<'T>>
        abstract member OnCreate : unit -> Async<ItemSelectorActivityTransition<'T>>
        abstract member OnSelect : unit -> Async<ItemSelectorActivityTransition<'T>>
        abstract member OnModify : unit -> Async<ItemSelectorActivityTransition<'T>>
    end

    [<AbstractClass>]
    type BaseActivity<'TIn, 'TModel, 'TOut>(ctx:IUnityContainer)  = class
        abstract member Main : 'TIn -> Async<'TOut>
        abstract member Load : 'TIn -> Async<'TModel>
        abstract member Show : 'TModel -> Async<'TOut>
        abstract member ShowError : exn -> Async<unit>
        abstract member ShowProgress : string -> Async<IDisposable>

        default this.Main(args) = async{
            let! cont = async{
                try
                    let! model = this.Load(args)
                    return this.Show(model)
                with err ->
                    dbg.Error(err)
                    do! this.ShowError(err)
                    return this.Main(args)
            }
            return! cont
        }

        default this.ShowError(err) = async{
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        default this.ShowProgress(message) = async{
            return! Progress.Show(ctx, message)
        }
    end

    type ItemSelectorViewResult<'a> = 
        |Create of ItemSelectorModel<'a>
        |Delete of ItemSelectorModel<'a>
        |Modify of ItemSelectorModel<'a>
        |Select of ItemSelectorModel<'a>
        |Finish of ItemSelectorModel<'a>
    and ItemSelectorModel<'a>(items:('a*ItemSelectorView.Item) list, flags:ItemSelectorView.Flags, selection:ItemSelectorView.Item) = class
        member this.items = items
        member this.selection = selection
        member this.Show(ctx) = async{
            let viewModel = ItemSelectorView.Model.Create(
                items = [|yield! items |> Seq.map(fun (_,i) -> i)|],
                selection = selection,
                flags = flags
            )
            let! res = ItemSelectorView.Show(ctx, viewModel)
            return res.Handle(
                create = (fun () -> ItemSelectorViewResult.Create(this.SetSelection(viewModel.selection))),
                delete = (fun item -> ItemSelectorViewResult.Delete(this.SetSelection(item))),
                modify = (fun item -> ItemSelectorViewResult.Modify(this.SetSelection(item))),
                select = (fun item -> ItemSelectorViewResult.Select(this.SetSelection(item))),
                close = (fun () -> ItemSelectorViewResult.Finish(this.SetSelection(viewModel.selection)))
            )
        }
        member this.TryExtractSelection() = 
            match items |> Seq.tryFind (fun (e,i)->i=selection) with
            |Some (e,i) -> Some e
            |None -> None

        member this.SetSelection(selection) = 
            new ItemSelectorModel<'a>(items, flags, selection)
    end

    [<AbstractClass>]
    type ItemSelectorActivity<'TSrc, 'TRes>(ctx:IUnityContainer) = class
        inherit BaseActivity<Async<'TSrc seq>, ItemSelectorModel<'TSrc> , 'TRes>(ctx)

        abstract member OnDelete : model:ItemSelectorModel<'TSrc> -> Async<Async<'TRes>>
        abstract member OnCreate : model:ItemSelectorModel<'TSrc> -> Async<Async<'TRes>>
        abstract member OnSelect : model:ItemSelectorModel<'TSrc> -> Async<Async<'TRes>>
        abstract member OnModify : model:ItemSelectorModel<'TSrc> -> Async<Async<'TRes>>
        abstract member OnFinish : model:ItemSelectorModel<'TSrc> -> Async<Async<'TRes>>
        abstract member BuildItem : 'TSrc -> ItemSelectorView.Item

        override this.Load(loader) = async{
            let! src = async{
                use! progress = this.ShowProgress("loading...")
                let! src = loader
                return src |> Seq.toList
            }
            return new ItemSelectorModel<'TSrc>(
                items = [for i in src -> (i, this.BuildItem(i))], 
                flags = (seq{
                    yield ItemSelectorView.Flags.CanCreate
                    yield ItemSelectorView.Flags.CanSelect
                    yield ItemSelectorView.Flags.CanDelete
                    yield ItemSelectorView.Flags.CanModify
                    yield ItemSelectorView.Flags.CanClose
                } |> Seq.fold (|||) (ItemSelectorView.Flags.NoOperationsAvailable)),
                selection = null
            )
        }
        
        override this.Show(model) = async{
            let! cont = async{
                try
                    let! res = model.Show(ctx)
                    match res with
                    |Create m -> return! this.OnCreate(m)
                    |Delete m -> return! this.OnDelete(m)
                    |Modify m -> return! this.OnModify(m)
                    |Select m -> return! this.OnSelect(m)
                    |Finish m -> return! this.OnFinish(m)
                with err->
                    dbg.Error(err)
                    do! this.ShowError(err)
                    return this.Show(model)
            }
            return! cont
        }

//        static member Run(ctx:IUnityContainer, src: seq<'a>, itemBuilder:'a->ItemSelectorView.Item, selectedIndex:int) =
//            let act = new ItemSelectorActivity(ctx)
//            let f = [for i in items -> i, itemBuilder(i)]
//            act.Main()
    end

    type NvaControlManagmentActivityResult =
        |Select of string
        |Refresh
        |Close
    
    type NvaControlManagmentActivityState(controls:seq<AnalyticsEngineControl>, selection:AnalyticsEngineControl, model:EngineControlsView.Model) = class
        member this.controls = controls
        member this.selection = selection
        member this.model = model
    end

    type NvaConfigureInputsActivityIn =
        | InputTokens of (string[])*(string[])
        | Inputs of (AnalyticsEngineInput[])*(Receiver[])

    type NvaConfigureInputsActivity(ctx:IUnityContainer) = class
        inherit BaseActivity<string[]*string[], AnalyticsEngineInput[]*Receiver[]*int, AnalyticsEngineInput[]*Receiver[]>(ctx)

        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        let recv = session :> IReceiverAsync
        let vand = session :> IAnalyticsDeviceAsync

//        static member Run(ctx:IUnityContainer, inputs:array<AnalyticsEngineInput*Receiver>) =
//            let act = new NvaConfigureInputsActivity(ctx)
//            act.Main(inputs, -1)
//        static member Run(ctx:IUnityContainer, inputTokens:seq<string*string>) = 
//            let act = new NvaConfigureInputsActivity(ctx)
//            act.Load(inputTokens, -1)

        override this.Load((configTokens, receiverTokens)) = async{
            use! progress = this.ShowProgress("loading...")
            let! (allConfigs, allReceivers) = Async.Parallel(
                vand.GetAnalyticsEngineInputs() |> Async.Map (SuppressNull [||]),
                recv.GetReceivers() |> Async.Map (SuppressNull [||])
            )
            let configs = configTokens |> Seq.map(fun ct->allConfigs |> Seq.firstOrDefault(fun c->c.token = ct))
            let receivers = receiverTokens |> Seq.map(fun rt->allReceivers |> Seq.firstOrDefault(fun r->r.token = rt))
            return (configs |> Seq.toArray, receivers |> Seq.toArray, -1)
        }
        override this.Show((configs, receivers, selectedIndex)) = async{
            let rec view = {
                new ItemSelectorActivity<AnalyticsEngineInput*Receiver, AnalyticsEngineInput[] *Receiver[] >(ctx) with
                    override this.BuildItem((config, receiver)) = 
                        let receiverName = 
                            if receiver |> IsNull then
                                "<no receiver>"
                            else if receiver.token |> IsNull then
                                "<null>"
                            else 
                                receiver.token
                    
                        let configName = 
                            if config |> IsNull then
                                "<no input configuration>"
                            elif not <| String.IsNullOrWhiteSpace(config.name) then
                                config.name
                            elif config.token |> IsNull then
                                "<null>"
                            elif String.IsNullOrWhiteSpace(config.token) then
                                "<empty>"
                            else
                                config.token

                        let details = seq{
                            if receiver |> NotNull then
                                let receiverDetails = GetReceiverDetails(receiver) |> Seq.toArray
                                yield new ItemSelectorView.ItemProp("receiver", receiverName, receiverDetails)
                            if config |> NotNull then
                                let configDetails = GetNvaInputDetails(config) |> Seq.toArray
                                yield new ItemSelectorView.ItemProp("input configuration", configName, configDetails)
                        }
                        let flags = (seq{
                            yield ItemSelectorView.ItemFlags.CanBeModified
                            yield ItemSelectorView.ItemFlags.CanBeDeleted
                        } |> Seq.fold (|||) (ItemSelectorView.ItemFlags.NoOperationsAvailable))
                        //yield new ItemSelectorView.Item(receiver.ToString(), details, flags)
                   
                        let itemName = sprintf "%s / %s" configName receiverName
                        new ItemSelectorView.Item(itemName, details |> Seq.toArray, flags)
                    override this.OnCreate(m) = async{
                        return this.Show(m)
                    }
                    override this.OnDelete(m) = async{
                        let f = (fun (co, ro) -> 
                            let unwrap_opt(o) = match o with Some x -> x | None -> null
                            (unwrap_opt co, unwrap_opt ro)
                        )
                        let items = m.items |> Seq.filter (fun (e,i)-> i<>m.selection) |> Seq.map (fun (e,i)->e)
                        return view.Main(async{
                            return items
                        })
                    }
                    override this.OnModify(m) = async{
                        return this.Show(m)
                    }
                    override this.OnSelect(m) = async{
                        return this.Show(m)
                    }
                    override this.OnFinish(m) = async{
                        return async{
                            let items = m.items |> Seq.map(fun (e,i)->e)
                            let receivers = items |> Seq.map(fun (i,r)->r) |> Seq.toArray
                            let inputs = items |> Seq.map(fun (i,r)->i) |> Seq.toArray
                            return (inputs, receivers)
                        }
                    }
            }
            let f = (fun (co, ro) -> 
                let unwrap_opt(o) = match o with Some x -> x | None -> null
                (unwrap_opt co, unwrap_opt ro)
            )
            return! view.Main(async{
                return receivers |> Seq.zipAll configs |> Seq.map f |> Seq.filter (fun (c,r) -> NotNull(c) || NotNull(r))
            })
        }
        
//        member private this.Main(inputs, selectedIndex:int) = async{
//            return ()
//        }

//        member private this.ShowForm(inputs, selectedIndex:int) = async{
//            let! cont = async{
//                try
//                    let items = [|
//                        for i,(config, receiver) in inputs |> Seq.index do
//                            let receiverDetails = GetReceiverDetails(receiver) |> Seq.toArray
//                            //let configDetails = GetNvaInputDetails(config) |> Seq.toArray
//                            let flags = (seq{
//                                yield ItemSelectorView.ItemFlags.CanBeModified
//                                yield ItemSelectorView.ItemFlags.CanBeDeleted
//                                if i <> selectedIndex then
//                                    yield ItemSelectorView.ItemFlags.CanBeSelected
//                            } |> Seq.fold (|||) (ItemSelectorView.ItemFlags.NoOperationsAvailable))
//                            //yield new ItemSelectorView.Item(receiver.ToString(), details, flags)
//                            yield new ItemSelectorView.Item(receiver.ToString(), null, flags)
//                    |]
//                    let selItem = items |> Array.getNthOrDefault(selectedIndex)
//                    let GetInputFromItem(item) = 
//                        items |> Seq.zip inputs |> Seq.tryFind (fun (r,i) -> i = item) |> Option.bind (fun (r, i) -> Some r)
//                    let GetIndexFromItem(item) =
//                        items |> Seq.tryFindIndex (fun x->x=item) 
//                    //let itemsModel = new SelectItemModel<Receiver>(items, selection)
//                    let! res = 
//                        let viewModel = 
//                            ItemSelectorView.Model.Create(
//                                items = items,
//                                selection = selItem,
//                                flags = (seq{
//                                    yield ItemSelectorView.Flags.CanCreate
//                                    yield ItemSelectorView.Flags.CanSelect
//                                    yield ItemSelectorView.Flags.CanDelete
//                                    yield ItemSelectorView.Flags.CanModify
//                                    yield ItemSelectorView.Flags.CanClose
//                                } |> Seq.fold (|||) (ItemSelectorView.Flags.NoOperationsAvailable))
//                            )
//                        ItemSelectorView.Show(ctx, viewModel)
//                    return res.Handle(
//                        create = (fun x -> async{
//                            let! res = this.CreateReceiver()
//                            match res with
//                            |Some r ->
//                                return! this.Main(r.Token)
//                            |None ->
//                                return! this.ShowForm(receivers, selection)
//                        }), 
//                        delete = (fun i -> async{
//                            match GetReceiverFromItem(i) with
//                            |Some r ->
//                                let! res = this.DeleteReceiver(r)
//                                if res then
//                                    return! this.Main(selection)
//                                else
//                                    return! this.ShowForm(receivers, selection)
//                            |None -> 
//                                return! this.Main(selection)
//                        }),
//                        modify = (fun i -> async{
//                            match GetReceiverFromItem(i) with
//                            |Some r ->
//                                let! res = this.ModifyReceiver(r)
//                                if res then
//                                    return! this.Main(r.Token)
//                                else
//                                    return! this.ShowForm(receivers, r.Token)
//                            |None ->
//                                return! this.ShowForm(receivers, selection)
//                        }), 
//                        select = (fun i -> async{
//                            return GetReceiverFromItem(i)
//                        }), 
//                        close = (fun () -> async{return None})
//                    )
//                with err ->
//                    dbg.Error(err)
//                    do! ErrorView.Show(ctx, err) |> Async.Ignore
//                    return async{return None}
//            }
//            return! cont
//        }
    end

    type NvaControlConfigurationActivity(ctx:IUnityContainer) = class
        inherit BaseActivity<AnalyticsEngineControl, unit, unit>(ctx)
        override this.Load(control) = async{
            return ()
        }
        override this.Show(()) = async{
            return ()
        }
    end

    type NvaControlManagmentActivity(ctx:IUnityContainer, engine: AnalyticsEngine) = class
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        let dev = session :> IDeviceAsync
        let adev = session :> IAnalyticsDeviceAsync
        let recv = session :> IReceiverAsync

        let load(selection:string) = async{
            let! controls = async{
                let! res = adev.GetAnalyticsEngineControls()
                return res |> SuppressNull [||]
            }
            
            let dict = new Dictionary<AnalyticsEngineControl, AnalyticsState>()
            for c in controls |> Seq.filter (fun x-> IsNull(x.engineToken) || x.engineToken = engine.token) do
                dict.Add(c, null)
            let model = new EngineControlsView.Model(
                controlstates = dict
            )
            model.selection <-
                if selection |> IsNull then
                    null
                else
                    match model.controlstates.Keys |> Seq.tryFind (fun x -> x.token = selection) with
                    | Some x -> x
                    | None -> null
            model.AcceptChanges()
            let state = new NvaControlManagmentActivityState(
                controls = controls,
                selection = (controls |> Seq.firstOrDefault(fun x-> x.token = selection)),
                model = model
            )
            return state
        }
        member private this.Main(selection) = async{
            let! cont = async{
                try
                    let! state = async{
                        use! progress = Progress.Show(ctx, "loading...")
                        return! load(selection)
                    }
                    //model.AcceptChanges()
                    return this.ShowForm(state)
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main(selection)
            }
            return! cont
        }

        member private this.ShowForm(state) = async{
            let! cont = async{
                try
                    let rec show()  = async{
                        let! res = EngineControlsView.Show(ctx, state.model)
                        return! res.Handle(
                            create = (fun m ->async{
                                //create new analytics engine control
                                return this.CreateControl(state)
                            }),
                            delete = (fun m ->async{
                                //delete selected engine control
                                return this.DeleteControl(state)
                            }),
                            modify = (fun m ->async{
                                //modify selected analytics engine control
                                return this.ModifyControl(state)
                            }),
                            select = (fun m ->async{
                                //create new analytics engine control
                                 return this.Complete(NvaControlManagmentActivityResult.Select(m.token))
                            })
                        )
                    }
                    return! show()

                with err ->
                   dbg.Error(err)
                   do! ErrorView.Show(ctx, err) |> Async.Ignore
                   return this.ShowForm(state)
            }
            return! cont
        }
        
        member private this.SelectVac(selectedVacToken:string) = async{
            try
//                let! engine = async{
//                    use! progress = Progress.Show(ctx, "loading engine...")
//                    let! eng = adev.GetAnalyticsEngine(config.EngineToken)
//                    return eng.AnalyticsEngineConfiguration
//                }
                let items = Seq.toList(seq{
                    for eng_cfg in engine.analyticsEngineConfiguration.engineConfiguration do
                        let vac = eng_cfg.videoAnalyticsConfiguration
                        let details = GetVacDetails(vac)
                        let flags = Seq.fold (|||) (ItemSelectorView.ItemFlags.NoOperationsAvailable) (seq{
                            yield ItemSelectorView.ItemFlags.CanBeModified
                            yield ItemSelectorView.ItemFlags.CanBeDeleted
                            if vac.token <> selectedVacToken then
                                yield ItemSelectorView.ItemFlags.CanBeSelected
                        })
                        yield (vac, new ItemSelectorView.Item(vac.GetName(), details |> Seq.toArray, flags))
                })
                let selection = 
                    let r = engine.analyticsEngineConfiguration.engineConfiguration |> Seq.tryFind (fun x -> x.videoAnalyticsConfiguration.token = selectedVacToken)
                    match r with
                    | Some x -> x.videoAnalyticsConfiguration
                    | None -> null
                let itemsModel = new SelectItemModel<VideoAnalyticsConfiguration>(items, selection)
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
                return res.Handle(
                    create = (fun x -> None), 
                    delete = (fun x -> None), 
                    modify = (fun x -> None), 
                    select = (fun x -> itemsModel.GetEntityFromItem(x)), 
                    close = (fun () -> None)
                )
            with err ->
                dbg.Error(err)
                do! ErrorView.Show(ctx, err) |> Async.Ignore
                return None
        }

        member private this.CreateControl(state) = async{
            let selection = if NotNull(state.model.selection) then state.model.selection.token else null
            let m = new EngineControlsCreationView.Model()

            let new_token = 
                let rec gen(num) = 
                    let tok = sprintf "aec_%d" num
                    if state.controls |> Seq.forall (fun x->x.token <> tok) then
                        tok
                    else
                        gen(num+1)
                gen(0)
            
            m.token <- new_token

            let engCfgs = engine |> IfNotNull(fun eng-> eng.analyticsEngineConfiguration |> IfNotNull(fun aec-> aec.engineConfiguration))
            let vac = engCfgs |> Seq.map(fun ec-> ec |> IfNotNull(fun x->x.videoAnalyticsConfiguration)) |> Seq.firstOrDefault NotNull
            m.vacToken <- vac |> IfNotNull(fun vac->vac.token)

            let rec show_internal() = async{
                let! cont = async{
                    try
                        let! res = EngineControlsCreationView.Show(ctx, m)
                        return res.Handle(
                            finish = (fun x -> this.CompleteCreateControl(x, selection)),
                            configureInputs = (fun x -> this.CreateControlAndConfigureInputs(x,selection)),
                            configureVac = (fun x -> async{
                                let! vac_opt = this.SelectVac(m.vacToken)
                                match vac_opt with
                                |Some vac -> 
                                    m.vacToken <- vac.token
                                    return! show_internal()
                                |None -> 
                                    return! show_internal()
                                
                            }),
                            abort = (fun () -> this.ShowForm(state))
                        )
                    with err -> 
                        dbg.Error(err)
                        do! ErrorView.Show(ctx, err) |> Async.Ignore
                        return this.Main(selection)
                    }
                return! cont
            }
            return! show_internal()
        }

        member private this.CompleteCreateControl(config, selection) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, "creating control...")
                    let ctrl = new AnalyticsEngineControl(
                        name = config.name,
                        token = config.token,
                        engineToken = engine.token,
                        engineConfigToken = config.vacToken
                    )
                    do! adev.CreateAnalyticsEngineControl(ctrl) |> Async.Ignore
                    return this.Main(selection)
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main(selection)
                }
            return! cont
        }

        member private this.CreateControlAndConfigureInputs(config, selection) = async{
            let! cont = async{
                try
                    let! inputs, ctrl = async{
                        use! progress = Progress.Show(ctx, "creating control...")
                        let ctrl = new AnalyticsEngineControl(
                            name = config.name,
                            token = config.token,
                            engineToken = engine.token,
                            engineConfigToken = config.vacToken
                        )
                        let! inputs = adev.CreateAnalyticsEngineControl(ctrl)
                        return inputs, ctrl
                    }
                    let! res = this.ConfigureInputs(inputs, null)
                    match res with
                    |Some (inputs, receivers) ->
//                        try
//                            use! progress = Progress.Show(ctx, "configuring control...")
//                            do! adev.DeleteAnalyticsEngineControl(ctrl.token)
//                        with err -> 
//                            dbg.Error(err)
//                            do! ErrorView.Show(ctx, err) |> Async.Ignore
                        return this.Main(selection)
                    |None ->
                        try
                            use! progress = Progress.Show(ctx, "aborting control creation...")
                            do! adev.DeleteAnalyticsEngineControl(ctrl.token)
                        with err -> 
                            dbg.Error(err)
                            do! ErrorView.Show(ctx, err) |> Async.Ignore
                        return this.Main(selection)
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main(selection)
                }
            return! cont
        }

        member private this.ConfigureInputs(inputs, receivers) = async{
            let model = new EngineControlsInputsCreationView.Model(
                inputs = inputs,
                receivers = receivers
            )
            model.AcceptChanges()
            let! res = EngineControlsInputsCreationView.Show(ctx, model)
            return! res.Handle(
                finish = (fun x->async{
                    return Some (inputs, receivers)
                }),
                modify = (fun input -> async{
                    let! cont = async{
                        try
                            let! res = SelectReceiverActivity.Run(ctx, null)
                            match res with
                            |Some receiver ->
                                let f(x) = 
                                    match x with
                                    | Some r, Some i -> (r,i)
                                    | Some r, None -> (r,null)
                                    | None, Some i -> (null,i)
                                    | None, None -> (null, null)
                                let g(r,i) =
                                    if i = input then
                                        (receiver, i)
                                    else
                                        (r,i)
                                let z = inputs |> Seq.zipAll receivers |> Seq.map f |> Seq.map g
                                let receivers = z |> Seq.map (fun (x,y)->x) |> Seq.toArray
                                let inputs = z |> Seq.map (fun (x,y)->y) |> Seq.toArray
                                return this.ConfigureInputs(inputs, receivers)
                            |None ->
                                return this.ConfigureInputs(inputs, receivers)
                        with err -> 
                            dbg.Error(err)
                            do! ErrorView.Show(ctx, err) |> Async.Ignore
                            return this.ConfigureInputs(inputs, receivers)
                    }
                    return! cont
                }),
                abort = (fun ()->async{
                    return None
                })
            )
        }

        member private this.ModifyControl(state) = async{
            let! cont = async{
                let selection = state.model.selection |> IfNotNull (fun x-> x.token)
                try
                    let m = new EngineControlsUpdatingView.Model()
                    m.control <- state.model.controlstates.Keys |> Seq.firstOrDefault(fun x-> x.token = selection)
                    let! res = EngineControlsUpdatingView.Show(ctx, m)
                    return res.Handle(
                        finish = (fun m -> this.ModifyControlComplete(m, state.selection)),
                        configureInputs = (fun m -> async{
                            let inputTokens = state.selection.inputToken |> SuppressNull [||]
                            let receiverTokens = state.selection.receiverToken |> SuppressNull [||]
                            let act = new NvaConfigureInputsActivity(ctx)
                            let! newInputs, newReceivers = act.Main(inputTokens, receiverTokens)
                            let newInputTokens = newInputs |> Seq.map (IfNotNull(fun x->x.token))
                            let newReceiverTokens = newReceivers |> Seq.map (IfNotNull(fun x->x.token))
                            let isModified = Seq.exists (not) (seq{
                                yield newInputTokens |> Seq.zipAll inputTokens |> Seq.forall (fun (x,y)-> x=y)
                                yield newReceiverTokens |> Seq.zipAll receiverTokens |> Seq.forall (fun (x,y)-> x=y)
                            })
                            if isModified then
                                use! progress = Progress.Show(ctx, "applying changes...")
                                state.selection.inputToken <- newInputTokens |> Seq.toArray
                                state.selection.receiverToken <- newReceiverTokens |> Seq.toArray
                                do! adev.SetAnalyticsEngineControl(state.selection, true)
                            return! this.Main(selection)
                        }),
                        configureVac = (fun m -> this.Main(selection)),
                        abort = (fun m -> this.Main(selection))
                    )
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main(selection)
                }
            return! cont
        }

        member private this.ModifyControlComplete(config, selection) = async{
            let selectionToken = selection |> IfNotNull(fun x->x.token)
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, "configuring control...")
                    do! adev.SetAnalyticsEngineControl(config, true)
                    if selectionToken = config.token then
                        do! InfoView.Show(ctx, "active control has been modified, nva needs to be reloaded") |> Async.Ignore
                        return this.Complete(NvaControlManagmentActivityResult.Refresh)
                    else
                        return this.Main(selectionToken)
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main(selectionToken)
            }
            return! cont
        }

//        member private this.ModifyControlInputs(config, inputs, selection) = async{
//            let selectionToken = 
//                if selection |> IsNull then
//                    null
//                else 
//                    selection.token
//            let! cont = async{
//                try
//                    let m = new EngineControlsInputsCreationView.Model()
//                    let! res = EngineControlsInputsCreationView.Show(ctx, m);
//                    return! res.Handle(
//                        finish = (fun m -> async{
//                            return this.ModifyControlComplete(config, selection)
//                        }),
//                        modify = (fun x->async{
//                            return this.Main(selectionToken)
//                        }),
//
//                        abort = (fun () -> async{
//                            return this.Main(selectionToken)
//                        })
//                    )
////                    use! progress = Progress.Show(ctx, "configuring control...")
////                    do! adev.SetAnalyticsEngineControl(config, true)
////                    if selection = config.token then
////                        do! InfoView.Show(ctx, "active control has been modified, nva needs to be reloaded") |> Async.Ignore
////                        return this.Complete(NvaControlManagmentActivityResult.Refresh)
////                    else
////                        return this.Main(selection)
//                with err -> 
//                    dbg.Error(err)
//                    do! ErrorView.Show(ctx, err) |> Async.Ignore
//                    return this.Main(selectionToken)
//            }
//            return! cont
//        }

        member private this.DeleteControl(state) = async{
            let! cont = async{
                let selection = if NotNull(state.model.selection) then state.model.selection.token else null
                try
                    do! async{
                        if selection |> NotNull then
                            use! progress = Progress.Show(ctx, "deleting control...")
                            do! adev.DeleteAnalyticsEngineControl(selection)
                    }
                    if selection = state.selection.token then
                        do! InfoView.Show(ctx, "active control has been deleted, nva needs to be reloaded") |> Async.Ignore
                        return this.Complete(NvaControlManagmentActivityResult.Refresh)
                    else
                        return this.Main(selection)
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main(selection)
                }
            return! cont
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx, engine, selection) = async{
            let act = new NvaControlManagmentActivity(ctx, engine)
            return! act.Main(selection)
        }
    end

    type NvaLiveVideoActivity(ctx:IUnityContainer) = class
        inherit BaseActivity<string*string*StreamSetup, AnalyticsLiveVideoView.Model, AnalyticsLiveVideoView.Result>(ctx)
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        let vand = session :> IAnalyticsDeviceAsync

        static member Run(ctx:IUnityContainer, controlToken:string, engineConfToken:string, streamSetup:StreamSetup) = 
            let act = new NvaLiveVideoActivity(ctx)
            act.Main(controlToken, engineConfToken, streamSetup)

        override this.Load((controlToken, engineConfToken, streamSetup)) = async{
            use! progress = this.ShowProgress("loading...")
            let! uri, size = Async.Parallel(
                vand.GetAnalyticsDeviceStreamUri(streamSetup, controlToken),
                async{
                    let! cfgs = Nva.GetControlInputConfigs(vand, controlToken)
                    return cfgs |> Seq.map(IfNotNull(fun cfg->cfg.videoInput |> IfNotNull(fun vi->vi.resolution))) |> Seq.firstOrDefault NotNull
                }
            )
            return new AnalyticsLiveVideoView.Model(
                uri = uri,
                size = size,
                engineConfToken = engineConfToken
            )
        }
        override this.Show(model) = async{
            let! cont = async{
                try
                    let! res = AnalyticsLiveVideoView.Show(ctx, model)
                    return async{return res}
                with err->
                    do! this.ShowError(err)
                    return this.Show(model)
            }
            return! cont
        }
        
    end


    type NvaMetadataSettingsActivity(ctx:IUnityContainer) = class
        inherit BaseActivity<string*string*StreamSetup, AnalyticsMetadataSettingsView.Model, AnalyticsMetadataSettingsView.Result>(ctx)
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        let vand = session :> IAnalyticsDeviceAsync

        static member Run(ctx:IUnityContainer, controlToken:string, engineConfToken:string, streamSetup:StreamSetup) = 
            let act = new NvaMetadataSettingsActivity(ctx)
            act.Main(controlToken, engineConfToken, streamSetup)

        override this.Load((controlToken, engineConfToken, streamSetup)) = async{
            use! progress = this.ShowProgress("loading...")
            let! uri, size = Async.Parallel(
                vand.GetAnalyticsDeviceStreamUri(streamSetup, controlToken),
                async{
                    let! cfgs = Nva.GetControlInputConfigs(vand, controlToken)
                    return cfgs |> Seq.map(IfNotNull(fun cfg->cfg.videoInput |> IfNotNull(fun vi->vi.resolution))) |> Seq.firstOrDefault NotNull
                }
            )
            return new AnalyticsMetadataSettingsView.Model(
                uri = uri,
                size = size,
                engineConfToken = engineConfToken
            )
        }
        override this.Show(model) = async{
            let! cont = async{
                try
                    let! res = AnalyticsMetadataSettingsView.Show(ctx, model)
                    return async{return res}
                with err->
                    do! this.ShowError(err)
                    return this.Show(model)
            }
            return! cont
        }
        
    end


    type NvaSimpleConfigureInputsActivity(ctx:IUnityContainer, control:AnalyticsEngineControl) = class
        inherit BaseActivity<unit, EngineControlSimpleInputModifyView.Model, unit>(ctx)
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        let vand = session :> IAnalyticsDeviceAsync
        let recv = session :> IReceiverAsync

        static member Run(ctx:IUnityContainer, control:AnalyticsEngineControl) = 
            let act = new NvaSimpleConfigureInputsActivity(ctx, control)
            act.Main()

        override this.Load(()) = async{
            use! progress = this.ShowProgress("loading...")
            if control |> IsNull then
                failwith "no control was specified"

            let input_cfg_t = 
                if NotNull(control.inputToken) && control.inputToken.Length = 1 then
                    control.inputToken.[0]
                else
                    failwith "invalid control inputs"

            let receiver_t = 
                if NotNull(control.receiverToken) && control.receiverToken.Length = 1 then
                    control.receiverToken.[0]
                else
                    failwith "invalid control receivers"
                
            let! input_cfg, receivers = Async.Parallel(
                vand.GetAnalyticsEngineInput(input_cfg_t),
                recv.GetReceivers()
            )
            return new EngineControlSimpleInputModifyView.Model(
                receiver = (receivers |> Seq.firstOrDefault(fun r -> r.token = receiver_t)),
                input = input_cfg,
                isEnable = (control.mode = ModeOfOperation.active),
                receivers = receivers
            )
        }
        override this.Show(model) = async{
            let! cont = async{
                try
                    let! res = EngineControlSimpleInputModifyView.Show(ctx, model)
                    let m = res.Handle(fun m->m)
                    return this.ApplyChanges(m)
                with err->
                    do! this.ShowError(err)
                    return this.Main()
            }
            return! cont
        }

        member this.ApplyChanges(model) = async{
            let! cont = async{
                try
                    use! progress = this.ShowProgress("applying changes...")
                    //TODO: do we need to stop analytics control before applying changes...
//                    if control.Mode = ModeOfOperation.Active then
//                        control.Mode <-  ModeOfOperation.Active
//                        do! vand.SetAnalyticsEngineControl(control, true)

                    control.mode <-
                        if model.isEnable then 
                            ModeOfOperation.active
                        else
                            ModeOfOperation.idle
                    if model.receiver <> null then
                        control.receiverToken <- [|model.receiver.token|]
                    else
                        control.receiverToken <- [||]
//                    do! vand.SetAnalyticsEngineInput(model.input, true)
//                    do! vand.SetAnalyticsEngineControl(control, true)
                    do! Async.Ignore(Async.Parallel(
//                        async{
//                            if model.receiver <> null then
//                                recv.ConfigureReceiver(model.receiver.Token, model.receiver.Configuration)
//                        },
                        vand.SetAnalyticsEngineControl(control, true),
                        vand.SetAnalyticsEngineInput(model.input, true)
                    ))

                    return this.Main()
                with err->
                    do! this.ShowError(err)
                    return this.Main()
            }
            return! cont
        }
        
    end
