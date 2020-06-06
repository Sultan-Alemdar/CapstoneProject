﻿using DesktopApp.Core.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PeerConnectionClientOperators.Signalling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using DesktopApp.Constants;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using Windows.Storage.Streams;
using System.IO;
using System.Threading;
using Windows.Storage.Provider;
using Windows.UI.Core;
using System.ComponentModel.DataAnnotations;

namespace DesktopApp.ViewModels
{
    public sealed partial class RemoteConnectionViewModel : ViewModelBase
    {
        private System.Timers.Timer _timer;
        private string _messageText;
        public string MessageText { get => _messageText; set => Set<string>("MessageText", ref _messageText, value); }
        private readonly RelayCommand _sendCommand;
        private readonly RelayCommand _offerCommand;
        private RelayCommand<string> _acceptCommand;
        private CoreDispatcher _coreDispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
        private static object _lockObject = new object();

        private enum MachineState
        {
            Idle = 0,
            InInteraction = 1,

        }

        private MachineState _state = MachineState.Idle;


        public RelayCommand<string> AcceptCommand
        {
            get => _acceptCommand; set => Set<RelayCommand<string>>("AcceptCommand", ref _acceptCommand, value);
        }
        public RelayCommand OfferCommand => _offerCommand;
        public RelayCommand SendCommand => _sendCommand;



        private Stream _downloadStream;
        private List<FileModel> _taskQueue = new List<FileModel>();

        private Dictionary<string, MessageModel> _allMessagesDictionary = new Dictionary<string, MessageModel>();
        private Dictionary<string, StorageFile> _allStoregeFilesDictionary = new Dictionary<string, StorageFile>();

        private Dictionary<string, Stream> _allUploadStreamsDictionary = new Dictionary<string, Stream>();
        private Dictionary<string, Stream> _allDownloadStreamsDictionary = new Dictionary<string, Stream>();



        private async Task SendSeenMessageAsync(string id)
        {
            await Task.Run(() =>
            {
                SendMessage(TreatmentMessageModel.GetSeenOfPlainTextOrOfferMessageType(id));
            });

        }

        private void SendMessage(TreatmentMessageModel treatmentMessageModel)
        {
            if (!(Conductor.Instance.MessageChannel.ReadyState == Org.WebRtc.RTCDataChannelState.Open))
            {
                Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Channel was not open, Message could not bening sended");
                return;
            }
            string strChannelMessageModel = JsonConvert.SerializeObject(treatmentMessageModel);
            Conductor.Instance.MessageChannel.Send(strChannelMessageModel);
        }



        private async void Send()
        {
            await SendPlainTextTypeMessageAsync();
            await RunOnUI(CoreDispatcherPriority.High, () =>
              {
                  MessageText = "";
              });
        }
        private async Task SendPlainTextTypeMessageAsync()
        {
            await Task.Run(async () =>
            {
                var id = CreateId(out int myId, out int messageId);
                MessageModel messageModel = null;
                messageModel = new MessageModel(id, DateTime.Now.ToString("hh:mm:ss"), MessageModel.EnumEvent.Send, MessageText);
                await UpdateAllDataStrucuresAndInterface(id, messageModel, null, null);
                SendMessage(TreatmentMessageModel.GetPlainTextType(messageModel));
            });
        }
        private bool SendCanExecute()
        {
            return true;
        }

        private async void Offer()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".doc");
            picker.FileTypeFilter.Add(".docx");
            picker.FileTypeFilter.Add(".7z");
            picker.FileTypeFilter.Add(".");

