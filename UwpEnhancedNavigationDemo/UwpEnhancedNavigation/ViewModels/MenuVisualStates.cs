using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpEnhancedNavigation
{
    /// <summary>
    /// VisualState of the SplitView based on the Adaptive Triggers defined and application size
    /// </summary>
    internal enum MenuVisualState
    {
        SMALL,               // The Hamburger Menu is hidden
        MEDIUM,              // Only the Hamburger Menu icons show
        LARGE,               // Hamburger Menu icons and labels are shown 
        UNKNOWN              // No idea
    };

    internal static class MenuVisualStateMethods
    {

        public static String ToString(this MenuVisualState vState)
        {
            switch (vState)
            {
                case MenuVisualState.SMALL:
                    {
                        return "SMALL";
                    }
                case MenuVisualState.MEDIUM:
                    {
                        return "MEDIUM";
                    }
                case MenuVisualState.LARGE:
                    {
                        return "LARGE";
                    }
                default:
                    {
                        return "UNKNOWN";
                    }
            }
        }

        public static MenuVisualState StringToVisualState(this String vState)
        {
            switch (vState)
            {
                case "SMALL":
                    {
                        return MenuVisualState.SMALL;
                    }
                case "MEDIUM":
                    {
                        return MenuVisualState.MEDIUM;
                    }
                case "LARGE":
                    {
                        return MenuVisualState.LARGE;
                    }
                default:
                    {
                        return MenuVisualState.UNKNOWN;
                    }
            }
        }
    }
}
