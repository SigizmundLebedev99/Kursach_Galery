﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Galery.Server.Service.DTO.PictureDTO;

namespace Galery.ClientLogic.Concreate
{
    class PictureClient : IPictureClient
    {
        readonly string _hostAdress;
        readonly HttpClient _client;

        public PictureClient(string hostAdress, HttpClient client)
        {
            _hostAdress = hostAdress;
            _client = client;
        }

        public Task<HttpResponseMessage> CreatePicture(CreatePictureDTO model)
        {
            return _client.PostAsJsonAsync("/api/picture", model);
        }

        public Task<HttpResponseMessage> DeletePicture(int id)
        {
            return _client.DeleteAsync($"/api/picture/{id}");
        }

        public Task<HttpResponseMessage> GetByUser(int user, int? skip, int? take)
        {
            return _client.GetAsync($"/api/picture/user/{user}" + SkipTakeQuery(skip,take));
        }

        public Task<HttpResponseMessage> GetLikedByUser(int userId, int? skip, int? take)
        {
            return _client.GetAsync($"/api/picture/liked/{userId}" + SkipTakeQuery(skip, take));
        }

        public Task<HttpResponseMessage> GetNewPictures(int? skip, int? take)
        {
            return _client.GetAsync($"/api/picture" + SkipTakeQuery(skip, take));
        }

        public Task<HttpResponseMessage> GetNewPicturesFromSubscribes(int userId, int? skip, int? take)
        {
            return _client.GetAsync($"/api/picture/fromSubscribes" + SkipTakeQuery(skip, take));
        }

        public Task<HttpResponseMessage> GetPictureById(int id)
        {
            return _client.GetAsync($"/api/picture/{id}");
        }

        public Task<HttpResponseMessage> GetPicturesByTag(int tagId, int? skip, int? take)
        {
            return _client.GetAsync($"/api/picture/tag/{tagId}" + SkipTakeQuery(skip, take));
        }

        public Task<HttpResponseMessage> GetTopPictures(int? skip, int? take)
        {
            return _client.GetAsync($"/api/picture/top" + SkipTakeQuery(skip, take));
        }

        public Task<HttpResponseMessage> RemoveLike(int pictureId)
        {
            return _client.DeleteAsync($"/api/picture/like/{pictureId}");
        }

        public Task<HttpResponseMessage> SetLike(int pictureId)
        {
            return _client.PostAsync($"/api/picture/like/{pictureId}", null);
        }

        public Task<HttpResponseMessage> UpdatePicture(int id, CreatePictureDTO model)
        {
            return _client.PutAsJsonAsync($"/api/picture/{id}", model);
        }

        private string SkipTakeQuery(int? skip, int? take)
        {
            var path = new StringBuilder();
            if (skip.HasValue)
            {
                path.Append($"?skip={skip.Value}");
                if (take.HasValue)
                    path.Append($"&take={take.Value}");
            }
            else if (take.HasValue)
                path.Append($"?take={take.Value}");
            return path.ToString();
        }
    }
}
