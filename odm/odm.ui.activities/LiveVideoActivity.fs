//module VideoSettingsActivity
namespace odm.ui.activities
    open System
    open System.Linq
    //open System.Disposables
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.core
    open odm.infra
    open utils
    //open odm.models
    open utils.fsharp
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    type LiveVideoActivity(ctx:IUnityContainer, profToken:string, videoSourceToken:string) = class
        do if profToken |> IsNull then raise( new ArgumentNullException("profToken") )
        do if videoSourceToken |> IsNull then raise( new ArgumentNullException("videoSourceToken") )
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        member private this.Main() = async{
            let! cont = async{
                try
                    let session = ctx.Resolve<INvtSession>()
                    let! profile = session.GetProfile(profToken)
                    let vec = 
                        if profile.videoEncoderConfiguration |> NotNull then
                            profile.videoEncoderConfiguration
                        else
                            failwith "the profile has no video encoder configuration"

                    let videoSourceConfToken = if profile.videoSourceConfiguration |> NotNull then profile.videoSourceConfiguration.token else null
                    let videoAnalyticsConfToken = if profile.videoAnalyticsConfiguration |> NotNull then profile.videoAnalyticsConfiguration.token else null

                    let model = new LiveVideoView.Model(
                        videoSourceToken = videoSourceToken,
                        profToken = profToken,
                        videoSourceConfToken = videoSourceConfToken,
                        videoAnalyticsConfToken = videoAnalyticsConfToken,
                        encoderResolution = vec.resolution
                    )
                    return this.ShowForm(model)
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = LiveVideoView.Show(ctx, model)
                    return res.Handle(
                        close = (fun ()->this.Complete())
                    )
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        member private this.Complete(result) = async{
            return result
        }
        
        static member Run(ctx:IUnityContainer, profToken:string, videoSourceToken:string) = 
            let act = new LiveVideoActivity(ctx, profToken, videoSourceToken)
            act.Main()
    end
