using Galery.Server.Service.DTO.PictureDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galery.Server.Interfaces
{
    public interface IPictureService
    {
        Task<PictureInfoDTO> CreatePictureAsync(CreatePictureDTO model);
        Task<PictureInfoDTO> UpdatePictureAsync(int id, CreatePictureDTO model);
        Task DeletePictureAsync(int id);
        Task<PictureFullInfoDTO> GetPictureByIdAsync(int id);
        Task<IEnumerable<PictureInfoDTO>> GetLikedByUserAsync(int userId);
        Task<IEnumerable<PictureInfoWithFeedbackDTO>> GetTopPicturesAsync(int? skip, int? take);
    }
}
