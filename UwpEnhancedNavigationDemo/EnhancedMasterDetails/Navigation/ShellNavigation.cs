using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

using Peamel.SimpleFiniteStateMachine;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Peamel.UwpShell
{
    /// <summary>
    /// Primary navigation system
    /// </summary>
    public class ShellNavigation 
    {
        ///  The frame is host for the navigation 
        private static Frame _frame;
        public static Frame Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }

        private static Frame _rightEdgePopupFrame;
        public static Frame RightEdgePopupFrame
        {
            get { return _rightEdgePopupFrame; }
            set { _rightEdgePopupFrame = value; }
        }

        private static Frame _centerPopupFrame;
        public static Frame CenterPopupFrame
        {
            get { return _centerPopupFrame; }
            set { _centerPopupFrame = value; }
        }
        

        #region Navigation Notification System, generally to the UI
        /// Event handlers to notify to UI when navigation events have occurred
        public delegate void NavEventHandler(object sender, NavEventArgs e);
        public static event NavEventHandler NavigationEvent;

        /// <summary>
        /// Notify interested parties of a Navigation event (typically the UI)
        /// </summary>
        /// <param name="e"></param>
        private static void OnNavigationEvent(NavEventArgs e)
        {
            NavEventHandler handler = NavigationEvent;
            if (handler != null)
            {
                handler(typeof(ShellNavigation), e);
            }
        }
        #endregion

        #region Primary Navigation Controls

        static FiniteStateMachine<States, Triggers> _fsm = null;                                                 // Used to send triggers to the UI
        static public FiniteStateMachine<States, Triggers> Fsm
        {
            get { return _fsm; }
            set
            {
                _fsm = value;
            }
        }

        /// <summary>
        /// Used to wrap frame navigation. Allows for clearing of the backstack if desired
        /// </summary>
        /// <param name="sourcePageType"></param>
        /// <param name="ClearNavStack"></param>
        /// <returns></returns>
        public static bool Navigate(Type sourcePageType, Boolean ClearNavStack = false)
        {
            if (_frame.CurrentSourcePageType != sourcePageType)
            {
                int initialDepth = _frame.BackStackDepth;
                Debug.WriteLine("**** MainNav: Navigate backstack = " + initialDepth);

                Boolean hadSuccess = _frame.Navigate(sourcePageType, null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                if (ClearNavStack == true)
                {
                    _frame.BackStack.Clear();
                }
                int newDepth = _frame.BackStackDepth;
                //NavEventArgs navEvent = new NavEventArgs(NavigationEventTypes.NAVIGATED);
                //OnNavigationEvent(navEvent);
                if (initialDepth != newDepth)
                {
                    if (newDepth <= 0)
                        _fsm.Fire(Triggers.PRIMARY_NAV_DISABLED);
                    else
                        _fsm.Fire(Triggers.PRIMARY_NAV_ENABLED);
                }
                Debug.WriteLine("**** MainNav: Navigate backstack = " + newDepth);
                return hadSuccess;
            }

            int i = _frame.BackStackDepth;
            Debug.WriteLine("**** MainNav: Navigate NOTHING DONE, backstack = " + i);
            return true;
        }

        public static bool Navigate(Type sourcePageType, String Title, Boolean ClearNavStack = false)
        {
            if (_frame.CurrentSourcePageType != sourcePageType)
            {
                int initialDepth = _frame.BackStackDepth;
                Debug.WriteLine("**** MainNav: Navigate backstack = " + initialDepth);

                Boolean hadSuccess = _frame.Navigate(sourcePageType);
                if (ClearNavStack == true)
                {
                    _frame.BackStack.Clear();
                }
                int newDepth = _frame.BackStackDepth;
                //NavEventArgs navEvent = new NavEventArgs(NavigationEventTypes.NAVIGATED);
                //OnNavigationEvent(navEvent);
                if (initialDepth != newDepth)
                {
                    if (newDepth <= 0)
                        _fsm.Fire(Triggers.PRIMARY_NAV_DISABLED);
                    else
                        _fsm.Fire(Triggers.PRIMARY_NAV_ENABLED);
                }
                Debug.WriteLine("**** MainNav: Navigate backstack = " + newDepth);
                return hadSuccess;
            }

            int i = _frame.BackStackDepth;
            Debug.WriteLine("**** MainNav: Navigate NOTHING DONE, backstack = " + i);
            return true;
        }


        /// <summary>
        /// Checks if the navigation can go backwards
        /// </summary>
        public static Boolean CanGoBack
        {
            get { return _frame.CanGoBack; }
        }

        /// <summary>
        /// Go back one level in the navigation stack. Notify the UI we have gone back.
        /// This allows for the humburger menu to adapt as needed
        /// </summary>
        public static void GoBack()
        {

            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
            int i = _frame.BackStackDepth;
            Debug.WriteLine("**** MainNav: GoBack Clicked: backstack = " + i);
            if (i<=0)
            {
                _fsm.Fire(Triggers.PRIMARY_NAV_DISABLED);
            }
            //NavEventArgs navEvent = new NavEventArgs(NavigationEventTypes.GO_BACK);
            //OnNavigationEvent(navEvent);
        }
        #endregion Primary Navigation Controls


        #region Popup Navigatiion Management/Helpers
        /// <summary>
        /// Notify the UI we want an Edge Popup, and to apply the page given
        /// TODO: make it so it doesn't need to be a page but any control
        /// </summary>
        /// <param name="content"></param>
        public static void ShowEdgePopup(Page content)
        {
            RightEdgePopupFrame.Content = content;
            _fsm.Fire(Triggers.EDGE_POPUP_NAV_ENABLED);
            //NavEventArgs navEvent;
            //navEvent = new NavEventArgs(NavigationEventTypes.SHOW_EDGE_POPUP, content);
            //OnNavigationEvent(navEvent);
        }


        /// <summary>
        /// Notify the UI that we are done with the Popup, and reenable the UI
        /// </summary>
        public static void CloseEdgePopup()
        {
            //CloseAnimation(RightEdgePopupFrame, RightEdgePopupFrame.ActualWidth);
            _fsm.Fire(Triggers.EDGE_POPUP_NAV_DISABLED);
            //RightEdgePopupFrame.Content = null;

            //NavEventArgs navEvent;
            //navEvent = new NavEventArgs(NavigationEventTypes.CLOSE_EDGE_POPUP);
            //OnNavigationEvent(navEvent);
        }

        public static void ShowCenterPopup(Page content, Boolean CloseOnTap = false)
        {
            CenterPopupFrame.Content = content;
            _fsm.Fire(Triggers.CENTER_POPUP_NAV_ENABLED);
            //NavEventArgs navEvent;
            //navEvent = new NavEventArgs(NavigationEventTypes.SHOW_CENTER_POPUP, content);
            //OnNavigationEvent(navEvent);
        }

        /// <summary>
        /// Notify the UI that we are done with the Popup, and reenable the UI
        /// </summary>
        public static void CloseCenterPopup()
        {
            _fsm.Fire(Triggers.CENTER_POPUP_NAV_DISABLED);
            CenterPopupFrame.Content = null;
            //NavEventArgs navEvent;
            //navEvent = new NavEventArgs(NavigationEventTypes.CLOSE_CENTER_POPUP);
            //OnNavigationEvent(navEvent);
        }

        /// <summary>
        /// This is used to notify the main split view / UI that a Flyout or similiar popup
        /// has been opned, and to disable the remaining portions of the UI
        /// </summary>
        /// <param name="Enable"></param>
        public static void PopupNavigation(Boolean Enable)
        {
            //NavEventArgs navEvent;

            if (Enable)
            {
                _fsm.Fire(Triggers.ENABLE_CONTENT);
            }
            else
            {
                _fsm.Fire(Triggers.DISABLE_CONTENT);
            }
            //OnNavigationEvent(navEvent);
        }
        #endregion Popup Navigatiion Management/Helpers

        #region Enable/Disable UWP standard behaviours / UX

        /// <summary>
        /// Enables the UWP standard back buttons
        /// </summary>
        //public static void EnableBackButton()
        //{
        //    var navManager = SystemNavigationManager.GetForCurrentView();
        //    navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        //    navManager.BackRequested -= GoBackHandler;
        //    navManager.BackRequested += GoBackHandler;
        //}

        ///// <summary>
        ///// Disables the UWP standard back buttons
        ///// </summary>
        //public static void DisableBackButton()
        //{
        //    var navManager = SystemNavigationManager.GetForCurrentView();
        //    navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        //    navManager.BackRequested -= GoBackHandler;
        //}
        #endregion Enable/Disable UWP standard behaviours / UX
    }
}
