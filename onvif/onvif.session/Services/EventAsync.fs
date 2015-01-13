namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Net
    open System.Text
    open System.Xml
    open System.Reflection
    open System.ServiceModel
    open System.ServiceModel.Channels
//    open onvif.services.events
//    open onvif.types
    open onvif.services
    open onvif10_events
    open wsn_bw_2
    open onvif
    
    //open odm.infra
    open utils
    open utils.fsharp
    type WcfHelper = class
        
        static member CorrectError<'T>(comp:Async<'T>):Async<'T> = async{
            let processWebResponse(response:HttpWebResponse, originException:Exception) = 
                try
                    use stream = response.GetResponseStream()
                    use xmlReader = XmlReader.Create(stream)
                    let msg = Message.CreateMessage(xmlReader, Int32.MaxValue, MessageVersion.Soap12WSAddressing10)
                    if msg.IsFault then
                        let fault = MessageFault.CreateFault(msg, Int32.MaxValue)
                        //if msg.Headers.Action = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessagesResponse" then 
                        if msg.Headers.Action = "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessages/Fault/PullMessagesFaultResponse" then
                            if fault.HasDetail then
                                let detailReader = fault.GetReaderAtDetailContents() :> XmlReader
                                let detail = detailReader.Deserialize<PullMessagesFaultResponse>(new XmlQualifiedName("PullMessagesFaultResponse", "http://www.onvif.org/ver10/events/wsdl"))
                                new FaultException<PullMessagesFaultResponse>(detail, fault.Reason, fault.Code, "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessages/Fault/PullMessagesFaultResponse") :> Exception
                            else
                                new FaultException<PullMessagesFaultResponse>(null, fault.Reason, fault.Code, "http://www.onvif.org/ver10/events/wsdl/PullPointSubscription/PullMessages/Fault/PullMessagesFaultResponse") :> Exception
                        else
    //                      let attr = expectedType.GetCustomAttributes(expectedType, true).OfType<PullMessagesFaultResponse>()
                        //expectedType.Attributes 
                            new FaultException(fault) :> Exception
                    else
                        originException
                with err ->
                    originException

            let processWebException(webException:WebException, originException:Exception) = 
                match webException.Response with
                | null -> originException
                | :? HttpWebResponse as response -> processWebResponse(response, originException)
                | _ -> originException
            
            try
                return! comp
            with 
            | :? ProtocolException as protocolException->
                return raise(
                    //let webException = CastAs<WebException>(protocolException.InnerException)
                    match protocolException.InnerException with
                        | null -> protocolException :> Exception
                        | :? WebException as webException -> processWebException(webException, protocolException)
                        | _ -> protocolException :> Exception
                )
            | err ->
                return raise(err)
        }
        
    end

    [<AllowNullLiteral>]
    [<ServiceContract>]
    type IEvent = interface
        inherit NotificationProducer
        inherit EventPortType
    end

    [<AllowNullLiteral>]
    type IEventAsync= interface
        //EventPortType onvif 1.2
        abstract CreatePullPointSubscription: filter:FilterType * initialTerminationTime:string * subscriptionPolicy:CreatePullPointSubscriptionSubscriptionPolicy * any:XmlElement[] -> Async<CreatePullPointSubscriptionResponse>
        abstract GetEventProperties: unit -> Async<GetEventPropertiesResponse>
        
        //EventPortType onvif 2.1
        //abstract GetServiceCapabilities: unit -> Async<Capabilities2>
        
        //NotificationProducer
        abstract Subscribe: consumerReference:EndpointReferenceType1* filter:FilterType* initialTerminationTime:string* subscriptionPolicy:SubscribeSubscriptionPolicy -> Async<onvif.services.SubscribeResponse>
        abstract GetCurrentMessage: topic:TopicExpressionType -> Async<unit>
    end

    [<AllowNullLiteral>]
    [<ServiceContract>]
    type ISubscriptionManager = interface
        inherit PullPointSubscription
        inherit SubscriptionManager
    end

    [<AllowNullLiteral>]
    type IPullPointSubscriptionAsync = interface
        abstract PullMessages: timeout:XsDuration * messageLimit:int * any:XmlElement[] -> Async<PullMessagesResponse>
        abstract SetSynchronizationPoint: unit -> Async<unit>
    end

    [<AllowNullLiteral>]
    type ISubscriptionManagerAsync = interface
        abstract PullMessages: timeout:XsDuration * messageLimit:int -> Async<PullMessagesResponse>
        //abstract PullMessages: timeout:string * messageLimit:string -> Async<PullMessagesResponse>
        abstract SetSynchronizationPoint: unit -> Async<unit>
        abstract Renew: terminationTime:string * any:XmlElement[]->Async<onvif.services.RenewResponse>;
        abstract Unsubscribe: unit->Async<unit>
    end
   
    type SubscriptionManagerAsync(proxy:ISubscriptionManager) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface ISubscriptionManagerAsync with
            member this.Renew(terminationTime:string, any:XmlElement[]): Async<onvif.services.RenewResponse> = async{
                let request = new RenewRequest()
                request.Renew <- new Renew(TerminationTime = terminationTime, Any = any)
                let! response = Async.FromBeginEnd(request, proxy.BeginRenew, proxy.EndRenew)
                return response.RenewResponse1
            }
            member this.Unsubscribe(): Async<unit> = async{
                let request = new UnsubscribeRequest()
                request.Unsubscribe <- new Unsubscribe()
                let! response = Async.FromBeginEnd(request, proxy.BeginUnsubscribe, proxy.EndUnsubscribe)
                return ()
            }
            member this.PullMessages(timeout:XsDuration, messageLimit:int): Async<PullMessagesResponse> = WcfHelper.CorrectError(async{
                let request = new PullMessagesRequest()
                request.Timeout <- timeout.Format()
                request.MessageLimit <- messageLimit
                //request.Any <- any
                let! response = Async.FromBeginEnd(request, proxy.BeginPullMessages, proxy.EndPullMessages)
                return response
            })
            member this.SetSynchronizationPoint(): Async<unit> = async{
                let request = new SetSynchronizationPointRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginSetSynchronizationPoint, proxy.EndSetSynchronizationPoint)
                return ()
            }
        end
    end 

    type EventAsync(proxy:IEvent) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IEventAsync with
            //EventPortType onvif 1.2
            member this.CreatePullPointSubscription(filter:FilterType, initialTerminationTime:string, subscriptionPolicy:CreatePullPointSubscriptionSubscriptionPolicy, any:XmlElement[]): Async<CreatePullPointSubscriptionResponse> = async{
                let request = new CreatePullPointSubscriptionRequest()
                request.Filter <- filter
                request.InitialTerminationTime <- initialTerminationTime
                request.SubscriptionPolicy <- subscriptionPolicy
                request.Any <- any
                let! response = Async.FromBeginEnd(request, proxy.BeginCreatePullPointSubscription, proxy.EndCreatePullPointSubscription)
                return response
            }
            member this.GetEventProperties(): Async<GetEventPropertiesResponse> = async{
                let request = new GetEventPropertiesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetEventProperties, proxy.EndGetEventProperties)
                return response
            }
            //EventPortType onvif 2.1
