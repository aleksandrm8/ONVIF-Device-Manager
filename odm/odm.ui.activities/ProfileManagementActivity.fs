namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Linq
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    //open System.Disposables
    
    open Microsoft.Practices.Unity

    //open odm.models
    open odm.onvif
    open odm.core
    open odm.infra

    open onvif.services
    open onvif.utils

    open utils
    open utils.fsharp
    open ProfileDescription
    open odm.ui

//    type ProfilesViewActResult =
//    |Create of string
//    |Delete of ProfileToken
//    |Close
    
//    type ProfilesViewAct(container:IUnityContainer, profiles:Profile[]) = class
//        static member Show(container:IUnityContainer, profiles:Profile[]) = 
//            let act = new ProfilesViewAct(container, profiles) :> IActivity<ProfilesViewActResult>
//            act.Run()
//        interface IActivity<ProfilesViewActResult> with
//            member this.Run() = async{
//                return ProfilesViewActResult.Close
//            }
//        end
//    end

    type ProfileManagementActivityResult =
        |Select of string
        |Refresh
        |Close

    type internal ProfileManagerModel(profiles, videoSources, audioSources, ptzNodes) = class
        member this.profiles:Profile[] = profiles
        member this.videoSources:VideoSource[] = videoSources
        member this.audioSources:AudioSource[] = audioSources
        member this.ptzNodes:PTZNode[] = ptzNodes

        //member this.items:list<Profile*ItemSelectorView.Item> = items
        //member this.selection:ItemSelectorView.Item = selection
