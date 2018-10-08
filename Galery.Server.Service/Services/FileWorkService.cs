using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Services
{
    public class FileWorkService : IFileWorkService
    {
        readonly IHostingEnvironment _hosting;

        public FileWorkService(IHostingEnvironment hosting)
        {
            _hosting = hosting;
        }

        public bool RemoveFile(string path)
        {
            try
            {
                FileInfo file = new FileInfo(_hosting.WebRootPath + path);
                if (file.Exists)
                    file.Delete();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsExist(string path)
        {
            return File.Exists(_hosting.WebRootPath + path);
        }

        public Task<string> SaveAvatar(IFormFile file)
        {
            return Task.Run(() =>
            {
                if (file == null || string.IsNullOrEmpty(_hosting.WebRootPath))
                    return null;


                Image<Rgba32> src = Image.Load(file.OpenReadStream());

                Image<Rgba32> image = src.Clone(x => x.Resize(new Size(128, 128)));


                string hash = GetHashFromFile(image.SavePixelData());


                string dir1 = _hosting.WebRootPath + "/Files/Avatars/" + hash.Substring(0, 2);
                string dir2 = $"{dir1}/{hash.Substring(2, 2)}/";


                if (!Directory.Exists(dir1))
                {
                    Directory.CreateDirectory(dir1);
                    Directory.CreateDirectory(dir2);
                }
                else if (!Directory.Exists(dir2))
                    Directory.CreateDirectory(dir2);


                string result = dir2 + file.FileName;
                image.Save(result);



                var res = result;
                return res.Replace(_hosting.WebRootPath, "");
            });
        }

        private string GetHashFromFile(byte[] bytes)
        {
            var hash = SHA1.Create().ComputeHash(bytes);
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        public Task<string> SavePicture(IFormFile file)
        {
            return Task.Run(() =>
            {
                if (file == null || string.IsNullOrEmpty(_hosting.WebRootPath))
                    return null;


                Image<Rgba32> image = Image.Load(file.OpenReadStream());


                string hash = GetHashFromFile(image.SavePixelData());


                string dir1 = _hosting.WebRootPath + "/Files/Pictures/" + hash.Substring(0, 2);
                string dir2 = $"{dir1}/{hash.Substring(2, 2)}/";


                if (!Directory.Exists(dir1))
                {
                    Directory.CreateDirectory(dir1);
                    Directory.CreateDirectory(dir2);
                }
                else if (!Directory.Exists(dir2))
                    Directory.CreateDirectory(dir2);


                string result = dir2 + file.FileName;
                image.Save(result);

                var res = result;
                return res.Replace(_hosting.WebRootPath, "");
            });
        }
    }
}
