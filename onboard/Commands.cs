using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Onboard
{
    /// <summary>
    /// Base class for command binding
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _Execute;
        private readonly Predicate<object> _CanExecute;
    
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }
      
        public DelegateCommand(Action<object> execute)
            : this(execute, null) { }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_CanExecute == null)
            {
                return true;
            }
            return _CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }

        #endregion
    }
}
