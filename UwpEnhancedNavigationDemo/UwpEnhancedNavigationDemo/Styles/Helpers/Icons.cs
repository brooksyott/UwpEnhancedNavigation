using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpEnhancedNavigationDemo
{
    public static class Icon
    {
        public static string GetIcon(string name)
        {
            return (string)Windows.UI.Xaml.Application.Current.Resources[name];
        }
    }
}
