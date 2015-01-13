#pragma once
#include "odm.player.lib/all.h"

namespace onvifmp{
	using namespace std;

	///<summary></summary>
	class H264VirtualSink : public VirtualSink{
	public:
		///<summary>
		///create new H264VirtualSink object
		///instance must be released by calling MediaSink::close
		///</summary>
		///<param name="usageEnvironment">usage environment</param>
		///<param name="bufferSize">max packet payload</param>
		///<returns>pointer to created H264VirtualSink object, if successed or nullptr otherwise</returns>
		static H264VirtualSink* CreateNew(UsageEnvironment& usageEnvironment, unsigned bufferSize = (4*1024*1024)){
			try{
				return new H264VirtualSink(usageEnvironment, bufferSize);
			}catch(void*){
				return nullptr;
			}
		}

	protected:
		typedef uint8_t StartCode4_t[4];
		unsigned startCodeSize;
		
		///<summary>constructor</summary>
		///<param name="usageEnvironment">usage environment</param>
		///<param name="bufferSize">max packet payload</param>
		H264VirtualSink(UsageEnvironment& evn, unsigned bufSize): VirtualSink(evn, bufSize+sizeof(StartCode4_t)){
			static const StartCode4_t startCode = {0x00, 0x00, 0x00, 0x01};
			memcpy(bufferPtr, (void*)startCode, sizeof(StartCode4_t));
			bufferPtr += sizeof(StartCode4_t);
			this->bufferSize -= sizeof(StartCode4_t);
		}

		///<summary>destructor, called by MediaSink::close</summary>
		virtual ~H264VirtualSink(){
			bufferPtr -= sizeof(StartCode4_t);
			this->bufferSize += sizeof(StartCode4_t);
		}

		///<summary></summary>
		///<param name=""></param>
		///<param name=""></param>
		///<returns></returns>
		virtual void AfterGettingFrame(unsigned frameSize, unsigned truncatedBytesCount, struct timeval presentationTime, unsigned durationInMicroseconds){
			static const uint8_t startCode3[] = {0x00, 0x00, 0x01};
			static const uint8_t startCode4[] = {0x00, 0x00, 0x00, 0x01};
			if(frameProcessor){
				auto correctedFrameSize = frameSize;
				auto correctedBufferPtr = bufferPtr;
				if(frameSize<sizeof(startCode4) || memcmp(startCode4, bufferPtr, sizeof(startCode4)) != 0){
					if(frameSize<sizeof(startCode3) || memcmp(startCode3, bufferPtr, sizeof(startCode3)) != 0){
						correctedFrameSize += sizeof(StartCode4_t);
						correctedBufferPtr -= sizeof(StartCode4_t);
					}
				}
				frameProcessor->ProcessFrame(correctedBufferPtr, correctedFrameSize, presentationTime, durationInMicroseconds);
			}
			if (!continuePlaying()) {
				onSourceClosure(this);
			}
		}
	};
}
