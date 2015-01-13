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
        abstract Probe: types:seq<XmlQualifiedName> * scopes:ScopesType->Async<ProbeMatchType[]>
    end

    type DiscoveryLookupAsync(proxy:DiscoveryLookupPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IDiscoveryLookupAsync with
            member this.Probe(types:seq<XmlQualifiedName>, scopes:ScopesType):Async<ProbeMatchType[]> = async{
                let request = new ProbeRequest()
                request.Types <- new QNameListType(types)
                request.Scopes <- scopes
                let! response = Async.FromBeginEnd(request, proxy.BeginProbe, proxy.EndProbe)
                return response.ProbeMatch
            }
        end
    end
