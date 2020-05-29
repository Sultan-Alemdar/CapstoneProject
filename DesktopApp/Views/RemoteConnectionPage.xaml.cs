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

        private Visibility closeButonVisibility = Visibility.Collapsed;
        private Visibility openButtonVisibility = Visibility.Visible;

        private bool pinned = false;

        public event PropertyChangedEventHandler PropertyChanged;

        private RemoteConnectionViewModel ViewModel => ViewModelLocator.Current.RemoteConnectionViewModel;

        public Visibility UnPinButtonVisibility { get => closeButonVisibility; set { closeButonVisibility = value; OnPropertyChanged("UnPinButtonVisibility"); } }
        public Visibility PinButtonVisibility { get => openButtonVisibility; set { openButtonVisibility = value; OnPropertyChanged("PinButtonVisibility"); } }



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
            AppBarClose.Begin();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ScrollViewer_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Manipualitaon Worked");

        }



        private void PinButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            pinned = true;
            SwichVisibility();

        }
        private void UnpinButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            pinned = false;
            SwichVisibility();
        }

        private void AppBarGrid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            #region eski kod
            //Debug.WriteLine("Ent");
            //if (pinned == false)
            //{
            //    if (DecideInOrOut(AppBarGrid) ==true && isOpen==false)
            //    {
            //        AppBarClose.Pause();
            //        isOpen = true;
            //        AppBarOpen.Begin();
            //        Debug.WriteLine("Opened");
            //    }
            //    else if(DecideInOrOut(AppBarGrid) ==false && isOpen==true)
            //    {
            //        AppBarOpen.Pause();
            //        isOpen = false;
            //        AppBarClose.Begin();
            //        Debug.WriteLine("Closed");
            //    }
            //} 
            #endregion
            if (pinned == false)
                AppBarOpen.Begin();
        }
        private void AppBarGrid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            #region eski kod
            //Debug.WriteLine("Ext");
            //if (pinned == false)
            //{
            //    if (DecideInOrOut(AppBarGrid) ==true && isOpen==false)
            //    {
            //        AppBarClose.Pause();
            //        isOpen = true;
            //        AppBarOpen.Begin();
            //        Debug.WriteLine("Opened");
            //    }
            //    else if(DecideInOrOut(AppBarGrid) ==false && isOpen==true)
            //    {
            //        AppBarOpen.Pause();
            //        isOpen = false;
            //        AppBarClose.Begin();
            //        Debug.WriteLine("Closed");
            //    }
            //} 
            #endregion
            if (pinned == false)
                AppBarClose.Begin();
        }


        private void SwichVisibility()
        {
            if (openButtonVisibility == Visibility.Collapsed)
            {
                UnPinButtonVisibility = Visibility.Collapsed;
                PinButtonVisibility = Visibility.Visible;
            }
            else
            {
                PinButtonVisibility = Visibility.Collapsed;
                UnPinButtonVisibility = Visibility.Visible;
            }
        }


        private async Task RunOnUI(CoreDispatcherPriority priority, Action action)
        {
            await _coreDispatcher.RunAsync(priority, new DispatchedHandler(action));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageModel messageModel = new MessageModel();
            FileModel fileModel = messageModel.File;
            ViewModel.FileItems.Add(fileModel);
            ViewModel.MessageItems.Add(messageModel);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageModel messageModel = new MessageModel();
            FileModel fileModel = messageModel.File;
            ViewModel.FileItems.Add(fileModel);
            ViewModel.MessageItems.Add(messageModel);
        }
    }
}
