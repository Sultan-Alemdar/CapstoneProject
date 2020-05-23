using System;
using System.Diagnostics;
using DesktopApp.ViewModels;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

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
            ApplicationView.PreferredLaunchViewSize = new Size(600, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //this.SizeChanged += OperationsPage_SizeChanged;
        }



        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            //{
            //    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            //}
            ViewModelLocator.Current.NavigationService.Navigate(Constants.MyConstants.REMOTE_CONNECTION_VIEW_MODEL_FULL_NAME);
        }

     
    }
}
