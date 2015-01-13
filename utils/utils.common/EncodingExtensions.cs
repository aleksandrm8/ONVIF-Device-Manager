using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.IO;

namespace utils {

	public static class EncodingEx {
		public static string ToBase64(this IEnumerable<byte> bytes, bool insertLineBreaks = false) {
			if (bytes == null) {
				return null;
			}
			var opt = insertLineBreaks ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None;
			return Convert.ToBase64String(bytes.ToArray(), opt);
		}
		public static string ToBase64(this byte[] bytes, bool insertLineBreaks = false) {
			if (bytes == null) {
				return null;
			}
			var opt = insertLineBreaks? Base64FormattingOptions.InsertLineBreaks:Base64FormattingOptions.None;
			return Convert.ToBase64String(bytes, opt);
		}
		public static byte[] FromBase64(this string base64) {
			if (base64 == null) {
				return null;
			}
			return Convert.FromBase64String(base64);
		}
		public static byte[] ToUtf8(this string str) {
			if (str == null) {
				return null;
			}
			return Encoding.UTF8.GetBytes(str);
		}
		public static string FromUtf8(this byte[] utf8) {
			if (utf8 == null) {
				return null;
			}
			return Encoding.UTF8.GetString(utf8);
		}
		public static string FromUtf8(this byte[] utf8, int index, int count) {
			dbg.Assert(utf8 != null);
			return Encoding.UTF8.GetString(utf8, index, count);
		}
		public static string FromUtf8(this byte[] utf8, int count) {
			dbg.Assert(utf8 != null);
			return Encoding.UTF8.GetString(utf8, 0, count);
		}

		public static byte[] ToAscii(this string str) {
			if (str == null) {
				return null;
			}
			return Encoding.ASCII.GetBytes(str);
		}
		public static string FromAscii(this byte[] ascii) {
			if (ascii == null) {
				return null;
			}
			return Encoding.ASCII.GetString(ascii);
		}

		private class Utf8Converter : IOutputStream<byte> {
			const int maxCharsToBuffer = 1024;
			String str;

			private class Reader : IStreamReader<byte> {
				String str;
				static byte[] intBuf = new byte[Encoding.UTF8.GetMaxByteCount(maxCharsToBuffer)];
				int intBufLength = 0;
				int intBufPosition = 0;
				int strPos = 0;
				public Reader(String str) {
					this.str = str;
				}
				bool UpdateBufferIfNecessary(){
					if (intBufLength > 0) {
						return true;
					}
					var charsLeft = str.Length - strPos;
					if (charsLeft <= 0) {
						return false;
					}
					var charsToBuffer = maxCharsToBuffer;
					if (charsLeft < charsToBuffer) {
						charsToBuffer = charsLeft;
					}

					intBufLength = Encoding.UTF8.GetBytes(str, strPos, charsToBuffer, intBuf, 0);
					intBufPosition = 0;
					strPos += charsToBuffer;
				
					return true;
				}

				public int Read(byte[] buffer, int offset, int count) {
					var bytesCopied = 0;
					while (UpdateBufferIfNecessary()) {
						var bytesToCopy = count;
						if (bytesToCopy > intBufLength) {
							bytesToCopy = intBufLength;
						}
						Array.Copy(intBuf, intBufPosition, buffer, offset, bytesToCopy);
						intBufLength -= bytesToCopy;
						intBufPosition += bytesToCopy;
						count -= bytesToCopy;
						offset += bytesToCopy;
						bytesCopied += bytesToCopy;
						if (count == 0) {
							return bytesCopied;
						}
					}
					return bytesCopied;
				}
				public void Dispose() {
					//throw new NotImplementedException();
				}
			}
			public Utf8Converter(String str) {
				this.str = str;
			}

			public IStreamReader<byte> CreateReader() {
				return new Reader(str);
			}
		}
		public static IOutputStream<byte> ToUtf8Stream(this string str) {
			//dbg.Assert(str != null);
			if (str == null) {
				return StreamExtensions.EmptyOutput<byte>();
			}
			return new Utf8Converter(str);
		}

	}
}
