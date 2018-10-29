using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galery.Server.Controllers
{
    [Route("api/tag")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TagController : Controller
    {
        readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody]string name)
        {
            var res = await _service.CreateTag(name);
            return Ok(res);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTags()
        {
            var res = await _service.GetAllTags();
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody]string name)
        {
            var res = await _service.UpdateTag(id, name);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            await _service.DeleteTag(id);
            return Ok();
        }
    }
}
