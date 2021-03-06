using System;
using System.Diagnostics;
using System.Windows.Input;

namespace LcrGame.Assets
{
    public class RelayCommand : ICommand
    {

        #region private fields
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        #endregion

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null) CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null) CommandManager.RequerySuggested -= value;
            }
        }

        public RelayCommand(Action execute)
            : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            Debug.Assert(execute != null, "Execute command cannot be null");
            _execute = execute;
            _canExecute = canExecute;
        }

        public void Execute(object parameter) { _execute(); }

        public bool CanExecute(object parameter) { return _canExecute == null || _canExecute(); }

        public void RaiseCanExecuteChanged() { CommandManager.InvalidateRequerySuggested(); }
    }
}
