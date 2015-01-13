namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Reactive.Disposables
    open System.Runtime.CompilerServices
    open System.ServiceModel
    open System.ServiceModel.Discovery
    open System.Text
    open System.Threading
    open System.Threading.Tasks

    open onvif.services
    open onvif10_device
    open utils
    open utils.fsharp

    [<AllowNullLiteral>]
    type IDeviceAsync = interface
        //onvif 1.2
        abstract GetDeviceInformation: unit->Async<GetDeviceInformationResponse>
        abstract SetSystemDateAndTime: dateTimeType:SetDateTimeType*daylightSavings:bool*timeZone:TimeZone*utcDateTime:DateTime->Async<unit>
        abstract GetSystemDateAndTime: unit->Async<SystemDateTime>
        abstract SetSystemFactoryDefault: factoryDefault:FactoryDefaultType->Async<unit>
        abstract UpgradeSystemFirmware: firmware:byte[] -> Async<string>
        abstract SystemReboot: unit->Async<string>
        abstract RestoreSystem: backupFiles:BackupFile[]->Async<unit>
        abstract GetSystemBackup: unit->Async<BackupFile[]>
        abstract GetSystemLog: logType:SystemLogType->Async<SystemLog>
        abstract GetSystemSupportInformation: unit->Async<SupportInformation>
        abstract GetScopes: unit->Async<Scope[]>
        abstract SetScopes: scopes:string[]->Async<unit>
        abstract AddScopes: scopeItems:string[] -> Async<unit>
        abstract RemoveScopes: scopeItems:string[] -> Async<string[]>
        abstract GetDiscoveryMode: unit -> Async<DiscoveryMode>
        abstract SetDiscoveryMode: mode:DiscoveryMode -> Async<unit>
        abstract GetRemoteDiscoveryMode: unit -> Async<DiscoveryMode>
        abstract SetRemoteDiscoveryMode: mode:DiscoveryMode -> Async<unit>
        abstract GetDPAddresses: unit -> Async<NetworkHost[]>
        abstract SetDPAddresses: addresses:NetworkHost[] -> Async<unit>
        abstract GetUsers: unit->Async<User[]>
        abstract CreateUsers: users:User[]->Async<unit>
        abstract DeleteUsers: userNames:string[]->Async<unit>
        abstract SetUser: users:User[]->Async<unit>
        abstract GetCapabilities: [<ParamArray>]categories:CapabilityCategory[]->Async<Capabilities>
        abstract GetHostname: unit -> Async<HostnameInformation>
        abstract SetHostname: name:string -> Async<unit>
        abstract GetDNS: unit->Async<DNSInformation>
        abstract SetDNS: fromDHCP:bool * searchDomain:string[] * dnsManual:IPAddress[]->Async<unit>
        abstract GetNTP: unit->Async<NTPInformation>
        abstract SetNTP: fromDhcp:bool*ntpManual:NetworkHost[]->Async<unit>
        abstract GetDynamicDNS: unit -> Async<DynamicDNSInformation>
        abstract SetDynamicDNS: dynDnsType:DynamicDNSType*name:string*ttl:XsDuration -> Async<unit>
        abstract GetNetworkInterfaces: unit->Async<NetworkInterface[]>
        abstract SetNetworkInterfaces: token:string*networkInterface:NetworkInterfaceSetConfiguration->Async<bool>
        abstract GetNetworkProtocols: unit -> Async<NetworkProtocol[]>
        abstract SetNetworkProtocols: protocols:NetworkProtocol[] -> Async<unit>
        abstract GetNetworkDefaultGateway: unit->Async<NetworkGateway>
        abstract SetNetworkDefaultGateway: ipv4Addresses:string[]*ipv6Addresses:string[]->Async<unit>
        abstract GetZeroConfiguration: unit -> Async<NetworkZeroConfiguration>
        abstract SetZeroConfiguration: nicToken: string*enabled:bool -> Async<unit>
        abstract GetIPAddressFilter: unit -> Async<IPAddressFilter>
        abstract SetIPAddressFilter: filter:IPAddressFilter -> Async<unit>
        abstract AddIPAddressFilter: filter:IPAddressFilter -> Async<unit>
        abstract RemoveIPAddressFilter: filter:IPAddressFilter -> Async<unit>
        abstract GetAccessPolicy: unit -> Async<BinaryData>
        abstract SetAccessPolicy: policyFile:BinaryData -> Async<unit>
        abstract CreateCertificate: certificateID:string*subject:string*validNotBefore:System.DateTime*validNotAfter:System.DateTime->Async<Certificate>
        abstract GetCertificates: unit->Async<Certificate[]>
        abstract GetCertificatesStatus: unit->Async<CertificateStatus[]>
        abstract SetCertificatesStatus: certificateStatuses:CertificateStatus[]->Async<unit>
        abstract DeleteCertificates: certificateIds:string[]->Async<unit>
        abstract GetPkcs10Request: certificateID:string*subject:string*attributes:BinaryData->Async<unit>
        abstract LoadCertificates: certificate:Certificate[]->Async<unit>
        abstract GetClientCertificateMode: unit->Async<bool>
        abstract SetClientCertificateMode: enabled:bool->Async<unit>
        abstract GetRelayOutputs: unit -> Async<RelayOutput[]>
        abstract SetRelayOutputSettings: token:string*settings:RelayOutputSettings -> Async<unit>
        abstract SetRelayOutputState: token:string*state:RelayLogicalState -> Async<unit>

        //onvif 2.1
        abstract GetServices: includeCapability:bool -> Async<Service[]>
        abstract GetServiceCapabilities: unit -> Async<DeviceServiceCapabilities1>
