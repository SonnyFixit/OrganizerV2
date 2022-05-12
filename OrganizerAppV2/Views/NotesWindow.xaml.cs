using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private void SpeechButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Counting amount of letters

            int ammountCharacters = (new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd)).Text.Length;

            statusTextBlock.Text = $"Document length: {ammountCharacters} characters";

            //To fix - improve counting, don't add some characters. Trim should do it
        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
        }
    }
}
