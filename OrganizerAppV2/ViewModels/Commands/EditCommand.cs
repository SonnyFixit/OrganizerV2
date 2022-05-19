using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace OrganizerAppV2.ViewModels.Commands
{
    public class EditCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;

        public NotesVM VM { get; set; }

        public EditCommand(NotesVM vm)
        {
            VM = vm;
        }


        

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.StartEditing();
        }
    }
}
