using System;

using DesktopApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DesktopApp.Views
{
    public sealed partial class OperationsPage : Page
    {
        private OperationsViewModel ViewModel
        {
            get { return ViewModelLocator.Current.OperationsViewModel; }
        }

        public OperationsPage()
        {
            InitializeComponent();
        }
    }
}
