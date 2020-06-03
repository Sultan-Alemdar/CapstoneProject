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
using Windows.UI.Xaml;
using static PeerConnectionClientOperators.Signalling.Conductor;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.Foundation;
using WebRTCAdapter.Utilities;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;

namespace DesktopApp.ViewModels
{
    public class OperationsViewModel : ViewModelBase
    {
        private ViewModelLocator _viewModelLocator = ViewModelLocator.Current;
        private AdapterViewModel _adapterViewModel;
        public AdapterViewModel AdapterViewModel { get => _adapterViewModel; set => Set<AdapterViewModel>("AdapterViewModel", ref this._adapterViewModel, value); }

        public RelayCommand OpenSettingsPageCommand { get; set; }
        public RelayCommand CopyToClipboardCommand { get; set; }
        public RelayCommand TryReConnectToServerCommand { get; set; }
        public RelayCommand _connecToPeerCommand;
        public RelayCommand ConnecToPeerCommand { get => _connecToPeerCommand; set => Set<RelayCommand>("ConnecToPeerCommand", ref this._connecToPeerCommand, value); }

        private bool _isConnected = false;
        public bool IsConnected { get => _isConnected; set => Set<bool>("IsConnected", ref this._isConnected, value); }

        private int _myId = -1;
        public int MyId { get => _myId; set => Set<int>("MyId", ref this._myId, value); }

        private string _foreground = "Red";
        public string Foreground { get => _foreground; set => Set<string>("Foreground", ref this._foreground, value); }

        private int _peerId;
        public int PeerId { get => _peerId; set => Set<int>("PeerId", ref this._peerId, value); }

        private CoreDispatcher _coreDispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
        public OperationsViewModel()
        {
            ConnectTo();
            OpenSettingsPageCommand = new RelayCommand(OpenSettingsPage);
            TryReConnectToServerCommand = new RelayCommand(TryReConnectToServer);
            CopyToClipboardCommand = new RelayCommand(CopyToClipboard, CopyToClipboardCanExecute);
            _connecToPeerCommand = new RelayCommand(ConnectToPeer, ConnectToPeerCanExecute);
        }

        private async void _adapterViewModel_OnInitialized()
        {
            await Task.Run(() =>
             {
                 _adapterViewModel.ConnectCommand.Execute(this);

             });

        }
        private async void ConnectToPeer()
        {
            try
            {
                await Task.Run(async () =>
                    {
                        var peers = (IList<WebRTCAdapter.Model.Peer>)AdapterViewModel.Instance.Peers;
                        if (peers != null)
                        {
                            var e = from p in peers where PeerId == p.Id select p;

                            if (e.Count() > 0)
                            {
                                AdapterViewModel.SelectedPeer = e.First<WebRTCAdapter.Model.Peer>();

                                if (AdapterViewModel.ConnectToPeerCommand.CanExecute(this))
                                {
                                    await RunOnUI(CoreDispatcherPriority.High, () =>
                                     {
                                         _viewModelLocator.NavigationService.Navigate(MyConstants.REMOTE_CONNECTION_VIEW_MODEL_FULL_NAME);
                                         AdapterViewModel.ConnectToPeerCommand.Execute(this);


                                     });
                                    return;
                                }
                            }
                        }
                        await RunOnUI(CoreDispatcherPriority.High, async () =>
                        {
                            var messageDialog = new MessageDialog("Peer could not being found. Please check Peer Id and try again.", "Maching Error!");
                            await messageDialog.ShowAsync();

                        });

                    });
            }
            catch (Exception e)
            {
                Debug.WriteLine("[Error] OperationsViewModel : Starting of Connect to Peer operation was ended with error : " + e.Message);
            }

        }
        private bool ConnectToPeerCanExecute()
        {

            return MyId != -1;
        }

        private async void CopyToClipboard()
        {
            await Task.Run(async () =>
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(MyId.ToString());
                await RunOnUI(CoreDispatcherPriority.High, () =>
                 {

                     Clipboard.SetContent(dataPackage);
                 });
            });

        }
        private bool CopyToClipboardCanExecute()
        {
            return MyId != -1;
        }


        private async void ConnectTo()
        {

            await Task.Run(() =>
             {
                 var task = RunOnUI(CoreDispatcherPriority.Low, () =>
                   {

                       AdapterViewModel = WebRTCAdapter.Adapters.AdapterViewModel.Instance;
                       AdapterViewModel.OnInitialized += Instance_OnInitialized;

                   });

                 Conductor.Instance.Signaller.OnMyIdCast += Signaller_OnMyIdCast;
                 Conductor.Instance.OnPeerConnectionCreated += Instance_OnPeerConnectionCreated;

             });
        }

        private async void Instance_OnPeerConnectionCreated()
        {
            await RunOnUI(CoreDispatcherPriority.High, () =>
            {
                _viewModelLocator.NavigationService.Navigate(MyConstants.REMOTE_CONNECTION_VIEW_MODEL_FULL_NAME);
            });
        }

    

        private async void Instance_OnInitialized()
        {
            await Task.Run(() =>
             {
                 var settings = ApplicationData.Current.LocalSettings;
                 string ip = "127.0.0.1", port = "8888";
                 if (settings.Values["PeerCCServerIp"] != null)
                 {
                     var peerCcServerIp = new ValidableNonEmptyString((string)settings.Values["PeerCCServerIp"]);
                     ip = peerCcServerIp.Value;
                 }

                 if (settings.Values["PeerCCServerPort"] != null)
                 {
                     var peerCcPortInt = Convert.ToInt32(settings.Values["PeerCCServerPort"]);
                     port = peerCcPortInt.ToString();
                 }
                 AdapterViewModel.IsConnecting = true;
                 Conductor.Instance.StartLogin(ip, port);
             });
        }

        private async void Signaller_OnMyIdCast(int id)
        {
            if (MyId == id)
                return;
            await _coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
              {
                  MyId = id;
                  CopyToClipboardCommand.RaiseCanExecuteChanged();
                  ConnecToPeerCommand.RaiseCanExecuteChanged();
              });
        }

        private async Task RunOnUI(CoreDispatcherPriority priority, Action action)
        {
            await _coreDispatcher.RunAsync(priority, new DispatchedHandler(action));
        }

        public void TryReConnectToServer()
        {
            if (AdapterViewModel == null)
                return;

            if (AdapterViewModel.DisconnectFromServerCommand.CanExecute(this))
                AdapterViewModel.DisconnectFromServerCommand.Execute(this);

            if (AdapterViewModel.ConnectCommand.CanExecute(this))
                AdapterViewModel.ConnectCommand.Execute(this);

        }
        public void TryConnectToPeerCommand()
        {
            //RunOnUiThread(() =>
            //{
            //    IsConnected = true;
            //    IsMicrophoneEnabled = true;
            //    IsCameraEnabled = true;
            //    IsConnecting = false;
            //});



        }
        public bool TryConnectToPeerCommandCanExecute()
        {
            return IsConnected;
        }

        public void OpenSettingsPage()
        {
            _viewModelLocator.NavigationService.Navigate(MyConstants.SETTINGS_VIEW_MODEL_FULL_NAME);
        }

    }
}
