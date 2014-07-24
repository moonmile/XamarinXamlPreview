using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XamarinXamlPreviewCS.Libs
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class DelegateCommand : ICommand
    {
        protected Func<object, bool> canExecute;
        protected Action<object> execute;
        public event EventHandler CanExecuteChanged = (s, e) => { };

        public DelegateCommand(Func<object, bool> can, Action<object> exec)
        {
            canExecute = can;
            execute = exec;
        }
        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute(parameter);
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            if (execute != null)
            {
                execute(parameter);
            }
        }
    }
}