//        abstract GetEndpointReference: request:GetEndpointReferenceRequest -> Async<GetEndpointReferenceResponse>
//        abstract GetRemoteUser: request:GetRemoteUserRequest -> Async<GetRemoteUserResponse>
//        abstract SetRemoteUser: request:SetRemoteUserRequest -> Async<SetRemoteUserResponse>
        abstract GetWsdlUrl: unit -> Async<string>
        abstract SetHostnameFromDHCP: fromDhcp:bool -> Async<bool>
//        abstract SendAuxiliaryCommand: request:SendAuxiliaryCommandRequest -> Async<SendAuxiliaryCommandResponse>
//        abstract GetCACertificates: request:GetCACertificatesRequest -> Async<GetCACertificatesResponse>
//        abstract LoadCertificateWithPrivateKey: request:LoadCertificateWithPrivateKeyRequest -> Async<LoadCertificateWithPrivateKeyResponse>
//        abstract GetCertificateInformation: request:GetCertificateInformationRequest -> Async<GetCertificateInformationResponse>
//        abstract LoadCACertificates: request:LoadCACertificatesRequest -> Async<LoadCACertificatesResponse>
//        abstract CreateDot1XConfiguration: request:CreateDot1XConfigurationRequest -> Async<CreateDot1XConfigurationResponse>
//        abstract SetDot1XConfiguration: request:SetDot1XConfigurationRequest -> Async<SetDot1XConfigurationResponse>
//        abstract GetDot1XConfiguration: request:GetDot1XConfigurationRequest -> Async<GetDot1XConfigurationResponse>
//        abstract GetDot1XConfigurations: request:GetDot1XConfigurationsRequest -> Async<GetDot1XConfigurationsResponse>
//        abstract DeleteDot1XConfiguration: request:DeleteDot1XConfigurationRequest -> Async<DeleteDot1XConfigurationResponse>
//        abstract GetDot11Capabilities: request:GetDot11CapabilitiesRequest -> Async<GetDot11CapabilitiesResponse>
//        abstract GetDot11Status: request:GetDot11StatusRequest -> Async<GetDot11StatusResponse>
//        abstract ScanAvailableDot11Networks: request:ScanAvailableDot11NetworksRequest -> Async<ScanAvailableDot11NetworksResponse>
//        abstract GetSystemUris: request:GetSystemUrisRequest -> Async<GetSystemUrisResponse>
        abstract StartFirmwareUpgrade: unit->Async<StartFirmwareUpgradeResponse>
