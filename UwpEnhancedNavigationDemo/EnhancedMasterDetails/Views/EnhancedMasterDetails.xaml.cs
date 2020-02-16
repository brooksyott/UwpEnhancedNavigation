using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace Peamel.UwpEnhancedMasterDetails
{
    public sealed partial class EnhancedMasterDetails : UserControl, INotifyPropertyChanged
    {
        // Attached to the view model
        private ShellViewModel ViewModel = ShellViewModel.Instance;

        public EnhancedMasterDetails()
        {
            this.InitializeComponent();
            this.DataContext = this;

            // Initialize the Navigation System so it can push/pop into the SplitView for navigation
            PrimaryNavigation.RightEdgePopupFrame = RightPopupFrame;
            PrimaryNavigation.CenterPopupFrame = CenterPopupFrame;

            // Setup so the code behind can receive events from the view model
            ViewModel.PropertyChanged += ShellViewPropertyChangedHandler;
        }

        #region Styling of the main control (Shell)
        public static readonly DependencyProperty ShellBackgroundProperty = DependencyProperty.Register(
              "ShellBackground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush ShellBackground
        {
            get { return (Brush)GetValue(ShellBackgroundProperty); }
            set
            {
                SetValue(ShellBackgroundProperty, value);
                var pb = (Brush)GetValue(PaneBackgroundProperty);
                if (pb == null)
                {
                    PaneBackground = value;
                }
                var bb = (Brush)GetValue(BackbuttonBackgroundProperty);
                if (bb == null)
                {
                    BackbuttonBackground = value;
                }
            }
        }

        public static readonly DependencyProperty ShellForegroundProperty = DependencyProperty.Register(
              "ShellForeground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush ShellForeground
        {
            get { return (Brush)GetValue(ShellForegroundProperty); }
            set
            {
                SetValue(ShellForegroundProperty, value);
                var pb = (Brush)GetValue(PaneForegroundProperty);
                if (pb == null)
                {
                    PaneForeground = value;
                }
                var bb = (Brush)GetValue(BackbuttonForegroundProperty);
                if (bb == null)
                {
                    BackbuttonForeground = value;
                }
            }
        }
        #endregion

        #region Button Styling
        public static readonly DependencyProperty HamburgerForegroundProperty = DependencyProperty.Register(
              "HamburgerForeground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush HamburgerForeground
        {
            get { return (Brush)GetValue(HamburgerForegroundProperty); }
            set { SetValue(HamburgerForegroundProperty, value); }
        }

        public static readonly DependencyProperty HamburgerHoverForegroundProperty = DependencyProperty.Register(
              "HamburgerHoverForeground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush HamburgerHoverForeground
        {
            get { return (Brush)GetValue(HamburgerHoverForegroundProperty); }
            set { SetValue(HamburgerHoverForegroundProperty, value); }
        }


        public static readonly DependencyProperty HamburgerBackgroundProperty = DependencyProperty.Register(
          "HamburgerBackground",
          typeof(Brush),
          typeof(EnhancedMasterDetails),
          new PropertyMetadata(null)
        );

        public Brush HamburgerBackground
        {
            get { return (Brush)GetValue(HamburgerBackgroundProperty); }
            set { SetValue(HamburgerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackbuttonBackgroundProperty = DependencyProperty.Register(
          "BackbuttonBackground",
          typeof(Brush),
          typeof(EnhancedMasterDetails),
          new PropertyMetadata(null)
        );

        public Brush BackbuttonBackground
        {
            get { return (Brush)GetValue(BackbuttonBackgroundProperty); }
            set { SetValue(BackbuttonBackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackbuttonForegroundProperty = DependencyProperty.Register(
              "BackbuttonForeground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush BackbuttonForeground
        {
            get { return (Brush)GetValue(BackbuttonForegroundProperty); }
            set { SetValue(BackbuttonForegroundProperty, value); }
        }

        public static readonly DependencyProperty BackbuttonHoverForegroundProperty = DependencyProperty.Register(
              "BackbuttonHoverForeground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush BackbuttonHoverForeground
        {
            get { return (Brush)GetValue(BackbuttonHoverForegroundProperty); }
            set { SetValue(BackbuttonHoverForegroundProperty, value); }
        }
        #endregion

        #region Pane Operations / Management / Styling

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

        //---------------------------------------------------------
        //   EXPOSE SPECIFIC DISPLAY MODES FOR THE PANE
        //---------------------------------------------------------

        public static readonly DependencyProperty PaneBorderBrushProperty = DependencyProperty.Register(
              "PaneBorderBrush",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush PaneBorderBrush
        {
            get { return (Brush)GetValue(PaneBorderBrushProperty); }
            set
            {
                SetValue(PaneBorderBrushProperty, value);
            }
        }

        public static readonly DependencyProperty PaneBorderThicknessProperty = DependencyProperty.Register(
              "PaneBorderThickness",
              typeof(Thickness),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Thickness PaneBorderThickness
        {
            get { return (Thickness)GetValue(PaneBorderThicknessProperty); }
            set
            {
                SetValue(PaneBorderThicknessProperty, value);
            }
        }

        public static readonly DependencyProperty PaneBackgroundProperty = DependencyProperty.Register(
              "PaneBackground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush PaneBackground
        {
            get { return (Brush)GetValue(PaneBackgroundProperty); }
            set
            {
                SetValue(PaneBackgroundProperty, value);
                var pb = (Brush)GetValue(HamburgerBackgroundProperty);
                if (pb == null)
                {
                    HamburgerBackground = value;
                }
            }
        }

        public static readonly DependencyProperty PaneForegroundProperty = DependencyProperty.Register(
              "PaneForeground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush PaneForeground
        {
            get { return (Brush)GetValue(PaneForegroundProperty); }
            set
            {
                SetValue(PaneForegroundProperty, value);
                var pb = (Brush)GetValue(HamburgerForegroundProperty);
                if (pb == null)
                {
                    HamburgerForeground = value;
                }
            }
        }

        public static readonly DependencyProperty PaneHoverForegroundProperty = DependencyProperty.Register(
              "PaneHoverForeground",
              typeof(Brush),
              typeof(EnhancedMasterDetails),
              new PropertyMetadata(null)
            );

        public Brush PaneHoverForeground
        {
            get { return (Brush)GetValue(PaneHoverForegroundProperty); }
            set
            {
                SetValue(PaneHoverForegroundProperty, value);
                var pb = (Brush)GetValue(HamburgerHoverForegroundProperty);
                if (pb == null)
                {
                    HamburgerHoverForeground = value;
                }
            }
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

        public SplitViewDisplayMode SmallDisplayMode
        {
            get { return (ViewModel.SmallDisplayMode); }
            set { ViewModel.SmallDisplayMode = value; }
        }

        private void PaneTappedEvent(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.PaneTappedEvent();
        }

        private void PaneClosingHandler(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            ViewModel.DisabledContentTapped();
        }


        private void PaneOpeningHandler(SplitView sender, object args)
        {

        }

        #endregion

        #region Main Content / Details Page 
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
        #endregion Main Content / Details Page

        #region Adapter Triggers
        //---------------------------------------------------------
        //   WIDTHS FOR THE ADAPTER TRIGGERS
        //---------------------------------------------------------
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
        #endregion 

        #region Custom Animations / Story Boards
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
            if (!(Width < 800)) Width = 800;
            animation.To = Width;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
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
            //Storyboard.SetTargetProperty(animation, "Width");
            Storyboard.SetTargetProperty(animation, "Width");
            animation.EnableDependentAnimation = true;
            //Debug.WriteLine("CloseAnimationWidth = " + Width);
            animation.From = Width;
            animation.To = 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
        #endregion

        #region Send and Receive Events 

        private void DisabledPaneAndContentTapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.DisabledContentTapped();
        }

        private void DisabledContent2Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.DisabledContentTapped();
        }

        private void RightEdgePopup_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void CenterPopup_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        /// <summary>
        /// This is registered to receive notifcation events, and then can start/stop animations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShellViewPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("ShellViewPropertyChangedHandler: " + e.PropertyName);
            Debug.WriteLine("ViewModel.EdgePopupNavigationEnabled: " + ViewModel.EdgePopupNavigationEnabled);

            try
            {
                // Listen for the edge popup
                // if it changes state, run the animation
                if (e.PropertyName == "EdgePopupNavigationOpen")
                {
                    if (ViewModel.EdgePopupNavigationOpen == true)
                    {
                        Page tp = (Page)RightPopupFrame.Content;
                        if (tp == null)
                        {
                            Debug.WriteLine("RightPopupFrame.Content was NULL");
                            return;
                        }
                        //OpenAnimation(tp, tp.Width);
                        ViewModel.EdgePopupNavigationEnabled = true;
                        if (tp != null)
                            OpenAnimation(tp, tp.Width);
                    }

                    if (ViewModel.EdgePopupNavigationOpen == false)
                    {
                        Page tp = (Page)RightPopupFrame.Content;
                        if (tp != null)
                            CloseAnimation(tp, tp.ActualWidth);
                        ViewModel.EdgePopupNavigationEnabled = false;
                    }
                }
                // Listen for the edge popup
                // if it changes state, run the animation
                if (e.PropertyName == "CenterPopupNavigationEnabled")
                {
                    if (ViewModel.CenterPopupNavigationEnabled == true)
                    {
                        //Page tp = (Page)CenterPopupFrame.Content;
                        //CenterPopupFrame.Width = tp.Width;
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

        #endregion

    }
}
