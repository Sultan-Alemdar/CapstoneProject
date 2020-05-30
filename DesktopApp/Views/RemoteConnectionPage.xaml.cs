using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

using System.Threading.Tasks;
using DesktopApp.Core.Models;
using DesktopApp.Old.Core.Models;
using DesktopApp.ViewModels;
using PeerConnectionClientOperators.Signalling;
using Windows.Foundation;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace DesktopApp.Views
{
    public sealed partial class RemoteConnectionPage : Page, INotifyPropertyChanged
    {
        private CoreDispatcher _coreDispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;




        public event PropertyChangedEventHandler PropertyChanged;

        private RemoteConnectionViewModel ViewModel => ViewModelLocator.Current.RemoteConnectionViewModel;


        public RemoteConnectionPage()
        {
            InitializeComponent();
            Loaded += RemoteConnectionPage_Loaded;

        }

        private async void RemoteConnectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            await RunOnUI(CoreDispatcherPriority.Low, () =>
            {
                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(600, 500));
                ApplicationView.GetForCurrentView().TryResizeView(new Size(1850, 970));
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            });

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private async Task RunOnUI(CoreDispatcherPriority priority, Action action)
        {
            await _coreDispatcher.RunAsync(priority, new DispatchedHandler(action));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int myid, messageid;
            var id = ViewModel.CreateId(out myid, out messageid);
            FileModel fileModel = new FileModel(id, "AhmetSaruhan", "pdf", 12053);
            MessageModel messageModel = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Send, null, fileModel);

            FileModel file = new FileModel("asdasd", "AhmetSaruhan", "pdf", 12053);
            MessageModel ms = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Received, null, fileModel);

            ViewModel.MessageItems.Add(messageModel);
            ViewModel.FileItems.Add(fileModel);
            ViewModel.MessageItems.Add(ms);
            ViewModel.FileItems.Add(file);
        }


    }
}
