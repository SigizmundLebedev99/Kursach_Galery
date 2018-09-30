using Galery.Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.DAL.Repository.Interfaces
{
    public interface IPictureRepository : IRepository<Picture>
    {
        Task<IEnumerable<Picture>> GetByAuthorAsync(int authorId, int? skip, int? take);
        Task<IEnumerable<Picture>> GetLikedByUserAsync(int userId, int? skip, int? take);
        Task PushLike(PictureLikes like);
        Task<int> GetLikesCount(int pictureId);
    }
}
