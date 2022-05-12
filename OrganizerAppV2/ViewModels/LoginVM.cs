using System;
using System.Collections.Generic;
using System.Text;
using OrganizerAppV2.Models;
using OrganizerAppV2.ViewModels.Commands;

namespace OrganizerAppV2.ViewModels
{
    public class LoginVM
    {
        private User user;
        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }

        public LoginVM()
        {
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
        }
    }
}
