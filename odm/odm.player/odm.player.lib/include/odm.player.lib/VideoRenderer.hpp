#pragma once
#include "odm.player.lib/all.h"

namespace onvifmp{

	class VideoRenderer : public IVideoRenderer{
	public:
		typedef function<shared_ptr<IVideoRenderer> (VideoDecoder* sink)> Factory;
		static Factory Create(shared_ptr<VideoBuffer> videoBuffer){
			return ([=](VideoDecoder* decoder)->shared_ptr<IVideoRenderer>{
				if(videoBuffer == nullptr){
					return nullptr;
				}
				return make_shared<VideoRenderer>(videoBuffer);
			});
		}

		VideoRenderer(shared_ptr<VideoBuffer> videoBuffer){
			this->videoBuffer = videoBuffer;
		}

		~VideoRenderer(){
		}

	protected:
		shared_ptr<VideoBuffer> videoBuffer;
		PixelFormat videoBufferPixelFormat;
		virtual void RenderFrame(AVCodecContext* avCodecContext, AVFrame* avFrame){
			if(videoBuffer == nullptr){
				return;
			}
			auto vb = videoBuffer.get();
			auto width = avCodecContext->width;
			auto height = avCodecContext->height;
			auto pixelFormat = avCodecContext->pix_fmt;
			auto stride = avFrame->linesize;
			auto scan0 = avFrame->data;
			auto scaleContext = sws_getContext(width, height, pixelFormat, vb->width, vb->height, vb->pixelFormat, SWS_FAST_BILINEAR,NULL,NULL,NULL);
			if (scaleContext == NULL){
				//TODO: log error
				return;
			}
			__try{
				*vb->signal = 1;
				sws_scale(scaleContext, scan0, stride, 0, height, vb->scan0, vb->stride);
			}__finally{
				sws_freeContext(scaleContext);
			}
		}
		virtual void Dispose(){
			//
		}
	};
}