//module VideoSettingsActivity
namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Linq
    open System.Net
    open System.Reactive.Disposables
    open System.Text
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    open System.Xml
    open System.Xml.Linq

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

    type MetadataSettingsActivity(ctx:IUnityContainer, profile:Profile, session:INvtSession, metaCfgOpts:MetadataConfigurationOptions) = class
        static let xmlns_wsnt = "http://docs.oasis-open.org/wsn/b-2"
        let mutable metaCfg = profile.metadataConfiguration
        let isPtzStatusSupported = 
            if NotNull(metaCfgOpts) && NotNull(metaCfgOpts.ptzStatusFilterOptions) then
                let opts = metaCfgOpts.ptzStatusFilterOptions
                opts.zoomStatusSupported || opts.panTiltStatusSupported
            else 
                false
        let isPtzPositionSupported = 
            if NotNull(metaCfgOpts) && NotNull(metaCfgOpts.ptzStatusFilterOptions) then
                let opts = metaCfgOpts.ptzStatusFilterOptions
                (opts.panTiltPositionSupportedSpecified && opts.panTiltPositionSupported) || 
                (opts.zoomPositionSupportedSpecified && opts.zoomPositionSupported)
            else 
                false
        let media = session :> IMediaAsync
        let device = session :> IDeviceAsync
        let event = session :> IEventAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            //refresh MetadataConfiguration of active profile
            let! newMetaCfg = session.GetMetadataConfiguration(metaCfg.token)
            metaCfg <- newMetaCfg
            
            let! caps = device.GetCapabilities()
            let! isEventsSupported = facade.IsEventsSupported()
            
            let! model = async{
                if isEventsSupported then
                    let! eventProps = event.GetEventProperties()
                    //let! messageContentSchemaSet = facade.DownloadSchemes(eventProps.MessageContentSchemaLocation)
                    return new MetadataSettingsView.Model(
                        messageContentFilterDialects = eventProps.MessageContentFilterDialect,
                        topicExpressionDialects = eventProps.TopicExpressionDialect,
                        topicSet = eventProps.TopicSet,
                        isFixedTopicSet = eventProps.FixedTopicSet,
                        //messageContentSchemaSet = messageContentSchemaSet,
                        isPtzStatusSupported = isPtzStatusSupported,
                        isPtzPositionSupported = isPtzPositionSupported
                    )
                else
                    return new MetadataSettingsView.Model(
                        messageContentFilterDialects = [||],
                        topicExpressionDialects = [||],
                        topicSet = null,
                        isFixedTopicSet = false,
                        //messageContentSchemaSet = null,
                        isPtzStatusSupported = isPtzStatusSupported,
                        isPtzPositionSupported = isPtzPositionSupported
                    )
            }

            if metaCfg.analyticsSpecified then
                model.includeAnalitycs <- metaCfg.analytics
            else
                model.includeAnalitycs <- false

            if metaCfg.ptzStatus |> NotNull then
                model.includePtzStatus <- metaCfg.ptzStatus.status
                model.includePtzPosition <- metaCfg.ptzStatus.position
            else
                model.includePtzStatus <- false
                model.includePtzPosition <- false
            
            //according [ONVIF-Media-Service-Spec-v210] topic 5.21.27 "MetadataConfiguration",
            //To get no events: Do not include the Events element.
            if metaCfg.events |> NotNull then
                model.includeEvents <- true //see [ONVIF-Media-Service-Spec-v210] topic 5.21.27 "MetadataConfiguration"
                model.topicExpressionFilters <- [|
                    let any = metaCfg.events |> IfNotNull(fun x->x.filter |> IfNotNull(fun x-> x.Any))
                    for x in any |> SuppressNull [||] do
                        if x.Name = XName.Get("TopicExpression", xmlns_wsnt) then
                            yield x.Deserialize<TopicExpressionFilter>()
                |]
                model.messageContentFilters <- [|
                    let any = metaCfg.events |> IfNotNull(fun x->x.filter |> IfNotNull(fun x-> x.Any))
                    for x in any |> SuppressNull [||] do
                        if x.Name = XName.Get("MessageContent", xmlns_wsnt) then
                            yield x.Deserialize<MessageContentFilter>()
                |]
            else
                model.includeEvents <- false //see [ONVIF-Media-Service-Spec-v210] topic 5.21.27 "MetadataConfiguration"
                model.topicExpressionFilters <- null
                model.messageContentFilters <- null

            model.AcceptChanges()
            return model
        }

        let apply(model:MetadataSettingsView.Model) = async{
            
            let! isAnalyticsSupported = facade.IsAnalyticsSupported()
            if isAnalyticsSupported then
                metaCfg.analyticsSpecified <- true
                metaCfg.analytics <- model.includeAnalitycs

            if isPtzStatusSupported || isPtzPositionSupported then
                metaCfg.ptzStatus <- new PTZFilter(
                    position = model.includePtzPosition,
                    status = model.includePtzStatus
                )
            if model.includeEvents then
                if metaCfg.events |> IsNull then
                    metaCfg.events <- new EventSubscription()
                let filters = [|
                    if model.messageContentFilters |> NotNull then
                        for x in model.messageContentFilters do
                            yield XmlExtensions.SerializeAsXElement(x)

                    if model.topicExpressionFilters |> NotNull then
                        for x in model.topicExpressionFilters do
                            yield XmlExtensions.SerializeAsXElement(x)
                |]
                if filters.Length>0 then
                    metaCfg.events.filter <- new FilterType()
                    metaCfg.events.filter.Any <- filters
                else
                    metaCfg.events.filter <- null //see [ONVIF-Media-Service-Spec-v210] topic 5.21.27 "MetadataConfiguration"
            else
                metaCfg.events <- null //see [ONVIF-Media-Service-Spec-v210] topic 5.21.27 "MetadataConfiguration"

            do! media.SetMetadataConfiguration(metaCfg, true)
            model.AcceptChanges()
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
                    return this.Main()
            }
            return! cont
        }

        ///<summary></summary>
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = MetadataSettingsView.Show(ctx, model)
                    return res.Handle(
                        apply = (fun model-> 
                            if model.isModified then 
                                this.ApplyModel(model)
                            else 
                                this.ShowForm(model)
                        ),
                        none = (fun ()->this.Complete())
                    )
                with err -> 
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        ///<summary></summary>
        member private this.ApplyModel(model) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        do! apply(model)
                    }
                    return this.Main()
                with err ->
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        ///<summary></summary>
        member private this.Complete(result) = async{
            return result
        }

        ///<summary></summary>
        static member Run(ctx:IUnityContainer, profile:Profile) = async{
            let! cont = async{
                try
                    let session = ctx.Resolve<INvtSession>()
                    let! act = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        let! metaCfgOpts = session.GetMetadataConfigurationOptions(profile.metadataConfiguration.token, profile.token)
                        return new MetadataSettingsActivity(ctx, profile, session, metaCfgOpts)
                    }
                    return act.Main()
                with err -> 
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return MetadataSettingsActivity.Run(ctx, profile)
            }
            return! cont
        }
    end
