namespace onvif.utils
    open System
    open System.Collections.Generic
    open System.Linq
    open System.Security.Cryptography
    open System.Text
    open global.utils
    
    type PasswordHelper = class
        
        static member GetPasswordEquivalence(userName:string, password:string):string = 
            let key = String.Concat(userName, password).ToUtf8()
            let data = "ONVIF password".ToUtf8()
            let hasher = new HMACSHA1(key)
            let hash = hasher.ComputeHash(data)
            EncodingEx.ToBase64(hash)

        static member GetPasswordEquivalence(userName:string, password:string, endpointReference: Guid):string = 
            let key = String.Concat(userName, password).ToUtf8()
            let data = Seq.toArray(seq{
                let ep_bytes = endpointReference.ToByteArray()
                yield! [|
                    ep_bytes.[3];
                    ep_bytes.[2];
                    ep_bytes.[1];
                    ep_bytes.[0];
                    ep_bytes.[5];
                    ep_bytes.[4];
                    ep_bytes.[7];
                    ep_bytes.[6];
                |]
                yield! ep_bytes.[8..15]
                yield! "ONVIF password".ToUtf8()
            })
            let hasher = new HMACSHA1(key)
            let hash = hasher.ComputeHash(data)
            EncodingEx.ToBase64(hash)

        static member GetPasswordEquivalence(userName:string, password:string, data:string):string = 
            let key = String.Concat(userName, password).ToUtf8()
            let data = Seq.toArray(seq{
                yield! data.ToUtf8()
                yield! "ONVIF password".ToUtf8()
            })
            let hasher = new HMACSHA1(key)
            let hash = hasher.ComputeHash(data)
            EncodingEx.ToBase64(hash)
    end
