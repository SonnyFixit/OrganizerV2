using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;

namespace OrganizerAppV2.ViewModels.Commands
{
    public class NewNotebookCommand: ICommand
    {
        public NotesVM ViewModel { get; set; }

        public event EventHandler CanExecuteChanged;

        public NewNotebookCommand(NotesVM vm)
        {
            ViewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.CreateNotebook();
        }
    }
}
