using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace utils {
    public class RelayCommand : ICommand {

        readonly Action<object> _execute;
        readonly Func<object, bool> _canExecute;
		readonly Action<Exception> _onerror;

		public RelayCommand(Action<object> execute, Action<Exception> onErr, Func<object, bool> canExecute = null) {
            if (execute == null)
                throw new ArgumentNullException("execute");
			if (onErr == null)
				throw new ArgumentNullException("on error");

            this._execute = execute;
            this._canExecute = canExecute;
			this._onerror = onErr;
        }

        public bool CanExecute(object parameter) {
			try {
				return _canExecute == null ? true : _canExecute(parameter);
			} catch (Exception err) {
				_onerror(err);
				return false;
			}
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) {
			if (_canExecute == null || CanExecute(parameter)) {
				try {
					_execute(parameter);
				} catch (Exception err) {
					_onerror(err);
				}
			}
        }

        public static void InvalidateCanExecute()
        {
            CommandManager.InvalidateRequerySuggested();
        }

    }
}
