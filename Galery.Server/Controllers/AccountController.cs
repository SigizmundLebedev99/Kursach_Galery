using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Galery.Server.DAL.Models;
using Galery.Server.DTO;
using Galery.Server.DTO.User;
using Galery.Server.JWT;
using Galery.Server.Service.DTO.User;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Galery.Server.Controllers
{
    [Route("api/Account")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly IEmailService _email;
        readonly IMapper _mapper;

        public AccountController(
            UserManager<User> userManager,
            IEmailService email, IMapper mapper)
        {
            _email = email;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Return jwt-token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        public async Task<IActionResult> Token([FromBody]LoginDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = await _userManager.FindByNameAsync(model.Name);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        if (!await _userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("Email", "Вы не подтвердили свой email");
                            return BadRequest(ModelState);
                        }
                        
                        return Ok(GetToken(user));
                    }
                    ModelState.AddModelError("Password", "Wrong password");
                    return BadRequest(ModelState);
                }

                ModelState.AddModelError("Email", $"Пользователя с email {model.Name} не существует");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<TokenResponse> GetToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var securityKey = AuthTokenOptions.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(AuthTokenOptions.ISSUER,
                AuthTokenOptions.AUDIENCE,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponse
            {
                Access_token = encodedJwt,
                Username = user.UserName,
                UserId = user.Id,
                Avatar = user.Avatar,
            };
        }

        private ClaimsIdentity GetIdentity(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            foreach (string role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Name");
            return claimsIdentity;
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = _mapper.Map<User>(model);
            user.DateOfCreation = DateTime.Now;
            var res = await _userManager.CreateAsync(user, model.Password);
            if (res.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, code },
                    protocol: HttpContext.Request.Scheme);
                await _email.SendEmailAsync(model.Email, "Confirm your account",
                    $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                return Ok();
            }
            return StatusCode((int)HttpStatusCode.Conflict, res.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("ConfirmationError");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("ConfirmationError");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return View("ConfirmEmail", user);
            return View("Error");
        }
    }
}