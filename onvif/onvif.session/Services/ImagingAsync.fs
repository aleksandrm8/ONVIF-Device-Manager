namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.ServiceModel

    open onvif.services
    open onvif10_imaging
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IImagingAsync= interface
        //onvif 1.2
        abstract GetImagingSettings: videoSourceToken:string->Async<ImagingSettings20>
        abstract SetImagingSettings: videoSourceToken:string * imagingSettings:ImagingSettings20 * forcePersistence:bool->Async<unit>
        abstract GetOptions: videoSourceToken:string->Async<ImagingOptions20>
        abstract Move: videoSourceToken:string * focus:FocusMove->Async<unit>
        abstract GetMoveOptions: videoSourceToken:string->Async<MoveOptions20>
        abstract Stop: videoSourceToken:string->Async<unit>
        abstract GetStatus: videoSourceToken:string->Async<ImagingStatus20>

        //onvif 2.1
//        abstract GetServiceCapabilities: unit -> Async<Capabilities3>
    end

    type ImagingAsync(proxy:ImagingPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy

        interface IImagingAsync with
            member this.GetImagingSettings(videoSourceToken:string):Async<ImagingSettings20> = async{
                let request = new GetImagingSettingsRequest()
                request.VideoSourceToken <- videoSourceToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetImagingSettings, proxy.EndGetImagingSettings)
                return response.ImagingSettings
            }

            member this.SetImagingSettings(videoSourceToken:string, imagingSettings:ImagingSettings20, forcePersistence:bool):Async<unit> = async{
                let request = new SetImagingSettingsRequest()
                request.VideoSourceToken <- videoSourceToken
                request.ImagingSettings <- imagingSettings
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetImagingSettings, proxy.EndSetImagingSettings)
                return ()
            }

            member this.GetOptions(videoSourceToken:string):Async<ImagingOptions20> = async{
                let request = new GetOptionsRequest()
                request.VideoSourceToken <- videoSourceToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetOptions, proxy.EndGetOptions)
                return response.ImagingOptions
            }

            member this.Move(videoSourceToken:string, focus:FocusMove):Async<unit> = async{
                let request = new MoveRequest()
                request.VideoSourceToken <- videoSourceToken
                request.Focus <- focus
                let! response = Async.FromBeginEnd(request, proxy.BeginMove, proxy.EndMove)
                return ()
            }

            member this.GetMoveOptions(videoSourceToken:string):Async<MoveOptions20> = async{
                let request = new GetMoveOptionsRequest()
                request.VideoSourceToken <- videoSourceToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetMoveOptions, proxy.EndGetMoveOptions)
                return response.MoveOptions
            }

            member this.Stop(videoSourceToken:string):Async<unit> = async{
                let request = new StopRequest()
                request.VideoSourceToken <- videoSourceToken
                let! response = Async.FromBeginEnd(request, proxy.BeginStop, proxy.EndStop)
                return ()
            }

            member this.GetStatus(videoSourceToken:string):Async<ImagingStatus20> = async{
                let request = new GetStatusRequest()
                request.VideoSourceToken <- videoSourceToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetStatus, proxy.EndGetStatus)
                return response.Status
            }

            //onvif 2.1
//            member this.GetServiceCapabilities(): Async<Capabilities3> = async{
//                let request = new GetServiceCapabilitiesRequest()
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
//                return response.Capabilities
//            }
        end
    end
