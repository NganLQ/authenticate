using auth.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static auth.APPLICATION.AuthModels;

namespace auth.APPLICATION.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Login(string username);
        Task<AuthModels.RegisterResponse> Register(User user);
        Task<bool> CheckUserExists(string username);
    }
}
