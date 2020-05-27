using System;
using System.Threading.Tasks;
using DesktopApp.ViewModels;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DesktopApp.Views
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class SettingsPage : Page
    {
        private CoreDispatcher _coreDispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
        private SettingsViewModel ViewModel
        {
            get { return ViewModelLocator.Current.SettingsViewModel; }
        }

        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        private async void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await RunOnUI(CoreDispatcherPriority.Low, () =>
            {
                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(600, 500));
                ApplicationView.GetForCurrentView().TryResizeView(new Size(1400, 900));
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            });
        }
        private async Task RunOnUI(CoreDispatcherPriority priority, Action action)
        {
            await _coreDispatcher.RunAsync(priority, new DispatchedHandler(action));
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }

        /// <summary>
        /// Invoked when the Add button is clicked 
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the exception routed event.</param>
        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddButton.Flyout.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
