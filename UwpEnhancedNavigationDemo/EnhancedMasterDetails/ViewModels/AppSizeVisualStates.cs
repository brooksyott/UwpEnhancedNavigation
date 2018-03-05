using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peamel.UwpEnhancedMasterDetails
{
    /// <summary>
    /// VisualState of the SplitView based on the Adaptive Triggers defined and application size
    /// </summary>
    internal enum AppSizeVisualState
    {
        SMALL,               // The Hamburger Menu is hidden
        MEDIUM,              // Only the Hamburger Menu icons show
        LARGE,               // Hamburger Menu icons and labels are shown 
        UNKNOWN              // No idea
    };

    internal static class MenuVisualStateMethods
    {

        public static String ToString(this AppSizeVisualState vState)
        {
            switch (vState)
            {
                case AppSizeVisualState.SMALL:
                    {
                        return "SMALL";
                    }
                case AppSizeVisualState.MEDIUM:
                    {
                        return "MEDIUM";
                    }
                case AppSizeVisualState.LARGE:
                    {
                        return "LARGE";
                    }
                default:
                    {
                        return "UNKNOWN";
                    }
            }
        }

        public static AppSizeVisualState StringToVisualState(this String vState)
        {
            switch (vState)
            {
                case "SMALL":
                    {
                        return AppSizeVisualState.SMALL;
                    }
                case "MEDIUM":
                    {
                        return AppSizeVisualState.MEDIUM;
                    }
                case "LARGE":
                    {
                        return AppSizeVisualState.LARGE;
                    }
                default:
                    {
                        return AppSizeVisualState.UNKNOWN;
                    }
            }
        }
    }
}
