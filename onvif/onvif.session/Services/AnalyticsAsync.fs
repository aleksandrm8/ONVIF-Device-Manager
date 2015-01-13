namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_analytics
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    [<ServiceContract>]
    type IAnalytics = interface
        inherit AnalyticsEnginePort
        inherit RuleEnginePort
    end

    [<AllowNullLiteral>]
    type IAnalyticsEngineAsync = interface
        //onvif 1.2
        abstract GetSupportedAnalyticsModules: vacToken:string->Async<SupportedAnalyticsModules>
        abstract CreateAnalyticsModules: vacToken:string * analyticsModules:Config[]->Async<unit>
        abstract DeleteAnalyticsModules: vacToken:string * analyticsModuleNames:string[]->Async<unit>
        abstract GetAnalyticsModules: vacToken:string->Async<Config[]>
        abstract ModifyAnalyticsModules: vacToken:string * analyticsModules:Config[]->Async<unit>

        //onvif 2.1
//        abstract GetServiceCapabilities: unit -> Async<Capabilities6>
    end

    [<AllowNullLiteral>]
    type IRuleEngineAsync = interface
        abstract GetSupportedRules: vacToken:string->Async<SupportedRules>
        abstract CreateRules: vacToken:string*rules:Config[]->Async<unit>
        abstract DeleteRules: vacToken:string*ruleNames:string[]->Async<unit>
        abstract GetRules: vacToken:string->Async<Config[]>
        abstract ModifyRules: vacToken:string*rules:Config[]->Async<unit>
    end

    [<AllowNullLiteral>]
    type IAnalyticsAsync = interface
        inherit IAnalyticsEngineAsync
        inherit IRuleEngineAsync
    end
    
    type AnalyticsAsync(proxy:IAnalytics) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IAnalyticsAsync

        interface IAnalyticsEngineAsync with
            //onvif 1.2
            member this.GetSupportedAnalyticsModules(vacToken:string):Async<SupportedAnalyticsModules> = async{
                let request = new GetSupportedAnalyticsModulesRequest()
                request.ConfigurationToken <- vacToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSupportedAnalyticsModules, proxy.EndGetSupportedAnalyticsModules)
                return response.SupportedAnalyticsModules
            }
            member this.CreateAnalyticsModules(vacToken:string, analyticsModules:Config[]):Async<unit> = async{
                let request = new CreateAnalyticsModulesRequest()
                request.ConfigurationToken <- vacToken
                request.AnalyticsModule <- analyticsModules
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateAnalyticsModules, proxy.EndCreateAnalyticsModules)
                return ()
            }
            member this.DeleteAnalyticsModules(vacToken:string, analyticsModuleNames:string[]):Async<unit> = async{
                let request = new DeleteAnalyticsModulesRequest()
                request.ConfigurationToken <- vacToken
                request.AnalyticsModuleName <- analyticsModuleNames
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteAnalyticsModules, proxy.EndDeleteAnalyticsModules)
                return ()
            }
            member this.GetAnalyticsModules(vacToken:string):Async<Config[]> = async{
                let request = new GetAnalyticsModulesRequest()
                request.ConfigurationToken <- vacToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsModules, proxy.EndGetAnalyticsModules)
                return response.AnalyticsModule
            }
            member this.ModifyAnalyticsModules(vacToken:string, analyticsModules:Config[]):Async<unit> = async{
                let request = new ModifyAnalyticsModulesRequest()
                request.ConfigurationToken <- vacToken
                request.AnalyticsModule <- analyticsModules
                let! response = Async.FromBeginEnd(request, proxy.BeginModifyAnalyticsModules, proxy.EndModifyAnalyticsModules)
                return ()
            }
            //onvif 2.1
//            member this.GetServiceCapabilities():Async<Capabilities6> = async{
//                let request = new GetServiceCapabilitiesRequest()
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
//                return response.Capabilities
//            }
        end

        interface IRuleEngineAsync with
            member this.GetSupportedRules(vacToken:string):Async<SupportedRules> = async{
                let request = new GetSupportedRulesRequest()
                request.ConfigurationToken <- vacToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSupportedRules, proxy.EndGetSupportedRules)
                return response.SupportedRules
            }
            member this.CreateRules(vacToken:string, rules:Config[]):Async<unit> = async{
                let request = new CreateRulesRequest()
                request.ConfigurationToken <- vacToken
                request.Rule <- rules
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateRules, proxy.EndCreateRules)
                return ()
            }
            member this.DeleteRules(vacToken:string, ruleNames:string[]):Async<unit> = async{
                let request = new DeleteRulesRequest()
                request.ConfigurationToken <- vacToken
                request.RuleName <- ruleNames
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteRules, proxy.EndDeleteRules)
                return ()
            }
            member this.GetRules(vacToken:string):Async<Config[]> = async{
                let request = new GetRulesRequest()
                request.ConfigurationToken <- vacToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRules, proxy.EndGetRules)
                return response.Rule
            }
            member this.ModifyRules(vacToken:string, rules:Config[]):Async<unit> = async{
                let request = new ModifyRulesRequest()
                request.ConfigurationToken <- vacToken
                request.Rule <- rules
                let! response = Async.FromBeginEnd(request, proxy.BeginModifyRules, proxy.EndModifyRules)
                return ()
            }
        end
    end
