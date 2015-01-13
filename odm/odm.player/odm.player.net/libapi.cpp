#include "odm.player.lib/core.h"
#include "odm.player.lib/Live555.hpp"

//#include "liveMedia.hh"
//#include "BasicUsageEnvironment.hh"
//#include "MediaSink.hh"

#include <Windows.h>
#include<vcclr.h>

#include <string>
//#include <map>
#include <deque>
#include <memory>
#include <functional>
#include <algorithm>

#include "odm.player.lib/Live555.hpp"

namespace odm{
	namespace player{

		using namespace System;
		using namespace System::Text;
		using namespace System::Runtime::InteropServices;
		using namespace utils;
		
		public interface class IPlaybackSession{
			void Close();
		};
		
		public interface class IPlaybackController {
			bool Initialized(IPlaybackSession^ playbackSession);
			void Shutdown();
		};

		public ref class ProxyPlaybackSession: public MarshalByRefObject, public IPlaybackSession{
		public:
			onvifmp::IPlaybackSession* real;
			ProxyPlaybackSession(onvifmp::IPlaybackSession* real){
				this->real = real;
			}
			
			virtual void Close(){
				real->Dispose();
			}
			virtual Object^ InitializeLifetimeService() override{
				//
				// Returning null designates an infinite non-expiring lease.
				// We must ensure that RemotingServices.Disconnect() is called 
				// when it's no longer needed otherwise there will be a memory leak.
				//
				return nullptr;
			}
		};

		class ProxyPlaybackController: public onvifmp::IPlaybackController{
		public:
			typedef bool (*InitializedCallback)(onvifmp::IPlaybackSession*);
			typedef bool (*ShutdownCallback)();

			InitializedCallback m_initialized;
			ShutdownCallback m_shutdown;

			ProxyPlaybackController(InitializedCallback initialized, ShutdownCallback shutdown){
				m_initialized = initialized;
				m_shutdown = shutdown;
			}

			virtual bool Initialized(onvifmp::IPlaybackSession* session){
				return m_initialized(session);
			}

			virtual void Shutdown(){
				m_shutdown();
			}
		};

