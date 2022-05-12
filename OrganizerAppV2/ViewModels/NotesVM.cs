using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

using OrganizerAppV2.Models;
using OrganizerAppV2.ViewModels.Commands;
using OrganizerAppV2.ViewModels.Helpers;

namespace OrganizerAppV2.ViewModels
{
    public class NotesVM: INotifyPropertyChanged
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                //Everytime the value changes, it will rise the OnPropertyChanged, allowing to access corresponding notes

                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                GetNotes();
            }
        }
        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public NotesVM()
        {
            NewNoteCommand = new NewNoteCommand(this);
            NewNotebookCommand = new NewNotebookCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            //Update list view with new notebooks
            GetNotebooks();
        }

#region Creating Notes and Notebooks methods

        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook
            {
                Name = "Notebook"
            };

            DatabaseHelper.Insert(newNotebook);

            GetNotebooks();
        }

        public void CreateNote(int notebookId)
        {
            Note newNote = new Note
            {
                NotebookId = notebookId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = $"Note for {DateTime.Now.ToString()}"
            };

            DatabaseHelper.Insert(newNote);

            //Update list view with new notes

            GetNotes();
        }

#endregion 

#region Getting Notes and Notebooks methods
        private void GetNotebooks()
        {
            var notebooks = DatabaseHelper.Read<Notebook>();

            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }



        private void GetNotes()
        {
            if (SelectedNotebook != null)
            {

                //Notes need to be filtered.
                //Only selected ones should be currenty displayed

                var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToList();

                Notes.Clear();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }

#endregion Getting Notes and Notebooks methods

        private void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
