namespace odm.infra
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Linq
    open System.Reactive.Disposables

    open utils
    open utils.fsharp

    type IChangeTrackable = interface
        abstract RevertChanges: unit->unit
        abstract AcceptChanges: unit->unit
        abstract isModified: bool with get
    end
    
    type IChangeTrackable<'T> = interface
        inherit IChangeTrackable
        abstract current: 'T with get, set
        abstract origin: 'T with get, set
    end

    type SimpleChangeTrackable<'T when 'T:equality>= struct
        //inherit IChangeTrackable
        val mutable current: 'T
        val mutable origin: 'T
        member this.AcceptChanges() = this.origin <- this.current
        member this.RevertChanges() = this.current <- this.origin
        member this.isModified = this.current<>this.origin
        interface IChangeTrackable<'T> with
            member t.current with get() = t.current and set(v) = t.current<-v
            member t.origin with get() = t.origin and set(v) = t.origin<-v
            member t.AcceptChanges() = t.AcceptChanges()
            member t.RevertChanges() = t.RevertChanges()
            member t.isModified = t.isModified
        end
    end

    type ChangeTrackableList<'T>= struct
        //inherit IChangeTrackable
        val mutable current: LinkedList<'T>
        val mutable origin: LinkedList<'T>
        member this.AcceptChanges() = 
            this.origin <- 
                if this.current |> IsNull then 
                    null 
                else 
                    new LinkedList<'T>(this.current)

        member this.RevertChanges() = 
            this.current <- 
                if this.origin |> IsNull then 
                    null 
                else 
                    new LinkedList<'T>(this.origin)

        member this.isModified = 
            if this.current = this.origin then
                false
            else if this.current |> IsNull then
                true
            else if this.origin |> IsNull then
                true
            else if this.origin.Count <> this.current.Count then
                true
            else
                let temp = new LinkedList<'T>(this.current)
                this.origin |> Seq.takeWhile(fun x->temp.Remove(x)) |> Seq.iter (fun x->())
                temp.Count <> 0
                
        interface IChangeTrackable<LinkedList<'T>> with
            member t.current with get() = t.current and set(v) = t.current<-v
            member t.origin with get() = t.origin and set(v) = t.origin<-v
            member t.AcceptChanges() = t.AcceptChanges()
            member t.RevertChanges() = t.RevertChanges()
            member t.isModified = t.isModified
        end
    end

    type ChangeTrackingSet() = class
        let tramp = new Trampoline()
        let props= new LinkedList<IChangeTrackable>();

        let add_prop (prop:IChangeTrackable) = 
            if prop.isModified then
                let disp = new SingleAssignmentDisposable()
                tramp.Drop(fun()->
                    let node = new LinkedListNode<IChangeTrackable>(prop)
                    props.AddFirst(node)

                    disp.Disposable <- Disposable.Create(fun()->
                        tramp.Drop(fun()->
                            props.Remove(node)
                        )
                    )
                )
                disp :> IDisposable
            else
                Disposable.Empty
        

        member this.RevertChanges() = tramp.Drop(fun()->
            for x in props do x.RevertChanges()
        )

        member this.AcceptChanges() = tramp.Drop(fun()->
            for x in props do x.AcceptChanges()
        )

        member this.isModified = 
            props.Count <> 0

        member this.CreateProperty<'T when 'T:equality>() = 
            let m_origin = ref Unchecked.defaultof<'T>
            let m_current = ref Unchecked.defaultof<'T>
            let gate = new obj()
            let disp = new SerialDisposable()
            
            let prop = {
                new IChangeTrackable<'T> with
                    member p.current
                        with get() = !m_current
                        and set v = lock gate (fun()->
                            m_current := v
                            disp.Disposable <- add_prop(p)
                        )

                    member p.origin 
                        with get() = !m_origin
                        and set v = lock gate (fun()->
                            m_origin := v
                            disp.Disposable <- add_prop(p)
                        )

                    member p.isModified
                        with get() = !m_current <> !m_origin
                    
                    member p.RevertChanges() = 
                        p.current <- p.origin

                    member p.AcceptChanges() = 
                        p.origin <- p.current
                end
            }
            prop
    end

    type ChangeTrackableCollection<'T>() = class
        //inherit IChangeTrackable
        let m_current = new ObservableCollection<'T>()
        let m_origin = new ObservableCollection<'T>()
        member this.AcceptChanges() = 
            m_origin.Clear()
            for x in m_current do
                m_origin.Add(x)

        member this.RevertChanges() = 
            m_current.Clear()
            for x in m_origin do
                m_current.Add(x)

        member this.isModified = 
            if m_current.Count <> m_origin.Count then
                true
            else
                let temp = new List<'T>(m_current)
                m_origin |> Seq.takeWhile(fun x->temp.Remove(x)) |> Seq.iter (fun x->())
                temp.Count <> 0
        
        member this.current 
            with get(): ObservableCollection<'T> = m_current
            and set(v: ObservableCollection<'T>) =
                m_current.Clear()
                for x in v do
                    m_current.Add(x)

        member this.origin 
            with get(): ObservableCollection<'T> = m_origin 
            and set(v: ObservableCollection<'T>) = 
                m_origin.Clear()
                for x in v do
                    m_origin.Add(x)

        interface IChangeTrackable<ObservableCollection<'T>> with
            member t.current with get() = t.current and set(v) = t.current <- v
            member t.origin with get() = t.origin and set(v) = t.origin <- v
            member t.AcceptChanges() = t.AcceptChanges()
            member t.RevertChanges() = t.RevertChanges()
            member t.isModified = t.isModified
        end
    end
