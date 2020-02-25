using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Peamel.SimpleFiniteStateMachine;

namespace Peamel.UwpShell
{
    /// <summary>
    /// The View Model that effectively controls the UI of the shell
    /// Interacts closely with the Navigation System
    /// Since these could be changed by non-UI functions (unlikley, but possible), 
    /// use the Bindable base the updates the properties on the UI thread
    /// </summary>
    internal class ShellViewModel : BindableBaseUI
    {
        

        #region Singleton Implementation
        static private ShellViewModel _instance = new ShellViewModel();
        static public ShellViewModel Instance
        {
            get { return _instance; }
        }

        private ShellViewModel()
        {
            // The UI bindable base needs a UI reference for the dispatcher
            displatcherContext = Window.Current.Content as Frame;
            SetupFSM();
        }
        #endregion Singleton Implementation

        #region Hamburger Menu Icon Stats / Elements
        private AppSizeVisualState _menuVisualState = AppSizeVisualState.UNKNOWN;

        HamburgerButtonState _hamburgerMenuState = HamburgerButtonState.Menu;
        public HamburgerButtonState HamburgerMenuState
        {
            get { return _hamburgerMenuState; }
            set
            {
                SetProperty(ref _hamburgerMenuState, value);
            }
        }
        #endregion Hamburger Menu Icon Stats / Elements

        #region Pane State
        Boolean _isOverlayed = false;

        #endregion Pane State

        #region App Size Handling
        #endregion App Size Handling

        #region Overall State Machine
        // The basic state machine to determine UI element behaviour
        public FiniteStateMachine<States, Triggers> _mainShellFsm = new FiniteStateMachine<States, Triggers>(States.NO_NAV);

