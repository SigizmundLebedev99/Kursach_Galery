using Galery.Common.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Galery.ClientLogic.Concreate
{
    class SubscribeClient : ISubscribeClient
    {
        readonly string _hostAdress;
        readonly HttpClient _client;

        public SubscribeClient(string hostAdress, HttpClient client)
        {
            _hostAdress = hostAdress;
            _client = client;
        }

        public Task<HttpResponseMessage> Desubscribing(int toId)
        {            
            return _client.DeleteAsync($"/api/Subscribe/{toId}");
        }

        public Task<HttpResponseMessage> GetSubscribers(int userId)
        {
            return _client.GetAsync($"/api/Subscribe/to/{userId}");
        }

        public Task<HttpResponseMessage> GetSubscribes(int userId)
        {
            return _client.GetAsync($"/api/Subscribe/from/{userId}");
        }

        public Task<HttpResponseMessage> GetUserInfo(int userId)
        {
            return _client.GetAsync($"/api/Subscribe/userinfo/{userId}");
        }

        public Task<HttpResponseMessage> Subscribing(int toId)
        {
            return _client.PostAsJsonAsync<object>($"/api/Subscribe/{toId}", null);
        }

        public Task<HttpResponseMessage> UserSearch(string search)
        {
            return _client.GetAsync($"/api/Subscribe/search/{search}");
        }
    }
}
