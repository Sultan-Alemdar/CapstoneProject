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

namespace DesktopApp.ViewModels
{
    public sealed partial class RemoteConnectionViewModel : ViewModelBase
    {
        List<FileModel> _pickedByteFileList = new List<FileModel>();
        List<StorageFile> _strorageFiles = new List<StorageFile>();

        // List<Tuple<StorageFile, FileModel, MessageModel>> _messagess = new List<Tuple<StorageFile, FileModel, MessageModel>>();

        //Tüm mesajları ve bilgilerini temsil ediyor. Ana kaynak yani mesaj veritabanı.
        Dictionary<string, Tuple<MessageModel, FileModel, StorageFile>> _MessageDic = new Dictionary<string, Tuple<MessageModel, FileModel, StorageFile>>();

        private RelayCommand _addCommand;

        public RelayCommand AddCommand
        {
            get => _addCommand; set
            {
                Set<RelayCommand>(ref _addCommand, value, "AddCommand");
            }

        }



        private BitmapImage storageItemThumbnail;
        public BitmapImage StorageItemThumbnail
        {
            get => storageItemThumbnail; set
            {
                Set<BitmapImage>(ref storageItemThumbnail, value, "StorageItemThumbnail");
            }
        }

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


                // Application now has read/write access to the picked file(s)
                foreach (Windows.Storage.StorageFile file in files)
                {
                    int count = _MessageDic.Count;
                    var fileprops = await file.GetBasicPropertiesAsync();
                    int myId, messageId;
                    var id = CreateId(out myId, out messageId);

                    FileModel fileModel = new FileModel(id, file.Name, file.FileType.Substring(1), fileprops.Size);

                    MessageModel messageModel = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Send, null, fileModel);
         
                    var messageInfo = new Tuple<MessageModel, FileModel, StorageFile>(messageModel, fileModel, file);
                    _MessageDic.Add(id, messageInfo);
                    MessageItems.Add(messageModel);
                }

            }
            else
            {

            }

        }

        public string CreateId(out int myId, out int messageId)
        {
            StringBuilder builder = new StringBuilder();
            myId = ViewModelLocator.Current.OperationsViewModel.MyId;
            messageId = _MessageDic.Count();

            builder.Append(MyConstants.ID_START_TAG);
            builder.Append(myId.ToString());
            builder.Append(MyConstants.ID_END_TAG);
            builder.Append(MyConstants.MESSAGE_START_TAG);
            builder.Append(messageId.ToString());
            builder.Append(MyConstants.MESSAGE_END_TAG);

            return builder.ToString();
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
