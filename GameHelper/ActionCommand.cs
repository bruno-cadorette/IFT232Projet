using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameHelper
{
    public class ActionCommand : ICommand
    {
        private Func<bool> canExecute;
        private Action execute;
        public ActionCommand(Action execute)
            : this(execute, () => true)
        {
        }
        public ActionCommand(Action execute, Func<bool> canExecute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}
