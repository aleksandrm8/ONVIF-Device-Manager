using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using utils;

namespace odm.ui.activities {
	public partial class ItemSelectorView : UserControl {
		public enum Flags {
			CanCreate = 1,
			CanDelete = 2,
			CanModify = 4,
			CanSelect = 8,
			CanClose = 16,
			NoOperationsAvailable = 0,
			AllOperationsAvailable = -1
		}

		public enum ItemFlags {
			CanBeDeleted = 1,
			CanBeModified = 2,
			CanBeSelected = 4,
			NoOperationsAvailable = 0,
			AllOperationsAvailable = -1,
		}

		public class Item {
			public Item(string name, ItemProp[] details, ItemFlags flags = ItemFlags.AllOperationsAvailable) {
				this.name = name;
				this.details = details;
				this.flags = flags;
			}
			public ItemFlags flags { get; private set; }
			public string name { get; private set; }
			public ItemProp[] details { get; private set; }
			public override string ToString() {
				return name;
			}
		}

		public class ItemProp {
			public ItemProp(string name, string value) {
				this.name = name;
				this.value = value;
				this.childs = null;
			}
			public ItemProp(string name, string value, ItemProp[] childs) {
				this.name = name;
				this.value = value;
				this.childs = childs;
			}
			public string name { get; private set; }
			public string value { get; private set; }
			public ItemProp[] childs { get; private set; }
			public bool hasChilds { get { return childs != null && childs.Length > 0; } }
		}

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ItemSelectorView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		public LocalButtons ButtonsStrings { get { return LocalButtons.instance; } }
		public PropertyVideoStreamingStrings Strings { get { return PropertyVideoStreamingStrings.instance; } }
		public CommonApplicationStrings CommmonStrings { get { return CommonApplicationStrings.instance; } }

		Model model;

		private void Init(Model model) {
			this.model = model;
			this.DataContext = model;

			SelectCommand = new DelegateCommand(
				() => Success(new Result.Select(model.selection)),
				() => (model.flags & Flags.CanSelect) == Flags.CanSelect
			);
			CreateCommand = new DelegateCommand(
				() => Success(new Result.Create()),
				() => (model.flags & Flags.CanCreate) == Flags.CanCreate
			);
			DeleteCommand = new DelegateCommand(
				() => Success(new Result.Delete(model.selection)),
				() => (model.flags & Flags.CanDelete) == Flags.CanDelete
			);
			ModifyCommand = new DelegateCommand(
				() => Success(new Result.Modify(model.selection)),
				() => (model.flags & Flags.CanModify) == Flags.CanModify
			);
			CloseCommand = new DelegateCommand(
				() => Success(new Result.Close()),
				() => (model.flags & Flags.CanClose) == Flags.CanClose
			);

			InitializeComponent();
			BindData(model);
		}

		StackPanel GetDetailView(string name, string detail) {
			StackPanel panel = new StackPanel();
			panel.Margin = new Thickness(2);
			panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			panel.Orientation = Orientation.Horizontal;
			panel.Children.Add(new TextBlock() { Foreground = Brushes.Black, FontWeight = FontWeights.Normal, Text = name + ":", Margin = new Thickness(0, 0, 10, 0) });
			panel.Children.Add(new TextBlock() { Text = detail, Foreground = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100)), FontWeight = FontWeights.Normal });
			return panel;
		}

		void FillItemProperty(ItemProp itProp, Expander parentcontainer) {
			StackPanel container = new StackPanel();
			container.Orientation = Orientation.Vertical;
			container.Margin = new Thickness(40, 0, 0, 0);

			itProp.childs.ForEach(v => {
				if (v.hasChilds) {
					Expander itemExpander = new Expander();
					itemExpander.IsExpanded = IsStartExpanded;
					itemExpander.Header = GetDetailView(v.name, v.value);
					itemExpander.Style = (Style)FindResource("expander");

					FillItemProperty(v, itemExpander);
					container.Children.Add(itemExpander);
				} else {
					container.Children.Add(GetDetailView(v.name, v.value));
				}
				parentcontainer.Content = container;
			});
		}
		void InitDetails() {
			IsStartExpanded = AppDefaults.visualSettings.ItemSelector_IsPropertiesExpanded;
			if (model.selection == null) {
				details.Child = null;
				return;
			}
			StackPanel container = new StackPanel();
			container.Orientation = Orientation.Vertical;

			var selection = model.selection;
			selection.details.ForEach(v => {
				if (v.hasChilds) {
					Expander itemExpander = new Expander();
					itemExpander.IsExpanded = IsStartExpanded;
					itemExpander.Style = (Style)FindResource("expander");
					//itemExpander.Foreground = Brushes.Gray;
					itemExpander.Header = GetDetailView(v.name, v.value);

					FillItemProperty(v, itemExpander);
					container.Children.Add(itemExpander);
				} else {
					container.Children.Add(GetDetailView(v.name, v.value));
				}
			});

			details.Child = container;
		}

		public bool IsStartExpanded {
			get { return (bool)GetValue(IsStartExpandedProperty); }
			set { SetValue(IsStartExpandedProperty, value); }
		}
		public static readonly DependencyProperty IsStartExpandedProperty =
			DependencyProperty.Register("IsStartExpanded", typeof(bool), typeof(ItemSelectorView), new PropertyMetadata((obj, evarg) => {
				var visset = AppDefaults.visualSettings;
				visset.ItemSelector_IsPropertiesExpanded = (bool)evarg.NewValue;
				AppDefaults.UpdateVisualSettings(visset);
			}));

		void RefreshItemButtons(Model model) {
			var selection = model.selection;
			btnDelete.IsEnabled = selection != null && (selection.flags & ItemFlags.CanBeDeleted) != 0;
			btnModify.IsEnabled = selection != null && (selection.flags & ItemFlags.CanBeModified) != 0;
			btnSelect.IsEnabled = selection != null && (selection.flags & ItemFlags.CanBeSelected) != 0;
		}
		void BindData(Model model) {
			ItemsList.CreateBinding(ListBox.SelectedItemProperty, model, x => x.selection, (m, v) => {
				m.selection = v;
				RefreshItemButtons(m);
				InitDetails();
			});
			RefreshItemButtons(model);
			
			ItemsList.CreateBinding(ListBox.ItemsSourceProperty, model, x => x.items);

			InitDetails();

			if ((model.flags & Flags.CanCreate) != Flags.CanCreate)
				btnCreate.Visibility = System.Windows.Visibility.Collapsed;
			if ((model.flags & Flags.CanDelete) != Flags.CanDelete)
				btnDelete.Visibility = System.Windows.Visibility.Collapsed;
			if ((model.flags & Flags.CanModify) != Flags.CanModify)
				btnModify.Visibility = System.Windows.Visibility.Collapsed;
			if ((model.flags & Flags.CanSelect) != Flags.CanSelect)
				btnSelect.Visibility = System.Windows.Visibility.Collapsed;
			if ((model.flags & Flags.CanClose) != Flags.CanClose)
				btnClose.Visibility = System.Windows.Visibility.Collapsed;

			CheckButtons();
		}

		void CheckButtons() {
			if (ItemsList.SelectedItem == null) {
				btnSelect.IsEnabled = false;
				btnModify.IsEnabled = false;
				btnDelete.IsEnabled = false;
			}
		}
	}
}
