using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpEnhancedNavigation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Shell()
        {
            this.InitializeComponent();
            HideTitleBar();
        }

        /// <summary>
        /// Hides the title bar
        /// </summary>
        public static void HideTitleBar()
        {
            // Get the application view title bar
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Make the title bar transparent
            appTitleBar.BackgroundColor = Colors.Transparent;

            // Get the core appication view title bar
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            /*
                ExtendViewIntoTitleBar
                    Gets or sets a value that specifies whether this title
                    bar should replace the default window title bar.
            */

            // Extend the core application view into title bar
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }

    }
}
