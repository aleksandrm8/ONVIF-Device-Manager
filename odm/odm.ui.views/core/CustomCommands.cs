using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace odm.ui.core {
    class CustomCommands {
    }
    public static class MouseUpDownBehavior {
        public static bool GetIsMouseCapture(DependencyObject obj) {
            return (bool)obj.GetValue(IsMouseCaptureProperty);
        }
        public static void SetIsMouseCapture(DependencyObject obj, bool value) {
            obj.SetValue(IsMouseCaptureProperty, value);
        }
        public static readonly DependencyProperty IsMouseCaptureProperty =
            DependencyProperty.RegisterAttached("IsMouseCapture", typeof(bool), typeof(MouseUpDownBehavior), new UIPropertyMetadata(false, OnMouseStateChanged));

        private static void OnMouseStateChanged(object sender, DependencyPropertyChangedEventArgs e) {
            Button btn = (Button)sender;

            btn.PreviewMouseDown += (obj, evargs) => {
                ExecuteMouseDownCommand((DependencyObject)sender);
            };
            btn.PreviewMouseUp += (obj, evargs) => {
                ExecuteMouseUpCommand((DependencyObject)sender);
            };
        }

        private static void ExecuteMouseUpCommand(DependencyObject obj) {
            var command = GetOnMouseUp(obj);
            if (command != null)
                command.Execute(null);
        }
        private static void ExecuteMouseDownCommand(DependencyObject obj) {
            var command = GetOnMouseDown(obj);
            if (command != null)
                command.Execute(null);
        }

        public static ICommand GetOnMouseDown(DependencyObject obj) {
            return (ICommand)obj.GetValue(OnMouseDownProperty);
        }
        public static void SetOnMouseDown(DependencyObject obj, ICommand value) {
            obj.SetValue(OnMouseDownProperty, value);
        }
        public static readonly DependencyProperty OnMouseDownProperty =
            DependencyProperty.RegisterAttached("OnMouseDown", typeof(ICommand), typeof(MouseUpDownBehavior));

        public static ICommand GetOnMouseUp(DependencyObject obj) {
            return (ICommand)obj.GetValue(OnMouseUpProperty);
        }
        public static void SetOnMouseUp(DependencyObject obj, ICommand value) {
            obj.SetValue(OnMouseUpProperty, value);
        }
        public static readonly DependencyProperty OnMouseUpProperty =
            DependencyProperty.RegisterAttached("OnMouseUp", typeof(ICommand), typeof(MouseUpDownBehavior));

    }
    public class SimpleCommand : ICommand {
        public SimpleCommand(Action<object> executeDelegate) {
            ExecuteDelegate = executeDelegate;
        }

        public SimpleCommand(Predicate<object> canExecuteDelegate, Action<object> executeDelegate) {
            CanExecuteDelegate = canExecuteDelegate;
            ExecuteDelegate = executeDelegate;
        }

        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        #region ICommand Members

        public bool CanExecute(object parameter) {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);
            return true;// if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parameter);
        }

        #endregion
    }
}
