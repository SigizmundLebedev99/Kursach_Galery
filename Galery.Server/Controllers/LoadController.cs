using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galery.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/load")]
    public class LoadController : Controller
    {
        readonly IFileWorkService _file;

        public LoadController([FromForm]IFileWorkService file)
        {
            _file = file;
        }

        [HttpPost("image")]
        public async Task<IActionResult> LoadPicture(IFormFile file)
        {
            var res = await _file.SavePicture(file);
            if (res == null)
                return BadRequest();
            return Ok(res);
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> LoadAvatar(IFormFile file)
        {
            var res = await _file.SaveAvatar(file);
            if (res == null)
                return BadRequest();
            return Ok(res);
        }
    }
}