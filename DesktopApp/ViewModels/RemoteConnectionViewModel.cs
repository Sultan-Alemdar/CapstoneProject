using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DesktopApp.Core.Models;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace DesktopApp.ViewModels
{
    public class RemoteConnectionViewModel : ViewModelBase
    {
        private StackPanel _pastInteractionStackPanel;
        public StackPanel PastInteractionStackPanel { get => _pastInteractionStackPanel; set => Set<StackPanel>("PastInteractionStackPanel", ref this._pastInteractionStackPanel, value); }

        private ObservableCollection<File> fileItems = new ObservableCollection<File>();
        public ObservableCollection<File> FileItems { get => fileItems; set => Set<ObservableCollection<File>>("FileItems", ref this.fileItems, value); }

        public RemoteConnectionViewModel()
        {
            PastInteractionStackPanel = new StackPanel();
            PastInteractionStackPanel.Children.Add(CreateMessage());
            // RichTextBlock asd = PastInteractionStackPanel.Children.Move(0)

            File file = new File("Deneme", "", "", 5, "", "");
            File file2 = new File("ikiinci", "", "", 5, "", "");
            FileItems.Add(file);
            FileItems.Add(file2);
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
        public void PeerVideo_MediaFailed(object sender, Windows.UI.Xaml.ExceptionRoutedEventArgs e)
        {
            ///TODO WTS: RemoteConnectionPage için PeerVideo_MediaFailed implement et.
            throw new NotImplementedException();
        }
    }
}
