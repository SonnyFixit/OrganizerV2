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
using System.IO;
using System.Linq;
using OrganizerAppV2.ViewModels;
using OrganizerAppV2.ViewModels.Helpers;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace OrganizerAppV2.Views
{
    /// <summary>
    /// Logika interakcji dla klasy NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {

        private NotesVM VM;

        public NotesWindow()
        {
            InitializeComponent();

            VM = Resources["vm"] as NotesVM;
            VM.SelectedNoteChanged += VM_SelectedNoteChanged;

            List<double> fontSizes = new List<double> { 6, 7, 8, 9, 10, 11, 12, 14, 16, 20, 24, 28, 32 };
            fontSizeComboBox.ItemsSource = fontSizes;

            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyComboBox.ItemsSource = fontFamilies;
            
        }

        private void VM_SelectedNoteChanged(object sender, EventArgs e)
        {
            contentRichTextBox.Document.Blocks.Clear();

            if (VM.SelectedNote != null)
            {
                if (!string.IsNullOrEmpty(VM.SelectedNote.FileLocation))
                {
                    FileStream fileStream = new FileStream(VM.SelectedNote.FileLocation, FileMode.Open);
                    var contents = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                    contents.Load(fileStream, DataFormats.Rtf);

                    fileStream.Close();
                }

            }

           
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
        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            boldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && (selectedWeight.Equals(FontWeights.Bold));

            var selectedStyle = contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            italicButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && (selectedStyle.Equals(FontStyles.Italic));

            var selecteDecoration = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            underlineButton.IsChecked = (selecteDecoration != DependencyProperty.UnsetValue) && (selecteDecoration.Equals(TextDecorations.Underline));

            fontFamilyComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);


            string fontSize = contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty).ToString();
            if (fontSize != "{DependencyProperty.UnsetValue}")
            {
                fontSizeComboBox.Text = fontSize;

               
            }

            else
            {
                fontSizeComboBox.Text = string.Empty;
            }
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonEnabled)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }


        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonEnabled)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                TextDecorationCollection textDecorations;
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontFamilyComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fontSizeComboBox.Text != string.Empty && fontSizeComboBox.Text != "0")
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
            }

          
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, $"{VM.SelectedNote.Id}.rtf");
            VM.SelectedNote.FileLocation = rtfFile;
            DatabaseHelper.Update(VM.SelectedNote);

            FileStream fileStream = new FileStream(rtfFile, FileMode.Create);
            var contents = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
            contents.Save(fileStream, DataFormats.Rtf);

            fileStream.Close();

        }







    }
}
