using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRTCAdapter.Adapters;
using Windows.UI.Xaml;

namespace DesktopApp.ViewModels
{
    public sealed partial class SettingsViewModel : ViewModelBase
    {
        private readonly AdapterViewModel _adapterViewModel = AdapterViewModel.Instance;

        public AdapterViewModel AdapterViewModel => _adapterViewModel;

        
    }
}
