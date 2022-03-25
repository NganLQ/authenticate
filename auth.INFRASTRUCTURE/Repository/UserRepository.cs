using auth.APPLICATION;
using auth.APPLICATION.Interfaces;
using auth.CORE.Entities;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using static auth.APPLICATION.AuthModels;

namespace auth.INFRASTRUCTURE.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> CheckUserExists(string username)
        {
            var sql = $"SELECT * FROM persons WHERE userName = '{username}'";
            using (var context = new MySqlConnection(_connectionString))
            {
                var result = await context.QueryFirstOrDefaultAsync<User>(sql, username);
                if(result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<User> Login(string username)
        {
            var sql = $"SELECT * FROM persons WHERE userName = '{username}'";
            using (var context = new MySqlConnection(_connectionString))
            {
                var result = await context.QueryFirstOrDefaultAsync<User>(sql, username);
                return new User
                {
                    Id = result.Id,
                    Username = result.Username,
                    Password = result.Password,
                };
            }
        }

        public async Task<AuthModels.RegisterResponse> Register(User user)
        {
            var sql = $"INSERT INTO persons(LastName, FirstName,Username, Password) VALUES " +
                $"('{user.LastName}', '{user.FirstName}', '{user.Username}', '{user.Password}')";
            
            using (var context = new MySqlConnection(_connectionString))
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
