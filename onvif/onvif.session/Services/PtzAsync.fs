namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.ServiceModel

    open onvif.services
    open onvif20_ptz
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IPtzAsync= interface
        //onvif 1.2
        abstract GetNodes: unit->Async<PTZNode[]>
        abstract GetNode: nodeToken:string->Async<PTZNode>
        abstract GetConfiguration: configurationToken:string->Async<PTZConfiguration>
        abstract GetConfigurations: unit->Async<PTZConfiguration[]>
        abstract SetConfiguration: ptzConfiguration:PTZConfiguration*forcePersistance:bool->Async<unit>
        abstract GetConfigurationOptions: configurationToken:string->Async<PTZConfigurationOptions>
        abstract SendAuxiliaryCommand: profileToken:string*auxiliaryData:string->Async<string>
        abstract GetPresets: profileToken:string->Async<PTZPreset[]>
        abstract SetPreset: profileToken:string*presetName:string*presetToken:string->Async<string>
        abstract RemovePreset: profileToken:string*presetToken:string->Async<unit>
        abstract GotoPreset: profileToken:string*presetToken:string*speed:PTZSpeed->Async<unit>
        abstract GotoHomePosition: profileToken:string*speed:PTZSpeed->Async<unit>
        abstract SetHomePosition: profileToken:string->Async<unit>
        abstract ContinuousMove: profileToken:string*velocity:PTZSpeed*timeout:XsDuration->Async<unit>
        abstract RelativeMove: profileToken:string*translation:PTZVector*speed:PTZSpeed->Async<unit>
        abstract GetStatus: profileToken:string->Async<PTZStatus>
        abstract AbsoluteMove: profileToken:string*position:PTZVector*speed:PTZSpeed->Async<unit>
        abstract Stop: profileToken:string*panTilt:bool*zoom:bool->Async<unit>

        //onvif 2.1
