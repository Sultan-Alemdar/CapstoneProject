using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PeerConnectionClientOperators.Signalling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp.ViewModels
{
    public sealed partial class RemoteConnectionViewModel : ViewModelBase
    {
        private bool _isPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set
            {
                Set<bool>(ref _isPaneOpen, value, "IsPaneOpen");
            }
        }
        private RelayCommand _disconnectFromPeerCommand;

        public RelayCommand DisconnectFromPeerCommand
        {
            get => _disconnectFromPeerCommand;
            set
            {
                Set<RelayCommand>(ref _disconnectFromPeerCommand, value, "DisconnectFromPeerCommand");
            }
        }


        private async void DiscconectFromPeer()
        {
            await Task.Run(() =>
            {
                if (AdapterViewModel.DisconnectFromPeerCommand.CanExecute(this))
                {
                    AdapterViewModel.DisconnectFromPeerCommand.Execute(this);
                }
            });
        }
        private bool DiscconectFromCanExecute()
        {
            return AdapterViewModel.DisconnectFromPeerCommand.CanExecute(this);
        }

    }
}
