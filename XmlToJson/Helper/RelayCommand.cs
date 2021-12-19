using System;
using System.Windows.Input;

namespace XmlToJson
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Predicate<object> _canExecute;
        public RelayCommand(Action method, Predicate<object> canExecute)
        {
            _execute = method ?? throw new ArgumentNullException("execute");

            if (canExecute != null)
            {
                this._canExecute = canExecute;
            }
        }
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }
    }
}