namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_analyticsdevice
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IAnalyticsDeviceAsync = interface
        //onvif 2.1
        abstract GetServiceCapabilities: unit -> Async<Capabilities12>
        abstract DeleteAnalyticsEngineControl: configurationToken:string -> Async<unit>
        abstract CreateAnalyticsEngineControl: configuration:AnalyticsEngineControl -> Async<AnalyticsEngineInput[]>
        abstract SetAnalyticsEngineControl: configuration:AnalyticsEngineControl * forcePersistence:bool -> Async<unit>
        abstract GetAnalyticsEngineControl: configurationToken:string -> Async<AnalyticsEngineControl>
        abstract GetAnalyticsEngineControls: unit -> Async<AnalyticsEngineControl[]>
        abstract GetAnalyticsEngine: configurationToken:string -> Async<AnalyticsEngine>
        abstract GetAnalyticsEngines: unit -> Async<AnalyticsEngine[]>
        abstract SetVideoAnalyticsConfiguration: configuration:VideoAnalyticsConfiguration * forcePersistence:bool -> Async<unit>
        abstract SetAnalyticsEngineInput: configuration:AnalyticsEngineInput * forcePersistence:bool -> Async<unit>
        abstract GetAnalyticsEngineInput: configurationToken:string -> Async<AnalyticsEngineInput>
        abstract GetAnalyticsEngineInputs: unit -> Async<AnalyticsEngineInput[]>
        abstract GetAnalyticsDeviceStreamUri: streamSetup:StreamSetup * analyticsEngineControlToken:string -> Async<string>
        abstract GetVideoAnalyticsConfiguration: configurationToken:string -> Async<VideoAnalyticsConfiguration>
        abstract CreateAnalyticsEngineInputs: configuration:AnalyticsEngineInput[] * forcePersistence:bool[] -> Async<AnalyticsEngineInput[]>
        abstract DeleteAnalyticsEngineInputs: configurationToken:string[] -> Async<unit>
        abstract GetAnalyticsState: analyticsEngineControlToken:string -> Async<AnalyticsStateInformation>
    end

    type AnalyticsDeviceAsync(proxy:AnalyticsDevicePort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IAnalyticsDeviceAsync with
            member this.GetServiceCapabilities():Async<Capabilities12> = async{
                let request = new GetServiceCapabilitiesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response |> IfNotNull (fun x->x.Capabilities)
            }
            member this.DeleteAnalyticsEngineControl(configurationToken:string):Async<unit> = async{
                let request = new DeleteAnalyticsEngineControlRequest()
                request.ConfigurationToken <- configurationToken
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteAnalyticsEngineControl, proxy.EndDeleteAnalyticsEngineControl)
                return ()
            }
            member this.CreateAnalyticsEngineControl(configuration:AnalyticsEngineControl):Async<AnalyticsEngineInput[]> = async{
                let request = new CreateAnalyticsEngineControlRequest()
                request.Configuration <- configuration
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateAnalyticsEngineControl, proxy.EndCreateAnalyticsEngineControl)
                return response |> IfNotNull (fun x->x.Configuration)
            }
            member this.SetAnalyticsEngineControl(configuration:AnalyticsEngineControl, forcePersistence:bool):Async<unit> = async{
                let request = new SetAnalyticsEngineControlRequest()
                request.Configuration <- configuration
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetAnalyticsEngineControl, proxy.EndSetAnalyticsEngineControl)
                return ()
            }
            member this.GetAnalyticsEngineControl(configurationToken:string):Async<AnalyticsEngineControl> = async{
                let request = new GetAnalyticsEngineControlRequest()
                request.ConfigurationToken <- configurationToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsEngineControl, proxy.EndGetAnalyticsEngineControl)
                return response |> IfNotNull (fun x->x.Configuration)
            }
            member this.GetAnalyticsEngineControls():Async<AnalyticsEngineControl[]> = async{
                let request = new GetAnalyticsEngineControlsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsEngineControls, proxy.EndGetAnalyticsEngineControls)
                return response |> IfNotNull (fun x->x.AnalyticsEngineControls)
            }
            member this.GetAnalyticsEngine(configurationToken:string):Async<AnalyticsEngine> = async{
                let request = new GetAnalyticsEngineRequest()
                request.ConfigurationToken <- configurationToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsEngine, proxy.EndGetAnalyticsEngine)
                return response |> IfNotNull (fun x->x.Configuration)
            }
            member this.GetAnalyticsEngines():Async<AnalyticsEngine[]> = async{
                let request = new GetAnalyticsEnginesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsEngines, proxy.EndGetAnalyticsEngines)
                return response |> IfNotNull (fun x->x.Configuration)
            }
            member this.SetVideoAnalyticsConfiguration(configuration:VideoAnalyticsConfiguration, forcePersistence:bool):Async<unit> = async{
                let request = new SetVideoAnalyticsConfigurationRequest()
                request.Configuration <- configuration
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetVideoAnalyticsConfiguration, proxy.EndSetVideoAnalyticsConfiguration)
                return ()
            }
            member this.SetAnalyticsEngineInput(configuration:AnalyticsEngineInput, forcePersistence:bool):Async<unit> = async{
                let request = new SetAnalyticsEngineInputRequest()
                request.Configuration <- configuration
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetAnalyticsEngineInput, proxy.EndSetAnalyticsEngineInput)
                return ()
            }
            member this.GetAnalyticsEngineInput(configurationToken:string):Async<AnalyticsEngineInput> = async{
                let request = new GetAnalyticsEngineInputRequest()
                request.ConfigurationToken <- configurationToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsEngineInput, proxy.EndGetAnalyticsEngineInput)
                return response |> IfNotNull (fun x->x.Configuration)
            }
            member this.GetAnalyticsEngineInputs():Async<AnalyticsEngineInput[]> = async{
                let request = new GetAnalyticsEngineInputsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsEngineInputs, proxy.EndGetAnalyticsEngineInputs)
                return response |> IfNotNull (fun x->x.Configuration)
            }
            member this.GetAnalyticsDeviceStreamUri(streamSetup:StreamSetup, analyticsEngineControlToken:string):Async<string> = async{
                let request = new GetAnalyticsDeviceStreamUriRequest()
                request.StreamSetup <- streamSetup
                request.AnalyticsEngineControlToken <- analyticsEngineControlToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsDeviceStreamUri, proxy.EndGetAnalyticsDeviceStreamUri)
                return response |> IfNotNull (fun x->x.Uri)
            }
            member this.GetVideoAnalyticsConfiguration(configurationToken:string):Async<VideoAnalyticsConfiguration> = async{
                let request = new GetVideoAnalyticsConfigurationRequest()
                request.ConfigurationToken <- configurationToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoAnalyticsConfiguration, proxy.EndGetVideoAnalyticsConfiguration)
                return response |> IfNotNull (fun x->x.Configuration)
            }
            member this.CreateAnalyticsEngineInputs(configuration:AnalyticsEngineInput[], forcePersistence:bool[]):Async<AnalyticsEngineInput[]> = async{
                let request = new CreateAnalyticsEngineInputsRequest()
                request.Configuration <- configuration
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateAnalyticsEngineInputs, proxy.EndCreateAnalyticsEngineInputs)
                return response |> IfNotNull (fun x->x.Configuration)
            }
            member this.DeleteAnalyticsEngineInputs(configurationToken:string[]):Async<unit> = async{
                let request = new DeleteAnalyticsEngineInputsRequest()
                request.ConfigurationToken <- configurationToken
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteAnalyticsEngineInputs, proxy.EndDeleteAnalyticsEngineInputs)
                return ()
            }
            member this.GetAnalyticsState(analyticsEngineControlToken:string):Async<AnalyticsStateInformation> = async{
                let request = new GetAnalyticsStateRequest()
                request.AnalyticsEngineControlToken <- analyticsEngineControlToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAnalyticsState, proxy.EndGetAnalyticsState)
                return response |> IfNotNull (fun x->x.State)
            }
        end
    end
