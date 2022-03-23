using auth.APPLICATION.Services;
using auth.CORE.Entities;
using auth.WEBAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auth.WEBAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserViewModel model)
        {
            if(model.Password != model.PasswordConfirm)
            {
                return BadRequest("Passwords does not matched");
            }
            var user = new User
            {
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
            model.Username = model.Username.Trim().ToLower();
            return Ok();
        }
    }
}
