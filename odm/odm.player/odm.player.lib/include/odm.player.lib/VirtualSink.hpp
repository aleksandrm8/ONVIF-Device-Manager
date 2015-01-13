#pragma once
#include "odm.player.lib/core.h"

namespace onvifmp{
	using namespace std;

	class VirtualSink : public MediaSink{
	public:
		///<summary>
		///create new VirtualSink object
		///instance must be released by calling MediaSink::close
		///</summary>
		///<param name="usageEnvironment">usage environment</param>
		///<param name="bufferSize">max packet payload</param>
		///<returns>pointer to created VirtualSink object, if successed or nullptr otherwise</returns>
		static VirtualSink* CreateNew(UsageEnvironment& usageEnvironment, unsigned bufferSize = (4*1024*1024)){
			try{
				return new VirtualSink(usageEnvironment, bufferSize);
			}catch(void*){
				//TODO: log error
				return nullptr;
			}
		}

		///<summary></summary>
		///<param name="factory"></param>
		///<returns></returns>
		void AddFrameProcessor(IFrameProcessorFactory factory){
			if(frameProcessor){
				frameProcessor->Dispose();
				frameProcessor = NULL;
			}
			if(factory!=nullptr){
				frameProcessor = factory(this);
			}
		}

	protected:
		unsigned char *bufferPtr;
		unsigned int bufferSize;
		shared_ptr<IFrameProcessor> frameProcessor;
		//HANDLE mEvent;

		///<summary>constructor</summary>
		///<param name="usageEnvironment">usage environment</param>
		///<param name="bufferSize">max packet payload</param>
		VirtualSink(UsageEnvironment& usageEnvironment, unsigned bufferSize): MediaSink(usageEnvironment){
			if (bufferSize <= 0){
				throw "invalid buffer size";
			}
			bufferPtr = new unsigned char[bufferSize];
			this->bufferSize = bufferSize;
		}

		///<summary>destructor, called by MediaSink::close</summary>
		virtual ~VirtualSink(){
			if(bufferPtr != NULL) {
				delete []bufferPtr;
				bufferPtr = NULL;
				bufferSize = 0;
			}
		}

		///<summary></summary>
		///<param name=""></param>
		///<returns></returns>
		virtual Boolean continuePlaying() {
			if (fSource == NULL) {
				//has been stopped
				return False;
			}
			typedef struct proxy{
				static void AfterGettingFrame(void* clientData, unsigned frameSize, unsigned truncatedBytesCount, struct timeval presentationTime, unsigned durationInMicroseconds){
					auto sink = static_cast<VirtualSink*>(clientData);
					sink->AfterGettingFrame(frameSize, truncatedBytesCount, presentationTime, durationInMicroseconds);
				}
			};
			fSource->getNextFrame(bufferPtr, bufferSize, proxy::AfterGettingFrame, this, onSourceClosure, this);
			return True;
		}

		///<summary></summary>
		///<param name=""></param>
		///<param name=""></param>
		///<returns></returns>
		virtual void AfterGettingFrame(unsigned frameSize, unsigned truncatedBytesCount, struct timeval presentationTime, unsigned durationInMicroseconds){
			if(frameProcessor){
				frameProcessor->ProcessFrame(bufferPtr, frameSize, presentationTime, durationInMicroseconds);
			}
			if (!continuePlaying()) {
				onSourceClosure(this);
			}
		}
	};
}
