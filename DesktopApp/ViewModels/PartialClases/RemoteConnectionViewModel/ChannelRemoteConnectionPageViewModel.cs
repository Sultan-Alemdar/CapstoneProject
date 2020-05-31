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

        private readonly RelayCommand _sendCommand;
        private readonly RelayCommand _addCommand;

        public RelayCommand AddCommand => _addCommand;

        public RelayCommand SendCommand => _sendCommand;

        List<FileModel> _pickedByteFileList = new List<FileModel>();
        List<StorageFile> _strorageFiles = new List<StorageFile>();

        Dictionary<string, MessageModel> _defaultTypeSendedMessages = new Dictionary<string, MessageModel>();
        //Dictionary<string, MessageModel> _defaultTypeSendedMessages = new Dictionary<string, MessageModel>();

        //Tüm mesajları ve bilgilerini temsil ediyor. Ana kaynak yani mesaj veritabanı.
        Dictionary<string, Tuple<MessageModel, FileModel, StorageFile>> _messageDic = new Dictionary<string, Tuple<MessageModel, FileModel, StorageFile>>();





        /// <summary>
        /// Temel, yalın mesaj gönderme işlemi.
        /// </summary>
        private void Send()
        {
            SendDefaultTypeMessageAsync(null);
        }
        private bool SendCanExecute()
        {
            return true;
        }

        public string MessageText { get; set; }





        private async void Add()
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
                    SendDefaultTypeMessageAsync(file);
                }
            }
        }

        private async void MessageChannel_OnMessage(Org.WebRtc.IMessageEvent Event)
        {
            await Task.Run(() =>
             {
                 ChannelMessageModel channelMessage = JsonConvert.DeserializeObject<ChannelMessageModel>(Event.Text);
                 if (channelMessage.MessageType == ChannelMessageModel.EnumMessageType.Notify)
                 {
                     switch (channelMessage.About)
                     {
                         case ChannelMessageModel.EnumAboutWhat.BeSeen:

                             break;
                         case ChannelMessageModel.EnumAboutWhat.Answer:
                             // gönderdiğin dosyayı kabul ettim, yollamaya başla gibi yani.
                             SendFileAsync(channelMessage.MessageModel);
                             break;
                         case ChannelMessageModel.EnumAboutWhat.Waiting:
                             // şimdilik dursun
                             break;
                         case ChannelMessageModel.EnumAboutWhat.Notify:
                             break;
                         case ChannelMessageModel.EnumAboutWhat.Null:
                             break;
                         default:
                             break;
                     }

                     return;
                 }
                 #region DefaultMessageBehavior
                 MessageModel messageModel = channelMessage.MessageModel;
                 messageModel.SwitchTreatment();
                 MessageItems.Add(messageModel);
                 SendNotifyMessageAsync(messageModel.Id, ChannelMessageModel.EnumAboutWhat.BeSeen);
                 #endregion


             });

        }
        private async void SendNotifyMessageAsync(string Id, ChannelMessageModel.EnumAboutWhat enumAboutWhat)
        {
            await Task.Run(() =>
           {
               var channelMessageModel = ChannelMessageModel.GetNotifyType(Id, enumAboutWhat);
               string strChannelMessageModel = JsonConvert.SerializeObject(channelMessageModel);
               Conductor.Instance.MessageChannel.Send(strChannelMessageModel);
           });
        }
        private async void SendFileAsync(MessageModel messageModel)
        {
            CancellationToken cancellationToken = new CancellationToken();
            await Task.Run(async () =>
           {
               try
               {
                   if (!_messageDic.TryGetValue(messageModel.Id, out Tuple<MessageModel, FileModel, StorageFile> messageInfo))
                       return;
                   if (!(Conductor.Instance.FileChannel.ReadyState == Org.WebRtc.RTCDataChannelState.Open))
                       Debug.WriteLine("[Warning] ChannelRemoteConnectionPageViewModel : File channel is not open.");

                   StorageFile storageFile = messageInfo.Item3;
                   using (var stream = await storageFile.OpenStreamForReadAsync())
                   {
                       messageModel.File.SetAcceptedStateConfig();
                       if (messageModel.File.TotalSize <= MyConstants.MAX_ONE_CHUNK_SIZE)
                       {
                           if (!messageModel.File.IsAccepted)
                               return;
                           byte[] buffer = new byte[(int)stream.Length];
                           await stream.ReadAsync(buffer, 0, (int)stream.Length);

                           Conductor.Instance.FileChannel.Send(buffer);

                       }
                       else
                       {
                           for (ulong i = 0; i < messageModel.File.TotalSize;)
                           {
                               if (!messageModel.File.IsAccepted)
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
                               CalculateIndex(ref i, messageModel.File.TotalSize);
                           }
                       }


                   }
                   _messageDic.Remove(messageModel.Id);//çöplüğü boşalt

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

        public string CreateId(out int myId, out int messageId)
        {
            StringBuilder builder = new StringBuilder();
            myId = ViewModelLocator.Current.OperationsViewModel.MyId;
            messageId = _messageDic.Count();

            builder.Append(MyConstants.ID_START_TAG);
            builder.Append(myId.ToString());
            builder.Append(MyConstants.ID_END_TAG);
            builder.Append(MyConstants.MESSAGE_START_TAG);
            builder.Append(messageId.ToString());
            builder.Append(MyConstants.MESSAGE_END_TAG);

            return builder.ToString();
        }

        /// <summary>
        /// All messages must go via this method to data channel. It provides default comunication. Simple or default message type comunication.
        /// </summary>
        /// <param name="file">It will be represent file input stream on later, if user wants to share a file.</param>
        private async void SendDefaultTypeMessageAsync(StorageFile file = null)
        {
            await Task.Run(async () =>
            {
                var id = CreateId(out int myId, out int messageId);
                MessageModel messageModel = null;
                if (file != null)
                {
                    var fileprops = await file.GetBasicPropertiesAsync();

                    FileModel fileModel = new FileModel(id, file.Name, file.FileType.Substring(1), fileprops.Size);

                    messageModel = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Send, null, fileModel);

                    var messageInfo = new Tuple<MessageModel, FileModel, StorageFile>(messageModel, fileModel, file);
                    _messageDic.Add(id, messageInfo);
                }
                messageModel = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Send, MessageText);
                _defaultTypeSendedMessages.Add(id, messageModel);
                MessageItems.Add(messageModel);
                var channelMessageModel = ChannelMessageModel.GetDefaultType(messageModel);
                string strChannelMessageModel = JsonConvert.SerializeObject(channelMessageModel);
                Conductor.Instance.MessageChannel.Send(strChannelMessageModel);
            });
        }
        private bool AddCanExecute()
        {
            return true;
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
