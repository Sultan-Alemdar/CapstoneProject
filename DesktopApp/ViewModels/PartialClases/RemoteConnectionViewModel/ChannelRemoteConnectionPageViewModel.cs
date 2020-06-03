using DesktopApp.Core.Models;
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

namespace DesktopApp.ViewModels
{
    public sealed partial class RemoteConnectionViewModel : ViewModelBase
    {
        private string _messageText;
        public string MessageText { get => _messageText; set => Set<string>("MessageText", ref _messageText, value); }
        private readonly RelayCommand _sendCommand;
        private readonly RelayCommand _offerCommand;
        private readonly RelayCommand<string> _acceptCommand;
        private CoreDispatcher _coreDispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
        private static object _lockObject = new object();

        private enum MachineState
        {
            Idle = 0,
            InInteraction = 1,

        }
        private MemoryStream _downloadStream;
        private MachineState _state = MachineState.Idle;


        public RelayCommand<string> AcceptCommand => _acceptCommand;
        public RelayCommand OfferCommand => _offerCommand;
        public RelayCommand SendCommand => _sendCommand;




        private List<FileModel> _taskQueue = new List<FileModel>();

        private Dictionary<string, MessageModel> _allMessagesDictionary = new Dictionary<string, MessageModel>();
        private Dictionary<string, StorageFile> _allStoregeFilesDictionary = new Dictionary<string, StorageFile>();
        //private Dictionary<string, StorageFile> _allAcceptedFilesDictionary= new Dictionary<string, StorageFile>();



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

            FileModel fileModel = new FileModel(id, storageFile.Name, storageFile.FileType.Substring(1), fileprops.Size, storageFile.DisplayName, storageFile.DisplayType);
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
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add(messageModel.File.FileDisplayName, new List<string>() { "." + messageModel.File.FileType });
            savePicker.SuggestedFileName = messageModel.File.FileName;
            Windows.Storage.StorageFile storageFile = await savePicker.PickSaveFileAsync();
            if (storageFile == null)
            {
                Debug.WriteLine("[Info] ChannelRemoteControlViewModel : Accept operation has being canceled :");
                return;
            }
            messageModel.File.SetAcceptedStateConfig();
            await UpdateAllDataStrucuresAndInterface(messageModel.Id, null, messageModel.File, storageFile);
            await SendFileNotifyMessageAsync(TreatmentMessageModel.GetAcceptedType(messageModel.Id));
            _downloadStream = new MemoryStream((int)messageModel.File.TotalSize);
        }

        private bool AcceptCanExecute(string id)
        {
            return true;
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
            CancellationToken cancellationToken = new CancellationToken();
            _state = MachineState.InInteraction;
            try
            {

                if (!(Conductor.Instance.FileChannel.ReadyState == Org.WebRtc.RTCDataChannelState.Open))
                    Debug.WriteLine("[Warning] ChannelRemoteConnectionPageViewModel : File channel is not open.");

                if (!_allStoregeFilesDictionary.TryGetValue(fileModel.Id, out StorageFile storageFile))
                    return;
                _taskQueue.Remove(fileModel);//delete from task queue;
                fileModel.SetStartedStateConfig();

                //upload
                await SendFileNotifyMessageAsync(TreatmentMessageModel.GetStartType(fileModel.Id));
                using (var stream = await storageFile.OpenStreamForReadAsync())
                {
                    if (fileModel.TotalSize <= (ulong)MyConstants.MAX_ONE_CHUNK_SIZE)
                    {//küçük dosya
                        byte[] buffer = new byte[fileModel.TotalSize];
                        await stream.ReadAsync(buffer, 0, (int)fileModel.TotalSize);
                        Conductor.Instance.FileChannel.Send(buffer);
                    }
                    else
                    { //büyük dosya

                        long total = (long)fileModel.TotalSize;
                        int bufferSize = MyConstants.CHUNK_SIZE;
                        while (stream.Position < total)
                        {
                            if (stream.Position >= total)
                                break;
                            if (CheckCancellationRequested(cancellationToken) || !fileModel.IsStarted)
                            {
                                Debug.WriteLine("[Information] ChannelRemoteConnectionViewModel : File upload operation was canceled. File state is " + fileModel.FileState);
                                if (await HandleCanceledTask(fileModel))
                                {
                                    return;
                                }
                            }
                            byte[] buffer = new byte[bufferSize];
                            await stream.ReadAsync(buffer, 0, bufferSize);
                            if (stream.Position + bufferSize > total)
                                bufferSize = (int)(total - stream.Position);
                            Conductor.Instance.FileChannel.Send(buffer);

                        }
                    }
                }
                _state = MachineState.Idle;
                await SendFileNotifyMessageAsync(TreatmentMessageModel.GetEndType(fileModel.Id));
                fileModel.SetEndedStateConfig();
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
        private async void MessageChannel_OnMessage(Org.WebRtc.IMessageEvent Event)
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

        private async void FileChannel_OnMessage(Org.WebRtc.IMessageEvent Event)
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

                        break;
                    case TreatmentMessageModel.EnumMessageType.End:
                        if (!_allMessagesDictionary.TryGetValue(id, out MessageModel endedMessage))
                        {
                            await SendErrorMessageAsync(id, "");
                            return;
                        }
                        _taskQueue.Remove(endedMessage.File);

                        if (!_allStoregeFilesDictionary.TryGetValue(id, out StorageFile storageFile))
                        {

                        }
                        if (storageFile != null)
                        {
                            CachedFileManager.DeferUpdates(storageFile);
                            // write to file
                            await FileIO.WriteBytesAsync(storageFile, _downloadStream.GetBuffer());
                            FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(storageFile);
                            if (status == FileUpdateStatus.Complete)
                            {
                                Debug.WriteLine("[Info] ChannelRemoteConnectionPageViewModel : File {0} was saved.", storageFile.Name);
                                endedMessage.File.SetEndedStateConfig();
                            }
                            else
                            {
                                Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : File {0} could not being saved.", storageFile.Name);
                                endedMessage.File.SetFailureStateConfig();
                            }
                        }
                        _allStoregeFilesDictionary.Remove(id);

                        _downloadStream.Dispose();

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
                    case TreatmentMessageModel.EnumMessageType.Waiting:
                        break;
                }
            }
            else
            {
                GetChunk(Event.Binary);
            }

        }
        private void GetChunk(byte[] chunk)
        {
            try
            {
                _downloadStream.Write(chunk, 0, chunk.Length);
            }
            catch (Exception e)
            {
                Debug.WriteLine("[Error] ChannelRemoteConnection : Error was occured in downloading proccess :" + e.Message);
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

        }

        private void MessageChannel_OnClose()
        {

        }

        private void MessageChannel_OnOpen()
        {


        }

        private void MessageChannel_OnBufferedAmountLow()
        {

        }
        private void FileChannel_OnError(Org.WebRtc.IRTCError Event)
        {

        }

        private void FileChannel_OnClose()
        {

        }

        private void FileChannel_OnOpen()
        {

        }

        private void FileChannel_OnBufferedAmountLow()
        {

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