//        abstract StartSystemRestore: request:StartSystemRestoreRequest -> Async<StartSystemRestoreResponse>
    end

    type DeviceAsync(proxy:Device) = class
        
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")
        
//        let commObj = proxy :?> ICommunicationObject

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy

//        interface IDisposable with
//            member this.Dispose() = 
//                let disp = proxy :?> IDisposable
//                disp.Dispose()
//        end

//        interface ICommunicationObject with
//            member this.Abort() = commObj.Abort()
//            member this.State = commObj.State
//            member this.BeginClose(callback:AsyncCallback, state:obj) = commObj.BeginClose(callback, state)
//            member this.BeginClose(timeout:TimeSpan, callback:AsyncCallback, state:obj) = commObj.BeginClose(timeout, callback, state)
//            member this.EndClose(result:IAsyncResult) = commObj.EndClose(result)
//            member this.Close() = commObj.Close()
//            member this.Close(timeout:TimeSpan) = commObj.Close(timeout)
//            member this.BeginOpen(callback:AsyncCallback, state:obj) = commObj.BeginOpen(callback, state)
//            member this.BeginOpen(timeout:TimeSpan, callback:AsyncCallback, state:obj) = commObj.BeginOpen(timeout, callback, state)
//            member this.EndOpen(result:IAsyncResult) = commObj.EndOpen(result)
//            member this.Open() = commObj.Open()
//            member this.Open(timeout:TimeSpan) = commObj.Open(timeout)
//            member this.add_Closed(handler) = commObj.Closed.AddHandler(handler)
//            member this.remove_Closed(handler) = commObj.Closed.RemoveHandler(handler)
//            member this.add_Closing(handler) = commObj.Closing.AddHandler(handler)
//            member this.remove_Closing(handler) = commObj.Closing.RemoveHandler(handler)
//            member this.add_Opened(handler) = commObj.Opened.AddHandler(handler)
//            member this.remove_Opened(handler) = commObj.Opened.RemoveHandler(handler)
//            member this.add_Opening(handler) = commObj.Opening.AddHandler(handler)
//            member this.remove_Opening(handler) = commObj.Opening.RemoveHandler(handler)
//            member this.add_Faulted(handler) = commObj.Faulted.AddHandler(handler)
//            member this.remove_Faulted(handler) = commObj.Faulted.RemoveHandler(handler)
//        end

        interface IDeviceAsync with
            //onvif 1.2
            member this.GetCapabilities(categories:CapabilityCategory[]):Async<Capabilities> = async{
                let request = new GetCapabilitiesRequest()
                if IsNull(categories) || categories.Length = 0 then
                    request.Category <- [|CapabilityCategory.all|]
                else
                    request.Category <- categories
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCapabilities, proxy.EndGetCapabilities)
                if response.Capabilities |> NotNull then
                    return response.Capabilities
                else
                    return new Capabilities()
            }
            
            member this.GetDeviceInformation() = async{
                let request = new GetDeviceInformationRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetDeviceInformation, proxy.EndGetDeviceInformation)
                return response
            }
            
            member this.GetScopes() = async{
                let request = new GetScopesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetScopes, proxy.EndGetScopes)
                return response.Scopes
            }
            
            member this.SetScopes(scopes:string[]) = async{
                let request = new SetScopesRequest()
                request.Scopes <- scopes
                let! response = Async.FromBeginEnd(request, proxy.BeginSetScopes, proxy.EndSetScopes)
                return ()
            }
            
            member this.GetNetworkInterfaces() = async{
                let request = new GetNetworkInterfacesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetNetworkInterfaces, proxy.EndGetNetworkInterfaces)
                return response.NetworkInterfaces
            }
            
            member this.SetNetworkInterfaces(token:string, networkInterface:NetworkInterfaceSetConfiguration) = async{
                let request = new SetNetworkInterfacesRequest()
                request.InterfaceToken <- token
                request.NetworkInterface <- networkInterface
                let! response = Async.FromBeginEnd(request, proxy.BeginSetNetworkInterfaces, proxy.EndSetNetworkInterfaces)
                return response.RebootNeeded
            }
            
            member this.GetNetworkDefaultGateway() = async{
                let request = new GetNetworkDefaultGatewayRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetNetworkDefaultGateway, proxy.EndGetNetworkDefaultGateway)
                return response.NetworkGateway
            }
            
            member this.SetNetworkDefaultGateway(ipv4Addresses:string[], ipv6Addresses:string[]) = async{
                let request = new SetNetworkDefaultGatewayRequest()
                request.IPv4Address <- ipv4Addresses
                request.IPv6Address <- ipv6Addresses
                let! response = Async.FromBeginEnd(request, proxy.BeginSetNetworkDefaultGateway, proxy.EndSetNetworkDefaultGateway)
                return ()
            }
            
            member this.GetDNS() = async{
                let request = new GetDNSRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetDNS, proxy.EndGetDNS)
                return response.DNSInformation
            }
            
            member this.SetDNS(fromDHCP:bool, searchDomain:string[], dnsManual:IPAddress[]) = async{
                let request = new SetDNSRequest()
                request.DNSManual <- dnsManual
                request.FromDHCP <- fromDHCP
                request.SearchDomain <- searchDomain
                let! response = Async.FromBeginEnd(request, proxy.BeginSetDNS, proxy.EndSetDNS)
                return ()
            }
            
            member this.GetSystemDateAndTime() = async{
                let request = new GetSystemDateAndTimeRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSystemDateAndTime, proxy.EndGetSystemDateAndTime)
                return response.SystemDateAndTime
            }
            
            member this.SetSystemDateAndTime(dateTimeType:SetDateTimeType, daylightSavings:bool, timeZone:TimeZone , utcDateTime:DateTime) = async{
                let request = new SetSystemDateAndTimeRequest()
                request.DateTimeType <- dateTimeType
                request.DaylightSavings <- daylightSavings
                request.TimeZone <- timeZone
                request.UTCDateTime <- utcDateTime
                let! response = Async.FromBeginEnd(request, proxy.BeginSetSystemDateAndTime, proxy.EndSetSystemDateAndTime)
                return ()
            }
            
            member this.GetNTP() = async{
                let request = new GetNTPRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetNTP, proxy.EndGetNTP)
                return response.NTPInformation
            }
            
            member this.SetNTP(fromDhcp: bool, ntpManual: NetworkHost[]) = async{
                let request = new SetNTPRequest()
                request.FromDHCP <- fromDhcp
                request.NTPManual <- ntpManual
                let! response = Async.FromBeginEnd(request, proxy.BeginSetNTP, proxy.EndSetNTP)
                return ()
            }
            
            member this.GetUsers() = async{
                let request = new GetUsersRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetUsers, proxy.EndGetUsers)
                return response.User
            }
            
            member this.CreateUsers(users: User[]) = async{
                let request = new CreateUsersRequest()
                request.User <- users
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateUsers, proxy.EndCreateUsers)
                return ()
            }
            
            member this.SetUser(users: User[]) = async{
                let request = new SetUserRequest()
                request.User <- users
                let! response = Async.FromBeginEnd(request, proxy.BeginSetUser, proxy.EndSetUser)
                return ()
            }
            
            member this.DeleteUsers(userNames:string[]) = async{
                let request = new DeleteUsersRequest()
                request.Username <- userNames
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteUsers, proxy.EndDeleteUsers)
                return ()
            }
            
            member this.SystemReboot() = async{
                let request = new SystemRebootRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginSystemReboot, proxy.EndSystemReboot)
                return response.Message
            }
            
            member this.SetSystemFactoryDefault(factoryDefault:FactoryDefaultType) = async{
                let request = new SetSystemFactoryDefaultRequest()
                request.FactoryDefault <- factoryDefault
                let! response = Async.FromBeginEnd(request, proxy.BeginSetSystemFactoryDefault, proxy.EndSetSystemFactoryDefault)
                return ()
            }
            
            member this.GetSystemLog(logType:SystemLogType) = async{
                let request = new GetSystemLogRequest()
                request.LogType <- logType
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSystemLog, proxy.EndGetSystemLog)
                return response.SystemLog
            }
            
            member this.UpgradeSystemFirmware(firmware:byte[]) = async{
                let request = new UpgradeSystemFirmwareRequest()
                request.Firmware <- new AttachmentData()
                request.Firmware.Include <- firmware
                let! response = Async.FromBeginEnd(request, proxy.BeginUpgradeSystemFirmware, proxy.EndUpgradeSystemFirmware)
                return response.Message
            }
            
            member this.RestoreSystem(backupFiles:BackupFile[]) = async{
                let request = new RestoreSystemRequest()
                request.BackupFiles <- backupFiles
                let! response = Async.FromBeginEnd(request, proxy.BeginRestoreSystem, proxy.EndRestoreSystem)
                return ()
            }
            
            member this.GetSystemBackup() = async{
                let request = new GetSystemBackupRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSystemBackup, proxy.EndGetSystemBackup)
                return response.BackupFiles
            }
            
            member this.GetSystemSupportInformation() = async{
                let request = new GetSystemSupportInformationRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSystemSupportInformation, proxy.EndGetSystemSupportInformation)
                return response.SupportInformation
            }
            
            member this.CreateCertificate(certificateID:string, subject:string, validNotBefore:System.DateTime, validNotAfter:System.DateTime) = async{
                let request = new CreateCertificateRequest()
                request.CertificateID <- certificateID
                request.Subject <- subject
                request.ValidNotBefore <- validNotBefore
                request.ValidNotAfter <- validNotAfter
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateCertificate, proxy.EndCreateCertificate)
                return response.NvtCertificate
            }
            
            member this.GetCertificates() = async{
                let request = new GetCertificatesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCertificates, proxy.EndGetCertificates)
                return response.NvtCertificate
            }
            
            member this.GetCertificatesStatus() = async{
                let request = new GetCertificatesStatusRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCertificatesStatus, proxy.EndGetCertificatesStatus)
                return response.CertificateStatus
            }
            
            member this.SetCertificatesStatus(certificateStatuses:CertificateStatus[]) = async{
                let request = new SetCertificatesStatusRequest()
                request.CertificateStatus <- certificateStatuses
                let! response = Async.FromBeginEnd(request, proxy.BeginSetCertificatesStatus, proxy.EndSetCertificatesStatus)
                return ()
            }
            
            member this.DeleteCertificates(certificateIds:string[]) = async{
                let request = new DeleteCertificatesRequest()
                request.CertificateID <- certificateIds
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteCertificates, proxy.EndDeleteCertificates)
                return ()
            }
            
            member this.LoadCertificates(certificates:Certificate[]) = async{
                let request = new LoadCertificatesRequest()
                request.NVTCertificate <- certificates
                let! response = Async.FromBeginEnd(request, proxy.BeginLoadCertificates, proxy.EndLoadCertificates)
                return ()
            }
            
            member this.GetClientCertificateMode() = async{
                let request = new GetClientCertificateModeRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetClientCertificateMode, proxy.EndGetClientCertificateMode)
                return response.Enabled
            }
            
            member this.SetClientCertificateMode(enabled:bool) = async{
                let request = new SetClientCertificateModeRequest()
                request.Enabled <- enabled
                let! response = Async.FromBeginEnd(request, proxy.BeginSetClientCertificateMode, proxy.EndSetClientCertificateMode)
                return ()
            }
            
            member this.GetPkcs10Request(certificateID:string, subject:string, attributes:BinaryData) = async{
                let request = new GetPkcs10RequestRequest()
                request.CertificateID <- certificateID
                request.Subject <- subject
                request.Attributes <- attributes
                let! response = Async.FromBeginEnd(request, proxy.BeginGetPkcs10Request, proxy.EndGetPkcs10Request)
                return ()
            }