        /// <summary>
        /// Configuration of the state macine
        /// </summary>
        private void SetupFSM()
        {
            ShellNavigation.Fsm = _mainShellFsm;
            
            _mainShellFsm.Configure(States.NO_NAV)
                .OnEntry((o, t) => SetNoNav(t))
                .Permit(Triggers.HAMBURGER_MENU_CLICKED, (o, t) => HamburgerMenuClickFired())
                .Permit(Triggers.PANE_TAPPED, (o, t) => PaneTapped())
                .Permit(Triggers.EDGE_POPUP_NAV_ENABLED, (o, t) => EdgePoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_DISABLED, (o, t) => CloseEdgePoppupRequest())
                .Permit(Triggers.CENTER_POPUP_NAV_ENABLED, (o, t) => OpenCenterPoppupRequest())
                .Permit(Triggers.CENTER_POPUP_NAV_DISABLED, (o, t) => CloseCenterPoppupRequest())
                .Permit(Triggers.ENABLE_CONTENT, (o, t) => EnabledContent())
                .Permit(Triggers.VISUAL_STATE_MEDIUM, States.MEDIUM_NO_NAV)
                .Permit(Triggers.VISUAL_STATE_SMALL, States.SMALL_NO_NAV)                 
                .Permit(Triggers.PRIMARY_NAV_ENABLED, States.PRIMARY_NAV);

            _mainShellFsm.Configure(States.PRIMARY_NAV)
                .OnEntry((o, t) => SetPrimaryNav(t))
                .Permit(Triggers.HAMBURGER_MENU_CLICKED, (o, t) => HamburgerMenuClickFired())
                .Permit(Triggers.EDGE_POPUP_NAV_ENABLED, (o, t) => EdgePoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_DISABLED, (o, t) => CloseEdgePoppupRequest())
                .Permit(Triggers.CENTER_POPUP_NAV_ENABLED, (o, t) => OpenCenterPoppupRequest())
                .Permit(Triggers.CENTER_POPUP_NAV_DISABLED, (o, t) => CloseCenterPoppupRequest())
                 .Permit(Triggers.ENABLE_CONTENT, (o, t) => EnabledContent())
                .Permit(Triggers.PANE_TAPPED, (o, t) => PaneTapped())
                .Permit(Triggers.VISUAL_STATE_MEDIUM, States.MEDIUM_PRIMARY_NAV)
                .Permit(Triggers.VISUAL_STATE_SMALL, States.SMALL_PRIMARY_NAV)
                .Permit(Triggers.PRIMARY_NAV_DISABLED, States.NO_NAV);

            _mainShellFsm.Configure(States.SMALL_NO_NAV)
                .OnEntry((o, t) => SetNoNavSmall())
                .Permit(Triggers.HAMBURGER_MENU_CLICKED, (o, t) => HamburgerMenuClickFired())
                .Permit(Triggers.PANE_TAPPED, (o, t) => PaneTapped())
                .Permit(Triggers.ENABLE_CONTENT, (o, t) => EnabledContent())
                .Permit(Triggers.CENTER_POPUP_NAV_ENABLED, (o, t) => OpenCenterPoppupRequest())
                .Permit(Triggers.CENTER_POPUP_NAV_DISABLED, (o, t) => CloseCenterPoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_ENABLED, (o, t) => EdgePoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_DISABLED, (o, t) => CloseEdgePoppupRequest())
                .Permit(Triggers.VISUAL_STATE_LARGE, States.NO_NAV)
                .Permit(Triggers.VISUAL_STATE_MEDIUM, States.MEDIUM_NO_NAV)               
                .Permit(Triggers.PRIMARY_NAV_ENABLED, States.SMALL_PRIMARY_NAV);

            _mainShellFsm.Configure(States.SMALL_PRIMARY_NAV)
                .OnEntry((o, t) => SetPrimaryNavSmall())
                .Permit(Triggers.HAMBURGER_MENU_CLICKED, (o, t) => HamburgerMenuClickFired())
                .Permit(Triggers.CENTER_POPUP_NAV_ENABLED, (o, t) => OpenCenterPoppupRequest())
                .Permit(Triggers.CENTER_POPUP_NAV_DISABLED, (o, t) => CloseCenterPoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_ENABLED, (o, t) => EdgePoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_DISABLED, (o, t) => CloseEdgePoppupRequest())
                .Permit(Triggers.ENABLE_CONTENT, (o, t) => EnabledContent())
                .Permit(Triggers.PANE_TAPPED, (o, t) => PaneTapped())
                .Permit(Triggers.VISUAL_STATE_LARGE, States.PRIMARY_NAV)
                .Permit(Triggers.VISUAL_STATE_MEDIUM, States.MEDIUM_PRIMARY_NAV)
                .Permit(Triggers.PRIMARY_NAV_DISABLED, States.SMALL_NO_NAV);

            _mainShellFsm.Configure(States.MEDIUM_NO_NAV)
                .OnEntry((o, t) => SetNoNavMedium())
                .Permit(Triggers.HAMBURGER_MENU_CLICKED, (o, t) => HamburgerMenuClickFired())
                .Permit(Triggers.PANE_TAPPED, (o, t) => PaneTapped())
                .Permit(Triggers.CENTER_POPUP_NAV_ENABLED, (o, t) => OpenCenterPoppupRequest())
                .Permit(Triggers.CENTER_POPUP_NAV_DISABLED, (o, t) => CloseCenterPoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_ENABLED, (o, t) => EdgePoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_DISABLED, (o, t) => CloseEdgePoppupRequest())
                .Permit(Triggers.ENABLE_CONTENT, (o, t) => EnabledContent())
                .Permit(Triggers.VISUAL_STATE_LARGE, States.NO_NAV)
                .Permit(Triggers.VISUAL_STATE_SMALL, States.SMALL_NO_NAV)
                .Permit(Triggers.PRIMARY_NAV_ENABLED, States.MEDIUM_PRIMARY_NAV);

            _mainShellFsm.Configure(States.MEDIUM_PRIMARY_NAV)
                .OnEntry((o, t) => SetPrimaryNavMedium())
                .Permit(Triggers.HAMBURGER_MENU_CLICKED, (o, t) => HamburgerMenuClickFired())
                .Permit(Triggers.CENTER_POPUP_NAV_ENABLED, (o, t) => OpenCenterPoppupRequest())
                .Permit(Triggers.CENTER_POPUP_NAV_DISABLED, (o, t) => CloseCenterPoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_ENABLED, (o, t) => EdgePoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_DISABLED, (o, t) => CloseEdgePoppupRequest())
                .Permit(Triggers.ENABLE_CONTENT, (o, t) => EnabledContent())
                .Permit(Triggers.PANE_TAPPED, (o, t) => PaneTapped())
                .Permit(Triggers.VISUAL_STATE_LARGE, States.PRIMARY_NAV)
                .Permit(Triggers.VISUAL_STATE_SMALL, States.SMALL_PRIMARY_NAV)
                .Permit(Triggers.PRIMARY_NAV_DISABLED, States.MEDIUM_NO_NAV);
        }

