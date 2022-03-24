using auth.APPLICATION.Services;
using auth.CORE.Entities;
using auth.WEBAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace auth.WEBAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<string> SignInWithClaims(string userId, string userName, string role = "User")
        {
            //var userClaims = ,

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("security key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", userId.ToString()),
                    new Claim("UserName", userName.ToString()),
                    new Claim("Role", role.ToString()),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]UserViewModel model)
        {
            if(model.Password != model.PasswordConfirm)
            {
                return BadRequest("Passwords does not matched");
            }
            var user = new User
            {
                LastName = model.LastName,
                FirstName = model.FirstName,
                Username = model.UserName,
                Password = model.Password
            };
            var res = await _userService.Register(user);
            if (res.Success)
            {
                return Ok("OK");
            }
            return StatusCode(500, res.Errors);
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]TokenRequestViewModel model)
        {
            var res = await _userService.Login(model.Username, model.Password);
            string role = res.UserId == 1 ? "Admin" : "User";
            var token = SignInWithClaims(res.UserId.ToString(), "abc", role);
            var result = new
            {
                userId = res.UserId,
                token = token,
            };
            return Ok(result);
        }
    }
}
