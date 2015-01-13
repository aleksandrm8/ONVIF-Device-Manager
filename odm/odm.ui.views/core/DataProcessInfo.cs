using System;
using System.Windows;
using Microsoft.FSharp.Control;
using utils;
using onvif.services;
using System.Collections.Generic;
using Microsoft.FSharp.Core;
using odm.core;

namespace odm.ui.core {
    public interface IVideoInfo {
        Size Resolution { get; set; }
        string MediaUri { get; set; }
        String ChanToken { get; set; }
        float Fps { get; set; }
    }
    class VideoInfo : IVideoInfo {
        public Size Resolution { get; set; }
        public string MediaUri { get; set; }
        public string ChanToken { get; set; }
        public float Fps { get; set; }
    }

	public class StreamInfoArgs {
		public StreamInfoArgs(INvtSession nvtSession) {
			this.nvtSession = nvtSession;
		}
		public StreamSetup streamSetup { get; set; }
		public string streamUri { get; set; }
		public Size encoderResolution { get; set; }
		public Size sourceResolution { get; set; }
		public INvtSession nvtSession { get; protected set; }
	}
	public interface IStreamInfoHelper {
		Func<FSharpAsync<Unit>> GetFunction();
		StreamInfoArgs GetInfoArgs();
	}
	public class StreamInfoHelper : IStreamInfoHelper {
		public StreamInfoHelper() {
		}
		public Func<FSharpAsync<Unit>> streamInfo;
		public Func<FSharpAsync<Unit>> GetFunction() {
			return streamInfo;
		}
		public StreamInfoArgs args { get; set; }
		public StreamInfoArgs GetInfoArgs() {
			return args;
		}
	}
}
