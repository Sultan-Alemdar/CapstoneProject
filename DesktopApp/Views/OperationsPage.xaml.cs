using System;

using DesktopApp.ViewModels;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;

namespace DesktopApp.Views
{
    public sealed partial class OperationsPage : Page
    {
        private OperationsViewModel ViewModel
        {
            get { return ViewModelLocator.Current.OperationsViewModel; }
        }
        public OperationsPage()
        {
            InitializeComponent();

        }

        private  void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            //{
            //    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            //}
            ViewModelLocator.Current.NavigationService.Navigate(Constants.MyConstants.REMOTE_CONNECTION_VIEW_MODEL_FULL_NAME);
        }



    }
}