        //===============================================================
        // State machine trigger firing
        //===============================================================

        /// <summary>
        /// Send a hamburger menu clicked event into the FSM
        /// </summary>
        internal void HambergurMenuClicked()
        {
            _mainShellFsm.Fire(Triggers.HAMBURGER_MENU_CLICKED);
        }

        internal void ArrowMenuClicked()
        {
            _mainShellFsm.Fire(Triggers.HAMBURGER_MENU_CLICKED);
        }

        internal void PaneTappedEvent()
        {
            _mainShellFsm.Fire(Triggers.PANE_TAPPED);
        }

        internal void PaneClosing()
        {
            DisableContent2 = false;
            IsPaneHeaderVisible = true;
        }

        //===============================================================
        // State machine handling of triggers / state transitions
        //===============================================================

        private void PaneTapped()
        {
            if ((_isOverlayed == true) && (IsPaneOpen == true))
            {
                SetPaneOpen(false);
                DisableContent2 = false;
            }
            return;
        }

        public void DisabledContentTapped()
        {
            _mainShellFsm.Fire(Triggers.ENABLE_CONTENT);
            return;
        }

        public void EnabledContent()
        {
            if (_edgePopupNavigationEnabled == true)
            {
                CloseEdgePoppupRequest();
                return;
            }

            if (_centerPopupNavigationEnabled == true)
            {
                CloseCenterPoppupRequest();
                return;
            }

            DisablePaneAndContent = false;
            DisableContent2 = false;
            SetPaneOpen(false);
            return;
        }

        private void EdgePoppupRequest()
        {
            DisablePaneAndContent = true;
            EdgePopupNavigationOpen = true;
            return;
        }

        private void CloseEdgePoppupRequest()
        {
            EdgePopupNavigationOpen = false;
            DisablePaneAndContent = false;
            return;
        }

        private void OpenCenterPoppupRequest()
        {
            DisablePaneAndContent = true;
            CenterPopupNavigationEnabled = true;
            return;
        }

        private void CloseCenterPoppupRequest()
        {
            DisablePaneAndContent = false;
            CenterPopupNavigationEnabled = false;
            return;
        }

        private Boolean _edgePopupNavigationEnabled = false;
        public Boolean EdgePopupNavigationEnabled
        {
            get { return _edgePopupNavigationEnabled; }
            set {
                SetProperty(ref _edgePopupNavigationEnabled, value);
            }
        }

        private Boolean _edgePopupOpen = false;
        public Boolean EdgePopupNavigationOpen
        {
            get { return _edgePopupOpen; }
            set
            {
                SetProperty(ref _edgePopupOpen, value);
            }
        }

        private Boolean _centerPopupNavigationEnabled = false;
        public Boolean CenterPopupNavigationEnabled
        {
            get { return _centerPopupNavigationEnabled; }
            set
            {
                SetProperty(ref _centerPopupNavigationEnabled, value);
            }
        }


