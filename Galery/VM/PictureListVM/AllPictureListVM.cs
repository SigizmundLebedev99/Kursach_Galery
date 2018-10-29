using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Galery.VM
{
    class AllPictureListVM : BasePictureListVM
    {
        public AllPictureListVM(MainVM mainVM) : base(mainVM)
        {
            Header = "Вся база";
            LoadData();
        }

        protected override Func<Task<HttpResponseMessage>> LoadAction =>
            () => App.ClientService.Picture.GetNewPictures(0, PAGE_LENGTH);

        protected override Func<Task<HttpResponseMessage>> UploadAction =>
            () => App.ClientService.Picture.GetNewPictures(Skip, PAGE_LENGTH);
    }
}
