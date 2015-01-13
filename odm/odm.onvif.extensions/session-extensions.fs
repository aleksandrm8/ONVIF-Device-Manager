namespace odm.infra
    open System
    open System.Collections.Generic
    open System.ComponentModel
    open System.IO
    open System.IO.Packaging
    open System.Linq
    open System.Net
    open System.Net.Mime
    open System.Net.Sockets
    open System.ServiceModel
    open System.Text
    open System.Threading
    open System.Xml
    open System.Xml.Linq
    open System.Xml.Schema
    open System.Reactive.Disposables
    open System.Reactive.Linq
    open System.Runtime.CompilerServices

    open odm.models

    open onvif.services
    open onvif.utils

    open utils
    open utils.fsharp

    [<Extension>]
    type OdmSessionExtensions private() = class
        
        [<Extension>]
        static member GetIdentity(odmSession:OdmSession, factory:Func<IChangeTrackable<IIdentificationModel>>):Async<IChangeTrackable<IIdentificationModel>> = async{
            let session = odmSession.GetSession()
            let change_trackable = factory.Invoke()
            let model = change_trackable.origin
            let! devInfo, scopes, netstat = Async.Parallel(
                session.GetDeviceInformation(),
                session.GetScopes(),
                odmSession.GetNetworkStatus()
            )
            model.name <- ScopeHelper.GetName(scopes |> Seq.map (fun x->x.scopeItem))
            model.location <- ScopeHelper.GetLocation(scopes |> Seq.map (fun x->x.scopeItem))
            model.iconUri <- ScopeHelper.GetDeviceIconUri(scopes |> Seq.map (fun x->x.scopeItem));
            model.firmware <- devInfo.FirmwareVersion
            model.hardware <- devInfo.HardwareId
            model.serial <- devInfo.SerialNumber
            model.manufacturer <- devInfo.Manufacturer
            model.model <- devInfo.Model
            //model.host
            model.mac  <- String.Join(", ", netstat.nics |> Seq.map (fun nic -> nic.mac))
            model.ip  <- String.Join(", ", netstat.nics |> Seq.map (fun nic -> nic.ip))
            change_trackable.RevertChanges()
            return change_trackable
        }

        [<Extension>]
        static member SetIdentity(odmSession:OdmSession, model:IChangeTrackable<IIdentificationModel>):Async<unit> = async{
            let session = odmSession.GetSession()
            let name_changed = model.origin.name <> model.current.name
            let location_changed = model.origin.location <> model.current.location
            let is_modified = name_changed || location_changed
            if not is_modified then return ()
            let! scopes = session.GetScopes()
            
            let scopes_to_set = seq{
                let vf_lst = Seq.toList (seq{
                    if name_changed then
                        let prefix = 
                            let use_onvif_scope = 
                                scopes 
                                    |> Seq.filter (fun x -> x.scopeDef = ScopeDefinition.configurable)
                                    |> Seq.exists (fun x -> x.scopeItem.StartsWith(ScopeHelper.onvifNameScope))
                            if use_onvif_scope then
                                ScopeHelper.onvifNameScope
                            else 
                                ScopeHelper.odmNameScope
                        let value = String.Concat(prefix, Uri.EscapeDataString(model.current.name))
                        let filter (x:string) = not (x.StartsWith(prefix))
                        yield (value, filter)
                    
                    if location_changed then
                        let prefix = 
                            let use_onvif_scope = 
                                scopes 
                                    |> Seq.filter (fun x -> x.scopeDef = ScopeDefinition.configurable)
                                    |> Seq.exists (fun x -> x.scopeItem.StartsWith(ScopeHelper.onvifLocationScope))
                            if use_onvif_scope then
                                ScopeHelper.onvifLocationScope
                            else 
                                ScopeHelper.odmLocationScope
                        let value = String.Concat(prefix, Uri.EscapeDataString(model.current.location))
                        let filter (x:string) = not (x.StartsWith(prefix))
                        yield (value, filter)
                })
                
                let filter (x:string) = vf_lst |> Seq.forall (fun (v,f) -> (f x))
                
                yield! scopes
                    |> Seq.filter (fun x -> x.scopeDef = ScopeDefinition.configurable)
                    |> Seq.map (fun x -> x.scopeItem)
                    |> Seq.filter filter
                
                yield! vf_lst |> Seq.map (fun (v,f) -> v)
            }
            do! session.SetScopes(scopes_to_set.ToArray())
        }

//        [<Extension>]
//        static member GetCertificateSettings(odmSession: OdmSession, factory:Func<IChangeTrackable<ICertificateManagementModel>>):Async<IChangeTrackable<ICertificateManagementModel>> = async{
//            let session = odmSession.GetSession()
//            let change_trackable = factory.Invoke()
//            let model = change_trackable.origin
//            let! certStatuses = session.GetCertificatesStatus()
//            let! certificates = session.GetCertificates()
//            model.serverCertificates <-
//                let list = new LinkedList<string>()
//                if certificates |> NotNull then
//                    for x in certificates do 
//                        list.AddLast(x.CertificateID) |> ignore
//                list
//            model.activeCertificateId <-
//                if certStatuses |> NotNull then
//                    match certStatuses |> Seq.tryFind (fun x->x.Status = true) with
//                    | Some c -> c.CertificateID
//                    | None -> null
//                else
//                    null
//            let! clientAuth = session.GetClientCertificateMode()
//            model.clientAuthentication <- clientAuth
//            change_trackable.RevertChanges()
//            return change_trackable
//        }
//        
//        [<Extension>]
//        static member SetCertificateSettings(odmSession: OdmSession, model:IChangeTrackable<ICertificateManagementModel>):Async<unit> = async{
//            let session = odmSession.GetSession()
//            if not (model.isModified) then return ()
//            let active_changed = 
//                model.origin.activeCertificateId <> model.current.activeCertificateId
//            
//            // client authentication mode has been changed
//            if model.origin.clientAuthentication <> model.current.clientAuthentication then 
//                do! session.SetClientCertificateMode(model.current.clientAuthentication)
//            
//            // active server certificate has been changed
//            if model.origin.activeCertificateId <> model.current.activeCertificateId then 
//                do! session.SetCertificatesStatus(seq{
//                    if model.origin.serverCertificates |> NotNull then
//                        for x in model.origin.serverCertificates -> 
//                            new CertificateStatus(CertificateID = x, Status = (x = model.current.activeCertificateId))
//                } |> Seq.toArray)
//            
//            // process removed certificates
//            let certificatesToRemove = Seq.toArray(seq{
//                if model.current.serverCertificates |> NotNull then 
//                    if model.origin.serverCertificates |> IsNull then
//                        yield! model.current.serverCertificates
//                    else
//                        for c in model.origin.serverCertificates do
//                            if model.current.serverCertificates |> Seq.forall (fun x -> x<>c) then
//                                yield c
//            })
//            if certificatesToRemove.Length>0 then
//                do! session.DeleteCertificates(certificatesToRemove)
//
//        }





    end




