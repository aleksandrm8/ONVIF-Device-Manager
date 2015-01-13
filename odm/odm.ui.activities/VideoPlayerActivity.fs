//module VideoSettingsActivity
namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Linq
    open System.Net
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.core
    open odm.infra
    open utils
    open utils.fsharp

    type VideoPlayerActivity(ctx:IUnityContainer, model: VideoPlayerActivityModel) = class
        do if model |> IsNull then raise(new ArgumentNullException("model"))
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        member private this.Main() = async{
            let! cont = async{
                try
                    let! profile = async{
                        if model.profile |> NotNull then
                            return model.profile
                        else
                            return! session.GetProfile(model.profileToken)
                    }
                    let encoderResolution = 
                        let vec = profile.videoEncoderConfiguration
                        if vec |> NotNull then
                            vec.resolution
                        else
                            null

                    let! mediaUri = session.GetStreamUri(model.streamSetup, model.profileToken)
                    let viewModel = new VideoPlayerView.Model(
                        streamSetup = model.streamSetup,
                        mediaUri = mediaUri,
                        encoderResolution = encoderResolution,
                        isUriEnabled = model.showStreamUrl,
                        metadataReceiver = model.metadataReceiver
                    )
                    do! VideoPlayerView.Show(ctx, viewModel) |> Async.Ignore
                    return async{return ()}
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx, model: VideoPlayerActivityModel) = 
            let act = new VideoPlayerActivity(ctx, model)
            act.Main()

        static member Create() = {
            new IVideoPlayerActivity with
                member this.Run(ctx:IUnityContainer, model: VideoPlayerActivityModel) =
                    VideoPlayerActivity.Run(ctx, model)
            end
        }
        
    end
