#pragma once
#include "odm.player.lib/core.h"
#include "odm.player.lib/MetadataCollector.hpp"

namespace onvifmp{
	using namespace std;

	class RtspOptions{
	public:
		bool getParamSupported;
	};

	class Live555 {
	public:

		///<summary>constructor</summary>
		Live555();

		///<summary>destructor</summary>
		~Live555();

		///<summary>Setup output for viedo playback</summary>
		///<param name="videoBuffer">video buffer proposed for rendering of decoded frames</param>
		void SetVideoOutput(shared_ptr<VideoBuffer> videoBuffer);

		///<summary>Setup output metadata stream</summary>
		///<param name="metadataListener">callback to recieve metadata</param>
		void SetMetadataOutput(MetadataCollector::Listener metadataListener);

		///<summary>playback stream</summary>
		///<param name="mediaStreamInfo"></param>
		///<param name="playbackController"></param>
		void Run(MediaStreamInfo* mediaStreamInfo, IPlaybackController* playbackController);

	protected:
		int packetReorderingThresholdTime;

		//Authenticator* authenticator;
		BasicTaskScheduler* scheduler;
		BasicUsageEnvironment* usageEnvironment;
		RTSPClient* rtspClient;
		MediaSession* mediaSession;
		RtspOptions rtspOptions;

		bool videoSubsessionInitialized;
		bool metadataSubsessionInitialized;

		shared_ptr<VideoBuffer> videoBuffer;
		MetadataCollector::Listener metadataListener;
		
		IFrameProcessorFactory InitVideoSubsession(CodecID codecId, const char* sprops);
		
		IFrameProcessorFactory InitMetadataSubsession(const char* sprops);
		
		IFrameProcessorFactory InitSubsession(const char* codecName, const char* sprops);
		
		void SetupSubsession(MediaStreamInfo* mediaStreamInfo, MediaSubsession* subsession);

		bool Init(MediaStreamInfo* mediaStreamInfo);

		void Cleanup();

		///<summary></summary>
		//void SetVideoPlaybackMode(VideoPlaybackMode mode){
		//	this->videoPlaybackMode = mode;
		//}

		///<summary></summary>
		//class RecordingInfo{
		//public:
		//	std::string filePath;
		//};

		///<summary></summary>
		///<returns>
		///true - if recording has been started successfully
		///false - if recording already was being started
		///</returns>
		//bool StartRecord(const char *filePath);
		
		///<summary></summary>
		//void StopRecord();
	protected:
		void WriteLog();
	};
}