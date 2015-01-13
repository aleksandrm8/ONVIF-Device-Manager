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
    //open utils.extensions
    //open odm.models
    open utils.fsharp
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs

    type PtzActivity private(ctx:IUnityContainer, profToken:string) = class
        do if profToken |> IsNull then raise( new ArgumentNullException("profToken") )
        let session = ctx.Resolve<INvtSession>()
        let ptz = session :> IPtzAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        //FIX: D-Link DSC-2230 doesn't support GetNode request
        let CreateStandartPtzNode(nodeToken) = new PTZNode( 
            token = nodeToken,
            name = nodeToken,
            supportedPTZSpaces = new PTZSpaces(
                absolutePanTiltPositionSpace = [|new Space2DDescription(
                    uri = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace",
                    xRange = new FloatRange(min = -1.0f, max = 1.0f),
                    yRange = new FloatRange(min = -1.0f, max = 1.0f)
                )|],
                absoluteZoomPositionSpace = [|new Space1DDescription(
                    uri = "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace",
                    xRange = new FloatRange(min = 0.0f, max = 1.0f)
                )|],
                relativePanTiltTranslationSpace = [|new Space2DDescription(
                    uri = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace",
                    xRange = new FloatRange(min = -1.0f, max = 1.0f),
                    yRange = new FloatRange(min = -1.0f, max = 1.0f)
                )|],
                relativeZoomTranslationSpace = [|new Space1DDescription(
                    uri = "http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace",
                    xRange = new FloatRange(min = -1.0f, max = 1.0f)
                )|],
                continuousPanTiltVelocitySpace = [|new Space2DDescription(
                    uri = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace",
                    xRange = new FloatRange(min = -1.0f, max = 1.0f),
                    yRange = new FloatRange(min = -1.0f, max = 1.0f)
                )|],
                continuousZoomVelocitySpace = [|new Space1DDescription(
                    uri = "http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace",
                    xRange = new FloatRange(min = -1.0f, max = 1.0f)
                )|],
                panTiltSpeedSpace = [|new Space1DDescription(
                    uri = "http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace",
                    xRange = new FloatRange(min = 0.0f, max = 1.0f)
                )|],
                zoomSpeedSpace = [|new Space1DDescription(
                    uri = "http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace",
                    xRange = new FloatRange(min = 0.0f, max = 1.0f)
                )|]
            ),
            homeSupported = false,
            maximumNumberOfPresets = 0,
            auxiliaryCommands = null,
            extension = null,
            fixedHomePosition = false,
            fixedHomePositionSpecified = false
        )
            
        let LoadPtzNode(cfg: PTZConfiguration) = async{
            try
                //FIX: D-Link DSC-2230 doesn't support GetNode request
                return! ptz.GetNode(cfg.nodeToken)
            with err->
                dbg.Error(err)
                return CreateStandartPtzNode(cfg.nodeToken)
        }

        let LoadPtzStatus(token:string) =  async{
            try
                return! ptz.GetStatus(token)
            with err->
                dbg.Error(err)
                return null;
        }

        let load() = async{
            let! (profile, node, status), presets = Async.Parallel(
                async{
                    let! profile = session.GetProfile(profToken)
                    let cfg = profile.ptzConfiguration
                    if cfg |> IsNull then
                        return (profile, null, null)
                    else
                        let! node, status = Async.Parallel(
                            LoadPtzNode(cfg),
                            LoadPtzStatus(profile.token)
                        )
                        return (profile, node, status)
                },
                //ptz.GetNodes(),
                async{
                    try
                        return! ptz.GetPresets(profToken)
                    with err->
                        dbg.Error(err)
                        return [||]
                }
            )
            
            let model = new PtzView.Model(
                profile = profile,
                node = node,
                status = status,
                //nodes = nodes,
                presets = presets
            )
            //model.AcceptChanges()
            return model
        }

        ///<summary></summary>
        member private this.Main() = async{
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
        
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    do! PtzView.Show(ctx, model) |> Async.Ignore
                    return this.Complete()
                with err -> 
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        member private this.Complete(res) = async{
            return res
        }
        
        static member Run(ctx:IUnityContainer, profToken:string) = 
            let act = new PtzActivity(ctx, profToken)
            act.Main()
    end
