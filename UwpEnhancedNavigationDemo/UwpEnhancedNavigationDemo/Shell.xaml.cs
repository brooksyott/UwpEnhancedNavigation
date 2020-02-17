using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Peamel.UwpEnhancedMasterDetails;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpEnhancedNavigationDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        MenuViewModel _mvm = new MenuViewModel();

        public ShellPage()
        {
            this.InitializeComponent();

            // Navigate to the home page.

            // Point the navigation system to my frame for navigation
            PrimaryNavigation.Frame = SplitViewFrame;
            this.DataContext = _mvm;


            // UNCOMMENT
            Menu.ItemsSource = _mvm.Menu;

            if (Shell.IsPaneOpen)
            {
                HeaderTextBlockGrid.Visibility = Visibility.Visible;
            }
            else
            {
                HeaderTextBlockGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // UNCOMMENT
            //double height = MainNavSplitView.ActualHeight;
            //double width = MainNavSplitView.ActualWidth;


            if (e.AddedItems.Count > 0)
            {
                // Unselect the other menu.
                if ((sender as ListView) == Menu)
                {
                    SecondMenu.SelectedItem = null;
                }
                else
                {
                    Menu.SelectedItem = null;
                }

                var menuItem = e.AddedItems.First() as MenuItem;
                if (menuItem != null && menuItem.IsNavigation)
                {
                    PrimaryNavigation.Navigate(menuItem.NavigationDestination, menuItem.ClearNavigationStack);
                }
            }
        }

        private void SplitViewFrame_OnNavigated(object sender, NavigationEventArgs e)
        {

        }

        private void SplitViewContent_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PaneOpeningHandler(object sender, Object e)
        {
            HeaderTextBlockGrid.Visibility = Visibility.Visible;
        }

        private void PaneClosingHandler(object sender, Object e)
        {
            HeaderTextBlockGrid.Visibility = Visibility.Collapsed;
        }
    }
}
