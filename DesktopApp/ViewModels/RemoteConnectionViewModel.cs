using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace DesktopApp.ViewModels
{
    public class RemoteConnectionViewModel : ViewModelBase
    {
        public StackPanel PastInteractionStackPanel { get; set; }
        public RemoteConnectionViewModel()
        {
            
            
            
        }

        public void createMessage(ICollection<string> parameters, bool received = false)
        {
            ///TODO WTS:Create message metodunu ipmlement et
            throw new NotImplementedException();
            //// Create a RichTextBlock, a Paragraph and a Run.
            //RichTextBlock richTextBlock = new RichTextBlock();
            //Paragraph paragraph = new Paragraph();
            //Run run = new Run();

            //// Customize some properties on the RichTextBlock.
            //richTextBlock.IsTextSelectionEnabled = true;
            //richTextBlock.SelectionHighlightColor = new SolidColorBrush(Windows.UI.Colors.Pink);
            //richTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Blue);
            //richTextBlock.FontWeight = Windows.UI.Text.FontWeights.Light;
            //richTextBlock.FontFamily = new FontFamily("Arial");
            //richTextBlock.FontStyle = Windows.UI.Text.FontStyle.Italic;
            //run.Text = "This is some sample text to demonstrate some properties.";

            ////Add the Run to the Paragraph, the Paragraph to the RichTextBlock.
            //paragraph.Inlines.Add(run);
            //richTextBlock.Blocks.Add(paragraph);

            //// Add the RichTextBlock to the visual tree (assumes stackPanel is decalred in XAML).
            //PastInteractionStackPanel.Children.Add(richTextBlock);
        }
    }
}