            var files = await picker.PickMultipleFilesAsync();
            if (files.Count > 0)
            {
                foreach (Windows.Storage.StorageFile file in files)
                {
                    await SendOfferTypeMessageAsync(file);
                }
            }
        }
        private async Task SendOfferTypeMessageAsync(StorageFile storageFile)
        {
            var id = CreateId(out int myId, out int messageId);
            var fileprops = await storageFile.GetBasicPropertiesAsync();

            FileModel fileModel = new FileModel(id, storageFile.Name, storageFile.FileType.Substring(1), (long)fileprops.Size, storageFile.DisplayName, storageFile.DisplayType);
            fileModel.SetOfferedStateConfig();
            MessageModel messageModel = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Send, null, fileModel);
            await UpdateAllDataStrucuresAndInterface(id, messageModel, null, storageFile);
            TreatmentMessageModel treatmentMessageModel = TreatmentMessageModel.GetFileOfferType(messageModel);
            SendMessage(treatmentMessageModel);

        }

        private bool OfferCanExecute()
        {
            return true;
        }

        private async void Accept(string id)
        {
            if (!_allMessagesDictionary.TryGetValue(id, out MessageModel messageModel))
            {
                await SendErrorMessageAsync(id, "");
                return;
            }
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            await RunOnUI(CoreDispatcherPriority.High, () =>
            {
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add(messageModel.File.FileDisplayName, new List<string>() { "." + messageModel.File.FileType });
                savePicker.SuggestedFileName = messageModel.File.FileName;
            });

            Windows.Storage.StorageFile storageFile = await savePicker.PickSaveFileAsync();
            if (storageFile == null)
            {
                Debug.WriteLine("[Info] ChannelRemoteControlViewModel : Accept operation has being canceled :");
                return;
            }
            messageModel.File.SetAcceptedStateConfig();
            _taskQueue.Add(messageModel.File);
            await UpdateAllDataStrucuresAndInterface(messageModel.Id, null, messageModel.File, storageFile);
            await SendFileNotifyMessageAsync(TreatmentMessageModel.GetAcceptedType(messageModel.Id));

        }

        private bool AcceptCanExecute(string id)
        {
            if (_allMessagesDictionary.TryGetValue(id, out MessageModel messageModel))
            {
                if (messageModel.File.ProccesStatus == FileModel.EnumProccesStatus.Waiting && messageModel.Event == MessageModel.EnumEvent.Received)
                    return true;
            }
            return false;
        }



        private async Task UpdateAllDataStrucuresAndInterface(string id, MessageModel messageModel, FileModel fileModel, StorageFile storageFile)
        {
            await RunOnUI(CoreDispatcherPriority.High, () =>
            {
                if (fileModel != null)
                    AllFilesOnInterfaceCollection.Add(fileModel);
                if (messageModel != null)
                    AllMessagesOnInterfaceCollection.Add(messageModel);
            });
            await Task.Run(() =>
            {
                if (storageFile != null)
                    _allStoregeFilesDictionary.Add(id, storageFile);

                if (messageModel != null)
                    _allMessagesDictionary.Add(id, messageModel);
            });
        }

        private async Task StartUpload()
        {
            FileModel fileModel = _taskQueue.First();
            if (fileModel == null)
            {//it means queue was ended.
                _state = MachineState.Idle;
                return;
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            CancellationToken cancellationToken = cancellationTokenSource.Token;
            _state = MachineState.InInteraction;
            try
            {

                if (!(Conductor.Instance.FileChannel.ReadyState == Org.WebRtc.RTCDataChannelState.Open))
                    Debug.WriteLine("[Warning] ChannelRemoteConnectionPageViewModel : File channel is not open.");

                if (!_allStoregeFilesDictionary.TryGetValue(fileModel.Id, out StorageFile storageFile))
                    return;

                fileModel.SetStartedStateConfig();

                //upload
                await SendFileNotifyMessageAsync(TreatmentMessageModel.GetStartType(fileModel.Id));
                var stream = await storageFile.OpenStreamForReadAsync();

                _allUploadStreamsDictionary.Add(fileModel.Id, stream);

                if (fileModel.TotalSize <= MyConstants.MAX_ONE_CHUNK_SIZE)
                {//küçük dosya
                    byte[] buffer = new byte[fileModel.TotalSize];
                    await stream.ReadAsync(buffer, 0, (int)fileModel.TotalSize);
                    Conductor.Instance.FileChannel.Send(buffer);
                    await ReleaseFileResources(fileModel, stream);
                }
                else
                { //büyük dosya

                    long total = (long)fileModel.TotalSize;
                    int bufferSize = MyConstants.CHUNK_SIZE;

                    await UploadFile(cancellationTokenSource, fileModel, stream);

                }
                #region CatchSide

            }
            catch (ArgumentOutOfRangeException ou)
            {
                Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Stream out of range error  : " + ou.Message);
            }
            catch (OperationCanceledException o)
            {
                Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Cancelation of task was wanted : " + o.Message);
            }
            catch (ArgumentException a)
            {
                Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Getting from dic was ended with error or stream caused this: " + a.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Error was occurred : " + e.Message);
            }

            #endregion
        }
        private async Task ReleaseFileResources(FileModel fileModel, Stream stream)
        {
            _taskQueue.Remove(fileModel);//delete from task queue;
            _state = MachineState.Idle;
            _allUploadStreamsDictionary.Remove(fileModel.Id);
            await RunOnUI(CoreDispatcherPriority.High, async () =>
            {
                fileModel.ProgressedSize = stream.Position;
                if (fileModel.IsCanceled)
                {
                    await SendFileNotifyMessageAsync(TreatmentMessageModel.GetFileCanceledType(fileModel.Id));
                    fileModel.SetCanceledStateConfig();
                }
                else
                {
                    await SendFileNotifyMessageAsync(TreatmentMessageModel.GetEndType(fileModel.Id));
                    fileModel.SetEndedStateConfig();
                }
            });
            stream.Dispose();
        }
        private static object _uploadLock = new object();
        private async Task<bool> UploadFile(CancellationTokenSource cancellationTokenSource, FileModel fileModel, Stream stream)
        {
            var total = fileModel.TotalSize;
            var bufferSize = MyConstants.CHUNK_SIZE;
            _timer = new System.Timers.Timer(1000);
            _timer.AutoReset = true;
            _timer.Elapsed += async (sender, e) =>
            {
                await RunOnUI(CoreDispatcherPriority.High, () =>
                {
                    fileModel.ActionSpeed = stream.Position - fileModel.ProgressedSize;
                    fileModel.ProgressedSize = stream.Position;

                });
            };
            _timer.Enabled = true;
            return await Task<bool>.Run(async () =>
            {
                var r = true;
                while (stream.Position < total)
                {
                    if (stream.Position >= total)
                        break;
                    if (!fileModel.IsStarted)
                    {
                        Debug.WriteLine("[Information] ChannelRemoteConnectionViewModel : File upload operation was canceled. File state is " + fileModel.FileState);
                        //fileModel.SetCanceledStateConfig();

                        r = false;
                    }

                    byte[] buffer = new byte[bufferSize];
                    await stream.ReadAsync(buffer, 0, bufferSize);
                    if (stream.Position + bufferSize > total)
                        bufferSize = (int)(total - stream.Position);
                    Conductor.Instance.FileChannel.Send(buffer);

                    if ((Conductor.Instance.FileChannel.BufferedAmount + (ulong)bufferSize) > 15 * 1024 * 1024)
                    {
                        Debug.WriteLine("[Information] ChannelRemoteConnectionViewModel : FileChannel buffer is full, Operation is going to sleep until channel will be ready :" + fileModel.FileName);
                        //cancellationTokenSource.Cancel();

                        r = false;
                    }
                }

                _timer.Enabled = false;
                await RunOnUI(CoreDispatcherPriority.High, () =>
                {
                    fileModel.ActionSpeed = -1;
                });
                await ReleaseFileResources(fileModel, stream);
                return r ? true : false;
            });
        }




        private async void MessageChannel_OnMessage(Org.WebRtc.IMessageEvent Event)
        {
            try
            {

                TreatmentMessageModel treatmentMessageModel = JsonConvert.DeserializeObject<TreatmentMessageModel>(Event.Text);
                MessageModel messageModel = null;
                switch (treatmentMessageModel.MessageType)
                {
                    case TreatmentMessageModel.EnumMessageType.PlainText:
                        messageModel = treatmentMessageModel.MessageModel;
                        messageModel.SwitchTreatment();
                        await UpdateAllDataStrucuresAndInterface(messageModel.Id, messageModel, null, null);
                        await SendSeenMessageAsync(messageModel.Id);
                        break;
                    case TreatmentMessageModel.EnumMessageType.Offer:
                        messageModel = treatmentMessageModel.MessageModel;
                        messageModel.SwitchTreatment();
                        await UpdateAllDataStrucuresAndInterface(messageModel.Id, messageModel, null, null);
                        await SendSeenMessageAsync(messageModel.Id);
                        break;
                    case TreatmentMessageModel.EnumMessageType.SeenOfPlainTextOrOfferMessage:
                        if (_allMessagesDictionary.TryGetValue(treatmentMessageModel.Id, out MessageModel messageForBeen))
                        {
                            messageForBeen.Seen = MessageModel.EnumSeen.Yes;
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception e)
            {

                Debug.WriteLine("[Error] ChannelRemoteConnectionViewModel : On Message channel event:" + e.Message);
            }

        }

        private async void FileChannel_OnMessage(Org.WebRtc.IMessageEvent Event)
        {

            try
            {
                if (Event.Text != "")
                {

                    TreatmentMessageModel treatmentMessageModel = JsonConvert.DeserializeObject<TreatmentMessageModel>(Event.Text);

                    string id = treatmentMessageModel.Id;

                    switch (treatmentMessageModel.MessageType)
                    {
                        case TreatmentMessageModel.EnumMessageType.Accepted:

                            if (_allMessagesDictionary.TryGetValue(id, out MessageModel message))
                            {
                                FileModel fileModel = message.File;
                                fileModel.SetAcceptedStateConfig();
                                _taskQueue.Add(message.File);
                                await UpdateAllDataStrucuresAndInterface(fileModel.Id, null, fileModel, null);
                                lock (_lockObject)
                                {
                                    if (_state != MachineState.Idle)
                                        break;
                                }
                                await StartUpload();
                                return;
                            }
                            await SendErrorMessageAsync(id, "");
                            break;
                        case TreatmentMessageModel.EnumMessageType.Start:
                            if (!_allMessagesDictionary.TryGetValue(id, out MessageModel startedMessage))
                            {
                                await SendErrorMessageAsync(id, "");
                                return;
                            }
                            startedMessage.File.SetStartedStateConfig();
                            _state = MachineState.InInteraction;
                            _allStoregeFilesDictionary.TryGetValue(id, out StorageFile currentStorageFile);
                            _downloadStream = await currentStorageFile.OpenStreamForWriteAsync();

                            _timer = new System.Timers.Timer(1000);
                            var startedFileModel = startedMessage.File;
                            _timer.Enabled = true;
                            _timer.AutoReset = true;
                            _timer.Elapsed += async (sender, e) =>
                            {
                                await RunOnUI(CoreDispatcherPriority.High, () =>
                                {
                                    startedFileModel.ActionSpeed = _downloadStream.Position - startedFileModel.ProgressedSize;
                                    startedFileModel.ProgressedSize = _downloadStream.Position;
                                    startedFileModel.ShowPercent();
                                });
                            };
                            break;
                        case TreatmentMessageModel.EnumMessageType.End:
                            _timer.Enabled = false;

                            if (!_allMessagesDictionary.TryGetValue(id, out MessageModel endedMessage))
                            {
                                await SendErrorMessageAsync(id, "");
                                return;
                            }
                            var file = endedMessage.File;
                            _taskQueue.Remove(file);
                            file.ProgressedSize -= _downloadStream.Position;
                            if (!_allStoregeFilesDictionary.TryGetValue(id, out StorageFile storageFile))
                            {
                                Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Message could not find to realize saving file operation. File Id :" + id);
                            }
                            _downloadStream.Dispose();
                            file.SetEndedStateConfig();
                            _state = MachineState.Idle;
                            _allStoregeFilesDictionary.Remove(id);
                            if (_taskQueue.Count > 0)
                            {
                                FileModel next = _taskQueue.First<FileModel>();

                                if (next.Event == FileModel.EnumEvent.Upload)
                                {
                                    _state = MachineState.InInteraction;
                                    await SendFileNotifyMessageAsync(TreatmentMessageModel.GetStartType(next.Id));
                                    next.SetStartedStateConfig();
                                    await StartUpload();
                                }
                                else
                                    await SendFileNotifyMessageAsync(TreatmentMessageModel.GetNextType(next.Id));

                            }
                            break;

                        case TreatmentMessageModel.EnumMessageType.Canceled:
                            if (!_allMessagesDictionary.TryGetValue(id, out MessageModel canceledMessage))
                            {
                                await SendErrorMessageAsync(id, "");
                                return;
                            }
                            canceledMessage.File.SetCanceledStateConfig();
                            _taskQueue.Remove(canceledMessage.File);
                            //TODO: clear stream. if it is necessary.
                            break;
                        case TreatmentMessageModel.EnumMessageType.Next:
                            var requested = _taskQueue.First<FileModel>();
                            _state = MachineState.InInteraction;
                            await SendFileNotifyMessageAsync(TreatmentMessageModel.GetStartType(requested.Id));
                            requested.SetStartedStateConfig();
                            await StartUpload();
                            break;
                    }
                }
                else
                {

                    await _downloadStream.WriteAsync(Event.Binary, 0, Event.Binary.Length);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Error was occcured in FileChannel on message :" +e.Message);
            }
        }




        private async Task<bool> HandleCanceledTask(FileModel fileModel)
        {
            fileModel.SetCanceledStateConfig();
            _state = MachineState.Idle;
            await SendFileNotifyMessageAsync(TreatmentMessageModel.GetFailureType(fileModel.Id));
            return true;
        }

        private bool CheckCancellationRequested(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (cancellationToken.CanBeCanceled)
                {
                    Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Task is ending : ");
                    return true;
                }
            }
            return false;
        }
        private async Task SendFileNotifyMessageAsync(TreatmentMessageModel treatmentMessageModel)
        {
            await Task.Run(() =>
           {
               string strChannelMessageModel = JsonConvert.SerializeObject(treatmentMessageModel);
               Conductor.Instance.FileChannel.Send(strChannelMessageModel);
           });
        }
        private async Task SendErrorMessageAsync(string id, string text)
        {
            await SendFileNotifyMessageAsync(TreatmentMessageModel.GetErrorType(id, text));
            Debug.WriteLine("[Error] My FileChannel : " + text);
        }


        public string CreateId(out int myId, out int messageId)
        {
            StringBuilder builder = new StringBuilder();
            myId = ViewModelLocator.Current.OperationsViewModel.MyId;
            messageId = _allMessagesDictionary.Count();

            builder.Append(MyConstants.ID_START_TAG);
            builder.Append(myId.ToString());
            builder.Append(MyConstants.ID_END_TAG);
            builder.Append(MyConstants.MESSAGE_START_TAG);
            builder.Append(messageId.ToString());
            builder.Append(MyConstants.MESSAGE_END_TAG);

            return builder.ToString();
        }
        private void AddMessageItemsOnInterface(MessageModel messageModel)
        {
            AllMessagesOnInterfaceCollection.Add(messageModel);
        }

        private async Task RunOnUI(CoreDispatcherPriority priority, Action action)
        {
            await _coreDispatcher.RunAsync(priority, new DispatchedHandler(action));
        }
        #region Evenler

        private void MessageChannel_OnError(Org.WebRtc.IRTCError Event)
        {
            Debug.WriteLine("[Error] ChannelRemoteConnecctionPageViewModel : MessageChannel has got a error:" + Event.Message);
        }

        private void MessageChannel_OnClose()
        {
            Debug.WriteLine("[Info] ChannelRemoteConnecctionPageViewModel : MessageChannel has closed :");
        }

        private void MessageChannel_OnOpen()
        {

            Debug.WriteLine("[Info] ChannelRemoteConnecctionPageViewModel : MessageChannel has opened :");
        }

        private void MessageChannel_OnBufferedAmountLow()
        {
            Debug.WriteLine("[Info] ChannelRemoteConnecctionPageViewModel : MessageChannel is state of buffered amount low :");
        }
        private void FileChannel_OnError(Org.WebRtc.IRTCError Event)
        {
            Debug.WriteLine("[Error] ChannelRemoteConnecctionPageViewModel : FileChannel has got a error :" + Event.Message);
        }

        private void FileChannel_OnClose()
        {
            Debug.WriteLine("[Info] ChannelRemoteConnecctionPageViewModel : FileChannel has closed :");
        }

        private void FileChannel_OnOpen()
        {
            Debug.WriteLine("[Info] ChannelRemoteConnecctionPageViewModel : FileChannel has opened :");
        }

        private void FileChannel_OnBufferedAmountLow()
        {

            lock (_uploadLock)
            {
                if (_state == MachineState.Idle)
                    return;
                FileModel fileModel = _taskQueue.First();
                if (fileModel == null)
                {
                    return;

                }
                if (!fileModel.IsStarted)
                    return;

                if (!_allUploadStreamsDictionary.TryGetValue(fileModel.Id, out Stream stream))
                {
                    Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Stream could not find. File Id:  : " + fileModel.Id);
                    return;
                }


                UploadFile(null, fileModel, stream);
            }
        }


        #endregion



    }
}
#region ThumtoBitmap
//var asdq = await file.GetBasicPropertiesAsync();
//var asdqd = await file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView);
//var thumb = await file.GetScaledImageAsThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView);
//if (thumb != null)
//{
//    BitmapImage img = new BitmapImage();
//    await img.SetSourceAsync(thumb);
//    this.StorageItemThumbnail = img;
//}

#endregion