using System;
using System.Windows;
using odm.ui.core;

namespace odm.ui.viewModels {
    public class InfoViewModel:DependencyObject {
        public InfoViewModel(string message) {
            Message = message;

            InitCommands();
        }

        public Action close;

        void InitCommands() {
            buttonClick = new SimpleCommand(execute => {
                if(close != null)
                    close();
            });
        }

        public SimpleCommand buttonClick {
            get { return (SimpleCommand)GetValue(buttonClickProperty); }
            set { SetValue(buttonClickProperty, value); }
        }
        public static readonly DependencyProperty buttonClickProperty = DependencyProperty.Register("buttonClick", typeof(SimpleCommand), typeof(InfoViewModel));

        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(InfoViewModel), new UIPropertyMetadata(""));

        public string Message {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(InfoViewModel));
    }
}
