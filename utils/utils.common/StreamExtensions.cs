using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	
	public interface IStreamReader<T> : IDisposable {
		int Read(T[] buffer, int offset, int count);
		//public IAsyncResult BeginRead(T[] buffer, int offset, int count, Action<IAsyncResult> callback);
		//public int EndRead(IAsyncResult ar);
		//public void AbortRead(IAsyncResult ar);
	}

	public interface IOutputStream<T> {
		IStreamReader<T> CreateReader();
		//public IAsyncResult BeginCreateReader(Action<IAsyncResult> callback);
		//public IStreamReader<T> EndCreateReader(IAsyncResult ar);
		//public void AbortCreateReader(IAsyncResult ar);
	}

	public interface IStreamWriter<T> : IDisposable {
		void Write(T[] buffer, int offset, int count);
		//public IAsyncResult BeginWrite(T[] buffer, int offset, int count, Action<IAsyncResult> callback);
		//public void EndWrite(IAsyncResult ar);
		//public void AbortWrite(IAsyncResult ar);
		
	}

	public interface IInputStream<T> {
		IStreamWriter<T> CreateWriter();
		//public IAsyncResult BeginCreateWriter(Action<IAsyncResult> callback);
		//public IStreamWriter<T> EndCreateWriter(IAsyncResult ar);
		//public void AbortCreateWriter(IAsyncResult ar);
	}

	//usage example
	public class MyHttpClient {
		Dictionary<string, string> headers;
		MyHttpClient(){
		}
		IOutputStream<byte> Post(Uri action, IInputStream<byte> requestStream, Encoding encoding) {
			return null;
		}
		IOutputStream<byte> Get(Uri action) {
			return null;
		}
	}

	public static class StreamExtensions {
		private class ConcatenatedStreamReader<T>: IStreamReader<T>{
			IEnumerator<IOutputStream<T>> itor = null;
			IStreamReader<T> reader = null;
			public ConcatenatedStreamReader(IEnumerable<IOutputStream<T>> streams){
				if(streams != null){
					itor = streams.GetEnumerator();
				}
				reader = GetNextReader();
			}
			IStreamReader<T> GetNextReader(){
				if(itor == null){
					return null;
				}
				while(itor.MoveNext()){
					if(itor.Current != null){
						return itor.Current.CreateReader();
					}
				}
				return null;
			}
			public int Read(T[] buffer, int offset, int count){
 				var totalCopiedItems = 0;
				while(count > 0 && reader != null){
					var itemsCopied = reader.Read(buffer, offset, count);
					if(itemsCopied > 0){
						count-=itemsCopied;
						offset+=itemsCopied;
						totalCopiedItems += itemsCopied;
					}else{
						try{
							reader.Dispose();
						}finally{
							reader = null;
						}
						reader = GetNextReader();
					}
				}
				return totalCopiedItems;
			}
			public void Dispose(){
				var errors = new List<Exception>();
				if(reader!=null) {
					try{
						reader.Dispose();
					}catch(Exception err){
						errors.Add(err);
					}finally{
						reader = null;
					}
				}
				if(itor!=null){
					try{
						itor.Dispose();
					}catch(Exception err){
						errors.Add(err);
					}finally{
						itor = null;
					}
				}
				if(errors.Count > 0){
					throw new AggregateException(errors);
				}
			}
		}
		private class ConcatenatedOutputStream<T>: IOutputStream<T>{
			IEnumerable<IOutputStream<T>> streams;
			public ConcatenatedOutputStream(IEnumerable<IOutputStream<T>> streams){
				this.streams = streams;
			}
			public IStreamReader<T> CreateReader(){
				return new ConcatenatedStreamReader<T>(streams);
			}
		}

		private class AnonymousStreamReader<T> : IStreamReader<T> {
			private Func<T[], int, int, int> readFunc;
			private Action disposeCallback;
			public AnonymousStreamReader(Func<T[], int, int, int> readFunc, Action disposeCallback = null) {
				this.readFunc = readFunc;
				this.disposeCallback = disposeCallback;
			}
			public int Read(T[] buffer, int offset, int count) {
				var res = 0;
				if (readFunc != null) {
					res = readFunc(buffer, offset, count);
					if (res < count) {
						readFunc = null;
					}
				};
				return res;
			}
			public void Dispose() {
				if (disposeCallback != null) {
					disposeCallback();
					disposeCallback = null;
				}
				readFunc = null;
			}
		}

		private class AnonymousOutputStream<T> : IOutputStream<T> {
			private Func<IStreamReader<T>> factory;
			public AnonymousOutputStream(Func<IStreamReader<T>> factory) {
				this.factory = factory;
			}
			public IStreamReader<T> CreateReader() {
				return factory();
			}
		}

		public static IOutputStream<T> CreateOutput<T>(Func<IStreamReader<T>> factory) {
			return new AnonymousOutputStream<T>(factory);
		}

		public static IOutputStream<T> CreateOutput<T>(Func<Func<T[], int, int, int>> factory){
			return CreateOutput(()=>{
				var readFunc = factory();
				return new AnonymousStreamReader<T>(readFunc);
			});
		}

		public static IStreamReader<T> CreateReader<T>(Func<T[], int, int, int> read, Action dispose) {
			return new AnonymousStreamReader<T>(read, dispose);
		}


		public static IOutputStream<T> EmptyOutput<T>() {
			return CreateOutput<T>(() => (buf, ofs, cnt) => 0);
		}

		public struct TransformationResult{
			public int readed;
			public int writed;
		}
		
		public struct TransformationBuffer<T>{
			public T[] items;
			public int offset;
			public int count;
		}

		public interface ITransformation<TSrc, TDst> {
			int Process(TSrc[] buf, int ofs, int cnt);
			int Copy(TDst[] buf, int ofs, int cnt);
		}
		public class StringToUtf8Transformation : ITransformation<char, byte> {
			readonly int maxCharsToBuffer = 1024;
			byte[] outBuf;
			int outBufPos = 0;
			int outBufCnt = 0;

			public StringToUtf8Transformation(int maxCharsToBuffer = 1024) {
				this.maxCharsToBuffer = maxCharsToBuffer;
				this.outBuf = new byte[Encoding.UTF8.GetMaxByteCount(maxCharsToBuffer)];
			}
			
			public int Process(char[] buf, int ofs, int cnt) {
				if (outBufCnt > 0) {
					return 0;
				}
				if (cnt > maxCharsToBuffer) {
					cnt = maxCharsToBuffer;
				}
				outBufCnt = Encoding.UTF8.GetBytes(buf, ofs, cnt, outBuf, 0);
				outBufPos = 0;
				return outBufCnt;
			}

			public int Copy(byte[] buf, int ofs, int cnt) {
				if (cnt > outBufCnt) {
					cnt = outBufCnt;
				}
				Array.Copy(outBuf, outBufPos, buf, ofs, cnt);
				outBufCnt -= cnt;
				outBufPos += cnt;
				return cnt;
			}
		}

		public class AsciiToStringTransformation : ITransformation<byte, char> {
			readonly int maxBytesToBuffer;
			char[] outBuf;
			int outBufPos = 0;
			int outBufCnt = 0;

			public AsciiToStringTransformation(int maxBytesToBuffer = 1024) {
				this.maxBytesToBuffer = maxBytesToBuffer;
				this.outBuf = new char[Encoding.ASCII.GetMaxCharCount(maxBytesToBuffer)];
			}
			public int Process(byte[] buf, int ofs, int cnt) {
				if (outBufCnt > 0) {
					return 0;
				}
				if (cnt > maxBytesToBuffer) {
					cnt = maxBytesToBuffer;
				}
				outBufCnt = Encoding.ASCII.GetChars(buf, ofs, cnt, outBuf, 0);
				outBufPos = 0;
				return outBufCnt;
			}

			public int Copy(char[] buf, int ofs, int cnt) {
				if (cnt > outBufCnt) {
					cnt = outBufCnt;
				}
				Array.Copy(outBuf, outBufPos, buf, ofs, cnt);
				outBufCnt -= cnt;
				outBufPos += cnt;
				return cnt;
			}
		}

		public delegate TransformationResult TransformationFunction<TSrc, TDst>(TransformationBuffer<TSrc> srcBuffer, TransformationBuffer<TDst> dstBuffer);
		public static IOutputStream<TDst> Transform<TSrc, TDst>(this IOutputStream<TSrc> stream, ITransformation<TSrc, TDst> transformation, int readBufSize = 1024) {
			//Encoding.UTF8.GetBytes(str, strPos, charsToBuffer, intBuf, 0);
			return CreateOutput<TDst>(() => {
				var inBuf = new TSrc[readBufSize];
				var inBufPos = 0;
				var inBufCnt = 0;
				IStreamReader<TSrc> reader = null;
				if (stream != null) {
					reader = stream.CreateReader();
				}
				return new AnonymousStreamReader<TDst>((buf, ofs, cnt) => {
					var totalBytesCopied = 0;
					while (cnt > 0) {
						var bytesCopied = transformation.Copy(buf, ofs, cnt);
						if (bytesCopied > 0) {
							ofs += bytesCopied;
							cnt -= bytesCopied;
							totalBytesCopied += bytesCopied;
						} else {
							Action process = () => {
								var itemsProcessed = transformation.Process(inBuf, inBufPos, inBufCnt);
								inBufCnt -= itemsProcessed;
								inBufPos += itemsProcessed;
							};
							if (inBufCnt > 0) {
								process();
							}else{
								if (reader == null) {
									return totalBytesCopied;
								}
								inBufPos = 0;
								//try {
									inBufCnt = reader.Read(inBuf, 0, inBuf.Length);
								//} catch (Exception err) {
									//inBufCnt = 0;
									//try {
									//    reader.Dispose();
									//} finally {
									//    reader = null;
									//}
								//}
								if (inBufCnt > 0) {
									process();
								}else{
									try {
										reader.Dispose();
									} finally {
										reader = null;
									}
									return totalBytesCopied;
								}
							}
						}
					}
					return totalBytesCopied;
				});
			});
		}
		
		public static IOutputStream<T> Concat<T>(this IEnumerable<IOutputStream<T>> streams) {
			return new ConcatenatedOutputStream<T>(streams);
		}

		public static IOutputStream<char> AsStream(this string str) {
			return CreateOutput<char>(() => {
				var pos = 0;
				return new AnonymousStreamReader<char>((buf, ofs, cnt) => {
					var len = str.Length - pos;
					if (cnt > len) {
						cnt = len;
					}
					str.CopyTo(pos, buf, ofs, cnt);
					pos += cnt;
					return cnt;
				});
			});
		}

		
		//public static Func<T[], int, int, int> ContinueWith<T>(this IOutputStream<T> stream, Func<IOutputStream<T>> continuation) {
		//}

		//public static Stream AsIoStream(this IStreamReader<byte> stream) {
		//}
		//public static Func<Stream> AsIoStreamFactory(this IOutputStream<byte> stream) {
		//}
		

		//private class IoReadableStreamDelegator : Stream {
		//    public delegate int ReadDelegate(byte[] buffer, int offset, int count);
		//    //public delegate int WriteDelegate(byte[] buffer, int offset, int count);

		//    private ReadDelegate readDelegate = null;

		//    public IoReadableStreamDelegator(ReadDelegate readDelegate) {
		//        if (readDelegate == null) {
		//            throw new ArgumentNullException("readDelegate");
		//        }
		//        this.readDelegate = readDelegate;
		//    }

		//    public override bool CanRead {
		//        get { return true; }
		//    }

		//    public override bool CanSeek {
		//        get { return false; }
		//    }

		//    public override bool CanWrite {
		//        get { return false; }
		//    }

		//    public override void Flush() {
		//    }

		//    public override long Length {
		//        get { throw new NotSupportedException(); }
		//    }

		//    public override long Position {
		//        get {
		//            throw new NotSupportedException();
		//        }
		//        set {
		//            throw new NotSupportedException();
		//        }
		//    }

		//    public override int Read(byte[] buffer, int offset, int count) {
		//        if (readDelegate == null) {
		//            throw new ObjectDisposedException(
		//                message: "can not read from closed stream", 
		//                innerException: null
		//            );
		//        }
		//        return readDelegate(buffer, offset, count);
		//    }

		//    public override long Seek(long offset, SeekOrigin origin) {
		//        throw new NotSupportedException();
		//    }

		//    public override void SetLength(long value) {
		//        throw new NotSupportedException();
		//    }

		//    public override void Write(byte[] buffer, int offset, int count) {
		//        throw new NotSupportedException();
		//    }
		//}

		//public static Stream CreateOutput(ReadDelegate readDelegate) {
		//    return new DelegateReadStream(readDelegate);
		//}
		//public static Stream EmptyOutput = CreateOutput(((buf, ofs, len)=>0));
	}

}
