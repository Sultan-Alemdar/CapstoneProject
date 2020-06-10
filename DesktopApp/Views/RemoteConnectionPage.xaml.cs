using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

using System.Threading.Tasks;
using DesktopApp.Core.Models;

using DesktopApp.ViewModels;
using PeerConnectionClientOperators.Signalling;
using Windows.Foundation;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.Storage;
using Windows.Storage.Provider;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using System.Globalization;

namespace DesktopApp.Views
{
    public sealed partial class RemoteConnectionPage : Page, INotifyPropertyChanged
    {
        private CoreDispatcher _coreDispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;




        public event PropertyChangedEventHandler PropertyChanged;

        private RemoteConnectionViewModel ViewModel => ViewModelLocator.Current.RemoteConnectionViewModel;


        public RemoteConnectionPage()
        {
            InitializeComponent();
            Loaded += RemoteConnectionPage_Loaded;

        }

        private async void RemoteConnectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            await RunOnUI(CoreDispatcherPriority.Low, () =>
            {
                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(600, 500));
                ApplicationView.GetForCurrentView().TryResizeView(new Size(1850, 970));
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            });

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private async Task RunOnUI(CoreDispatcherPriority priority, Action action)
        {
            await _coreDispatcher.RunAsync(priority, new DispatchedHandler(action));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int myid, messageid;
            var id = ViewModel.CreateId(out myid, out messageid);
            FileModel fileModel = new FileModel(id, "AhmetSaruhan", "pdf", 12053, "PDF Dökümanı", "asdqwd");
            MessageModel messageModel = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Send, null, fileModel);

            FileModel file = new FileModel("asdasd", "AhmetSaruhan", "pdf", 12053, "PDF Dökümanı", "sad");
            MessageModel ms = new MessageModel(id, DateTime.Now.ToString("mm:ss"), MessageModel.EnumEvent.Received, null, fileModel);

            ViewModel.AllMessagesOnInterfaceCollection.Add(messageModel);
            ViewModel.AllFilesOnInterfaceCollection.Add(fileModel);
            ViewModel.AllMessagesOnInterfaceCollection.Add(ms);
            ViewModel.AllFilesOnInterfaceCollection.Add(file);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
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

