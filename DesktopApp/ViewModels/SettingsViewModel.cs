﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

using DesktopApp.Helpers;
using DesktopApp.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace DesktopApp.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public sealed partial class SettingsViewModel : ViewModelBase
    {
        private ViewModelLocator _viewModelLocator = ViewModelLocator.Current;
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        public RelayCommand BackCommand { get; set; }
        public RelayCommand ForwardCommand { get; set; }

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }


        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

        public SettingsViewModel()
        {

            BackCommand = new RelayCommand(GoBack, CanGoBack);
            ForwardCommand = new RelayCommand(GoForward, CanGoForward);
        }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            await Task.CompletedTask;
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
        public bool CanGoBack()
        {
            return _viewModelLocator.NavigationService.CanGoBack;
        }
        public bool CanGoForward()
        {
            return _viewModelLocator.NavigationService.CanGoForward;
        }
        public void GoBack()
        {
            _viewModelLocator.NavigationService.GoBack();
        }
        public void GoForward()
        {
            _viewModelLocator.NavigationService.GoForward();
        }
    }
}
