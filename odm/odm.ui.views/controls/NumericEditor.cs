using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using utils;
using Microsoft.Practices.Prism.Commands;
using System.Text.RegularExpressions;
using System.Globalization;

namespace odm.ui.controls {
	public class IntegerTextBox : TextBox {
		public IntegerTextBox() {
			this.AllowDrop = false;

			btnMinus = new DelegateCommand(() => {
				Value--;
			});
			btnPlus = new DelegateCommand(() => {
				Value++;
			});
		}
		bool IsNumeric(string val) {
			int result;
			return Int32.TryParse(val, System.Globalization.NumberStyles.Integer,CultureInfo.InvariantCulture, out result);
		}
		protected override void OnTextChanged(TextChangedEventArgs e) {
			if (IsNumeric(Text))
				base.OnTextChanged(e);
			else
				Text = Value.ToString();
		}
		protected override void OnTextInput(System.Windows.Input.TextCompositionEventArgs e) {
			if (!IsNumeric(e.Text))
				return;
			int result;
			if (this.Text != "") {
				bool expr = Int32.TryParse(this.Text, out result) && result <= MaxValue && result >= MinValue;
				if (expr)
					base.OnTextInput(e);
			} else {
				base.OnTextInput(e);
			}
		}

		public string WarningText {get { return (string)GetValue(WarningTextProperty); } set { SetValue(WarningTextProperty, value); }}
		public static readonly DependencyProperty WarningTextProperty = DependencyProperty.Register("WarningText", typeof(string), typeof(IntegerTextBox), new UIPropertyMetadata(""));

		public int Value { 
			get {
				int result = 0;
				string text = (string)GetValue(TextProperty);
				if(Int32.TryParse(text, out result)){
				}
				return result; 
			} set { 
				SetValue(TextProperty, value.ToString()); 
			} 
		}

		public int MinValue {get { return (int)GetValue(MinValueProperty); }set { SetValue(MinValueProperty, value); }}
		public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(IntegerTextBox), new UIPropertyMetadata(0));

		public int MaxValue {get { return (int)GetValue(MaxValueProperty); }set { SetValue(MaxValueProperty, value); }}
		public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(IntegerTextBox), new UIPropertyMetadata(0));

		public DelegateCommand btnMinus {get { return (DelegateCommand)GetValue(btnMinusProperty); }set { SetValue(btnMinusProperty, value); }}
		public static readonly DependencyProperty btnMinusProperty = DependencyProperty.Register("btnMinus", typeof(DelegateCommand), typeof(IntegerTextBox));

		public DelegateCommand btnPlus { get { return (DelegateCommand)GetValue(btnPlusProperty); } set { SetValue(btnPlusProperty, value); }}
		public static readonly DependencyProperty btnPlusProperty = DependencyProperty.Register("btnPlus", typeof(DelegateCommand), typeof(IntegerTextBox));
	}
}
