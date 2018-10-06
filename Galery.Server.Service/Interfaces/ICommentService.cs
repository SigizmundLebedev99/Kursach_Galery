using Galery.Server.Service.DTO.CommentDTO;
using Galery.Server.Service.Infrostructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Interfaces
{
    public interface ICommentService
    {
        Task<OperationResult<CommentInfoDTO>> CreateCommentAsync(CreateCommentDTO model);
        Task<OperationResult<CommentInfoDTO>> UpdateCommentAsync(int id, CreateCommentDTO model);
        Task DeleteCommentAsync(int id);
    }
}
