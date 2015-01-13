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
    open odm.ui
    open utils.fsharp
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    //TODO:get rid of extra dependencies, use videoSourceToken instead of profToken
    type ImagingSettingsActivity(ctx:IUnityContainer, profToken:string) = class
        do if profToken |> IsNull then raise( new ArgumentNullException("profToken") )
        let session = ctx.Resolve<INvtSession>()
        let img = session :> IImagingAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            
            let! profile = session.GetProfile(profToken)
            let videoSourceToken = profile.videoSourceConfiguration.sourceToken
            let! options, settings, moveOptions = Async.Parallel(
                async{
                    try
                        return! img.GetOptions(videoSourceToken)
                    with err ->
                        dbg.Error(err)
                        return null
                },
                async{
                    try
                        return! img.GetImagingSettings(videoSourceToken)
                    with err ->
                        dbg.Error(err)
                        return null
                },
                async{
                    try
                        return! img.GetMoveOptions(videoSourceToken)
                    with err ->
                        dbg.Error(err)
                        return null
                }
            )
            
            let model = new ImagingSettingsView.Model(
                profToken = profile.token,
                sourceToken = profile.videoSourceConfiguration.sourceToken,
                options = options,
                moveOptions = moveOptions
            )
            model.settings <- settings
            model.AcceptChanges()
            return model
        }

        let apply_changes(model:ImagingSettingsView.Model) = async{
            let settings = model.current.settings
            let! profile = session.GetProfile(profToken)
            let videoSourceToken = profile.videoSourceConfiguration.sourceToken
            do! img.SetImagingSettings(videoSourceToken, settings, true)
        }
        
        ///<summary></summary>
        member this.Main() = async{
            let! cont = async{
                try
                    let! model = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! load()
                    }
                    return this.ShowForm(model)
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        member this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = ImagingSettingsView.Show(ctx, model)
                    return res.Handle(
                        apply = (fun model-> 
                            //if model.isModified then 
                                this.ApplyModel(model)
                            //else 
                                //this.ShowForm(model)
                        ),
                        none = (fun ()->this.Complete())
                    )
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member this.ApplyModel(model) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        do! apply_changes(model)
                    }
                    return this.Main()
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        member this.Complete(result) = async{
            return result
        }
        
        static member Run(ctx, profToken) = 
            let act = new ImagingSettingsActivity(ctx, profToken)
            act.Main()
    end
