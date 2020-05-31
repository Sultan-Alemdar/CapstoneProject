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

namespace DesktopApp.ViewModels
{
    public sealed partial class RemoteConnectionViewModel : ViewModelBase
    {
        public string MessageText { get; set; }
        private readonly RelayCommand _sendCommand;
        private readonly RelayCommand _offerFileCommand;

        public RelayCommand OfferFileCommand => _offerFileCommand;
        public RelayCommand SendCommand => _sendCommand;


        Dictionary<string, MessageModel> _allMessagesDictionary = new Dictionary<string, MessageModel>();
        Dictionary<string, StorageFile> _allStoregeFilesDictionary = new Dictionary<string, StorageFile>();




        private async void SendNotifyMessageAsync(string id)
        {
            await Task.Run(() =>
            {
                SendMessage(TreatmentMessageModel.GetSeenOfPlainTextOrFileOfferMessageType(id));
            });
        }

        private void SendMessage(TreatmentMessageModel treatmentMessageModel)
        {
            string strChannelMessageModel = JsonConvert.SerializeObject(treatmentMessageModel);
            Conductor.Instance.MessageChannel.Send(strChannelMessageModel);
        }



        private void Send()
        {
            SendPlainTextTypeMessageAsync();
        }
        private async void SendPlainTextTypeMessageAsync()
        {
            await Task.Run(() =>
            {
                var id = CreateId(out int myId, out int messageId);
                MessageModel messageModel = null;
                messageModel = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Send, MessageText);
                UpdateAllDataStrucuresAndInterface(id, messageModel, null);
                SendMessage(TreatmentMessageModel.GetPlainTextType(messageModel));
            });
        }
        private bool SendCanExecute()
        {
            return true;
        }

