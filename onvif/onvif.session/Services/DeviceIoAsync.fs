namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_deviceio
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IDeviceIoAsync = interface
        //onvif 2.1
        abstract GetServiceCapabilities: request:GetServiceCapabilitiesRequest -> Async<GetServiceCapabilitiesResponse>
        abstract GetRelayOutputOptions: request:GetRelayOutputOptionsRequest -> Async<GetRelayOutputOptionsResponse>
        abstract GetAudioSources: request:GetAudioSourcesRequest -> Async<GetAudioSourcesResponse>
        abstract GetAudioOutputs: request:GetAudioOutputsRequest -> Async<GetAudioOutputsResponse>
        abstract GetVideoSources: request:GetVideoSourcesRequest -> Async<GetVideoSourcesResponse>
        abstract GetVideoOutputs: request:GetVideoOutputsRequest -> Async<GetVideoOutputsResponse>
        abstract GetVideoSourceConfiguration: request:GetVideoSourceConfigurationRequest -> Async<GetVideoSourceConfigurationResponse>
        abstract GetVideoOutputConfiguration: request:GetVideoOutputConfigurationRequest -> Async<GetVideoOutputConfigurationResponse>
        abstract GetAudioSourceConfiguration: request:GetAudioSourceConfigurationRequest -> Async<GetAudioSourceConfigurationResponse>
        abstract GetAudioOutputConfiguration: request:GetAudioOutputConfigurationRequest -> Async<GetAudioOutputConfigurationResponse>
        abstract SetVideoSourceConfiguration: request:SetVideoSourceConfigurationRequest -> Async<SetVideoSourceConfigurationResponse>
        abstract SetVideoOutputConfiguration: request:SetVideoOutputConfigurationRequest -> Async<SetVideoOutputConfigurationResponse>
        abstract SetAudioSourceConfiguration: request:SetAudioSourceConfigurationRequest -> Async<SetAudioSourceConfigurationResponse>
        abstract SetAudioOutputConfiguration: request:SetAudioOutputConfigurationRequest -> Async<SetAudioOutputConfigurationResponse>
        abstract GetVideoSourceConfigurationOptions: request:GetVideoSourceConfigurationOptionsRequest -> Async<GetVideoSourceConfigurationOptionsResponse>
        abstract GetVideoOutputConfigurationOptions: request:GetVideoOutputConfigurationOptionsRequest -> Async<GetVideoOutputConfigurationOptionsResponse>
        abstract GetAudioSourceConfigurationOptions: request:GetAudioSourceConfigurationOptionsRequest -> Async<GetAudioSourceConfigurationOptionsResponse>
        abstract GetAudioOutputConfigurationOptions: request:GetAudioOutputConfigurationOptionsRequest -> Async<GetAudioOutputConfigurationOptionsResponse>
        abstract GetRelayOutputs: request:GetRelayOutputsRequest -> Async<GetRelayOutputsResponse>
        abstract SetRelayOutputSettings: request:SetRelayOutputSettingsRequest -> Async<SetRelayOutputSettingsResponse>
        abstract SetRelayOutputState: request:SetRelayOutputStateRequest -> Async<SetRelayOutputStateResponse>
    end

    type DeviceIoAsync(proxy:DeviceIOPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IDeviceIoAsync with
            member this.GetServiceCapabilities(request:GetServiceCapabilitiesRequest):Async<GetServiceCapabilitiesResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response
            }
            member this.GetRelayOutputOptions(request:GetRelayOutputOptionsRequest):Async<GetRelayOutputOptionsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRelayOutputOptions, proxy.EndGetRelayOutputOptions)
                return response
            }
            member this.GetAudioSources(request:GetAudioSourcesRequest):Async<GetAudioSourcesResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioSources, proxy.EndGetAudioSources)
                return response
            }
            member this.GetAudioOutputs(request:GetAudioOutputsRequest):Async<GetAudioOutputsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioOutputs, proxy.EndGetAudioOutputs)
                return response
            }
            member this.GetVideoSources(request:GetVideoSourcesRequest):Async<GetVideoSourcesResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoSources, proxy.EndGetVideoSources)
                return response
            }
            member this.GetVideoOutputs(request:GetVideoOutputsRequest):Async<GetVideoOutputsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoOutputs, proxy.EndGetVideoOutputs)
                return response
            }
            member this.GetVideoSourceConfiguration(request:GetVideoSourceConfigurationRequest):Async<GetVideoSourceConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoSourceConfiguration, proxy.EndGetVideoSourceConfiguration)
                return response
            }
            member this.GetVideoOutputConfiguration(request:GetVideoOutputConfigurationRequest):Async<GetVideoOutputConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoOutputConfiguration, proxy.EndGetVideoOutputConfiguration)
                return response
            }
            member this.GetAudioSourceConfiguration(request:GetAudioSourceConfigurationRequest):Async<GetAudioSourceConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioSourceConfiguration, proxy.EndGetAudioSourceConfiguration)
                return response
            }
            member this.GetAudioOutputConfiguration(request:GetAudioOutputConfigurationRequest):Async<GetAudioOutputConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioOutputConfiguration, proxy.EndGetAudioOutputConfiguration)
                return response
            }
            member this.SetVideoSourceConfiguration(request:SetVideoSourceConfigurationRequest):Async<SetVideoSourceConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetVideoSourceConfiguration, proxy.EndSetVideoSourceConfiguration)
                return response
            }
            member this.SetVideoOutputConfiguration(request:SetVideoOutputConfigurationRequest):Async<SetVideoOutputConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetVideoOutputConfiguration, proxy.EndSetVideoOutputConfiguration)
                return response
            }
            member this.SetAudioSourceConfiguration(request:SetAudioSourceConfigurationRequest):Async<SetAudioSourceConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetAudioSourceConfiguration, proxy.EndSetAudioSourceConfiguration)
                return response
            }
            member this.SetAudioOutputConfiguration(request:SetAudioOutputConfigurationRequest):Async<SetAudioOutputConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetAudioOutputConfiguration, proxy.EndSetAudioOutputConfiguration)
                return response
            }
            member this.GetVideoSourceConfigurationOptions(request:GetVideoSourceConfigurationOptionsRequest):Async<GetVideoSourceConfigurationOptionsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoSourceConfigurationOptions, proxy.EndGetVideoSourceConfigurationOptions)
                return response
            }
            member this.GetVideoOutputConfigurationOptions(request:GetVideoOutputConfigurationOptionsRequest):Async<GetVideoOutputConfigurationOptionsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoOutputConfigurationOptions, proxy.EndGetVideoOutputConfigurationOptions)
                return response
            }
            member this.GetAudioSourceConfigurationOptions(request:GetAudioSourceConfigurationOptionsRequest):Async<GetAudioSourceConfigurationOptionsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioSourceConfigurationOptions, proxy.EndGetAudioSourceConfigurationOptions)
                return response
            }
            member this.GetAudioOutputConfigurationOptions(request:GetAudioOutputConfigurationOptionsRequest):Async<GetAudioOutputConfigurationOptionsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioOutputConfigurationOptions, proxy.EndGetAudioOutputConfigurationOptions)
                return response
            }
            member this.GetRelayOutputs(request:GetRelayOutputsRequest):Async<GetRelayOutputsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRelayOutputs, proxy.EndGetRelayOutputs)
                return response
            }
            member this.SetRelayOutputSettings(request:SetRelayOutputSettingsRequest):Async<SetRelayOutputSettingsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetRelayOutputSettings, proxy.EndSetRelayOutputSettings)
                return response
            }
            member this.SetRelayOutputState(request:SetRelayOutputStateRequest):Async<SetRelayOutputStateResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetRelayOutputState, proxy.EndSetRelayOutputState)
                return response
            }
        end
    end
