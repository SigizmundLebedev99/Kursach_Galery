using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galery.Server.DAL.Models;
using Galery.Server.Interfaces;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.Infrostructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Galery.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/picture")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PictureController : Controller
    {
        readonly IPictureService _service;
        readonly UserManager<User> _userManager;

        public PictureController(IPictureService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        /// <summary>
        /// Create new picture
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PictureInfoDTO), 200)]
        public async Task<IActionResult> CreatePicture([FromBody]CreatePictureDTO model)
        {
            var result = await _service.CreatePictureAsync(model);
            return this.GetResult(result, true);
        }

        /// <summary>
        /// Update picture info
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PictureInfoDTO), 200)]
        public async Task<IActionResult> UpdatePicture(int id, [FromBody]CreatePictureDTO model)
        {
            var result = await _service.UpdatePictureAsync(id, model);
            return this.GetResult(result, true);
        }

        /// <summary>
        /// Delete picture
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            await _service.DeletePictureAsync(id);
            return Ok();
        }

        /// <summary>
        /// Get full info about picture by id for authentificated user
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PictureFullInfoDTO), 200)]
        public async Task<IActionResult> GetPictureById(int id)
        {
            var userId = User.GetUserId();
            var res = await _service.GetPictureByIdAsync(id, userId);
            return this.GetResult(res, true);
        }

        /// <summary>
        /// Get full info about picture by id for anon
        /// </summary>
        [HttpGet("anon/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PictureFullInfoDTO), 200)]
        public async Task<IActionResult> GetPictureByIdAnonimous(int id)
        {
            var res = await _service.GetPictureByIdAnonimousAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get user's liked pictures
        /// </summary>
        [HttpGet("liked/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<PictureInfoDTO>), 200)]
        public async Task<IActionResult> GetLikedByUser([FromQuery]int skip = 0, [FromQuery]int take = 50)
        {
           var userId = User.GetUserId();
            var res = await _service.GetLikedByUserAsync(userId, skip, take);
            return Ok(res);
        }

        /// <summary>
        /// Get user's pictures
        /// </summary>
        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<Picture>), 200)]
        public async Task<IActionResult> GetByUser(int userId, [FromQuery]int skip = 0, [FromQuery]int take = 50)
        {
            var res = await _service.GetByUserAsync(userId, skip, take);
            return Ok(res);
        }

        /// <summary>
        /// Get pictures with top likes count
        /// </summary>
        [HttpGet("top")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<PictureInfoWithFeedbackDTO>), 200)]
        public async Task<IActionResult> GetTopPictures([FromQuery]int skip = 0, [FromQuery]int take = 50)
        {
            var res = await _service.GetTopPicturesAsync(skip, take);
            return Ok(res);
        }

        /// <summary>
        /// Create like by user to picture
        /// </summary>
        
        [HttpPost("like/{pictureId}")]
        public async Task<IActionResult> SetLike(int pictureId)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
           var userId = User.GetUserId();
            await _service.SetLikeAsync(userId, pictureId);
            return Ok();
        }

        /// <summary>
        /// Delete like by user to picture
        /// </summary>
        [HttpDelete("like/{pictureId}")]
        public async Task<IActionResult> RemoveLikeAsync(int pictureId)
        {
           var userId = User.GetUserId();
            await _service.RemoveLikeAsync(userId, pictureId);
            return Ok();
        }

        /// <summary>
        /// Get pictures by tag
        /// </summary>
        [HttpGet("tag/{tagId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<PictureInfoDTO>), 200)]
        public async Task<IActionResult> GetPicturesByTag(int tagId, [FromQuery]int skip = 0, [FromQuery]int take = 50)
        {
            var res = await _service.GetPicturesByTagAsync(tagId, skip, take);
            return Ok(res);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PictureInfoDTO>), 200)]
        public async Task<IActionResult> GetNewPictures([FromQuery]int skip = 0, [FromQuery]int take = 50)
        {
            var res = await _service.GetNewPicturesAsync(skip, take);
            return Ok(res);
        }
        
        [HttpGet("fromSubscribes")]       
        [ProducesResponseType(typeof(IEnumerable<PictureInfoDTO>), 200)]
        public async Task<IActionResult> GetNewPicturesFromSubscribes([FromQuery]int skip = 0, [FromQuery]int take = 50)
        {
           var userId = User.GetUserId();
            var res = await _service.GetPicsFromSubscribes(userId, skip, take);
            return Ok(res);
        }        
    }
}
