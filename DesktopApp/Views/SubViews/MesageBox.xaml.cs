using DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DesktopApp.Views.SubViews
{
    public sealed partial class MesageBox : UserControl
    {
        public RemoteConnectionViewModel ViewModel => this.DataContext as RemoteConnectionViewModel;
        public MesageBox()
        {
            this.InitializeComponent();
        }

        private void SubmitedReceivedMessageBodyFile_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AcceptCommand.CanExecute(((Button)sender).Tag))
            {
                ViewModel.AcceptCommand.Execute(((Button)sender).Tag);
            }
        }
    }
}
