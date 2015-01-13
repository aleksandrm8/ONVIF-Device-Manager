using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Xsl;

using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;

using odm.core;
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views;
using onvif.services;
using utils;
using System.ComponentModel;
using System.Reactive.Linq;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for EngineControlsView.xaml
	/// </summary>
	public partial class EngineControlsView : UserControl, IDisposable, INotifyPropertyChanged {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineControlsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		public static void Start(Model model, IUnityContainer container, ContentColumn content) {
			var view = new EngineControlsView(model, container);

			content.Content = view;
		}
		#endregion

		public EngineControlsView(Model model, IUnityContainer container) {
			InitializeComponent();

			Init(model); 
		}

		CompositeDisposable disposables = new CompositeDisposable();
		Model model;

		public void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.model = model;
			InitializeComponent();

			SelectCommand = new DelegateCommand(
				() => Success(
					new Result.Select(model.selection)
					),
				() => controls.SelectedItem != null
			);
			btnSelect.Command = SelectCommand;

			CreateCommand = new DelegateCommand(
				() => Success(new Result.Create()),
				() => true
			);
			btnCreate.Command = CreateCommand;

			DeleteCommand = new DelegateCommand(
				() => Success(new Result.Delete(model.selection)),
				() => controls.SelectedItem != null
			);
			btnDelete.Command = DeleteCommand;

			ModifyCommand = new DelegateCommand(
				() => Success(new Result.Modify(model.selection)),
				() => controls.SelectedItem != null
			);
			btnModify.Command = ModifyCommand;

			List<ExtendedControl> excontrols = new List<ExtendedControl>();

			if(model.controlstates != null)
			model.controlstates.ForEach(ctr=>{
				excontrols.Add(new ExtendedControl(ctr.Key, ctr.Value));
			});

			disposables.Add(Observable.FromEventPattern<SelectionChangedEventArgs>(controls, "SelectionChanged")
				.Subscribe(ev => {
					var exitem = controls.SelectedItem as ExtendedControl;
					if (exitem == null) return;

					captionVAC.Text = exitem.cntrl.engineConfigToken;

					var s = SelectCommand as DelegateCommand;
					if (s != null) s.RaiseCanExecuteChanged();
					var mod = ModifyCommand as DelegateCommand;
					if (mod != null) mod.RaiseCanExecuteChanged();
					var d = DeleteCommand as DelegateCommand;
					if (d != null) d.RaiseCanExecuteChanged();
				}));
			controls.ItemsSource = excontrols;
			controls.CreateBinding(ListBox.SelectedItemProperty, model, x=>excontrols.FirstOrDefault(f => f.cntrl == x.selection), (m,v)=>{
				m.selection = v.cntrl;
			});

			Localization();
		}

		void Localization() {
			captionMode.CreateBinding(TextBlock.TextProperty, LocalEngineControl.instance, s => s.mode);
			captionName.CreateBinding(TextBlock.TextProperty, LocalEngineControl.instance, s => s.name);
			captionStatus.CreateBinding(TextBlock.TextProperty, LocalEngineControl.instance, s => s.status);
			captionVAC.CreateBinding(TextBlock.TextProperty, LocalEngineControl.instance, s => s.vac);
			captionDetails.CreateBinding(TextBlock.TextProperty, LocalEngineControl.instance, s => s.details);

			btnCreate.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.create);
			btnDelete.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.delete);
			btnModify.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.modify);
			btnSelect.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.select);
		}
		
		public void Dispose() {
			disposables.Dispose();
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
	public class ExtendedControl {
		public ExtendedControl(AnalyticsEngineControl cntrl, AnalyticsState state) {
			this.cntrl = cntrl;
			this.state = state;
		}
		public AnalyticsEngineControl cntrl { get; private set; }
		public AnalyticsState state { get; private set; }
		public string Name { get { return cntrl.name; } }
		public ModeOfOperation Mode { get { return cntrl.mode; } }
		public AnalyticsState State { get { return state; } }
	}
}