//------------------------------------------------------------------------------------------------------------
//
//
//------------------------------------------------------------------------------------------------------------

            member this.AddScopes(scopeItems:string[]):Async<unit> = async{
                let request = new AddScopesRequest()
                request.ScopeItem <- scopeItems
                let! response = Async.FromBeginEnd(request, proxy.BeginAddScopes, proxy.EndAddScopes)
                return ()
            }
            member this.RemoveScopes(scopeItems:string[]):Async<string[]> = async{
                let request = new RemoveScopesRequest()
                request.ScopeItem <- scopeItems
                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveScopes, proxy.EndRemoveScopes)
                return response.ScopeItem
            }
            member this.GetDiscoveryMode():Async<DiscoveryMode> = async{
                let request = new GetDiscoveryModeRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetDiscoveryMode, proxy.EndGetDiscoveryMode)
                return response.DiscoveryMode
            }
            member this.SetDiscoveryMode(mode:DiscoveryMode):Async<unit> = async{
                let request = new SetDiscoveryModeRequest()
                request.DiscoveryMode <- mode
                let! response = Async.FromBeginEnd(request, proxy.BeginSetDiscoveryMode, proxy.EndSetDiscoveryMode)
                return ()
            }
            member this.GetRemoteDiscoveryMode():Async<DiscoveryMode> = async{
                let request = new GetRemoteDiscoveryModeRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRemoteDiscoveryMode, proxy.EndGetRemoteDiscoveryMode)
                return response.RemoteDiscoveryMode
            }
            member this.SetRemoteDiscoveryMode(mode:DiscoveryMode):Async<unit> = async{
                let request = new SetRemoteDiscoveryModeRequest()
                request.RemoteDiscoveryMode <- mode
                let! response = Async.FromBeginEnd(request, proxy.BeginSetRemoteDiscoveryMode, proxy.EndSetRemoteDiscoveryMode)
                return ()
            }
            member this.GetDPAddresses():Async<NetworkHost[]> = async{
                let request = new GetDPAddressesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetDPAddresses, proxy.EndGetDPAddresses)
                return response.DPAddress
            }
            member this.SetDPAddresses(addresses:NetworkHost[]):Async<unit> = async{
                let request = new SetDPAddressesRequest()
                request.DPAddress <- addresses
                let! response = Async.FromBeginEnd(request, proxy.BeginSetDPAddresses, proxy.EndSetDPAddresses)
                return ()
            }
            member this.GetHostname():Async<HostnameInformation> = async{
                let request = new GetHostnameRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetHostname, proxy.EndGetHostname)
                return response.HostnameInformation
            }
            member this.SetHostname(name:string):Async<unit> = async{
                let request = new SetHostnameRequest()
                request.Name <- name
                let! response = Async.FromBeginEnd(request, proxy.BeginSetHostname, proxy.EndSetHostname)
                return ()
            }
            member this.GetDynamicDNS():Async<DynamicDNSInformation> = async{
                let request = new GetDynamicDNSRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetDynamicDNS, proxy.EndGetDynamicDNS)
                return response.DynamicDNSInformation
            }
            member this.SetDynamicDNS(dynDnsType:DynamicDNSType, name:string, ttl:XsDuration):Async<unit> = async{
                let request = new SetDynamicDNSRequest()
                request.Type <- dynDnsType
                request.Name <- name
                if ttl |> NotNull then
                    request.TTL <- ttl.Format()
                let! response = Async.FromBeginEnd(request, proxy.BeginSetDynamicDNS, proxy.EndSetDynamicDNS)
                return ()
            }
            member this.GetNetworkProtocols():Async<NetworkProtocol[]> = async{
                let request = new GetNetworkProtocolsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetNetworkProtocols, proxy.EndGetNetworkProtocols)
                return response.NetworkProtocols
            }
            member this.SetNetworkProtocols(protocols:NetworkProtocol[]):Async<unit> = async{
                let request = new SetNetworkProtocolsRequest()
                request.NetworkProtocols <- protocols
                let! response = Async.FromBeginEnd(request, proxy.BeginSetNetworkProtocols, proxy.EndSetNetworkProtocols)
                return ()
            }
            member this.GetZeroConfiguration():Async<NetworkZeroConfiguration> = async{
                let request = new GetZeroConfigurationRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetZeroConfiguration, proxy.EndGetZeroConfiguration)
                return response.ZeroConfiguration
            }
            member this.SetZeroConfiguration(nicToken: string, enabled:bool):Async<unit> = async{
                let request = new SetZeroConfigurationRequest()
                request.InterfaceToken <- nicToken
                request.Enabled <- enabled
                let! response = Async.FromBeginEnd(request, proxy.BeginSetZeroConfiguration, proxy.EndSetZeroConfiguration)
                return ()
            }
            member this.GetIPAddressFilter():Async<IPAddressFilter> = async{
                let request = new GetIPAddressFilterRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetIPAddressFilter, proxy.EndGetIPAddressFilter)
                return response.IPAddressFilter
            }
            member this.SetIPAddressFilter(filter:IPAddressFilter):Async<unit> = async{
                let request = new SetIPAddressFilterRequest()
                request.IPAddressFilter <- filter
                let! response = Async.FromBeginEnd(request, proxy.BeginSetIPAddressFilter, proxy.EndSetIPAddressFilter)
                return ()
            }
            member this.AddIPAddressFilter(filter:IPAddressFilter):Async<unit> = async{
                let request = new AddIPAddressFilterRequest()
                request.IPAddressFilter <- filter
                let! response = Async.FromBeginEnd(request, proxy.BeginAddIPAddressFilter, proxy.EndAddIPAddressFilter)
                return ()
            }
            member this.RemoveIPAddressFilter(filter:IPAddressFilter):Async<unit> = async{
                let request = new RemoveIPAddressFilterRequest()
                request.IPAddressFilter <- filter
                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveIPAddressFilter, proxy.EndRemoveIPAddressFilter)
                return ()
            }
            member this.GetAccessPolicy():Async<BinaryData> = async{
                let request = new GetAccessPolicyRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAccessPolicy, proxy.EndGetAccessPolicy)
                return response.PolicyFile
            }
            member this.SetAccessPolicy(policyFile:BinaryData):Async<unit> = async{
                let request = new SetAccessPolicyRequest()
                request.PolicyFile <- policyFile
                let! response = Async.FromBeginEnd(request, proxy.BeginSetAccessPolicy, proxy.EndSetAccessPolicy)
                return ()
            }
            member this.GetRelayOutputs():Async<RelayOutput[]> = async{
                let request = new GetRelayOutputsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRelayOutputs, proxy.EndGetRelayOutputs)
                return response.RelayOutputs
            }
            member this.SetRelayOutputSettings(token:string, settings:RelayOutputSettings):Async<unit> = async{
                let request = new SetRelayOutputSettingsRequest()
                request.RelayOutputToken <- token
                request.Properties <- settings
                let! response = Async.FromBeginEnd(request, proxy.BeginSetRelayOutputSettings, proxy.EndSetRelayOutputSettings)
                return ()
            }
            member this.SetRelayOutputState(token:string, state:RelayLogicalState):Async<unit> = async{
                let request = new SetRelayOutputStateRequest()
                request.RelayOutputToken <- token
                request.LogicalState <- state
                let! response = Async.FromBeginEnd(request, proxy.BeginSetRelayOutputState, proxy.EndSetRelayOutputState)
                return ()
            }
