using System;
using System.Reflection.Metadata;
using System.Windows.Input;
using DesktopApp.Services;
using GalaSoft.MvvmLight;
using DesktopApp.Constants;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Threading.Tasks;
using WebRTCAdapter.Adapters;
using PeerConnectionClientOperators.Signalling;

namespace DesktopApp.ViewModels
{
    public class OperationsViewModel : ViewModelBase
    {
        private ViewModelLocator _viewModelLocator = ViewModelLocator.Current;
        private AdapterViewModel _adapterViewModel;

        public AdapterViewModel AdapterViewModel => _adapterViewModel;
        // public string OpSetButNam { get => "Ayarları Aç"; }
        public ICommand OpenSettingsPageCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ForwardCommand { get; set; }
        public ICommand ConnectCommand { get; set; }
        private int _myId = -1;
        public int MyId { get => _myId; set => Set<int>("MyId", ref this._myId, value); }


        private string _peerId = "Remote ID";
        public string PeerId { get => _peerId; set => Set<string>("PeerId", ref this._peerId, value); }




        public OperationsViewModel()
        {
            Task.Factory.StartNew(() =>
            {
                _adapterViewModel = AdapterViewModel.Instance;
                OpenSettingsPageCommand = new RelayCommand(OpenSettingsPage);
                ConnectCommand = new RelayCommand(Connect);
                // Conductor.Instance.
            });

        }

        private bool ConnectCommandCanExecute()
        {
            return true;
        }

        private void Connect()
        {

        }

        public void OpenSettingsPage()
        {
            _viewModelLocator.NavigationService.Navigate(MyConstants.SETTINGS_VIEW_MODEL_FULL_NAME);
        }

    }
}
