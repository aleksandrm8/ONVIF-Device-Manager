namespace onvif.utils 
    open System
    open System.Collections.Generic
    open System.Linq
    open System.Threading
    open System.ComponentModel
    open System.Net
    open System.Net.Sockets
    open System.Net.Mime
    open System.IO
    open System.IO.Packaging
    open System.Text
    open System.ServiceModel
    open System.Xml
    open System.Xml.Linq
    open System.Xml.Schema
    open System.Reactive.Disposables
    open System.Reactive.Linq

    open onvif.services
    open onvif10_events
    open wsn_bw_2
    open onvif

    open odm.core
    open odm.onvif
    open global.utils
    open utils.fsharp
    type NetworkInterface = onvif.services.NetworkInterface
    type IPAddress = onvif.services.IPAddress
    
    type NicInfo(mac, ip, subnet) = class
        member this.mac: string = mac
        member this.ip: System.Net.IPAddress = ip
        ///<summary>subnet mask in CIDR notation</summary>
        member this.subnet: int = subnet
    end

    type NetStatus(dns, nics) = class
        member this.dns: System.Net.IPAddress = dns
        member this.nics: NicInfo[] = nics
    end

    type SupportedFirmwareUpgradeModes(mtom:bool, http:bool) = class
        member this.mtom = mtom
        member this.http = http
    end

    type SupportedLogRetrievingModes =
        | NotSupported = 0
        | Http = 1
        | Mtom = 2

    type ChannelDescription(vs:VideoSource, profiles:Profile[]) = class
        member this.videoSource = vs
        member this.profiles = profiles
    end

    type IChannel = interface
        abstract profileToken: string
        //abstract encoderResolution: VideoResolution
        //abstract mediaUri: MediaUri
        abstract name: String
    end

    type OnvifEvent((*arrivalTime: System.DateTime,*) topic: TopicExpressionType, message: Message, subscriptionReference: EndpointReferenceType1, producerReference: EndpointReferenceType1) = class
        //member this.arrivalTime = arrivalTime
        member this.topic = topic
        member this.message = message
        member this.subscriptionReference = subscriptionReference
        member this.producerReference = producerReference
    end
    //FIX: AddressFilterMode is set to Any to allow processing notifications
    //without "to" header, see ONVIFDM-451(ODM cannot show events from some 
    //Bosch cameras) for more info
    [<ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Single, 
        InstanceContextMode = InstanceContextMode.Single, 
        AddressFilterMode = AddressFilterMode.Any
    )>]
    type NotificationConsumerService private(f) = class
        interface NotificationConsumer with
            member this.Notify(request: wsn_bw_2.Notify): unit = 
                if request |> NotNull && request.Notify1 |> NotNull && request.Notify1.NotificationMessage |> NotNull then
                    f(request.Notify1.NotificationMessage)
    //            if NotNull(request) && NotNull(request.Notify) && NotNull(request.Notify.NotificationMessage) then
    //                for m in request.Notify.NotificationMessage do
    //                    let topic = String.Concat(m.Topic.Any |> Seq.map (fun x->x.Value))
    //                    printfn "message arrived - %A" topic
    //            ()
            member this.BeginNotify(request:wsn_bw_2.Notify, callback:AsyncCallback, asyncState:obj):IAsyncResult = 
                failwith "not implemented"
            member this.EndNotify(result:IAsyncResult):unit =
                failwith "not implemented"
        end
        static member Create(f) =
            new NotificationConsumerService(f)
    end

    type IPtzControler = interface
        inherit IDisposable
        abstract MoveUp: unit -> unit
        abstract MoveDown: unit -> unit
        abstract MoveLeft: unit -> unit
        abstract MoveRight: unit -> unit
        abstract Stop: unit -> unit
    end

    type OdmSession(session:INvtSession) = class
        do if session |> IsNull then raise <| new ArgumentException("session")

        let caсhedSchemasLock = new obj()
        let caсhedSchemasByLocation = new Dictionary<string, Async<XmlSchema>>(StringComparer.OrdinalIgnoreCase)

        //TODO: extend Async
        let OnErrorContinueWith cont comp = async{
            let! res = comp |> Async.Catch
            match res with
            |Choice1Of2 v -> return v
            |Choice2Of2 e -> return! cont(e)
        }

        static let GetSubscriptionId =
            let id = ref 0L
            fun()->Interlocked.Increment(id)

        //TODO: move to separate class
        static member NetHostToStr(netHost:NetworkHost) = 
            if netHost |> IsNull then
                null
            else
                match netHost.``type`` with
                | NetworkHostType.iPv4 -> netHost.iPv4Address
                | NetworkHostType.iPv6 -> netHost.iPv6Address
                | NetworkHostType.dns -> netHost.dnSname
                | _ -> null
        
        //TODO: move to separate class
        static member NetHostFromStr(netHost:string) = 
            if netHost |> IsNull then
                null
            else
                let netHost = netHost.Trim()
                let isIp, ip = System.Net.IPAddress.TryParse(netHost)
                if isIp then
                    match ip.AddressFamily with
                    | System.Net.Sockets.AddressFamily.InterNetwork ->
                        new NetworkHost(
                            ``type`` = NetworkHostType.iPv4,
                            iPv4Address = netHost
                        )
                    | System.Net.Sockets.AddressFamily.InterNetworkV6 ->
                        new NetworkHost(
                            ``type`` = NetworkHostType.iPv6,
                            iPv4Address = netHost
                        )
                    | _ ->
                        new NetworkHost(
                            ``type`` = NetworkHostType.dns,
                            dnSname = netHost
                        )
                else
                    new NetworkHost(
                        ``type`` = NetworkHostType.dns,
                        dnSname = netHost
                    )

        member this.GetSession() = session

        member this.GetNetworkStatus() = async{
            let! nics, dnsInfo = Async.Parallel(
                session.GetNetworkInterfaces(),
                session.GetDNS()
            )
            
            let dns = 
                if dnsInfo |> NotNull then
                    if dnsInfo.fromDHCP && dnsInfo.dnsFromDHCP |> NotNull && dnsInfo.dnsFromDHCP.Count() > 0 then
                        System.Net.IPAddress.Parse(dnsInfo.dnsFromDHCP.[0].iPv4Address)
                    elif not dnsInfo.fromDHCP && dnsInfo.dnsManual |> NotNull && dnsInfo.dnsManual.Count() > 0 &&  not (String.IsNullOrWhiteSpace(dnsInfo.dnsManual.[0].iPv4Address)) then 
                        System.Net.IPAddress.Parse(dnsInfo.dnsManual.[0].iPv4Address)
                    else
                        new System.Net.IPAddress(0L)
                else
                    new System.Net.IPAddress(0L)
            
            let nicInfs = 
                nics |> Seq.filter (fun x -> x.enabled) |> Seq.map (fun nic->
                    let nic_cfg = nic.iPv4.config
                    let mac = 
                        if nic.info |> NotNull then
                            nic.info.hwAddress.Replace(':', '-').ToUpper()
                        else
                            null
                    let ip = System.Net.IPAddress(0L)
                    let subnet = 0
                    //let dhcp = nic.IPv4.Config.DHCP
                    if nic_cfg.dhcp && nic_cfg.fromDHCP |> NotNull then
                        let ip = System.Net.IPAddress.Parse(nic_cfg.fromDHCP.address)
                        let subnet = nic_cfg.fromDHCP.prefixLength
                        NicInfo(mac, ip, subnet)
                    elif not nic_cfg.dhcp && NotNull(nic_cfg.manual) && nic_cfg.manual.Count()>0 then
                        let ip = System.Net.IPAddress.Parse(nic_cfg.manual.[0].address)
                        let subnet = nic_cfg.manual.[0].prefixLength
                        NicInfo(mac, ip, subnet)
                    else
                        NicInfo(mac, ip, subnet)
                ) |> Seq.toArray
            
            return NetStatus(dns,nicInfs)
        }


        member this.UploadStream(uri:Uri, stream:Stream, contentType:string, credentials:NetworkCredential):Async<string> = async{
            if not(uri.IsAbsoluteUri) then
                raise <| new ArgumentException(String.Format("uri({0}) is not valid", uri.ToString()))
            if uri.Scheme<>Uri.UriSchemeHttp then
                raise <| new NotSupportedException(String.Format("specified protocol ({0}) not suppoted", uri.Scheme))
            

            let request = HttpWebRequest.Create(uri) :?> HttpWebRequest
            request.Proxy <- null
            if credentials |> NotNull then
//                let SetBasicAuthHeader(request:HttpWebRequest, userName:String, userPassword:String) =
//                    let authStr = sprintf "%s:%s" userName userPassword
//                    let authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authStr))
//                    request.Headers.["Authorization"] <- "Basic " + authInfo
//                SetBasicAuthHeader(request, credentials.UserName, credentials.Password)
                request.Credentials <- credentials

            request.Method <- WebRequestMethods.Http.Post
            request.KeepAlive <- false
            //request.Method <- WebRequestMethods.File.UploadFile
            //request.Method <- WebRequestMethods.Ftp.UploadFile
            request.MaximumResponseHeadersLength <- 10 * 1024
            //request.UserAgent <- "Mozilla/4.0 (compatible)"
            request.ContentType <- contentType
            request.ContentLength <- stream.Length
            request.Timeout <- 60*60*1000//60 min.
            request.ReadWriteTimeout <- 3000//3 sec.
            request.ProtocolVersion <- HttpVersion.Version11
            //request.ProtocolVersion <- HttpVersion.Version10
            let! requestStream = Async.FromBeginEnd(request.BeginGetRequestStream, request.EndGetRequestStream)
            try
                let bufferSize = 0x1000 //4KB
                let buffer = Array.create bufferSize 0uy
                let rec write_data() = async{
                    let! readed = stream.AsyncRead(buffer, 0, bufferSize)
                    if readed <> 0 then 
                        do! requestStream.AsyncWrite(buffer,0,readed)
                        return! write_data()
                }
                do! write_data()
                requestStream.Flush()
            finally
                requestStream.Close()

            use! response = request.AsyncGetResponse() |> Async.Map (fun x-> x:?>HttpWebResponse)
            if response.StatusCode <> HttpStatusCode.OK then
                failwith "upload failed"
            use responseStream = response.GetResponseStream()
            
            use! cancelation = Async.OnCancel(fun ()->
                request.Abort()
            )

            if not(response.ContentType.Contains(@"text/plain")) then
                return null
            else
                let maxResponseLength = 0x1000 //4KB
                let! txt = responseStream.AsyncRead(maxResponseLength)
                            
                if txt.Length = 0 then
                    return null
                else            
                    let enc =
                        if String.IsNullOrWhiteSpace(response.CharacterSet) then
                            Encoding.UTF8
                        else
                            Encoding.GetEncoding(response.CharacterSet)
                    
                    
                    return enc.GetString(txt)
        }
        member this.FtpDownload(uri:Uri):Async<Stream> = async{
            let request:ref<FtpWebRequest> = ref null
            let response:ref<FtpWebResponse> = ref null
            let stream:ref<Stream> = ref null
            let dispose() = 
                if !request |> NotNull then
                    try 
                        (!request).Abort()
                    with err ->
                        dbg.Error(err)
                    request := null
                if !response |> NotNull then
                    try
                        (!response).Close()
                    with err->
                        dbg.Error(err)
                    response := null
                if !stream |> NotNull then
                    try
                        (!stream).Close()
                    with err->
                        dbg.Error(err)
                    stream := null
            let ensure_not_disposed() = 
                if !stream |> IsNull then
                    failwith "stream was disposed"
            try
                if uri.Scheme<>Uri.UriSchemeFtp then
                    raise <| new NotSupportedException(String.Format("specified protocol ({0}) not suppoted", uri.Scheme))
                request := FtpWebRequest.Create(uri) :?> FtpWebRequest
                (!request).Proxy <- null
                if NotNull(session.credentials) && String.IsNullOrEmpty(uri.UserInfo) then
                    (!request).Credentials <- session.credentials
                (!request).Method <- WebRequestMethods.Ftp.DownloadFile
                //request.KeepAlive <- false
                //request.Timeout <- 60*60*1000//60min
                //request.ReadWriteTimeout <- 60*60*1000//60min
                do! async{
                    let! _response = (!request).AsyncGetResponse() |> Async.Map (fun x-> x:?>FtpWebResponse)
                    response := _response
//                    if (!response).StatusCode <> FtpStatusCode.FileActionOK then
//                        failwith "download failed"
                    let _stream = _response.GetResponseStream()
                    stream := _stream
                }
                return {
                    new Stream() with
                        override s.CanRead = 
                            ensure_not_disposed()
                            (!stream).CanRead
                        
                        override s.CanSeek = 
                            ensure_not_disposed()
                            (!stream).CanSeek
                        
                        override s.CanWrite = 
                            ensure_not_disposed()
                            (!stream).CanWrite
                        
                        override s.Flush() = 
                            ensure_not_disposed()
                            (!stream).Flush()
                        
                        override s.Length = 
                            ensure_not_disposed()
                            (!stream).Length
                        
                        override s.Position 
                            with get() = 
                                ensure_not_disposed()
                                (!stream).Position 
                            and set(value) = 
                                ensure_not_disposed()
                                (!stream).Position <- value
                        
                        override s.Read(buffer, offset, count) = 
                            ensure_not_disposed()
                            (!stream).Read(buffer, offset, count)
                        
                        override s.Write(buffer, offset, count) = 
                            ensure_not_disposed()
                            (!stream).Write(buffer, offset, count)
                        
                        override s.BeginRead(buffer, offset, count, callback, state) = 
                            ensure_not_disposed()
                            (!stream).BeginRead(buffer, offset, count, callback, state)
                        
                        override s.EndRead(asyncResult) = 
                            ensure_not_disposed()
                            (!stream).EndRead(asyncResult)
                        
                        override s.BeginWrite(buffer, offset, count, callback, state) = 
                            ensure_not_disposed()
                            (!stream).BeginWrite(buffer, offset, count, callback, state)
                        
                        override s.EndWrite(asyncResult) = 
                            ensure_not_disposed()
                            (!stream).EndWrite(asyncResult)
                        
                        override s.WriteByte(value) = 
                            ensure_not_disposed()
                            (!stream).WriteByte(value)
                        
                        override s.ReadByte() = 
                            ensure_not_disposed()
                            (!stream).ReadByte()
                        
                        override s.Seek(offset, origin) = 
                            ensure_not_disposed()
                            (!stream).Seek(offset, origin)
                        
                        override s.SetLength(value) = 
                            ensure_not_disposed()
                            (!stream).SetLength(value)
                        
                        override s.CanTimeout = 
                            ensure_not_disposed()
                            (!stream).CanTimeout
                        
                        override s.ReadTimeout 
                            with get() = 
                                ensure_not_disposed()
                                (!stream).ReadTimeout
                            and set(value) = 
                                ensure_not_disposed()
                                (!stream).ReadTimeout <- value
                        
                        override s.WriteTimeout 
                            with get() = 
                                ensure_not_disposed()
                                (!stream).WriteTimeout
                            and set(value) = 
                                ensure_not_disposed()
                                (!stream).WriteTimeout <- value
                        
                        override s.Dispose(disposing) = 
                            if disposing then dispose()
                }
            with err ->
                dbg.Error(err)
                dispose()
                return raise(err) //reraise()
        }
        member this.DownloadStream(uri:Uri, accept:String):Async<Stream> = async{
            if uri.Scheme<>Uri.UriSchemeHttp && uri.Scheme<>Uri.UriSchemeHttps then
                raise <| new NotSupportedException(String.Format("specified protocol ({0}) not suppoted", uri.Scheme))
            
            let request = HttpWebRequest.Create(uri) :?> HttpWebRequest
            request.Proxy <- null
            //request.ContentType <- sprintf "%s; charset=utf-8" (MediaTypeNames.Text.Html)
            if session.credentials |> NotNull then
//                let SetBasicAuthHeader(request:HttpWebRequest, userName:String, userPassword:String) =
//                    // according to RFC 2617 userid can not contain ':'
//                    //if NotNull(userName) && userName.Contains(':') then
//                    //    failwith "invalid userid"
//                    let authStr = sprintf "%s:%s" userName userPassword
//                    let authInfo = Convert.ToBase64String(authStr.ToUtf8())
//                    request.Headers.["Authorization"] <- "Basic " + authInfo
//                SetBasicAuthHeader(request, credentials.UserName, credentials.Password)
                request.PreAuthenticate <- false
                request.UseDefaultCredentials <- false
                //request.AuthenticationLevel <- AuthenticationLevel.
                
//                let cred_cache = new CredentialCache()
//                cred_cache.Add(uri, "Basic", session.credentials)
//                cred_cache.Add(
//                    uri, "Digest", 
//                    new NetworkCredential(
//                        userName = session.credentials.UserName,
//                        password = session.credentials.Password
//                    )
//                )
//                request.Credentials <- cred_cache
                request.Credentials <- session.credentials
                
                
            
            //request.TransferEncoding <-
            request.Method <- WebRequestMethods.Http.Get
            //request.Method <- WebRequestMethods.File.UploadFile
            //request.Method <- WebRequestMethods.Ftp.UploadFile
            request.MaximumResponseHeadersLength <- 10 * 1024
            request.ContentLength <- 0L
            request.KeepAlive <- false
            if accept |> NotNull then
                request.Accept <- accept
            //request.UserAgent <- "Mozilla/4.0 (compatible)"
            //request.ContentType <- MediaTypeNames.Application.Octet //"application/octet-stream"
            //request.Timeout <- 60*60*1000//60min
            //request.ReadWriteTimeout <- 60*60*1000//60min
            request.ProtocolVersion <- HttpVersion.Version11

            let! response = request.AsyncGetResponse() |> Async.Map (fun x-> x:?>HttpWebResponse)
            if response.StatusCode <> HttpStatusCode.OK then
                response.Close() 
                failwith "download failed"
            //BUGFIX: HttpWebResponse.Dispose() may hang, we need to call request.Abort() before, to avoid this incorrect behaviour.
            use disp = Disposable.Create(fun()->
                request.Abort()
                response.Close()
            )

            //accodrding to MSDN: You must call either the Stream.Close or the HttpWebResponse.Close method to close the stream 
            //and release the connection for reuse. It is not necessary to call both Stream.Close and HttpWebResponse.Close, but 
            //doing so does not cause an error. Failure to close the stream will cause your application to run out of connections.
            let responseStream = response.GetResponseStream()
            
            let chunkSize = 0x1000 //4KB
            let chunkList = new LinkedList<byte[]>()
            let rec read_stream rem len = async{
                let chunk, cnt = 
                    if rem = 0 then
                        Array.zeroCreate<byte>(chunkSize), chunkSize
                    else
                        chunkList.Last.Value, rem
                let! bytes_readed = responseStream.AsyncRead(chunk, chunkSize-cnt, cnt)
                if bytes_readed = 0 then
                    return len
                else
                    if rem=0 then
                        chunkList.AddLast(chunk) |> ignore
                    return! read_stream (cnt-bytes_readed) (len + bytes_readed)
            }
            let! totalLength = read_stream 0 0
            let streamLength = ref totalLength
            let chunkOffset = ref 0
            responseStream.Close()
            return {
                new Stream() with
                    override s.Position
                        with get() = raise <| new NotImplementedException()
                        and set(v) = raise <| new NotImplementedException()
                    override s.CanRead = true
                    override s.CanWrite = false
                    override s.CanSeek = false
                    override s.Length = int64(!streamLength)
                    override s.Seek(offset:int64, origin:SeekOrigin) = raise <| new NotImplementedException()
                    override s.SetLength(value:int64) = raise <| new NotImplementedException()
                    override s.Read(buffer:byte[], offset:int, count:int) = lock chunkList (fun ()->
                        if count = 0 then 
                            0
                        else
                            let rec copy ofs cnt = 
                                if !streamLength = 0 then
                                    ofs
                                else
                                    let chunkNode = chunkList.First
                                    let chunk = chunkNode.Value
                                    let chunkLength = chunk.Length - !chunkOffset
                                    if chunkLength <= cnt then
                                        Array.Copy(chunk, !chunkOffset, buffer, ofs, chunkLength)
                                        chunkOffset := 0
                                        streamLength := !streamLength - chunkLength
                                        chunkList.Remove(chunkNode)
                                        copy (ofs+chunkLength) (cnt-chunkLength)
                                    else
                                        Array.Copy(chunk, !chunkOffset, buffer, ofs, cnt)
                                        streamLength := !streamLength - cnt
                                        chunkOffset := !chunkOffset + cnt
                                        ofs+cnt
                            if count > !streamLength then
                                (copy offset !streamLength) - offset
                            else
                                (copy offset count) - offset
                    )
                    override s.Write(buffer:byte[], offset:int, count:int) = raise <| new NotImplementedException()
                    override s.Flush() = raise <| new NotImplementedException()
                end
            }
            
        }

        member this.RestoreSystem(backupPath:string):Async<unit> = async{
            let dev = session :> IDeviceAsync
            do! Async.SwitchToThreadPool()
            let files = Seq.toList(seq{
                use zip = Package.Open(backupPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                for part in zip.GetParts() do
                    let file = new BackupFile()
                    file.name <- part.Uri.OriginalString
                    file.data <- new AttachmentData()
                    use ps = part.GetStream()
                    file.data.Include <- Array.zeroCreate<byte>(int(ps.Length))
                    //TODO: transorm to async 
                    //let! bytesReaded = ps.AsyncRead(file.Data.Include, 0, int(ps.Length))
                    ps.Read(file.data.Include, 0, int(ps.Length)) |> ignore
                    yield file
            })
            do! dev.RestoreSystem(files |> List.toArray)
        }
        
//        member this.GetDefaultProfile() = async {
//            //TODO: get default profile, or create if there is no one
//            let! profiles = session.GetProfiles()
//            return profiles.First()
//        }

        member this.GetChannel(profileToken:string):Async<IChannel> = async{
            let! profile = session.GetProfile(profileToken)
//            let! profile = 
//                if profileToken |> IsNull then
//                    this.GetDefaultProfile()
//                else
//                    session.GetProfile(profileToken)
            
            //let streamSetup = new StreamSetup()
            //streamSetup.Stream <- StreamType.RTPUnicast
            //streamSetup.Transport <- new Transport()
            //streamSetup.Transport.Protocol <- TransportProtocol.UDP
            //streamSetup.Transport.Tunnel <- null
            //let! mediaUri = session.GetStreamUri(streamSetup, profile.token)
            //let vec = profile.VideoEncoderConfiguration
            
            let name = profile.name
            //let encoderResolution = vec.Resolution
            let profileToken = profile.token
            
            let ch = { 
                new IChannel with
                    member this.name = name
                    //member this.encoderResolution = encoderResolution
                    //member this.mediaUri = mediaUri
                    member this.profileToken = profileToken
                end
            }
            return ch
        }

        member this.GetChannelDescriptions():Async<ChannelDescription[]> = async{
            let! videoSources = session.GetVideoSources()
            let! profiles = session.GetProfiles()
            let channels = Seq.toList(seq{
                for vs in videoSources.OrderBy(fun x->x.token) do
                    yield new ChannelDescription(
                        vs = vs,
                        profiles = (profiles |> Seq.filter (fun p-> 
                            let vsc = p.videoSourceConfiguration
                            vsc |> NotNull && vsc.sourceToken = vs.token
                        ) |> Seq.toArray)
                    )
            })
            return channels |> List.toArray
        }

        member this.SetNameLocation(name:string, location:string) = async{
            let name_changed = name <> null
            let location_changed = location <> null
            let is_modified = name_changed || location_changed
            if not is_modified then return ()
            let! scopes = session.GetScopes()
            
            let scopes_to_set = seq{
                let changed_scopes = Seq.toList (seq{
                    if name_changed then
                        let prefix = 
                            let use_onvif_scope = 
                                scopes 
                                    |> Seq.filter (fun x -> x.scopeDef = ScopeDefinition.``fixed``)
                                    |> Seq.forall (fun x -> not(x.scopeItem.StartsWith(ScopeHelper.onvifNameScope)))
                            if use_onvif_scope then
                                ScopeHelper.onvifNameScope
                            else 
                                ScopeHelper.odmNameScope
                        //let value = String.Concat(prefix, Uri.EscapeDataString(model.current.name))
                        let value = Uri.EscapeUriString(name)
                        yield (prefix, value)
                    
                    if location_changed then
                        let prefix = 
                            let use_onvif_scope = 
                                scopes 
                                    |> Seq.filter (fun x -> x.scopeDef = ScopeDefinition.``fixed``)
                                    |> Seq.forall (fun x -> not(x.scopeItem.StartsWith(ScopeHelper.onvifLocationScope)))
                            if use_onvif_scope then
                                ScopeHelper.onvifLocationScope
                            else 
                                ScopeHelper.odmLocationScope
                        //let value = String.Concat(prefix, Uri.EscapeDataString(model.current.location))
                        let values = seq{
                            if location |> NotNull then
                                for x in location.Split([|';'|]) do
                                    let v = x.Trim()
                                    if not(String.IsNullOrEmpty(v)) then
                                        yield Uri.EscapeUriString(v)
                        }
                        for value in values do
                            yield (prefix, value)
                })
                
                let filter (x:string) = changed_scopes |> Seq.forall (fun (prefix, value) -> not(x.StartsWith(prefix)))
                
                yield! scopes
                    |> Seq.filter (fun x -> x.scopeDef = ScopeDefinition.configurable)
                    |> Seq.map (fun x -> x.scopeItem)
                    |> Seq.filter filter
                
                yield! changed_scopes |> Seq.map (fun (prefix, value) -> String.Concat(prefix, value))
            }
            do! session.SetScopes(scopes_to_set.ToArray())
        }

        member this.GetSnapshot(token:string) = async {
            let profileToken = token
//            let! profileToken = 
//                if token |> IsNull then
//                    async{
//                        let! profile = this.GetDefaultProfile()
//                        return profile.token
//                    }
//                else
//                    async{
//                        return token
//                    }
            let! mediaUri = session.GetSnapshotUri(profileToken)
            let! caps = session.GetCapabilities()
            let baseUri = new Uri(caps.media.xAddr)
            let uri = new Uri(baseUri, mediaUri.uri)
            let! stream = this.DownloadStream(uri, null(*"image/jpeg"*))
            return stream
        }

        member this.DownloadSchemes(uris:seq<Uri>) = async{
            let! schemes =
                uris
                    |> Seq.filter(fun uri-> uri.OriginalString <> "http://www.w3.org/2001/XMLSchema")
                    |> Seq.distinct
                    |> Seq.map (fun uri->(*async{*)
                        let schemaAsync = lock (caсhedSchemasLock) (fun () -> 
                            let caсheHit, cachedSchemaAsync = caсhedSchemasByLocation.TryGetValue(uri.OriginalString)
                            if caсheHit then
                                cachedSchemaAsync
                            else
                                let op = Async.Memoize(async{
                                    let! stream = this.DownloadStream(uri, "application/xml")
                                    do! Async.SwitchToThreadPool()
                                    return XmlSchema.Read(stream, null)
                                })
                                caсhedSchemasByLocation.[uri.OriginalString] <- op
                                op
                        )
                        schemaAsync
                        (*let! stream = this.DownloadStream(uri, "application/xml")
                        do! Async.SwitchToThreadPool()
                        return XmlSchema.Read(stream, null)*)
                    (*}*))
                    |> Async.Parallel
            do! Async.SwitchToThreadPool()
            let schemaSet = new XmlSchemaSet()
            for schema in schemes do
                schemaSet.Add(schema) |> ignore
            schemaSet.Compile()
            return schemaSet
        }
        
        member this.DownloadSchemes(uris:string[])= async{
            if uris |> NotNull then
                return! this.DownloadSchemes(uris |> Seq.map (fun uri->new Uri(uri)))
            else
                return null
        }


        member this.IsZeroConfigurationSupported() = async{
            let! caps = session.GetCapabilities()
            let net = caps.device |> IfNotNull (fun x->x.network)
            return NotNull(net) && net.zeroConfigurationSpecified && net.zeroConfiguration
        }

        member this.IsAnalyticsSupported() = async{
            let! caps = session.GetCapabilities()
            return not(String.IsNullOrWhiteSpace(caps.analytics |> IfNotNull (fun x->x.xAddr)))
        }

        member this.IsEventsSupported() = async{
            let! caps = session.GetCapabilities()
            return not(String.IsNullOrWhiteSpace(caps.events |> IfNotNull (fun x->x.xAddr)))
        }

        member this.IsPtzSupported() = async{
            let! caps = session.GetCapabilities()
            return NotNull(caps.ptz |> IfNotNull (fun x->x.xAddr))
        }

        member this.IsFirmwareUpgradeSupported():Async<bool> = async{
            let! modes = this.GetSupportedFirmwareUpgradeModes()
            return modes.mtom || modes.http
        }
//        member this.GetSupportedLogRetrievingModes():Async<SupportedLogRetrievingModes> = async{
//            let dev = session :> IDeviceAsync
//            let! caps = session.GetCapabilities()
//            if caps |> IsNull then
//                let! srvCaps  = dev.GetServiceCapabilities()
//                //TODO: try determine logging capabilities via service capabilities
//                return SupportedLogRetrievingModes.NotSupported
//            let sysCaps = 
//                if caps.Device=null then
//                    null
//                else
//                    caps.Device.System
//            if sysCaps |> IsNull then
//                return SupportedLogRetrievingModes.NotSupported
//            else
//                
//                let modes = seq{
//                    if  (NotNull(sysCaps.Extension) &&
//                        sysCaps.Extension.HttpSystemLoggingSpecified &&
//                        sysCaps.Extension.HttpSystemLogging)
//                    then
//                        yield async{return SupportedLogRetrievingModes.Http}
//                    else
//                        let onvifVersion = 
//                            if NotNull(sysCaps.SupportedVersions) then
//                                sysCaps.SupportedVersions.Max()
//                            else
//                                null
//
//                        ()
//
//                    if (let! srvCaps  = dev.GetServiceCapabilities() in srvcaps <> null) then
//                        ()
//                    if sysCaps.SystemLogging then
//                        yield async{ return SupportedLogRetrievingModes.Mtom}
//                }
//
//                let AsyncFold (src:seq<'a>) (h:'state->'a->async<'state>) (init:'state) = async{
//                    use itor = src.GetEnumerator()
//                    let rec loop(state) = async{
//                        if itor.MoveNext() then 
//                            let! new_state = h state (itor.Current)
//                            return! loop(new_state)
//                        else
//                            return state
//                    }
//                    return! loop(init) 
//                }
//                
//                //let! r2 = AsyncFold(
//
//                let! r = async{
//                    use itor = modes.GetEnumerator()
//                    let rec loop(s) = async{
//                        if not(itor.MoveNext()) then 
//                            return s 
//                        else 
//                            
//                            return! loop h ()
//                    }
//                    return! loop(SupportedLogRetrievingModes.NotSupported) 
//                }
//                let mtomSupported = sysCaps.FirmwareUpgrade
//                let! httpSupported = async{
//                    let onvifVersion = 
//                            if caps.Device.System.SupportedVersions <> null then
//                                caps.Device.System.SupportedVersions.Max()
//                            else
//                                null
//                    if onvifVersion<>null && onvifVersion >= new OnvifVersion(2,1) then
//                        let dev = session :> IDeviceAsync
//                        let! srvCaps  = dev.GetServiceCapabilities()
//                        return
//                            if srvCaps <> null && srvCaps.System<>null then
//                                srvCaps.System. . HttpFirmwareUpgradeSpecified && srvCaps.System.HttpFirmwareUpgrade
//                            else
//                                false
//                    else
//                        return false
//                }
//                return SupportedLogRetrievingModes.Mtom ||| SupportedLogRetrievingModes.Http
//        }
        member this.GetSupportedFirmwareUpgradeModes():Async<SupportedFirmwareUpgradeModes> = async{
            let! devInfo = session.GetDeviceInformation()
            //HACK: workaround for synesis devices
            let synBrands = ["synesis"; "euresys"; "euresys s.a."; "incotex"; "byterg" ]
            if NotNull(devInfo) && NotNull(devInfo.Manufacturer) && synBrands.Contains(devInfo.Manufacturer.Trim().ToLowerInvariant()) then
                return new SupportedFirmwareUpgradeModes(mtom=false, http=true)
            else
                let! caps = session.GetCapabilities()
                let sysCaps = caps.device |> IfNotNull(fun x->x.system)
                if sysCaps |> IsNull then
                    return new SupportedFirmwareUpgradeModes(false, false)
                else
                    let mtomSupported = sysCaps.firmwareUpgrade
                    let! httpSupported = async{
                        let onvifVersion = 
                                if caps.device.system.supportedVersions |> NotNull then
                                    caps.device.system.supportedVersions.Max()
                                else
                                    null

                        if NotNull(onvifVersion) && onvifVersion >= new OnvifVersion(2,1) then
                            let dev = session :> IDeviceAsync
                            let! srvCaps  = dev.GetServiceCapabilities()
                            return
                                if NotNull(srvCaps) && NotNull(srvCaps.System) then
                                    srvCaps.System.HttpFirmwareUpgradeSpecified && srvCaps.System.HttpFirmwareUpgrade
                                else
                                    false
                        else
                            return false
                    }
                    return new SupportedFirmwareUpgradeModes(mtom = mtomSupported, http = httpSupported)
        }

        member this.UpgradeFirmware(firmwarePath:string):Async<string> = async{
            let! modes = this.GetSupportedFirmwareUpgradeModes()
            
            if modes.http then
                use fs = new FileStream(firmwarePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                let! upgradeInfo = session.StartFirmwareUpgrade()
    //            if upgradeInfo.UploadDelay>0 then
    //                do! Async.Sleep(upgradeInfo.UploadDelay)
                let requestUri = new Uri(upgradeInfo.UploadUri)
                return! this.UploadStream(requestUri, fs, MediaTypeNames.Application.Octet (*"application/octet-stream"*), session.credentials)
            else if modes.mtom then
                use fs = new FileStream(firmwarePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                let! firmware = fs.AsyncRead(int(fs.Length))
                return! session.UpgradeSystemFirmware(firmware)
            else
                return raise (new Exception("firmware upgade not supported"))
        }


        /// <summary>Returns observable which can be used to retrieve events via PullPoint mechanism.</summary>
        member this.GetPullPointEvents():IObservable<OnvifEvent> =
            this.GetPullPointEvents(null :> FilterType)

        /// <summary>Returns observable which can be used to retrieve events via PullPoint mechanism.</summary>
        /// <param name="topicExpressionFilters">Describe subset of filters, which notification producer should use to filter events.</param>
        /// <param name="messageContentFilters">Describe subset of filters, which notification producer should use to filter events.</param>
        member this.GetPullPointEvents(topicExpressionFilters:seq<TopicExpressionFilter>, messageContentFilters:seq<MessageContentFilter>):IObservable<OnvifEvent> =
            let filters = seq{
                if messageContentFilters |> NotNull then
                    for x in messageContentFilters do
                        yield XmlExtensions.SerializeAsXElement(x)

                if topicExpressionFilters |> NotNull then
                    for x in topicExpressionFilters do
                        yield XmlExtensions.SerializeAsXElement(x)
            }
            this.GetPullPointEvents(filters)
        
        /// <summary>Returns observable which can be used to retrieve events via PullPoint mechanism.</summary>
        /// <param name="filters">Describe set of filters, which notification producer should use to filter events.</param>
        member this.GetPullPointEvents(filters:seq<XElement>):IObservable<OnvifEvent> = 
            let filter = 
                if filters |> NotNull then
                    let filters = filters |> Seq.toArray
                    if filters.Length >0 then
                        new FilterType(Any = filters)
                    else
                        null
                else
                    null
            this.GetPullPointEvents(filter)
        
        /// <summary>Returns observable which can be used to retrieve events via PullPoint mechanism.</summary>
        /// <param name="filter">
        /// Describe set of filters, which notification producer should use to filter events. 
        /// If null the pullpoint should notify all occurring events to the client.
        /// </param>
        member this.GetPullPointEvents(filter:FilterType):IObservable<OnvifEvent> = 
            Observable.Create(fun (observer:IObserver<OnvifEvent>) ->
                let cts = new CancellationTokenSource()
                let comp = async{
                    try
                        let! caps = session.GetCapabilities()
                        let evt = session :> IEventAsync
                        //let initialTerminationTime = Duration.FromSeconds(10.0)
                        //let initialTerminationTime = new AbsoluteOrRelativeTimeType()
                        //initialTerminationTime.text <- "PT10S"
                        let unsubscribe(subman:ISubscriptionManagerAsync)=async{
                            try
                                do! subman.Unsubscribe()
                            with err->
                                dbg.Error(err)
                        }
                        
                        let notify(resp:PullMessagesResponse) = 
                            for x in resp.NotificationMessage do
                                let msg = 
                                    if x.Message |> NotNull then
                                        let tt = "http://www.onvif.org/ver10/schema"
                                        if x.Message.LocalName = "Message" && x.Message.NamespaceURI = tt then
                                            x.Message.Deserialize<Message>()
                                        else
                                            null
                                    else
                                        null
                                let evt = 
                                    new OnvifEvent(
                                        //arrivalTime = resp.CurrentTime,
                                        producerReference = x.ProducerReference,
                                        subscriptionReference = x.SubscriptionReference,
                                        topic = x.Topic,
                                        message = msg
                                    )
                                observer.OnNext(evt)

                        let rec main() = 
                            async{
                                ///let! pullpoint = evt.CreatePullPointSubscription(filter,"PT20S", null, null)
                                //let! subman = session.CreateSubscriptionManagerClient(pullpoint.SubscriptionReference.Address.Value)

                                do! async{
                                    use disp = new SerialDisposable()
                                    let! subman = session.CreatePullPointSubscription(filter, "PT600S", null)
                                    disp.Disposable <- Disposable.Create(fun()->
                                        Async.StartImmediate(unsubscribe(subman))
                                    )
                                    do! loop(subman, XsDuration.FromSeconds(60.0), 1024)
                                }
                                do! Async.SleepEx(1000)//in case if something goes wrong wait 1sec. before resubscribe again
                                return! main()
                            }
                        and loop(subman, timeout, messageLimit) = 
                            async{
                                let! cont = async{
                                    try
                                        let! msgs = subman.PullMessages(timeout, messageLimit)
                                        return async{
                                            notify(msgs)
                                            return! renew(subman, timeout, messageLimit)
                                        }
                                    with
                                    | :? FaultException<PullMessagesFaultResponse> as fault ->
                                        //dbg.Error(fault)
                                        let maxTimeout = XsDuration.Parse(fault.Detail.MaxTimeout)
                                        let newTimeout = 
                                            if timeout.timeSpan > maxTimeout.timeSpan then
                                                maxTimeout
                                            else
                                                timeout
                                        let maxMesssageLimit = fault.Detail.MaxMessageLimit
                                        let newMessageLimit = 
                                            if messageLimit > maxMesssageLimit then
                                                maxMesssageLimit
                                            else
                                                messageLimit
                                        return loop(subman, XsDuration.Parse(fault.Detail.MaxTimeout) , fault.Detail.MaxMessageLimit)
                                    | err -> 
                                        dbg.Error(err)
                                        return async{return ()}
                                }
                                return! cont
                            }
                        and renew(subman, timeout, messageLimit) = 
                            async{
                                let! cont = async{
                                    try
                                        do! subman.Renew(timeout.Format(), null) |> Async.Ignore
                                        return loop(subman, timeout, messageLimit)
                                    with err->
                                        dbg.Error(err)
                                        return async{return ()}
                                }
                                return! cont
                            }
                        do! main()
                    with err->
                        dbg.Error(err)
                        observer.OnError(err)
                    return ()
                }
                Async.StartWithContinuations(
                    comp,
                    (fun res->()),
                    (fun err->dbg.Error(err)),
                    (fun err->()),
                    cts.Token
                )
                Disposable.Create(fun ()->
                    cts.Cancel()
                )
            )

        /// <summary>Returns observable which can be used to retrieve events via BaseNotification mechanism.</summary>
        /// <param name="port">Number of tcp port, that will be used to listen for events from subscribtion manager</param>
        member this.GetBaseEvents(port:int):IObservable<OnvifEvent> =
            this.GetBaseEvents(port, null :> FilterType)

        /// <summary>Returns observable which can be used to retrieve events via BaseNotification mechanism.</summary>
        /// <param name="port">Number of tcp port, that will be used to listen for events from subscribtion manager</param>
        /// <param name="topicExpressionFilters">Describe subset of filters, which notification producer should use to filter events. </param>
        /// <param name="messageContentFilters">Describe subset of filters, which notification producer should use to filter events. </param>
        member this.GetBaseEvents(port:int, topicExpressionFilters:seq<TopicExpressionFilter>, messageContentFilters:seq<MessageContentFilter>):IObservable<OnvifEvent> =
            let filters = seq{
                if messageContentFilters |> NotNull then
                    for x in messageContentFilters do
                        yield XmlExtensions.SerializeAsXElement(x)

                if topicExpressionFilters |> NotNull then
                    for x in topicExpressionFilters do
                        yield XmlExtensions.SerializeAsXElement(x)
            }
            this.GetBaseEvents(port, filters)

        /// <summary>Returns observable which can be used to retrieve events via BaseNotification mechanism.</summary>
        /// <param name="port">Number of tcp port, that will be used to listen for events from subscribtion manager</param>
        /// <param name="filters">Describe set of filters, which notification producer should use to filter events. </param>
        member this.GetBaseEvents(port:int, filters:seq<XElement>):IObservable<OnvifEvent> = 
            let filter = 
                if filters |> NotNull then
                    let filters = filters |> Seq.toArray
                    if filters.Length >0 then
                        new FilterType(Any = filters)
                    else
                        null
                else
                    null
            this.GetBaseEvents(port, filter)

        /// <summary>Returns observable which can be used to retrieve events via BaseNotification mechanism.</summary>
        /// <param name="port">Number of tcp port, that will be used to listen for events from subscribtion manager</param>
        /// <param name="filter">
        /// Describe set of filters, which notification producer should use to filter events. 
        /// If null the notification producer should notify all occurring events to the client.
        /// </param>
        member this.GetBaseEvents(port:int, filter:FilterType):IObservable<OnvifEvent> = 
            Observable.Create(fun (observer:IObserver<OnvifEvent>) ->
                let cts = new CancellationTokenSource()
                let consumerInstance = NotificationConsumerService.Create(fun messages ->
                    for x in messages do
                        let msg = 
                            if x.Message |> NotNull then
                                let tt = "http://www.onvif.org/ver10/schema"
                                if x.Message.LocalName = "Message" && x.Message.NamespaceURI = tt then
                                    x.Message.Deserialize<Message>()
                                else
                                    null
                            else
                                null
                        let evt = 
                            new OnvifEvent(
                                producerReference = x.ProducerReference,
                                subscriptionReference = x.SubscriptionReference,
                                topic = x.Topic,
                                message = msg
                            )
                        observer.OnNext(evt)
                )
                let serviceHost = new ServiceHost(consumerInstance)
                let comp = async{
                    try
                        use disposables = new CompositeDisposable()
                        let devUri = session.deviceUri
                        let socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        let conectAsync(socket:Socket, host:string, port:int) = Async.FromBeginEnd(host, port, (fun (host:string, port:int, cb:AsyncCallback, o:obj)->socket.BeginConnect(host, port, cb,o)), socket.EndConnect)
                        do! conectAsync(socket, devUri.Host, devUri.Port)
                        let localEndPoint = 
                            using (Disposable.Create(fun ()->socket.Close())) (fun _->
                                socket.LocalEndPoint :?> IPEndPoint
                            )
                        let CheckConnectivity(locIp:System.Net.IPAddress) = async{
                            use socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                            socket.SendBufferSize <- 128
                            socket.SendTimeout <- 2000 //2 sec.
                            socket.ReceiveBufferSize <- 128
                            socket.ReceiveTimeout <- 2000 //2 sec.
                            socket.Bind(new IPEndPoint(locIp, 0))
                            try
                                do! conectAsync(socket, devUri.Host, devUri.Port)
                                if socket.Connected then
                                    return Some locIp
                                else
                                    return None
                            with err->
                                return None
                        }
                        let! localIp = async{
                            if not(IPAddress.IsLoopback(localEndPoint.Address)) then
                                return localEndPoint.Address
                            else
                                let! ct = Async.CancellationToken
                                let CompleteWith = CreateCompletionPoint(fun cont ->
                                    cont()
                                )
                                return! Async.FromContinuations(fun (success, error, cancel)->
                                    let hostInfo = Dns.GetHostEntry(String.Empty)
                                    let locIps = 
                                        hostInfo.AddressList 
                                            |> Seq.filter (fun x->x.AddressFamily = AddressFamily.InterNetwork) 
                                            |> Seq.filter (fun x->not(IPAddress.IsLoopback(x))) 
                                            |> Seq.toArray
                                    let cnt = ref locIps.Length
                                    for locIp in locIps do
                                        Async.StartWithContinuations(
                                            async{
                                                return! CheckConnectivity(locIp)
                                            },
                                            (fun result->
                                                let c = Interlocked.Decrement(cnt)
                                                match result with
                                                | Some ip -> 
                                                    CompleteWith(fun()->
                                                        success ip
                                                    )
                                                | None -> 
                                                    if c = 0 then 
                                                        CompleteWith(fun()->
                                                            error(new Exception("failed to establish connection")) 
                                                        )
                                                    else ()
                                            ),
                                            (fun err->
                                                let c = Interlocked.Decrement(cnt)
                                                if c = 0 then 
                                                    CompleteWith(fun()->
                                                        error(new Exception("failed to establish connection")) 
                                                    )
                                                else 
                                                    ()
                                            ),
                                            (fun err->
                                                let c = Interlocked.Decrement(cnt)
                                                if c = 0 then 
                                                    CompleteWith(fun()->
                                                        error(new Exception("failed to establish connection")) 
                                                    )
                                                else 
                                                    ()
                                            ),
                                            ct
                                        )
                                )
                        }
                        
                        let consumerReference = sprintf "http://%s:%d/subscription-%d" (localIp.ToString()) port (GetSubscriptionId())
                        let binding = new WSHttpBinding(SecurityMode.None)
                        let serviceEndpoint = serviceHost.AddServiceEndpoint(typeof<NotificationConsumer>,binding, consumerReference)
                        let openHost() = Async.FromBeginEnd(serviceHost.BeginOpen, serviceHost.EndOpen)
                        let closeHost() = Async.FromBeginEnd(serviceHost.BeginClose, serviceHost.EndClose)
                        do! openHost()
                        disposables.Add(
                            Disposable.Create(fun()->
                                Async.StartWithContinuations(
                                    closeHost(),
                                    (fun res->()),
                                    (fun err->dbg.Error(err)),
                                    (fun err->())
                                )
                            )
                        )
                        let consumerEndpointReference = new EndpointReferenceType1()
                        consumerEndpointReference.Address <- new AttributedURIType() 
                        consumerEndpointReference.Address.Value <- consumerReference
                        let! subman = session.CreateBaseSubscription(consumerEndpointReference, filter, "PT60S", null)
                        disposables.Add(
                            Disposable.Create(fun()->
                                Async.StartWithContinuations(
                                    subman.Unsubscribe(),
                                    (fun res->()),
                                    (fun err->dbg.Error(err)),
                                    (fun err->())
                                )
                            )
                        )
                        let rec loop() = 
                            async{
                                do! Async.SleepEx(50000(*50sec*))
                                do! subman.Renew("PT60S", null) |> Async.Ignore
                                return! loop()
                            }
                        do! loop()
                    with err->
                        observer.OnError(err)
                        raise err
                    return ()
                }
                Async.StartWithContinuations(
                    comp,
                    (fun res->()),
                    (fun err->dbg.Error(err)),
                    (fun err->()),
                    cts.Token
                )
                Disposable.Create(fun ()->
                    cts.Cancel()
                )
            )

    end


    type FtpDownloader(session:INvtSession, baseDir:DirectoryInfo, maxFtpConnections) = class
        let baseDir =
            if baseDir |> IsNull then
                new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
            else
                baseDir
        let sem = new AsyncSemaphore(maxFtpConnections)
        //let baseDir = AppDomain.CurrentDomain.BaseDirectory
        member this.Download(uri:Uri) = async{
            let acquired = ref false
            let completed = ref false
            
            let path = new FileInfo(sprintf "%s\\%s" (baseDir.FullName) (uri.LocalPath))
            let tmp_path = new FileInfo(sprintf "%s.tmp" (path.FullName))
            
            try
                do! sem.Wait(acquired)
                if not(tmp_path.Directory.Exists) then
                    tmp_path.Directory.Create()
                use fs = new FileStream(tmp_path.FullName, FileMode.Create, FileAccess.Write, FileShare.None)
                //use fs = tmp_path.Create()
                let s = new OdmSession(session)
                use! os = s.FtpDownload(uri)
                let buffer = Array.zeroCreate<byte>(1024)
                let rec loop() = async{
                    let! readed = os.AsyncRead(buffer, 0, buffer.Length)
                    if readed <> 0 then
                        do! fs.AsyncWrite(buffer, 0, readed)
                        return! loop()
                    else
                        return ()
                }
                do! loop()
                fs.Close()
                if path.Exists then
                    path.Delete()
                elif not(path.Directory.Exists) then
                    path.Directory.Create()
                tmp_path.MoveTo(path.FullName)
                completed := true
                path.Refresh()
                return path
            finally
                if !acquired then
                    sem.Release()
                if not(!completed) && tmp_path.Exists then
                    tmp_path.Delete()
        }
    end
