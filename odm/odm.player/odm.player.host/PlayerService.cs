using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.Text;

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading;

using utils;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Media;

namespace odm.player {

	//public class MetadataFramer {
	//    public static OdmPlayer.MetadataCallback Create(Action<byte[],int> callback) {
	//        var initialized = false;
	//        var expectedSeqNum = 0;
	//        Byte[] frame = null;
	//        int frameCapacity = 0;
	//        int farmeOffset = 0;

	//        OdmPlayer.MetadataCallback metadataHandler = (buffer, size, markerBit, seqNum) => {
	//            if (!initialized) {
	//                if (markerBit) {
	//                    expectedSeqNum = seqNum + 1;
	//                    initialized = true;
	//                }
	//                return;
	//            }
	//            if (expectedSeqNum != seqNum) {
	//                //metadata corrupted
	//                if (!markerBit) {
	//                    initialized = false;
	//                    frame = null;
	//                    return;
	//                }
	//                expectedSeqNum = seqNum + 1;
	//                return;
	//            }
	//            expectedSeqNum = seqNum + 1;
	//            if (!markerBit) {
	//                if (frame == null) {
	//                    frameCapacity = 2 * size;
	//                    frame = new Byte[frameCapacity];
	//                    farmeOffset = size;
	//                    Marshal.Copy(buffer, frame, 0, size);
	//                } else {
	//                    //ensure capacity
	//                    if (frameCapacity < farmeOffset + size) {
	//                        //reallocate array
	//                        frameCapacity = (farmeOffset + size) * 2;
	//                        var newFrame = new byte[frameCapacity];
	//                        frame.CopyTo(newFrame, 0);
	//                        frame = newFrame;
	//                    }
	//                    Marshal.Copy(buffer, frame, farmeOffset, size);
	//                    farmeOffset += size;
	//                }
	//            } else {
	//                if (frame == null) {
	//                    frame = new Byte[size];
	//                    farmeOffset = size;
	//                    frameCapacity = size;
	//                    Marshal.Copy(buffer, frame, 0, size);
	//                } else {
	//                    //ensure capacity
	//                    if (frameCapacity < farmeOffset + size) {
	//                        frameCapacity = (farmeOffset + size) * 2;
	//                        var newFrame = new byte[frameCapacity];
	//                        frame.CopyTo(newFrame, 0);
	//                        frame = newFrame;
	//                    }
	//                    Marshal.Copy(buffer, frame, farmeOffset, size);
	//                    farmeOffset += size;
	//                }
	//                callback(frame, farmeOffset);
	//                frame = null;
	//                frameCapacity = 0;
	//                farmeOffset = 0;
	//            };
	//        };
	//        return metadataHandler;
	//    }
	//}


	//[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
	//class PlayerService : IPlayerTest {
	//    private Action m_shutdownHandler;
	//    private object m_gate = new object();
	//    private bool isPlaying = false;
	//    private Action m_stopHandler = null;
	//    Queue<string> m_queue = new Queue<string>();

	//    private Action<string, int> m_startRecordAction = null;
	//    private Action m_stopRecordAction = null;

	//    private string m_videoBufferMapName = null;
	//    private int m_videoBufferWidth;
	//    private int m_videoBufferHeight;
	//    private int m_videoBufferStride;
	//    PixelFormat m_videoBufferPixelFormat;
	//    private string m_playingUri = null;
	//    IScheduler m_scheduler = null;

	//    List<IPlayerCallbacks> m_subscribers = new List<IPlayerCallbacks>();

	//    public PlayerService(IScheduler scheduler, Action shutdownHandler) {
	//        if (scheduler == null) {
	//            throw new ArgumentNullException("scheduler");
	//        }
	//        m_scheduler = scheduler;
	//        m_shutdownHandler = shutdownHandler;
	//    }

	//    public void SetVideoBuffer(string mapName, int width, int height, int stride, PixelFormat pixelFormat) {
	//        log.WriteInfo("PlayerService::SetVideoBuffer(...)");
	//        var restartPlayback = false;
	//        lock (m_gate) {
	//            if (isPlaying) {
	//                restartPlayback = true;
	//            }
	//        }

	//        if (restartPlayback && m_stopHandler != null) {
	//            m_stopHandler();
	//            m_stopHandler = null;
	//        }

	//        m_videoBufferMapName = mapName;
	//        m_videoBufferWidth = width;
	//        m_videoBufferHeight = height;
	//        m_videoBufferStride = stride;
	//        m_videoBufferPixelFormat = pixelFormat;

