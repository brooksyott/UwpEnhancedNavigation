using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class EnhancedNavigationShell : UserControl, INotifyPropertyChanged
    {
        private ShellViewModel ViewModel = ShellViewModel.Instance;
        public EnhancedNavigationShell()
        {
            this.InitializeComponent();
            //ShellSplitView.DisplayMode = SplitViewDisplayMode.Inline;
            this.DataContext = this;
            ShellSplitView.IsPaneOpen = false;
            HideTitleBar();
            
        }

        #region Controlling the Pane behavour

        // Sizes for adaptive triggers, with default values
        public static readonly DependencyProperty LargeDisplayModeProperty = DependencyProperty.Register(
              "LargeDisplayMode",
              typeof(SplitViewDisplayMode),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(SplitViewDisplayMode.Inline)
            );

        public SplitViewDisplayMode LargeDisplayMode
        {
            get { return (ViewModel.LargeDisplayMode); }
            set { ViewModel.LargeDisplayMode = value;  }
        }

        // Sizes for adaptive triggers, with default values
        public static readonly DependencyProperty MediumDisplayModeProperty = DependencyProperty.Register(
              "MediumDisplayMode",
              typeof(SplitViewDisplayMode),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(SplitViewDisplayMode.CompactOverlay)
            );

        public SplitViewDisplayMode MediumDisplayMode
        {
            get { return (ViewModel.MediumDisplayMode); }
            set { ViewModel.MediumDisplayMode = value; }
        }

        // Sizes for adaptive triggers, with default values
        public static readonly DependencyProperty SmallDisplayModeProperty = DependencyProperty.Register(
              "SmallDisplayMode",
              typeof(SplitViewDisplayMode),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(SplitViewDisplayMode.CompactOverlay)
            );

        public SplitViewDisplayMode SmallDisplayMode
        {
            get { return (ViewModel.SmallDisplayMode); }
            set { ViewModel.SmallDisplayMode = value; }
        }

        // Sizes for adaptive triggers, with default values
        public static readonly DependencyProperty LargeMinWindowWidthProperty = DependencyProperty.Register(
              "LargeMinWindowWidth",
              typeof(Int32),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(800)
            );

        public Int32 LargeMinWindowWidth
        {
            get { return (Int32)GetValue(LargeMinWindowWidthProperty); }
            set { SetValue(LargeMinWindowWidthProperty, value); ViewModel.LargeMinWindowWidth = value; }
        }

        public static readonly DependencyProperty MediumMinWindowWidthProperty = DependencyProperty.Register(
              "MediumMinWindowWidth",
              typeof(Int32),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(500)
            );

        public Int32 MediumMinWindowWidth
        {
            get { return (Int32)GetValue(MediumMinWindowWidthProperty); }
            set { SetValue(MediumMinWindowWidthProperty, value); ViewModel.MediumMinWindowWidth = value; }
        }

        public static readonly DependencyProperty SmallMinWindowWidthProperty = DependencyProperty.Register(
              "SmallMinWindowWidth",
              typeof(Int32),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(0)
            );

        public Int32 SmallMinWindowWidth
        {
            get { return (Int32)GetValue(SmallMinWindowWidthProperty); }
            set { SetValue(SmallMinWindowWidthProperty, value); ViewModel.SmallMinWindowWidth = value; }
        }

        public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register(
              "OpenPaneLength",
              typeof(Double),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(300)
            );

        public Double OpenPaneLength
        {
            get { return (Double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }

        // Used when the menu is compated, but open
        public static readonly DependencyProperty CompactPaneLengthProperty = DependencyProperty.Register(
              "CompactPaneLength",
              typeof(Double),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(45)
            );

        public Double CompactPaneLength
        {
            get { return (Double)GetValue(CompactPaneLengthProperty); }
            set { SetValue(CompactPaneLengthProperty, value); }
        }

        #endregion Controlling the Pane behavour

        #region Visual State Triggers and Properties
        #endregion Visual State Triggers and Properties

        #region Color Properties
        public static readonly DependencyProperty ShellForegroundProperty = DependencyProperty.Register(
              "ShellForeground",
              typeof(SolidColorBrush),
              typeof(EnhancedNavigationShell),
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
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(null)
            );

        public SolidColorBrush ShellBackground
        {
            get { return (SolidColorBrush)GetValue(ShellBackgroundProperty); }
            set { SetValue(ShellBackgroundProperty, value); SetTitleBackgroundColor(value.Color); }
        }
        #endregion

        #region Content Properties
        public static readonly DependencyProperty PainContentProperty = DependencyProperty.Register(
              "PainContent",
              typeof(Object),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(null)
            );

        public object PainContent
        {
            get { return (object)GetValue(PainContentProperty); }
            set { SetValue(PainContentProperty, value); }
        }

        public static readonly DependencyProperty HamburgerTitleContentProperty = DependencyProperty.Register(
              "HamburgerTitleContent",
              typeof(Object),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(null)
            );

        public object HamburgerTitleContent
        {
            get { return (object)GetValue(HamburgerTitleContentProperty); }
            set { SetValue(HamburgerTitleContentProperty, value); }
        }


        public static readonly DependencyProperty MainContentProperty = DependencyProperty.Register(
              "MainContent",
              typeof(Object),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(null)
            );

        public object MainContent
        {
            get { return (object)GetValue(MainContentProperty); }
            set {
                SetValue(MainContentProperty, value);
            }
        }

        public static readonly DependencyProperty PageTitleContentProperty = DependencyProperty.Register(
              "PageTitleContent",
              typeof(Object),
              typeof(EnhancedNavigationShell),
              new PropertyMetadata(null)
            );

        public object PageTitleContent
        {
            get { return (object)GetValue(PageTitleContentProperty); }
            set
            {
                SetValue(PageTitleContentProperty, value);
            }
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

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }


        /// <summary>
        /// Hides the title bar
        /// </summary>
        private static void HideTitleBar()
        {
            // Get the application view title bar
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Make the title bar transparent
            appTitleBar.BackgroundColor = Windows.UI.Colors.Transparent;
            //appTitleBar.BackgroundColor = Windows.UI.Colors.Red;
            //appTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Red;

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

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void VisualStateChange_Event(object sender, VisualStateChangedEventArgs e)
        {
            var newState = e.NewState.Name;
            if (e.OldState != null)
            {
                var oldState = e.OldState.Name;
            }
            ViewModel.NotifySizeChange(newState);
            Debug.WriteLine("New VisualState = " + newState + ", Width = " + this.ActualWidth);
        }


        private void PaneClosingHandler(object sender, SplitViewPaneClosingEventArgs e)
        {
            ViewModel.PaneClosing();
        }

        private void SplitViewFrame_OnNavigated(object sender, NavigationEventArgs e)
        {

        }

        private void SplitViewContent_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
