namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.IO
    open System.Linq
    open System.Net
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    //open Org.BouncyCastle.X509
    open System.Security.Cryptography.X509Certificates
    open odm.ui

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.core
    open odm.infra
    open utils
    //open odm.models
    open utils.fsharp

    type CertificateManagementActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        let load() = async{
            let! certificates, statuses = Async.Parallel(
                dev.GetCertificates(),
                dev.GetCertificatesStatus()
            )
//            for cert in certificates do
//                let x509 = new X509Certificate2(cert.Certificate1.Data)
//                let subj = x509.Subject
//                let subjName = x509.SubjectName
//                //let certParser = new X509CertificateParser()
//                //let x509 = certParser.ReadCertificate(cert.Certificate1.Data)
//                x509.ToString() |> ignore

            let model = new CertificatesView.Model(
                certificates = Seq.toArray(seq{
                    for cert in certificates do
                        let cid = cert.certificateID
                        let enabled = 
                            (statuses |> Seq.tryPick (fun c -> 
                                if (c.certificateID = cid) then 
                                    Some(c.status)
                                else
                                    None
                            )) = Some(true)
                        yield CertificatesView.Certificate.Create(
                            data = cert.certificate.data,
                            cid = cid,
                            enabled = enabled
                        )
                })
            )
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
                    let! res = CertificatesView.Show(ctx, model)
                    return res.Handle(
                        upload = (fun model-> this.UploadCertificate(model)),
                        delete = (fun model cid -> this.Delete(model, cid)),
                        apply = (fun model-> this.Apply(model))
                    )
                with err -> 
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        member private this.UploadCertificate(model) = async{
            let! cont = async{
                try
                    let! filePath = OpenFileActivity.Run("Select certificate to upload", "Pem files (*.pem)|*.pem|All files (*.*)|*.*")
                    if filePath |> NotNull then
                        use! progress = Progress.Show(ctx, "reading certificate...")
                        let cert = new Certificate()
                        let finfo = new FileInfo(filePath)
                        use fstream = finfo.OpenRead() 
                        let! data = fstream.AsyncRead(int(fstream.Length))
                        cert.certificateID <- finfo.Name
                        cert.certificate <- new BinaryData()
                        cert.certificate.data <- data
                        let! res = CertificateUploadView.Show(ctx, new CertificateUploadView.Model(cert))
                        return! res.Handle(
                            upload = (fun ()-> async{
                                use! progress = Progress.Show(ctx, LocalDevice.instance.uploading)
                                do! dev.LoadCertificates([|cert|])
                                return this.Main()
                            }),
                            cancel = (fun ()->async{
                                return this.ShowForm(model)
                            })
                        )
                    else
                        return this.ShowForm(model)
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.Apply(model) = async{
            let! cont = async{
                try
                    if NotNull(model) && NotNull(model.certificates) && model.certificates.Any(fun c-> c.isModified) then
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        let statuses = seq{
                            for cert in model.certificates -> new CertificateStatus(
                                certificateID = cert.cid,
                                status = cert.enabled
                            )
                        }
                        do! dev.SetCertificatesStatus(statuses |> Seq.toArray)
                    return this.Main()
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.Delete(model, cid) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, "removing certificate...")
                    do! dev.DeleteCertificates([|cid|])
                    return this.Main()
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.Complete(res) = async{
            return res
        }

        static member Run(ctx:IUnityContainer) = 
            let act = new CertificateManagementActivity(ctx)
            act.Main()
    end
