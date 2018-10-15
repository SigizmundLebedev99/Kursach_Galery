using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Galery.Server.DTO;
using Galery.Server.DTO.User;
using Galery.Server.Service.DTO.User;

namespace Galery.ClientLogic.Concreate
{
    class AccountClient : IAccountClient
    {
        readonly string _hostAdress;
        readonly HttpClient _client;

        public AccountClient(string hostAdress, HttpClient client)
        {
            _hostAdress = hostAdress;
            _client = client;
        }

        public Task<HttpResponseMessage> CreateUser(CreateUserDTO model)
        {
            return _client.PostAsJsonAsync("/api/Account", model);
        }

        public Task<HttpResponseMessage> Token(LoginDTO model)
        {
            return _client.PostAsJsonAsync("/api/Account/token", model);
        }
    }
}
