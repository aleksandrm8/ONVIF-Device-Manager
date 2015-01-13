using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

using odm.controllers;
using odm.infra;
using odm.ui.controls;
using odm.ui.views;

using utils;

namespace odm.ui.activities {
	public partial class TimeSettingsView : BasePropertyControl {
		public enum SetDateTimeMode {
			SetManually,
			SyncWithNtp,
			SyncWithComp
		}
		
		PosixTz originPosixTz;
		DateTime originUtcDateTime = default(DateTime);
		TimeZoneViewModel originTimeZone;
		TimeZoneViewModel newTimeZone;

		private void Init(Model model) {
			//Save model handle
			this.model = model;

			originPosixTz = PosixTz.TryParse(model.timeZone);
			if(model.utcDateTime != null){
				originUtcDateTime = model.utcDateTime.Value;
			} else if ((object)originPosixTz != null && model.localDateTime != null) {
				try{
					originUtcDateTime = originPosixTz.ConvertLocalTimeToUtc(
						(DateTime)model.localDateTime, model.origin.daylightSavings
					);
				}catch(Exception err){
					dbg.Error(err);
				}
			}
			

			//Init command
			OnCompleted += () => {disposables.Dispose();};

			//var applyCommand = new DelegateCommand(
			//    () => OnApplyChanges(),
			//    () => true
			//);
			//ApplyCommand = applyCommand;

			var closeCommand = new DelegateCommand(
				() => Success(new Result.Close()),
				() => true
			);
			CloseCommand = closeCommand;

			var cancelCommand = new DelegateCommand(
				() => {
					OnRevertChanges();
				},
				() => true
			);
			CancelCommand = cancelCommand;

			//init all UI controls
			InitializeComponent();
			//Init time zones
			InitTimeZones();
			//Init and fill
			Init();
			//set up local strings
			Localization();
		}

		void InitTimeZones() {
			//refresh list
			timeZonesComboBox.Items.Clear();
			//Get system timezones list
			var listSystem = TimeZoneViewModel.GetSystemTimeZones();
			listSystem.ForEach(obj => {
				timeZonesComboBox.Items.Add(obj);
			});

			//try to load and fill manual list
			//TODO: get rid of hardcoded filename
			var manualListpath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory,
				"custom-time-zones.xml"
			);
			var listManual = TimeZoneViewModel.GetManualTimeZones(manualListpath);
			if (listManual.Count() != 0) {
				timeZonesComboBox.Items.Add(new Separator());
				listManual.ForEach(obj => {
					timeZonesComboBox.Items.Add(obj);
				});
			}
			
			//Find and try to select device tz
			TimeZoneViewModel tzmodel = null;
			ErrorBlock.Visibility = Visibility.Collapsed;
			if (originPosixTz != null) {
				if (model.localDateTime != null && model.utcDateTime!=null) {
					try {
						if (originPosixTz.ConvertUtcTimeToLocal((DateTime)model.utcDateTime, model.daylightSavings) != (DateTime)model.localDateTime) {
							//TODO: needs to be localized
							ErrorMessage.Text = "Validation failed. Local time sent by device differs from calculated.";
							ErrorBlock.Visibility = Visibility.Visible;
						};
					} catch (Exception err) {
						dbg.Error(err);
						//TODO: needs to be localized
						ErrorMessage.Text = "failed to validate local time";
						ErrorBlock.Visibility = Visibility.Visible;
					}
				}
				tzmodel = new TimeZoneViewModel(model.timeZone, model.timeZone, originPosixTz);

				
				if (listSystem.Any(f => f == tzmodel)) {
					timeZonesComboBox.SelectedItem = listSystem.First(f => f == tzmodel);
				}
				//else if (listSystem.Any(f => f.LogicallyEquals(tzmodel))) {
				//    timeZonesComboBox.SelectedItem = listSystem.First(f => f.LogicallyEquals(tzmodel));
				//} 
				else if (listManual.Any(f => f == tzmodel)) {
					timeZonesComboBox.SelectedItem = listManual.First(f => f == tzmodel);
				} 
				//else if (listManual.Any(f => f.LogicallyEquals(tzmodel))) {
				//    timeZonesComboBox.SelectedItem = listSystem.First(f => f.LogicallyEquals(tzmodel));
				//} 
				else {
					timeZonesComboBox.Text = tzmodel.originalString;
				}
			} else {
				//Invalid TZ
				timeZonesComboBox.Text = model.timeZone;
				
			}
		}
		


