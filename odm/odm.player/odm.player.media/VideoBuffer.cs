using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO.MemoryMappedFiles;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Threading;
using utils;

namespace odm.player {
	public class MappedData {
		public MappedData(IntPtr handle) { 
			signalPtr = handle;
			scan0Ptr = handle + VideoBuffer.EXTRA_DATA_SIZE;
		}
		public readonly IntPtr signalPtr;
		public readonly IntPtr scan0Ptr;
		public byte signal { 
			get {
				return Marshal.ReadByte(signalPtr);
			}
			set {
				Marshal.WriteByte(signalPtr, value);
			}
		}
	}
	[Serializable]
	public class VideoBuffer : IDisposable, IDeserializationCallback {
		public const int BUFFER_PADDING_SIZE = 16;
		public const int EXTRA_DATA_SIZE = 16;
		public VideoBuffer(int width, int height) {
			this.memoryMappedFileName = Guid.NewGuid().ToString();
			this.width = width;
			this.height = height;
			this.pixelFormat = PixFrmt.rgb24;
			this.stride = ((width * pixelFormat.bitsPerPixel + 7) / 8 + 15) & ~15;
		}
		public VideoBuffer(int width, int height, PixFrmt pixFrmt) {
			this.memoryMappedFileName = Guid.NewGuid().ToString();
			this.width = width;
			this.height = height;
			this.pixelFormat = pixFrmt;
			this.stride = ((width * pixelFormat.bitsPerPixel + 7) / 8 + 15) & ~15;
		}
		public VideoBuffer(int width, int height, PixFrmt pixFrmt, int stride) {
			this.memoryMappedFileName = Guid.NewGuid().ToString();
			this.width = width;
			this.height = height;
			this.pixelFormat = pixFrmt;
			this.stride = stride;
		}
		private string memoryMappedFileName;
		[NonSerialized]
		private object sync = new object();
		[NonSerialized]
		private IDisposable<MappedData> mappedData = null;
		[NonSerialized]
		private int refCnt = 0;

		public readonly int height;
		public readonly PixFrmt pixelFormat;
		public int size { get { return height * stride + BUFFER_PADDING_SIZE; } }
		public readonly int stride;
		public readonly int width;

		public IDisposable<MappedData> Lock() {
			lock (sync) {
				if (mappedData == null) {
					var file = MemoryMappedFile.CreateOrOpen(memoryMappedFileName, size + EXTRA_DATA_SIZE);
					try {
						var stream = file.CreateViewStream();
						try {
							var handle = stream.SafeMemoryMappedViewHandle;
							mappedData = DisposableExt.Create(
								new MappedData(handle.DangerousGetHandle()),
								() => {
									stream.Dispose();
									file.Dispose();
								}
							);
						} catch (Exception err) {
							dbg.Error(err);
							stream.Dispose();
						}
					} catch (Exception err) {
						dbg.Error(err);
						file.Dispose();
					}
				}
				++refCnt;
				return DisposableExt.Create<MappedData>(
					mappedData.value,
					() => {
						lock (sync) {
							--refCnt;
							if (refCnt == 0) {
								mappedData.Dispose();
								mappedData = null;
							}
						}
					}
				);
			}
			//lock (sync) {
			//    if (scan0Ptr == null) {
			//        var file = MemoryMappedFile.CreateOrOpen(memoryMappedFileName, size);
			//        try {
			//            var stream = file.CreateViewStream();
			//            try {
			//                var handle = stream.SafeMemoryMappedViewHandle;
			//                scan0Ptr = DisposableExt.Create(
			//                    handle.DangerousGetHandle(),
			//                    () => {
			//                        stream.Dispose();
			//                        file.Dispose();
			//                    }
			//                );
			//            } catch (Exception err) {
			//                dbg.Error(err);
			//                stream.Dispose();
			//            }
			//        } catch (Exception err) {
			//            dbg.Error(err);
			//            file.Dispose();
			//        }
			//    }
			//    ++refCnt;
			//    return DisposableExt.Create<IntPtr>(
			//        scan0Ptr.value,
			//        () => {
			//            lock (sync) {
			//                --refCnt;
			//                if (refCnt == 0) {
			//                    scan0Ptr.Dispose();
			//                    scan0Ptr = null;
			//                }
			//            }
			//        }
			//    );
			//}
		}

		public void Dispose() {
		}
		void IDeserializationCallback.OnDeserialization(object sender) {
			sync = new object();
			refCnt = 0;
			mappedData = null;
		}
	}
}
