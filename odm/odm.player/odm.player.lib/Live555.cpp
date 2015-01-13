#include "odm.player.lib/all.h"
#include "GroupsockHelper.hh"

namespace onvifmp{
	using namespace std;

	Live555::Live555(){
		//authenticator = nullptr;
		scheduler = nullptr;
		usageEnvironment = nullptr;
		rtspClient = nullptr;
		mediaSession = nullptr;
		
		videoSubsessionInitialized = false;
		metadataSubsessionInitialized = false;

		packetReorderingThresholdTime = -1;
	}

	Live555::~Live555(){
	}

	void Live555::SetVideoOutput(shared_ptr<VideoBuffer> videoBuffer){
		this->videoBuffer = videoBuffer;
	}

	void Live555::SetMetadataOutput(MetadataCollector::Listener metadataListener){
		this->metadataListener = metadataListener;
	}

	void Live555::Run(MediaStreamInfo* mediaStreamInfo, IPlaybackController* playbackController){
			
		if(!Init(mediaStreamInfo)){
			return;
		}
		//this.playbackController = controller;
			
		struct PlaybackSession: IPlaybackSession{
			PlaybackSession(){
				disposed = false;
			}
			bool disposed;
			virtual void Dispose(){
				disposed = true;
			}
		}playbackSession;

		if(!playbackController->Initialized(&playbackSession)){
			Cleanup();
			return;
		}
		
		rtspClient->playMediaSession(*mediaSession);

		//synchronize via fake GET_PARAMETER request
		//if(rtspOptions.getParamSupported){
		//	char* getParamResponse = nullptr;
		//	rtspClient->getMediaSessionParameter(*mediaSession, nullptr, getParamResponse);
		//}

		scheduler->doEventLoop((char*)&(playbackSession.disposed));
		if(!playbackSession.disposed){
			playbackController->Shutdown();
		}
		rtspClient->teardownMediaSession(*mediaSession);
		Cleanup();
	}


	IFrameProcessorFactory Live555::InitVideoSubsession(CodecID codecId, const char* sprops){
		//dbg::Info(sys::String::Format("processing subsession for {0}",gcnew sys::String(codecName)));
		if(videoBuffer == nullptr || videoSubsessionInitialized){
			return nullptr;
		}
		videoSubsessionInitialized = true;
		auto videoDecoderFactory = VideoDecoder::Create(codecId, sprops);
		auto videoRendererFactory = VideoRenderer::Create(videoBuffer);
		return ([=](VirtualSink* sink)->shared_ptr<IFrameProcessor>{
			auto videoDecoder = videoDecoderFactory(sink);
			videoDecoder->AddVideoRenderer(videoRendererFactory);
			return videoDecoder;
		});
	}

	IFrameProcessorFactory Live555::InitMetadataSubsession(const char* sprops){
		//auto sink = MetaSink::Create(*env, buf_size, pLive->mEvent, pLive->mCallback);
		if(metadataListener == nullptr || metadataSubsessionInitialized){
			return nullptr;
		}
		metadataSubsessionInitialized = true;
		auto metadataCollectorFactory = MetadataCollector::Create();
		return ([=](VirtualSink* sink)->shared_ptr<IFrameProcessor>{
			auto metadataCollector = metadataCollectorFactory(sink);
			metadataCollector->AddListener(this->metadataListener);
			return metadataCollector;
		});
	}
		
	IFrameProcessorFactory Live555::InitSubsession(const char* codecName, const char* sprops){
		if(_stricmp(codecName, "META")==0){
			return InitMetadataSubsession(sprops);
		}else if (_stricmp(codecName, "vnd.onvif.metadata")==0){
			return InitMetadataSubsession(sprops);
		}else if (_stricmp(codecName, "JPEG")==0){
			return InitVideoSubsession(CODEC_ID_MJPEG, sprops);
		}else if (_stricmp(codecName, "H264")==0){
			return InitVideoSubsession(CODEC_ID_H264, sprops);
		}else if (_stricmp(codecName, "MPEG4")==0){
			return InitVideoSubsession(CODEC_ID_MPEG4, sprops);
		}else if (_stricmp(codecName, "MP4V-ES")==0){
			return InitVideoSubsession(CODEC_ID_MPEG4, sprops);
		}
		return nullptr;
	}
		
	void Live555::SetupSubsession(MediaStreamInfo* mediaStreamInfo, MediaSubsession* subsession){
		auto rtpSource = subsession->rtpSource();
		if(rtpSource==NULL){
			//dbg::Info("rtpSource is NULL");
			return;
		}
		if(packetReorderingThresholdTime != -1){
			rtpSource->setPacketReorderingThresholdTime(
				packetReorderingThresholdTime
			);
		}
		auto streamUsingTcp = mediaStreamInfo->transport != StreamTransport::Udp;
		//rtspClient->setupMediaSubsession(*subsession, false, false, false);
		auto codecName = subsession->codecName();
		auto sprops = subsession->fmtp_spropparametersets();
		auto frameProcessorFactory = InitSubsession(codecName, sprops);
		if(frameProcessorFactory!=nullptr){
			rtspClient->setupMediaSubsession(*subsession, false, streamUsingTcp, false);
			VirtualSink* sink;
			if(_stricmp(codecName, "H264")==0){
				//create special sink to fix H264 payload format
				sink = H264VirtualSink::CreateNew(*usageEnvironment);
				//sink = VirtualSink::CreateNew(*usageEnvironment);
			}else{
				sink = VirtualSink::CreateNew(*usageEnvironment);
			}
			
			//TODO: increase receiver buffer size for HD videos
			increaseReceiveBufferTo(*usageEnvironment, rtpSource->RTPgs()->socketNum(), 5000*1024);

			sink->AddFrameProcessor(frameProcessorFactory);
			subsession->sink = sink;
			auto source = subsession->readSource();
			//dbg::Info(sys::String::Format("setup subsession for {0}",gcnew sys::String(codecName)));
			sink->startPlaying(*source, 0, 0);
		}
	}

