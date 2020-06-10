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
            string id = (string)((Button)sender).Tag;
            ViewModel.AllMessagesDictionary.TryGetValue(id, out Core.Models.MessageModel messageModel);
            var file = messageModel.File;
            if (file.FileState != Core.Models.FileModel.EnumFileState.Offered)
                ViewModel._fileListView.ScrollIntoView(file);

            if (ViewModel.AcceptCommand.CanExecute(id))
            {
                ViewModel.AcceptCommand.Execute(id);
            }

        }
    }
}
