using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebRTCAdapter.Adapters;
using Windows.UI.Xaml;

namespace DesktopApp.ViewModels
{
    public sealed partial class SettingsViewModel : ViewModelBase
    {
        private readonly AdapterViewModel _adapterViewModel = AdapterViewModel.Instance;

        public AdapterViewModel AdapterViewModel => _adapterViewModel;

        private object InstanceLock = new object();

        private ICommand _reRonnectCommand;

        public ICommand ReConnectCommand
        {
            get
            {
                if (_reRonnectCommand == null)
                {
                    lock (InstanceLock)
                    {
                        _reRonnectCommand = new RelayCommand(ReConnect);
                    }
                }
                return _reRonnectCommand;
            }
        }

        public void ReConnect()
        {
            lock (InstanceLock)
            {
                if (AdapterViewModel.DisconnectFromServerCommand.CanExecute(this))
                    AdapterViewModel.DisconnectFromServerCommand.Execute(this);

                if (AdapterViewModel.ConnectCommand.CanExecute(this))
                    AdapterViewModel.ConnectCommand.Execute(this);
            }
        }
    }
}
