using Galery.Common.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Galery.ClientLogic
{
    interface ISubscribeClient
    {
        Task<HttpResponseMessage> GetUserInfo(int userId);

        Task<HttpResponseMessage> Subscribing(int toId);

        Task<HttpResponseMessage> Desubscribing(int toId);

        Task<HttpResponseMessage> GetSubscribes(int userId);

        Task<HttpResponseMessage> GetSubscribers(int userId);

        Task<HttpResponseMessage> UserSearch(string search);
    }
}
