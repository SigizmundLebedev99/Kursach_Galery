﻿using Galery.Common.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Galery.ClientLogic
{
    interface ISubscribeClient
    {
        Task<HttpResponseMessage> GetUserInfo(int userId);

        Task<HttpResponseMessage> Subscribing(Subscribe model);

        Task<HttpResponseMessage> Desubscribing(Subscribe model);

        Task<HttpResponseMessage> GetSubscribes(int userId);

        Task<HttpResponseMessage> GetSubscribers(int userId);
    }
}