        private Boolean HamburgerMenuClickFired()
        {
            if (IsPaneOpen == false)
            {
                if (_isOverlayed == true)
                {
                    DisableContent2 = true;
                }
                SetPaneOpen(true);
            }
            else
            {
                //if (_isOverlayed == true)
                //{
                //    DisableContent2 = true;
                //}
                DisableContent2 = false;
                SetPaneOpen(false);
            }

            return true;
        }


        /// <summary>
        /// Sets the UI to have no navigation
        /// </summary>
        /// <returns></returns>
        private Boolean SetNoNav(Triggers trigger)
        {
            Debug.WriteLine(" * ********** IN SetNoNav = " + _mainShellFsm.CurrentState);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            HamburgerMenuState = HamburgerButtonState.Menu;


            DisplayMode = LargeDisplayMode;
            // Do not change pane opened or closed on navigation events
            if (trigger != Triggers.PRIMARY_NAV_DISABLED)
                SetPaneOpen(true);

            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            Debug.WriteLine("*********** EXITING SetNoNav = " + _mainShellFsm.CurrentState);
            return true;
        }

        private Boolean SetPrimaryNav(Triggers trigger)
        {
            // Set the type of hamburger menu visisble based on the size of the app
            // No matter what size, we are navigating, so set the previous arrow
            Debug.WriteLine(" * ********** IN SetPrimaryNav = " + _mainShellFsm.CurrentState);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            HamburgerMenuState = HamburgerButtonState.Previous;
            // Do not change pane opened or closed on navigation events
            DisplayMode = LargeDisplayMode;
            if (trigger != Triggers.PRIMARY_NAV_ENABLED)
                SetPaneOpen(true);

            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            Debug.WriteLine("*********** EXITING SetPrimaryNav = " + _mainShellFsm.CurrentState);
            return true;
        }

        private Boolean SetNoNavSmall()
        {
            Debug.WriteLine(" * ********** IN SetNoNavSmall = " + _mainShellFsm.CurrentState);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            HamburgerMenuState = HamburgerButtonState.ArrowMenu;
            DisplayMode = SmallDisplayMode;
            SetPaneOpen(false);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            Debug.WriteLine("*********** EXITING SetNoNavSmall = " + _mainShellFsm.CurrentState);
            return true;
        }

        private Boolean SetPrimaryNavSmall()
        {
            Debug.WriteLine(" * ********** IN SetPrimaryNavSmall = " + _mainShellFsm.CurrentState);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            HamburgerMenuState = HamburgerButtonState.Previous;
            DisplayMode = SmallDisplayMode;
            SetPaneOpen(false);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            Debug.WriteLine("*********** EXITING SetPrimaryNavSmall = " + _mainShellFsm.CurrentState);
            return true;
        }

        private Boolean SetNoNavMedium()
        {
            Debug.WriteLine(" * ********** IN SetNoNavMedium = " + _mainShellFsm.CurrentState);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            HamburgerMenuState = HamburgerButtonState.Menu;
            DisplayMode = MediumDisplayMode;
            SetPaneOpen(false);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            Debug.WriteLine("*********** EXITING SetNoNavMedium = " + _mainShellFsm.CurrentState);
            return true;
        }

        private Boolean SetPrimaryNavMedium()
        {
            Debug.WriteLine(" * ********** IN SetPrimaryNavMedium = " + _mainShellFsm.CurrentState);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            HamburgerMenuState = HamburgerButtonState.Previous;
            DisplayMode = MediumDisplayMode;
            SetPaneOpen(false);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            Debug.WriteLine("*********** EXITING SetPrimaryNavMedium = " + _mainShellFsm.CurrentState);
            return true;
        }
        #endregion

        private Boolean SetPaneDisplayModeInternal()
        {
            Debug.WriteLine("Internal Set Display");
            return SetPaneDisplayMode();
        }

        private Boolean HamburgerMenuClicked()
        {
            Debug.WriteLine("HamburgerMenuClicked : CurrentState = " + _mainShellFsm.CurrentState);
            SetPaneOpen(!IsPaneOpen);
            return true;
        }

