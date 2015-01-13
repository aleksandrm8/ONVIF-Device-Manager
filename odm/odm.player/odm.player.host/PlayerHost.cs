using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;
using odm.hosting;
using utils;

namespace odm.player {

	public class HostedPlayer : IPlayer {
		PlayerTask playerTask = new PlayerTask();
		PlayerHost playerHost = null;

		public HostedPlayer() {
		}

		private class PlayerHost : MarshalByRefObject, IHostController, IPlaybackController, IDisposable {
			private object syn = new object();
			private IPlaybackController playbackController = null;
			private IPlaybackSession playbackSession;
			public PlayerTask playerTask = null;
			public PlayerHost(PlayerTask playerTask) {
				playbackController = playerTask.playbackController;
				playerTask.playbackController = this;
				this.playerTask = playerTask;
			}
			public override Object InitializeLifetimeService() {
				//
				// Returning null designates an infinite non-expiring lease.
				// We must ensure that RemotingServices.Disconnect() is called 
				// when it's no longer needed otherwise there will be a memory leak.
				//
				return null;
			}

			bool IPlaybackController.Initialized(IPlaybackSession playbackSession) {
				IPlaybackController playbackController = null;
				lock (syn) {
					if (this.playbackController == null) {
						return false;
					}
					playbackController = this.playbackController;

					dbg.Assert(this.playbackSession == null);
				}
				var result = playbackController.Initialized(playbackSession);
				lock (syn) {
					if (this.playbackController == null) {
						return false;
					}
					dbg.Assert(this.playbackSession == null);
					if (result) {
						this.playbackSession = playbackSession;
					}
					return result;
				}
			}

			void IPlaybackController.Shutdown() {
				Dispose();
			}

			Action<IHostController> IHostController.Hello() {
				if (playbackController != null) {
					return playerTask.Start;
				} else {
					return (hostController) => { };
				}
			}

			void IHostController.Bye() {
				//Dispose();
			}

			bool IHostController.isAlive() {
				return playbackController != null;
			}

			protected virtual void Dispose(bool disposing) {
				if (disposing) {
					IPlaybackController playbackController = null;
					IPlaybackSession playbackSession = null;
					lock (syn) {
						playbackSession = this.playbackSession;
						playbackController = this.playbackController;
						this.playbackController = null;
						this.playbackSession = null;
					}
					if (playbackController != null) {
						playbackController.Shutdown();
					}
					if (playbackSession != null) {
						try {
							playbackSession.Close();
						} catch (Exception err) {
							dbg.Error(err);
						}
					}
				}

			}
			public void Dispose() {
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			~PlayerHost() {
				Dispose(false);
			}

		}

		[Serializable]
		public class PlayerTask {
			public VideoBuffer videoBuffer;
			public IMetadataReceiver metadataReceiver;
			public MediaStreamInfo mediaStreamInfo;
			public IPlaybackController playbackController;
			public bool keepAlive = true;

			public void Start(IHostController hostController) {
				AppHosting.SetupChannel();
				var d = new SingleAssignmentDisposable();
				if (keepAlive) {
					d.Disposable = Observable.Interval(TimeSpan.FromMilliseconds(500))
						.Subscribe(i => {
							try {
								if (!hostController.isAlive()) {
									d.Dispose();
									Process.GetCurrentProcess().Kill();
								}
							} catch (Exception err) {
								dbg.Error(err);
								Process.GetCurrentProcess().Kill();
							}
						});
				}

				var live555 = new Live555(videoBuffer, metadataReceiver);
				live555.Play(mediaStreamInfo, playbackController);
				d.Dispose();
			}
		}

