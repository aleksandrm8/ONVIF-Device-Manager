using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.core;
using odm.infra;
using odm.ui.controls;
using odm.ui.core;
using utils;
using onvif.services;

namespace odm.ui.viewModels {
	public class DigitalIOViewModel : ViewModelDeviceBase {

		public LocalDevice AppStrings { get { return LocalDevice.instance; } }
		public PropertyDigitalIOStrings Strings { get { return PropertyDigitalIOStrings.instance; } }
		public RelayIdleState[] IdleStates { get; private set; }
		public RelayMode[] RelayModes { get; private set; }

		public DigitalIOViewModel(IUnityContainer container)
			: base(container) {
			relays = new ObservableCollection<RelayOutput>();

			Activate = new DelegateCommand(() => {
				ActivateExecute();
			}, () => {
				return SelectedRelay != null;
			});
			Deactivate = new DelegateCommand(() => {
				DeactivateExecute();
			}, () => {
				return SelectedRelay != null;
			});

			SetIO = new DelegateCommand(() => {
				SetIOExecute();
			}, () => {
				return SelectedRelay != null;
			});

			RelayModes = EnumHelper.GetValues<RelayMode>();
			IdleStates = EnumHelper.GetValues<RelayIdleState>();
		}

		void SetDelay() {
			if (SelectedRelay == null)
				return;
			try {
				Delay = SelectedRelay.properties.delayTime.timeSpan.TotalSeconds.ToInt32().ToString();
			} catch (Exception err) {
				dbg.Error(err.Message);
			}
		}
		XsDuration GetDelay() {
			int idelay;
			if (SelectedRelay == null)
				idelay = 0;

			if (!Int32.TryParse(Delay, out idelay)) {
				idelay = 0;
			}
			return new XsDuration() {
				timeSpan = TimeSpan.FromSeconds(idelay)
			};
		}
		void ActivateExecute() {
			if (SelectedRelay == null)
				return;
			Current = States.Loading;
			subscription.Add(session.SetRelayOutputState(SelectedRelay.token, RelayLogicalState.active)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(unit => {
					 Current = States.Common;
				 }, err => {
					 ErrorMessage = err.Message;
					 ErrorBtnClick = new DelegateCommand(() => { Current = States.Common; });
					 Current = States.Error;
				 }));
		}
		void DeactivateExecute() {
			if (SelectedRelay == null)
				return;
			Current = States.Loading;
			subscription.Add(session.SetRelayOutputState(SelectedRelay.token, RelayLogicalState.inactive)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(unit => {
					 Current = States.Common;
				 }, err => {
					 ErrorMessage = err.Message;
					 ErrorBtnClick = new DelegateCommand(() => { Current = States.Common; });
					 Current = States.Error;
				 }));
		}
		void SetIOExecute() {
			if (SelectedRelay == null)
				return;

			try {
				SelectedRelay.properties.delayTime = GetDelay();
			} catch (Exception err) {
				dbg.Error(err);
			}

			Current = States.Loading;
			subscription.Add(session.SetRelayOutputSettings(SelectedRelay.token, SelectedRelay.properties)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(unit => {
					 Reload();
				 }, err => {
					 ErrorMessage = err.Message;
					 ErrorBtnClick = new DelegateCommand(() => { Current = States.Common; });
					 Current = States.Error;
				 }));

		}
		INvtSession session;
		public ObservableCollection<RelayOutput> relays { get; set; }

		void Reload() {
			Delay = "";
			relays.Clear();
			Load(session, CurrentAccount);
		}

		public override void Load(INvtSession session, Account account) {
			this.session = session;
			CurrentAccount = account;
			Current = States.Loading;
			session.GetRelayOutputs()
				.ObserveOnCurrentDispatcher()
				.Subscribe(relayOutputs => {
					relayOutputs.ForEach(x => {
						relays.Add(x);
					});
					Current = States.Common;
				}, err => {
					ErrorMessage = err.Message;
					ErrorBtnClick = new DelegateCommand(() => { Reload(); });
					Current = States.Error;
				});
		}

		public DelegateCommand Deactivate {
			get { return (DelegateCommand)GetValue(DeactivateProperty); }
			set { SetValue(DeactivateProperty, value); }
		}
		public static readonly DependencyProperty DeactivateProperty =
			 DependencyProperty.Register("Deactivate", typeof(DelegateCommand), typeof(DigitalIOViewModel));

		public DelegateCommand Activate {
			get { return (DelegateCommand)GetValue(ActivateProperty); }
			set { SetValue(ActivateProperty, value); }
		}
		public static readonly DependencyProperty ActivateProperty =
			 DependencyProperty.Register("Activate", typeof(DelegateCommand), typeof(DigitalIOViewModel));

		public DelegateCommand SetIO {
			get { return (DelegateCommand)GetValue(SetIOProperty); }
			set { SetValue(SetIOProperty, value); }
		}
		public static readonly DependencyProperty SetIOProperty =
			 DependencyProperty.Register("SetIO", typeof(DelegateCommand), typeof(DigitalIOViewModel));

		public string Delay {
			get { return (string)GetValue(DelayProperty); }
			set { SetValue(DelayProperty, value); }
		}
		public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(string), typeof(DigitalIOViewModel));

		public RelayOutput SelectedRelay {
			get { return (RelayOutput)GetValue(SelectedRelayProperty); }
			set { SetValue(SelectedRelayProperty, value); }
		}
		public static readonly DependencyProperty SelectedRelayProperty = DependencyProperty.Register("SelectedRelay", typeof(RelayOutput), typeof(DigitalIOViewModel),
			 new PropertyMetadata(null, (obj, evargs) => {
				 var digIo = (DigitalIOViewModel)obj;
				 digIo.SetDelay();
				 digIo.Activate.RaiseCanExecuteChanged();
				 digIo.Deactivate.RaiseCanExecuteChanged();
				 digIo.SetIO.RaiseCanExecuteChanged();
			 }));


	}
}
