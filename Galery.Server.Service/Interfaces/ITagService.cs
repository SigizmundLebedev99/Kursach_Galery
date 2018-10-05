using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.PictureDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Interfaces
{
    public interface ITagService
    {
        Task<Tag> CreateTag(string name);
        Task<IEnumerable<TagDTO>> GetAllTags();
        Task<Tag> UpdateTag(int id, string name);
        Task DeleteTag(int id);
    }
}
