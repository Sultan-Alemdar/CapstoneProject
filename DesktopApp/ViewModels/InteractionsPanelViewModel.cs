﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace DesktopApp.ViewModels
{
    public partial class InteractionsPanelViewModel : ViewModelBase
    {
        private StackPanel _pastInteractionStackPanel;
        public StackPanel PastInteractionStackPanel { get => _pastInteractionStackPanel; set => Set<StackPanel>("PastInteractionStackPanel", ref this._pastInteractionStackPanel, value); }
        public double Height
        {
            get;set;
        }
        public InteractionsPanelViewModel()
        {

            PastInteractionStackPanel = new StackPanel();
            PastInteractionStackPanel.Children.Add(CreateMessage());
            // RichTextBlock asd = PastInteractionStackPanel.Children.Move(0)
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
