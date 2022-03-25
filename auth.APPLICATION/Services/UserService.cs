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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new AuthModels.LoginResponse
                {
                    Success = false,
                    Errors = "Username and Password cannot be empty."
                };
            }
            // check user
            var user = await _userRepository.Login(username);
            if(user == null)
            {
                return new AuthModels.LoginResponse
                {
                    Success = false,
                    Errors = $"Cannot find user {username}. Please register befor login."
                };
            }
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
            if(result != PasswordVerificationResult.Success)
            {
                return new AuthModels.LoginResponse
                {
                    Success = false,
                    Errors = "Incorrect Password"
                };
            }
            // 
            return new AuthModels.LoginResponse
            {
                Success = true,
                UserId = user.Id,
                Username = user.Username,
            };
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
            var userExists = await _userRepository.CheckUserExists(user.Username);
            if(userExists == true)
            {
                return new AuthModels.RegisterResponse
                {
                    Success = false,
                    Username = user.Username,
                    Errors = "Username already exists. Pleas use different usernam."
                };
            }
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
