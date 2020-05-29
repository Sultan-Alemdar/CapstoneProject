using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace DesktopApp.Views.SubViews
{
    public sealed partial class TopBar : UserControl, INotifyPropertyChanged
    {
        private bool _isPinned = false;

        public bool IsPinned
        {
            get => _isPinned;
            set
            {
                _isPinned = value;
                OnPropertyChanged("IsPinned");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public TopBar()
        {
            this.InitializeComponent();
            this.Loaded += TopBar_Loaded;
        }

        private void TopBar_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarClose.Begin();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void AppBarGrid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            if (_isPinned == false)
                AppBarOpen.Begin();
        }
        private void AppBarGrid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            if (_isPinned == false)
                AppBarClose.Begin();
        }

    }
}
