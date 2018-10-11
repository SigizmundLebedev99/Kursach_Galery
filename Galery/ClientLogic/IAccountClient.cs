using Galery.Server.DTO.User;
using Galery.Server.Service.DTO.User;
using System.Net.Http;
using System.Threading.Tasks;

namespace Galery.ClientLogic
{
    interface IAccountClient
    {
        Task<HttpResponseMessage> Token(LoginDTO model);
        Task<HttpResponseMessage> CreateUser(CreateUserDTO model);
    }
}
