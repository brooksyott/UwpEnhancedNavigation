using System;
using System.Windows.Input;

namespace UwpEnhancedNavigationDemo
{
    public enum MenuTypes
    {
        IMAGE,
        GLYPH,
        INITIALS
    };

    public class MenuItem : BindableBase
    {
        private string _glyph;
        private string _text;
        private string _imagePage;
        private string _initials;
        private DelegateCommand _command;
        private Type _navigationDestination;
        private Boolean _clearNavigationStack = true;
        private MenuTypes _menuType = MenuTypes.GLYPH;

        public string Glyph
        {
            get { return _glyph; }
            set { _menuType = MenuTypes.GLYPH; SetProperty(ref _glyph, value); }
        }

        public string ImagePath
        {
            get { return _imagePage; }
            set { _menuType = MenuTypes.IMAGE; SetProperty(ref _imagePage, value); }
        }

        public string Initials
        {
            get { return _initials; }
            set { _menuType = MenuTypes.INITIALS; SetProperty(ref _initials, value); }
        }

        public MenuTypes MenuType
        {
            get { return _menuType; }
            set { SetProperty(ref _menuType, value); }
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public ICommand Command
        {
            get { return _command; }
            set { SetProperty(ref _command, (DelegateCommand)value); }
        }

        public Type NavigationDestination
        {
            get { return _navigationDestination; }
            set { SetProperty(ref _navigationDestination, value); }
        }

        // Added this type which will cause the navigation stack to be cleared
        // and adds for flatter navigation
        public Boolean ClearNavigationStack
        {
            get { return _clearNavigationStack; }
            set { SetProperty(ref _clearNavigationStack, value); }
        }

        public bool IsNavigation => _navigationDestination != null;
    }
}
