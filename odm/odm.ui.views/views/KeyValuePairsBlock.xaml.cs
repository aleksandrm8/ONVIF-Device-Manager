using odm.ui.controls;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using utils;

namespace odm.ui.views {
    /// <summary>
    /// Interaction logic for KeyValuePairsBlock.xaml
    /// </summary>
    public partial class KeyValuePairsBlock : UserControl {

        readonly IDictionary<string, Tuple<Control, Type>> valueControls = new Dictionary<string, Tuple<Control, Type>>();

        public KeyValuePairsBlock() {
            InitializeComponent();
        }

        private static Control CreateItem(string name, object val, int row, Grid parent) {
            var caption = new TextBlock() { Text = name, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(2, 2, 4, 2) };

            Control value;
            if (val is string || val is double)
                value = new TextBox() { Text = val.ToString(), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(2, 2, 2, 2) };
            else if (val is int)
                value = new ControlNumericTextBox() { Text = val.ToString(), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(2, 2, 2, 2) };
            else if (val is bool)
                value = new CheckBox() { IsChecked = (bool)val, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(2, 2, 2, 2) };
            else
                throw new ArgumentException("val type is not supported");

            caption.SetValue(Grid.ColumnProperty, 0);
            value.SetValue(Grid.ColumnProperty, 1);
            caption.SetValue(Grid.RowProperty, row);
            value.SetValue(Grid.RowProperty, row);

            parent.Children.Add(caption);
            parent.Children.Add(value);

            return value;
        }

        public void InitValues(IDictionary<string, object> defaultValues) {
            valueControls.Clear();
            items.Children.Clear();
            items.RowDefinitions.Clear();

            var defVals = defaultValues.ToList();
            for (int i = 0; i < defVals.Count; ++i) {
                var p = defVals[i];

                items.RowDefinitions.Add(new RowDefinition());
                var control = CreateItem(p.Key, p.Value, i, items);
                valueControls.Add(p.Key, new Tuple<Control, Type>(control, p.Value.GetType()));
            }
        }

        public void SetValues(IDictionary<string, object> newValues) {
            foreach (var newValue in newValues) {
                var key = newValue.Key;
                var value = newValue.Value;
                if (value.GetType() != valueControls[key].Item2)
                    throw new InvalidOperationException("types mismatch");

                if (value is string || value is int)
                    ((TextBox)valueControls[key].Item1).Text = value.ToString();
                else if (value is double)
                    ((TextBox)valueControls[key].Item1).Text = ((double)value).ToString(CultureInfo.InvariantCulture);
                else if (value is bool)
                    ((CheckBox)valueControls[key].Item1).IsChecked = (bool)value;
                else
                    throw new ArgumentException("newValues contains a value of an unsupported type");
            }

        }

        public IDictionary<string, object> GetValues() {
            return valueControls.ToDictionary(p => p.Key, new Func<KeyValuePair<string, Tuple<Control,Type>>,object>(p => 
            {
                var control = valueControls[p.Key].Item1;
                var type = valueControls[p.Key].Item2;
                if (type == typeof(string))
                    return ((TextBox)control).Text;
                else if (type == typeof(double))
                    return ((TextBox)control).Text.ParseInvariant();
                else if (type == typeof(int))
                    return int.Parse(((TextBox)control).Text);
                else if (type == typeof(bool))
                    return ((CheckBox)control).IsChecked;
                else
                    throw new InvalidOperationException();
            }));
        }
    }
}
