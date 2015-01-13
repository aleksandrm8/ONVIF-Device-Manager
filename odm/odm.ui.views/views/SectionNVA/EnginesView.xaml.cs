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
using Microsoft.FSharp.Control;
using onvif.services;
using odm.core;
using onvif.utils;
using Microsoft.FSharp.Core;
using utils;
using Microsoft.Practices.Unity;
using System.Reactive.Disposables;
using Microsoft.Practices.Prism.Events;
using odm.ui.links;
using System.Reactive.Linq;
using odm.ui.controls;
using odm.ui.core;

namespace odm.ui.views.SectionNVA {
	/// <summary>
	/// Interaction logic for EnginesView.xaml
	/// </summary>
	public partial class EnginesView : UserControl, IDisposable {
		#region Loader
		public static FSharpAsync<EngineViewArgs> Load(AnalyticsEngine engine,
														Capabilities capabilities,
														INvtSession nvtSession,
														OdmSession odmsession,
														string ctrl = null) {
			EngineViewArgs args = new EngineViewArgs();
			args.nvtSession = nvtSession;
			args.odmSession = odmsession;
			args.capabilities = capabilities;
			args.engine = engine;
			args.selectedEngineControlToken = ctrl;

			return Apm.Iterate(LoadImpl(args)).Select(f => { return args; });
		}
		static IEnumerable<FSharpAsync<Unit>> LoadImpl(EngineViewArgs args) {
			AnalyticsEngineControl[] controls;
			yield return args.nvtSession.GetAnalyticsEngineControls().Select(ctrls => {
				args.controls = ctrls;
				controls = ctrls;
				if (controls == null) {
					args.selectedEngineControl = null;
					args.selectedEngineControlToken = null;
					return (Unit)null;
				}
				if (args.selectedEngineControlToken == null) {
					//load default (first in list)
					var contr = controls.FirstOrDefault(f => {
						if (f.engineToken == null)
							return false;//to show profile even if it corrupted
						return f.engineToken == args.engine.token;
					});
					if (contr == null) {
						args.selectedEngineControl = null;
						args.selectedEngineControlToken = null;
						return (Unit)null;
					}
					args.selectedEngineControl = contr;
					args.selectedEngineControlToken = args.selectedEngineControl.token;
				} else {
					var contr = ctrls.FirstOrDefault(f => {
						if (f.engineToken == null)
							return false;
						return f.engineToken == args.engine.token && f.token == args.selectedEngineControlToken;
					});
					if (contr == null) {
						args.selectedEngineControl = null;
						args.selectedEngineControlToken = null;
						return (Unit)null;
					}
					args.selectedEngineControl = contr;
				}
				return (Unit)null;
			});
		}
		#endregion Loader
		public EnginesView(IUnityContainer container) {
			InitializeComponent();

			this.container = container;
		}

		IUnityContainer container;
		IEventAggregator eventAggregator;
		CompositeDisposable disposables = new CompositeDisposable();
		LinkButtonSwitch btnSwitch;
		List<ButtonBase> Buttons = new List<ButtonBase>();

		public void Init(EngineViewArgs args) {
			eventAggregator = container.Resolve<IEventAggregator>();

			btnSwitch = new LinkButtonSwitch(eventAggregator);
			btnSwitch.ClearSelection = () => {
				buttonsList.UnselectAll();
			};
			disposables.Add(Observable.FromEventPattern(buttonsList, "SelectionChanged")
				.Subscribe(e => {
					btnSwitch.SelectedButton = (ButtonBase)buttonsList.SelectedItem;
				}));
			disposables.Add(btnSwitch);

			//dataProcInfo = new VideoInfo();

			//GetSnapshot(args);

			if (args.selectedEngineControl == null) {
				log.WriteInfo(String.Format("no control is associated with engine '{0}'", args.engine.token));
				CreateEmergencyButtons(args);
			} else {
				//dataProcInfo.ChanToken = args.channelDescr.videoSource.token;

				LoadButtons(args);
				//disposables.Add(Observable.FromEventPattern(vsSnapshot, "MouseDown").Subscribe(ev => { GetSnapshot(args); }));
			}
			buttonsList.ItemsSource = Buttons;
		}
		void CreateEmergencyButtons(EngineViewArgs args) {
			var curAccount = AccountManager.Instance.CurrentAccount;
			Buttons.Add(new NVAControlsButton(container.Resolve<EventAggregator>(), args.nvtSession, args.engine, null, curAccount));//, dataProcInfo));
		}
		void LoadButtons(EngineViewArgs args) {
			var curAccount = AccountManager.Instance.CurrentAccount;

			Buttons.Add(new NVALiveVideoButton(container.Resolve<EventAggregator>(), args.nvtSession, args.engine, args.selectedEngineControl, curAccount));//, dataProcInfo));

			//Buttons.Add(new NVAControlsButton(container.Resolve<EventAggregator>(), args.nvtSession, args.engine, args.selectedEngineControl, curAccount));//, dataProcInfo));

			Buttons.Add(new NVAAnalyticsButton(container.Resolve<EventAggregator>(), args.nvtSession, args.engine, args.selectedEngineControl, curAccount));
			//Buttons.Add(new NVAInputsButton(container.Resolve<EventAggregator>(), args.nvtSession, args.engine, args.selectedEngineControl, curAccount));
			Buttons.Add(new NVAMetadataButton(container.Resolve<EventAggregator>(), args.nvtSession, args.engine, args.selectedEngineControl, curAccount));
			Buttons.Add(new NVASettingsButton(container.Resolve<EventAggregator>(), args.nvtSession, args.engine, args.selectedEngineControl, curAccount));

		}
		public void Dispose() {
			disposables.Dispose();
		}
	}
	public class EngineViewArgs {
		public String selectedEngineControlToken { get; set; }
		public AnalyticsEngineControl selectedEngineControl { get; set; }
		public AnalyticsEngineControl[] controls { get; set; }
		public INvtSession nvtSession { get; set; }
		public OdmSession odmSession { get; set; }
		public Capabilities capabilities { get; set; }
		public AnalyticsEngine engine { get; set; }
	}
}
