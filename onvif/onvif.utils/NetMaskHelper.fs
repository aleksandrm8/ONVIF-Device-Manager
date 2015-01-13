module onvif.utils.NetMaskHelper
    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Net

    type private MB = {
        mask:byte
        bits:int
        next: list<MB>
    }

    let CidrToIpMask(cidrMask:Int32):IPAddress = 
        if cidrMask > 32 then failwith "invalid argument - cidrMask"
        if cidrMask < 0 then failwith "invalid argument - cidrMask"
        let rec GetBytes(cidrMask, cnt) = 
            if cnt = 0 then
                []
            else if cidrMask > 8 then
                255uy::GetBytes(cidrMask-8, cnt-1)
            else if cidrMask>0 then
                (255uy <<< 8-cidrMask)::GetBytes(cidrMask-8, cnt-1)
            else
                0uy::GetBytes(0, cnt-1)

        new IPAddress(GetBytes(cidrMask,4) |> List.toArray)


    let IpToCidrMask(ipMask:IPAddress):Int32 = 
        let mutable cidrMask = 0
        let bytes = ipMask.GetAddressBytes()
        
        let rec zero_m = [{mask=0uy; bits=0; next=zero_m}]
        let rec any_m = [
            {mask=255uy; bits=8; next=any_m}; 
            {mask=254uy; bits=7; next=zero_m};
            {mask=252uy; bits=6; next=zero_m};
            {mask=248uy; bits=5; next=zero_m};
            {mask=240uy; bits=4; next=zero_m};
            {mask=224uy; bits=3; next=zero_m};
            {mask=192uy; bits=2; next=zero_m};
            {mask=128uy; bits=1; next=zero_m};
            {mask=0uy; bits=0; next=zero_m}
        ]
        
        let rec find_m v lst = 
            match lst with
            |h::t -> 
                if h.mask = v then
                    Some h
                elif h.mask<v then
                    None
                else
                    find_m v t
            |[] -> None
        
        let cidrMask, s = bytes |> ((0, any_m) |> Array.fold (fun (acc, pos) x -> 
            match find_m x pos with
            |Some v -> (acc+v.bits, v.next)
            |None -> failwith "invalid ip mask"
        ))
        cidrMask