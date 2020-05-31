using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DesktopApp.Core.Models;

using GalaSoft.MvvmLight;
using PeerConnectionClientOperators.Signalling;
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
        public System.Type TypeOfEnumStatus => typeof(FileModel.EnumProccesStatus);
        public System.Type TypeOfMessageEnumEvent => typeof(MessageModel.EnumEvent);


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

            _addCommand = new GalaSoft.MvvmLight.Command.RelayCommand(Add, AddCanExecute);

            Conductor.Instance.MessageChannel.OnBufferedAmountLow += MessageChannel_OnBufferedAmountLow;
            Conductor.Instance.MessageChannel.OnOpen += MessageChannel_OnOpen;
            Conductor.Instance.MessageChannel.OnClose += MessageChannel_OnClose;
            Conductor.Instance.MessageChannel.OnError += MessageChannel_OnError;
            Conductor.Instance.MessageChannel.OnMessage += MessageChannel_OnMessage; ;

            Conductor.Instance.FileChannel.OnBufferedAmountLow += FileChannel_OnBufferedAmountLow; ;
            Conductor.Instance.FileChannel.OnOpen += FileChannel_OnOpen; ;
            Conductor.Instance.FileChannel.OnClose += FileChannel_OnClose; ;
            Conductor.Instance.FileChannel.OnError += FileChannel_OnError; ;
            Conductor.Instance.FileChannel.OnMessage += FileChannel_OnMessage;

        }

        private void FileChannel_OnMessage(Org.WebRtc.IMessageEvent Event)
        {
            throw new NotImplementedException();
        }

        private void FileChannel_OnError(Org.WebRtc.IRTCError Event)
        {
            throw new NotImplementedException();
        }

        private void FileChannel_OnClose()
        {
            throw new NotImplementedException();
        }

        private void FileChannel_OnOpen()
        {
            throw new NotImplementedException();
        }

        private void FileChannel_OnBufferedAmountLow()
        {
            throw new NotImplementedException();
        }

        
    }
}
