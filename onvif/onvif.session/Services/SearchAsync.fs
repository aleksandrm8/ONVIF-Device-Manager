namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_search
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type ISearchAsync = interface
        //onvif 2.1
        abstract GetServiceCapabilities: request:GetServiceCapabilitiesRequest -> Async<GetServiceCapabilitiesResponse>
        abstract GetRecordingSummary: request:GetRecordingSummaryRequest -> Async<GetRecordingSummaryResponse>
        abstract GetRecordingInformation: request:GetRecordingInformationRequest -> Async<GetRecordingInformationResponse>
        abstract GetMediaAttributes: request:GetMediaAttributesRequest -> Async<GetMediaAttributesResponse>
        abstract FindRecordings: request:FindRecordingsRequest -> Async<FindRecordingsResponse>
        abstract GetRecordingSearchResults: request:GetRecordingSearchResultsRequest -> Async<GetRecordingSearchResultsResponse>
        abstract FindEvents: request:FindEventsRequest -> Async<FindEventsResponse>
        abstract GetEventSearchResults: request:GetEventSearchResultsRequest -> Async<GetEventSearchResultsResponse>
        abstract FindPTZPosition: request:FindPTZPositionRequest -> Async<FindPTZPositionResponse>
        abstract GetPTZPositionSearchResults: request:GetPTZPositionSearchResultsRequest -> Async<GetPTZPositionSearchResultsResponse>
        abstract GetSearchState: request:GetSearchStateRequest -> Async<GetSearchStateResponse>
        abstract EndSearch: request:EndSearchRequest -> Async<EndSearchResponse>
        abstract FindMetadata: request:FindMetadataRequest -> Async<FindMetadataResponse>
        abstract GetMetadataSearchResults: request:GetMetadataSearchResultsRequest -> Async<GetMetadataSearchResultsResponse>
    end

    type SearchAsync(proxy:SearchPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface ISearchAsync with
            member this.GetServiceCapabilities(request:GetServiceCapabilitiesRequest):Async<GetServiceCapabilitiesResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response
            }
            member this.GetRecordingSummary(request:GetRecordingSummaryRequest):Async<GetRecordingSummaryResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRecordingSummary, proxy.EndGetRecordingSummary)
                return response
            }
            member this.GetRecordingInformation(request:GetRecordingInformationRequest):Async<GetRecordingInformationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRecordingInformation, proxy.EndGetRecordingInformation)
                return response
            }
            member this.GetMediaAttributes(request:GetMediaAttributesRequest):Async<GetMediaAttributesResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetMediaAttributes, proxy.EndGetMediaAttributes)
                return response
            }
            member this.FindRecordings(request:FindRecordingsRequest):Async<FindRecordingsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginFindRecordings, proxy.EndFindRecordings)
                return response
            }
            member this.GetRecordingSearchResults(request:GetRecordingSearchResultsRequest):Async<GetRecordingSearchResultsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetRecordingSearchResults, proxy.EndGetRecordingSearchResults)
                return response
            }
            member this.FindEvents(request:FindEventsRequest):Async<FindEventsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginFindEvents, proxy.EndFindEvents)
                return response
            }
            member this.GetEventSearchResults(request:GetEventSearchResultsRequest):Async<GetEventSearchResultsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetEventSearchResults, proxy.EndGetEventSearchResults)
                return response
            }
            member this.FindPTZPosition(request:FindPTZPositionRequest):Async<FindPTZPositionResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginFindPTZPosition, proxy.EndFindPTZPosition)
                return response
            }
            member this.GetPTZPositionSearchResults(request:GetPTZPositionSearchResultsRequest):Async<GetPTZPositionSearchResultsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetPTZPositionSearchResults, proxy.EndGetPTZPositionSearchResults)
                return response
            }
            member this.GetSearchState(request:GetSearchStateRequest):Async<GetSearchStateResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSearchState, proxy.EndGetSearchState)
                return response
            }
            member this.EndSearch(request:EndSearchRequest):Async<EndSearchResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginEndSearch, proxy.EndEndSearch)
                return response
            }
            member this.FindMetadata(request:FindMetadataRequest):Async<FindMetadataResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginFindMetadata, proxy.EndFindMetadata)
                return response
            }
            member this.GetMetadataSearchResults(request:GetMetadataSearchResultsRequest):Async<GetMetadataSearchResultsResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetMetadataSearchResults, proxy.EndGetMetadataSearchResults)
                return response
            }
        end
    end
