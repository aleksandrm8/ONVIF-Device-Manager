using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.player;
using odm.ui.core;
using onvif.services;
using utils;
using Unit = Microsoft.FSharp.Core.Unit;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for VideoPlayer.xaml
	/// </summary>
	public partial class VideoPlayerView : UserControl, IDisposable, IPlaybackController, INotifyPropertyChanged {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new VideoPlayerView(model, context);

				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public VideoPlayerView() {

		}

		private CompositeDisposable disposables = new CompositeDisposable();
		private SerialDisposable renderSubscription = new SerialDisposable();
		bool _isPaused = false;
		public bool isPaused {
			get {
				return _isPaused;
			}
			set {
				_isPaused = value;
				NotifyPropertyChanged("isPaused");
			}
		}
		IPlaybackSession playbackSession;
		public LocalVideoPlayer Strings { get { return LocalVideoPlayer.instance; } }
		public void Init(Model model) {
			OnCompleted += () => {
				try {
					disposables.Dispose();
				} catch (Exception err) {
					dbg.Error(err);
				}
			};

			VerifyAccess();
			disposables.Add(renderSubscription);
			isPaused = false;
			InitializeComponent();

			captionNoSignal.CreateBinding(TextBlock.TextProperty, Strings, s => s.noSignal);
			noSignalPanel.Visibility = System.Windows.Visibility.Hidden;

			btnPause.Click += new RoutedEventHandler(btnPause_Click);
			btnResume.Click += new RoutedEventHandler(btnPause_Click);

			btnPause.CreateBinding(Button.VisibilityProperty, this, x => { return x.isPaused ? Visibility.Collapsed : Visibility.Visible; });
			btnResume.CreateBinding(Button.VisibilityProperty, this, x => { return !x.isPaused ? Visibility.Collapsed : Visibility.Visible; });

			playbackStatistics.Visibility = AppDefaults.visualSettings.ShowVideoPlaybackStatistics ? Visibility.Visible : Visibility.Collapsed;

			if (model.isUriEnabled) {
				uriString.Visibility = System.Windows.Visibility.Visible;
				uriString.Text = model.mediaUri.uri;
			} else {
				uriString.Visibility = System.Windows.Visibility.Collapsed;
			}
			VideoStartup(model);
		}

		void btnPause_Click(object sender, RoutedEventArgs e) {
			if (isPaused) {
				Resume();
			} else {
				Pause();
			}
		}

		IPlayer player;
		VideoBuffer videoBuff;

		void VideoStartup(Model model) {
			player = new HostedPlayer();
			var res = model.encoderResolution;
			videoBuff = new VideoBuffer(res.width, res.height);
			player.SetVideoBuffer(videoBuff);

			var account = AccountManager.Instance.CurrentAccount;
			UserNameToken utoken = null;
			if (!account.IsAnonymous) {
				utoken = new UserNameToken(account.Name, account.Password);
			}

			if (model.metadataReceiver != null) {
				player.SetMetadataReciever(model.metadataReceiver);
			}

			MediaStreamInfo.Transport transp;
			switch (model.streamSetup.transport.protocol) {
				case TransportProtocol.http:
					transp = MediaStreamInfo.Transport.Http;
					break;
				case TransportProtocol.rtsp:
					transp = MediaStreamInfo.Transport.Tcp;
					break;
				default:
					transp = MediaStreamInfo.Transport.Udp;
					break;
			}
			MediaStreamInfo mstrInfo = new MediaStreamInfo(model.mediaUri.uri, transp, utoken);
			disposables.Add(player.Play(mstrInfo, this));
			InitPlayback(videoBuff);
		}
		public void Dispose() {
			try {
				Cancel();
			} catch (Exception err) {
				dbg.Error(err);
			}
		}

		public void Shutdown() {

		}

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Pause() {
			VerifyAccess();
			isPaused = true;
		}
		public void Resume() {
			VerifyAccess();
			isPaused = false;
		}

		private class PlaybackStatistics {
			private PlaybackStatistics() {
				signal = 0;
				noSignalProcessor = NoSignalProcessor().GetEnumerator();
				UpdateNoSignal();
			}
			static public PlaybackStatistics Start() {
				return new PlaybackStatistics();
			}

			public const long noSignalDelay = 300;
			public const long noSignalTimeout = 2000;
			public const long noSignalTimeoutInitial = 5000;

			public bool isNoSignal { get; private set; }
			public byte signal { get; private set; }
			public double avgRenderingFps { get; private set; }
			public double avgDecodingFps { get; private set; }

			CircularBuffer<long> renderTimes = new CircularBuffer<long>(128);
			CircularBuffer<long> decodeTimes = new CircularBuffer<long>(128);
			IEnumerator<bool> noSignalProcessor;

			private static double SecondsFromTicks(long ticks) {
				return (double)ticks / (double)Stopwatch.Frequency;
			}

			private static double CalculateAvgFpsFromTimes(CircularBuffer<long> times) {
				if (times.length < 2) {
					return 0;
				}
				return times.length / SecondsFromTicks(times.last - times.first);
			}

			private void UpdateNoSignal() {
				noSignalProcessor.MoveNext();
				isNoSignal = noSignalProcessor.Current;
			}

			/// <summary>state machine for no signal</summary>
			/// <returns>true if no signal detected</returns>
			private IEnumerable<bool> NoSignalProcessor() {
				var isNoSignal = false;
				var timer = Stopwatch.StartNew();

				while (signal == 0) {
					if (timer.ElapsedMilliseconds > noSignalTimeoutInitial) {
						isNoSignal = true;
						timer.Restart();
						break;
					}
					yield return isNoSignal;
				}

				while (true) {
					if (signal != 0) {
						decodeTimes.Enqueue(Stopwatch.GetTimestamp());
						if (!isNoSignal) {
							timer.Restart();
						} else if (timer.ElapsedMilliseconds > noSignalDelay) {
							isNoSignal = false;
							timer.Restart();
						}
					} else if (!isNoSignal && timer.ElapsedMilliseconds > noSignalTimeout) {
						isNoSignal = true;
						timer.Restart();
					}
					yield return isNoSignal;
				}
			}

			public void Update(VideoBuffer videoBuffer) {
				//update rendering times history
				renderTimes.Enqueue(Stopwatch.GetTimestamp());

				//evaluate averange rendering fps
				avgRenderingFps = CalculateAvgFpsFromTimes(renderTimes);

				//update no signal indicator
				using (var md = videoBuffer.Lock()) {
					signal = md.value.signal;
					md.value.signal = 0;
				}
				UpdateNoSignal();

				//evaluate averange rendering fps
				avgDecodingFps = CalculateAvgFpsFromTimes(decodeTimes);
			}
		}
		/// <summary>
		/// Initiate rendering loop
		/// </summary>
		/// <param name="videoBuffer"></param>
		public void InitPlayback(VideoBuffer videoBuffer) {
			if (videoBuffer == null) {
				throw new ArgumentNullException("videoBufferDescription");
			}
			VerifyAccess();

			TimeSpan renderinterval;
			try {
				int fps = AppDefaults.visualSettings.ui_video_rendering_fps;
				fps = (fps <= 0 || fps > 100) ? 100 : fps;
				renderinterval = TimeSpan.FromMilliseconds(1000 / fps);
			} catch {
				renderinterval = TimeSpan.FromMilliseconds(1000 / 30);
			}

			var cancellationTokenSource = new CancellationTokenSource();
			renderSubscription.Disposable = Disposable.Create(() => {
				cancellationTokenSource.Cancel();
			});
			var bitmap = PrepareForRendering(videoBuffer);
			var cancellationToken = cancellationTokenSource.Token;
			var dispatcher = this.Dispatcher; //Application.Current.Dispatcher;
			var renderingTask = Task.Factory.StartNew(() => {
				var statistics = PlaybackStatistics.Start();
				using (videoBuffer.Lock()) {
					try {
						//start rendering loop
						while (!cancellationToken.IsCancellationRequested) {
							using (var processingEvent = new ManualResetEventSlim(false)) {
								dispatcher.BeginInvoke(() => {
									using (Disposable.Create(() => processingEvent.Set())) {
										if (!cancellationToken.IsCancellationRequested) {
											//update statisitc info
											statistics.Update(videoBuffer);

											//render farme to screen
											DrawFrame(bitmap, videoBuffer, statistics);
										}
									}
								});
								processingEvent.Wait(cancellationToken);
							}
							cancellationToken.WaitHandle.WaitOne(renderinterval);
						}
					} catch (OperationCanceledException) {
						//swallow exception
					} catch (Exception error) {
						dbg.Error(error);
					}
				}
			}, cancellationToken);

		}

		private WriteableBitmap PrepareForRendering(VideoBuffer videoBuffer) {
			PixelFormat pixelFormat;
			if (videoBuffer.pixelFormat == PixFrmt.rgb24) {
				pixelFormat = PixelFormats.Rgb24;
			} else if (videoBuffer.pixelFormat == PixFrmt.bgra32) {
				pixelFormat = PixelFormats.Bgra32;
			} else if (videoBuffer.pixelFormat == PixFrmt.bgr24) {
				pixelFormat = PixelFormats.Bgr24;
			} else {
				throw new Exception("unsupported pixel format");
			}
			var bitmap = new WriteableBitmap(
				videoBuffer.width, videoBuffer.height,
				96, 96,
				pixelFormat, null
			);
			_imgVIew.Source = bitmap;
			return bitmap;
		}

		private void DrawFrame(WriteableBitmap bitmap, VideoBuffer videoBuffer, PlaybackStatistics statistics) {
			VerifyAccess();
			if (isPaused) {
				return;
			}
			using (var md = videoBuffer.Lock()) {
				// internally calls lock\unlock, uses MILUtilities.MILCopyPixelBuffer
				bitmap.WritePixels(
					new Int32Rect(0, 0, videoBuffer.width, videoBuffer.height),
					md.value.scan0Ptr, videoBuffer.size, videoBuffer.stride,
					0, 0
				);
			}
			renderingFps.Text = String.Format("rendering fps: {0:F1}", statistics.avgRenderingFps);
			decodingFps.Text = String.Format("decoding fps: {0:F1}", statistics.avgDecodingFps);
			noSignalPanel.Visibility =
				statistics.isNoSignal ? Visibility.Visible : Visibility.Hidden;

		}
		private void BaseVideoPlayer_Loaded(object sender, RoutedEventArgs e) {

		}
		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class VideoPlayerActivityModel {
		public VideoPlayerActivityModel(StreamSetup streamSetup, string profileToken, bool showStreamUrl, IMetadataReceiver metadataReceiver) {
			this.profile = null;
			this.profileToken = profileToken;
			this.streamSetup = streamSetup;
			this.showStreamUrl = showStreamUrl;
			this.metadataReceiver = metadataReceiver;
		}
		public VideoPlayerActivityModel(StreamSetup streamSetup, Profile profile, bool showStreamUrl, IMetadataReceiver metadataReceiver) {
			this.profile = profile;
			this.profileToken = profile.token;
			this.streamSetup = streamSetup;
			this.showStreamUrl = showStreamUrl;
			this.metadataReceiver = metadataReceiver;
		}

		public VideoPlayerActivityModel(StreamSetup streamSetup, string profileToken, bool showStreamUrl) {
			this.profile = null;
			this.profileToken = profileToken;
			this.streamSetup = streamSetup;
			this.showStreamUrl = showStreamUrl;
			this.metadataReceiver = null;
		}
		public VideoPlayerActivityModel(StreamSetup streamSetup, Profile profile, bool showStreamUrl) {
			this.profile = profile;
			this.profileToken = profile.token;
			this.streamSetup = streamSetup;
			this.showStreamUrl = showStreamUrl;
			this.metadataReceiver = null;
		}
		public readonly Profile profile;
		public readonly string profileToken;
		public readonly StreamSetup streamSetup;
		public readonly bool showStreamUrl;
		public readonly IMetadataReceiver metadataReceiver;
	}
	public interface IVideoPlayerActivity {
		FSharpAsync<Unit> Run(IUnityContainer ctx, VideoPlayerActivityModel model);
	}
}
