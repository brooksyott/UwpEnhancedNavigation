using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peamel.UwpShell
{
    public enum HamburgerButtonState
    {
        Menu,                               // The normal hamburgermenu, shown in a medium or large state
        ArrowMenu,                          // Shows a back arrow, when the visual state is small
        Previous                            // When navigation has occurred, we show a previous arrow
    }
}
