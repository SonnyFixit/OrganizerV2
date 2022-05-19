using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using OrganizerAppV2.Models;

namespace OrganizerAppV2.ViewModels.Commands
{
    public class StopEditingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public NotesVM VM { get; set; }

        public StopEditingCommand (NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Notebook notebook = parameter as Notebook;
            if (notebook != null)
            {
                VM.StopEditing(notebook);
            }
            
        }
    }
}
