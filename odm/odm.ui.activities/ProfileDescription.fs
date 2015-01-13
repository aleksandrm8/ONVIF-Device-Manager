module odm.ui.activities.ProfileDescription
    open System
    open System.Linq
    open System.Collections.ObjectModel
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    open System.Collections.Generic
    //open System.Disposables
    
    //open Microsoft.Practices.Unity
    open onvif.services
    open onvif.utils

    open odm.core
    open odm.infra
    open utils
    //open utils.extensions
    //open odm.models
    open utils.fsharp

    ///<summary></summary>
    let CreateProp(name:string, value:obj, childs:ItemSelectorView.ItemProp[]) = 
        let valStr = value |> IfNotNull (fun x->x.ToString())
        new ItemSelectorView.ItemProp(name, valStr, childs)
    
    ///<summary></summary>
    let CreateSimpleProp(name, value:obj) = 
        CreateProp(name, value, null)

    ///<summary></summary>
    let GetMulticastDetails(mc:MulticastConfiguration) = Seq.toList(seq{
            if mc.address |> NotNull then
                if mc.address.``type`` = IPType.iPv4 then
                    yield CreateProp("ip address", mc.address.iPv4Address, null)
                else
                    yield CreateProp("ip address", mc.address.iPv6Address, null)
            yield CreateProp("port", mc.port, null)
            yield CreateProp("TTL", mc.ttl, null)
            yield CreateProp("auto start", mc.autoStart, null)
    })

    ///<summary></summary>
    let GetPtzDetails(ptz:PTZConfiguration, nodes:seq<PTZNode>) = Seq.toList(seq{
        yield CreateProp("name", ptz.name, null)
        yield CreateProp("token", ptz.token, null)
        yield CreateProp("use count", ptz.useCount, null)
        
        if NotNull(ptz.panTiltLimits) && NotNull(ptz.panTiltLimits.range) && NotNull(ptz.panTiltLimits.range.xRange) then
            yield CreateProp("pan tilt limits", ptz.panTiltLimits.range.xRange, null)
        if NotNull(ptz.zoomLimits) && NotNull(ptz.zoomLimits.range) && NotNull(ptz.zoomLimits.range.xRange) then
            yield CreateProp("zoom limits", ptz.zoomLimits.range.xRange, null)
        
        yield CreateProp("default ptz speed", ptz.defaultPTZSpeed, null)
        yield CreateProp("default ptz timeout", ptz.defaultPTZTimeout, null)
        yield CreateProp("default absolute pant tilt position space", ptz.defaultAbsolutePantTiltPositionSpace, null)
        yield CreateProp("default absolute zoom position space", ptz.defaultAbsoluteZoomPositionSpace, null)
        yield CreateProp("default continuous pant tilt velocity space", ptz.defaultContinuousPanTiltVelocitySpace, null)
        yield CreateProp("default continuous zoom velocity space", ptz.defaultContinuousZoomVelocitySpace, null)
        yield CreateProp("default relative pant tilt translation space", ptz.defaultRelativePanTiltTranslationSpace, null)
        yield CreateProp("default relative zoom translation space", ptz.defaultRelativeZoomTranslationSpace, null)
        
        let node = 
            if nodes |> NotNull then
                nodes |> Seq.tryFind(fun x->x.token = ptz.nodeToken)
            else
                None
        match node with
        |Some node->
            let childs = Seq.toList(seq{
                yield CreateProp("name", node.name, null)
                yield CreateProp("token", node.token, null)
                yield CreateProp("home supported", node.homeSupported, null)
                yield CreateProp("maximum number of presets", node.maximumNumberOfPresets, null)
                yield CreateProp("auxiliary commands", String.Join("; ", node.auxiliaryCommands |> SuppressNull [||]), null)
                yield CreateProp("supported ptz spaces", node.supportedPTZSpaces, null)
            })
            yield CreateProp("Node", node.name, childs |> List.toArray)
        |None ->
            yield CreateProp("node token", ptz.nodeToken, null)
    })

    ///<summary></summary>
    let GetVideoSourceDetails(vsrc:VideoSource) = Seq.toList(seq{
        yield CreateProp("token", vsrc.token, null)
        yield CreateProp("framerate", vsrc.framerate, null)
        yield CreateProp("resolution", vsrc.resolution, null)
        if vsrc.imaging |> NotNull then
            let img = vsrc.imaging
            let childs = Seq.toList(seq{
                if img.brightnessSpecified then
                    yield CreateProp("brightness", img.brightness, null)
                if img.colorSaturationSpecified then
                    yield CreateProp("color saturation", img.colorSaturation, null)
                if img.contrastSpecified then
                    yield CreateProp("contrast", img.contrast, null)
                if img.sharpnessSpecified then
                    yield CreateProp("sharpness", img.sharpness, null)
                if img.exposure |> NotNull then
                    yield CreateProp("exposure", img.exposure, null)
                if img.focus |> NotNull then
                    yield CreateProp("focus", img.focus, null)
                if img.irCutFilterSpecified then
                    yield CreateProp("IrCut filter", img.irCutFilter, null)
                if img.backlightCompensation |> NotNull then
                    let level = img.backlightCompensation.level
                    let mode = img.backlightCompensation.mode
                    let value = sprintf "level=%g, mode=%A" level mode
                    yield CreateProp("backlight compensation", value, null)
                if img.whiteBalance |> NotNull then
                    let cbGain = img.whiteBalance.cbGain
                    let crGain = img.whiteBalance.crGain
                    let mode = img.whiteBalance.mode
                    let value = sprintf "CbGain=%g, CrGain=%g, mode=%A" cbGain crGain mode
                    yield CreateProp("white balance", value, null)
                if img.wideDynamicRange |> NotNull then
                    let level = img.wideDynamicRange.level
                    let mode = img.whiteBalance.mode
                    let value = sprintf "level=%g, mode=%A" level mode
                    yield CreateProp("wide dynamic range", value, null)
            })
            yield CreateProp("Imaging", null, childs |> List.toArray)
    })

    ///<summary></summary>
    let GetAudioSourceDetails(asrc:AudioSource) = Seq.toList(seq{
        yield CreateProp("token", asrc.token, null)
        yield CreateProp("channels", asrc.channels, null)
    })
        
    ///<summary></summary>
    let GetVscDetails(vsc:VideoSourceConfiguration, videoSrcs:seq<VideoSource>) = Seq.toList(seq{
        yield CreateProp("name", vsc.name, null)
        yield CreateProp("token", vsc.token, null)
        yield CreateProp("use count", vsc.useCount, null)
        yield CreateProp("bounds", vsc.bounds.ToString(), null)
        let vs = 
            if videoSrcs |> NotNull then
                videoSrcs |> Seq.tryFind(fun vs->vs.token = vsc.sourceToken)
            else
                None
        match vs with
        | Some vs ->
            let childs = GetVideoSourceDetails(vs)
            yield  CreateProp("Video Source", vs.token, childs |> List.toArray)
        | None ->
            yield CreateProp("video source token", vsc.sourceToken, null)
    })

    ///<summary></summary>
    let GetAscDetails(asc:AudioSourceConfiguration, audioSrcs:seq<AudioSource>) = Seq.toList(seq{
        yield CreateProp("name", asc.name, null)
        yield CreateProp("token", asc.token, null)
        yield CreateProp("use count", asc.useCount, null)
        let audioSrc = 
            if audioSrcs |> NotNull then
                audioSrcs |> Seq.tryFind(fun x->x.token = asc.sourceToken)
            else
                None
        match audioSrc with
        | Some x ->
            let childs = GetAudioSourceDetails(x)
            yield  CreateProp("Audio Source", x.token, childs |> List.toArray)
        | None ->
            yield CreateProp("audio source token", asc.sourceToken, null)
    })

    ///<summary></summary>
    let GetVecDetails(vec:VideoEncoderConfiguration) = Seq.toList(seq{
        yield CreateProp("name", vec.name, null)
        yield CreateProp("token", vec.token, null)
        yield CreateProp("use count", vec.useCount, null)
        yield CreateProp("encoding", vec.encoding, null)
        yield CreateProp("resolution", vec.resolution, null)
        yield CreateProp("session timeout", vec.sessionTimeout, null)
        yield CreateProp("quality", vec.quality, null)
        if vec.rateControl |> NotNull then
            yield CreateProp("frame rate", vec.rateControl.frameRateLimit, null)
            yield CreateProp("bitrate", vec.rateControl.bitrateLimit, null)
            yield CreateProp("encoding interval", vec.rateControl.encodingInterval, null)
    })

    ///<summary></summary>
    let GetAecDetails(aec:AudioEncoderConfiguration) = Seq.toList(seq{
        yield CreateProp("name", aec.name, null)
        yield CreateProp("token", aec.token, null)
        yield CreateProp("use count", aec.useCount, null)
        yield CreateProp("encoding", aec.encoding, null)
        yield CreateProp("bitrate", aec.bitrate, null)
        yield CreateProp("sample rate", aec.sampleRate, null)
        yield CreateProp("session timeout", aec.sessionTimeout, null)
        if aec.multicast |> NotNull then
            let mc = aec.multicast
            let childs = GetMulticastDetails(mc)
            yield CreateProp("multicast", null, childs |> List.toArray)
    })

    ///<summary></summary>
    let GetVacDetails(vac:VideoAnalyticsConfiguration) = Seq.toList(seq{
        yield CreateProp("name", vac.name, null)
        yield CreateProp("token", vac.token, null)
        yield CreateProp("use count", vac.useCount, null)
        if vac.analyticsEngineConfiguration |> NotNull then
            let anEng = vac.analyticsEngineConfiguration
            if anEng.analyticsModule |> NotNull then
                for m in anEng.analyticsModule do
                    yield CreateProp("name", m.name, null)
        if vac.ruleEngineConfiguration |> NotNull then
            let ruleEng = vac.ruleEngineConfiguration
            if ruleEng.rule |> NotNull then
                for r in ruleEng.rule do
                    yield CreateProp("name", r.name, null)
    })

    ///<summary></summary>
    let GetMetaDetails(meta:MetadataConfiguration) = Seq.toList(seq{
        yield CreateProp("name", meta.name, null)
        yield CreateProp("token", meta.token, null)
        yield CreateProp("use count", meta.useCount, null)
        yield CreateProp("session timeout", meta.sessionTimeout, null)
        if meta.analyticsSpecified then
            yield CreateProp("analytics", meta.analytics, null)
        
        if meta.ptzStatus |> NotNull then
            let ptzStatus = meta.ptzStatus
            yield CreateProp("ptz position", ptzStatus.position, null)
            yield CreateProp("ptz status", ptzStatus.status, null)
        
        if meta.events |> NotNull then
            let events = meta.events
            yield CreateProp("events filter", events.filter, null)
            yield CreateProp("events subscription policy", events.subscriptionPolicy, null)
                
        if meta.multicast |> NotNull then
            let mc = meta.multicast
            let childs = GetMulticastDetails(mc)
            yield CreateProp("multicast", null, childs |> List.toArray)
            
    })

    ///<summary></summary>
    let GetProfileDetails(profile:Profile, videoSources:seq<VideoSource>, audioSources:seq<AudioSource>, ptzNodes:seq<PTZNode>) = Seq.toList(seq{
        yield CreateProp("name", profile.name, null)
        yield CreateProp("token", profile.token, null)
        if profile.fixedSpecified then
            yield CreateProp("fixed", profile.``fixed``, null)

        if profile.videoSourceConfiguration |> NotNull then
            let vsc = profile.videoSourceConfiguration
            let childs = GetVscDetails(vsc, videoSources)
            let displayName = 
                if not(String.IsNullOrWhiteSpace(vsc.name)) then
                    vsc.name
                else
                    vsc.token
            yield  CreateProp("Video Source Configuration", displayName, childs |> List.toArray)

        if profile.audioSourceConfiguration |> NotNull then
            let asc = profile.audioSourceConfiguration
            let childs = GetAscDetails(asc, audioSources)
            let displayName = 
                if not(String.IsNullOrWhiteSpace(asc.name)) then
                    asc.name
                else
                    asc.token
            yield  CreateProp("Audio Source Configuration", displayName, childs |> List.toArray)

        if profile.videoEncoderConfiguration |> NotNull then
            let vec = profile.videoEncoderConfiguration
            let childs = GetVecDetails(vec)
            yield CreateProp("Video Encoder Configuration", vec.GetName(), childs |> List.toArray)

        if profile.audioEncoderConfiguration |> NotNull then
            let aec = profile.audioEncoderConfiguration
            let childs = GetAecDetails(aec)
            yield CreateProp("Audio Encoder Configuration", aec.GetName(), childs |> List.toArray)

        if profile.ptzConfiguration |> NotNull then
            let ptz = profile.ptzConfiguration
            let childs = GetPtzDetails(ptz, ptzNodes)
            yield  CreateProp("PTZ Configuration", ptz.GetName(), childs |> List.toArray)

        if profile.videoAnalyticsConfiguration |> NotNull then
            let vac = profile.videoAnalyticsConfiguration
            let childs = GetVacDetails(vac)
            yield  CreateProp("Video Analytics Configuration", vac.GetName(), childs |> List.toArray)

        if profile.metadataConfiguration |> NotNull then
            let meta = profile.metadataConfiguration
            let childs = GetMetaDetails(meta)
            yield  CreateProp("Metadata Configuration", meta.GetName(), childs |> List.toArray)
    })

    ///<summary></summary>
    let GetReceiverDetails(receiver:Receiver) = Seq.toList(seq{
        if receiver |> NotNull then
            yield CreateProp("token", receiver.token, null)
            if receiver.configuration |> NotNull then
                let cfg = receiver.configuration
                yield CreateProp("uri", cfg.mediaUri, null)
                yield CreateProp("mode", cfg.mode, null)
                if cfg.streamSetup |> NotNull then
                    let streamSetup = cfg.streamSetup
                    yield CreateProp("stream type", streamSetup.stream, null)
                    yield CreateProp("transport", streamSetup.transport, null)
    })

    let GetNvaInputDetails(input:AnalyticsEngineInput) = Seq.toList(seq{
        if input |> NotNull then
            yield CreateProp("name", input.name, null)
            yield CreateProp("token", input.token, null)
            yield CreateProp("use count", input.useCount, null)
            if input.sourceIdentification |> NotNull then
                let src_id = input.sourceIdentification
                yield CreateProp("source name", src_id.name, null)
                yield CreateProp("source tokens", String.Join(", ", src_id.token), null)
                
            if input.videoInput |> NotNull then
                let videoInput = input.videoInput
                let videoInputName = 
                    if not <| String.IsNullOrWhiteSpace(videoInput.name) then
                        videoInput.name
                    elif videoInput.token |> IsNull then
                        "<null>"
                    elif String.IsNullOrWhiteSpace(videoInput.token) then
                        "<empty>"
                    else
                        videoInput.token

                yield CreateProp("video input", videoInputName, GetVecDetails(videoInput) |> List.toArray)
    })