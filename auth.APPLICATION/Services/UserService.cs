using auth.APPLICATION.Interfaces;
using auth.CORE.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace auth.APPLICATION.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<AuthModels.LoginResponse> Login(string username, string password)
        {
            
            throw new NotImplementedException();
        }

        public async Task<AuthModels.RegisterResponse> Register(User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return new AuthModels.RegisterResponse
                {
                    Success = false,
                    Username = user.Username,
                    Errors = "Username and Password cannot be empty."
                };
            }
            //check user already exists
            //
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, user.Password);
            var res = await _userRepository.Register(user);
            return new AuthModels.RegisterResponse
            {
                Username = res.Username,
                Success = res.Success
            };
        }
    }
}