//        member this.GetProfileFromItem(item:ItemSelectorView.Item) = 
//            let profItem = items |> List.tryFind(fun(p, i) -> i=item)
//            match profItem with
//            | Some (p,i)-> p
//            | None -> null
    end

    type ProfileManagementActivity(ctx:IUnityContainer, activeProfToken:string, vsToken: string) = class
        do if vsToken |> IsNull then raise (new ArgumentNullException("vsToken"))
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)

        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load(selctedProfToken:string) = async{
            let! caps = session.GetCapabilities()
            let! profiles = async{
                let! profs = session.GetProfiles()
                return profs 
                    |> Seq.filter(fun p-> 
                        let vsc = p.videoSourceConfiguration 
                        IsNull(vsc) || vsc.sourceToken = vsToken
                    )
                    |> Seq.toArray
            }
            let! videoSources = session.GetVideoSources()
            let! audioSources = session.GetAudioSources()
            let! ptzNodes = async{
                let! isPtzSupported = facade.IsPtzSupported()
                if isPtzSupported then
                    try
                        let ptz = session :> IPtzAsync
                        let! nodes = ptz.GetNodes()
                        return nodes |> SuppressNull [||]
                    with err->
                        dbg.Error(err)
                        return [||]
                else
                    return [||]
            }
            
            return new ProfileManagerModel(
                profiles, 
                videoSources, 
                audioSources, 
                ptzNodes 
            )
        }
        member private this.Main(selctedProfToken:string) = async{
            let! cont = async{
                try
                    let! model = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! load(selctedProfToken)
                    }
                    let selectedProfile = 
                        if not(String.IsNullOrEmpty(selctedProfToken)) then
                            model.profiles |> Seq.firstOrDefault(fun p -> p.token = selctedProfToken)
                        else
                            null
                    return this.ShowForm(model, selectedProfile)
                with err -> 
                    do! show_error(err)
                    return this.Main(selctedProfToken)
            }
            return! cont
        }

        member private this.ShowForm(model, selectedProfile) = async{
            let! cont = async{
                try
                    let profItems = Seq.toList(seq{
                        for prof in model.profiles do
                            let details = GetProfileDetails(prof, model.videoSources, model.audioSources, model.ptzNodes)
                            let flags = 
                                if prof.videoSourceConfiguration |> NotNull then
                                    ItemSelectorView.ItemFlags.AllOperationsAvailable
                                else
                                    ItemSelectorView.ItemFlags.CanBeDeleted ||| ItemSelectorView.ItemFlags.CanBeModified
        
                            let item = new ItemSelectorView.Item(prof.ToString(), details |> Seq.toArray, flags)
                            yield (prof, item)
                    })
                    let GetProfileFromItem(item) = 
                        let profItem = profItems |> List.tryFind(fun(p, i) -> i=item)
                        match profItem with
                        | Some (p,i)-> p
                        | None -> null
                    let items = profItems |> List.map(fun (p,i)->i)
                    let selection = 
                        if selectedProfile |> NotNull then
                            match profItems |> List.tryFind (fun (x,y) -> x.token = selectedProfile.token) with
                            | Some (cfg, item) -> 
                                item
                            | None -> 
                                null
                        else
                            null
                    let itemSelectorModel = new ItemSelectorView.Model(
                        items = (
                            items |> List.toArray
                        ),
                        flags = (
                            ItemSelectorView.Flags.CanCreate ||| 
                            ItemSelectorView.Flags.CanDelete |||
                            ItemSelectorView.Flags.CanModify |||
                            ItemSelectorView.Flags.CanSelect
                        )
                    )
                    itemSelectorModel.selection <- selection
                    itemSelectorModel.AcceptChanges()

                    let rec show()  = async{
                        let! res = ItemSelectorView.Show(ctx, itemSelectorModel)

                        return! res.Handle(
                            create = (fun ()->async{
                                //create new profile
                                let! res = CreateProfileActivity.Run(ctx, vsToken)
                                match res with
                                |CreateProfileActivityResult.Created profile -> 
                                    return this.Main(profile.token)
                                |CreateProfileActivityResult.Aborted ->
                                    return! show()
                            }),
                            delete = (fun item -> async{
                                let profileToDelete = GetProfileFromItem(item)
                                return this.DeleteProfile(model, profileToDelete)
                            }),
                            modify = (fun item -> async{
                                //configure profile
                                let prof = GetProfileFromItem(item)
                                if prof |> NotNull then
                                    let! was_aborted = ConfigureProfileActivity.Run(ctx, prof)
                                    if was_aborted then
                                        return! show()
                                    elif prof.token = activeProfToken then
                                        return async{
                                            do! InfoView.Show(ctx, LocalProfile.instance.modifiedSuccess) |> Async.Ignore
                                            return! this.Complete(ProfileManagementActivityResult.Refresh)
                                        }
                                    else
                                        return this.Main(prof.token)
                                else
                                    do! show_error(new Exception(LocalProfile.instance.modifiedFail))
                                    return this.ShowForm(model, selectedProfile)
                            }),
                            select = (fun item -> async{
                                //activate selected profile
                                let prof = GetProfileFromItem(item)
                                if prof |> NotNull then
                                    return this.Complete(ProfileManagementActivityResult.Select(prof.token))
                                else
                                    do! show_error(new Exception(LocalProfile.instance.selectFail))
                                    return! show()
                            }),
                            close = (fun item -> async{
                                return this.Complete(ProfileManagementActivityResult.Close)
                            })
                        )
                    }
                    return! show()

                with err ->
                   do! show_error(err)
                   return this.ShowForm(model, selectedProfile)
            }
            return! cont
        }

        member private this.DeleteProfile(model, profileToDelete) = async{
            let! cont = async{
                try
                    if profileToDelete |> IsNull then
                        do! show_error(new Exception(LocalProfile.instance.deleteFail))
                        return this.ShowForm(model, profileToDelete)
                    else
                        do! async{
                            use! progress = Progress.Show(ctx, LocalProfile.instance.deleting)
                            do! session.DeleteProfile(profileToDelete.token)
                        }
                        if profileToDelete.token = activeProfToken then
                            async{
                                use ctx = new ModalDialogContext(LocalDevice.instance.information) :> IUnityContainer
                                do! InfoView.Show(ctx, LocalProfile.instance.deletingSuccess) |> Async.Ignore
                            } |> Async.StartImmediate
                            let newActiveProfileToken = 
                                match model.profiles |> Seq.tryFind (fun p->p.token<>activeProfToken && p.videoSourceConfiguration |> NotNull) with
                                | Some p -> p.token
                                | None -> null
                            //return this.Complete(ProfileManagerActivityResult.Refresh)
                            return this.Complete(ProfileManagementActivityResult.Select(newActiveProfileToken))
                        else
                            return this.Main(activeProfToken)
                with err -> 
                    do! show_error(err)
                    return this.Main(activeProfToken)
                }
            return! cont
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx, activeProfToken, vsToken) = 
            let act = new ProfileManagementActivity(ctx, activeProfToken, vsToken)
            act.Main(activeProfToken)
    end
