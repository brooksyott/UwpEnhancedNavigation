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
        
        private MenuVisualState _menuVisualState = MenuVisualState.UNKNOWN;

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

        EnhancedNavigationFSM<States, Triggers> _mainShellFsm = new EnhancedNavigationFSM<States, Triggers>(States.NO_NAV);

        private void SetupFSM()
        {

            _mainShellFsm.Configure(States.NO_NAV)
                .OnEntry(SetPaneDisplayMode)
                .Permit(Triggers.VISUAL_STATE_LARGE, SetPaneDisplayModeInternal)
                .Permit(Triggers.VISUAL_STATE_MEDIUM, SetPaneDisplayModeInternal)
                .Permit(Triggers.VISUAL_STATE_SMALL, States.SMALL_NO_NAV);

            _mainShellFsm.Configure(States.SMALL_NO_NAV)
                .OnEntry(SetPaneDisplayMode)
                .Permit(Triggers.VISUAL_STATE_MEDIUM, States.NO_NAV);

        }

        private Boolean SetPaneDisplayModeInternal()
        {
            Debug.WriteLine("Internal Set Display");
            return SetPaneDisplayMode();
        }

        private Boolean SetPaneDisplayMode()
        {
            Debug.WriteLine("Hit Trigger1 : CurrentState = " + _mainShellFsm.CurrentState);
            if (_menuVisualState == MenuVisualState.LARGE)
            {
                DisplayMode = LargeDisplayMode;
                IsPaneOpen = true;
                return true;
            }

            if (_menuVisualState == MenuVisualState.MEDIUM)
            {
                DisplayMode = MediumDisplayMode;
                IsPaneOpen = false;
                return true;
            }

            if (_menuVisualState == MenuVisualState.SMALL)
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

        #endregion Singleton Implementation

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

        public void VisualStateToFsmTrigger(MenuVisualState state)
        {
            switch(state)
            {
                case MenuVisualState.LARGE:
                    {
                        _mainShellFsm.Fire(Triggers.VISUAL_STATE_LARGE);
                        return;
                    }
                case MenuVisualState.MEDIUM:
                    {
                        _mainShellFsm.Fire(Triggers.VISUAL_STATE_MEDIUM);
                        return;
                    }
                case MenuVisualState.SMALL:
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
        private SplitViewDisplayMode _largeDisplayMode = SplitViewDisplayMode.Inline;
        public SplitViewDisplayMode LargeDisplayMode
        {
            get { return _largeDisplayMode; }
            set
            {
                _largeDisplayMode = value;
            }
        }

        private SplitViewDisplayMode _mediumDisplayMode = SplitViewDisplayMode.CompactInline;
        public SplitViewDisplayMode MediumDisplayMode
        {
            get { return _mediumDisplayMode; }
            set
            {
                _mediumDisplayMode = value;
            }
        }

        private SplitViewDisplayMode _smallDisplayMode = SplitViewDisplayMode.Overlay;
        public SplitViewDisplayMode SmallDisplayMode
        {
            get { return _smallDisplayMode; }
            set
            {
                _smallDisplayMode = value;
            }
        }

        private SplitViewDisplayMode _displayMode = SplitViewDisplayMode.Inline;
        public SplitViewDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set
            {
                SetProperty(ref _displayMode, value);
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
