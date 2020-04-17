using System;

using DesktopApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

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
            ViewModelLocator.Current.RemoteConnectionViewModel.PastInteractionStackPanel = PastInteractionStackPanel;

        }

        private void ScrollViewer_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Manipualitaon Worked");

        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModelLocator.Current.RemoteConnectionViewModel.ndk();
        }
    }
}
