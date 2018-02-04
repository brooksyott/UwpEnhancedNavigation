using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            DisableContent = true;
            Task.Run(DelaySet);
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
        public void UpdateVisualState(String state)
        {
            _menuVisualState = state.ToUpperInvariant().StringToVisualState();
            Debug.WriteLine("VisualState Updated = " + _menuVisualState);
        }

        #endregion

        // Sets the properties specific to the Pane
        #region Pane Properties
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
