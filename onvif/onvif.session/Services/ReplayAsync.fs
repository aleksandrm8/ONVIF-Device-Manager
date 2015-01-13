namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_replay
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IReplayAsync = interface
        //onvif 2.1
        abstract GetServiceCapabilities: unit -> Async<Capabilities10>
        abstract GetReplayUri: recordingToken:string * streamSetup:StreamSetup -> Async<string>
        abstract GetReplayConfiguration: unit -> Async<ReplayConfiguration>
        abstract SetReplayConfiguration: replayConfiguration:ReplayConfiguration -> Async<unit>
    end

    type ReplayAsync(proxy:ReplayPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IReplayAsync with
            member this.GetServiceCapabilities():Async<Capabilities10> = async{
                let request = new GetServiceCapabilitiesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response.Capabilities
            }
            member this.GetReplayUri(recordingToken:string, streamSetup:StreamSetup):Async<string> = async{
                let request = new GetReplayUriRequest()
                request.RecordingToken <- recordingToken
                if streamSetup |> NotNull then
                    request.StreamSetup <- streamSetup
                else
                    request.StreamSetup <- new StreamSetup(
                        transport = new Transport(protocol = TransportProtocol.udp),
                        stream = StreamType.rtpUnicast
                    )

                let! response = Async.FromBeginEnd(request, proxy.BeginGetReplayUri, proxy.EndGetReplayUri)
                return response.Uri
            }
            member this.GetReplayConfiguration():Async<ReplayConfiguration> = async{
                let request = new GetReplayConfigurationRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetReplayConfiguration, proxy.EndGetReplayConfiguration)
                return response.Configuration
            }
            member this.SetReplayConfiguration(replayConfiguration:ReplayConfiguration):Async<unit> = async{
                let request = new SetReplayConfigurationRequest()
                request.Configuration <- replayConfiguration
                let! response = Async.FromBeginEnd(request, proxy.BeginSetReplayConfiguration, proxy.EndSetReplayConfiguration)
                return ()
            }
        end
    end
