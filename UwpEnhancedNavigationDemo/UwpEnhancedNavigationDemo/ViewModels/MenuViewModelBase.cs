using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpEnhancedNavigationDemo
{
    /// <summary>
    /// Model used for the Menu in the Split Panel for navigation
    /// </summary>
    internal class MenuViewModelBase : BindableBase
    {
        private static readonly ObservableCollection<MenuItem> AppMenu = new ObservableCollection<MenuItem>();
        private static readonly ObservableCollection<MenuItem> AppSecondMenu = new ObservableCollection<MenuItem>();

        public MenuViewModelBase()
        { }

        public ObservableCollection<MenuItem> Menu => AppMenu;

        public ObservableCollection<MenuItem> SecondMenu => AppSecondMenu;
    }
}
