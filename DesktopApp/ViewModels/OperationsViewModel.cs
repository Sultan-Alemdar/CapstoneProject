using System;
using System.Reflection.Metadata;
using System.Windows.Input;
using DesktopApp.Services;
using GalaSoft.MvvmLight;
using DesktopApp.Constants;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Threading.Tasks;
using DesktopApp.Signaling;

namespace DesktopApp.ViewModels
{
    public class OperationsViewModel : ViewModelBase
    {
        private ViewModelLocator _viewModelLocator = ViewModelLocator.Current;
        // public string OpSetButNam { get => "Ayarları Aç"; }
        public ICommand OpenSettingsPageCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ForwardCommand { get; set; }
        public ICommand ConnectCommand { get; set; }
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
            OpenSettingsPageCommand = new RelayCommand(OpenSettingsPageExecute);
            BackCommand = new RelayCommand(GoBackExecute, CanGoBack);
            ForwardCommand = new RelayCommand(GoForwardExecute, CanGoForward);
            ConnectCommand = new RelayCommand(ConnectCommandExecute, ConnectCommandCanExecute);
        }

        private bool ConnectCommandCanExecute()
        {
            throw new NotImplementedException();
        }

        private void ConnectCommandExecute()
        {
            new Task(() =>
            {
                //IsConnecting = true;
                //Conductor.Instance.StartLogin(Ip.Value, Port.Value);
            }).Start();
        }

        public void OpenSettingsPageExecute()
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
        public void GoBackExecute()
        {
            _viewModelLocator.NavigationService.GoBack();
        }
        public void GoForwardExecute()
        {
            _viewModelLocator.NavigationService.GoForward();
        }
    }
}
