#pragma once
#include "odm.player.lib/core.h"

namespace onvifmp{
	using namespace std;

	class MetadataCollector : public IFrameProcessor{
	public:
		typedef function<shared_ptr<MetadataCollector> (VirtualSink* sink)> Factory;
		typedef function<void(void* buffer, int size, bool markerBit, int seqNum)> Listener;

		static MetadataCollector::Factory MetadataCollector::Create();
		
		MetadataCollector();
		~MetadataCollector();

		bool Init(VirtualSink* sink);
		void Cleanup();

		///<summary></summary>
		///<param name="factory"></param>
		///<returns></returns>
		void AddListener(Listener listener);

	protected:
		Listener listener;
		VirtualSink* sink;

		virtual void ProcessFrame(unsigned char* framePtr, int frameSize, struct timeval presentationTime, unsigned duration);
		virtual void Dispose();
	};

}