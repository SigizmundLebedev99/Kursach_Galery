using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Galery.Server.Service.DTO.CommentDTO;

namespace Galery.ClientLogic.Concreate
{
    class CommentClient : ICommentClient
    {
        readonly string _hostAdress;
        readonly HttpClient _client;

        public CommentClient(string hostAdress, HttpClient client)
        {
            _hostAdress = hostAdress;
            _client = client;
        }

        public Task<HttpResponseMessage> CreateComment(CreateCommentDTO model)
        {
            return _client.PostAsJsonAsync("/api/Comment", model);
        }

        public Task<HttpResponseMessage> DeleteComment(int id)
        {
            return _client.DeleteAsync($"/api/Comment/{id}");
        }
    }
}
