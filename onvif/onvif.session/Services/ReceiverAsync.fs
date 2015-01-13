namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_receiver
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IReceiverAsync = interface
        //onvif 2.1
        abstract GetServiceCapabilities: unit -> Async<Capabilities11>
        abstract GetReceivers: unit -> Async<Receiver[]>
        abstract GetReceiver: receiverToken:string -> Async<Receiver>
        abstract CreateReceiver: receiverConfiguration:ReceiverConfiguration -> Async<Receiver>
        abstract DeleteReceiver: receiverToken:string -> Async<unit>
        abstract ConfigureReceiver: receiverToken:string * receiverConfiguration:ReceiverConfiguration -> Async<unit>
        abstract SetReceiverMode: receiverToken:string * receiverMode:ReceiverMode -> Async<unit>
        abstract GetReceiverState: receiverToken:string -> Async<ReceiverStateInformation>
    end

    type ReceiverAsync(proxy:ReceiverPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IReceiverAsync with
            member this.GetServiceCapabilities():Async<Capabilities11> = async{
                let request = GetServiceCapabilitiesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response.Capabilities
            }
            member this.GetReceivers():Async<Receiver[]> = async{
                let request = GetReceiversRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetReceivers, proxy.EndGetReceivers)
                return response.Receivers
            }
            member this.GetReceiver(receiverToken:string):Async<Receiver> = async{
                let request = GetReceiverRequest()
                request.ReceiverToken <- receiverToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetReceiver, proxy.EndGetReceiver)
                return response.Receiver
            }
            member this.CreateReceiver(receiverConfigurution:ReceiverConfiguration):Async<Receiver> = async{
                let request = CreateReceiverRequest()
                request.Configuration <- receiverConfigurution
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateReceiver, proxy.EndCreateReceiver)
                return response.Receiver
            }
            member this.DeleteReceiver(receiverToken:string):Async<unit> = async{
                let request = DeleteReceiverRequest()
                request.ReceiverToken <- receiverToken
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteReceiver, proxy.EndDeleteReceiver)
                return ()
            }
            member this.ConfigureReceiver(receiverToken:string, receiverConfiguration:ReceiverConfiguration):Async<unit> = async{
                let request = ConfigureReceiverRequest()
                request.ReceiverToken <- receiverToken
                request.Configuration <- receiverConfiguration
                let! response = Async.FromBeginEnd(request, proxy.BeginConfigureReceiver, proxy.EndConfigureReceiver)
                return ()
            }
            member this.SetReceiverMode(receiverToken:string, receiverMode:ReceiverMode):Async<unit> = async{
                let request = SetReceiverModeRequest()
                request.ReceiverToken <- receiverToken
                request.Mode <- receiverMode
                let! response = Async.FromBeginEnd(request, proxy.BeginSetReceiverMode, proxy.EndSetReceiverMode)
                return ()
            }
            member this.GetReceiverState(receiverToken:string):Async<ReceiverStateInformation> = async{
                let request = GetReceiverStateRequest()
                request.ReceiverToken <- receiverToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetReceiverState, proxy.EndGetReceiverState)
                return response.ReceiverState
            }
        end
    end
