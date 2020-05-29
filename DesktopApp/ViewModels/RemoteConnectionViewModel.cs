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

        public string AssetsDirectory { get; set; }
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
            PastInteractionStackPanel.Children.Add(CreateMessage());
            AssetsDirectory = "../Assets/File_Operations_Svg/";
            _disconnectFromPeerCommand = new GalaSoft.MvvmLight.Command.RelayCommand(DiscconectFromPeer, DiscconectFromCanExecute);
        }

        public RichTextBlock CreateMessage(Dictionary<string, string> parameters = null, bool received = false)
        {
            ///TODO WTS:Create message metodunu implement et
            //throw new NotImplementedException();
            // Create a RichTextBlock, a Paragraph and a Run.
            RichTextBlock richTextBlock = new RichTextBlock();
            Paragraph paragraph = new Paragraph();
            Run run = new Run();

            // Customize some properties on the RichTextBlock.
            richTextBlock.IsTextSelectionEnabled = true;
            richTextBlock.SelectionHighlightColor = new SolidColorBrush(Windows.UI.Colors.Pink);
            richTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Blue);
            richTextBlock.FontWeight = Windows.UI.Text.FontWeights.Light;
            richTextBlock.FontFamily = new FontFamily("Arial");
            richTextBlock.FontStyle = Windows.UI.Text.FontStyle.Italic;
            run.Text = "This is some sample text to demonstrate some properties.";

            //Add the Run to the Paragraph, the Paragraph to the RichTextBlock.
            paragraph.Inlines.Add(run);

            #region ReWrite
            //var asd = (IEnumerable)paragraph.Inlines;
            //IEnumerator dad = asd.GetEnumerator();

            //while (dad.MoveNext())
            //{
            //    Run item = (Run)dad.Current;
            //    run.Text = "1010101";
            //} 
            #endregion

            richTextBlock.Blocks.Add(paragraph);

            // Add the RichTextBlock to the visual tree (assumes stackPanel is decalred in XAML).
            return richTextBlock;
        }

    }
}