		public IDisposable Play(MediaStreamInfo mediaStreamInfo, IPlaybackController playbackController) {
			//fix url
			var url = new Uri(mediaStreamInfo.url);
			if (url == null || !url.IsAbsoluteUri) {
				throw new Exception("Invalid playback url");
			}
			if (mediaStreamInfo.transport != MediaStreamInfo.Transport.Http) {
				if (String.Compare(url.Scheme, "rtsp", true) != 0) {
					throw new Exception("Invalid playback url");
				}
			} else if (String.Compare(url.Scheme, "rtsp", true) != 0) {
				int defPort;
				if (String.Compare(url.Scheme, Uri.UriSchemeHttp, true) == 0) {
					defPort = 80;
				} else if (String.Compare(url.Scheme, Uri.UriSchemeHttps, true) == 0) {
					defPort = 443;
				} else {
					throw new Exception("Invalid playback url");
				}
				var ub = new UriBuilder(url);
				ub.Scheme = "rtsp";
				if (ub.Port == -1) {
					ub.Port = defPort;
				}
				url = ub.Uri;
				mediaStreamInfo = new MediaStreamInfo(url.ToString(), mediaStreamInfo.transport, mediaStreamInfo.userNameToken);
			}

			var disposable = new SingleAssignmentDisposable();
			playerTask.mediaStreamInfo = mediaStreamInfo;
			playerTask.playbackController = playbackController;
			if (playerHost != null) {
				playerHost.Dispose();
				RemotingServices.Disconnect(playerHost);
				playerHost = null;
			}

			playerHost = new PlayerHost(playerTask);
			RemotingServices.Marshal(playerHost);
			var ipcChannel = AppHosting.SetupChannel();
			var hostControllerUri = RemotingServices.GetObjectUri(playerHost);
			var hostControllerUrl = ipcChannel.GetUrlsForUri(hostControllerUri).First();

			//start player host process
			var hostProcessArgs = new CommandLineArgs();
			var t = Uri.EscapeDataString(hostControllerUrl);
			hostProcessArgs.Add("controller-url", new List<string> { hostControllerUrl });

			var pi = new ProcessStartInfo() {
				FileName = Assembly.GetExecutingAssembly().Location,
				UseShellExecute = false,
				Arguments = String.Join(" ", hostProcessArgs.Format()),
			};
			pi.EnvironmentVariables["PATH"] = String.Join("; ", Bootstrapper.specialFolders.dlls.Select(sfd => sfd.directory.FullName).Append(pi.EnvironmentVariables["PATH"]));

			StartHostProcess(pi);
			return Disposable.Create(() => {
				Dispose();
			});
		}

		Process hostProcess;
		Timer startHostProcessTimer;
		volatile bool unexpectedTermination = true;
		private void StartHostProcess(ProcessStartInfo pi) {
			if (hostProcess != null)
				hostProcess.Dispose();
			hostProcess = Process.Start(pi);

			hostProcess.Exited += (s, o) => {
				if (unexpectedTermination) {
					if (startHostProcessTimer != null)
						startHostProcessTimer.Dispose();
					startHostProcessTimer = new Timer((state) => StartHostProcess(pi), null, TimeSpan.FromSeconds(1.5), TimeSpan.FromMilliseconds(-1));
				}
			};
			hostProcess.EnableRaisingEvents = true;
		}

		public void SetVideoBuffer(VideoBuffer videoBuffer) {
			playerTask.videoBuffer = videoBuffer;
		}

		//public void SetUserNamePassword(string userName, string password){
		//    playerTask.userNameToken = new UserNameToken(userName, password);
		//}

		public void SetMetadataReciever(IMetadataReceiver metadataReceiver) {
			playerTask.metadataReceiver = metadataReceiver;
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing) {
			unexpectedTermination = false;

			if (playerHost != null) {
				RemotingServices.Disconnect(playerHost);
				playerHost.Dispose();
				playerHost = null;
			}
			if (disposing) {
				playerTask = null;
			}
			if (startHostProcessTimer != null)
				startHostProcessTimer.Dispose();
			if (hostProcess != null)
				hostProcess.Dispose();
		}
		~HostedPlayer() {
			Dispose(false);
		}
	}

}


