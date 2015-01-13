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
using odm.models;
using odm.controllers;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for LinkCheckButton.xaml
	/// </summary>
	public partial class LinkCheckButton : UserControl {
		public LinkCheckButton() {
			InitializeComponent();

            ////_checkBox.IsChecked = true;
            //NameLable.Click += new RoutedEventHandler(_linkLabel_Click);
            //_checkBox.Checked += new RoutedEventHandler(_checkBox_Checked);
            //_checkBox.Unchecked += new RoutedEventHandler(_checkBox_Checked);
			
            ////// This part will be moved to xaml
            ////MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            ////_linkLabel.MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            ////_checkBox.MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            ////MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);
            ////_linkLabel.MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);
            ////_checkBox.MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);
            ////_linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
		}

        //protected bool _isClicked = false;
        ////protected Panel _propertyContainer;

        //public Visibility IsCheckable { get { return _checkBox.Visibility; } set { _checkBox.Visibility = value; } }
        //public bool IsChecked {
        //    get { return _checkBox.IsChecked.Value; }
        //    set {
        //        _checkBox.IsChecked = value;
        //        if (IsCheckable == System.Windows.Visibility.Visible)
        //            CheckChecked();
        //    }
        //}
        //public Action Click { get; set; }
        //public Action<LinkCheckButton> SelectionChanged { get; set; }
        //public Action<bool, Action> ChBoxSwitched { get; set; }
        //// This boolean helps to return check box to initial state if error apears
        //bool silendSwitch = false;
        //public ChannelModel Channel {
        //    get;
        //    set;
        //}
        ////public Session ModelSession { get; set; }
        //public Button NameLable {
        //    get { return _linkLabel; }
        //}
		

        //void _checkBox_Checked(object sender, RoutedEventArgs e) {
        //    if (silendSwitch == false) {
        //        CheckChecked();

        //        if (ChBoxSwitched != null)
        //            ChBoxSwitched(_checkBox.IsChecked.Value, OnSwitchError);
        //    } else
        //        silendSwitch = false; //Reset silenth switch flag
        //}

        //void _linkLabel_Click(object sender, RoutedEventArgs e) {
        //    if (!_isClicked) {
        //        Click();
        //        SelectionChanged(this);
        //    }
        //}

        //public void SetUnclicked() {
        //    _isClicked = false;
        //}
        //public void SetClicked() {
        //    _isClicked = true;
        //}

        //public void ResetLink() {
        //    _isClicked = false;
        //    NameLable.IsEnabled = true;
        //}

        //protected void CheckChecked() {
        //    NameLable.IsEnabled = IsChecked;
        //}
		
        //protected void OnSwitchError() {
        //    silendSwitch = true;
        //    IsChecked = !IsChecked;
        //    CheckChecked();
        //}

        //private void LinkCheckButton_Load(object sender, EventArgs e) {
        //}

		//void LinkCheckButton_MouseLeave(object sender, EventArgs e) {
		//    if (sender.GetType() == typeof(Label)) {
		//        if (!_isClicked) {
		//            _linkLabel.ForeColor = ColorDefinition.colLinkButtonsIitial;
		//            _linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
		//        }
		//    }
		//    if (eMouseLeave != null)
		//        eMouseLeave(sender, e);
		//}

		//void LinkCheckButton_MouseEnter(object sender, EventArgs e) {
		//    if (sender.GetType() == typeof(Label)) {
		//        if (!_isClicked) {
		//            _linkLabel.ForeColor = ColorDefinition.colLinkButtonsHovered;
		//            _linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
		//        }
		//    }
		//    if (eMouseEnter != null)
		//        eMouseEnter(sender, e);
		//}
	}
}
