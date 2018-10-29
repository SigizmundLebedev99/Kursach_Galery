using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galery.Common.DTO.User;
using Galery.Common.Models;
using Galery.Server.Service.Infrostructure;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galery.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Subscribe")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SubscribeController : Controller
    {
        readonly ISubscribeService _subscribeService;

        public SubscribeController(ISubscribeService service)
        {
            _subscribeService = service;
        }

        [HttpGet("userinfo/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            UserInfoDTO res = null;
            if (User.Identity.IsAuthenticated)
            {
                var fromUserId = User.GetUserId();
                res = await _subscribeService.GetUserInfo(userId, fromUserId);
            }
            else
                res = await _subscribeService.GetUserInfo(userId);
            return Ok(res);
        }
        
        [HttpPost("{toId}")]
        public async Task<IActionResult> Subscribing(int toId)
        {
            var userId = User.GetUserId();
            var res = await _subscribeService.Subscribing(new Subscribe { FromUserId = userId, ToUserId = toId});
            return this.GetResult(res, true);
        }

        [HttpDelete("{toId}")]
        public async Task<IActionResult> Desubscribing(int toId)
        {
            int fromId = User.GetUserId();
            await _subscribeService.Desubscribing(fromId, toId);
            return Ok();
        }

        [HttpGet("from/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubscribes(int userId)
        {
            var res = await _subscribeService.GetSubscribes(userId);
            return Ok(res);
        }

        [HttpGet("to/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubscribers(int userId)
        {
            var res = await _subscribeService.GetSubscribers(userId);
            return Ok(res);
        }

        [HttpGet("search/{search}")]
        [AllowAnonymous]
        public async Task<IActionResult> UserSerch(string search)
        {
            var res = await _subscribeService.UserSearch(search);
            return Ok(res);
        }
    }
}