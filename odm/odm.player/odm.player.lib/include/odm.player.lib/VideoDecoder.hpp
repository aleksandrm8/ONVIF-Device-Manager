#pragma once
#include "odm.player.lib/all.h"

namespace onvifmp{

	class VideoDecoder : public IFrameProcessor{
	public:
		typedef function<shared_ptr<VideoDecoder> (VirtualSink* sink)> Factory;
		static Factory Create(CodecID codecId, const char* sporps){
			return ([=](VirtualSink* sink)->shared_ptr<VideoDecoder>{
				//avcodec_init();
				av_register_all();
				avcodec_register_all();

				auto avCodec = avcodec_find_decoder(codecId);
				if (avCodec == NULL) {
					return nullptr;
				}
				auto videoDecoder = make_shared<VideoDecoder>();
				if(!videoDecoder->Init(sink, avCodec, sporps)){
					return nullptr;
				}
				return videoDecoder;
			});
		}

		VideoDecoder(){
			avCodec = NULL;
			avCodecContext = NULL;
			avFrame = NULL;
			frameBuffer = NULL;
			extraDataSize = 0;
		}

		~VideoDecoder(){
			Cleanup();
		}

		bool Init(VirtualSink* sink, AVCodec* avCodec, const char* sprops){
			//if it has been initialized before, we should do cleanup first
			Cleanup();

			avCodecContext = avcodec_alloc_context();
			if (!avCodecContext) {
				//failed to allocate codec context
				Cleanup();
				return false;
			}
			uint8_t startCode[] = {0x00, 0x00, 0x01};
			if(sprops != NULL){
				unsigned spropCount;
				SPropRecord* spropRecords = parseSPropParameterSets(sprops, spropCount);
				try{
					for (unsigned i = 0; i < spropCount; ++i) {
						AddExtraData(startCode, sizeof(startCode));
						AddExtraData(spropRecords[i].sPropBytes, spropRecords[i].sPropLength);
					}
				}catch(void*){
					//extradata exceeds size limit
					delete[] spropRecords;
					Cleanup();
					return false;
				}
				delete[] spropRecords;
					
				avCodecContext->extradata = extraDataBuffer;
				avCodecContext->extradata_size = extraDataSize;
			}
			AddExtraData(startCode, sizeof(startCode));
			avCodecContext->flags = 0;

			if (avcodec_open(avCodecContext, avCodec) < 0) {
				//failed to open codec
				Cleanup();
				return false;
			}
			if (avCodecContext->codec_id == CODEC_ID_H264){
				avCodecContext->flags2 |= CODEC_FLAG2_CHUNKS;
				//avCodecContext->flags2 |= CODEC_FLAG2_SHOW_ALL;
			}
			avFrame = avcodec_alloc_frame();
			if (!avFrame){
				//failed to allocate frame
				Cleanup();
				return false;
			}
			return true;
		}

		void Cleanup(){
			extraDataSize = 0;
			if (avFrame != NULL){
				av_free(avFrame);
				avFrame = NULL;
			}
			if(avCodecContext != NULL){
				avcodec_close(avCodecContext);
				av_free(avCodecContext);
				avCodecContext = NULL;
			}
		}

		///<summary></summary>
		///<param name="factory"></param>
		///<returns></returns>
		void AddVideoRenderer(IVideoRendererFactory factory){
			if(videoRenderer!=nullptr){
				videoRenderer->Dispose();
				videoRenderer = NULL;
			}
			if(factory!=nullptr){
				videoRenderer = factory(this);
			}
		}

	protected:
		AVCodec* avCodec;
		AVCodecContext* avCodecContext;
		AVFrame* avFrame;
		char* frameBuffer;
		int extraDataSize;
		static const int MaxExtraDataSize = 1024;
		uint8_t extraDataBuffer[MaxExtraDataSize];
		shared_ptr<IVideoRenderer> videoRenderer;
		
		void AddExtraData(uint8_t* data, int size){
			auto newSize = extraDataSize+size;
			if(newSize > MaxExtraDataSize){
				throw "extradata exceeds size limit";
			}
			memcpy(extraDataBuffer + extraDataSize, data, size);
			extraDataSize = newSize;
		}

		virtual void ProcessFrame(unsigned char* framePtr, int frameSize, struct timeval presentationTime, unsigned duration){
			static int frame_num = 0;
			auto started = clock();

			AVPacket avpkt;
			avpkt.data = framePtr;
			avpkt.size = frameSize;
			while (avpkt.size > 0) {
				int got_frame = 0;
				auto len = avcodec_decode_video2(avCodecContext, avFrame, &got_frame, &avpkt);
				if (len < 0) {
					//TODO: log error
					return;
				}
				//int silentMode = 0;
				//mSilentMode.GetValue(silentMode);
				if (got_frame /*&& !silentMode*/) {
					//printf("frame decoded...\n");
					if(videoRenderer!=nullptr){
						videoRenderer->RenderFrame(avCodecContext, avFrame);
					}
				}
				avpkt.size -= len;
				avpkt.data += len;
			}
			printf("processed in %ldms\n", clock()-started);
			//dbg::Info(sys::String::Format("processed in {0}ms",clock()-started));
		}
		virtual void Dispose(){
			//
		}
	};
}