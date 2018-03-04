using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UwpEnhancedNavigationDemo
{
    public class MenuDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MenuItemTemplateImage { get; set; }
        public DataTemplate MenuItemTemplateGlyph { get; set; }
        public DataTemplate MenuItemTemplateInitials { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            MenuItem menuItem = item as MenuItem;
            if (menuItem.MenuType == MenuTypes.IMAGE)
            {
                return MenuItemTemplateImage;
            }
            if (menuItem.MenuType == MenuTypes.GLYPH)
            {
                return MenuItemTemplateGlyph;
            }
            return MenuItemTemplateInitials;
        }
    }
}
