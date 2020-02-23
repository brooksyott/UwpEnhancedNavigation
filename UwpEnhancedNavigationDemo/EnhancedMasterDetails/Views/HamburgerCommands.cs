using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Peamel.UwpShell
{
    public class HamburgerCommands: ICommand
    {
        public event EventHandler CanExecuteChanged;

        ShellViewModel _shellViewModel = ShellViewModel.Instance;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            _shellViewModel.HambergurMenuClicked();
        }
    }

    public class PreviousCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ShellNavigation.GoBack();
        }
    }

    public class HamburgerPageMenuCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        ShellViewModel _shellViewModel = ShellViewModel.Instance;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            _shellViewModel.ArrowMenuClicked();
        }
    }
}
