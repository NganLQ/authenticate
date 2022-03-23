using auth.APPLICATION;
using auth.APPLICATION.Interfaces;
using auth.CORE.Entities;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace auth.INFRASTRUCTURE.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public UserRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public Task<AuthModels.LoginResponse> Login(string username, string password)
        {
            throw new NotFiniteNumberException();
        }

        public async Task<AuthModels.RegisterResponse> Register(User user)
        {
            var sql = $"INSERT INTO users(Username, Password, LastName, FirstName) VALUES " +
                $"('{user.Username}', '{user.Password}', '{user.LastName}', '{user.FirstName}')";
            using (var context = new SqlConnection(_connectionString))
            {
                var result = await context.ExecuteAsync(sql, user);
                return new AuthModels.RegisterResponse
                {
                    Username = user.Username,
                    Success = true,
                };
            }
        }
    }
}
