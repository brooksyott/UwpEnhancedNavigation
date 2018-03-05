using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Peamel.UwpEnhancedMasterDetails
{
    #region Event Defitions to send to the UI
    /// <summary>
    /// Definition of Navigation notification events
    /// </summary>
    public enum NavigationEventTypes
    {
        GO_BACK,
        SECONDARY_NAVIGATION_REGISTERED,
        SECONDARY_NAVIGATION_UNREGISTERED,
        POPUP_NAVIGATION_ENABLED,
        POPUP_NAVIGATION_DISABLED,
        SHOW_POPUP_MENU,
        CLOSE_POPUP_MENU,
        SHOW_EDGE_POPUP,
        CLOSE_EDGE_POPUP,
        SHOW_CENTER_POPUP,
        CLOSE_CENTER_POPUP,
        CONFIGURE_MENU_POPUP,
        NAVIGATED
    };

    /// <summary>
    /// Args used to notify interested parties of the Navigation Event that occurred
    /// </summary>
    public class NavEventArgs : EventArgs
    {
        public NavigationEventTypes NavEvent;
        public Page Content;
        public Boolean CloseOnTap = false;              // If the user taps the popup, it will close when tapped if set to true

        public NavEventArgs()
        { }
        public NavEventArgs(NavigationEventTypes navEvent)
        {
            NavEvent = navEvent;
            Content = null;
        }
        public NavEventArgs(NavigationEventTypes navEvent, Page content, Boolean CloseOnTap = false)
        {
            NavEvent = navEvent;
            Content = content;
            this.CloseOnTap = CloseOnTap;
        }
    }
    #endregion
}
