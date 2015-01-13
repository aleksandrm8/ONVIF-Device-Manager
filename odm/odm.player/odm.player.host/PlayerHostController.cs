using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;
//using System.Windows.Forms;
//using System.Windows.Media;
using utils;

namespace odm.player {


	public interface IPlayer: IDisposable {
		void SetVideoBuffer(VideoBuffer videoBuffer);
		//void SetUserNamePassword(string userName, string password);
		void SetMetadataReciever(IMetadataReceiver metadataReceiver);
		//IDisposable Play(String streamUrl, IPlaybackController playbackController);
		IDisposable Play(MediaStreamInfo mediaStreamInfo, IPlaybackController playbackController);
	}
}


