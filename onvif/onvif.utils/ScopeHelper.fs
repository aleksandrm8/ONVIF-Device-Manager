module onvif.utils.ScopeHelper
    open System
    open System.Collections.Generic
    open System.Linq

    open utils.fsharp
    
    let defaultProfileName = @"odm-dp-{0}"
    let onvifNameScope = @"onvif://www.onvif.org/name/"
    let onvifHardwareScope = @"onvif://www.onvif.org/hardware/"
    let onvifLocationScope = "onvif://www.onvif.org/location/"
    let odmNameScope = @"odm:name:"
    let odmLocationScope = @"odm:location:"
    //let odmProfileScope = @"odm:profile:";
    let odmDeviceIdScope = @"odm:device-id:"
    //TODO:place it in synesis specific plugin
    //let synesisDeviceIconScope = "synesis:device-icon:"
    let synesisDeviceIconScope = "image:"
    

    let GetScopeValues(scopes: IEnumerable<string>, scopePrefix: string): string[] = 
        if scopes |> IsNull then
            null
        else
            scopes
                |> Seq.filter (fun x-> x.StartsWith(scopePrefix))
                |> Seq.map (fun x -> x.Substring(scopePrefix.Length))
                |> Seq.map (fun x -> Uri.UnescapeDataString(x))
                |> Seq.toArray
            

    let GetName(scopes: IEnumerable<string>): string =
        let names = GetScopeValues(scopes, odmNameScope)
        if names.Length > 0 then
            names.[names.Length - 1]
        else
            let names = GetScopeValues(scopes, onvifNameScope)
            if names.Length > 0 then
                names.[names.Length - 1]
            else
                null

    let GetHardware(scopes: IEnumerable<string>): string =
        String.Join("; ",GetScopeValues(scopes, onvifHardwareScope))

    let GetLocation(scopes:IEnumerable<string>):string = 
        let locations = GetScopeValues(scopes, odmLocationScope)
        if locations.Length > 0 then
            String.Join("; ", locations.Where(fun x->not(String.IsNullOrEmpty(x))))
        else
            let locations = GetScopeValues(scopes, onvifLocationScope)
            if locations |> NotNull then
                String.Join("; ", locations.Where(fun x->not(String.IsNullOrEmpty(x))))
            else
                null

    //TODO:place it in synesis specific plugin
    let GetDeviceIconUri(scopes:IEnumerable<string>):string = 
        let uris = GetScopeValues(scopes, synesisDeviceIconScope)
        uris.FirstOrDefault(fun uri->not(String.IsNullOrWhiteSpace(uri)))

//
//		public static string GetDeviceId(IEnumerable<string> scopes) {
//			return GetScopeValues(scopes, odmDeviceIdScope).Single();
//		}
//
//		public static string GetChannelProfileToken(VideoSourceToken videoSourceToken) {
//			//var ch = Convert.ToBase64String(Encoding.UTF8.GetBytes(videoSourceToken.value));
//			var ch = videoSourceToken.value;
//			return new string(String.Format(defaultProfileName, ch));
//		}