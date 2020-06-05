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
        public System.Type EnumProccesStatus => typeof(FileModel.EnumProccesStatus);
        public System.Type TypeOfMessageEnumEvent => typeof(MessageModel.EnumEvent);


        private StackPanel _pastInteractionStackPanel;
        public StackPanel PastInteractionStackPanel { get => _pastInteractionStackPanel; set => Set<StackPanel>("PastInteractionStackPanel", ref this._pastInteractionStackPanel, value); }

        private ObservableCollection<FileModel> _allFilesOnInterfaceCollection = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> AllFilesOnInterfaceCollection { get => _allFilesOnInterfaceCollection; set => Set<ObservableCollection<FileModel>>("AllFilesOnInterfaceCollection", ref this._allFilesOnInterfaceCollection, value); }

        private ObservableCollection<MessageModel> _allMessagesOnInterfaceCollection = new ObservableCollection<MessageModel>();
        public ObservableCollection<MessageModel> AllMessagesOnInterfaceCollection { get => _allMessagesOnInterfaceCollection; set => Set<ObservableCollection<MessageModel>>("AllMessagesOnInterfaceCollection", ref this._allMessagesOnInterfaceCollection, value); }

        public AdapterViewModel AdapterViewModel => AdapterViewModel.Instance;

        public RemoteConnectionViewModel()
        {
            PastInteractionStackPanel = new StackPanel();


            _disconnectFromPeerCommand = new GalaSoft.MvvmLight.Command.RelayCommand(DiscconectFromPeer, DiscconectFromPeerCanExecute);

            _offerCommand = new GalaSoft.MvvmLight.Command.RelayCommand(Offer, OfferCanExecute);

            _sendCommand = new GalaSoft.MvvmLight.Command.RelayCommand(Send, SendCanExecute);

            _acceptCommand = new GalaSoft.MvvmLight.Command.RelayCommand<string>(Accept, AcceptCanExecute);
            _downloadStream = new System.IO.MemoryStream();

            Conductor.Instance.OnDataChanelWasCreated += Instance_OnDataChanelWasCreated;
            if (Conductor.Instance.MessageChannel != null)
            {
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
            
        }

        private void Instance_OnDataChanelWasCreated()
        {
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
    }
}
