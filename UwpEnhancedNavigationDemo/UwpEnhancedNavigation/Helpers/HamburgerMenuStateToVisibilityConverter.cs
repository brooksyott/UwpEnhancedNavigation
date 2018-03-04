using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UwpEnhancedNavigation
{
    /// <summary>
    /// Based on the state, determines if the hamburgermenu should be visible
    /// </summary>
    public class HamburgerButtonStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //HamburgerButtonState state = (HamburgerButtonState)value;
            //if (state == HamburgerButtonState.Menu) return Visibility.Visible;
            //return Visibility.Collapsed;
            return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }

    /// <summary>
    /// Convert the inverse Boolean value to a UI Element visibility
    /// </summary>
    public class HamburgerArrowStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            HamburgerButtonState state = (HamburgerButtonState)value;
            if (state == HamburgerButtonState.ArrowMenu)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Collapsed;
        }
    }

    public class PreviousArrowStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            HamburgerButtonState state = (HamburgerButtonState)value;
            if (state == HamburgerButtonState.Previous) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Collapsed;
        }
    }
}
