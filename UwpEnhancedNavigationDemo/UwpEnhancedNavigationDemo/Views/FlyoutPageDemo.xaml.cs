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

using Peamel.UwpShell;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpEnhancedNavigationDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlyoutDemoPage : Page
    {
        public FlyoutDemoPage()
        {
            this.InitializeComponent();
        }

        private Double _flyoutWidth = 200;

        /// <summary>
        /// Notify the Navigation manager Popup navigation is done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlyoutContent_Unloaded(object sender, RoutedEventArgs e)
        {
            //FlyoutContent.Unloaded -= FlyoutContent_Unloaded;

            ShellNavigation.PopupNavigation(Enable: false);
        }

        /// <summary>
        /// Displays the edge popup controlled basically by the Main SplitView page
        /// The content that is rendered is responsible to close the Popup
        /// The Main SplitView may close it as well if a click occurs outside the popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdgeFlyout_Clicked(object sender, RoutedEventArgs e)
        {
            ShellNavigation.ShowEdgePopup(new FlyoutDemoContentPage());
        }

        private void GearsButton_Clicked(object sender, RoutedEventArgs e)
        {
            ShellNavigation.ShowCenterPopup(new CenterPopupContentPage());
        }

        private void CenterPopup_Clicked(object sender, RoutedEventArgs e)
        {
            ShellNavigation.ShowCenterPopup(new CenterPopupContentPage());
        }
    }
}