                foreach (StorageFile storageFile in files)
                {

                    using (var stream = await storageFile.OpenStreamForReadAsync())
                    {
                        _downloadStream = new MemoryStream((int)stream.Length);
                        int bufferSize = 16 * 1024;
                        long total = (long)stream.Length;
                        while (stream.Position < total)
                        {
                            byte[] buffer = new byte[bufferSize];
                            //await stream.ReadAsync(buffer, (int)i, (int)(i + bufferSize));
                            await stream.ReadAsync(buffer, 0, (int)bufferSize);
                            if (stream.Position + bufferSize > total)
                                bufferSize = (int)(total - stream.Position);
                            GetMemory(buffer);
                        }

                    }
                }
            }
        }
        private MemoryStream _downloadStream = new MemoryStream();
        private void GetMemory(byte[] chunk)
        {
            _downloadStream.Write(chunk, 0, chunk.Length);
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("7z", new List<string>() { "." + "7z" });
            savePicker.SuggestedFileName = "Deneme";
            Windows.Storage.StorageFile storageFile = await savePicker.PickSaveFileAsync();
            CachedFileManager.DeferUpdates(storageFile);
            // write to file

            await Task.Run(async () =>
            {

                await FileIO.WriteBytesAsync(storageFile, _downloadStream.GetBuffer());
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(storageFile);
                if (status == FileUpdateStatus.Complete)
                {
                    Debug.WriteLine("[Info] ChannelRemoteConnectionPageViewModel : File {0} was saved.", storageFile.Name);
                    // endedMessage.File.SetEndedStateConfig();
                }
                else
                {
                    Debug.WriteLine("[Error] ChannelRemoteConnectionPageViewModel : File {0} could not being saved.", storageFile.Name);
                    //endedMessage.File.SetFailureStateConfig();
                }
                _downloadStream = new MemoryStream();

            });
            // _downloadStream.Write(chunk, 0, chunk.Length);
        }
        private FileModel fileModel = new FileModel("Dnk", "Deneme", "pdf", 50064, "Pdf Dosyası", "*.pdf");


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FileModel upCompletef = new FileModel("send", "upCompletef file", "pdf", 50064, "Zip Dosyası", "*.pdf");
            FileModel downCompletef = new FileModel("send", "downCompletef file", "pdf", 50064, "Zip Dosyası", "*.pdf");
            FileModel senf = new FileModel("send", "Send file", "zip", 50064, "Zip Dosyası", "*.zip");
            FileModel recf = new FileModel("rec", "recf file", "zip", 50064, "Zip Dosyası", "*.zip");
            FileModel senf2 = new FileModel("send2", "senf2 file2", "zip", 50064, "Zip Dosyası", "*.zip");
            FileModel recf2 = new FileModel("rec2", "recf2 file", "zip", 50064, "Zip Dosyası", "*.zip");

            downCompletef.Event = FileModel.EnumEvent.Download;
            upCompletef.Event = FileModel.EnumEvent.Upload;
            senf.Event = FileModel.EnumEvent.Upload;
            senf2.Event = FileModel.EnumEvent.Upload;
            recf.Event = FileModel.EnumEvent.Download;
            recf2.Event = FileModel.EnumEvent.Download;


            senf.ProgressedSize = 20000;

            upCompletef.ProgressedSize = 50064;
            downCompletef.ProgressedSize = 50064;

            upCompletef.SetEndedStateConfig();
            var cul = CultureInfo.CreateSpecificCulture("en-US");
            var a = DateTime.Now.ToString("t");
            MessageModel upComplete = new MessageModel("upComplete", a, MessageModel.EnumEvent.Send, "", upCompletef);
            ViewModel.AllFilesOnInterfaceCollection.Add(upCompletef);
            ViewModel.AllMessagesOnInterfaceCollection.Add(upComplete);

            downCompletef.SetEndedStateConfig();
            MessageModel downComplete = new MessageModel("downComplete", a, MessageModel.EnumEvent.Received, "", downCompletef);
            ViewModel.AllFilesOnInterfaceCollection.Add(downCompletef);
            ViewModel.AllMessagesOnInterfaceCollection.Add(downComplete);


            senf.SetStartedStateConfig();
            MessageModel send = new MessageModel("send", a, MessageModel.EnumEvent.Send, "", senf);
            ViewModel.AllFilesOnInterfaceCollection.Add(senf);
            ViewModel.AllMessagesOnInterfaceCollection.Add(send);

            MessageModel t1 = new MessageModel("t1", a, MessageModel.EnumEvent.Send, "Hello World! What are you doing", null);
            // ViewModel.AllFilesOnInterfaceCollection.Add(fileModel);
            ViewModel.AllMessagesOnInterfaceCollection.Add(t1);

            recf.SetAcceptedStateConfig();
            MessageModel rec = new MessageModel("rec", a, MessageModel.EnumEvent.Received, "", recf);
            ViewModel.AllFilesOnInterfaceCollection.Add(recf);
            ViewModel.AllMessagesOnInterfaceCollection.Add(rec);

            MessageModel t2 = new MessageModel("t2", a, MessageModel.EnumEvent.Received, "Hiiiiiiii I am using desktop app!", null);
            //  ViewModel.AllFilesOnInterfaceCollection.Add(fileModel);
            ViewModel.AllMessagesOnInterfaceCollection.Add(t2);

            senf2.SetAcceptedStateConfig();
            MessageModel send2 = new MessageModel("send2", a, MessageModel.EnumEvent.Send, "", senf2);
            ViewModel.AllFilesOnInterfaceCollection.Add(senf2);
            ViewModel.AllMessagesOnInterfaceCollection.Add(send2);


            recf2.SetOfferedStateConfig();
            MessageModel rec2 = new MessageModel("rec2", a, MessageModel.EnumEvent.Received, "", recf2);
            ViewModel.AllFilesOnInterfaceCollection.Add(recf2);
            ViewModel.AllMessagesOnInterfaceCollection.Add(rec2);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            fileModel.ActionSpeed += 10000;
            fileModel.ProgressedSize += 10000;
            fileModel.ShowPercent();
        }


    }
}
