using auth.APPLICATION.Services;
using auth.CORE.Entities;
using auth.WEBAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        public AccountController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        public async Task<string> SignInWithClaims(string userName, string role = "User")
        {
            var mySecret = _configuration["Jwt:Key"];
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
            var myIssuer = _configuration["Jwt:Issuer"];
            var myAudience = _configuration["Jwt:Audience"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim("UserName", userName),
                    new Claim("Role", role),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(100),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, 
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
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]TokenRequestViewModel model)
        {
            var res = await _userService.Login(model.Username, model.Password);
            string role = res.Username == "abc" ? "Admin" : "User";
            var token = SignInWithClaims(res.Username.ToString(), role);
            var result = new
            {
                userName = res.Username,
                token = token.Result,
            };
            return Ok(result);
        }
    }
}
