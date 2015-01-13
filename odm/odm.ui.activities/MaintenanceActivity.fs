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

    type MaintenanceActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            let! devInfo, caps, isFirmwareUpgradeSupported = Async.Parallel(
                dev.GetDeviceInformation(),
                dev.GetCapabilities(),
                facade.IsFirmwareUpgradeSupported()
            )
            return new MaintenanceView.Model(
                firmwareVersion = devInfo.FirmwareVersion,
                capabilities = caps,
                isFirmwareUpgradeSupported = isFirmwareUpgradeSupported
            )
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

        ///<summary></summary>
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = MaintenanceView.Show(ctx, model)
                    return res.Handle(
                        backup = (fun backupPath-> this.Backup(model, backupPath)),
                        restore = (fun backupPath-> this.Restore(model, backupPath)),
                        softReset = (fun ()-> this.Reset(model, FactoryDefaultType.soft)),
                        hardReset = (fun ()-> this.Reset(model, FactoryDefaultType.hard)),
                        reboot = (fun ()-> this.Reboot(model)),
                        upgrade = (fun firmwarePath-> this.Upgrade(model, firmwarePath)),
                        close = (fun ()-> this.Complete())
                    )
                with err -> 
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        ///<summary></summary>
        member private this.Backup(model, backupPath) = async{
            try
                use! progress = Progress.Show(ctx, LocalMaintenance.instance.doingBackup)
                let! files = dev.GetSystemBackup()
                do! Async.SwitchToThreadPool()
                use zip = Package.Open(backupPath, FileMode.Create)
                for file in files do
                    let uri = PackUriHelper.CreatePartUri(new Uri(file.name, UriKind.Relative))
                    if zip.PartExists(uri) then
                        zip.DeletePart(uri)
                    let contentType = 
                        if NotNull(file.data) && NotNull(file.data.contentType) then
                            file.data.contentType
                        else
                            ""
                    let part = zip.CreatePart(uri, contentType)
                    if NotNull(file.data) && NotNull(file.data.Include) then
                        use ps = part.GetStream()
                        do! ps.AsyncWrite(file.data.Include, 0, file.data.Include.GetLength(0))
            with err ->
                do! show_error(err)

            return! this.Main()
        }

        ///<summary></summary>
        member private this.Restore(model, backupPath) = async{
            Async.StartImmediate(async{
                use ctx = new ModalDialogContext() :> IUnityContainer
                try
                    let! msg = async{
                        use! progress = Progress.Show(ctx, "restoring system...")
                        do! facade.RestoreSystem(backupPath)
                        return LocalMaintenance.instance.restoreSuccess;
                    }
                    do! InfoView.Show(ctx, msg) |> Async.Ignore
                with err ->
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
            })
            return! this.Main()
        }

        ///<summary></summary>
        member private this.Reset(model, resetType) = async{
            Async.StartImmediate(async{
                use ctx = new ModalDialogContext(LocalDevice.instance.information) :> IUnityContainer
                try
                    let! msg = async{
                        use! progress = Progress.Show(ctx, LocalMaintenance.instance.resetting)
                        do! dev.SetSystemFactoryDefault(resetType)
                        if resetType = FactoryDefaultType.soft then
                            return LocalMaintenance.instance.factorySoftSuccess; 
                        else
                            return LocalMaintenance.instance.factoryHardSuccess; 
                    }
                    do! InfoView.Show(ctx, msg) |> Async.Ignore
                with err->
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
            })
            return! this.ShowForm(model)
        }

        ///<summary></summary>
        member private this.Reboot(model) = async{
            Async.StartImmediate(async{
                use ctx = new ModalDialogContext() :> IUnityContainer
                try
                    let! msg = async{
                        use! progress = Progress.Show(ctx, LocalMaintenance.instance.rebooting)
                        let! response = dev.SystemReboot()
                        if not(String.IsNullOrWhiteSpace(response)) then
                            return String.Concat(response, LocalMaintenance.instance.waitWhileAppear)
                        else
                            return LocalMaintenance.instance.rebootSuccess;
                    }
                    do! InfoView.Show(ctx, msg) |> Async.Ignore
                with err ->
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
            })
            return! this.ShowForm(model)
        }

        ///<summary></summary>
        member private this.Upgrade(model, firmwarePath) = async{
            Async.StartImmediate(async{
                use ctx = new ModalDialogContext() :> IUnityContainer
                try
                    ctx.RegisterInstance<INvtSession>(session) |> ignore
                    do! UpgradeFirmwareActivity.Run(ctx, firmwarePath)
                with err->
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
            })
            return! this.ShowForm(model)
        }

        ///<summary></summary>
        member private this.Complete(result) = async{
            return result
        }
        
        static member Run(ctx) = 
            let act = new MaintenanceActivity(ctx)
            act.Main()
    end
