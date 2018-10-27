using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galery.Server.Service.DTO.CommentDTO;
using Galery.Server.Service.Infrostructure;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galery.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Comment")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CommentController : Controller
    {
        readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody]CreateCommentDTO model)
        {
            var res = await _service.CreateCommentAsync(model);
            return this.GetResult(res, true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _service.DeleteCommentAsync(id);
            return Ok();
        }
    }
}