        private async void OfferFile()
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
                    SendFileOfferTypeMessageAsync(file);
                }
            }
        }
        private async void SendFileOfferTypeMessageAsync(StorageFile storageFile)
        {
            await Task.Run(async () =>
            {
                var id = CreateId(out int myId, out int messageId);
                var fileprops = await storageFile.GetBasicPropertiesAsync();
                FileModel fileModel = new FileModel(id, storageFile.Name, storageFile.FileType.Substring(1), fileprops.Size);
                fileModel.SetOfferedStateConfig();
                MessageModel messageModel = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Send, null, fileModel);
                UpdateAllDataStrucuresAndInterface(id, messageModel, storageFile);
                TreatmentMessageModel treatmentMessageModel = TreatmentMessageModel.GetFileOfferType(messageModel);
                SendMessage(treatmentMessageModel);
            });
        }

        private bool OfferFileCanExecute()
        {
            return true;
        }


        private async void UpdateAllDataStrucuresAndInterface(string id, MessageModel messageModel, StorageFile storageFile)
        {
            await Task.Run(() =>
            {
                if (storageFile != null)
                {
                    _allStoregeFilesDictionary.Add(id, storageFile);
                }
                _allMessagesDictionary.Add(id, messageModel);
                AllMessagesOnInterfaceCollection.Add(messageModel);
            });
        }
        private async void MessageChannel_OnMessage(Org.WebRtc.IMessageEvent Event)
        {
            await Task.Run(() =>
             {
                 TreatmentMessageModel treatmentMessageModel = JsonConvert.DeserializeObject<TreatmentMessageModel>(Event.Text);
                 MessageModel messageModel = null;
                 switch (treatmentMessageModel.MessageType)
                 {
                     case TreatmentMessageModel.EnumMessageType.PlainText:
                         messageModel = treatmentMessageModel.MessageModel;
                         messageModel.SwitchTreatment();
                         UpdateAllDataStrucuresAndInterface(messageModel.Id, messageModel, null);
                         SendNotifyMessageAsync(messageModel.Id);
                         break;
                     case TreatmentMessageModel.EnumMessageType.FileOffer:
                         messageModel = treatmentMessageModel.MessageModel;
                         messageModel.SwitchTreatment();
                         UpdateAllDataStrucuresAndInterface(messageModel.Id, messageModel, null);
                         SendNotifyMessageAsync(messageModel.Id);
                         break;
                     case TreatmentMessageModel.EnumMessageType.FileOfferAnswer:
                         string id = treatmentMessageModel.Id;
                         if (_allMessagesDictionary.TryGetValue(id, out MessageModel message))
                         {
                             FileModel fileModel = message.File;
                             fileModel.SetAcceptedStateConfig();
                             SendFileAsync(id, fileModel);
                         }
                         break;
                     case TreatmentMessageModel.EnumMessageType.FileWaiting:
                         break;
                     case TreatmentMessageModel.EnumMessageType.FileOfferDeclined:
                         break;
                     case TreatmentMessageModel.EnumMessageType.FileOfferCompleted:
                         break;
                     case TreatmentMessageModel.EnumMessageType.FileOfferError:
                         break;
                     case TreatmentMessageModel.EnumMessageType.SeenOfPlainTextOrFileOfferMessage:
                         if (_allMessagesDictionary.TryGetValue(treatmentMessageModel.Id, out MessageModel messageForBeen))
                         {
                             messageForBeen.Seen = MessageModel.EnumSeen.Yes;
                         }
                         break;
                     default:
                         break;
                 }



             });

        }

        private async void SendFileAsync(string id, FileModel fileModel)
        {
            CancellationToken cancellationToken = new CancellationToken();
            await Task.Run(async () =>
           {
               try
               {

                   if (!(Conductor.Instance.FileChannel.ReadyState == Org.WebRtc.RTCDataChannelState.Open))
                       Debug.WriteLine("[Warning] ChannelRemoteConnectionPageViewModel : File channel is not open.");

                   if (!_allStoregeFilesDictionary.TryGetValue(id, out StorageFile storageFile))
                       return;

                   using (var stream = await storageFile.OpenStreamForReadAsync())
                   {

                       if (fileModel.TotalSize <= MyConstants.MAX_ONE_CHUNK_SIZE)
                       {
                           if (!fileModel.IsAccepted)
                               return;
                           byte[] buffer = new byte[(int)stream.Length];
                           await stream.ReadAsync(buffer, 0, (int)stream.Length);

                           Conductor.Instance.FileChannel.Send(buffer);

                       }
                       else
                       {
                           for (ulong i = 0; i < fileModel.TotalSize;)
                           {
                               if (!fileModel.IsAccepted)
                                   return;
                               if (cancellationToken.IsCancellationRequested)
                               {
                                   cancellationToken.ThrowIfCancellationRequested();
                                   if (cancellationToken.CanBeCanceled)
                                   {
                                       Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Task is ending : ");
                                       return;
                                   }
                               }
                               byte[] buffer = new byte[(int)MyConstants.CHUNK_SIZE];

                               await stream.ReadAsync(buffer, (int)i, (int)(i + MyConstants.CHUNK_SIZE));

                               Conductor.Instance.FileChannel.Send(buffer);
                               CalculateIndex(ref i, fileModel.TotalSize);
                           }
                       }


                   }
                   _allStoregeFilesDictionary.Remove(id);// delete storagefile

               }
               catch (ArgumentOutOfRangeException ou)
               {
                   Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Stream.ReadAsync out of range error  : " + ou.Message);
               }
               catch (OperationCanceledException o)
               {
                   Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Cancelation of task was wanted : " + o.Message);
               }
               catch (ArgumentException a)
               {
                   Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Getting from dic was ended with error or stream.ReadAsync caused this: " + a.Message);
               }
               catch (Exception e)
               {

                   Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : Error was oqqured : " + e.Message);
               }

           }, cancellationToken);

        }

        private void CalculateIndex(ref ulong i, ulong totalSize)
        {
            if (i + MyConstants.CHUNK_SIZE > totalSize)
            {
                i += (MyConstants.CHUNK_SIZE - i);
            }
            else
            {
                i += MyConstants.CHUNK_SIZE;
            }

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

        #endregion
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
