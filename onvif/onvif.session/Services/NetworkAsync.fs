namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_network
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IDiscoveryLookupAsync = interface
        abstract Probe: request:ProbeRequest -> Async<ProbeResponse>
    end

    [<AllowNullLiteral>]
    type IRemoteDiscoveryAsync = interface
        abstract Hello: request:HelloRequest -> Async<HelloResponse>
        abstract Bye: request:ByeRequest -> Async<ByeResponse>
    end

    type DiscoveryLookupAsync(proxy:DiscoveryLookupPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IDiscoveryLookupAsync with
            member this.Probe(request:ProbeRequest):Async<ProbeResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginProbe, proxy.EndProbe)
                return response
            }
        end
    end

    type RemoteDiscoveryAsync(proxy:RemoteDiscoveryPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IRemoteDiscoveryAsync with
            member this.Hello(request:HelloRequest):Async<HelloResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginHello, proxy.EndHello)
                return response
            }
            member this.Bye(request:ByeRequest):Async<ByeResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginBye, proxy.EndBye)
                return response
            }
        end
    end
