namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_recording
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IRecordingAsync = interface
        //onvif 2.1
        abstract GetServiceCapabilities: unit -> Async<Capabilities7>
        abstract CreateRecording: recordingConfig:RecordingConfiguration -> Async<string>
        abstract DeleteRecording: recordingToken:string -> Async<unit>
        abstract GetRecordings: unit -> Async<GetRecordingsResponseItem[]>
        abstract SetRecordingConfiguration: recordingToken:string* recordingConfig:RecordingConfiguration -> Async<unit>
        abstract GetRecordingConfiguration: recordingToken:string -> Async<RecordingConfiguration>
        abstract CreateTrack: recordingToken:string* trackConfig:TrackConfiguration -> Async<string>
        abstract DeleteTrack: recordingToken:string* trackToken:string -> Async<unit>
        abstract GetTrackConfiguration: recordingToken:string* trackToken:string -> Async<TrackConfiguration>
        abstract SetTrackConfiguration: recordingToken:string* trackToken:string* trackConfig:TrackConfiguration -> Async<unit>
        abstract CreateRecordingJob: jobConfiguration:RecordingJobConfiguration -> Async<string* RecordingJobConfiguration>
        abstract DeleteRecordingJob: jobToken:string -> Async<unit>
        abstract GetRecordingJobs: unit -> Async<GetRecordingJobsResponseItem[]>
        abstract SetRecordingJobConfiguration: jobToken:string* recordingJobConfig:RecordingJobConfiguration -> Async<RecordingJobConfiguration>
        abstract GetRecordingJobConfiguration: jobToken:string -> Async<RecordingJobConfiguration>
        abstract SetRecordingJobMode: jobToken:string* mode:string -> Async<unit>
        abstract GetRecordingJobState: jobToken:string -> Async<RecordingJobStateInformation>
    end

    type RecordingAsync(proxy:RecordingPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IRecordingAsync with
            member this.GetServiceCapabilities():Async<Capabilities7> = async{
                let request = GetServiceCapabilitiesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response.Capabilities
            }
            member this.CreateRecording(recordingConfig:RecordingConfiguration) = async{
                let request = new CreateRecordingRequest()
                request.RecordingConfiguration <- recordingConfig

                let! response = Async.FromBeginEnd(request, proxy.BeginCreateRecording, proxy.EndCreateRecording)
                return response.RecordingToken
            }
            member this.DeleteRecording(recordingToken:string):Async<unit> = async{
                let request = new DeleteRecordingRequest()
                request.RecordingToken <- recordingToken

                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteRecording, proxy.EndDeleteRecording)
                return ()
            }
            member this.GetRecordings():Async<GetRecordingsResponseItem[]> = async{
                let request = new GetRecordingsRequest()

                let! response = Async.FromBeginEnd(request, proxy.BeginGetRecordings, proxy.EndGetRecordings)
                return response.RecordingItem
            }
            member this.SetRecordingConfiguration(recordingToken:string, recordingConfig:RecordingConfiguration):Async<unit> = async{
                let request = new SetRecordingConfigurationRequest()
                request.RecordingConfiguration <- recordingConfig
                request.RecordingToken <- recordingToken

                let! response = Async.FromBeginEnd(request, proxy.BeginSetRecordingConfiguration, proxy.EndSetRecordingConfiguration)
                return ()
            }
            member this.GetRecordingConfiguration(recordingToken:string):Async<RecordingConfiguration> = async{
                let request = new GetRecordingConfigurationRequest()
                request.RecordingToken <- recordingToken
                
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRecordingConfiguration, proxy.EndGetRecordingConfiguration)
                return response.RecordingConfiguration
            }
            member this.CreateTrack(recordingToken:string, trackConfig:TrackConfiguration):Async<string> = async{
                let request = new CreateTrackRequest()
                request.RecordingToken <- recordingToken
                request.TrackConfiguration <- trackConfig

                let! response = Async.FromBeginEnd(request, proxy.BeginCreateTrack, proxy.EndCreateTrack)
                return response.TrackToken
            }
            member this.DeleteTrack(recordingToken:string, trackToken:string):Async<unit> = async{
                let request = new DeleteTrackRequest()
                request.RecordingToken <- recordingToken
                request.TrackToken <- trackToken
                
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteTrack, proxy.EndDeleteTrack)
                return ()
            }
            member this.GetTrackConfiguration(recordingToken:string, trackToken:string):Async<TrackConfiguration> = async{
                let request = new GetTrackConfigurationRequest()
                request.RecordingToken <- recordingToken
                request.TrackToken <- trackToken

                let! response = Async.FromBeginEnd(request, proxy.BeginGetTrackConfiguration, proxy.EndGetTrackConfiguration)
                return response.TrackConfiguration
            }
            member this.SetTrackConfiguration(recordingToken:string, trackToken:string, trackConfig:TrackConfiguration):Async<unit> = async{
                let request = new SetTrackConfigurationRequest()
                request.RecordingToken <- recordingToken
                request.TrackToken <- trackToken
                request.TrackConfiguration <- trackConfig
                
                let! response = Async.FromBeginEnd(request, proxy.BeginSetTrackConfiguration, proxy.EndSetTrackConfiguration)
                return ()
            }
            member this.CreateRecordingJob(jobConfiguration:RecordingJobConfiguration):Async<string* RecordingJobConfiguration> = async{
                let request = new CreateRecordingJobRequest()
                request.JobConfiguration <- jobConfiguration

                let! response = Async.FromBeginEnd(request, proxy.BeginCreateRecordingJob, proxy.EndCreateRecordingJob)
                return response.JobToken, response.JobConfiguration
            }
            member this.DeleteRecordingJob(jobToken:string):Async<unit> = async{
                let request = new DeleteRecordingJobRequest()
                request.JobToken <- jobToken

                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteRecordingJob, proxy.EndDeleteRecordingJob)
                return ()
            }
            member this.GetRecordingJobs():Async<GetRecordingJobsResponseItem[]> = async{
                let request = new GetRecordingJobsRequest()

                let! response = Async.FromBeginEnd(request, proxy.BeginGetRecordingJobs, proxy.EndGetRecordingJobs)
                return response.JobItem
            }
            member this.SetRecordingJobConfiguration(jobToken:string, recordingJobConfig:RecordingJobConfiguration):Async<RecordingJobConfiguration> = async{
                let request = new SetRecordingJobConfigurationRequest()
                request.JobConfiguration <- recordingJobConfig
                request.JobToken <- jobToken

                let! response = Async.FromBeginEnd(request, proxy.BeginSetRecordingJobConfiguration, proxy.EndSetRecordingJobConfiguration)
                return response.JobConfiguration
            }
            member this.GetRecordingJobConfiguration(jobToken:string):Async<RecordingJobConfiguration> = async{
                let request = new GetRecordingJobConfigurationRequest()
                request.JobToken <- jobToken

                let! response = Async.FromBeginEnd(request, proxy.BeginGetRecordingJobConfiguration, proxy.EndGetRecordingJobConfiguration)
                return response.JobConfiguration
            }
            member this.SetRecordingJobMode(jobToken:string, mode:string):Async<unit> = async{
                let request = new SetRecordingJobModeRequest()
                request.JobToken <- jobToken
                request.Mode <- mode

                let! response = Async.FromBeginEnd(request, proxy.BeginSetRecordingJobMode, proxy.EndSetRecordingJobMode)
                return ()
            }
            member this.GetRecordingJobState(jobToken:string):Async<RecordingJobStateInformation> = async{
                let request = new GetRecordingJobStateRequest()
                request.JobToken <- jobToken

                let! response = Async.FromBeginEnd(request, proxy.BeginGetRecordingJobState, proxy.EndGetRecordingJobState)
                return response.State
            }

        end
    end
