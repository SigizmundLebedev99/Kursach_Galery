using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.CommentDTO;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.Infrostructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galery.Server.Interfaces
{
    public interface IPictureService
    {
        Task<PictureInfoDTO> CreatePictureAsync(CreatePictureDTO model);
        Task<OperationResult<PictureInfoDTO>> UpdatePictureAsync(int id, CreatePictureDTO model);
        Task DeletePictureAsync(int id);
        Task<OperationResult<PictureFullInfoDTO>> GetPictureByIdAsync(int id, int userId);
        Task<PictureFullInfoDTO> GetPictureByIdAnonimousAsync(int id);
        Task<IEnumerable<PictureInfoDTO>> GetLikedByUserAsync(int userId, int? skip, int? take);
        Task<IEnumerable<Picture>> GetByUserAsync(int userId, int? skip, int? take);
        Task<IEnumerable<PictureInfoWithFeedbackDTO>> GetTopPicturesAsync(int? skip, int? take);

        Task SetLikeAsync(int userId, int pictureId);
        Task RemoveLikeAsync(int userId, int pictureId);

        Task<IEnumerable<PictureInfoDTO>> GetPictursByTag(int tagId, int? skip, int? take);
    }
}