	//        if (restartPlayback) {
	//            try {
	//                PlayImpl();
	//            } catch (Exception err) {
	//                lock (m_gate) {
	//                    isPlaying = false;
	//                }
	//                throw new FaultException(err.Message);
	//            }
	//        }
	//    }

		//NativePlayer.onvifmp_meta_callback m_MetadataReceivedCallback = null ;
		//NativePlayer.onvifmp_error_handler m_ErrorOccurredCallback = null;
		//NativePlayer.onvifmp_log_handler m_LoggerCallback = null;


		//private static VideoBufferDescription.PixelFormat ToNativePixelFormat(PixelFormat pixelFormat) {
		//    switch (pixelFormat) {
		//        case PixelFormat.Format32bppArgb:
		//        case PixelFormat.Format32bppRgb:
		//            return VideoBufferDescription.PixelFormat.RGB32;
		//    }
		//    throw new Exception("invalid pixel format");
		//}

		//public static TraceEventType ToTraceEventType(NativePlayer.LogType type) {
		//    switch (type) {
		//        case NativePlayer.LogType.LOG_ERROR:
		//            return TraceEventType.Error;
		//        case NativePlayer.LogType.LOG_WARNING:
		//            return TraceEventType.Warning;
		//        case NativePlayer.LogType.LOG_INFORMATION:
		//            return TraceEventType.Information;

		//    }
		//    return TraceEventType.Error;
		//}

	//    private void PlayImpl() {
	//        log.WriteInfo("PlayerService::PlayImpl()");
	//        var playingUri = m_playingUri;
	//        Int32 parseErr = -1;
	//        //IntPtr handle = IntPtr.Zero;
	//        OdmPlayer player = null;
	//        var subscr = Disposable.Create(() => {
	//            player.Stop();
	//            player.Dispose();
	//            player = null;
	//        });

	//        m_stopHandler = () => {
	//            subscr.Dispose();
	//        };

	//        m_startRecordAction = (filePath, frameRate) => {
	//            player.StartRecord(filePath);
	//        };

	//        m_stopRecordAction = () => {
	//            player.StopRecord();
	//        };

	//        //m_ErrorOccurredCallback = errStr => {
	//        //    log.WriteEvent(errStr, "odm-player", TraceEventType.Critical);
	//        //    subscr.Dispose();
	//        //};

	//        //m_LoggerCallback = (msg, src, type) => {
	//        //    TraceEventType evtType = ToTraceEventType(type);
	//        //    log.WriteEvent(msg, src, evtType);				
	//        //};

	//        try {
	//            player = OdmPlayer.Create();
	//            //VideoBufferDescription.PixelFormat pf = VideoBufferDescription.PixelFormat.RGB32;
	//            //if (String.IsNullOrEmpty(m_videoBufferMapName)) {
	//            //    m_videoBufferMapName = null;
	//            //} else {
	//            //    pf = ToNativePixelFormat(m_videoBufferPixelFormat);
	//            //}
	//            var bufferDescription = new VideoBuffer(
	//                m_videoBufferWidth, m_videoBufferHeight
	//            );

	//            var metadataCallback = MetadataFramer.Create((buffer,size)=>{
	//                NotifyMetadataReceived(buffer, size);
	//                //using (var stream = new MemoryStream(buffer, 0, size, false)) {
	//                //    using (var reader = XmlReader.Create(stream)) {
	//                //        var xml = XElement.Load(reader);
	//                //        if (size>1024) {
	//                //            using (var witer = new XmlTextWriter(Console.Out)) {
	//                //                witer.Formatting = Formatting.Indented;
	//                //                xml.WriteTo(witer);
	//                //            }
	//                //        }
	//                //    }
	//                //}
	//            });

	//            player.Play(m_playingUri, bufferDescription, m_videoBufferMapName, metadataCallback);
	//        } catch (Exception err) {
	//            dbg.Error(err);
	//            subscr.Dispose();
	//            throw;
	//        }
	//    }

	//    public void Play(string uri) {
	//        if (String.IsNullOrEmpty(uri)) {
	//            throw new FaultException("invalid uri");
	//        }
	//        lock (m_gate) {
	//            if (isPlaying) {
	//                throw new FaultException("already playing");
	//            }
	//            isPlaying = true;
	//        }
	//        log.WriteInfo(String.Format("PlayerService::Play({0})", uri));

