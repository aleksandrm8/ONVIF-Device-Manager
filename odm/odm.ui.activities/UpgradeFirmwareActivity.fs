//module VideoSettingsActivity
namespace odm.ui.activities
    open System
    open System.Linq
    //open System.Disposables
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Net.Mime
    open System.Windows
    open System.Windows.Media
    open System.Windows.Threading
    open System.IO
    open System.IO.Packaging
    open System.Reactive.Disposables
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
    open odm.ui
    open odm.ui.views
    open odm.ui.controls
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.dialogs
    
    type UpgradeFirmwareActivity(ctx:IUnityContainer, firmwarePath:string) = class
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        let load() = async{
            return ()
        }
        
        ///<summary></summary>
        member private this.Main() = async{
            let! cont = async{
                try
                    return this.ShowForm()
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        ///<summary></summary>
        member private this.ShowForm(model) = async{
            try
                let! res = UpgradeFirmwareView.Show(ctx, fun()->facade.UpgradeFirmware(firmwarePath)|>Async.Ignore)
                return! res.Handle(
                    completed = (fun (displayCompleteNotification)-> async{
                        if displayCompleteNotification then
                            do! InfoView.Show(ctx, LocalMaintenance.instance.upgradeCompleteSuccess) |> Async.Ignore
                        return! this.Complete()
                    }),
                    background = (fun disposable -> async{
                        //do! InfoView.Show(ctx, "continued in background") |> Async.Ignore
                        return! this.Complete()
                    }),
                    canceled = (fun ()-> this.Complete())
                )
            with err -> 
                do! show_error(err)
                return! this.Complete()
        }
        
        ///<summary></summary>
        member private this.Complete(result) = async{
            return result
        }
        
        static member Run(ctx, firmwarePath) = 
            let act = new UpgradeFirmwareActivity(ctx, firmwarePath)
            act.Main()
    end
