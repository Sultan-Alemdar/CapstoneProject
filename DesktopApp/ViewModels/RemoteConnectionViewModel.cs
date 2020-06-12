using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using DesktopApp.Constants;
using DesktopApp.Core.Models;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PeerConnectionClientOperators.Signalling;
using WebRTCAdapter.Adapters;
using Windows.UI.Core;
using Windows.UI.Popups;
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

        public ListView _fileListView { get; set; }
        private StackPanel _pastInteractionStackPanel;
        public StackPanel PastInteractionStackPanel { get => _pastInteractionStackPanel; set => Set<StackPanel>("PastInteractionStackPanel", ref this._pastInteractionStackPanel, value); }

        private ObservableCollection<FileModel> _allFilesOnInterfaceCollection = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> AllFilesOnInterfaceCollection { get => _allFilesOnInterfaceCollection; set => Set<ObservableCollection<FileModel>>("AllFilesOnInterfaceCollection", ref this._allFilesOnInterfaceCollection, value); }

        private ObservableCollection<MessageModel> _allMessagesOnInterfaceCollection = new ObservableCollection<MessageModel>();
        public ObservableCollection<MessageModel> AllMessagesOnInterfaceCollection { get => _allMessagesOnInterfaceCollection; set => Set<ObservableCollection<MessageModel>>("AllMessagesOnInterfaceCollection", ref this._allMessagesOnInterfaceCollection, value); }

        private readonly RelayCommand _goBackCommand;

        public AdapterViewModel AdapterViewModel => AdapterViewModel.Instance;

        public RelayCommand GoBackCommand => _goBackCommand;

        public bool GoBackButtonVisibility { get => _goBackButtonVisibility; set => Set<bool>("GoBackButtonVisibility", ref this._goBackButtonVisibility, value); }

        private bool _goBackButtonVisibility = false;


        public RemoteConnectionViewModel()
        {
            PastInteractionStackPanel = new StackPanel();


          

            _offerCommand = new GalaSoft.MvvmLight.Command.RelayCommand(Offer, OfferCanExecute);

            _sendCommand = new GalaSoft.MvvmLight.Command.RelayCommand(Send, SendCanExecute);
            _goBackCommand = new GalaSoft.MvvmLight.Command.RelayCommand(GoBackExecute);
            _acceptCommand = new GalaSoft.MvvmLight.Command.RelayCommand<string>(Accept, AcceptCanExecute);
            _cancelCommand = new GalaSoft.MvvmLight.Command.RelayCommand<string>(CancelExecute);
            _openFileDirectoryCommand = new GalaSoft.MvvmLight.Command.RelayCommand<string>(OpenFileDirectoryExecute);
            _openFileCommand = new GalaSoft.MvvmLight.Command.RelayCommand<string>(OpenFileExecuteAsync);
            _goBackButtonVisibility = false;
            Conductor.Instance.OnSafe += Instance_OnSafe;
            Conductor.Instance.OnDataChanelWasCreated += Instance_OnDataChanelWasCreated;

            if (Conductor.Instance.MessageChannel != null)
            {
                Conductor.Instance.MessageChannel.OnBufferedAmountLow += MessageChannel_OnBufferedAmountLow;
                Conductor.Instance.MessageChannel.OnOpen += MessageChannel_OnOpen;
                Conductor.Instance.MessageChannel.OnClose += MessageChannel_OnClose;
                Conductor.Instance.MessageChannel.OnError += MessageChannel_OnError;
                Conductor.Instance.MessageChannel.OnMessage += MessageChannel_OnMessage; ;

                Conductor.Instance.FileChannel.BufferedAmountLowThreshold = MyConstants.BUFFER_THRESHOLD;
                Conductor.Instance.FileChannel.OnBufferedAmountLow += FileChannel_OnBufferedAmountLow; ;
                Conductor.Instance.FileChannel.OnOpen += FileChannel_OnOpen; ;
                Conductor.Instance.FileChannel.OnClose += FileChannel_OnClose; ;
                Conductor.Instance.FileChannel.OnError += FileChannel_OnError; ;
                Conductor.Instance.FileChannel.OnMessage += FileChannel_OnMessage;
            }

        }
        private async void GoBackExecute()
        {
            await RunOnUI(CoreDispatcherPriority.High, async () =>
            {

                if (ViewModelLocator.Current.NavigationService.CanGoBack)
                {
                    AllFilesOnInterfaceCollection = new ObservableCollection<FileModel>();
                    AllMessagesOnInterfaceCollection = new ObservableCollection<MessageModel>();
                    if (_allMessagesDictionary != null)
                    {
                        if (_allMessagesDictionary.Count > 0)
                            AllMessagesDictionary.Clear();
                    }

                    if (_allStoregeFilesDictionary.Count > 0)
                    {
                        var e = _allStoregeFilesDictionary.GetEnumerator();
                        while (e.MoveNext())
                        {
                            e.Dispose();
                        }
                        _allStoregeFilesDictionary = new Dictionary<string, Windows.Storage.StorageFile>();
                    }
                    ViewModelLocator.Current.NavigationService.GoBack();
                }

            });

        }
        private void Instance_OnSafe()
        {
            Task clear = Task.Run(() =>
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = new CancellationTokenSource();
                }
                if (_downloadCancellationTokenSource != null)
                {
                    _downloadCancellationTokenSource.Cancel();
                    _downloadCancellationTokenSource = new CancellationTokenSource();
                }
                if (_allStreamsDictionary.Count > 0)
                {
                    var se = _allStreamsDictionary.GetEnumerator();
                    while (se.MoveNext())
                    {
                        se.Dispose();
                    }

                    _allStreamsDictionary = new Dictionary<string, System.IO.Stream>();
                }


                _taskQueue = new List<FileModel>();
                _downloadStream = null;
                _downloadedSize = 0;
                _uploadSize = 0;
                _uploading = false;
                _state = MachineState.Idle;
                _timer = null;
                _bufferQueue = new System.Collections.Concurrent.ConcurrentQueue<byte[]>();
                Task update = RunOnUI(CoreDispatcherPriority.High, () =>
                {
                    GoBackButtonVisibility = true;
                    var messageDialog = new MessageDialog("Peer connection has been closed. If you want, you can walk around or go back the main page", "Peer Connection Is Over!");
                    var show = messageDialog.ShowAsync();
                });


            });

        }

        private void Instance_OnDataChanelWasCreated()
        {
            Conductor.Instance.MessageChannel.OnBufferedAmountLow += MessageChannel_OnBufferedAmountLow;
            Conductor.Instance.MessageChannel.OnOpen += MessageChannel_OnOpen;
            Conductor.Instance.MessageChannel.OnClose += MessageChannel_OnClose;
            Conductor.Instance.MessageChannel.OnError += MessageChannel_OnError;
            Conductor.Instance.MessageChannel.OnMessage += MessageChannel_OnMessage; ;

            Conductor.Instance.FileChannel.BufferedAmountLowThreshold = 6 * 1024 * 1024;
            Conductor.Instance.FileChannel.OnBufferedAmountLow += FileChannel_OnBufferedAmountLow; ;
            Conductor.Instance.FileChannel.OnOpen += FileChannel_OnOpen; ;
            Conductor.Instance.FileChannel.OnClose += FileChannel_OnClose; ;
            Conductor.Instance.FileChannel.OnError += FileChannel_OnError; ;
            Conductor.Instance.FileChannel.OnMessage += FileChannel_OnMessage;


        }
    }
}