	//        m_playingUri = uri;
	//        try {
	//            PlayImpl();
	//            log.WriteInfo(String.Format("Playing {0}", uri));
	//        } catch (Exception err) {
	//            lock (m_gate) {
	//                isPlaying = false;
	//            }
	//            throw new FaultException(err.Message);
	//        }
	//    }

	//    public void Stop() {
	//        lock (m_gate) {
	//            if (!isPlaying) {
	//                throw new FaultException("not playing");
	//            }
	//            isPlaying = false;
	//        }
	//        log.WriteInfo("PlayerService::Stop()");
	//        if (m_stopHandler != null) {
	//            m_stopHandler();
	//            m_stopHandler = null;
	//        }
	//    }

	//    public void Subscribe() {
	//        log.WriteInfo("PlayerService::Subscribe()");
	//        var subscriber = OperationContext.Current.GetCallbackChannel<IPlayerCallbacks>();
	//        lock (m_subscribers) {
	//            if (m_subscribers.Contains(subscriber)) {
	//                throw new FaultException("already subscribed");
	//            }
	//            m_subscribers.Add(subscriber);
	//        }
	//    }

	//    public void Unsubscribe() {
	//        log.WriteInfo("PlayerService::Unsubscribe()");
	//        var subscriber = OperationContext.Current.GetCallbackChannel<IPlayerCallbacks>();
	//        lock (m_subscribers) {
	//            if (!m_subscribers.Remove(subscriber)) {
	//                throw new FaultException("subscription does not exist");
	//            }
	//        }
	//    }

	//    public void NotifyMetadataReceived(byte[] buffer, int size) {
	//        log.WriteInfo("PlayerService::NotifyMetadataReceived(...)");
	//        IPlayerCallbacks[] subscribers = null;
	//        lock (m_subscribers) {
	//            subscribers = m_subscribers.ToArray();
	//        }
	//        foreach (var s in subscribers) {
	//            try {
	//                var stream = new MemoryStream(buffer, 0, size, false);
	//                //using(var stream = new MemoryStream(buffer,0,size,false)){
	//                    s.MetadataReceived(stream);
	//                //}
	//            } catch (Exception err) {
	//                dbg.Error(err);
	//                //swallow error
	//            }
	//        }
	//    }

	//    public void NotifyLogMessage(LogMessage logMessage) {
	//        IPlayerCallbacks[] subscribers = null;
	//        lock (m_subscribers) {
	//            subscribers = m_subscribers.ToArray();
	//        }
	//        foreach (var s in subscribers) {
	//            try {
	//                s.LogMessageAcquired(logMessage);
	//            } catch {
	//                //swallow error
	//            }
	//        }
	//    }

	//    public void Shutdown() {
	//        log.WriteInfo("PlayerService::Shutdown()");
	//        if (m_shutdownHandler != null) {
	//            m_shutdownHandler();
	//        }
	//    }

	//    public string CreateMetadataPullPoint() {
	//        throw new NotImplementedException();
	//    }

	//    public string PullMetadata(string pullPoint) {
	//        log.WriteInfo("PullMetadata");
	//        return "hello!";
	//    }

	//    public IAsyncResult BeginPullMetadata(string pullPoint, AsyncCallback callback, object asyncState) {
	//        log.WriteInfo("BeginPullMetadata");

	//        var ar = new MyAsyncResult<string>(callback, asyncState);

	//        var subscription = Scheduler.ThreadPool.Schedule(
	//            TimeSpan.FromSeconds(1000000),
	//            () => {
	//                ar.Complete(() => {
	//                    return PullMetadata(pullPoint);
	//                }, false);
	//            }
	//        );

	//        return ar;
	//        //Func<string, string> func = PullMetadata;
	//        //return func.BeginInvoke(pullPoint, callback, asyncState);
	//    }

	//    public string EndPullMetadata(IAsyncResult asyncResult) {
	//        log.WriteInfo("EndPullMetadata");
	//        var ar = (MyAsyncResult<string>)asyncResult;
	//        return ar.EndHandler();
	//        //AsyncResult ar = (AsyncResult)asyncResult;
	//        //Func<string, string> func = (Func<string, string>)ar.AsyncDelegate;
	//        //return func.EndInvoke(asyncResult);
	//    }


	//    public void StartRecord(string filePath, int frameRate) {
	//        if (m_startRecordAction != null) {
	//            m_startRecordAction(filePath, frameRate);
	//        }
	//    }

