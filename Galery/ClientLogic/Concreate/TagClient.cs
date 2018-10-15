using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Galery.ClientLogic.Concreate
{
    class TagClient : ITagClient
    {
        readonly string _hostAdress;
        readonly HttpClient _client;

        public TagClient(string hostAdress, HttpClient client)
        {
            _hostAdress = hostAdress;
            _client = client;
        }

        public Task<HttpResponseMessage> CreateTag(string name)
        {
            return _client.PostAsJsonAsync("/api/tag", name);
        }

        public Task<HttpResponseMessage> DeleteTag(int id)
        {
            return _client.DeleteAsync($"/api/tag/{id}");
        }

        public Task<HttpResponseMessage> GetAllTags()
        {
            return _client.GetAsync($"/api/tag");
        }

        public Task<HttpResponseMessage> UpdateTag(int id, string name)
        {
            return _client.PutAsJsonAsync($"/api/tag/{id}", name);
        }
    }
}
