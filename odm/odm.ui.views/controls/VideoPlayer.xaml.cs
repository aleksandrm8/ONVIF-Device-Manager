using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using utils;
using System.Globalization;
using System.Diagnostics;
using odm.ui.core;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.IO.MemoryMappedFiles;
using odm.player;
using System.Threading.Tasks;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for VideoPlayer.xaml
	/// </summary>
	public partial class VideoPlayer : BaseVideoPlayer {
		private CompositeDisposable disposables = new CompositeDisposable();
		private SerialDisposable renderSubscription = new SerialDisposable();
		public bool isPaused { get; private set; }
		
		public VideoPlayer() {
			VerifyAccess();
			disposables.Add(renderSubscription);
			isPaused = false;
			InitializeComponent();

			if (AppDefaults.visualSettings.Enable_UI_Fps_Caption) {
				fpsCaption.Visibility = System.Windows.Visibility.Visible;
			} else {
				fpsCaption.Visibility = System.Windows.Visibility.Hidden;
			}
		}
		public void ReleaseAll() {
			VerifyAccess();
			disposables.Dispose();
		}

		public void Pause() {
			VerifyAccess();
			isPaused = true;
		}
		public void Resume() {
			VerifyAccess();
			isPaused = false;
		}

		/// <summary>
		/// Start Playback
		/// </summary>
		/// <param name="res"></param>
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
			var dispatcher = Application.Current.Dispatcher;
			var renderingTask = Task.Factory.StartNew(() => {
				var statistics = new CircularBuffer<long>(100);
				using (videoBuffer.Lock()) {
					try {
						//start rendering loop
						while (!cancellationToken.IsCancellationRequested) {
							using (var processingEvent = new ManualResetEventSlim(false)) {
								dispatcher.BeginInvoke(() => {
									using (Disposable.Create(() => processingEvent.Set())) {
										if (!cancellationToken.IsCancellationRequested) {
											//update statisitc info
											statistics.Enqueue(Stopwatch.GetTimestamp());
											//evaluate averange rendering fps
											var ticksEllapsed = statistics.last - statistics.first;
											double avgFps = 0;
											if (ticksEllapsed > 0) {
												avgFps = ((double)statistics.length * (double)Stopwatch.Frequency) / (double)ticksEllapsed;
											}
											//render farme to screen
											DrawFrame(bitmap, videoBuffer, avgFps);
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

		private void DrawFrame(WriteableBitmap bitmap, VideoBuffer videoBuffer, double averangeFps) {
			VerifyAccess();
			if (isPaused) {
				return;
			}

			bitmap.Lock();
			try {
				using (var ptr = videoBuffer.Lock()) {
					bitmap.WritePixels(
						new Int32Rect(0, 0, videoBuffer.width, videoBuffer.height),
						ptr.value, videoBuffer.size, videoBuffer.stride,
						0, 0
					);
				}
			} finally {
				bitmap.Unlock();
			}
			fpsCaption.Text = averangeFps.ToString("F1");
		}
		private void BaseVideoPlayer_Loaded(object sender, RoutedEventArgs e) {

		}
	}
}
