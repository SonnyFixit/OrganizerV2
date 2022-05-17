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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OrganizerAppV2.Models;

namespace OrganizerAppV2.Views.UserControls
{
    /// <summary>
    /// Logika interakcji dla klasy DisplayNotebook.xaml
    /// </summary>
    public partial class DisplayNotebook : UserControl
    {


        public Notebook MyProperty
        {
            get { return (Notebook)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(Notebook), typeof(DisplayNotebook), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNotebook notebookUserControl = d as DisplayNotebook;

            if (notebookUserControl != null)
            {
                notebookUserControl.DataContext = notebookUserControl.MyProperty;
            }
        }

        public DisplayNotebook()
        {
            InitializeComponent();
        }
    }
}
