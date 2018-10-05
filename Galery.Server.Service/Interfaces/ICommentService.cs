using Galery.Server.Service.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Interfaces
{
    public interface ICommentService
    {
        Task<CommentInfoDTO> CreateCommentAsync { get; set; }
        Task<CommentInfoDTO> UpdateCommentAsync { get; set; }
        Task DeleteCommentAsync { get; set; }
    }
}
