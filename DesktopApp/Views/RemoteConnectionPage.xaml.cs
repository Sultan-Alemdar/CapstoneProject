using System;

using DesktopApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DesktopApp.Views
{
    public sealed partial class RemoteConnectionPage : Page
    {
        private RemoteConnectionViewModel ViewModel
        {
            get { return ViewModelLocator.Current.RemoteConnectionViewModel; }
        }

        public RemoteConnectionPage()
        {
            InitializeComponent();
        }
    }
}
