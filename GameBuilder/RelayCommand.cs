using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameBuilder
{
    public class RelayCommand<T> : ICommand
    {
        private Predicate<T> canExecute;
        private Action<T> execute;
        public RelayCommand(Action<T> execute)
            : this(execute, p => true)
        {
        }
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
