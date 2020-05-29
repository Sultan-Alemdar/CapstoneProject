using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DesktopApp.ViewModels;
using WebRTCAdapter.Adapters;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace DesktopApp.Views
{
    public sealed partial class OperationsPage : Page
    {
        private CoreDispatcher _coreDispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
        public OperationsViewModel ViewModel
        {
            get => ViewModelLocator.Current.OperationsViewModel;

        }
        public OperationsPage()
        {
            InitializeComponent();
            this.Loaded += OperationsPage_Loaded;
        }

        private async void OperationsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await RunOnUI(CoreDispatcherPriority.Low, () =>
            {
                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(600, 600));
                ApplicationView.GetForCurrentView().TryResizeView(new Size(600, 600));
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
                PeerId.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            });
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            //{
            //    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            //}

        }

        private async Task RunOnUI(CoreDispatcherPriority priority, Action action)
        {
            await _coreDispatcher.RunAsync(priority, new DispatchedHandler(action));
        }

       
    }
}