//        abstract GetServiceCapabilities: unit -> Async<Capabilities1>
    end

    type PtzAsync(proxy:PTZ) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy

        interface IPtzAsync with
            //onvif 1.2
            member this.GetNodes():Async<PTZNode[]> = async{
                let request = new GetNodesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetNodes, proxy.EndGetNodes)
                return response.PTZNode
            }
            member this.GetNode(nodeToken:string):Async<PTZNode> = async{
                let request = new GetNodeRequest()
                request.NodeToken <- nodeToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetNode, proxy.EndGetNode)
                return response.PTZNode
            }
            member this.GetConfiguration(configurationToken:string):Async<PTZConfiguration> = async{
                let request = new GetConfigurationRequest()
                request.PTZConfigurationToken <- configurationToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetConfiguration, proxy.EndGetConfiguration)
                return response.PTZConfiguration
            }
            member this.GetConfigurations():Async<PTZConfiguration[]> = async{
                let request = new GetConfigurationsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetConfigurations, proxy.EndGetConfigurations)
                return response.PTZConfiguration
            }
            member this.SetConfiguration(ptzConfiguration:PTZConfiguration, forcePersistance:bool):Async<unit> = async{
                let request = new SetConfigurationRequest()
                request.PTZConfiguration <- ptzConfiguration
                request.ForcePersistence <- forcePersistance
                let! response = Async.FromBeginEnd(request, proxy.BeginSetConfiguration, proxy.EndSetConfiguration)
                return ()
            }
            member this.GetConfigurationOptions(configurationToken:string):Async<PTZConfigurationOptions> = async{
                let request = new GetConfigurationOptionsRequest()
                request.ConfigurationToken <- configurationToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetConfigurationOptions, proxy.EndGetConfigurationOptions)
                return response.PTZConfigurationOptions
            }
            member this.SendAuxiliaryCommand(profileToken:string, auxiliaryData:string):Async<string> = async{
                let request = new SendAuxiliaryCommandRequest()
                request.ProfileToken <- profileToken
                request.AuxiliaryData <- auxiliaryData
                let! response = Async.FromBeginEnd(request, proxy.BeginSendAuxiliaryCommand, proxy.EndSendAuxiliaryCommand)
                return response.AuxiliaryResponse
            }
            member this.GetPresets(profileToken:string):Async<PTZPreset[]> = async{
                let request = new GetPresetsRequest()
                request.ProfileToken <- profileToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetPresets, proxy.EndGetPresets)
                return response.Preset
            }
            member this.SetPreset(profileToken:string, presetName:string, presetToken:string):Async<string> = async{
                let request = new SetPresetRequest()
                request.ProfileToken <- profileToken
                request.PresetName <- presetName
                request.PresetToken <- presetToken
                let! response = Async.FromBeginEnd(request, proxy.BeginSetPreset, proxy.EndSetPreset)
                return response.PresetToken
            }
            member this.RemovePreset(profileToken:string, presetToken:string):Async<unit> = async{
                let request = new RemovePresetRequest()
                request.ProfileToken <- profileToken
                request.PresetToken <- presetToken
                let! response = Async.FromBeginEnd(request, proxy.BeginRemovePreset, proxy.EndRemovePreset)
                return ()
            }
            member this.GotoPreset(profileToken:string, presetToken:string, speed:PTZSpeed):Async<unit> = async{
                let request = new GotoPresetRequest()
                request.ProfileToken <- profileToken
                request.PresetToken <- presetToken
                request.Speed <- speed
                let! response = Async.FromBeginEnd(request, proxy.BeginGotoPreset, proxy.EndGotoPreset)
                return ()
            }
            member this.GotoHomePosition(profileToken:string, speed:PTZSpeed):Async<unit> = async{
                let request = new GotoHomePositionRequest()
                request.ProfileToken <- profileToken
                request.Speed <- speed
                let! response = Async.FromBeginEnd(request, proxy.BeginGotoHomePosition, proxy.EndGotoHomePosition)
                return ()
            }
            member this.SetHomePosition(profileToken:string):Async<unit> = async{
                let request = new SetHomePositionRequest()
                request.ProfileToken <- profileToken
                let! response = Async.FromBeginEnd(request, proxy.BeginSetHomePosition, proxy.EndSetHomePosition)
                return ()
            }
            member this.ContinuousMove(profileToken:string, velocity:PTZSpeed, timeout:XsDuration):Async<unit> = async{
                let request = new ContinuousMoveRequest()
                request.ProfileToken <- profileToken
                request.Velocity <- velocity
                if timeout |> NotNull then
                    request.Timeout <- timeout.Format()
                let! response = Async.FromBeginEnd(request, proxy.BeginContinuousMove, proxy.EndContinuousMove)
                return ()
            }
            member this.RelativeMove(profileToken:string, translation:PTZVector, speed:PTZSpeed):Async<unit> = async{
                let request = new RelativeMoveRequest()
                request.ProfileToken <- profileToken
                request.Translation <- translation
                request.Speed <- speed
                let! response = Async.FromBeginEnd(request, proxy.BeginRelativeMove, proxy.EndRelativeMove)
                return ()
            }
            member this.GetStatus(profileToken:string):Async<PTZStatus> = async{
                let request = new GetStatusRequest()
                request.ProfileToken <- profileToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetStatus, proxy.EndGetStatus)
                return response.PTZStatus
            }
            member this.AbsoluteMove(profileToken:string, position:PTZVector, speed:PTZSpeed):Async<unit> = async{
                let request = new AbsoluteMoveRequest()
                request.ProfileToken <- profileToken
                request.Position <- position
                request.Speed <- speed
                let! response = Async.FromBeginEnd(request, proxy.BeginAbsoluteMove, proxy.EndAbsoluteMove)
                return ()
            }
            member this.Stop(profileToken:string, panTilt:bool, zoom:bool):Async<unit> = async{
                let request = new StopRequest()
                request.ProfileToken <- profileToken
                request.PanTilt <- panTilt
                request.Zoom <- zoom
                let! response = Async.FromBeginEnd(request, proxy.BeginStop, proxy.EndStop)
                return ()
            }

            //onvif 2.1
//            member this.GetServiceCapabilities(): Async<Capabilities1> = async{
//                let request = new GetServiceCapabilitiesRequest()
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
//                return response.Capabilities
//            }

        end

    end
