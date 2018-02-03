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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UwpEnhancedNavigation
{
    public sealed partial class ShellUserControl : UserControl
    {
        public ShellUserControl()
        {
            this.InitializeComponent();
            ShellSplitView.DisplayMode = SplitViewDisplayMode.Inline;
            ShellSplitView.IsPaneOpen = true;
            HideTitleBar();
        }

        #region Color Properties

        #endregion
        public static readonly DependencyProperty ShellForegroundProperty = DependencyProperty.Register(
              "ShellForeground",
              typeof(SolidColorBrush),
              typeof(ShellUserControl),
              new PropertyMetadata(null)
            );

        public SolidColorBrush ShellForeground
        {
            get { return (SolidColorBrush)GetValue(ShellForegroundProperty); }
            set { SetValue(ShellForegroundProperty, value); SetTitleForegroundColor(value.Color); }
        }

        public static readonly DependencyProperty ShellBackgroundProperty = DependencyProperty.Register(
              "ShellBackground",
              typeof(SolidColorBrush),
              typeof(ShellUserControl),
              new PropertyMetadata(null)
            );

        public SolidColorBrush ShellBackground
        {
            get { return (SolidColorBrush)GetValue(ShellBackgroundProperty); }
            set { SetValue(ShellBackgroundProperty, value); SetTitleBackgroundColor(value.Color); }
        }
        #region Content Properties

        public static readonly DependencyProperty PainContentProperty = DependencyProperty.Register(
              "PainContent",
              typeof(Object),
              typeof(ShellUserControl),
              new PropertyMetadata(null)
            );

        public object PainContent
        {
            get { return (object)GetValue(PainContentProperty); }
            set { SetValue(PainContentProperty, value); }
        }

        public static readonly DependencyProperty MainContentProperty = DependencyProperty.Register(
              "MainContent",
              typeof(Object),
              typeof(ShellUserControl),
              new PropertyMetadata(null)
            );

        public object MainContent
        {
            get { return (object)GetValue(MainContentProperty); }
            set { SetValue(MainContentProperty, value); }
        }
        #endregion Content Properties

        #region Title Bar

        public static void SetTitleBackgroundColor(Color color)
        {
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Make the title bar transparent
            //appTitleBar.BackgroundColor = Windows.UI.Colors.Transparent;
            //appTitleBar.BackgroundColor = Windows.UI.Colors.Red;
            appTitleBar.ButtonBackgroundColor = color;
        }

        public static void SetTitleForegroundColor(Color color)
        {
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Make the title bar transparent
            //appTitleBar.BackgroundColor = Windows.UI.Colors.Transparent;
            //appTitleBar.BackgroundColor = Windows.UI.Colors.Red;
            appTitleBar.ButtonForegroundColor = color;
        }

        /// <summary>
        /// Hides the title bar
        /// </summary>
        public static void HideTitleBar()
        {
            // Get the application view title bar
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Make the title bar transparent
            //appTitleBar.BackgroundColor = Windows.UI.Colors.Transparent;
            appTitleBar.BackgroundColor = Windows.UI.Colors.Red;
            appTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Red;

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
        #endregion Title Bar
    }
}
