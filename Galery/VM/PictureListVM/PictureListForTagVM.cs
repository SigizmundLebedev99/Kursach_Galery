using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.PictureDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Galery.VM
{
    class PictureListForTagVM : BasePictureListVM
    {
        private int _tagId;

        public PictureListForTagVM(MainVM mainVM, Tag tag) : base(mainVM)
        {
            Header = tag.Name;
            OnPropertyChanged("Header");
            _tagId = tag.Id;
            LoadData();
        }

        protected override Func<Task<HttpResponseMessage>> LoadAction => 
            ()=>App.ClientService.Picture.GetPicturesByTag(_tagId, 0, PAGE_LENGTH);

        protected override Func<Task<HttpResponseMessage>> UploadAction => 
            ()=>App.ClientService.Picture.GetPicturesByTag(_tagId, Skip, PAGE_LENGTH);
    }
}