//------------------------------------------------------------------------------------------------------------
//
//
//------------------------------------------------------------------------------------------------------------
            //onvif 2.1
            member this.GetServices(includeCapability:bool):Async<Service[]> = async{
                let request = new GetServicesRequest()
                request.IncludeCapability <- includeCapability
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServices, proxy.EndGetServices)
                return response.Service
            }
            member this.GetServiceCapabilities():Async<DeviceServiceCapabilities1> = async{
                let request = new GetServiceCapabilitiesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response.Capabilities
            }
//            member this.GetEndpointReference(request:GetEndpointReferenceRequest): Async<GetEndpointReferenceResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetEndpointReference, proxy.EndGetEndpointReference)
//                return response
//            }
//            member this.GetRemoteUser(request:GetRemoteUserRequest): Async<GetRemoteUserResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetRemoteUser, proxy.EndGetRemoteUser)
//                return response
//            }
//            member this.SetRemoteUser(request:SetRemoteUserRequest): Async<SetRemoteUserResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginSetRemoteUser, proxy.EndSetRemoteUser)
//                return response
//            }
            member this.GetWsdlUrl():Async<string> = async{
                let request = new GetWsdlUrlRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetWsdlUrl, proxy.EndGetWsdlUrl)
                return response.WsdlUrl
            }
            member this.SetHostnameFromDHCP(fromDhcp:bool): Async<bool> = async{
                let request = new SetHostnameFromDHCPRequest()
                request.FromDHCP <- fromDhcp;
                let! response = Async.FromBeginEnd(request, proxy.BeginSetHostnameFromDHCP, proxy.EndSetHostnameFromDHCP)
                return response.RebootNeeded
            }
