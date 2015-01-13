namespace odm.ui.activities
open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Linq
open System.Threading
open System.Windows
open System.Windows.Threading

open Microsoft.Practices.Unity

//open odm.models
open odm.onvif
open odm.core
open odm.infra
open odm.ui

open onvif.services
open onvif.utils
open utils
open utils.fsharp
open ProfileDescription

type SelectReceiverActivity(ctx:IUnityContainer, selectedRecieverToken:string) = class
    let session = ctx.Resolve<INvtSession>()
    let dev = session :> IDeviceAsync
    let recv = session :> IReceiverAsync
    member private this.Main(selection) = async{
        let! cont = async{
            try
                let! receivers = async{
                    use! progress = Progress.Show(ctx, "retrieving receivers...")
                    let! receivers = recv.GetReceivers()
                    return receivers |> SuppressNull [||]
                }
                return this.ShowForm(receivers, selection)
            with err ->
                dbg.Error(err)
                do! ErrorView.Show(ctx, err) |> Async.Ignore
                return this.Main(selection)
        }
        return! cont
    }
    member private this.ShowForm(receivers, selection) = async{
        let! cont = async{
            try
                let items = [|
                    for receiver in receivers do
                        let details = GetReceiverDetails(receiver) |> Seq.toArray
                        let flags = (seq{
                            yield ItemSelectorView.ItemFlags.CanBeModified
                            yield ItemSelectorView.ItemFlags.CanBeDeleted
                            if receiver.token <> selectedRecieverToken then
                                yield ItemSelectorView.ItemFlags.CanBeSelected
                        } |> Seq.fold (|||) (ItemSelectorView.ItemFlags.NoOperationsAvailable))
                        yield new ItemSelectorView.Item(receiver.ToString(), details, flags)
                |]
                let (selRecv, selItem) = (
                    items |> Seq.zip receivers |> Seq.tryFind (fun (r,i) -> r.token = selection) |> (fun o-> match o with |None->(null,null) |Some x->x)
                )
                let GetReceiverFromItem(item) = 
                    items |> Seq.zip receivers |> Seq.tryFind (fun (r,i) -> i = item) |> Option.bind (fun (r, i) -> Some r)
                //let itemsModel = new SelectItemModel<Receiver>(items, selection)
                let! res = 
                    let viewModel = 
                        ItemSelectorView.Model.Create(
                            items = items,
                            selection = selItem,
                            flags = (seq{
                                yield ItemSelectorView.Flags.CanCreate
                                yield ItemSelectorView.Flags.CanSelect
                                yield ItemSelectorView.Flags.CanDelete
                                yield ItemSelectorView.Flags.CanModify
                                yield ItemSelectorView.Flags.CanClose
                            } |> Seq.fold (|||) (ItemSelectorView.Flags.NoOperationsAvailable))
                        )
                    ItemSelectorView.Show(ctx, viewModel)
                return res.Handle(
                    create = (fun x -> async{
                        let! res = this.CreateReceiver()
                        match res with
                        |Some r ->
                            return! this.Main(r.token)
                        |None ->
                            return! this.ShowForm(receivers, selection)
                    }), 
                    delete = (fun i -> async{
                        match GetReceiverFromItem(i) with
                        |Some r ->
                            let! res = this.DeleteReceiver(r)
                            if res then
                                return! this.Main(selection)
                            else
                                return! this.ShowForm(receivers, selection)
                        |None -> 
                            return! this.Main(selection)
                    }),
                    modify = (fun i -> async{
                        match GetReceiverFromItem(i) with
                        |Some r ->
                            let! res = this.ModifyReceiver(r)
                            if res then
                                return! this.Main(r.token)
                            else
                                return! this.ShowForm(receivers, r.token)
                        |None ->
                            return! this.ShowForm(receivers, selection)
                    }), 
                    select = (fun i -> async{
                        return GetReceiverFromItem(i)
                    }), 
                    close = (fun () -> async{return None})
                )
            with err ->
                dbg.Error(err)
                do! ErrorView.Show(ctx, err) |> Async.Ignore
                return async{return None}
        }
        return! cont
    }

    member private this.CreateReceiver():Async<Receiver option> = async{
        let model = new ReceiversModifyView.Model()
        let! r = ReceiversModifyView.Show(ctx, model)
        return! r.Handle(
            apply = (fun m -> async{
                try
                    use! progress = Progress.Show(ctx, "creating new receiver...")
                    let! res = recv.CreateReceiver(m.receiver.configuration)
                    return Some res
                with err -> 
                    dbg.Error(err)
                    do! ErrorView.Show(ctx, err) |> Async.Ignore
                    return None
            }),
            close = (fun m -> async{ return None} ),
            cancel = (fun m -> async{ return None})
        )
    }
    member private this.ModifyReceiver(receiverToConfigure) = async{
        try
            let m = new ReceiversModifyView.Model()
            m.receiver <- receiverToConfigure
            m.AcceptChanges()
            let! r = ReceiversModifyView.Show(ctx, m)
            return! r.Handle(
                apply = (fun m -> async{
                    try
                        use! progress = Progress.Show(ctx, "configuring receiver...")
                        do! recv.ConfigureReceiver(m.receiver.token, m.receiver.configuration)
                        return true
                    with err -> 
                        dbg.Error(err)
                        do! ErrorView.Show(ctx, err) |> Async.Ignore
                        return false
                }),
                close = (fun m -> async{ return false}),
                cancel = (fun m -> async{return false})
            )
        with err ->
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
            return false
    }
    member private this.DeleteReceiver(receiverToDelete) = async{
        try
            use! progress = Progress.Show(ctx, "deleting receiver...")
            do! recv.DeleteReceiver(receiverToDelete.token)
            return true
        with err ->
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
            return false
    }
    static member public Run(ctx:IUnityContainer, selectedReceiverToken:string) = 
        let act = new SelectReceiverActivity(ctx, selectedReceiverToken)
        act.Main(selectedReceiverToken)
end

