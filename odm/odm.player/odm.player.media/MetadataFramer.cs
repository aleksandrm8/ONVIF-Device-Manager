using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO.MemoryMappedFiles;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using utils;

namespace odm.player {
	public interface IMetadataReceiver{
		void ProcessMetadata(IntPtr buffer, int size, bool markerBit, int seqNum);
	}

	public class ActionByRef<TArg> : MarshalByRefObject {
		Action<TArg> action;
		public ActionByRef(Action<TArg> action) {
			this.action = action;
		}
		public void Invoke(TArg arg) {
			action(arg);
		}
		public override object InitializeLifetimeService() {
			//
			// Returning null designates an infinite non-expiring lease.
			// We must ensure that RemotingServices.Disconnect() is called 
			// when it's no longer needed otherwise there will be a memory leak.
			//
			return null;
		}
	}

	public class ActionByRef<TArg1, TArg2> : MarshalByRefObject {
		Action<TArg1, TArg2> action;
		public ActionByRef(Action<TArg1, TArg2> action) {
			this.action = action;
		}
		public void Invoke(TArg1 arg1, TArg2 arg2) {
			action(arg1, arg2);
		}
		public override object InitializeLifetimeService() {
			//
			// Returning null designates an infinite non-expiring lease.
			// We must ensure that RemotingServices.Disconnect() is called 
			// when it's no longer needed otherwise there will be a memory leak.
			//
			return null;
		}
	}

	[Serializable]
	public class MetadataFramer : IMetadataReceiver {
		//bool initialized = false;
		//int expectedSeqNum = 0;
		//Byte[] frame = null;
		//int frameCapacity = 0;
		//int farmeOffset = 0;
		ActionByRef<Stream> callback = null;

		public MetadataFramer(Action<Stream> callback) {
			this.callback = new ActionByRef<Stream>(callback);
		}

		//OdmPlayer.MetadataCallback metadataHandler = (buffer, size, markerBit, seqNum) => 

		public unsafe void ProcessMetadata(IntPtr buffer, int size, bool markerBit, int seqNum) {
			//if (!initialized) {
			//	if (markerBit) {
			//		expectedSeqNum = seqNum + 1;
			//		initialized = true;
			//	}
			//	return;
			//}
			//if (expectedSeqNum != seqNum) {
			//	//metadata corrupted
			//	if (!markerBit) {
			//		initialized = false;
			//		frame = null;
			//		return;
			//	}
			//	expectedSeqNum = seqNum + 1;
			//	return;
			//}
			//expectedSeqNum = seqNum + 1;
			//if (!markerBit) {
			//	if (frame == null) {
			//		frameCapacity = 2 * size;
			//		frame = new Byte[frameCapacity];
			//		farmeOffset = size;
			//		Marshal.Copy(buffer, frame, 0, size);
			//	} else {
			//		//ensure capacity
			//		if (frameCapacity < farmeOffset + size) {
			//			//reallocate array
			//			frameCapacity = (farmeOffset + size) * 2;
			//			var newFrame = new byte[frameCapacity];
			//			frame.CopyTo(newFrame, 0);
			//			frame = newFrame;
			//		}
			//		Marshal.Copy(buffer, frame, farmeOffset, size);
			//		farmeOffset += size;
			//	}
			//} else {
			//	if (frame == null) {
			//		frame = new Byte[size];
			//		farmeOffset = size;
			//		frameCapacity = size;
			//		Marshal.Copy(buffer, frame, 0, size);
			//	} else {
			//		//ensure capacity
			//		if (frameCapacity < farmeOffset + size) {
			//			frameCapacity = (farmeOffset + size) * 2;
			//			var newFrame = new byte[frameCapacity];
			//			frame.CopyTo(newFrame, 0);
			//			frame = newFrame;
			//		}
			//		Marshal.Copy(buffer, frame, farmeOffset, size);
			//		farmeOffset += size;
			//	}
				using (var stream = new UnmanagedMemoryStream((byte*)buffer, size)) {
					try {
						callback.Invoke(stream);
					} catch (Exception err) {
						//swallow error
						log.WriteError(err);
					}
				}
				//frame = null;
				//frameCapacity = 0;
				//farmeOffset = 0;
			//}
		}
	}
}
