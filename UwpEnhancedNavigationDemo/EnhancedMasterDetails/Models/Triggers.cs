using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peamel.UwpEnhancedMasterDetails
{
    public enum Triggers
    {
        UNKNOWN = 0,
        VISUAL_STATE_SMALL = 1,
        VISUAL_STATE_MEDIUM = 2,
        VISUAL_STATE_LARGE = 3,
        PRIMARY_NAV_ENABLED = 4,
        PRIMARY_NAV_DISABLED = 5,
        SECONDARY_NAV_ENABLED = 6,
        SECONDARY_NAV_DISABLED = 7,
        HAMBURGER_MENU_CLICKED = 8,
        EDGE_POPUP_NAV_ENABLED = 9,
        EDGE_POPUP_NAV_DISABLED = 10,
        CENTER_POPUP_NAV_ENABLED = 11,
        CENTER_POPUP_NAV_DISABLED = 12,
        ENABLE_CONTENT = 13,
        DISABLE_CONTENT = 14,
        PANE_TAPPED
    }
}
