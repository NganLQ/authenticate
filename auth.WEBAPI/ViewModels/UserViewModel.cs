using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auth.WEBAPI.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string UserName { get; set; }
    }
}
