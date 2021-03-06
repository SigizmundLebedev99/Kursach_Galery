﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Galery.Server.DTO;

namespace Galery.ClientLogic.Concreate
{
    class ClientService : IClientService
    {
        readonly HttpClient _httpClient;

        public ClientService(string hostAdress)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostAdress);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Account = new AccountClient(hostAdress, _httpClient);
            Load = new LoadClient(hostAdress, _httpClient);
            Picture = new PictureClient(hostAdress, _httpClient);
            Comment = new CommentClient(hostAdress, _httpClient);
            Subscribe = new SubscribeClient(hostAdress, _httpClient);
            Tag = new TagClient(hostAdress, _httpClient);
        }

        public IAccountClient Account { get; set; }
        public ILoadClient Load { get; set; }
        public IPictureClient Picture { get; set; }
        public ICommentClient Comment { get; set; }
        public ISubscribeClient Subscribe { get; set; }
        public ITagClient Tag { get; set; }

        public void SetAutenticationHeader(TokenResponse token)
        {
            if (_httpClient.DefaultRequestHeaders.Any(h => h.Key == "Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Access_token}");
            }
            else
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Access_token}");
        }
    }
}