        private Boolean SetPaneDisplayMode()
        {
            Debug.WriteLine("Hit Trigger1 : CurrentState = " + _mainShellFsm.CurrentState);
            if (_menuVisualState == AppSizeVisualState.LARGE)
            {
                DisplayMode = LargeDisplayMode;
                SetPaneOpen(true);
                return true;
            }

            if (_menuVisualState == AppSizeVisualState.MEDIUM)
            {
                DisplayMode = MediumDisplayMode;
                SetPaneOpen(false);
                return true;
            }

            if (_menuVisualState == AppSizeVisualState.SMALL)
            {
                DisplayMode = SmallDisplayMode;
                SetPaneOpen(false);
                return true;
            }
            return true;
        }

        private Boolean DisplayTrigger2()
        {
            Debug.WriteLine("Hit Trigger2 : CurrentState = " + _mainShellFsm.CurrentState);
            return true;
        }

        private Boolean EmptyGuard()
        {
            Debug.WriteLine("Hit Guard");
            return true;
        }


        private async Task DelaySet()
        {
            await Task.Delay(4000);
            try
            {
                SetPaneOpen(true);
                DisplayMode = SplitViewDisplayMode.CompactInline;
                await Task.Delay(4000);
                DisablePaneAndContent = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Hit Exception");
            }
        }


        // The UI will overlay a grid, to make the content not accessbile
        // It should look like a hamburgermenu on mobile apps
        #region Disable Content
        private Boolean _disablePandAndContent = false;
        public Boolean DisablePaneAndContent
        {
            get { return _disablePandAndContent; }
            set
            {
                SetProperty(ref _disablePandAndContent, value);
            }
        }

        private Boolean _disableContent2 = false;
        public Boolean DisableContent2
        {
            get { return _disableContent2; }
            set
            {
                SetProperty(ref _disableContent2, value);
            }
        }

        private Boolean _navigationEnabled = true;
        public Boolean NavigationEnabled
        {
            get { return _navigationEnabled; }
            set
            {
                SetProperty(ref _navigationEnabled, value);
            }
        }

        #endregion Disable Content

        #region Size Properties for Visual States
        private Int32 _largeMinWindowWidth = 800;
        public Int32 LargeMinWindowWidth
        {
            get { return _largeMinWindowWidth; }
            set
            {
                _largeMinWindowWidth = value;
            }
        }

        private Int32 _mediumMinWindowWidth = 500;
        public Int32 MediumMinWindowWidth
        {
            get { return _mediumMinWindowWidth; }
            set
            {
                _mediumMinWindowWidth = value;
            }
        }

        public void InitWidth(Double width)
        {
            Debug.WriteLine("InitWidth = " + width);
        }

        private Int32 _smallMinWindowWidth = 0;
        public Int32 SmallMinWindowWidth
        {
            get { return _smallMinWindowWidth; }
            set
            {
                _smallMinWindowWidth = value;
            }
        }

        /// <summary>
        /// Converts a String into a internal represtnation of the visual state
        /// </summary>
        /// <param name="state"></param>
        public void NotifySizeChange(String state)
        {
            _menuVisualState = state.ToUpperInvariant().StringToVisualState();

            // Update the FSM we have had a size change
            VisualStateToFsmTrigger(_menuVisualState);
            Debug.WriteLine("VisualState Updated = " + _menuVisualState);
        }

        public void VisualStateToFsmTrigger(AppSizeVisualState state)
        {
            switch(state)
            {
                case AppSizeVisualState.LARGE:
                    {
                        _mainShellFsm.Fire(Triggers.VISUAL_STATE_LARGE);
                        return;
                    }
                case AppSizeVisualState.MEDIUM:
                    {
                        _mainShellFsm.Fire(Triggers.VISUAL_STATE_MEDIUM);
                        return;
                    }
                case AppSizeVisualState.SMALL:
                    {
                        _mainShellFsm.Fire(Triggers.VISUAL_STATE_SMALL);
                        return;
                    }
            }
            return;
        }

