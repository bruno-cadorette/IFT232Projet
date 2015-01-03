using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameHelper
{
    public class MacroRelayCommand<T> : List<RelayCommand<T>>,  ICommand
    {
        private Predicate<T> canExecute;
        public MacroRelayCommand()
        {

        }
        public MacroRelayCommand(Predicate<T> canExecute)
        {
            this.canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return canExecute((T)parameter) && this.All(x => x.CanExecute(parameter));
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            foreach (var command in this)
            {
                command.Execute(parameter);
            }
        }
    }
}
