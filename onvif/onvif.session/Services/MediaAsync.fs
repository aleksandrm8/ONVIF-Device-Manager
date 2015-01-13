namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.ServiceModel

    open onvif.services
    open onvif10_media
    open onvif

    open utils.fsharp

    [<AllowNullLiteral>]
    type IMediaAsync= interface
        //onvif 1.2
        abstract GetVideoSources: unit->Async<VideoSource[]>
        abstract GetAudioSources: unit->Async<AudioSource[]>
        abstract CreateProfile: name:String*token:string->Async<Profile>
        abstract GetProfiles: unit -> Async<Profile[]>
        abstract GetProfile: profileToken:string -> Async<Profile>
        abstract AddVideoEncoderConfiguration: profileToken:string*cofigurationToken:string -> Async<unit>
        abstract RemoveVideoEncoderConfiguration: profToken:string -> Async<unit>
        abstract AddVideoSourceConfiguration: profToken:string*cofigToken:string -> Async<unit>
        abstract RemoveVideoSourceConfiguration: profToken:string -> Async<unit>
        abstract AddAudioEncoderConfiguration: profToken:string*cofigToken:string -> Async<unit>
        abstract RemoveAudioEncoderConfiguration: profToken:string -> Async<unit>
        abstract AddAudioSourceConfiguration: profToken:string*cofigToken:string -> Async<unit>
        abstract RemoveAudioSourceConfiguration: profToken:string -> Async<unit>
        abstract AddPTZConfiguration: profToken:string*cofigToken:string -> Async<unit>
        abstract RemovePTZConfiguration: profToken:string -> Async<unit>
        abstract AddVideoAnalyticsConfiguration: profToken:string*cofigToken:string -> Async<unit>
        abstract RemoveVideoAnalyticsConfiguration: profToken:string -> Async<unit>
        abstract AddMetadataConfiguration: profToken:string*cofigToken:string -> Async<unit>
        abstract RemoveMetadataConfiguration: profToken:string -> Async<unit>
        abstract DeleteProfile: token:string -> Async<unit>
        abstract GetVideoSourceConfigurations: unit -> Async<VideoSourceConfiguration[]>
        abstract GetVideoEncoderConfigurations: unit -> Async<VideoEncoderConfiguration[]>
        abstract GetAudioSourceConfigurations: unit -> Async<AudioSourceConfiguration[]>
        abstract GetAudioEncoderConfigurations: unit -> Async<AudioEncoderConfiguration[]>
        abstract GetVideoAnalyticsConfigurations: unit -> Async<VideoAnalyticsConfiguration[]>
        abstract GetMetadataConfigurations: unit -> Async<MetadataConfiguration[]>
        abstract GetVideoSourceConfiguration: token:string -> Async<VideoSourceConfiguration>
        abstract GetVideoEncoderConfiguration: token:string -> Async<VideoEncoderConfiguration>
        abstract GetAudioSourceConfiguration: token:string -> Async<AudioSourceConfiguration>
        abstract GetAudioEncoderConfiguration: token:string -> Async<AudioEncoderConfiguration>
        abstract GetVideoAnalyticsConfiguration: token:string -> Async<VideoAnalyticsConfiguration>
        abstract GetMetadataConfiguration: token:string -> Async<MetadataConfiguration>
        abstract GetCompatibleVideoEncoderConfigurations: profToken:string -> Async<VideoEncoderConfiguration[]>
        abstract GetCompatibleVideoSourceConfigurations: profToken:string -> Async<VideoSourceConfiguration[]>
        abstract GetCompatibleAudioEncoderConfigurations: profToken:string -> Async<AudioEncoderConfiguration[]>
        abstract GetCompatibleAudioSourceConfigurations: profToken:string -> Async<AudioSourceConfiguration[]>
        abstract GetCompatibleVideoAnalyticsConfigurations: profToken:string -> Async<VideoAnalyticsConfiguration[]>
        abstract GetCompatibleMetadataConfigurations: profToken:string -> Async<MetadataConfiguration[]>
        abstract SetVideoSourceConfiguration: config:VideoSourceConfiguration*forcePersistence:bool -> Async<unit>
        abstract SetVideoEncoderConfiguration: config:VideoEncoderConfiguration*forcePersistence:bool -> Async<unit>
        abstract SetAudioSourceConfiguration: config:AudioSourceConfiguration*forcePersistence:bool -> Async<unit>
        abstract SetAudioEncoderConfiguration: config:AudioEncoderConfiguration*forcePersistence:bool -> Async<unit>
        abstract SetVideoAnalyticsConfiguration: config:VideoAnalyticsConfiguration*forcePersistence:bool -> Async<unit>
        abstract SetMetadataConfiguration: config:MetadataConfiguration*forcePersistence:bool -> Async<unit>
        abstract GetVideoSourceConfigurationOptions: configToken:string*profToken:string -> Async<VideoSourceConfigurationOptions>
        abstract GetVideoEncoderConfigurationOptions: configToken:string*profToken:string -> Async<VideoEncoderConfigurationOptions>
        abstract GetAudioSourceConfigurationOptions: configToken:string*profToken:string -> Async<AudioSourceConfigurationOptions>
        abstract GetAudioEncoderConfigurationOptions: configToken:string*profToken:string -> Async<AudioEncoderConfigurationOptions>
        abstract GetMetadataConfigurationOptions: configToken:string*profToken:string -> Async<MetadataConfigurationOptions>
        abstract GetGuaranteedNumberOfVideoEncoderInstances: configToken:string -> Async<GetGuaranteedNumberOfVideoEncoderInstancesResponse>
        abstract GetStreamUri: streamSetup:StreamSetup*profToken:string->Async<MediaUri>
        abstract StartMulticastStreaming: profToken:string -> Async<unit>
        abstract StopMulticastStreaming: profToken:string -> Async<unit>
        abstract SetSynchronizationPoint: profToken:string -> Async<unit>
        abstract GetSnapshotUri: profToken:string->Async<MediaUri>
        
        
        //onvif 2.1
        abstract GetServiceCapabilities: unit -> Async<MediaServiceCapabilities>
