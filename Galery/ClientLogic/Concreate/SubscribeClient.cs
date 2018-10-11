﻿using Galery.Common.Models;
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

        public Task<HttpResponseMessage> Desubscribing(Subscribe model)
        {
            HttpRequestMessage mess = new HttpRequestMessage
            {
                Content = new StringContent("", Encoding.UTF8, JsonConvert.SerializeObject(model)),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_hostAdress + "/api/Subscribe")
            };

            return _client.SendAsync(mess);
        }

        public Task<HttpResponseMessage> GetSubscribers(int userId)
        {
            return _client.GetAsync(_hostAdress + $"/api/Subscribe/to/{userId}");
        }

        public Task<HttpResponseMessage> GetSubscribes(int userId)
        {
            return _client.GetAsync(_hostAdress + $"/api/Subscribe/from/{userId}");
        }

        public Task<HttpResponseMessage> GetUserInfo(int userId)
        {
            return _client.GetAsync(_hostAdress + $"/api/Subscribe/userinfo/{userId}");
        }

        public Task<HttpResponseMessage> Subscribing(Subscribe model)
        {
            return _client.PostAsJsonAsync(_hostAdress + $"/api/Subscribe", model);
        }
    }
}