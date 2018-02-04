using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UwpEnhancedNavigation
{
    // Allowable modes when the app is in the defined large state
    #region App Size Large
    internal enum SplitViewModeLarge
    {
        CompactInline,
        CompactOverlay,
        Inline,
        Overlay
    }

    internal static class SplitViewModeLargeMethods
    {

        public static SplitViewDisplayMode ToSplitViewDisplayMode(this SplitViewModeLarge svMode)
        {
            switch(svMode)
            {
                case SplitViewModeLarge.CompactInline:
                    {
                        return SplitViewDisplayMode.CompactInline;
                    }
                case SplitViewModeLarge.CompactOverlay:
                    {
                        return SplitViewDisplayMode.CompactOverlay;
                    }
                case SplitViewModeLarge.Inline:
                    {
                        return SplitViewDisplayMode.Inline;
                    }
                default:
                    {
                        return SplitViewDisplayMode.Overlay;
                    }
            }
        }
    }
    #endregion App Size Large

    // Allowable modes when the app is in the defined medium state
    #region App Medium Large
    internal enum SplitViewModeMedium
    {
        CompactInline,
        CompactOverlay,
        Overlay
    }

    internal static class SplitViewModeMediumeMethods
    {

        public static SplitViewDisplayMode ToSplitViewDisplayMode(this SplitViewModeMedium svMode)
        {
            switch (svMode)
            {
                case SplitViewModeMedium.CompactInline:
                    {
                        return SplitViewDisplayMode.CompactInline;
                    }
                case SplitViewModeMedium.CompactOverlay:
                    {
                        return SplitViewDisplayMode.CompactOverlay;
                    }
                default:
                    {
                        return SplitViewDisplayMode.Overlay;
                    }
            }
        }
    }
    #endregion App Medium Large

    // Allowable modes when the app is in the defined small state
    #region App Small Large
    internal enum SplitViewModeSmall
    {
        CompactOverlay,
        Overlay
    }

    internal static class SplitViewModeSmallMethods
    {

        public static SplitViewDisplayMode ToSplitViewDisplayMode(this SplitViewModeSmall svMode)
        {
            switch (svMode)
            {
                case SplitViewModeSmall.CompactOverlay:
                    {
                        return SplitViewDisplayMode.CompactOverlay;
                    }
                default:
                    {
                        return SplitViewDisplayMode.Overlay;
                    }
            }
        }
    }
    #endregion App Small Large


}
