﻿using System.Windows.Input;

namespace Resident.Service
{
    public class LocalRelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public LocalRelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Manually raise the CanExecuteChanged event to force the UI to re-check CanExecute.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// A convenience alias for RaiseCanExecuteChanged(), 
        /// matching the naming convention used by many MVVM libraries.
        /// </summary>
        public void NotifyCanExecuteChanged()
            => RaiseCanExecuteChanged();

    }
}
