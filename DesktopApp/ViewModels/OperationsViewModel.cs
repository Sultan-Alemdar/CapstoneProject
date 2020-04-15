using System;
using System.Reflection.Metadata;
using System.Windows.Input;
using DesktopApp.Services;
using GalaSoft.MvvmLight;
using DesktopApp.Constants;
using GalaSoft.MvvmLight.Command;

namespace DesktopApp.ViewModels
{
    public class OperationsViewModel : ViewModelBase
    {
        private ViewModelLocator _viewModelLocator = ViewModelLocator.Current;
        public string OpSetButNam { get => "Ayarları Aç"; }
        public ICommand OnOpenSettingsPageCommand { get; set; }

        public OperationsViewModel()
        {
            OnOpenSettingsPageCommand = new RelayCommand(OnOpenSettingsPageMethod);
        }
        public void OnOpenSettingsPageMethod()
        {
            _viewModelLocator.NavigationService.Navigate(MyConstants.SETTINGS_VIEW_MODEL_FULL_NAME);
        }
    }
}
