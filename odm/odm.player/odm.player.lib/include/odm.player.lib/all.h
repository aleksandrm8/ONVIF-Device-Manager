#pragma once

//#include <Windows.h>

//extern "C"{
//#include "libavformat/avformat.h"
//#include "libavcodec/avcodec.h"
//#include "libswscale/swscale.h"
//#include "libavutil/pixfmt.h"
//}
//
////extern "C"{
//#include "BasicUsageEnvironment.hh"
//#include "MediaSink.hh"
//#include "MediaSession.hh"
//#include "RTSPClient.hh"
//#include "AVIFileSink.hh"
//#include "H264VideoRTPSource.hh"
////}

#include "odm.player.lib/core.h"

//#include <string>
////#include <map>
//#include <deque>
//#include <memory>
//#include <functional>
//#include <algorithm>

namespace onvifmp{
//	using namespace std;
//	
//	//forward declaration of classes
//	class Live555;
//	class VirtualSink;
//	class VideoRenderer;
//	class VideoDecoder;
//
//	class VideoBuffer{
//	public:
//		int width;
//		int height;
//		PixelFormat pixelFormat;
//		int stride[4];
//		uint8_t* scan0[4];
//		uint8_t* signal;
//	};
//
//	enum StreamTransport{
//		Udp,
//		Tcp,
//		Http
//	};
//
//	class MediaStreamInfo{
//	public:
//		char* url;
//		Authenticator* authenticator;
//		StreamTransport transport;
//	};
//	
//	enum VideoPlaybackMode{
//		//normal playback, decoding and render to video buffer
//		Rendering,
//
//		//decoded frames are not copied to video buffer
//		//this mode intended to be used as fast variant of pause
//		DecodingOnly
//	};
//
//	//interfaces
//
//	///<summary></summary>
//	class IDisposable{
//	public:
//		///<summary></summary>
//		///<param name=""></param>
//		///<returns></returns>
//		virtual void Dispose()=0;
//	};
//
//	///<summary></summary>
//	class ILive555Unit: public IDisposable{
//	public:
//		///<summary></summary>
//		///<returns></returns>
//		//virtual UsageEnvironment& GetUsageEnvironment();
//	};
//
//	///<summary></summary>
//	class IFrameProcessor: public ILive555Unit{
//	public:
//		///<summary></summary>
//		///<param name="frame">pointer to frame</param>
//		///<param name="frameSize">size of frame in bytes</param>
//		///<param name="presentationTime">time of frame presentation</param>
//		///<param name="duration">duration of frame presentation in microseconds</param>
//		virtual void ProcessFrame(unsigned char* framePtr, int frameSize, struct timeval presentationTime, unsigned duration)=0;
//		
//		///<summary></summary>
//		///<param name="reason"></param>
//		//virtual void Shutdown(int reason) = 0;
//	};
//
//	class IVideoRenderer: public ILive555Unit{
//	public:
//		virtual void RenderFrame(AVCodecContext* avCodecContext, AVFrame* avFrame)=0;
//	};
//
//	class IPlaybackSession: public IDisposable{
//		//StartRecord
//		//StopRecord
//		//SetPlaybackMode
//	};
//
//	class IPlaybackController{
//	public:
//		virtual bool Initialized(IPlaybackSession* session)=0;
//		virtual void Shutdown()=0;
//	};
//
//	typedef function<shared_ptr<VirtualSink*> (UsageEnvironment*)> VirtualSinkFactory;
//	typedef function<shared_ptr<IFrameProcessor> (VirtualSink* sink)> IFrameProcessorFactory;
//	typedef function<shared_ptr<IVideoRenderer> (VideoDecoder* videoDecoder)> IVideoRendererFactory;
//	
	
	

}

#include "odm.player.lib/VirtualSink.hpp"
#include "odm.player.lib/H264VirtualSink.hpp"
#include "odm.player.lib/VideoDecoder.hpp"
#include "odm.player.lib/VideoRenderer.hpp"
#include "odm.player.lib/VideoRecorder.hpp"
#include "odm.player.lib/AudioDecoder.hpp"
#include "odm.player.lib/AudioRenderer.hpp"
#include "odm.player.lib/Live555.hpp"

