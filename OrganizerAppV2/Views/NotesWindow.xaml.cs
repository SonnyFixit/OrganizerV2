using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace OrganizerAppV2.Views
{
    /// <summary>
    /// Logika interakcji dla klasy NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        public NotesWindow()
        {
            InitializeComponent();
        }
        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Language to text funcionality.
        //Tested and implemented with the use of Azure, for .NetCore.
        //For personal reasons (and to not forget about it and get a huge bill, the key for the service is not actually provided with the code)
        //But the whole mechanic should work fine

        private async void SpeechButton_Click(object sender, RoutedEventArgs e)
        {
            string region = "westeurope";
            string key = "";

            var speechConfig = SpeechConfig.FromSubscription(key, region);

            //Other audio input can be used, if needed
            using(var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            using (var recognizer = new SpeechRecognizer(speechConfig))
            {
                var result = await recognizer.RecognizeOnceAsync();

                //After text is recognized, access the document and block, to save the spokone words. Text is already in the var result.

                contentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(result.Text)));
            }
        }

        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Counting amount of letters

            string ammountCharacters = (new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd)).Text.Trim();

            int charactersCount = ammountCharacters.Length;

            statusTextBlock.Text = $"Document length: {charactersCount} characters";

            //To fix - improve counting, don't add some characters. 
        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {

            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonChecked == true)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }

            
        }


        //Check, if text is bolded or not in specific selection, to signify it in the button
        private void contentRichTextBox_SelectionChanged (object sender, RoutedEventArgs e)
        {
            //Whatever weight is the weight of currently selected text
            var selectedWeight = contentRichTextBox.Selection.GetPropertyValue(FontWeightProperty);


            boldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && (selectedWeight.Equals(FontWeights.Bold));
        }
    }
}
