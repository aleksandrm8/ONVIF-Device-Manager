namespace odm.ui.activities
    open System
    open System.Xml
    open System.Xml.Schema
    open System.Xml.Serialization
    open System.Xml.Linq
    open System.Linq
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Net.Mime
    open System.Windows
    open System.Windows.Threading
    open System.Linq
//    open System.Disposables
//    open System.Concurrency

    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.core
    open odm.infra
    open utils
    //open odm.models
    open utils.fsharp
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    type AnalyticsActivity(ctx:IUnityContainer, vacToken:string) = class
        do if vacToken |> IsNull then raise( new ArgumentNullException("vac token does not specified") )
        let session = ctx.Resolve<INvtSession>()
        let van = session :> IAnalyticsAsync
        let dev = session :> IDeviceAsync
        let facade = new OdmSession(session)
        
        let load() = async{
            return! async{
                let! caps = dev.GetCapabilities()
                if caps.analytics |> NotNull then
                    //TODO: do this in parallel
                    let! (modules, moduleTypes, moduleSchemes) = async{
                        if caps.analytics.analyticsModuleSupport then
                            let! modules, moduleTypes = Async.Parallel(
                                van.GetAnalyticsModules(vacToken),
                                van.GetSupportedAnalyticsModules(vacToken)
                            )
                            let! moduleSchemes = facade.DownloadSchemes(moduleTypes.analyticsModuleContentSchemaLocation)
                            return (modules, moduleTypes.analyticsModuleDescription, moduleSchemes)
                        else
                            return ([||], [||], new XmlSchemaSet())
                    }

                    let! (rules, ruleTypes, ruleSchemes) = async{
                        if caps.analytics.ruleSupport then
                            let! rules, ruleTypes = Async.Parallel(
                                van.GetRules(vacToken),
                                van.GetSupportedRules(vacToken)
                            )
                            let! ruleSchemes = facade.DownloadSchemes(ruleTypes.ruleContentSchemaLocation)
                            return (rules, ruleTypes.ruleDescription, ruleSchemes)
                        else
                            return ([||], [||], new XmlSchemaSet())
                    }
                    return new AnalyticsView.Model(
                        rules = rules, 
                        ruleTypes = ruleTypes,
                        ruleSchemes = ruleSchemes, 
                        modules = modules,
                        moduleTypes = moduleTypes,
                        moduleSchemes = moduleSchemes
                    )
                else
                    return new AnalyticsView.Model(
                        rules = [||], 
                        ruleTypes = [||],
                        ruleSchemes = new XmlSchemaSet(), 
                        modules = [||],
                        moduleTypes = [||],
                        moduleSchemes = new XmlSchemaSet()
                    )
            }
        }

        ///<summary></summary>
        member private this.Main() = async{
            let! cont = async{
                try
                    return! async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        let! model = load()
                        return this.ShowForm(model)
                    }
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main()
            }
            return! cont
        }
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = AnalyticsView.Show(ctx, model)
                    return res.Handle(
                        createModule = (fun()  -> 
                            this.CreateModule(model)
                        ),
                        configureModule = (fun moduleCfg -> 
                            this.ConfigModule(model, moduleCfg)
                        ),
                        deleteModule = (fun moduleName -> 
                            this.DeleteModule(model, moduleName)
                        ),
                        createRule = (fun ruleName -> 
                            this.CreateRule(model)
                        ),
                        configureRule = (fun ruleCfg -> 
                            this.ConfigRule(model, ruleCfg)
                        ),
                        deleteRule = (fun ruleName ->
                            this.DeleteRule(model, ruleName)
                        ),
                        close = (fun () ->
                            this.Complete()
                        )
                    )
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Complete()
            }
            return! cont
        }
        member private this.CreateDefaultConfig(name:string, description:ConfigDescription, schemaSet: XmlSchemaSet, existingConfigs: Config[]) = 
            let cfg = new Config()
            cfg.xmlns <- new XmlSerializerNamespaces()
            cfg.xmlns.Add("tt", "http://www.onvif.org/ver10/schema")
            cfg.name <- name
            cfg.``type`` <- description.name
            let simpleItems = Seq.toList(seq{
                let sids = description.parameters |> IfNotNull (fun x->x.simpleItemDescription)
                for sid in sids |> SuppressNull [||] do
                    let item = new ItemList.SimpleItem()
                    item.name <- sid.name
                    item.value <- 
                        if sid.``type``.Namespace = XmlSchema.Namespace then
                            let simpleType = XmlSchemaSimpleType()
                            ProtoSchemeGenerator.CreateProtoXsdType(sid.``type``.Name)
                        else
                            let simpleTypes = schemaSet.GlobalTypes.Values.OfType<XmlSchemaSimpleType>()
                            let simpleType = simpleTypes |> Seq.find(fun x->x.QualifiedName =  sid.``type``)
                            ProtoSchemeGenerator.CreateProtoSimpleType(simpleType)
                    yield item
            })
            let elementItems = Seq.toList(seq{
                let eids = description.parameters |> IfNotNull (fun x->x.elementItemDescription) 
                for eid in eids |> SuppressNull [||] do
                    let item = new ItemList.ElementItem()
                    item.name <- eid.name
                    let schemaElements = schemaSet.GlobalElements.Values.OfType<XmlSchemaElement>()
                    let schemaElement = schemaElements |> Seq.tryFind(fun t-> t.QualifiedName = eid.``type``)
                    item.any <- 
                        match schemaElement with
                        | Some sel ->
                            ProtoSchemeGenerator.CreateProtoElement(sel).ToXmlElement()
                        | None ->
                            let err = new Exception(String.Format("scheme definition for element {0} is missing", eid.``type``))
                            dbg.Error(err)
                            raise err
                    //item.Any <- get_element_default(eid.Type)
                    yield item
            })
            cfg.parameters <- new ItemList()
            cfg.parameters.simpleItem <- simpleItems |> List.toArray
            cfg.parameters.elementItem <- elementItems |> List.toArray
            cfg
            
            
        member private this.CreateConfig(name:string, description:ConfigDescription, schemaSet: XmlSchemaSet, existingConfigs: Config[]) = 
            let defaultConfig = this.CreateDefaultConfig(name, description, schemaSet, existingConfigs)
            let customConfig = ctx.Resolve<odm.ui.core.IConfiguratorFactory>().Create(description.name).Configure(defaultConfig, existingConfigs)
            customConfig


        member private this.CreateModule(model) = 
            let rec set_name() = 
                async{
                    let! cont = async{
                        try
                            let vm = new AnalyticsSetNameView.Model(
                                types = model.moduleTypes
                            )
                            let! res = AnalyticsSetNameView.Show(ctx, vm)
                            return res.Handle(
                                configure = (fun name description ->
                                    configure(name, description)
                                ),
                                abort = (fun()-> 
                                    this.ShowForm(model)
                                )
                            )
                        with err ->
                            dbg.Error(err)
                            do! ErrorView.Show(ctx, err) |> Async.Ignore
                            return this.ShowForm(model)
                    }
                    return! cont
                } 
            and configure(name, description) = 
                async{
                    let! cont = async{
                        try
                            let vm = new ConfigureAnalyticView.Model(
                                config = this.CreateConfig(name, description, model.moduleSchemes, existingConfigs = model.modules),
                                configDescription = description,
                                schemes = model.moduleSchemes
                            )
                            let! res = ConfigureAnalyticView.Show(ctx, vm)
                            return res.Handle(
                                apply = (fun m->
                                    create(m.config)
                                ),
                                abort = (fun()->
                                    this.ShowForm(model)
                                )
                            )
                        with err ->
                            dbg.Error(err)
                            do! ErrorView.Show(ctx, err) |> Async.Ignore
                            return this.ShowForm(model)
                    }
                    return! cont
                }
            and create(config) = 
                async{
                    try
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        do! van.CreateAnalyticsModules(vacToken, [|config|])
                    with err ->
                        dbg.Error(err)
                        do! ErrorView.Show(ctx, err) |> Async.Ignore

                    return! this.Main()
                }
                
            set_name()

        member private this.CreateRule(model) = 
            let rec set_name() = 
                async{
                    let! cont = async{
                        try
                            let vm = 
                                new AnalyticsSetNameView.Model(
                                    types = model.ruleTypes
                                )
                            let! res = AnalyticsSetNameView.Show(ctx, vm)
                            return res.Handle(
                                configure = (fun name description ->
                                    configure(name, description)
                                ),
                                abort = (fun()-> 
                                    this.ShowForm(model)
                                )
                            )
                        with err ->
                            dbg.Error(err)
                            do! ErrorView.Show(ctx, err) |> Async.Ignore
                            return this.ShowForm(model)
                    }
                    return! cont
                } 
            and configure(name, description) = 
                async{
                    let! cont = async{
                        try
                            let vm = new ConfigureAnalyticView.Model(
                                config = this.CreateConfig(name, description, model.ruleSchemes, existingConfigs = model.rules),
                                configDescription = description,
                                schemes = model.ruleSchemes
                            )
                            let! res = ConfigureAnalyticView.Show(ctx, vm)
                            return res.Handle(
                                apply = (fun m->
                                    create(m.config)
                                ),
                                abort = (fun()->
                                    this.ShowForm(model)
                                )
                            )
                        with err ->
                            dbg.Error(err)
                            do! ErrorView.Show(ctx, err) |> Async.Ignore
                            return this.ShowForm(model)
                    }
                    return! cont
                }
            and create(config) = 
                async{
                    try
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        do! van.CreateRules(vacToken, [|config|])
                    with err ->
                        dbg.Error(err)
                        do! ErrorView.Show(ctx, err) |> Async.Ignore

                    return! this.Main()
                }

            set_name()
        member private this.ConfigModule(model, moduleCfg) = async{
            let! cont = async{
                try
                    let vm = new ConfigureAnalyticView.Model(
                        config = moduleCfg,
                        configDescription = (
                            model.moduleTypes |> Seq.find(fun x->
                                x.name = moduleCfg.``type``
                            )
                        ),
                        schemes = model.moduleSchemes
                    )
                    let! res = ConfigureAnalyticView.Show(ctx, vm)
                    return res.Handle(
                        apply = (fun m->
                            this.ApplyModuleChanges(model, m.config)
                        ),
                        abort = (fun()->
                            this.ShowForm(model)
                        )
                    )
                with err ->
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.ShowForm(model)
            }
            return! cont
        }
        member private this.ConfigRule(model, ruleCfg) = async{
            let! cont = async{
                try
                    let vm = new ConfigureAnalyticView.Model(
                        config = ruleCfg,
                        configDescription = (
                            model.ruleTypes |> Seq.find(fun x-> x.name = ruleCfg.``type``)
                        ),
                        schemes = model.ruleSchemes
                    )
                    let! res = ConfigureAnalyticView.Show(ctx, vm)
                    return res.Handle(
                        apply = (fun m->
                            this.ApplyRuleChanges(model, m.config)
                        ),
                        abort = (fun()->
                            this.ShowForm(model)
                        )
                    )
                with err ->
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        member private this.ApplyModuleChanges(model, config) = async{
            try
                use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                do! van.ModifyAnalyticsModules(vacToken, [|config|])
            with err ->
                dbg.Error(err)
                do! ErrorView.Show(ctx, err) |> Async.Ignore
            
            return! this.Main()
        }
        
        member private this.ApplyRuleChanges(model, config) = async{
            try
                use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                do! van.ModifyRules(vacToken, [|config|])
            with err ->
                dbg.Error(err)
                do! ErrorView.Show(ctx, err) |> Async.Ignore
            
            return! this.Main()
        }

        member private this.DeleteModule(model, moduleName) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalDevice.instance.deleting)
                    do! van.DeleteAnalyticsModules(vacToken, [|moduleName|])
                    return this.Main()
                with err ->
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main()
            }
            return! cont
        }

        member private this.DeleteRule(model, ruleName) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalDevice.instance.deleting)
                    do! van.DeleteRules(vacToken, [|ruleName|])
                    return this.Main()
                with err ->
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return this.Main()
            }
            return! cont
        }

        member private this.Complete(res) = async{
            return res
        }

        static member Run(ctx, vacToken) = 
            let act = new AnalyticsActivity(ctx, vacToken)
            act.Main()
    end

