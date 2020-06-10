using DesktopApp.Core.Models;
using DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class FileBox : UserControl
    {
        public RemoteConnectionViewModel ViewModel => this.DataContext as RemoteConnectionViewModel;
        public FileBox()
        {
            this.InitializeComponent();
            Loaded += FileBox_Loaded;
        }

        private void FileBox_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel._fileListView = FileListView;
        }

        private void SubmitedReceivedMessageBodyFile_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var fileModel = (FileModel)button.DataContext;
            if (fileModel.IsAccepted || fileModel.IsStarted)
            {
                ViewModel.CancelCommand.Execute(fileModel.Id);
            }
            else
                ViewModel.OpenFileCommand.Execute(fileModel.Id);


        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var fileModel = (FileModel)button.DataContext;
            ViewModel.OpenFileDirectoryCommand.Execute(fileModel.Id);
        }
    }
}
