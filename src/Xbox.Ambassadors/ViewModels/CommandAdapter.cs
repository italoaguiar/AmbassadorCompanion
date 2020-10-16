using System;
using System.Windows.Input;

namespace Xbox.Ambassadors.ViewModels
{
    public class CommandAdapter : ICommand
    {
        public CommandAdapter()
        {

        }
        public CommandAdapter(Action<object> e)
        {
            Action = e;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return CanExecuteAction;
        }

        public void Execute(object parameter)
        {
            Action?.Invoke(parameter);
        }

        public Action<object> Action { get; set; }

        private bool _canExecute = true;
        public bool CanExecuteAction
        {
            get => _canExecute;
            set
            {
                _canExecute = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
