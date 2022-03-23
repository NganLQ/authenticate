using auth.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace auth.APPLICATION.Services
{
    public interface IUserService
    {
        Task<AuthModels.LoginResponse> Login(string username, string password);
        Task<AuthModels.RegisterResponse> Register(User user);
    }
}
