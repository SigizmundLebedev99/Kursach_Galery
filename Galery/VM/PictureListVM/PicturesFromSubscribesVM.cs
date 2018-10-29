using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Galery.Server.Service.DTO.PictureDTO;

namespace Galery.VM
{
    class PicturesFromSubscribesVM : BasePictureListVM
    {
        private readonly int userId;

        public PicturesFromSubscribesVM(int userId, MainVM mainVM) : base(mainVM)
        {
            this.userId = userId;
            LoadData();
        }

        protected override Func<Task<HttpResponseMessage>> LoadAction =>
           () => App.ClientService.Picture.GetNewPicturesFromSubscribes(userId, 0, PAGE_LENGTH);

        protected override Func<Task<HttpResponseMessage>> UploadAction =>
            () => App.ClientService.Picture.GetNewPicturesFromSubscribes(userId, Skip, PAGE_LENGTH);
    }
}
