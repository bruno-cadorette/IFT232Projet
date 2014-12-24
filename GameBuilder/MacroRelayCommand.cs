using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameBuilder
{
    class MacroRelayCommand<T> : ICommand, ICollection<RelayCommand<T>>
    {
        private List<RelayCommand<T>> commands = new List<RelayCommand<T>>();


        public void Add(RelayCommand<T> item)
        {
            commands.Add(item);
        }

        public void Clear()
        {
            commands.Clear();
        }

        public bool Contains(RelayCommand<T> item)
        {
            return commands.Contains(item);
        }

        public void CopyTo(RelayCommand<T>[] array, int arrayIndex)
        {
            commands.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return commands.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(RelayCommand<T> item)
        {
            return commands.Remove(item);
        }

        public IEnumerator<RelayCommand<T>> GetEnumerator()
        {
            return commands.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool CanExecute(object parameter)
        {
            return commands.All(x => x.CanExecute(parameter));
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            foreach (var command in commands)
            {
                command.Execute(parameter);
            }
        }
    }
}
