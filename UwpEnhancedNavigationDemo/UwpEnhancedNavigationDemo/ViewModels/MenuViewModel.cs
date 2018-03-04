using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpEnhancedNavigationDemo
{
    // The below link demostrates using a DataTemplate selector
    // https://eren.ws/2013/10/13/using-multiple-item-templates-together-itemtemplateselector-boredom-challenge-day-9/

    /// <summary>
    /// Demonstrates display a Glyph, or an Image in the menu. A DataTemplateSelector is used
    /// </summary>
    internal class MenuViewModel : MenuViewModelBase
    {
        public MenuViewModel()
        {
            // Build the menus
            Menu.Add(new MenuItem() { Glyph = Icon.GetIcon("Page1Icon"), Text = "Simple Page", NavigationDestination = typeof(BasicPage) });
            //Menu.Add(new MenuItem() { Initials = "SN", Text = "Secondary Navigation", NavigationDestination = typeof(SecondaryNavPage) });
            Menu.Add(new MenuItem() { ImagePath = "/Assets/Flyout.png", Text = "Flyout Demo", NavigationDestination = typeof(FlyoutDemoPage) });
            Menu.Add(new MenuItem() { Initials = "FO", Text = "Flyout Demo", NavigationDestination = typeof(FlyoutDemoPage) });

            //// The menu that appears at the bottom of the list
            //SecondMenu.Add(new MenuItem() { Glyph = Icon.GetIcon("GearIcon"), Text = "Settings", NavigationDestination = typeof(SettingsPage) });
            //SecondMenu.Add(new MenuItem() { Glyph = Icon.GetIcon("InfoIcon"), Text = "About", NavigationDestination = typeof(AboutPage) });
        }
    }
}
