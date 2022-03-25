using System;
using System.Collections.Generic;
using System.Text;

namespace auth.APPLICATION
{
    public class AuthModels
    {
        public class RegisterResponse
        {
            public string Username { get; set; }
            public string Errors { get; set; }
            public bool Success { get; set; }
        }

        public class LoginResponse
        {
            public bool Success { get; set; }
            public string Errors { get; set; }
            public long UserId { get; set; }
            public string Username { get; set; }
        }

        public class Login
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Register
        {
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }
    }
}