	//    public void StopRecord() {
	//        if (m_stopRecordAction != null) {
	//            m_stopRecordAction();
	//        }
	//    }

	//    public IAsyncResult BeginStartRecord(string filePath, int frameRate, AsyncCallback callback, object asyncState) {
	//        throw new NotImplementedException();
	//    }

	//    public void EndStartRecord(IAsyncResult result) {
	//        throw new NotImplementedException();
	//    }

	//    public IAsyncResult BeginStopRecord(AsyncCallback callback, object asyncState) {
	//        throw new NotImplementedException();
	//    }

	//    public void EndStopRecord(IAsyncResult result) {
	//        throw new NotImplementedException();
	//    }


	//    public IAsyncResult BeginSetVideoBuffer(string mapName, int width, int height, int stride, PixelFormat pixelFormat, AsyncCallback callback, object asyncState) {
	//        throw new NotImplementedException();
	//    }

	//    public void EndSetVideoBuffer(IAsyncResult result) {
	//        throw new NotImplementedException();
	//    }

	//    public IAsyncResult BeginSubscribe(AsyncCallback callback, object asyncState) {
	//        throw new NotImplementedException();
	//    }

	//    public void EndSubscribe(IAsyncResult result) {
	//        throw new NotImplementedException();
	//    }

	//    public IAsyncResult BeginUnsubscribe(AsyncCallback callback, object asyncState) {
	//        throw new NotImplementedException();
	//    }

	//    public void EndUnsubscribe(IAsyncResult result) {
	//        throw new NotImplementedException();
	//    }

	//    public IAsyncResult BeginPlay(string uri, AsyncCallback callback, object asyncState) {
	//        throw new NotImplementedException();
	//    }

	//    public void EndPlay(IAsyncResult result) {
	//        throw new NotImplementedException();
	//    }

	//    public IAsyncResult BeginStop(AsyncCallback callback, object asyncState) {
	//        throw new NotImplementedException();
	//    }

	//    public void EndStop(IAsyncResult result) {
	//        throw new NotImplementedException();
	//    }

	//    public IAsyncResult BeginShutdown(AsyncCallback callback, object asyncState) {
	//        throw new NotImplementedException();
	//    }

	//    public void EndShutdown(IAsyncResult result) {
	//        throw new NotImplementedException();
	//    }
	//};


	//public class MyAsyncResult<T> : IAsyncResult {
	//    // Fields
	//    private Func<T> m_endHandler;
	//    private object m_asyncState;
	//    private AsyncCallback m_callback;
	//    private bool m_completedSynchronously = false;
	//    private bool m_isCompleted = false;
	//    private object m_gate = new object();
	//    private ManualResetEvent m_waitHandle = new ManualResetEvent(false);

	//    // Methods
	//    public MyAsyncResult(AsyncCallback callback, object state) {
	//        m_callback = callback;
	//        m_asyncState = state;
	//    }

	//    internal void Complete(Func<T> endHandler, bool synchronously) {
	//        lock (this.m_gate) {
	//            if (m_isCompleted) {
	//                log.WriteError("MyAsyncResult::Complete - operation already comleted.");
	//                return;
	//            }
	//            m_isCompleted = true;
	//            m_endHandler = endHandler;
	//            m_completedSynchronously = synchronously;
	//            if (m_waitHandle != null) {
	//                m_waitHandle.Set();
	//            }
	//        }
	//        if (m_callback != null) {
	//            try {
	//                m_callback(this);
	//            } catch (Exception err) {
	//                dbg.Error(err);
	//            }
	//        }
	//    }
	//    public T EndHandler() {
	//        if (m_endHandler != null) {
	//            log.WriteInfo("MyAsyncResult::EndHandler()");
	//            return m_endHandler();
	//        }
	//        return default(T);
	//    }

	//    // Properties
	//    public WaitHandle AsyncWaitHandle {
	//        get {
	//            lock (this.m_gate) {
	//                if (m_waitHandle == null) {
	//                    m_waitHandle = new ManualResetEvent(m_isCompleted);
	//                }
	//            }
	//            return m_waitHandle;
	//        }
	//    }
	//    public bool CompletedSynchronously {
	//        get {
	//            return m_completedSynchronously;
	//        }
	//    }
	//    public bool IsCompleted {
	//        get {
	//            return m_isCompleted;
	//        }
	//    }
	//    public object AsyncState {
	//        get {
	//            return m_asyncState;
	//        }
	//    }
	//};
}