//            member this.GetServiceCapabilities():Async<Capabilities2> = async{
//                let request = new GetServiceCapabilitiesRequest()
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
//                return response.Capabilities
//            }
            //NotificationProducer
            member this.Subscribe(consumerReference:EndpointReferenceType1, filter:FilterType, initialTerminationTime:string, subscriptionPolicy:SubscribeSubscriptionPolicy): Async<onvif.services.SubscribeResponse> = async{
                let request = new SubscribeRequest()
                request.Subscribe <- new Subscribe()
                request.Subscribe.ConsumerReference <- consumerReference
                request.Subscribe.Filter <- filter
                request.Subscribe.InitialTerminationTime <- initialTerminationTime
                request.Subscribe.SubscriptionPolicy <- subscriptionPolicy
                let! response = Async.FromBeginEnd(request, proxy.BeginSubscribe, proxy.EndSubscribe)
                return response.SubscribeResponse1
            }
            member this.GetCurrentMessage(topic:TopicExpressionType): Async<unit> = async{
                let request = new GetCurrentMessageRequest()
                request.GetCurrentMessage <- new GetCurrentMessage()
                request.GetCurrentMessage.Topic <- topic
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCurrentMessage, proxy.EndGetCurrentMessage)
                return ()
            }
        end
    end

    type PullPointSubscriptionAsync(proxy:PullPointSubscription) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        interface IPullPointSubscriptionAsync with
            member this.PullMessages(timeout:XsDuration, messageLimit:int, any:XmlElement[]): Async<PullMessagesResponse> = WcfHelper.CorrectError(async{
                let request = new PullMessagesRequest()
                request.Timeout <- timeout.Format()
                request.MessageLimit <- messageLimit
                request.Any <- any
                
                let! response = Async.FromBeginEnd(request, proxy.BeginPullMessages, proxy.EndPullMessages)
                return response
            })
            member this.SetSynchronizationPoint(): Async<unit> = async{
                let request = new SetSynchronizationPointRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginSetSynchronizationPoint, proxy.EndSetSynchronizationPoint)
                return ()
            }
        end
    end
