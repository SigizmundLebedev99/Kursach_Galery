using Galery.Server.Service.DTO.PictureDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Galery.ClientLogic
{
    interface IPictureClient
    {
        Task<HttpResponseMessage> CreatePicture(CreatePictureDTO model);

        Task<HttpResponseMessage> UpdatePicture(int id, CreatePictureDTO model);

        Task<HttpResponseMessage> DeletePicture(int id);

        Task<HttpResponseMessage> GetPictureById(int id);

        Task<HttpResponseMessage> GetPictureByIdAnon(int id);

        Task<HttpResponseMessage> GetLikedByUser(int userId, int? skip, int? take);

        Task<HttpResponseMessage> GetByUser(int user, int? skip, int? take);

        Task<HttpResponseMessage> GetTopPictures(int? skip, int? take);

        Task<HttpResponseMessage> SetLike(int userId, int pictureId);

        Task<HttpResponseMessage> RemoveLike(int userId, int pictureId);

        Task<HttpResponseMessage> GetPicturesByTag(int tagId, int? skip, int? take);

        Task<HttpResponseMessage> GetNewPictures(int? skip, int? take);

        Task<HttpResponseMessage> GetNewPicturesFromSubscribes(int userId, int? skip, int? take);
    }
}
