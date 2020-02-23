﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Peamel.UwpShell
{
    public sealed partial class ShellNavigationButton : UserControl, INotifyPropertyChanged
    {
        // Attached to the view model
        private ShellViewModel ViewModel = ShellViewModel.Instance;

        public ShellNavigationButton()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register(
              "ButtonBackground",
              typeof(Brush),
              typeof(EnhancedShell),
              new PropertyMetadata(null)
            );

        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ButtonForegroundProperty = DependencyProperty.Register(
              "ButtonForeground",
              typeof(Brush),
              typeof(EnhancedShell),
              new PropertyMetadata(null)
            );

        public Brush ButtonForeground
        {
            get { return (Brush)GetValue(ButtonForegroundProperty); }
            set { SetValue(ButtonForegroundProperty, value); }
        }

        public static readonly DependencyProperty ButtonHoverForegroundProperty = DependencyProperty.Register(
              "ButtonHoverForeground",
              typeof(Brush),
              typeof(EnhancedShell),
              new PropertyMetadata(null)
            );

        public Brush ButtonHoverForeground
        {
            get { return (Brush)GetValue(ButtonHoverForegroundProperty); }
            set { SetValue(ButtonHoverForegroundProperty, value); }
        }

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
