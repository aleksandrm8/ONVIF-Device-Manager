namespace odm.ui.activities
    open System
    open System.Linq
    //open System.Disposables
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Net.Sockets
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    open onvif.services
    open onvif.utils

    open odm.core
    open odm.infra
    open odm.onvif
    open utils
    //open utils.extensions
    //open odm.models
    open utils.fsharp
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    type NetworkSettingsActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            let! nics, ntpInfo, zeroConfSupported, zeroConf, host, gatewayInfo, dnsInfo, netProtocols, (discoveryModeSupported, discoveryMode) = 
                Async.Parallel(
                    async{
                        let! nics = dev.GetNetworkInterfaces()
                        return nics |> SuppressNull [||]
                    },
                    dev.GetNTP(),
                    facade.IsZeroConfigurationSupported(),
                    async{
                        let! zeroConfSupported = facade.IsZeroConfigurationSupported()
                        if zeroConfSupported then
                            return! dev.GetZeroConfiguration()
                        else
                            return null
                    },
                    dev.GetHostname(),
                    dev.GetNetworkDefaultGateway(),
                    dev.GetDNS(),
                    async{
                        let! protocols = dev.GetNetworkProtocols()
                        return protocols |> SuppressNull [||]
                    },
                    async{
                        try
                            let! dm = dev.GetDiscoveryMode()
                            return (true, dm)
                        with err ->
                            dbg.Error(err)
                            return (false, DiscoveryMode.nonDiscoverable)
                    }
                )

            let zeroConfIp = 
                if zeroConfSupported && NotNull(zeroConf) && NotNull(zeroConf.addresses) then
                    zeroConf.addresses |> Seq.filter (not << String.IsNullOrWhiteSpace)
                else
                    Seq.empty

            let model = new NetworkSettingsView.Model(
                zeroConfIp = String.Join("; ", zeroConfIp),
                zeroConfSupported = zeroConfSupported,
                discoveryModeSupported = discoveryModeSupported
            )
            
            model.zeroConfEnabled <- zeroConfSupported && NotNull(zeroConf) && zeroConf.enabled
            model.useHostFromDhcp <- host.fromDHCP
            model.host <- host.name
            model.netProtocols <- netProtocols
            model.discoveryMode <- discoveryMode

            if ntpInfo |> NotNull then
                model.useNtpFromDhcp <- ntpInfo.fromDHCP
                let ntp = 
                    if NotNull(ntpInfo.ntpManual) && ntpInfo.ntpManual.Length>0 then
                        ntpInfo.ntpManual
                    else
                        ntpInfo.ntpFromDHCP

                if ntp |> NotNull then
                    model.ntpServers <- String.Join("; ", seq{
                        for n in ntp do
                            let s = OdmSession.NetHostToStr(n)
                            if not(String.IsNullOrWhiteSpace(s)) then
                                yield s
                    })
            
            if NotNull(gatewayInfo) then
                let addresses = seq{
                    if NotNull(gatewayInfo.iPv4Address) then
                        for x in gatewayInfo.iPv4Address |> Seq.filter NotNull do
                            let ip = x.Trim()
                            if not(String.IsNullOrWhiteSpace(ip)) then
                                yield ip
                    if NotNull(gatewayInfo.iPv6Address) then
                        for x in gatewayInfo.iPv6Address |> Seq.filter NotNull do
                            let ip = x.Trim()
                            if not(String.IsNullOrWhiteSpace(ip)) then
                                yield ip
                }
                model.gateway <-  String.Join(";", addresses)

            if dnsInfo |> NotNull then
                model.useDnsFromDhcp <- dnsInfo.fromDHCP
                let dns = 
                    if NotNull(dnsInfo.dnsManual) && dnsInfo.dnsManual.Length>0 then
                        dnsInfo.dnsManual
                    else
                        dnsInfo.dnsFromDHCP
                if dns |> NotNull then
                    let ipaddrs = seq{
                        yield! dns |> Seq.filter NotNull |> Seq.map (fun x-> x.iPv4Address) |> Seq.filter (not << String.IsNullOrWhiteSpace)
                        yield! dns |> Seq.filter NotNull |> Seq.map (fun x-> x.iPv6Address) |> Seq.filter (not << String.IsNullOrWhiteSpace)
                    }
                    model.dns <- String.Join("; ", ipaddrs)
            
            match nics |> Seq.tryFind (fun x -> x.enabled) with
            | Some nic ->
                let nic_cfg = nic.iPv4.config
                model.dhcp <- nic.iPv4.config.dhcp
                if NotNull(nic_cfg.manual) && nic_cfg.manual.Length>0 then 
                    let ipInfo = nic_cfg.manual.[0]
                    model.ip <- ipInfo.address
                    model.subnet <- (ipInfo.prefixLength |> NetMaskHelper.CidrToIpMask).ToString()
                else 
                    if nic_cfg.fromDHCP |> NotNull then
                        model.ip <- nic_cfg.fromDHCP.address
                        model.subnet <- (nic_cfg.fromDHCP.prefixLength |> NetMaskHelper.CidrToIpMask).ToString()
            | None ->
                model.dhcp <- false
                model.ip <- "255.255.255.255"
                model.subnet <- "255.255.255.255"
                
            model.AcceptChanges()
            return model
        }

        let apply(model:NetworkSettingsView.Model) = async{
            //apply changes of network settings
            
            if not (model.isModified) then 
                //nothing has been changed
                return ()

            let dns_changed = 
                model.origin.dns <> model.current.dns ||
                model.origin.dhcp <> model.current.dhcp ||
                model.origin.useDnsFromDhcp <>  model.current.useDnsFromDhcp

            let host_changed = 
                model.origin.host <> model.current.host ||
                model.origin.useHostFromDhcp <> model.current.useHostFromDhcp

            let gateway_changed = 
                model.origin.gateway <> model.current.gateway
                            
            let ip_changed = 
                model.origin.ip <> model.current.ip ||
                model.origin.subnet <> model.current.subnet ||
                model.origin.dhcp <> model.current.dhcp
            
            let ntp_changed = 
                model.origin.ntpServers <> model.current.ntpServers ||
                model.origin.dhcp <> model.current.dhcp ||
                model.origin.useNtpFromDhcp <> model.current.useNtpFromDhcp
                
            let zero_conf_changed = 
                model.zeroConfSupported && (model.origin.zeroConfEnabled <> model.current.zeroConfEnabled)
            
            let discovery_mode_changed =
                model.discoveryModeSupported && (model.origin.discoveryMode <> model.current.discoveryMode)

            let currentNetProtocols = 
                if model.netProtocols |> NotNull then
                    model.netProtocols
                else
                    [||]
            
            let protocols_changed = 
                let PortsChanged(protocolType: NetworkProtocolType) = 
                    let GetProtocolPorts(protocols:NetworkProtocol[]) = seq{
                        if protocols |> NotNull then
                            for p in protocols do 
                                if NotNull(p.port) && p.enabled && p.name = protocolType then
                                    yield! p.port
                    }
                    let origin = GetProtocolPorts(model.origin.netProtocols) |> Seq.toArray
                    let current = GetProtocolPorts(currentNetProtocols) |> Seq.toArray
                    not(origin.All( fun x-> current.Contains(x))) || not(current.All(fun x-> origin.Contains(x)))
                PortsChanged(NetworkProtocolType.http) || PortsChanged(NetworkProtocolType.https) || PortsChanged(NetworkProtocolType.rtsp)

            if protocols_changed then
                do! async{
                    use! progress = Progress.Show(ctx, "applying protocol settings...")
                    do! session.SetNetworkProtocols(currentNetProtocols)
                }
            if ntp_changed then
                do! async{
                    use! progress = Progress.Show(ctx, LocalNetworkSettings.instance.applyindNtp)
                    let ntp_addresses = [| 
                        if model.current.ntpServers |> NotNull then
                            for x in model.current.ntpServers.Split([|';'; ' '; ','|], StringSplitOptions.RemoveEmptyEntries) do
                                if not(String.IsNullOrWhiteSpace(x)) then
                                    yield OdmSession.NetHostFromStr(x)
                        
                    |]
                    let useDhcp = model.current.dhcp && model.current.useNtpFromDhcp
                    do! session.SetNTP(useDhcp, ntp_addresses)
                }

            if dns_changed then
                do! async{
                    use! progress = Progress.Show(ctx, LocalNetworkSettings.instance.applyindDns)
                    let dns_addresses = [| 
                        if model.current.dns |> NotNull then
                            for x in model.current.dns.Split([|';'; ' '; ','|], StringSplitOptions.RemoveEmptyEntries) do
                                let x = x.Trim()
                                if not(String.IsNullOrWhiteSpace(x)) then
                                    yield new IPAddress(
                                        ``type`` = IPType.iPv4, 
                                        iPv4Address = x
                                    )
                    |]
                    let useDhcp = model.current.dhcp && model.current.useDnsFromDhcp
                    do! session.SetDNS(useDhcp, null, dns_addresses)
                }

            if gateway_changed then
                do! async{
                    use! view = Progress.Show(ctx, LocalNetworkSettings.instance.applyindGateway)
                    let ips = [
                        if model.current.gateway |> NotNull then
                            for x in model.current.gateway.Split([|';'; ' '; ','|], StringSplitOptions.RemoveEmptyEntries) do
                                let valid,ip = IPAddress.TryParse(x.Trim())
                                if not(valid) then
                                    failwith LocalNetworkSettings.instance.invalidIpForGateway
                                yield ip
                    ]
                    let ipv4_list = [
                        for x in ips do
                            if x.AddressFamily=AddressFamily.InterNetwork then
                                yield x.ToString()
                    ]
                    let ipv6_list = [
                        for x in ips do
                            if x.AddressFamily=AddressFamily.InterNetworkV6 then
                                yield x.ToString()
                    ]
                    do! session.SetNetworkDefaultGateway(ipv4_list |> List.toArray, ipv6_list |> List.toArray)
                }

            if zero_conf_changed then
                do! async{
                    use! view = Progress.Show(ctx, LocalNetworkSettings.instance.applyindZeroConf)
                    let! zeroConf = dev.GetZeroConfiguration()
                    do! dev.SetZeroConfiguration(zeroConf.interfaceToken ,model.zeroConfEnabled)
                }

            if host_changed then
                do! async{
                    use! view = Progress.Show(ctx, LocalNetworkSettings.instance.applyindHostName)
                    if model.useHostFromDhcp then
                        do! dev.SetHostname("")
                    else
                        do! dev.SetHostname(model.host |> SuppressNull "")
                }

            if discovery_mode_changed then
                do! async{
                    use! progress = Progress.Show(ctx, "setting discovery mode...")
                    do! dev.SetDiscoveryMode(model.discoveryMode)
                }

            if ip_changed then
                let! rebootIsNeeded = 
                    async{
                        use! progress = Progress.Show(ctx, LocalNetworkSettings.instance.applyindIp)
                        
                        let! nics = session.GetNetworkInterfaces()
                        let nic = nics |> Seq.find(fun x -> x.enabled)
                        
                        let nic_set = new NetworkInterfaceSetConfiguration()
                        nic_set.enabled <- true
                        nic_set.enabledSpecified <- true
                        nic_set.mtuSpecified <- NotNull(nic.info) && nic.info.mtuSpecified
                        if nic_set.mtuSpecified then
                            nic_set.mtu <- nic.info.mtu

                        nic_set.iPv4 <- new IPv4NetworkInterfaceSetConfiguration()
                        nic_set.iPv4.dhcp <- model.dhcp
                        nic_set.iPv4.dhcpSpecified <- true
                        nic_set.iPv4.enabled <- true
                        nic_set.iPv4.enabledSpecified <- true
                        if not( model.dhcp) then
                            nic_set.iPv4.manual <- 
                                let ip = model.current.ip
                                if String.IsNullOrWhiteSpace(ip) then
                                    [||]
                                else [|
                                    new PrefixedIPv4Address(
                                        address = model.current.ip.ToString(),
                                        prefixLength = (model.current.subnet |> IPAddress.Parse |> NetMaskHelper.IpToCidrMask)
                                    )
                                |]
                        
        //                if nic.Link <> null then
        //                    //nic_set_cfg.Link = new NetworkInterfaceConnectionSetting()
        //                    if nic.Link.AdminSettings <> null then
        //                        nic_set.Link <- nic.Link.AdminSettings
        //                    else if nic.Link.OperSettings <> null then
        //                        nic_set.Link <- nic.Link.OperSettings

                        return! session.SetNetworkInterfaces(nic.token, nic_set)
                    }
                // according to 5.13 we should send explicit reboot
                if rebootIsNeeded then
                    do! async{
                        use! progress = Progress.Show(ctx, LocalMaintenance.instance.rebooting)
                        do! session.SystemReboot() |> Async.Ignore
                    }
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
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = NetworkSettingsView.Show(ctx, model)
                    return res.Handle(
                        apply = (fun model->this.ApplyChanges(model)),
                        validationFailed = (fun model err -> this.ValidationFailed(model, err)),
                        close = (fun ()->this.Complete())
                    )
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.ValidationFailed(model, err) = async{
            do! show_error(err)
            return! this.ShowForm(model)
        }

        member private this.ApplyChanges(model) = async{
            try
                use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                do! apply(model)
            with err -> 
                dbg.Error(err)
                do! show_error(err)

            return! this.Main()
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx) = 
            let act = new NetworkSettingsActivity(ctx)
            act.Main()
        
    end