		TimeZoneViewModel GetTimeZoneSelection() {
			TimeZoneViewModel timezonemosel = null;
			if (timeZonesComboBox.SelectedIndex == -1) {
				timezonemosel = new TimeZoneViewModel(timeZonesComboBox.Text, timeZonesComboBox.Text, PosixTz.TryParse(timeZonesComboBox.Text));
			}else{
				var tmodel = timeZonesComboBox.SelectedItem as TimeZoneViewModel;
				if(tmodel != null)
					timezonemosel = tmodel;
			}
			return timezonemosel;
		}
		DateTime GetCurrentDeviceTime() {
			var elapsedTicks = Stopwatch.GetTimestamp() - model.timestamp;
			var elapsedMilliseconds = (double)elapsedTicks * 1000.0 / (double)Stopwatch.Frequency;
			var utc = originUtcDateTime.AddMilliseconds(elapsedMilliseconds);
			try {
				if (originPosixTz == null) {
					return utc;
				}
				return originPosixTz.ConvertUtcTimeToLocal(utc, model.origin.daylightSavings);
			} catch {
				//TODO: show notification when posix timezone can not be converted to system timzone
				return utc;
			}
		}
		DateTime GetNewDeviceTime() {
			var utc = DateTime.UtcNow;
			try {
				var posixTz = GetTimeZoneSelection().posixTz;
				if (posixTz == null) {
					return utc;
				}
				return posixTz.ConvertUtcTimeToLocal(utc, model.daylightSavings);
			} catch {
				//TODO: show notification when posix timezone can not be converted to system timzone
				return utc;
			}
		}
		void Init() {
			
			newManualTimeValue.SelectedTime = DateTime.Now.TimeOfDay;
			newManualDateValue.SelectedDate = DateTime.Now;
			//Set time and date dislay
			//valueDeviceTime.SetTime(model.utcDateTime, GetTimeZoneSelection(), GetDeviceUtcTime);
			valueDeviceTime.Init(GetCurrentDeviceTime);
			
			valueNewTime.Init(GetNewDeviceTime);

			daylightCheckBox.CreateBinding(
				CheckBox.IsCheckedProperty, model, 
				m => m.daylightSavings, 
				(m, v) => {
					m.daylightSavings = v; 
					valueNewTime.Invalidate();
				}
			);

			disposables.Add(timeZonesComboBox
				.GetPropertyChangedEvents(ComboBox.SelectedItemProperty)
				.OfType <TimeZoneViewModel>()
				.Subscribe(next => {
					valueNewTime.Invalidate();
					//valueNewTime.TimeZoneChanged(next);
				})
			);

			//Set selection mode
			setDateTimeMode = model.useDateTimeFromNtp ? SetDateTimeMode.SyncWithNtp : SetDateTimeMode.SyncWithComp;
			PanelNtpMode.Visibility =
				setDateTimeMode == SetDateTimeMode.SyncWithNtp ?
				Visibility.Visible : Visibility.Collapsed;

			syncWithNtp.IsSelected = setDateTimeMode == SetDateTimeMode.SyncWithNtp;
			PanelSystemMode.Visibility =
				setDateTimeMode == SetDateTimeMode.SyncWithComp ?
				Visibility.Visible : Visibility.Collapsed;
			syncWithComp.IsSelected = setDateTimeMode == SetDateTimeMode.SyncWithComp;

			PanelManualMode.Visibility = System.Windows.Visibility.Collapsed;
			setManual.IsSelected = false;

			applyButton.Click += (s, a) => OnApplyChanges(); //.Command = ApplyCommand;
			cancelButton.Command = CancelCommand;


			disposables.Add(
				syncWithNtp
					.GetPropertyChangedEvents(ComboBoxItem.IsSelectedProperty)
					.OfType<bool>()
					.Subscribe(value => {
						PanelNtpMode.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
						setDateTimeMode = SetDateTimeMode.SyncWithNtp;
					})
			);

			disposables.Add(
				syncWithComp
					.GetPropertyChangedEvents(ComboBoxItem.IsSelectedProperty)
					.OfType<bool>()
					.Subscribe(value => {
						PanelSystemMode.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
						setDateTimeMode = SetDateTimeMode.SyncWithComp;
					})
			);

			disposables.Add(
				setManual
					.GetPropertyChangedEvents(ComboBoxItem.IsSelectedProperty)
					.OfType<bool>()
					.Subscribe(value => {
						PanelManualMode.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
						setDateTimeMode = SetDateTimeMode.SetManually;
					})
			);

			btnCustomTZ.Click += new RoutedEventHandler((o, e) => {
				string rowStr = "";
				if (timeZonesComboBox.SelectedItem != null) {
					var tz = timeZonesComboBox.SelectedItem as TimeZoneViewModel;
					if (tz != null) {
						if (tz.posixTz != null) {
							rowStr = tz.posixTz.Format();
						} else {
							rowStr = tz.originalString;
						}
						
					}
				} else {
					rowStr = timeZonesComboBox.Text;
				}
				var dlg = new ManualTZ(rowStr);
				if (dlg.ShowDialog() == true) {
					timeZonesComboBox.Text = dlg.posixTZ;
					valueNewTime.Invalidate();
					//valueNewTime.TimeZoneChanged(new TimeZoneViewModel(dlg.posixTZ, dlg.posixTZ, PosixTz.TryParse(dlg.posixTZ)));
				}
			});
		}

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new TimeSettingsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		CompositeDisposable disposables = new CompositeDisposable();
		public ICommand CancelCommand { get; private set; }
		public PropertyTimeZoneStrings Strings { get { return PropertyTimeZoneStrings.instance; } }
		public Model model;