//            member this.SendAuxiliaryCommand(request:SendAuxiliaryCommandRequest): Async<SendAuxiliaryCommandResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginSendAuxiliaryCommand, proxy.EndSendAuxiliaryCommand)
//                return response
//            }
//            member this.GetCACertificates(request:GetCACertificatesRequest): Async<GetCACertificatesResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetCACertificates, proxy.EndGetCACertificates)
//                return response
//            }
//            member this.LoadCertificateWithPrivateKey(request:LoadCertificateWithPrivateKeyRequest): Async<LoadCertificateWithPrivateKeyResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginLoadCertificateWithPrivateKey, proxy.EndLoadCertificateWithPrivateKey)
//                return response
//            }
//            member this.GetCertificateInformation(request:GetCertificateInformationRequest): Async<GetCertificateInformationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetCertificateInformation, proxy.EndGetCertificateInformation)
//                return response
//            }
//            member this.LoadCACertificates(request:LoadCACertificatesRequest): Async<LoadCACertificatesResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginLoadCACertificates, proxy.EndLoadCACertificates)
//                return response
//            }
//            member this.CreateDot1XConfiguration(request:CreateDot1XConfigurationRequest): Async<CreateDot1XConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginCreateDot1XConfiguration, proxy.EndCreateDot1XConfiguration)
//                return response
//            }
//            member this.SetDot1XConfiguration(request:SetDot1XConfigurationRequest): Async<SetDot1XConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginSetDot1XConfiguration, proxy.EndSetDot1XConfiguration)
//                return response
//            }
//            member this.GetDot1XConfiguration(request:GetDot1XConfigurationRequest): Async<GetDot1XConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetDot1XConfiguration, proxy.EndGetDot1XConfiguration)
//                return response
//            }
//            member this.GetDot1XConfigurations(request:GetDot1XConfigurationsRequest): Async<GetDot1XConfigurationsResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetDot1XConfigurations, proxy.EndGetDot1XConfigurations)
//                return response
//            }
//            member this.DeleteDot1XConfiguration(request:DeleteDot1XConfigurationRequest): Async<DeleteDot1XConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteDot1XConfiguration, proxy.EndDeleteDot1XConfiguration)
//                return response
//            }
//            member this.GetDot11Capabilities(request:GetDot11CapabilitiesRequest): Async<GetDot11CapabilitiesResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetDot11Capabilities, proxy.EndGetDot11Capabilities)
//                return response
//            }
//            member this.GetDot11Status(request:GetDot11StatusRequest): Async<GetDot11StatusResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetDot11Status, proxy.EndGetDot11Status)
//                return response
//            }
//            member this.ScanAvailableDot11Networks(request:ScanAvailableDot11NetworksRequest): Async<ScanAvailableDot11NetworksResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginScanAvailableDot11Networks, proxy.EndScanAvailableDot11Networks)
//                return response
//            }
//            member this.GetSystemUris(request:GetSystemUrisRequest): Async<GetSystemUrisResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetSystemUris, proxy.EndGetSystemUris)
//                return response
//            }
            member this.StartFirmwareUpgrade() = async{
                let request = new StartFirmwareUpgradeRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginStartFirmwareUpgrade, proxy.EndStartFirmwareUpgrade)
                return response
            }
//            member this.StartSystemRestore(request:StartSystemRestoreRequest): Async<StartSystemRestoreResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginStartSystemRestore, proxy.EndStartSystemRestore)
//                return response
//            }

        end
    end