		public ref class Live555{
		public:
			Live555(VideoBuffer^ videoBuffer, IMetadataReceiver^ metadataReceiver){
				try{
					Init(videoBuffer, metadataReceiver);
				}catch(void*){
					Cleanup();
					throw;
				}
				//errorHandler = gcnew ErrorCallback(this, &OdmPlayer::HandleError);
				//auto handleError = (onvifmp::ErrorCallback)(void*) Marshal::GetFunctionPointerForDelegate(
				//	errorHandler
				//);
				//logHandler = gcnew LoggerCallback(this, &OdmPlayer::ProcessLogInfo);
				//auto processLogInfo = (onvifmp::LogCallback)(void*) Marshal::GetFunctionPointerForDelegate(
				//	logHandler
				//);
				////metadataHandler = gcnew MetadataCallback(this, &OdmPlayer::HandleMetadata);
				//m_nativePlayer = new OnvifInstance(handleError, processLogInfo);
				//sync = gcnew Object();
				//streamUrl = nullptr;
			}
			~Live555(){
				log::WriteInfo("Live555::~Live555(...)");
				Cleanup();
			}
			void Init(VideoBuffer^ videoBuffer, IMetadataReceiver^ metadataReceiver){
				log::WriteInfo("Live555::Init(...)");
				playbackController = nullptr;
				onvifmpInstance = new onvifmp::Live555();
				if(onvifmpInstance == nullptr){
					throw gcnew Exception("failed to create Live555 native instance");
				}
				
				this->metadataReceiver = metadataReceiver;
				if(metadataReceiver != nullptr){
					void(*metadataListener)(void*, int, bool, int) = nullptr;
					metadataCallback = gcnew MetadataCallback_t(this, &Live555::MetadataCallback);
					metadataListener = (void(__cdecl*)(void*, int, bool, int)) Marshal::GetFunctionPointerForDelegate(
						metadataCallback
					).ToPointer();
					onvifmpInstance->SetMetadataOutput(metadataListener);
				}

				if(videoBuffer != nullptr){
					auto onvifmpVideoBuffer = std::make_shared<onvifmp::VideoBuffer>();
					
					if(videoBuffer->pixelFormat == PixFrmt::rgb24){
						onvifmpVideoBuffer->pixelFormat = PixelFormat::PIX_FMT_RGB24;
					}else if(videoBuffer->pixelFormat == PixFrmt::argb32){
						onvifmpVideoBuffer->pixelFormat = PixelFormat::PIX_FMT_ARGB;
					}else if(videoBuffer->pixelFormat == PixFrmt::bgra32){
						onvifmpVideoBuffer->pixelFormat = PixelFormat::PIX_FMT_BGRA;
					}else if(videoBuffer->pixelFormat == PixFrmt::bgr24){
						onvifmpVideoBuffer->pixelFormat = PixelFormat::PIX_FMT_BGR24;
					}else{
						throw gcnew Exception("invalid pixel format");
					}
					onvifmpVideoBuffer->width = videoBuffer->width;
					onvifmpVideoBuffer->height = videoBuffer->height;
					onvifmpVideoBuffer->stride[0] = videoBuffer->stride;
					videoBufferLock = videoBuffer->Lock();
					onvifmpVideoBuffer->scan0[0] = static_cast<uint8_t*>(videoBufferLock->value->scan0Ptr.ToPointer());
					//h->
					for(int i = 1; i<4; ++i){
						onvifmpVideoBuffer->stride[i] = 0;
						onvifmpVideoBuffer->scan0[i] = nullptr;
					}
					onvifmpVideoBuffer->signal = static_cast<uint8_t*>(videoBufferLock->value->signalPtr.ToPointer());
					//onvifmpVideoBuffer.width = videoBuffer
					onvifmpInstance->SetVideoOutput(onvifmpVideoBuffer);
				}
				
				//onvifmpVideoBuffer.width = videoBuffer
				
			}
			void Cleanup(){
				if(onvifmpInstance != nullptr){
					delete onvifmpInstance;
					onvifmpInstance = nullptr;
				}
				if(videoBufferLock != nullptr){
					delete videoBufferLock;
					videoBufferLock = nullptr;
				}
				/*if(onvifmpVideoBuffer != nullptr){
					delete onvifmpVideoBuffer;
					onvifmpVideoBuffer = nullptr;
				}*/
				metadataReceiver = nullptr;
			}
			
			[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
			delegate bool PlaybackSessionInitialized_t(onvifmp::IPlaybackSession* session);
			PlaybackSessionInitialized_t^ playbackSessionInitialized;
			bool PlaybackSessionInitialized(onvifmp::IPlaybackSession* session){
				return playbackController->Initialized(gcnew ProxyPlaybackSession(session));
			}
			
			[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
			delegate void PlaybackSessionShutdown_t();
			PlaybackSessionShutdown_t^ playbackSessionShutdown;
			void PlaybackSessionShutdown(){
				return playbackController->Shutdown();
			}

			[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
			delegate void MetadataCallback_t(void* buffer, int size, bool markerBit, int seqNum);
			MetadataCallback_t^ metadataCallback;
			//void*, int, int, int
			void MetadataCallback(void* buffer, int size, bool markerBit, int seqNum){
				//Console::WriteLine("MetadataCallback");
				if(metadataReceiver != nullptr){
					try{
						metadataReceiver->ProcessMetadata(IntPtr(buffer), size, markerBit, seqNum);
					}catch(Exception^){
						//TODO: log error
					}
				}
			}

			//void Play(String^ streamUrl, IPlaybackController^ playbackController){
			//	Play(streamUrl, nullptr, playbackController);
			//}

			void Play(MediaStreamInfo^ mediaStreamInfo, IPlaybackController^ playbackController){
				//streamUrl
				IntPtr urlIntPtr = IntPtr::Zero;
				IntPtr userNameIntPtr = IntPtr::Zero;
				IntPtr passwordIntPtr = IntPtr::Zero;
				auto msi = std::make_shared< onvifmp::MediaStreamInfo >();
				if(mediaStreamInfo->transport == MediaStreamInfo::Transport::Udp){
					msi->transport = onvifmp::StreamTransport::Udp;
				}else if (mediaStreamInfo->transport == MediaStreamInfo::Transport::Tcp){
					msi->transport = onvifmp::StreamTransport::Tcp;
				}else if (mediaStreamInfo->transport == MediaStreamInfo::Transport::Http){
					msi->transport = onvifmp::StreamTransport::Http;
				}else{
					throw gcnew Exception("unsupported transport");
				}
				try{
					urlIntPtr = Marshal::StringToHGlobalAnsi(mediaStreamInfo->url);
					auto url = static_cast<char*>(urlIntPtr.ToPointer());
					Authenticator* authenticator = nullptr;
					if(mediaStreamInfo->userNameToken != nullptr){
						auto userNameToken = mediaStreamInfo->userNameToken;
						
						auto userNameBytes = Encoding::UTF8->GetBytes(userNameToken->userName);
						userNameIntPtr = Marshal::AllocHGlobal(userNameBytes->Length+1);
						Marshal::Copy(userNameBytes, 0, userNameIntPtr, userNameBytes->Length);
						auto userName = static_cast<char*>(userNameIntPtr.ToPointer());
						userName[userNameBytes->Length] = 0;
						
						
						auto passwordBytes = Encoding::UTF8->GetBytes(userNameToken->password);
						passwordIntPtr = Marshal::AllocHGlobal(passwordBytes->Length+1);
						Marshal::Copy(passwordBytes, 0, passwordIntPtr, passwordBytes->Length);
						auto password = static_cast<char*>(passwordIntPtr.ToPointer());
						password[passwordBytes->Length] = 0;

						authenticator = new Authenticator(userName, password);
					}
					//auto proxyController = std::make_shared<ProxyPlaybackController>();
					this->playbackController = playbackController;
					playbackSessionInitialized = gcnew PlaybackSessionInitialized_t(this, &Live555::PlaybackSessionInitialized);
					auto sessionInitialized = (ProxyPlaybackController::InitializedCallback)Marshal::GetFunctionPointerForDelegate(
						playbackSessionInitialized
					).ToPointer();

					playbackSessionShutdown = gcnew PlaybackSessionShutdown_t(this, &Live555::PlaybackSessionShutdown);
					auto sessionSutdown = (ProxyPlaybackController::ShutdownCallback)Marshal::GetFunctionPointerForDelegate(
						playbackSessionShutdown
					).ToPointer();

					auto proxyController = new ProxyPlaybackController(
						sessionInitialized,
						sessionSutdown
					);

					
					msi->url = url;
					msi->authenticator = authenticator;

					onvifmpInstance->Run(msi.get(), proxyController);
					Cleanup();
				}finally{
					if(urlIntPtr != IntPtr::Zero){
						Marshal::FreeHGlobal(urlIntPtr);
					}
				}
			}
			//[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
			//delegate void ErrorCallback(const char* msg);
			//[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
			//delegate void LoggerCallback(const char *aMsg, const char *source, LogType aType);
			//[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
			//delegate void MetadataCallback(IntPtr buffer, int size, bool markerBit, int seqNum);
		protected:
			//ErrorCallback^ errorHandler;
			//LoggerCallback^ logHandler;
			//MetadataCallback^ metadataHandler;
		protected:
			//Object^ sync;
			//OnvifInstance *m_nativePlayer;
			//String^ streamUrl;
			onvifmp::Live555* onvifmpInstance;
			VideoBuffer^ videoBuffer;
			//onvifmp::VideoBuffer* onvifmpVideoBuffer;
			IMetadataReceiver^ metadataReceiver;

			IPlaybackController^ playbackController;
			utils::IDisposable<MappedData^>^ videoBufferLock;

			
			//void HandleError(const char* msg){
			//	dbg::Error(gcnew String(msg));
			//}
			//void ProcessLogInfo(const char *msg, const char *source, LogType type){
			//	dbg::Error(gcnew String(msg));
			//}
			//void HandleMetadata(IntPtr buffer, UInt32 size){
				//dbg::Info("metadata arrived");
			//}
		public:
			//void Play(String^ url, VideoBuffer^ videoBuffer, MetadataCallback^ metadataCallback = nullptr){
			//	if(String::IsNullOrEmpty(url)){
			//		throw gcnew ArgumentException("invalid url");
			//	}
			//	OnvifmpPixelFormat pixelFormat;
			//	if(videoBuffer->pixelFormat == PixFrmt::rgb32){
			//		pixelFormat = OnvifmpPixelFormat::ONVIFMP_PF_RGB32;
			//	}if(videoBuffer->pixelFormat == PixFrmt::rgba32){
			//		pixelFormat = OnvifmpPixelFormat::ONVIFMP_PF_RGB32;
			//	}else{
			//		throw gcnew Exception("unsupported pixel format");
			//	}
			//	
			//	if(streamUrl != nullptr){
			//		throw gcnew Exception("playback already started");
			//	}
			//	auto width = videoBuffer->width;
			//	auto height = videoBuffer->height;
			//	auto stride = videoBuffer->stride;
			//	IntPtr urlIntPtr = IntPtr::Zero;
			//	IntPtr mappedFileNameIntPtr = IntPtr::Zero;
			//	try{
			//		urlIntPtr = Marshal::StringToHGlobalAnsi(url);
			//		//mappedFileNameIntPtr = Marshal::StringToHGlobalAnsi(mappedFileName);
			//		auto aUrl = static_cast<char*>(urlIntPtr.ToPointer());
			//		//auto aMappedFileName = static_cast<char*>(mappedFileNameIntPtr.ToPointer());
			//		metadataHandler = metadataCallback;
			//		onvifmp_meta_callback handleMetadata = NULL;
			//		if(metadataHandler!=nullptr){
			//			handleMetadata = (onvifmp_meta_callback)(void*) Marshal::GetFunctionPointerForDelegate(
			//				metadataHandler
			//			);
			//		}
			//		if(!m_nativePlayer->StartParsing(aUrl, width, height, stride, aMappedFileName, pixelFormat, handleMetadata, 0)){
			//			throw gcnew Exception("failed to start play");
			//		}
			//	}finally{
			//		if(urlIntPtr != IntPtr::Zero){
			//			Marshal::FreeHGlobal(urlIntPtr);
			//		}
			//		if(mappedFileNameIntPtr != IntPtr::Zero){
			//			Marshal::FreeHGlobal(mappedFileNameIntPtr);
			//		}
			//	}
			//}
			//void Stop(){
			//	if(streamUrl != nullptr){
			//		IntPtr streamUrlIntPtr = IntPtr::Zero;
			//		try{
			//			auto aUrl = static_cast<char*>(streamUrlIntPtr.ToPointer());
			//			m_nativePlayer->StopParsing(aUrl);
			//		}finally{
			//			if(streamUrlIntPtr != IntPtr::Zero){
			//				Marshal::FreeHGlobal(streamUrlIntPtr);
			//			}
			//		}
			//	}
			//}
			//void SetSilentMode(bool silentPlayback){
			//	if(streamUrl != nullptr){
			//		IntPtr streamUrlIntPtr = IntPtr::Zero;
			//		try{
			//			auto aUrl = static_cast<char*>(streamUrlIntPtr.ToPointer());
			//			m_nativePlayer->SetSilentMode(aUrl, silentPlayback);
			//		}finally{
			//			if(streamUrlIntPtr != IntPtr::Zero){
			//				Marshal::FreeHGlobal(streamUrlIntPtr);
			//			}
			//		}
			//	}
			//}

			//void StartRecord(String^ filePath){
			//	if(streamUrl != nullptr){
			//		IntPtr streamUrlIntPtr = IntPtr::Zero;
			//		IntPtr filePathIntPtr = IntPtr::Zero;
			//		try{
			//			auto aUrl = static_cast<char*>(streamUrlIntPtr.ToPointer());
			//			auto aFilePath = static_cast<char*>(filePathIntPtr.ToPointer());
			//			m_nativePlayer->StartRecord(aUrl, aFilePath);
			//		}finally{
			//			if(streamUrlIntPtr != IntPtr::Zero){
			//				Marshal::FreeHGlobal(streamUrlIntPtr);
			//			}
			//			if(filePathIntPtr != IntPtr::Zero){
			//				Marshal::FreeHGlobal(filePathIntPtr);
			//			}
			//		}
			//	}
			//}
			//void StopRecord(){
			//	if(streamUrl != nullptr){
			//		IntPtr streamUrlIntPtr = IntPtr::Zero;
			//		try{
			//			auto aUrl = static_cast<char*>(streamUrlIntPtr.ToPointer());
			//			m_nativePlayer->StopRecord(aUrl);
			//		}finally{
			//			if(streamUrlIntPtr != IntPtr::Zero){
			//				Marshal::FreeHGlobal(streamUrlIntPtr);
			//			}
			//		}
			//	}
			//}
			//static OdmPlayer^ Create(){
			//	return gcnew OdmPlayer();
			//}
		
			//~OdmPlayer(){
			//	delete m_nativePlayer;
			//	m_nativePlayer = NULL;
			//}
		};
	}
}
