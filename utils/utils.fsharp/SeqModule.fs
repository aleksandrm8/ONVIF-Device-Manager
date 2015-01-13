module Seq
    open utils.fsharp

    ///<summary></summary>
    let inline repeat times sequence = seq{
        for i=1 to int(times) do
            yield! sequence
    }
    
    ///<summary></summary>
    let inline emptyIfNull src =
        if src |> NotNull then src else Seq.empty

    ///<summary></summary>
    let inline tryGetNth (n:int) (src:seq<'a>) = 
        if n<0 || IsNull(src) then
            None
        else
            src |> Seq.mapi(fun i v -> if i=n then Some v else None) |> Seq.tryPick (fun x->x)

    ///<summary></summary>
    let inline getNthOrDefault (n:int) (src:seq<'a>) = 
        match src |> tryGetNth n  with
        | Some v -> v
        | None -> Unchecked.defaultof<'a>

    let inline firstOrDefault (pred:'a->bool) (src:seq<'a>) = 
        match src |> Seq.tryFind pred with
        | Some v -> v
        | None -> Unchecked.defaultof<'a>

    ///<summary></summary>
    let inline zipAll (srcA:seq<'a>) (srcB:seq<'b>) = seq{
        let srcA = srcA |> SuppressNull Seq.empty
        let srcB = srcB |> SuppressNull Seq.empty
        use enumA = srcA.GetEnumerator()
        use enumB = srcB.GetEnumerator()
        let rec loop() = seq{
            let aNotAtEnd = enumA.MoveNext()
            let bNotAtEnd = enumB.MoveNext()
            if aNotAtEnd || bNotAtEnd then
                let a = if aNotAtEnd then Some enumA.Current else None
                let b = if bNotAtEnd then Some enumB.Current else None
                yield (a,b)
                yield! loop()
        }
        yield! loop()
    }

    ///<summary></summary>
    let inline index (src:seq<'a>) = 
        src |> Seq.mapi (fun i e -> i,e)
