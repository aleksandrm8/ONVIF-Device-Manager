using System;
using System.Windows;
using System.Windows.Controls;
using utils;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Data;
using System.Reflection;
using System.Globalization;

namespace odm.ui.views {
	public class NullSelectionToBooleanConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) return false;
			PropertyInfo propertyInfo = value.GetType().GetProperty("SelectedItem");
			if (propertyInfo != null) {
				var item = propertyInfo.GetValue(value, null);
				return item != null;
			}
			if (!(value is bool || value is bool?)) return true;
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return value;
		}
	}

    public class ListBoxUtility {
        public static readonly DependencyProperty IsUnselectOnRBTNProperty =
            DependencyProperty.RegisterAttached("IsUnselectOnRBTN", typeof(bool), typeof(ListBoxUtility), new PropertyMetadata(OnIsUnselectChanged));

        public static bool GetIsUnselectOnRBTN(DependencyObject dependencyObject) {
            return (bool)dependencyObject.GetValue(IsUnselectOnRBTNProperty);
        }

        public static void SetIsUnselectOnRBTN(DependencyObject dependencyObject, bool IsUnselectOnRBTN) {
            dependencyObject.SetValue(IsUnselectOnRBTNProperty, IsUnselectOnRBTN);
        }

        private static void OnIsUnselectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var lbox = (ListBox)d;
            if (((bool)e.NewValue) == true) {
                lbox.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(lbox_MouseRightButtonUp);
            } else {
                lbox.MouseRightButtonUp -= lbox_MouseRightButtonUp;
            }
        }

        static void lbox_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            try {
                var lbox = (ListBox)sender;
                lbox.SelectedItem = null;
            } catch (Exception err) {
                dbg.Error(err);
            }
        }

        public static readonly DependencyProperty LastItemProperty =
            DependencyProperty.RegisterAttached("LastItem", typeof(Object), typeof(ListBoxUtility), new PropertyMetadata(OnLastItemChanged));

        public static Object GetLastItem(DependencyObject dependencyObject) {
            return (Object)dependencyObject.GetValue(LastItemProperty);
        }

        public static void SetLastItem(DependencyObject dependencyObject, Object LastItem) {
            dependencyObject.SetValue(LastItemProperty, LastItem);
        }

        private static void OnLastItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            try {
                var lbox = (ListBox)d;
                if (e.NewValue != null && lbox.SelectedItem == null) {
                    try {
                        lbox.ScrollIntoView(e.NewValue);
                    } catch (Exception err) {
                        dbg.Error(err);
                    }
                }

            } catch (Exception err) {
                dbg.Error(err);
            }
        }
    }

    public class GridViewSort {
        #region Attached properties

        public static ICommand GetCommand(DependencyObject obj) {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value) {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    null,
                    (o, e) => {
                        ItemsControl listView = o as ItemsControl;
                        if (listView != null) {
                            if (!GetAutoSort(listView)) // Don't change click handler if AutoSort enabled
                            {
                                if (e.OldValue != null && e.NewValue == null) {
                                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                                if (e.OldValue == null && e.NewValue != null) {
                                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                            }
                        }
                    }
                )
            );

        public static bool GetAutoSort(DependencyObject obj) {
            return (bool)obj.GetValue(AutoSortProperty);
        }

        public static void SetAutoSort(DependencyObject obj, bool value) {
            obj.SetValue(AutoSortProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoSort.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoSortProperty =
            DependencyProperty.RegisterAttached(
                "AutoSort",
                typeof(bool),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    false,
                    (o, e) => {
                        ListView listView = o as ListView;
                        if (listView != null) {
                            if (GetCommand(listView) == null) // Don't change click handler if a command is set
                            {
                                bool oldValue = (bool)e.OldValue;
                                bool newValue = (bool)e.NewValue;
                                if (oldValue && !newValue) {
                                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                                if (!oldValue && newValue) {
                                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                            }
                        }
                    }
                )
            );

        public static string GetPropertyName(DependencyObject obj) {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        public static void SetPropertyName(DependencyObject obj, string value) {
            obj.SetValue(PropertyNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new UIPropertyMetadata(null)
            );

        #endregion

        #region Column header click event handler

        private static void ColumnHeader_Click(object sender, RoutedEventArgs e) {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null) {
                string propertyName = GetPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName)) {
                    ListView listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null) {
                        ICommand command = GetCommand(listView);
                        if (command != null) {
                            if (command.CanExecute(propertyName)) {
                                command.Execute(propertyName);
                            }
                        } else if (GetAutoSort(listView)) {
                            ApplySort(listView.Items, propertyName);
                        }
                    }
                }
            }
        }

        #endregion

        #region Helper methods

        public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T)) {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
                return (T)parent;
            else
                return null;
        }

        public static void ApplySort(ICollectionView view, string propertyName) {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0) {
                SortDescription currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName) {
                    if (currentSort.Direction == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }
                view.SortDescriptions.Clear();
            }
            if (!string.IsNullOrEmpty(propertyName)) {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }
        }

        #endregion
    }
}
