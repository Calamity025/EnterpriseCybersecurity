using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserManager;
using UserManager.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentitiesController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public IdentitiesController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var identity = await _userManager.CheckUserAsync(login.Login, login.Password);
                
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: DateTime.UtcNow,
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, identity.Role.ToString()),
                        new Claim("login", identity.Login)
                    },
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var response = new
                {
                    user = new {
                        Name = identity.Name,
                        Login = identity.Login,
                        Status = identity.Status.ToString(),
                        Role = identity.Role.ToString()
                    },
                    access_token = encodedJwt
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userManager.GetUsers();
                return Ok(users.ToList().Select(x => new
                {
                    Name = x.Name,
                    Login = x.Login,
                    Role = x.Role.ToString(),
                    Status = x.Status.ToString()
                }));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch("{newStatus}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus([FromBody] string login, string newStatus)
        {
            try
            {
                await _userManager.ChangeStatus(login,
                    newStatus == UserManager.User.Statuses.Active.ToString()
                        ? UserManager.User.Statuses.Active
                        : UserManager.User.Statuses.Banned);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{role}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole([FromBody] string login, string role)
        {
            try
            {
                await _userManager.ChangePermissionAsync(login,
                    role == UserManager.User.Roles.Admin.ToString()
                        ? UserManager.User.Roles.Admin
                        : UserManager.User.Roles.User);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordPayload passwordPayload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var regex = new Regex("(?=[0-9]+)(?=[A-Za-z]+)");
            if (!regex.IsMatch(passwordPayload.NewPassword))
            {
                return BadRequest("Password must contain at least 1 character and 1 number");
            }
            try
            {
                await _userManager.ChangePasswordAsync(User.Claims.FirstOrDefault(x => x.Type.Equals("login"))?.Value,
                    passwordPayload.OldPassword, passwordPayload.NewPassword);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}