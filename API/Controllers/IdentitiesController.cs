using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API;
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

        [HttpPost("{attempts}")]
        public async Task<IActionResult> Login([FromRoute]int attempts, LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(attempts >= 3)
            {
                await _userManager.Suspend(login.Login);
                return BadRequest("maximum attempts exceeded");
            }

            try
            {
                var identity = await _userManager.CheckUserAsync(login.Login, login.Password);
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: DateTime.UtcNow,
                    claims: new List<Claim> { new Claim(ClaimTypes.Role, identity.Role.ToString()) },
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var response = new
                {
                    user = new {
                        Name = identity.Name,
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
    }
}