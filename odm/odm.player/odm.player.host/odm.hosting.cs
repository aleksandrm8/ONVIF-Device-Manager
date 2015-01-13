using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace odm.hosting {

	public interface IHostController {
		Action<IHostController> Hello();
		bool isAlive();
		void Bye();
	}

	public static class AppHosting {
		private static object sync = new object();
		private static IpcChannel m_hostChannel = null;
		public static IpcChannel SetupChannel() {
			lock (sync) {
				if (m_hostChannel == null) {
					var ipcChannelName = Guid.NewGuid().ToString();
					var serverSinkProvider = new BinaryServerFormatterSinkProvider();
					var clientSinkProvider = new BinaryClientFormatterSinkProvider();
					serverSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;
					var props = new Dictionary<string, string>{
						{"portName", ipcChannelName}
					};
					m_hostChannel = new IpcChannel(props, clientSinkProvider, serverSinkProvider);
					ChannelServices.RegisterChannel(m_hostChannel, false);
				}
			}
			return m_hostChannel;
		}
	}
}


