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

namespace UwpEnhancedNavigation
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
            PrimaryNavigation.Fsm = _mainShellFsm;
            
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
                .Permit(Triggers.EDGE_POPUP_NAV_ENABLED, (o, t) => EdgePoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_DISABLED, (o, t) => CloseEdgePoppupRequest())
                .Permit(Triggers.VISUAL_STATE_LARGE, States.NO_NAV)
                .Permit(Triggers.VISUAL_STATE_MEDIUM, States.MEDIUM_NO_NAV)               
                .Permit(Triggers.PRIMARY_NAV_ENABLED, States.SMALL_PRIMARY_NAV);

            _mainShellFsm.Configure(States.SMALL_PRIMARY_NAV)
                .OnEntry((o, t) => SetPrimaryNavSmall())
                .Permit(Triggers.HAMBURGER_MENU_CLICKED, (o, t) => HamburgerMenuClickFired())
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
                .Permit(Triggers.EDGE_POPUP_NAV_ENABLED, (o, t) => EdgePoppupRequest())
                .Permit(Triggers.EDGE_POPUP_NAV_DISABLED, (o, t) => CloseEdgePoppupRequest())
                .Permit(Triggers.ENABLE_CONTENT, (o, t) => EnabledContent())
                .Permit(Triggers.VISUAL_STATE_LARGE, States.NO_NAV)
                .Permit(Triggers.VISUAL_STATE_SMALL, States.SMALL_NO_NAV)
                .Permit(Triggers.PRIMARY_NAV_ENABLED, States.MEDIUM_PRIMARY_NAV);

            _mainShellFsm.Configure(States.MEDIUM_PRIMARY_NAV)
                .OnEntry((o, t) => SetPrimaryNavMedium())
                .Permit(Triggers.HAMBURGER_MENU_CLICKED, (o, t) => HamburgerMenuClickFired())
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
            DisableContent = false;
        }

        //===============================================================
        // State machine handling of triggers / state transitions
        //===============================================================

        private void PaneTapped()
        {
            if ((_isOverlayed == true) && (IsPaneOpen == true))
            {
                IsPaneOpen = false;
                DisableContent = false;
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

            DisableContent = false;
            return;
        }

        private void EdgePoppupRequest()
        {
            DisableContent = true;
            EdgePopupNavigationEnabled = true;
            return;
        }

        private void CloseEdgePoppupRequest()
        {
            DisableContent = false;
            EdgePopupNavigationEnabled = false;
            return;
        }

        private void OpenCenterPoppupRequest()
        {
            DisableContent = true;
            CenterPopupNavigationEnabled = true;
            return;
        }

        private void CloseCenterPoppupRequest()
        {
            DisableContent = false;
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
                    DisableContent = true;
                }
                IsPaneOpen = true;
            }
            else
            {
                if (_isOverlayed == true)
                {
                    DisableContent = false;
                }
                IsPaneOpen = false;
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

            // Do not change pane opened or closed on navigation events
            if (trigger != Triggers.PRIMARY_NAV_DISABLED)
                IsPaneOpen = true;

            DisplayMode = LargeDisplayMode;
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
            if (trigger != Triggers.PRIMARY_NAV_ENABLED)
                IsPaneOpen = true;

            DisplayMode = LargeDisplayMode;
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            Debug.WriteLine("*********** EXITING SetPrimaryNav = " + _mainShellFsm.CurrentState);
            return true;
        }

        private Boolean SetNoNavSmall()
        {
            Debug.WriteLine(" * ********** IN SetNoNavSmall = " + _mainShellFsm.CurrentState);
            Debug.WriteLine("              HamburgerButtonState = " + HamburgerMenuState.ToString());
            HamburgerMenuState = HamburgerButtonState.ArrowMenu;
            IsPaneOpen = false;
            DisplayMode = SmallDisplayMode;
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
            IsPaneOpen = false;
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
            IsPaneOpen = false;
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
            IsPaneOpen = false;
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
            IsPaneOpen = !IsPaneOpen;
            return true;
        }

        private Boolean SetPaneDisplayMode()
        {
            Debug.WriteLine("Hit Trigger1 : CurrentState = " + _mainShellFsm.CurrentState);
            if (_menuVisualState == AppSizeVisualState.LARGE)
            {
                DisplayMode = LargeDisplayMode;
                IsPaneOpen = true;
                return true;
            }

            if (_menuVisualState == AppSizeVisualState.MEDIUM)
            {
                DisplayMode = MediumDisplayMode;
                IsPaneOpen = false;
                return true;
            }

            if (_menuVisualState == AppSizeVisualState.SMALL)
            {
                DisplayMode = SmallDisplayMode;
                IsPaneOpen = false;
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
                IsPaneOpen = true;
                DisplayMode = SplitViewDisplayMode.CompactInline;
                await Task.Delay(4000);
                DisableContent = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Hit Exception");
            }
        }


        // The UI will overlay a grid, to make the content not accessbile
        // It should look like a hamburgermenu on mobile apps
        #region Disable Content
        private Boolean _disableContent = false;
        public Boolean DisableContent
        {
            get { return _disableContent; }
            set
            {
                SetProperty(ref _disableContent, value);
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
                }
                else
                {
                    _isOverlayed = false;
                }
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
