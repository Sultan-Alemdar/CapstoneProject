using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DesktopApp.Core.Models;
using DesktopApp.Old.Core.Models;
using GalaSoft.MvvmLight;
using WebRTCAdapter.Adapters;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace DesktopApp.ViewModels
{
    public sealed partial class RemoteConnectionViewModel : ViewModelBase
    {
        public System.Type TypeOfEnumExist => typeof(MessageModel.EnumIsExist);
        public System.Type TypeOfEnumStatus => typeof(FileModel.EnumStatus);

    
        private StackPanel _pastInteractionStackPanel;
        public StackPanel PastInteractionStackPanel { get => _pastInteractionStackPanel; set => Set<StackPanel>("PastInteractionStackPanel", ref this._pastInteractionStackPanel, value); }

        private ObservableCollection<FileModel> _fileItems = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> FileItems { get => _fileItems; set => Set<ObservableCollection<FileModel>>("FileItems", ref this._fileItems, value); }

        private ObservableCollection<MessageModel> _messageItems = new ObservableCollection<MessageModel>();
        public ObservableCollection<MessageModel> MessageItems { get => _messageItems; set => Set<ObservableCollection<MessageModel>>("MessageItems", ref this._messageItems, value); }

        public AdapterViewModel AdapterViewModel => AdapterViewModel.Instance;

        public RemoteConnectionViewModel()
        {
            PastInteractionStackPanel = new StackPanel();
         
    
            _disconnectFromPeerCommand = new GalaSoft.MvvmLight.Command.RelayCommand(DiscconectFromPeer, DiscconectFromCanExecute);
        }

     
    }
}
