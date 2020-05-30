using System;

using DesktopApp.Services;
using DesktopApp.Views;
using WebRTCAdapter.Adapters;
using GalaSoft.MvvmLight.Ioc;

namespace DesktopApp.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;

        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            Register<OperationsViewModel, OperationsPage>();
            Register<RemoteConnectionViewModel, RemoteConnectionPage>();
            Register<SettingsViewModel, SettingsPage>();
     

        }

        public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public RemoteConnectionViewModel RemoteConnectionViewModel => SimpleIoc.Default.GetInstance<RemoteConnectionViewModel>();

        public OperationsViewModel OperationsViewModel => SimpleIoc.Default.GetInstance<OperationsViewModel>();

        public NavigationServiceEx NavigationService => SimpleIoc.Default.GetInstance<NavigationServiceEx>();

        

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
