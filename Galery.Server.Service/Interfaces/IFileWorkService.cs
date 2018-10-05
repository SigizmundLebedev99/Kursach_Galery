using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Interfaces
{
    public interface IFileWorkService
    {
        Task<string> SaveAvatar(IFormFile file);
        Task<string> SavePicture(IFormFile file);
        bool RemoveFile(string path);
    }
}
