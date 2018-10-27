using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galery.Common.Models;
using Galery.Server.Service.Infrostructure;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galery.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Subscribe")]
    public class SubscribeController : Controller
    {
        readonly ISubscribeService _subscribeService;

        public SubscribeController(ISubscribeService service)
        {
            _subscribeService = service;
        }

        [HttpGet("userinfo/{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var res = await _subscribeService.GetUserInfo(userId);
            return Ok(res);
        }
        
        [HttpPost]
        public async Task<IActionResult> Subscribing([FromBody]Subscribe model)
        {
            var res = await _subscribeService.Subscribing(model);
            return this.GetResult(res, true);
        }

        [HttpDelete]
        public async Task<IActionResult> Desubscribing([FromBody]Subscribe model)
        {
            await _subscribeService.Desubscribing(model.FromUserId, model.ToUserId);
            return Ok();
        }

        [HttpGet("from/{userId}")]
        public async Task<IActionResult> GetSubscribes(int userId)
        {
            var res = await _subscribeService.GetSubscribes(userId);
            return Ok(res);
        }

        [HttpGet("to/{userId}")]
        public async Task<IActionResult> GetSubscribers(int userId)
        {
            var res = await _subscribeService.GetSubscribers(userId);
            return Ok(res);
        }
    }
}