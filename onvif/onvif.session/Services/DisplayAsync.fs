namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_display
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IDisplayAsync = interface
        //onvif 2.1
        abstract GetServiceCapabilities: request:GetServiceCapabilitiesRequest -> Async<GetServiceCapabilitiesResponse>
        abstract GetLayout: request:GetLayoutRequest -> Async<GetLayoutResponse>
        abstract SetLayout: request:SetLayoutRequest -> Async<SetLayoutResponse>
        abstract GetDisplayOptions: request:GetDisplayOptionsRequest -> Async<GetDisplayOptionsResponse>
        abstract GetPaneConfigurations: request:GetPaneConfigurationsRequest -> Async<GetPaneConfigurationsResponse>
        abstract GetPaneConfiguration: request:GetPaneConfigurationRequest -> Async<GetPaneConfigurationResponse>
        abstract SetPaneConfigurations: request:SetPaneConfigurationsRequest -> Async<SetPaneConfigurationsResponse>
        abstract SetPaneConfiguration: request:SetPaneConfigurationRequest -> Async<SetPaneConfigurationResponse>
        abstract CreatePaneConfiguration: request:CreatePaneConfigurationRequest -> Async<CreatePaneConfigurationResponse>
        abstract DeletePaneConfiguration: request:DeletePaneConfigurationRequest -> Async<DeletePaneConfigurationResponse>
    end

    type DisplayAsync(proxy:DisplayPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IDisplayAsync with
            member this.GetServiceCapabilities(request:GetServiceCapabilitiesRequest):Async<GetServiceCapabilitiesResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response
            }
            member this.GetLayout(request:GetLayoutRequest):Async<GetLayoutResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetLayout, proxy.EndGetLayout)
                return response
            }
            member this.SetLayout(request:SetLayoutRequest):Async<SetLayoutResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetLayout, proxy.EndSetLayout)
                return response
            }
            member this.GetDisplayOptions(request:GetDisplayOptionsRequest):Async<GetDisplayOptionsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetDisplayOptions, proxy.EndGetDisplayOptions)
                return response
            }
            member this.GetPaneConfigurations(request:GetPaneConfigurationsRequest):Async<GetPaneConfigurationsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetPaneConfigurations, proxy.EndGetPaneConfigurations)
                return response
            }
            member this.GetPaneConfiguration(request:GetPaneConfigurationRequest):Async<GetPaneConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetPaneConfiguration, proxy.EndGetPaneConfiguration)
                return response
            }
            member this.SetPaneConfigurations(request:SetPaneConfigurationsRequest):Async<SetPaneConfigurationsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetPaneConfigurations, proxy.EndSetPaneConfigurations)
                return response
            }
            member this.SetPaneConfiguration(request:SetPaneConfigurationRequest):Async<SetPaneConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetPaneConfiguration, proxy.EndSetPaneConfiguration)
                return response
            }
            member this.CreatePaneConfiguration(request:CreatePaneConfigurationRequest):Async<CreatePaneConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginCreatePaneConfiguration, proxy.EndCreatePaneConfiguration)
                return response
            }
            member this.DeletePaneConfiguration(request:DeletePaneConfigurationRequest):Async<DeletePaneConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginDeletePaneConfiguration, proxy.EndDeletePaneConfiguration)
                return response
            }
        end
    end
