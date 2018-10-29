using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Galery.Server.Service.DTO.PictureDTO;

namespace Galery.VM
{
    class LikedPictureListVM : BasePictureListVM
    {
        private readonly int userId;

        public LikedPictureListVM(int userId, MainVM mainVm) : base(mainVm)
        {
            this.userId = userId;
            Header = "Понравившиеся";
            LoadData();
        }

        protected override Func<Task<HttpResponseMessage>> LoadAction =>
            () => App.ClientService.Picture.GetLikedByUser(userId, 0, PAGE_LENGTH);

        protected override Func<Task<HttpResponseMessage>> UploadAction =>
            () => App.ClientService.Picture.GetLikedByUser(userId, Skip, PAGE_LENGTH);
    }
}
