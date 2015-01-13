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
using odm.ui.controls;
using Microsoft.FSharp.Control;
using utils;
using odm.ui.core;
using Microsoft.FSharp.Core;
using odm.core;
using onvif.services;
using onvif.utils;
using odm.ui.activities;
using System.Reactive.Disposables;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using odm.ui.links;

namespace odm.ui.views.SectionNVT {
	/// <summary>
	/// Interaction logic for SectionSources.xaml
	/// </summary>
	public partial class SectionSources : UserControl, IDisposable {

		public SectionSources(IUnityContainer container) {
			InitializeComponent();

			this.container = container;
		}

		IUnityContainer container;
		IEventAggregator eventAggregator;
		CompositeDisposable disposables = new CompositeDisposable();
		Dictionary<string, DeviceChannelControl> channelControls = new Dictionary<string, DeviceChannelControl>();

		public void Init(SourcesArgs args) {
			eventAggregator = container.Resolve<IEventAggregator>();

			args.channels.ForEach(chan => {
				LoadChannel(chan, args);
			});
		}


		void InitChannelControl(DeviceChannelControl channControl, ChannelDescription chan, SourcesArgs args, string proftoken = null) {
			//try to remove and clear all needed data
			if (channControl.Content is IDisposable) {
				var disp = channControl.Content as IDisposable;
				//try to remove content from disposables collection
				if(disposables.Contains(disp))
					disposables.Remove(disp);
				//dispose existing control
				disp.Dispose();
			}

			//Begin load channels section
			disposables.Add(SourceView.Load(chan, args.capabilities, args.nvtSession, args.odmSession, proftoken)
				.ObserveOnCurrentDispatcher()
				.Subscribe(sourceArgs => {
					if (sourceArgs.selectedProfile != null)
						channControl.Title = sourceArgs.channelDescr.videoSource.token + ": " + sourceArgs.selectedProfile.name;
					else
						channControl.Title = sourceArgs.channelDescr.videoSource.token;

					SourceView sourceView = new SourceView(container);
					disposables.Add(sourceView);

					sourceView.Init(sourceArgs);
					channControl.Content = sourceView;
				}, err => {
					ErrorView errorView = new ErrorView(err);
					disposables.Add(errorView);

					channControl.Content = errorView;
				}
			));
		}
		void ShowLoadingProgress(DeviceChannelControl channControl, string title) {
			ProgressView progress = new ProgressView("Loading ..");
			channControl.Content = progress;
			channControl.Title = title;
		}
		void LoadChannel(ChannelDescription chan, SourcesArgs args, string proftoken = null) {
			//Create channel control
			DeviceChannelControl channControl = new DeviceChannelControl();
			//add control to controls dictionary
			channelControls.Add(chan.videoSource.token, channControl);
			
			//Display progress bar
			ShowLoadingProgress(channControl, chan.videoSource.token);

			//add control to parent UI panel
			parent.Children.Add(channControl);

			InitChannelControl(channControl, chan, args);

			//subscribe to profile changed event
			var subsToken = eventAggregator.GetEvent<ProfileChangedEvent>().Subscribe(evargs => {
				if (evargs.vsToken == chan.videoSource.token) {
					//reload channel with new profile
					InitChannelControl(channControl, chan, args, evargs.profToken);
				}
			}, false);
			disposables.Add(Disposable.Create(() => {
				eventAggregator.GetEvent<ProfileChangedEvent>().Unsubscribe(subsToken);
			}));
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
	public class SourcesArgs {
		public INvtSession nvtSession { get; set; }
		public OdmSession odmSession { get; set; }
		public Capabilities capabilities { get; set; }
		public ChannelDescription[] channels { get; set; }
	}
}
