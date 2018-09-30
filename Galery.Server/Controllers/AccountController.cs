using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Galery.Server.DAL.Models;
using Galery.Server.DTO;
using Galery.Server.DTO.User;
using Galery.Server.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Galery.Server.Controllers
{
    [Route("api/Account")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly IConfiguration Configuration;

        public AccountController(
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            Configuration = configuration;
        }

        /// <summary>
        /// Return jwt-token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        public async Task<IActionResult> Token(LoginDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        if (!await _userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("Email", "Вы не подтвердили свой email");
                            return BadRequest(ModelState);
                        }
                        var roles = await _userManager.GetRolesAsync(user);
                        var now = DateTime.UtcNow;
                        // создаем JWT-токен
                        var jwt = new JwtSecurityToken(
                            issuer: AuthTokenOptions.ISSUER,
                            notBefore: now,
                            expires: now.Add(TimeSpan.FromMinutes(AuthTokenOptions.LIFETIME)),
                            signingCredentials: new SigningCredentials(AuthTokenOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                        var response = new TokenResponse
                        {
                            Access_token = encodedJwt,
                            Username = user.UserName,
                            UserId = user.Id,
                            Roles = roles,
                            Start = now,
                            Finish = now.Add(TimeSpan.FromMinutes(AuthTokenOptions.LIFETIME))
                        };
                        return Ok(response);
                    }
                    ModelState.AddModelError("Password", "Wrong password");
                    return BadRequest(ModelState);
                }

                ModelState.AddModelError("Email", $"Пользователя с email {model.Email} не существует");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}