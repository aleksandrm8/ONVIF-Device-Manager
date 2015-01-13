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
    open utils
    open utils.fsharp

    [<AllowNullLiteral>]
    type IActionEngineAsync = interface
        abstract GetSupportedActions: unit->Async<SupportedActions>
        abstract GetActions: unit->Async<Action1[]>
        abstract CreateActions: ActionConfiguration[] -> Async<Action1[]>
        abstract DeleteActions: string[] -> Async<unit>
        abstract ModifyActions: Action1[] -> Async<unit>
        abstract GetServiceCapabilities: unit -> Async<ActionEngineCapabilities>
       
        abstract GetActionTriggers: unit -> Async<ActionTrigger[]>
        abstract CreateActionTriggers: ActionTriggerConfiguration[] -> Async<ActionTrigger[]>
        abstract DeleteActionTriggers: string[] -> Async<unit> 
        abstract ModifyActionTriggers: ActionTrigger[] -> Async<unit>
    end

    type ActionEngineAsync(proxy:ActionEnginePort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        interface IActionEngineAsync with
            member this.GetSupportedActions():Async<SupportedActions> = async {
                let request = new GetSupportedActionsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetSupportedActions, proxy.EndGetSupportedActions)
                return response.SupportedActions
            }
            member this.GetActions():Async<Action1[]> = async {
                let request = new GetActionsRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetActions, proxy.EndGetActions)
                return response.Action
            }
            member this.CreateActions(actions:ActionConfiguration[]):Async<Action1[]> = async{
                let request = new CreateActionsRequest(actions)
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateActions, proxy.EndCreateActions)
                return response.Action
            }
            member this.DeleteActions(tokens:string[]):Async<unit> = async{
                let request = new DeleteActionsRequest(tokens)
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteActions, proxy.EndDeleteActions)
                return ()
            }
            member this.ModifyActions(actions:Action1[]):Async<unit> = async{
                let request = new ModifyActionsRequest(actions)
                let! response = Async.FromBeginEnd(request, proxy.BeginModifyActions, proxy.EndModifyActions)
                return ()
            }
            member this.GetServiceCapabilities():Async<ActionEngineCapabilities> = async{
                let request = new GetServiceCapabilitiesRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response.Capabilities
            }

            member this.GetActionTriggers():Async<ActionTrigger[]> = async{
                let request = new GetActionTriggersRequest()
                let! response = Async.FromBeginEnd(request, proxy.BeginGetActionTriggers, proxy.EndGetActionTriggers)
                return response.ActionTrigger
            }
            member this.CreateActionTriggers(actionTriggers:ActionTriggerConfiguration[]):Async<ActionTrigger[]> = async{
                let request = new CreateActionTriggersRequest(actionTriggers)
                let! response = Async.FromBeginEnd(request, proxy.BeginCreateActionTriggers, proxy.EndCreateActionTriggers)
                return response.ActionTrigger
            }
            member this.DeleteActionTriggers(tokens:string[]):Async<unit> = async{
                let request = new DeleteActionTriggersRequest(tokens)
                let! response = Async.FromBeginEnd(request, proxy.BeginDeleteActionTriggers, proxy.EndDeleteActionTriggers)
                return ()
            }
            member this.ModifyActionTriggers(actionTriggers:ActionTrigger[]):Async<unit> = async{
                let request = new ModifyActionTriggersRequest(actionTriggers)
                let! response = Async.FromBeginEnd(request, proxy.BeginModifyActionTriggers, proxy.EndModifyActionTriggers)
                return ()
            }
        end
    end