		public SetDateTimeMode setDateTimeMode { get; set; }
		public bool IsNTPFromDHCP { get; set; }
		public string NtpServerPath { get; set; }

		public LinkButtonsStrings Title { get { return LinkButtonsStrings.instance; } }

		void Localization() {
			daylightCheckBox.CreateBinding(CheckBox.ContentProperty, Strings, s => s.autoAdjustString);
			ntpSettingsToolTip.CreateBinding(
				TextBlock.TextProperty, Strings,
				s => s.ntpSetupInfo
			);
			deviceDateTimeCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.currentTime);
			syncWithNtp.CreateBinding(ComboBoxItem.ContentProperty, Strings, s => s.synchronizeWithNtp);
			setManual.CreateBinding(ComboBoxItem.ContentProperty, Strings, s => s.manually);
			timeZoneGroupBox.CreateBinding(GroupBox.HeaderProperty, Strings, s => s.timeZone);
            timeSettingsGroupBox.CreateBinding(GroupBox.HeaderProperty, Strings, s => s.timeSettings);
			newCompTimeCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.newDateTime);
			applyButton.CreateBinding(Button.ContentProperty, SaveCancelStrings.instance, s => s.save);
			cancelButton.CreateBinding(Button.ContentProperty, SaveCancelStrings.instance, s => s.cancel);
			newManualTimeCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.newDateTime);
			syncWithComp.CreateBinding(ComboBoxItem.ContentProperty, Strings, s => s.synchronizeWithComp);
		}

		void OnRevertChanges() {
			//valueDeviceTime.Stop();
			//valueDeviceTime.Stop();
			//valueNewTime.Stop();
			model.RevertChanges();
			disposables.Dispose();
			disposables = new CompositeDisposable();
			InitTimeZones();
			Init();
			//BindModel(model);
		}

		void OnApplyChanges() {
			var tzm = GetTimeZoneSelection();
			if (tzm == null) {
				Error(new Exception("time zone can't be null"));
				return;
			}
			string tz = null;
			if (tzm.originalString != null) {
				tz = tzm.originalString;
			} else if (tzm.posixTz != null) {
				tz = tzm.posixTz.Format();
			}
			model.timeZone = tz;

			switch (setDateTimeMode) {
				case SetDateTimeMode.SyncWithComp:
					Success(new Result.SyncWithSystem(model));
					return;
				case SetDateTimeMode.SetManually:
					if (newManualDateValue.SelectedDate == null) {
						Error(new Exception("date wasn't selected"));
						return;
					}
					var manUtcDate = newManualDateValue.SelectedDate.Value;
					var newUtc = new DateTime(
						manUtcDate.Year, manUtcDate.Month, manUtcDate.Day,
						newManualTimeValue.SelectedHour, newManualTimeValue.SelectedMinute, newManualTimeValue.SelectedSecond
					);
					if (tzm.posixTz != null) {
						try {
							var systz = tzm.posixTz.ToSystemTimeZone(newUtc.Year, model.daylightSavings);
							if (systz.IsInvalidTime(newUtc)) {
								Error(new Exception("date wasn't selected"));
								return;
							} if (systz.IsAmbiguousTime(newUtc)) {
								Error(new Exception("date time is ambiguous"));
								return;
							}
							newUtc = TimeZoneInfo.ConvertTimeToUtc(newUtc, systz);
						} catch (Exception err) {
							dbg.Error(err);
							Error(new Exception("selected posix time zone can not be converted to system time zone", err));
							return;
						}
					}
					Success(new Result.SetManual(model, newUtc));
					return;
				case SetDateTimeMode.SyncWithNtp:
					Success(new Result.SyncWithNtp(model));
					return;
				default:
					Error(new Exception("invalid mode"));
					return;
			}
		} 

		public void Dispose() {
			valueDeviceTime.Dispose();
			valueNewTime.Dispose();
			Cancel();
		}
	}
	public class TimeZoneViewModel {
		[Serializable]
		[XmlRoot("tz")]
		public class ManualTimeZone {
			[XmlAttribute("name")]
			public string displayName;
			[XmlAttribute("value")]
			public string posixTz;
		}
		[Serializable]
		[XmlRoot("time-zones")]
		public class ManualTimeZones {
			[XmlElement("tz")]
			public ManualTimeZone[] timeZones;
		}

		public readonly string displayName;
		public readonly string originalString;
		public readonly PosixTz posixTz;
		public TimeZoneViewModel(string displayName, string originalString, PosixTz posixTz) {
			this.displayName = displayName;
			this.originalString = originalString;
			this.posixTz = posixTz;
		}
		public override string ToString() {
			if (!String.IsNullOrWhiteSpace(displayName)) {
				return displayName;
			} else if (!String.IsNullOrWhiteSpace(originalString)) {
				return originalString;
			} else if ((object)posixTz != null) {
				return posixTz.Format();
			}
			return "<null>";
		}
		static TimeZoneInfo.AdjustmentRule GetSystemTimeZoneRule(DateTime now, TimeZoneInfo tzi) {
			if (!tzi.SupportsDaylightSavingTime) {
				return null;
			}
			var rules = tzi.GetAdjustmentRules();
			if (rules == null) {
				return null;
			}
			foreach (var rule in rules) {
				if ((rule.DateStart <= now) && (rule.DateEnd >= now)) {
					return rule;
				}
			}
			return null;
		}

		public static TimeZoneViewModel[] GetSystemTimeZones() {
			var tz_list = new List<TimeZoneViewModel>(256);
			var sys_tzs = TimeZoneInfo.GetSystemTimeZones();
			var now = DateTime.Now;
			foreach (var tzi in sys_tzs) {
				PosixTz.Dst dst = null;
				var rule = GetSystemTimeZoneRule(now, tzi);
				var std_offset = -tzi.BaseUtcOffset.TotalSeconds.ToInt32();
				if (rule != null) {
					var start = PosixTzExtensions.GetPosixRuleFromTransitionTime(
						rule.DaylightTransitionStart
					);
					var end = PosixTzExtensions.GetPosixRuleFromTransitionTime(
						rule.DaylightTransitionEnd
					);
					var dst_offset = std_offset - rule.DaylightDelta.TotalSeconds.ToInt32();
					dst = new PosixTz.Dst(
						/*PosixTzWriter.NormalizeName(tzi.DaylightName)*/"DaylightTime", dst_offset, start, end
					);
				}
				var posixTz = new PosixTz(
					PosixTzWriter.NormalizeName(/*tzi.StandardName*/tzi.Id), std_offset, dst
				);
				tz_list.Add(new TimeZoneViewModel(
					tzi.DisplayName, null, posixTz
				));
			}

			return tz_list.OrderBy(x =>
				new Tuple<int, string>(-x.posixTz.offset, x.displayName)
			).ToArray();
		}

		public static TimeZoneViewModel[] GetManualTimeZones(string fileName) {
			ManualTimeZones mtzs = null;
			try {
				var doc = new XmlDocument();
				doc.Load(fileName);
				mtzs = doc.DocumentElement.Deserialize<TimeZoneViewModel.ManualTimeZones>();
			} catch (Exception err) {
				dbg.Error(err);
				return new TimeZoneViewModel[0];
			}
			if (mtzs == null || mtzs.timeZones == null || mtzs.timeZones.Length == 0) {
				return new TimeZoneViewModel[0];
			}

			var tz_list = new List<TimeZoneViewModel>(256);
			foreach (var tz in mtzs.timeZones) {
				if (tz.posixTz != null) {
					var posixTz = PosixTz.TryParse(tz.posixTz);
					tz_list.Add(new TimeZoneViewModel(
						tz.displayName, tz.posixTz, posixTz
					));
				}
			}

			return tz_list.OrderBy(x => {
				if (x.posixTz != null) {
					return new Tuple<int, int, string>(0, -x.posixTz.offset, x.displayName);
				} else {
					return new Tuple<int, int, string>(1, 0, x.displayName);
				}
			}).ToArray();

		}

		public static bool LogicallyEquals(TimeZoneViewModel left, TimeZoneViewModel right) {
			return Object.ReferenceEquals(left, right) || (
				!Object.ReferenceEquals(left, null) && (
					PosixTz.LogicallyEquals(left.posixTz, right.posixTz) &&
					((object)left.posixTz != null || left.originalString == right.originalString)
				)
			);
		}
		public bool LogicallyEquals(TimeZoneViewModel other) {
			return LogicallyEquals(this, other);
		}
		public static int GetLogicalHashCode(TimeZoneViewModel tzm) {
			if ((object)tzm == null) {
				return 0;
			}
			if ((object)tzm.posixTz != null) {
				return tzm.posixTz.GetLogicalHashCode();
			}
			return HashCode.Get(tzm.originalString);
		}
		public int GetLogicalHashCode() {
			return GetLogicalHashCode(this);
		}

		public override bool Equals(object obj) {
			return Equals(obj as TimeZoneViewModel);
		}
		public bool Equals(TimeZoneViewModel other) {
			return !Object.ReferenceEquals(other, null) && (
				(posixTz == other.posixTz) &&
				((object)posixTz != null || originalString == other.originalString)
			);
		}
		public override int GetHashCode() {
			if ((object)posixTz != null) {
				return posixTz.GetHashCode();
			}
			return HashCode.Get(originalString);
		}
		public static bool operator ==(TimeZoneViewModel left, TimeZoneViewModel right) {
			return Object.ReferenceEquals(left, right) || (
				!Object.ReferenceEquals(left, null) && left.Equals(right)
			);
		}
		public static bool operator !=(TimeZoneViewModel left, TimeZoneViewModel right) {
			return !(left == right);
		}
	}
}
