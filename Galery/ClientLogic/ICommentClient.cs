using Galery.Server.Service.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Galery.ClientLogic
{
    interface ICommentClient
    {
        Task<HttpResponseMessage> CreateComment(CreateCommentDTO model);
        Task<HttpResponseMessage> DeleteComment(int id);
    }
}