//        abstract GetAudioOutputs: request:GetAudioOutputsRequest -> Async<GetAudioOutputsResponse>
//        abstract AddAudioOutputConfiguration: request:AddAudioOutputConfigurationRequest -> Async<AddAudioOutputConfigurationResponse>
//        abstract RemoveAudioOutputConfiguration: request:RemoveAudioOutputConfigurationRequest -> Async<RemoveAudioOutputConfigurationResponse>
//        abstract AddAudioDecoderConfiguration: request:AddAudioDecoderConfigurationRequest -> Async<AddAudioDecoderConfigurationResponse>
//        abstract RemoveAudioDecoderConfiguration: request:RemoveAudioDecoderConfigurationRequest -> Async<RemoveAudioDecoderConfigurationResponse>
//        abstract GetAudioOutputConfigurations: request:GetAudioOutputConfigurationsRequest -> Async<GetAudioOutputConfigurationsResponse>
//        abstract GetAudioDecoderConfigurations: request:GetAudioDecoderConfigurationsRequest -> Async<GetAudioDecoderConfigurationsResponse>
//        abstract GetAudioOutputConfiguration: request:GetAudioOutputConfigurationRequest -> Async<GetAudioOutputConfigurationResponse>
//        abstract GetAudioDecoderConfiguration: request:GetAudioDecoderConfigurationRequest -> Async<GetAudioDecoderConfigurationResponse>
//        abstract GetCompatibleAudioOutputConfigurations: request:GetCompatibleAudioOutputConfigurationsRequest -> Async<GetCompatibleAudioOutputConfigurationsResponse>
//        abstract GetCompatibleAudioDecoderConfigurations: request:GetCompatibleAudioDecoderConfigurationsRequest -> Async<GetCompatibleAudioDecoderConfigurationsResponse>
//        abstract SetAudioOutputConfiguration: request:SetAudioOutputConfigurationRequest -> Async<SetAudioOutputConfigurationResponse>
//        abstract SetAudioDecoderConfiguration: request:SetAudioDecoderConfigurationRequest -> Async<SetAudioDecoderConfigurationResponse>
//        abstract GetAudioOutputConfigurationOptions: request:GetAudioOutputConfigurationOptionsRequest -> Async<GetAudioOutputConfigurationOptionsResponse>
//        abstract GetAudioDecoderConfigurationOptions: request:GetAudioDecoderConfigurationOptionsRequest -> Async<GetAudioDecoderConfigurationOptionsResponse>
    end

    type MediaAsync(proxy:Media) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy

        interface IMediaAsync with
            //onvif 1.2
            member this.GetVideoSources() = async{
                let request = new GetVideoSourcesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoSources, proxy.EndGetVideoSources)
                return response.VideoSources
            }

            member this.GetAudioSources() = async{
                let request = new GetAudioSourcesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioSources, proxy.EndGetAudioSources)
                return response.AudioSources
            }

            member this.CreateProfile(name:String, token:string) = async{
                let request = new CreateProfileRequest()
                request.Name <- name
                request.Token <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateProfile, proxy.EndCreateProfile)
                return response.Profile
            }

            member this.GetProfiles() = async{
                let request = new GetProfilesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetProfiles, proxy.EndGetProfiles)
                return response.Profiles
            }
       
            member this.GetProfile(token:string) = async{
                let request = new GetProfileRequest()
                request.ProfileToken <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginGetProfile, proxy.EndGetProfile)
                return response.Profile
            }

            member this.DeleteProfile(token:string) = async{
                let request = new DeleteProfileRequest()
                request.ProfileToken <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteProfile, proxy.EndDeleteProfile)
                return ()
            }

            member this.GetStreamUri(streamSetup:StreamSetup, profToken:string) = async{
                let request = new GetStreamUriRequest()
                request.ProfileToken <- profToken
                request.StreamSetup <- streamSetup
                let! response = Async.FromBeginEnd(request, proxy.BeginGetStreamUri, proxy.EndGetStreamUri)
                return response.MediaUri
            }

            member this.GetSnapshotUri(profToken:string) = async{
                let request = new GetSnapshotUriRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSnapshotUri, proxy.EndGetSnapshotUri)
                return response.MediaUri
            }

            member this.AddVideoEncoderConfiguration(profToken:string, cofigToken:string): Async<unit> = async{
                let request = new AddVideoEncoderConfigurationRequest()
                request.ProfileToken <- profToken
                request.ConfigurationToken <- cofigToken
                let! response = Async.FromBeginEnd(request, proxy.BeginAddVideoEncoderConfiguration, proxy.EndAddVideoEncoderConfiguration)
                return ()
            }

            member this.RemoveVideoEncoderConfiguration(profToken:string): Async<unit> = async{
                let request = new RemoveVideoEncoderConfigurationRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveVideoEncoderConfiguration, proxy.EndRemoveVideoEncoderConfiguration)
                return ()
            }

            member this.AddVideoSourceConfiguration(profToken:string, cofigToken:string): Async<unit> = async{
                let request = new AddVideoSourceConfigurationRequest()
                request.ProfileToken <- profToken
                request.ConfigurationToken <- cofigToken
                let! response = Async.FromBeginEnd(request, proxy.BeginAddVideoSourceConfiguration, proxy.EndAddVideoSourceConfiguration)
                return ()
            }

            member this.RemoveVideoSourceConfiguration(profToken:string): Async<unit> = async{
                let request = new RemoveVideoSourceConfigurationRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveVideoSourceConfiguration, proxy.EndRemoveVideoSourceConfiguration)
                return ()
            }

            member this.AddAudioEncoderConfiguration(profToken:string, cofigToken:string): Async<unit> = async{
                let request = new AddAudioEncoderConfigurationRequest()
                request.ProfileToken <- profToken
                request.ConfigurationToken <- cofigToken
                let! response = Async.FromBeginEnd(request, proxy.BeginAddAudioEncoderConfiguration, proxy.EndAddAudioEncoderConfiguration)
                return ()
            }

            member this.RemoveAudioEncoderConfiguration(profToken:string): Async<unit> = async{
                let request = new RemoveAudioEncoderConfigurationRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveAudioEncoderConfiguration, proxy.EndRemoveAudioEncoderConfiguration)
                return ()
            }

            member this.AddAudioSourceConfiguration(profToken:string, cofigToken:string): Async<unit> = async{
                let request = new AddAudioSourceConfigurationRequest()
                request.ProfileToken <- profToken
                request.ConfigurationToken <- cofigToken
                let! response = Async.FromBeginEnd(request, proxy.BeginAddAudioSourceConfiguration, proxy.EndAddAudioSourceConfiguration)
                return ()
            }

            member this.RemoveAudioSourceConfiguration(profToken:string): Async<unit> = async{
                let request = new RemoveAudioSourceConfigurationRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveAudioSourceConfiguration, proxy.EndRemoveAudioSourceConfiguration)
                return ()
            }

            member this.AddPTZConfiguration(profToken:string, cofigToken:string): Async<unit> = async{
                let request = new AddPTZConfigurationRequest()
                request.ProfileToken <- profToken
                request.ConfigurationToken <- cofigToken
                let! response = Async.FromBeginEnd(request, proxy.BeginAddPTZConfiguration, proxy.EndAddPTZConfiguration)
                return ()
            }

            member this.RemovePTZConfiguration(profToken:string): Async<unit> = async{
                if profToken |> IsNull then raise (new ArgumentNullException("profToken"))
                let request = new RemovePTZConfigurationRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginRemovePTZConfiguration, proxy.EndRemovePTZConfiguration)
                return ()
            }

            member this.AddVideoAnalyticsConfiguration(profToken:string, cofigToken:string): Async<unit> = async{
                let request = new AddVideoAnalyticsConfigurationRequest()
                request.ProfileToken <- profToken
                request.ConfigurationToken <- cofigToken
                let! response = Async.FromBeginEnd(request, proxy.BeginAddVideoAnalyticsConfiguration, proxy.EndAddVideoAnalyticsConfiguration)
                return ()
            }

            member this.RemoveVideoAnalyticsConfiguration(profToken:string): Async<unit> = async{
                let request = new RemoveVideoAnalyticsConfigurationRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveVideoAnalyticsConfiguration, proxy.EndRemoveVideoAnalyticsConfiguration)
                return ()
            }

            member this.AddMetadataConfiguration(profToken:string, cofigToken:string): Async<unit> = async{
                let request = new AddMetadataConfigurationRequest()
                request.ProfileToken <- profToken
                request.ConfigurationToken <- cofigToken
                let! response = Async.FromBeginEnd(request, proxy.BeginAddMetadataConfiguration, proxy.EndAddMetadataConfiguration)
                return ()
            }

            member this.RemoveMetadataConfiguration(profToken:string): Async<unit> = async{
                let request = new RemoveMetadataConfigurationRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveMetadataConfiguration, proxy.EndRemoveMetadataConfiguration)
                return ()
            }

            member this.GetVideoSourceConfigurations(): Async<VideoSourceConfiguration[]> = async{
                let request = new GetVideoSourceConfigurationsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoSourceConfigurations, proxy.EndGetVideoSourceConfigurations)
                return response.Configurations
            }

            member this.GetVideoEncoderConfigurations(): Async<VideoEncoderConfiguration[]> = async{
                let request = new GetVideoEncoderConfigurationsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoEncoderConfigurations, proxy.EndGetVideoEncoderConfigurations)
                return response.Configurations
            }

            member this.GetAudioSourceConfigurations(): Async<AudioSourceConfiguration[]> = async{
                let request = new GetAudioSourceConfigurationsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioSourceConfigurations, proxy.EndGetAudioSourceConfigurations)
                return response.Configurations
            }

            member this.GetAudioEncoderConfigurations(): Async<AudioEncoderConfiguration[]> = async{
                let request = new GetAudioEncoderConfigurationsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioEncoderConfigurations, proxy.EndGetAudioEncoderConfigurations)
                return response.Configurations
            }

            member this.GetVideoAnalyticsConfigurations(): Async<VideoAnalyticsConfiguration[]> = async{
                let request = new GetVideoAnalyticsConfigurationsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoAnalyticsConfigurations, proxy.EndGetVideoAnalyticsConfigurations)
                return response.Configurations
            }

            member this.GetMetadataConfigurations(): Async<MetadataConfiguration[]> = async{
                let request = new GetMetadataConfigurationsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetMetadataConfigurations, proxy.EndGetMetadataConfigurations)
                return response.Configurations
            }

            member this.GetVideoSourceConfiguration(token:string): Async<VideoSourceConfiguration> = async{
                let request = new GetVideoSourceConfigurationRequest()
                request.ConfigurationToken <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoSourceConfiguration, proxy.EndGetVideoSourceConfiguration)
                return response.Configuration
            }

            member this.GetVideoEncoderConfiguration(token:string): Async<VideoEncoderConfiguration> = async{
                let request = new GetVideoEncoderConfigurationRequest()
                request.ConfigurationToken <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoEncoderConfiguration, proxy.EndGetVideoEncoderConfiguration)
                return response.Configuration
            }

            member this.GetAudioSourceConfiguration(token:string): Async<AudioSourceConfiguration> = async{
                let request = new GetAudioSourceConfigurationRequest()
                request.ConfigurationToken <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioSourceConfiguration, proxy.EndGetAudioSourceConfiguration)
                return response.Configuration
            }

            member this.GetAudioEncoderConfiguration(token:string): Async<AudioEncoderConfiguration> = async{
                let request = new GetAudioEncoderConfigurationRequest()
                request.ConfigurationToken <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioEncoderConfiguration, proxy.EndGetAudioEncoderConfiguration)
                return response.Configuration
            }

            member this.GetVideoAnalyticsConfiguration(token:string): Async<VideoAnalyticsConfiguration> = async{
                let request = new GetVideoAnalyticsConfigurationRequest()
                request.ConfigurationToken <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoAnalyticsConfiguration, proxy.EndGetVideoAnalyticsConfiguration)
                return response.Configuration
            }

            member this.GetMetadataConfiguration(token:string): Async<MetadataConfiguration> = async{
                let request = new GetMetadataConfigurationRequest()
                request.ConfigurationToken <- token
                let! response = Async.FromBeginEnd(request, proxy.BeginGetMetadataConfiguration, proxy.EndGetMetadataConfiguration)
                return response.Configuration
            }

            member this.GetCompatibleVideoEncoderConfigurations(profToken:string): Async<VideoEncoderConfiguration[]> = async{
                let request = new GetCompatibleVideoEncoderConfigurationsRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCompatibleVideoEncoderConfigurations, proxy.EndGetCompatibleVideoEncoderConfigurations)
                return response.Configurations
            }

            member this.GetCompatibleVideoSourceConfigurations(profToken:string): Async<VideoSourceConfiguration[]> = async{
                let request = new GetCompatibleVideoSourceConfigurationsRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCompatibleVideoSourceConfigurations, proxy.EndGetCompatibleVideoSourceConfigurations)
                return response.Configurations
            }

            member this.GetCompatibleAudioEncoderConfigurations(profToken:string): Async<AudioEncoderConfiguration[]> = async{
                let request = new GetCompatibleAudioEncoderConfigurationsRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCompatibleAudioEncoderConfigurations, proxy.EndGetCompatibleAudioEncoderConfigurations)
                return response.Configurations
            }

            member this.GetCompatibleAudioSourceConfigurations(profToken:string): Async<AudioSourceConfiguration[]> = async{
                let request = new GetCompatibleAudioSourceConfigurationsRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCompatibleAudioSourceConfigurations, proxy.EndGetCompatibleAudioSourceConfigurations)
                return response.Configurations
            }

            member this.GetCompatibleVideoAnalyticsConfigurations(profToken:string): Async<VideoAnalyticsConfiguration[]> = async{
                let request = new GetCompatibleVideoAnalyticsConfigurationsRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCompatibleVideoAnalyticsConfigurations, proxy.EndGetCompatibleVideoAnalyticsConfigurations)
                return response.Configurations
            }

            member this.GetCompatibleMetadataConfigurations(profToken:string): Async<MetadataConfiguration[]> = async{
                let request = new GetCompatibleMetadataConfigurationsRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetCompatibleMetadataConfigurations, proxy.EndGetCompatibleMetadataConfigurations)
                return response.Configurations
            }

            member this.SetVideoSourceConfiguration(config:VideoSourceConfiguration, forcePersistence:bool): Async<unit> = async{
                let request = new SetVideoSourceConfigurationRequest()
                request.Configuration <- config
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetVideoSourceConfiguration, proxy.EndSetVideoSourceConfiguration)
                return ()
            }

            member this.SetVideoEncoderConfiguration(config:VideoEncoderConfiguration, forcePersistence:bool): Async<unit> = async{
                let request = new SetVideoEncoderConfigurationRequest()
                request.Configuration <- config
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetVideoEncoderConfiguration, proxy.EndSetVideoEncoderConfiguration)
                return ()
            }

            member this.SetAudioSourceConfiguration(config:AudioSourceConfiguration, forcePersistence:bool): Async<unit> = async{
                let request = new SetAudioSourceConfigurationRequest()
                request.Configuration <- config
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetAudioSourceConfiguration, proxy.EndSetAudioSourceConfiguration)
                return ()
            }

            member this.SetAudioEncoderConfiguration(config:AudioEncoderConfiguration, forcePersistence:bool): Async<unit> = async{
                let request = new SetAudioEncoderConfigurationRequest()
                request.Configuration <- config
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetAudioEncoderConfiguration, proxy.EndSetAudioEncoderConfiguration)
                return ()
            }

            member this.SetVideoAnalyticsConfiguration(config:VideoAnalyticsConfiguration, forcePersistence:bool): Async<unit> = async{
                let request = new SetVideoAnalyticsConfigurationRequest()
                request.Configuration <- config
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetVideoAnalyticsConfiguration, proxy.EndSetVideoAnalyticsConfiguration)
                return ()
            }

            member this.SetMetadataConfiguration(config:MetadataConfiguration, forcePersistence:bool): Async<unit> = async{
                let request = new SetMetadataConfigurationRequest()
                request.Configuration <- config
                request.ForcePersistence <- forcePersistence
                let! response = Async.FromBeginEnd(request, proxy.BeginSetMetadataConfiguration, proxy.EndSetMetadataConfiguration)
                return ()
            }

            member this.GetVideoSourceConfigurationOptions(configToken:string, profToken:string): Async<VideoSourceConfigurationOptions> = async{
                let request = new GetVideoSourceConfigurationOptionsRequest()
                request.ConfigurationToken <- configToken
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoSourceConfigurationOptions, proxy.EndGetVideoSourceConfigurationOptions)
                return response.Options
            }

            member this.GetVideoEncoderConfigurationOptions(configToken:string, profToken:string): Async<VideoEncoderConfigurationOptions> = async{
                let request = new GetVideoEncoderConfigurationOptionsRequest()
                request.ConfigurationToken <- configToken
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetVideoEncoderConfigurationOptions, proxy.EndGetVideoEncoderConfigurationOptions)
                return response.Options
            }

            member this.GetAudioSourceConfigurationOptions(configToken:string, profToken:string): Async<AudioSourceConfigurationOptions> = async{
                let request = new GetAudioSourceConfigurationOptionsRequest()
                request.ConfigurationToken <- configToken
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioSourceConfigurationOptions, proxy.EndGetAudioSourceConfigurationOptions)
                return response.Options
            }

            member this.GetAudioEncoderConfigurationOptions(configToken:string, profToken:string): Async<AudioEncoderConfigurationOptions> = async{
                let request = new GetAudioEncoderConfigurationOptionsRequest()
                request.ConfigurationToken <- configToken
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioEncoderConfigurationOptions, proxy.EndGetAudioEncoderConfigurationOptions)
                return response.Options
            }

            member this.GetMetadataConfigurationOptions(configToken:string, profToken:string): Async<MetadataConfigurationOptions> = async{
                let request = new GetMetadataConfigurationOptionsRequest()
                request.ConfigurationToken <- configToken
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetMetadataConfigurationOptions, proxy.EndGetMetadataConfigurationOptions)
                return response.Options
            }

            member this.GetGuaranteedNumberOfVideoEncoderInstances(vscToken:string): Async<GetGuaranteedNumberOfVideoEncoderInstancesResponse> = async{
                let request = new GetGuaranteedNumberOfVideoEncoderInstancesRequest()
                request.ConfigurationToken <- vscToken
                let! response = Async.FromBeginEnd(request, proxy.BeginGetGuaranteedNumberOfVideoEncoderInstances, proxy.EndGetGuaranteedNumberOfVideoEncoderInstances)
                return response
            }

            member this.StartMulticastStreaming(profToken:string): Async<unit> = async{
                let request = new StartMulticastStreamingRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginStartMulticastStreaming, proxy.EndStartMulticastStreaming)
                return ()
            }

            member this.StopMulticastStreaming(profToken:string): Async<unit> = async{
                let request = new StopMulticastStreamingRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginStopMulticastStreaming, proxy.EndStopMulticastStreaming)
                return ()
            }

            member this.SetSynchronizationPoint(profToken:string): Async<unit> = async{
                let request = new SetSynchronizationPointRequest()
                request.ProfileToken <- profToken
                let! response = Async.FromBeginEnd(request, proxy.BeginSetSynchronizationPoint, proxy.EndSetSynchronizationPoint)
                return ()
            }

            //onvif 2.1
            member this.GetServiceCapabilities(): Async<MediaServiceCapabilities> = async{
                let request = new GetServiceCapabilitiesRequest()
                request.GetServiceCapabilities <- new GetServiceCapabilities()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response.GetServiceCapabilitiesResponse1.Capabilities
            }
