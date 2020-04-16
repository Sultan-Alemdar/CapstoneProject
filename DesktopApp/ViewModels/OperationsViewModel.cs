using System;
using System.Reflection.Metadata;
using System.Windows.Input;
using DesktopApp.Services;
using GalaSoft.MvvmLight;
using DesktopApp.Constants;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;

namespace DesktopApp.ViewModels
{
    public class OperationsViewModel : ViewModelBase
    {
        private ViewModelLocator _viewModelLocator = ViewModelLocator.Current;
        // public string OpSetButNam { get => "Ayarları Aç"; }
        public ICommand OnOpenSettingsPageCommand { get; set; }
        public ICommand OnBackCommand { get; set; }
        public ICommand OnForwardCommand { get; set; }

        private string _ID = "ID";
        public string ID { get => _ID; set => Set<string>("ID", ref this._ID, value); }
        private string _password = "Password";
        public string Password { get => _password; set => Set<string>("Password", ref this._password, value); }

        private string _remoteID="Remote ID";
        public string RemoteID { get => _remoteID; set => Set<string>("RemoteID", ref this._remoteID, value); }
        private string _remotePassword = "Remote Password";
        public string RemotePassword { get => _remotePassword; set => Set<string>("RemotePassword", ref this._remotePassword, value); }



        public OperationsViewModel()
        {
            OnOpenSettingsPageCommand = new RelayCommand(OnOpenSettingsPageMethod);
            OnBackCommand = new RelayCommand(OnGoBackMethod, CanGoBack);
            OnForwardCommand = new RelayCommand(OnGoForwardMethod, CanGoForward);
        }
        public void OnOpenSettingsPageMethod()
        {
            _viewModelLocator.NavigationService.Navigate(MyConstants.SETTINGS_VIEW_MODEL_FULL_NAME);
        }
        public bool CanGoBack()
        {
            return _viewModelLocator.NavigationService.CanGoBack;
        }
        public bool CanGoForward()
        {
            return _viewModelLocator.NavigationService.CanGoForward;
        }
        public void OnGoBackMethod()
        {
            _viewModelLocator.NavigationService.GoBack();
        }
        public void OnGoForwardMethod()
        {
            _viewModelLocator.NavigationService.GoForward();
        }
    }
}
