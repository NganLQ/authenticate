using auth.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace auth.APPLICATION.Interfaces
{
    public interface IUserRepository
    {
        Task<AuthModels.LoginResponse> Login(string username, string password);
        Task<AuthModels.RegisterResponse> Register(User user);
    }
}