//            member this.GetAudioOutputs(request:GetAudioOutputsRequest): Async<GetAudioOutputsResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioOutputs, proxy.EndGetAudioOutputs)
//                return response
//            }
//            member this.AddAudioOutputConfiguration(request:AddAudioOutputConfigurationRequest): Async<AddAudioOutputConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginAddAudioOutputConfiguration, proxy.EndAddAudioOutputConfiguration)
//                return response
//            }
//            member this.RemoveAudioOutputConfiguration(request:RemoveAudioOutputConfigurationRequest): Async<RemoveAudioOutputConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveAudioOutputConfiguration, proxy.EndRemoveAudioOutputConfiguration)
//                return response
//            }
//            member this.AddAudioDecoderConfiguration(request:AddAudioDecoderConfigurationRequest): Async<AddAudioDecoderConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginAddAudioDecoderConfiguration, proxy.EndAddAudioDecoderConfiguration)
//                return response
//            }
//            member this.RemoveAudioDecoderConfiguration(request:RemoveAudioDecoderConfigurationRequest): Async<RemoveAudioDecoderConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginRemoveAudioDecoderConfiguration, proxy.EndRemoveAudioDecoderConfiguration)
//                return response
//            }
//            member this.GetAudioOutputConfigurations(request:GetAudioOutputConfigurationsRequest): Async<GetAudioOutputConfigurationsResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioOutputConfigurations, proxy.EndGetAudioOutputConfigurations)
//                return response
//            }
//            member this.GetAudioDecoderConfigurations(request:GetAudioDecoderConfigurationsRequest): Async<GetAudioDecoderConfigurationsResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioDecoderConfigurations, proxy.EndGetAudioDecoderConfigurations)
//                return response
//            }
//            member this.GetAudioOutputConfiguration(request:GetAudioOutputConfigurationRequest): Async<GetAudioOutputConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioOutputConfiguration, proxy.EndGetAudioOutputConfiguration)
//                return response
//            }
//            member this.GetAudioDecoderConfiguration(request:GetAudioDecoderConfigurationRequest): Async<GetAudioDecoderConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioDecoderConfiguration, proxy.EndGetAudioDecoderConfiguration)
//                return response
//            }
//            member this.GetCompatibleAudioOutputConfigurations(request:GetCompatibleAudioOutputConfigurationsRequest): Async<GetCompatibleAudioOutputConfigurationsResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetCompatibleAudioOutputConfigurations, proxy.EndGetCompatibleAudioOutputConfigurations)
//                return response
//            }
//            member this.GetCompatibleAudioDecoderConfigurations(request:GetCompatibleAudioDecoderConfigurationsRequest): Async<GetCompatibleAudioDecoderConfigurationsResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetCompatibleAudioDecoderConfigurations, proxy.EndGetCompatibleAudioDecoderConfigurations)
//                return response
//            }
//            member this.SetAudioOutputConfiguration(request:SetAudioOutputConfigurationRequest): Async<SetAudioOutputConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginSetAudioOutputConfiguration, proxy.EndSetAudioOutputConfiguration)
//                return response
//            }
//            member this.SetAudioDecoderConfiguration(request:SetAudioDecoderConfigurationRequest): Async<SetAudioDecoderConfigurationResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginSetAudioDecoderConfiguration, proxy.EndSetAudioDecoderConfiguration)
//                return response
//            }
//            member this.GetAudioOutputConfigurationOptions(request:GetAudioOutputConfigurationOptionsRequest): Async<GetAudioOutputConfigurationOptionsResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioOutputConfigurationOptions, proxy.EndGetAudioOutputConfigurationOptions)
//                return response
//            }
//            member this.GetAudioDecoderConfigurationOptions(request:GetAudioDecoderConfigurationOptionsRequest): Async<GetAudioDecoderConfigurationOptionsResponse> = async{
//                let! response = Async.FromBeginEnd(request, proxy.BeginGetAudioDecoderConfigurationOptions, proxy.EndGetAudioDecoderConfigurationOptions)
//                return response
//            }
        end
    end
