namespace onvif.services {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Dynamic;
	using System.Globalization;
	using System.Linq;
	using System.Net;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.Text;
	using System.Xml;
	using System.Xml.Linq;
	using System.Xml.Schema;
	using System.Xml.Serialization;
	using onvif10_analyticsdevice;
	using onvif10_device;
	using onvif10_deviceio;
	using onvif10_display;
	using onvif10_imaging;
	using onvif10_media;
	using onvif10_receiver;
	using onvif10_recording;
	using onvif10_replay;
	using onvif10_search;
	using onvif20_ptz;
	using utils;

	public class OnvifFactory {
		[ServiceContract]
		private interface OnvifServices : Device, Media, PTZ, ImagingPort,
			ReceiverPort, DeviceIOPort, DisplayPort, AnalyticsDevicePort,
			RecordingPort, ReplayPort/*, SearchPort, IEvent, IAnalytics*/ {
		}
		private ChannelFactory<OnvifServices> factory;
		private static HttpTransportBindingElement CreateTransportBindingElement(bool useTls){
			if(useTls){
				var transport = new HttpsTransportBindingElement();
				transport.RequireClientCertificate = false;
				return transport;
			}else{
				return new HttpTransportBindingElement();
			}
		}
		private static IEnumerable<BindingElement> CreateBindingElements(bool mtomEncoding, bool wsAddressing, bool securityToken, bool useTls){
			var msgVer = wsAddressing ? 
				MessageVersion.Soap12WSAddressing10 :
				MessageVersion.Soap12;
			if(mtomEncoding){
				var encoding = new MtomMessageEncodingBindingElement(msgVer, Encoding.UTF8);
				encoding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
				encoding.MaxBufferSize = Int32.MaxValue;
				yield return encoding;
			}else{
				var encoding = new TextMessageEncodingBindingElement(msgVer, Encoding.UTF8);
				encoding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue; //100 * 1024 * 1024
				yield return encoding;
			}

			//var security = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
			//security..M .MessageSecurityVersion = MessageSecurityVersion.

			HttpTransportBindingElement transport = CreateTransportBindingElement(useTls);
			transport.MaxReceivedMessageSize = Int32.MaxValue; //100L * 1024L * 1024L
			transport.KeepAliveEnabled = false;
			transport.MaxBufferSize = Int32.MaxValue;
			transport.ProxyAddress = null;
			transport.BypassProxyOnLocal = true;
			//transport.ManualAddressing = true
			transport.UseDefaultWebProxy = false;
			transport.TransferMode =TransferMode.StreamedResponse;
			//transport.TransferMode = TransfeStreamedResponse
			transport.AuthenticationScheme = AuthenticationSchemes.Basic;
			//transport.AuthenticationScheme = AuthenticationSchemes.Digest;
			
			yield return transport;
		}
		public OnvifFactory(bool mtomEncoding, bool wsAddressing, bool securityToken, bool useTls) {
			var binding = new CustomBinding(
				CreateBindingElements(mtomEncoding, wsAddressing, securityToken, useTls)
			);
			
			binding.CloseTimeout = TimeSpan.FromSeconds(30.0);
			binding.OpenTimeout = TimeSpan.FromSeconds(30.0);
			//binding.SendTimeout = TimeSpan.FromMinutes(3.0);
			binding.SendTimeout = TimeSpan.FromMinutes(10.0);
			binding.ReceiveTimeout = TimeSpan.FromMinutes(3.0);
			//binding.Security.Mode = SecurityMode.Transport;
			//binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
			var factory = new ChannelFactory<OnvifServices>(binding);
			factory.Credentials.UserName.UserName = "admin";
			factory.Credentials.UserName.Password = "123";
			if (securityToken) {
				//factory.Endpoint.Behaviors.Add(new CustomBehavior());
			}
			this.factory = factory;
		}
		//T CreateChannel<T>(Uri deviceUri, params AddressHeader[] addressHeaders) {
		//    throw new NotImplementedException();
		//}
		public Device CreateDeviceChannel(Uri deviceUri, params AddressHeader[] addressHeaders){
			return factory.CreateChannel(new EndpointAddress(deviceUri, addressHeaders));
		}
		public Media CreateMediaChannel(Uri mediaUri, params AddressHeader[] addressHeaders) {
			return factory.CreateChannel(new EndpointAddress(mediaUri, addressHeaders));
		}
		//public PTZ CreatePtzChannel(Uri ptzUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(ptzUri, addressHeaders));
		//}
		//IEvent CreateEventChannel(Uri eventUri) {
		//    throw new NotImplementedException();
		//}
		//IAnalytics CreateAnalyticsChannel(Uri analyticsUri) {
		//    throw new NotImplementedException();
		//}
		//public ImagingPort CreateImagingChannel(Uri imagingUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(imagingUri, addressHeaders));
		//}
		//Network CreateNetworkChannel(Uri networkUri) {
		//    throw new NotImplementedException();
		//}
		//public ReceiverPort CreateReceiverChannel(Uri receiverUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(receiverUri, addressHeaders));
		//}
		//public DeviceIOPort CreateDeviceIoChannel(Uri deviceIoUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(deviceIoUri, addressHeaders));
		//}
		//public DisplayPort CreateDisplayChannel(Uri displayUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(displayUri, addressHeaders));
		//}
		//public AnalyticsDevicePort CreateAnalyticsDeviceChannel(Uri analyticsDeviceUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(analyticsDeviceUri, addressHeaders));
		//}
		//public RecordingPort CreateRecordingChannel(Uri recordingUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(recordingUri, addressHeaders));
		//}
		//public ReplayPort CreateReplayChannel(Uri replayUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(replayUri, addressHeaders));
		//}
		//public SearchPort CreateSearchChannel(Uri searchUri, params AddressHeader[] addressHeaders) {
		//    return factory.CreateChannel(new EndpointAddress(searchUri, addressHeaders));
		//}
	}
}
