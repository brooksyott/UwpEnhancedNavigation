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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UwpEnhancedNavigation
{
    public sealed partial class EnhancedMasterDetails : UserControl, INotifyPropertyChanged
    {
        private ShellViewModel ViewModel = ShellViewModel.Instance;
        public EnhancedMasterDetails()
        {
            this.InitializeComponent();
            //ShellSplitView.DisplayMode = SplitViewDisplayMode.Inline;
            this.DataContext = this;
            ShellSplitView.IsPaneOpen = false;
            HideTitleBar();
            PrimaryNavigation.RightEdgePopupFrame = RightPopupFrame;
            PrimaryNavigation.CenterPopupFrame = CenterPopupFrame;

            // Register for changes
            ViewModel.PropertyChanged += ShellViewPropertyChangedHandler;
        }

        #region Controlling the Pane behavour

        // Sizes for adaptive triggers, with default values
        public static readonly DependencyProperty LargeDisplayModeProperty = DependencyProperty.Register(
              "LargeDisplayMode",
              typeof(SplitViewDisplayMode),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(SplitViewDisplayMode.Inline)
            );

        public SplitViewDisplayMode LargeDisplayMode
        {
            get { return (ViewModel.LargeDisplayMode); }
            set { ViewModel.LargeDisplayMode = value; }
        }

        // Sizes for adaptive triggers, with default values
        public static readonly DependencyProperty MediumDisplayModeProperty = DependencyProperty.Register(
              "MediumDisplayMode",
              typeof(SplitViewDisplayMode),
              typeof(EnhancedMasterDetails),
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
              typeof(EnhancedMasterDetails),
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
              typeof(EnhancedMasterDetails),
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
              typeof(EnhancedMasterDetails),
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
              typeof(EnhancedMasterDetails),
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
              typeof(EnhancedMasterDetails),
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
              typeof(EnhancedMasterDetails),
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

        #region Pane events / Queries
        public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register(
              "IsPaneOpen ",
              typeof(Boolean),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Boolean IsPaneOpen
        {
            get { return (Boolean)ShellSplitView.IsPaneOpen; }
            set
            {
                ShellSplitView.IsPaneOpen = value;
            }
        }
        #endregion Pane Events / Queries

        #region Color Properties
        public static readonly DependencyProperty ShellForegroundProperty = DependencyProperty.Register(
              "ShellForeground",
              typeof(SolidColorBrush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public SolidColorBrush ShellForeground
        {
            get { return (SolidColorBrush)GetValue(ShellForegroundProperty); }
            set
            {
                SetValue(ShellForegroundProperty, value);
                SetTitleForegroundColor(value.Color);
                var pb = (SolidColorBrush)GetValue(PaneForegroundProperty);
                if (pb == null)
                {
                    PaneForeground = value;
                }
                var bb = (SolidColorBrush)GetValue(BackbuttonForegroundProperty);
                if (bb == null)
                {
                    BackbuttonForeground = value;
                }
            }
        }

        public static readonly DependencyProperty ShellBackgroundProperty = DependencyProperty.Register(
              "ShellBackground",
              typeof(SolidColorBrush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public SolidColorBrush ShellBackground
        {
            get { return (SolidColorBrush)GetValue(ShellBackgroundProperty); }
            set
            {
                SetValue(ShellBackgroundProperty, value);
                SetTitleBackgroundColor(value.Color);
                var pb = (SolidColorBrush)GetValue(PaneBackgroundProperty);
                if (pb == null)
                {
                    PaneBackground = value;
                }
                var bb = (SolidColorBrush)GetValue(BackbuttonBackgroundProperty);
                if (bb == null)
                {
                    BackbuttonBackground = value;
                }
            }
        }

        public static readonly DependencyProperty PaneBackgroundProperty = DependencyProperty.Register(
              "PaneBackground",
              typeof(SolidColorBrush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public SolidColorBrush PaneBackground
        {
            get { return (SolidColorBrush)GetValue(PaneBackgroundProperty); }
            set
            {
                SetValue(PaneBackgroundProperty, value);
                var pb = (SolidColorBrush)GetValue(HamburgerBackgroundProperty);
                if (pb == null)
                {
                    HamburgerBackground = value;
                }
            }
        }

        public static readonly DependencyProperty BackbuttonForegroundProperty = DependencyProperty.Register(
          "BackbuttonForeground",
          typeof(SolidColorBrush),
          typeof(EnhancedMasterDetails),
          new PropertyMetadata(null)
        );

        public SolidColorBrush BackbuttonForeground
        {
            get { return (SolidColorBrush)GetValue(BackbuttonForegroundProperty); }
            set { SetValue(BackbuttonForegroundProperty, value); }
        }


        public static readonly DependencyProperty PaneForegroundProperty = DependencyProperty.Register(
          "PaneForeground",
          typeof(SolidColorBrush),
          typeof(EnhancedMasterDetails),
          new PropertyMetadata(null)
        );

        public SolidColorBrush PaneForeground
        {
            get { return (SolidColorBrush)GetValue(PaneForegroundProperty); }
            set
            {
                SetValue(PaneForegroundProperty, value);
                var pb = (SolidColorBrush)GetValue(HamburgerForegroundProperty);
                if (pb == null)
                {
                    HamburgerForeground = value;
                }
            }
        }

        public static readonly DependencyProperty HamburgerForegroundProperty = DependencyProperty.Register(
          "HamburgerForeground",
          typeof(SolidColorBrush),
          typeof(EnhancedMasterDetails),
          new PropertyMetadata(null)
        );

        public SolidColorBrush HamburgerForeground
        {
            get { return (SolidColorBrush)GetValue(HamburgerForegroundProperty); }
            set { SetValue(HamburgerForegroundProperty, value); }
        }


        public static readonly DependencyProperty HamburgerBackgroundProperty = DependencyProperty.Register(
          "HamburgerBackground",
          typeof(SolidColorBrush),
          typeof(EnhancedMasterDetails),
          new PropertyMetadata(null)
        );

        public SolidColorBrush HamburgerBackground
        {
            get { return (SolidColorBrush)GetValue(HamburgerBackgroundProperty); }
            set { SetValue(HamburgerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackbuttonBackgroundProperty = DependencyProperty.Register(
          "BackbuttonBackground",
          typeof(SolidColorBrush),
          typeof(EnhancedMasterDetails),
          new PropertyMetadata(null)
        );

        public SolidColorBrush BackbuttonBackground
        {
            get { return (SolidColorBrush)GetValue(BackbuttonBackgroundProperty); }
            set { SetValue(BackbuttonBackgroundProperty, value); }
        }
        #endregion

        #region Content Properties
        public static readonly DependencyProperty PainContentProperty = DependencyProperty.Register(
              "PainContent",
              typeof(Object),
              typeof(EnhancedMasterDetails),
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
              typeof(EnhancedMasterDetails),
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
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public object MainContent
        {
            get { return (object)GetValue(MainContentProperty); }
            set
            {
                SetValue(MainContentProperty, value);
            }
        }

        public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.Register(
              "TitleBarContent",
              typeof(Object),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public object TitleBarContent
        {
            get { return (object)GetValue(TitleBarContentProperty); }
            set
            {
                SetValue(TitleBarContentProperty, value);
            }
        }

        public static readonly DependencyProperty PageTitleContentProperty = DependencyProperty.Register(
              "PageTitleContent",
              typeof(Object),
              typeof(EnhancedMasterDetails),
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

        private void PaneTappedEvent(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.PaneTappedEvent();
        }

        private void PaneClosingHandler(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            ViewModel.DisabledContentTapped();
        }

        private void DisabledContentTapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.DisabledContentTapped();
        }

        private void RightEdgePopup_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void CenterPopup_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void ShellViewPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                // Listen for the edge popup
                // if it changes state, run the animation
                if (e.PropertyName == "EdgePopupNavigationEnabled")
                {
                    if (ViewModel.EdgePopupNavigationEnabled == true)
                    {
                        Page tp = (Page)RightPopupFrame.Content;
                        if (tp == null) return;
                        OpenAnimation(tp, tp.Width);
                    }

                    if (ViewModel.EdgePopupNavigationEnabled == false)
                    {
                        Page tp = (Page)RightPopupFrame.Content;
                        if (tp == null) return;
                        CloseAnimation(tp, tp.Width);
                    }
                }
                // Listen for the edge popup
                // if it changes state, run the animation
                if (e.PropertyName == "CenterPopupNavigationEnabled")
                {
                    if (ViewModel.CenterPopupNavigationEnabled == true)
                    {
                        //Page tp = (Page)RightPopupFrame.Content;
                        //if (tp == null) return;
                        //OpenAnimation(tp, tp.Width);
                    }

                    if (ViewModel.EdgePopupNavigationEnabled == false)
                    {
                        //Page tp = (Page)RightPopupFrame.Content;
                        //if (tp == null) return;
                        //CloseAnimation(tp, tp.Width);
                    }
                }

            }
            catch
            {
                return;
            }

        }

        // Take from the follow stackoverflow Q&A
        // https://stackoverflow.com/questions/45470247/how-to-animate-column-width-in-uwp-app
        /// <summary>
        /// Open the UIElement with a little width animation :)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="Width"></param>
        static void OpenAnimation(UIElement element, Double Width)
        {
            //MenuGrid.Name = nameof(MenuGrid);
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation();
            Storyboard.SetTargetName(animation, nameof(element));
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, "Width");
            animation.EnableDependentAnimation = true;
            animation.From = 0;
            if (!(Width < 200)) Width = 200;
            animation.To = Width;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        /// <summary>
        /// Close the UIElement with a little width animation :)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="Width"></param>
        static void CloseAnimation(UIElement element, Double Width)
        {
            //MenuGrid.Name = nameof(MenuGrid);
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation();
            Storyboard.SetTargetName(animation, nameof(element));
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, "Width");
            animation.EnableDependentAnimation = true;
            //Debug.WriteLine("CloseAnimationWidth = " + Width);
            animation.From = Width;
            animation.To = 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }


    }
}

