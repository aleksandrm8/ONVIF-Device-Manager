module Array
    open utils.fsharp

    let inline emptyIfNull array =
        if array |> NotNull then array else [||]

    ///<summary></summary>
    let inline tryGetNth (n:int) (array:'a[]) = 
        if n>=0 && NotNull(array) && n < array.Length then
            Some array.[n]
        else
            None

    ///<summary></summary>
    let inline getNthOrDefault (index:int) (array:'a[]) = 
        match array |> tryGetNth index  with
        | Some v -> v
        | None -> Unchecked.defaultof<'a>

    let inline zipAll (arrayA:'a[]) (arrayB:'b[]) = 
        let arrayA = arrayA |> emptyIfNull
        let arrayB = arrayB |> emptyIfNull
        let len = max (arrayA.Length) (arrayB.Length)
        let arrayC = Array.zeroCreate(len)
        for i in {0..len} do
            arrayC.[i] <- (arrayA |> getNthOrDefault i, arrayB |> getNthOrDefault i)
        arrayC

    let inline index (src:seq<'a>) = 
        src |> Seq.mapi (fun i e -> i,e)
