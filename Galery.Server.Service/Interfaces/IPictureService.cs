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
        Task<IEnumerable<PictureInfoDTO>> GetLikedByUserAsync(int userId);
    }
}
