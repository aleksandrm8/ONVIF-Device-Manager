using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using utils;

namespace odm.player {
	

	[Serializable]
	public class MediaStreamInfo {
		
		[Serializable]
		public enum Transport{
			Udp,
			Tcp,
			Http
		}

		public readonly string url;
		public readonly Transport transport;
		public readonly UserNameToken userNameToken;

		public MediaStreamInfo(string url, Transport transport = Transport.Udp, UserNameToken userNameToken = null) {
			this.url = url;
			this.transport = transport;
			this.userNameToken = userNameToken;
		}
	}


	[Serializable]
	public class UserNameToken {
		
		public readonly string userName;
		public readonly string password;

		public UserNameToken(string userName, string password) {
			this.userName = userName;
			this.password = password;
		}
	}

	[Serializable]
	public class PixFrmt : ISerializable {
		private class PixFrmtImpl {
			private static List<PixFrmtImpl> impls = new List<PixFrmtImpl>(16);
			private string formatStr;
			private int m_bitsPerPixel;
			private int m_id;
			private PixFrmtImpl(string formatStr, int bitsPerPixel) {
				this.m_bitsPerPixel = bitsPerPixel;
				this.formatStr = formatStr;
			}
			public static PixFrmtImpl Create(string formatStr, int bitsPerPixel) {
				log.WriteInfo("PixFrmtImpl::Create(...)");
				var impl = new PixFrmtImpl(formatStr, bitsPerPixel);
				lock (impls) {
					impl.m_id = impls.Count;
					impls.Add(impl);
				}
				return impl;
			}
			public static PixFrmtImpl GetById(int id) {
				log.WriteInfo(String.Format("PixFrmtImpl::GetById({0})", id));
				log.WriteInfo(String.Format("impls length is {0}", impls.Count));
				lock (impls) {
					return impls[id];
				}
			}
			public int bitsPerPixel {
				get { return m_bitsPerPixel; }
			}
			public int id {
				get { return m_id; }
			}
			public override string ToString() {
				return formatStr;
			}
		}
		private PixFrmtImpl m_pixFmtImpl;

		private PixFrmt(PixFrmtImpl pixFmtImpl) {
			this.m_pixFmtImpl = pixFmtImpl;
		}
		public int bitsPerPixel {
			get { return m_pixFmtImpl.bitsPerPixel; }
		}
		public override string ToString() {
			return m_pixFmtImpl.ToString();
		}
		// Explicit static constructor to tell C# compiler 
		// not to mark type as beforefieldinit 
		static PixFrmt() {
		}
		public static readonly PixFrmt rgb24 = new PixFrmt(PixFrmtImpl.Create("rgb24", 24));
		public static readonly PixFrmt bgr24 = new PixFrmt(PixFrmtImpl.Create("bgr24", 24));
		public static readonly PixFrmt argb32 = new PixFrmt(PixFrmtImpl.Create("argb32", 32));
		public static readonly PixFrmt bgra32 = new PixFrmt(PixFrmtImpl.Create("bgra32", 32));
		public static bool operator ==(PixFrmt left, PixFrmt right) {
			return left.m_pixFmtImpl == right.m_pixFmtImpl;
		}
		public static bool operator !=(PixFrmt left, PixFrmt right) {
			return left.m_pixFmtImpl != right.m_pixFmtImpl;
		}
		public override bool Equals(object obj) {
			return obj is PixFrmt && m_pixFmtImpl == ((PixFrmt)obj).m_pixFmtImpl;
		}
		public override int GetHashCode() {
			return m_pixFmtImpl.id;
		}

		private static string impl_key { get { return "id"; } }
		//deserialization constructor
		private PixFrmt(SerializationInfo info, StreamingContext context) {
			log.WriteInfo("PixFrmt::PixFrmt(SerializationInfo info, StreamingContext context)");
			if (info == null) {
				log.WriteInfo("info is null");
				throw new System.ArgumentNullException("info");
			}
			var id = info.GetInt32(impl_key);
			this.m_pixFmtImpl = PixFrmtImpl.GetById(id);
			log.WriteInfo("-------------------------------------------");
		}

		//ISerializable implementation
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue(impl_key, m_pixFmtImpl.id);
		}
	}
}