	bool Live555::Init(MediaStreamInfo* mediaStreamInfo){
		//if it has been initialized before, we should do cleanup first
		Cleanup();

		scheduler = BasicTaskScheduler::createNew();
		if(scheduler == NULL){
			return false;
		}
		usageEnvironment = BasicUsageEnvironment::createNew(*scheduler);
		if(usageEnvironment == NULL){
			Cleanup();
			return false;
		}
		auto httpProt = 0U;
		
		if(mediaStreamInfo->transport == StreamTransport::Http){
			char* username = nullptr;
			char* password = nullptr;
			NetAddress address;
			portNumBits port;
			if(!RTSPClient::parseRTSPURL(*usageEnvironment, mediaStreamInfo->url, username, password, address, port)){
				//TODO: log error
				fprintf(stderr, "failed to parse url with error message: %s\n", usageEnvironment->getResultMsg());
				if(username != nullptr){
					delete[] username;
				}
				if(password != nullptr){
					delete[] password;
				}
				Cleanup();
				return false;
			}
			httpProt = port;
			delete[] username;
			delete[] password;
		}
		rtspClient = RTSPClient::createNew(*usageEnvironment, nullptr, 1/*verbosityLevel*/, nullptr/*applicationName*/, httpProt);
		if(rtspClient == NULL){
			//TODO: log error
			fprintf(stderr, "error %d: %s\n", usageEnvironment->getErrno(), usageEnvironment->getResultMsg());
			Cleanup();
			return false;
		}

		//rtspClient->fCurrentAuthenticator = mediaStreamInfo->authenticator;
		auto options = rtspClient->sendOptionsCmd(mediaStreamInfo->url, nullptr, nullptr, mediaStreamInfo->authenticator, 5/*timeout in seconds*/);
		rtspOptions.getParamSupported = (options!= nullptr && strstr(options, "GET_PARAMETER")!=nullptr);
		
		//fprintf(stderr, "options : %s\n", options);

		//should we take care of release sdp string???
		auto sdp = rtspClient->describeURL(mediaStreamInfo->url, mediaStreamInfo->authenticator,0U, 5/*timeout in seconds*/);
		if(sdp == NULL){
			//TODO: log error
			fprintf(stderr, "error %d: %s", usageEnvironment->getErrno(), usageEnvironment->getResultMsg());
			Cleanup();
			return false;
		}
		
		mediaSession = MediaSession::createNew(*usageEnvironment, sdp);
		if(mediaSession == NULL){
			//TODO: log error
			fprintf(stderr, "error %d: %s", usageEnvironment->getErrno(), usageEnvironment->getResultMsg());
			delete[] sdp;
			Cleanup();
			return false;
		}
		delete[] sdp;
		sdp = nullptr;

		MediaSubsessionIterator itor(*mediaSession);
		auto subsession = itor.next();
		while(subsession != NULL){
			//subsession->setClientPortNum(-1);
			if(subsession->initiate(0)){
				SetupSubsession(mediaStreamInfo, subsession);
			}
			subsession = itor.next();
		}
		return true;
	}

	void Live555::Cleanup(){
		if(mediaSession != nullptr){
			MediaSubsessionIterator itor(*mediaSession);
			auto subsession = itor.next();
			while(NULL != subsession) {
				//auto rtpSource = subsession->rtpSource();
				//if(rtpSource != nullptr){
				//	rtpSource->stopGettingFrames();
				//}
				if(subsession->sink != NULL){
					subsession->sink->stopPlaying();
					MediaSink::close(subsession->sink);
					subsession->sink = NULL;
				}
				subsession->deInitiate();
				subsession = itor.next();
			}
			MediaSession::close(mediaSession);
			mediaSession = nullptr;
		}
		if(rtspClient != nullptr){
			RTSPClient::close(rtspClient);
			rtspClient = nullptr;
		}
		if(usageEnvironment != nullptr){
			usageEnvironment->reclaim();
			usageEnvironment = nullptr;
		}
		if(scheduler != nullptr){
			delete scheduler;
			scheduler = nullptr;
		}
		videoSubsessionInitialized = false;
		metadataSubsessionInitialized = false;
	}

	void Live555::WriteLog(){
		//char buf[255];
		//const char *source = NULL;

		////work around // we have %td from av_codec
		//string msg(aMsg);
		//size_t pos = std::string::npos;
		//while (std::string::npos != (pos = msg.find("%td")))
		//msg.replace(pos, 3, "%ld");
		////work around // we have %td from av_codec

		//vsprintf_s<255>(buf, msg.c_str(), aArgs);
		//if (aClass)
		//{
		//AVClass* avc= aClass ? *(AVClass**)aClass : NULL;
		//if (avc)
		//source = avc->item_name(aClass);
		//}
		//pLive->mInstance.Log(buf, source, (AV_LOG_FATAL == aLevel || AV_LOG_ERROR == aLevel) ?
		//LOG_ERROR : (AV_LOG_WARNING == aLevel ? LOG_WARNING : LOG_INFORMATION));
	}
}