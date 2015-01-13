#pragma once
#include "odm.player.lib/core.h"
#include "odm.player.lib/MetadataCollector.hpp"
#include "odm.player.lib/VirtualSink.hpp"

namespace onvifmp{

	MetadataCollector::Factory MetadataCollector::Create(){
		return ([=](VirtualSink* sink)->shared_ptr<MetadataCollector>{
			auto instance = make_shared<MetadataCollector>();
			if(!instance->Init(sink)){
				return nullptr;
			}
			return instance;
		});
	}

	MetadataCollector::MetadataCollector(){
		listener = nullptr;
	}

	MetadataCollector::~MetadataCollector(){
		Cleanup();
	}

	bool MetadataCollector::Init(VirtualSink* sink){
		//if it has been initialized before, we should do cleanup first
		Cleanup();

		if(sink == nullptr){
			Cleanup();
			return false;
		}

		this->sink = sink;
		return true;
	}

	void MetadataCollector::Cleanup(){
	}

	void MetadataCollector::AddListener(MetadataCollector::Listener listener){
		this->listener = listener;
	}

	void MetadataCollector::ProcessFrame(unsigned char* framePtr, int frameSize, struct timeval presentationTime, unsigned duration){
		auto rtpSource = (RTPSource*)sink->source();
		if(rtpSource == nullptr || !((FramedSource*)rtpSource)->isRTPSource()){
			return;
		}
		if(listener!=nullptr){
			listener(framePtr, frameSize, rtpSource->curPacketMarkerBit(), rtpSource->curPacketRTPSeqNum());
		}
	}
	
	void MetadataCollector::Dispose(){
		//
	}

	
}