        #endregion

        // Sets the properties specific to the Pane
        #region Pane Properties
        Boolean _isLargeOverlayed = false;
        private SplitViewDisplayMode _largeDisplayMode = SplitViewDisplayMode.Inline;
        public SplitViewDisplayMode LargeDisplayMode
        {
            get { return _largeDisplayMode; }
            set
            {
                _largeDisplayMode = value;
                if ((_largeDisplayMode == SplitViewDisplayMode.CompactOverlay) || (_largeDisplayMode == SplitViewDisplayMode.Overlay))
                {
                    _isLargeOverlayed = true;
                }
                else
                {
                    _isLargeOverlayed = false;
                }
            }
        }

        Boolean _isMediumOverlayed = true;
        private SplitViewDisplayMode _mediumDisplayMode = SplitViewDisplayMode.CompactOverlay;
        public SplitViewDisplayMode MediumDisplayMode
        {
            get { return _mediumDisplayMode; }
            set
            {
                _mediumDisplayMode = value;
                if ((_mediumDisplayMode == SplitViewDisplayMode.CompactOverlay) || (_mediumDisplayMode == SplitViewDisplayMode.Overlay))
                {
                    _isMediumOverlayed = true;
                }
                else
                {
                    _isMediumOverlayed = false;
                }
            }
        }

        Boolean _isSmallOverlayed = true;
        private SplitViewDisplayMode _smallDisplayMode = SplitViewDisplayMode.Overlay;
        public SplitViewDisplayMode SmallDisplayMode
        {
            get { return _smallDisplayMode; }
            set
            {
                _smallDisplayMode = value;
                if ((_smallDisplayMode == SplitViewDisplayMode.CompactOverlay) || (_smallDisplayMode == SplitViewDisplayMode.Overlay))
                {
                    _isSmallOverlayed = true;
                }
                else
                {
                    _isSmallOverlayed = false;
                }
            }
        }

        private SplitViewDisplayMode _displayMode = SplitViewDisplayMode.Inline;
        public SplitViewDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set
            {
                SetProperty(ref _displayMode, value);
                if ((_displayMode == SplitViewDisplayMode.CompactOverlay) || (_displayMode == SplitViewDisplayMode.Overlay))
                {
                    _isOverlayed = true;
                    Debug.WriteLine("**** DisplayMode = {0}, overlayed = {1}", _isPaneHeaderVisible, _isOverlayed);
                }
                else
                {
                    _isOverlayed = false;
                    Debug.WriteLine("**** DisplayMode = {0}, overlayed = {1}", _isPaneHeaderVisible, _isOverlayed);
                }
            }
        }

        private void SetPaneOpen(Boolean paneOpen)
        {
            if (paneOpen == true)
            {
                if (_isOverlayed == false)
                {
                    IsPaneHeaderVisible = true;
                }
                else
                {
                    IsPaneHeaderVisible = false;
                }
                IsPaneOpen = true;
            }
            else
            {
                IsPaneHeaderVisible = true;
                IsPaneOpen = false;
            }
        }

        private Boolean _isPaneHeaderVisible = false;
        public Boolean IsPaneHeaderVisible
        {
            get {
                Debug.WriteLine("**** IsPaneHeaderVisible = {0}, overlayed = {1}", _isPaneHeaderVisible, _isOverlayed);
                return _isPaneHeaderVisible; 
            }
            set
            {
                Debug.WriteLine("**** Set IsPaneHeaderVisible = {0}, overlayed = {1}", value, _isOverlayed);
                SetProperty(ref _isPaneHeaderVisible, value);
            }
        }

        private Boolean _isPaneOpen = false;
        public Boolean IsPaneOpen
        {
            get { return _isPaneOpen; }
            set
            {
                SetProperty(ref _isPaneOpen, value);
            }
        }
        #endregion Pane Properties


    }
}
