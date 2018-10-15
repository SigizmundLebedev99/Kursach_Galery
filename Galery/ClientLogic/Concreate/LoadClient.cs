using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Galery.ClientLogic.Concreate
{
    class LoadClient : ILoadClient
    {
        readonly string _hostAdress;
        readonly HttpClient _client;

        public LoadClient(string hostAdress, HttpClient client)
        {
            _hostAdress = hostAdress;
            _client = client;
        }

        public Task<HttpResponseMessage> LoadAvatar(byte[] file, string name)
        {
            var avatar = new ByteArrayContent(file);
            return _client.PostAsync("/api/load/avatar", new MultipartFormDataContent
            {
                {avatar, "file", name}
            });
        }

        public Task<HttpResponseMessage> LoadImage(byte[] file, string name)
        {
            var avatar = new ByteArrayContent(file);
            return _client.PostAsync("/api/load/image", new MultipartFormDataContent
            {
                {avatar, "file", name}
            });
        }
    }
}
