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
        public Task<string> LoadPicture(IFormFile file)
        {
            return _file.SavePicture(file);
        }

        [HttpPost("avatar")]
        public Task<string> LoadAvatar([FromForm]IFormFile file)
        {
            return _file.SaveAvatar(file);
        }
    }
}