using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace odm.ui.controls {
    public class DirectionRoseControl: ContentControl {
        public DirectionRoseControl() {
            InitCommands();
            captionNone = "None";
            captionAll = "All";
        }
        void InitCommands() {
            btnAll = new DelegateCommand(() => {
                btnUp = true;
                btnUpLeft = true;
                btnUpRight = true;
                btnLeft = true;
                btnRight = true;
                btnDown = true;
                btnDownLeft = true;
                btnDownRight = true;
            });
            btnNone = new DelegateCommand(() => {
                btnUp = false;
                btnUpLeft = false;
                btnUpRight = false;
                btnLeft = false;
                btnRight = false;
                btnDown = false;
                btnDownLeft = false;
                btnDownRight = false;
            });
            btnUpCmd = new DelegateCommand(() => {
                btnUp = !btnUp;
            });
            btnUpLeftCmd = new DelegateCommand(() => {
                btnUpLeft = !btnUpLeft;
            });
            btnUpRightCmd = new DelegateCommand(() => {
                btnUpRight = !btnUpRight;
            });
            btnDownCmd = new DelegateCommand(() => {
                btnDown = !btnDown;
            });
            btnDownLeftCmd = new DelegateCommand(() => {
                btnDownLeft = !btnDownLeft;
            });
            btnDownRightCmd = new DelegateCommand(() => {
                btnDownRight = !btnDownRight;
            });
            btnLeftCmd = new DelegateCommand(() => {
                btnLeft = !btnLeft;
            });
            btnRightCmd = new DelegateCommand(() => {
                btnRight = !btnRight;
            });
        }

        public string captionNone {
            get { return (string)GetValue(captionNoneProperty); }
            set { SetValue(captionNoneProperty, value); }
        }
        public static readonly DependencyProperty captionNoneProperty =
            DependencyProperty.Register("captionNone", typeof(string), typeof(DirectionRoseControl));

        public string captionAll {
            get { return (string)GetValue(captionAllProperty); }
            set { SetValue(captionAllProperty, value); }
        }
        public static readonly DependencyProperty captionAllProperty =
            DependencyProperty.Register("captionAll", typeof(string), typeof(DirectionRoseControl));

        public ICommand btnNone {
            get { return (ICommand)GetValue(btnNoneProperty); }
            set { SetValue(btnNoneProperty, value); }
        }
        public static readonly DependencyProperty btnNoneProperty =
            DependencyProperty.Register("btnNone", typeof(ICommand), typeof(DirectionRoseControl));

        public ICommand btnAll {
            get { return (ICommand)GetValue(btnAllProperty); }
            set { SetValue(btnAllProperty, value); }
        }
        public static readonly DependencyProperty btnAllProperty =
            DependencyProperty.Register("btnAll", typeof(ICommand), typeof(DirectionRoseControl));



        public ICommand btnUpCmd {
            get { return (ICommand)GetValue(btnUpCmdProperty); }
            set { SetValue(btnUpCmdProperty, value); }
        }
        public static readonly DependencyProperty btnUpCmdProperty =
            DependencyProperty.Register("btnUpCmd", typeof(ICommand), typeof(DirectionRoseControl));

        public ICommand btnDownCmd {
            get { return (ICommand)GetValue(btnDownCmdProperty); }
            set { SetValue(btnDownCmdProperty, value); }
        }
        public static readonly DependencyProperty btnDownCmdProperty =
            DependencyProperty.Register("btnDownCmd", typeof(ICommand), typeof(DirectionRoseControl));

        public ICommand btnLeftCmd {
            get { return (ICommand)GetValue(btnLeftCmdProperty); }
            set { SetValue(btnLeftCmdProperty, value); }
        }
        public static readonly DependencyProperty btnLeftCmdProperty =
            DependencyProperty.Register("btnLeftCmd", typeof(ICommand), typeof(DirectionRoseControl));

        public ICommand btnRightCmd {
            get { return (ICommand)GetValue(btnRightCmdProperty); }
            set { SetValue(btnRightCmdProperty, value); }
        }
        public static readonly DependencyProperty btnRightCmdProperty =
            DependencyProperty.Register("btnRightCmd", typeof(ICommand), typeof(DirectionRoseControl));

        public ICommand btnUpLeftCmd {
            get { return (ICommand)GetValue(btnUpLeftCmdProperty); }
            set { SetValue(btnUpLeftCmdProperty, value); }
        }
        public static readonly DependencyProperty btnUpLeftCmdProperty =
            DependencyProperty.Register("btnUpLeftCmd", typeof(ICommand), typeof(DirectionRoseControl));

        public ICommand btnUpRightCmd {
            get { return (ICommand)GetValue(btnUpRightCmdProperty); }
            set { SetValue(btnUpRightCmdProperty, value); }
        }
        public static readonly DependencyProperty btnUpRightCmdProperty =
            DependencyProperty.Register("btnUpRightCmd", typeof(ICommand), typeof(DirectionRoseControl));

        public ICommand btnDownLeftCmd {
            get { return (ICommand)GetValue(btnDownLeftCmdProperty); }
            set { SetValue(btnDownLeftCmdProperty, value); }
        }
        public static readonly DependencyProperty btnDownLeftCmdProperty =
            DependencyProperty.Register("btnDownLeftCmd", typeof(ICommand), typeof(DirectionRoseControl));

        public ICommand btnDownRightCmd {
            get { return (ICommand)GetValue(btnDownRightCmdProperty); }
            set { SetValue(btnDownRightCmdProperty, value); }
        }
        public static readonly DependencyProperty btnDownRightCmdProperty =
            DependencyProperty.Register("btnDownRightCmd", typeof(ICommand), typeof(DirectionRoseControl));

        
        public bool btnUp {
            get { return (bool)GetValue(btnUpProperty); }
            set { SetValue(btnUpProperty, value); }
        }
        public static readonly DependencyProperty btnUpProperty =
            DependencyProperty.Register("btnUp", typeof(bool), typeof(DirectionRoseControl), new PropertyMetadata((obj, ev) => {
                var o = (DirectionRoseControl)obj;
            }));
        
        public bool btnDown {
            get { return (bool)GetValue(btnDownProperty); }
            set { SetValue(btnDownProperty, value); }
        }
        public static readonly DependencyProperty btnDownProperty =
        DependencyProperty.Register("btnDown", typeof(bool), typeof(DirectionRoseControl));
        
        public bool btnLeft {
            get { return (bool)GetValue(btnLeftProperty); }
            set { SetValue(btnLeftProperty, value); }
        }
        public static readonly DependencyProperty btnLeftProperty =
        DependencyProperty.Register("btnLeft", typeof(bool), typeof(DirectionRoseControl));
        
        public bool btnRight {
            get { return (bool)GetValue(btnRightProperty); }
            set { SetValue(btnRightProperty, value); }
        }
        public static readonly DependencyProperty btnRightProperty =
        DependencyProperty.Register("btnRight", typeof(bool), typeof(DirectionRoseControl));
        
        public bool btnUpLeft {
            get { return (bool)GetValue(btnUpLeftProperty); }
            set { SetValue(btnUpLeftProperty, value); }
        }
        public static readonly DependencyProperty btnUpLeftProperty =
        DependencyProperty.Register("btnUpLeft", typeof(bool), typeof(DirectionRoseControl));
        
        public bool btnUpRight {
            get { return (bool)GetValue(btnUpRightProperty); }
            set { SetValue(btnUpRightProperty, value); }
        }
        public static readonly DependencyProperty btnUpRightProperty =
        DependencyProperty.Register("btnUpRight", typeof(bool), typeof(DirectionRoseControl));
        
        public bool btnDownLeft {
            get { return (bool)GetValue(btnDownLeftProperty); }
            set { SetValue(btnDownLeftProperty, value); }
        }
        public static readonly DependencyProperty btnDownLeftProperty =
        DependencyProperty.Register("btnDownLeft", typeof(bool), typeof(DirectionRoseControl));

        public bool btnDownRight {
            get { return (bool)GetValue(btnDownRightProperty); }
            set { SetValue(btnDownRightProperty, value); }
        }
        public static readonly DependencyProperty btnDownRightProperty =
            DependencyProperty.Register("btnDownRight", typeof(bool), typeof(DirectionRoseControl));
    }
}
