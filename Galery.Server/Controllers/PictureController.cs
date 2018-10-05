using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galery.Server.Interfaces;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.Infrostructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Galery.Server.Controllers
{
    [Route("picture")]
    public class PictureController : Controller
    {
        readonly IPictureService _service;

        public PictureController(IPictureService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePicture([FromForm]CreatePictureDTO model)
        {
            var result = await _service.CreatePictureAsync(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePicture(int id, [FromForm]CreatePictureDTO model)
        {
            var result = await _service.UpdatePictureAsync(id, model);
            return GetResult(result, true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            await _service.DeletePictureAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPictureById(int id, [FromHeader]int userId)
        {
            var res = await _service.GetPictureByIdAsync(id, userId);
            return GetResult(res, true);
        }

        [HttpGet("anon/{id}")]
        public async Task<IActionResult> GetPictureByIdAnonimous(int id)
        {
            var res = await _service.GetPictureByIdAnonimousAsync(id);
            return Ok(res);
        }

        [HttpGet("liked/{userId}")]
        public async Task<IActionResult> GetLikedByUser(int userId, [FromQuery]int? skip, [FromQuery]int? take)
        {
            var res = await _service.GetLikedByUserAsync(userId, skip, take);
            return Ok(res);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId, [FromQuery]int? skip, [FromQuery]int? take)
        {
            var res = await _service.GetByUserAsync(userId, skip, take);
            return Ok(res);
        }

        [HttpGet("top/{userId}")]
        public async Task<IActionResult> GetTopPictures([FromQuery]int? skip, [FromQuery]int? take)
        {
            var res = await _service.GetTopPicturesAsync(skip, take);
            return Ok(res);
        }

        [HttpPost("like/{userId}/{pictureId}")]
        public async Task<IActionResult> SetLike(int userId, int pictureId)
        {
            await _service.SetLikeAsync(userId, pictureId);
            return Ok();
        }

        [HttpPost("relike/{userId}/{pictureId}")]
        public async Task<IActionResult> RemoveLikeAsync(int userId, int pictureId)
        {
            await _service.RemoveLikeAsync(userId, pictureId);
            return Ok();
        }

        [HttpPost("tag/{tagId}")]
        public async Task<IActionResult> GetPictursByTag(int tagId, [FromQuery]int? skip, [FromQuery]int? take)
        {
            var res = await _service.GetPictursByTag(tagId, skip, take);
            return Ok(res);
        }


        private ActionResult GetResult<T>(OperationResult<T> operRes, bool isSingle)
        {
            if (operRes.Succeeded)
            {
                if (!isSingle)
                    return Ok(operRes.Results);
                else
                    return Ok(operRes.Results.First());
            }
            {
                foreach (var e in operRes.ErrorMessages)
                {
                    ModelState.AddModelError(e.Property, e.Message);
                }
                return BadRequest(ModelState);
            }
        }
    }
}
