using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UwpEnhancedNavigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpEnhancedNavigationDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlyoutDemoContentPage : Page
    {
        public FlyoutDemoContentPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Notifies the primary navigation we are done :)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMe_Clicked(object sender, RoutedEventArgs e)
        {
            PrimaryNavigation.CloseEdgePopup();
        }

        private void Content_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Uncomment the blow out and any tap of the flyout will cause it to close

            //PrimaryNavigation.CloseEdgePopup();
        }
    }
}
