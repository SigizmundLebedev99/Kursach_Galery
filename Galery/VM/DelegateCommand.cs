using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Galery.VM
{
    class DelegateCommand : ICommand
    {
        private event Func<object, bool> canExecute;
        private event Action<object> execute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public DelegateCommand(Action<object> act, Func<object, bool> func = null)
        {
            canExecute = func;
            execute = act;
        }
    }
}
