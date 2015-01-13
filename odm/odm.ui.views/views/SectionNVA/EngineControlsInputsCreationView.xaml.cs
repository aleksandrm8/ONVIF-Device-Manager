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
using Microsoft.Practices.Unity;
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;
using utils;
using System.Reactive.Disposables;
using Microsoft.Practices.Prism.Commands;
using System.Reactive.Linq;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for EngineControlsInputsCreationView.xaml
	/// </summary>
	public partial class EngineControlsInputsCreationView : UserControl, IDisposable {
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new EngineControlsInputsCreationView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion Activity definition
		public EngineControlsInputsCreationView() {
			InitializeComponent();
		}

		CompositeDisposable disposables = new CompositeDisposable();
		Model model;
		public List<InputReceiverPair> inputsList { get; private set; }

		public void Init(Model model) {
			inputsList = new List<InputReceiverPair>();

			OnCompleted += () => {
				disposables.Dispose();
			};
			this.model = model;

			InitializeComponent();

			int count = 0;
			if (model.inputs != null)
				count = model.inputs.Count();

			for (int i = 0; i < count; i++) {
				Receiver rec = null;
				AnalyticsEngineInput inp = null;

				if (model.receivers != null && model.receivers.Count() > i)
					rec = model.receivers[i];

				if (model.inputs != null && model.inputs.Count() > i)
					inp = model.inputs[i];

				inputsList.Add(new InputReceiverPair(rec, inp));
			}

			inputsList.ForEach(inp => {
				CreateInputUI(inp);
			});

			FinishCommand = new DelegateCommand(
				() => Success(new Result.Finish(model)),
				() => true
			);
			btnFinish.Command = FinishCommand;

			AbortCommand = new DelegateCommand(
				() => Success(new Result.Abort()),
				() => true
			);
			btnAbort.Command = AbortCommand;
			
			Localization();
		}

		void Localization() {
			captionInputs.CreateBinding(TextBlock.TextProperty, LocalEngineControlsInputsCreation.instance, s=>s.input);

			btnAbort.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.abort);
			btnFinish.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.finish);
		}

		public void CreateInputUI(InputReceiverPair inp) {
			TextBlock inputText = new TextBlock() { 
				Text = inp.input, 
				Margin = new Thickness(0, 2, 0, 2), 
				Height = 24, 
				VerticalAlignment = System.Windows.VerticalAlignment.Center
			};
			panelInputs.Children.Add(inputText);

			TextBlock receiverText = new TextBlock() { 
				Text = inp.receiver, 
				Margin = new Thickness(0, 2, 0, 2), 
				Height = 24, 
				VerticalAlignment = System.Windows.VerticalAlignment.Center 
			};
			panelReceivers.Children.Add(receiverText);

			Button select = new Button() { 
				Height=24, 
				Margin = new Thickness(0, 2, 0, 2), 
				VerticalAlignment = System.Windows.VerticalAlignment.Center,
				Content = "..."
			};

			disposables.Add(Observable.FromEventPattern(select, "Click").Subscribe(c => {
				Success(new Result.Modify(inp.inp));
			}));

			panelButtons.Children.Add(select);
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
	public class InputReceiverPair {
		public InputReceiverPair(Receiver rec, AnalyticsEngineInput inp) {
			this.rec = rec;
			this.inp = inp;
		}
		public Receiver rec { get; protected set; }
		public AnalyticsEngineInput inp { get; protected set; }
		public string receiver {
			get {
				if (rec == null)
					return "None";
				return rec.token;
			}
		}
		public string input { 
			get {
				if (inp == null)
					return "None";
				if (inp.name == null || inp.name == "")
					return inp.token;
				return inp.name;
			} 
		}
	}
}
