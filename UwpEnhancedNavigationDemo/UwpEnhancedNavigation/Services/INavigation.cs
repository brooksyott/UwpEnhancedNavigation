using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpEnhancedNavigation
{
    /// <summary>
    /// Interface definition for Navigation Helpers
    /// </summary>
    public interface INavigation
    {
        bool Navigate(Type sourcePageType, Boolean ClearNavStack = false);
        void GoBack();
        Boolean CanGoBack();
        void ClearNavigationHistory();
    }
}
