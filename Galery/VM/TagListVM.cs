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
    class TagListVM : BaseWithCommonVM
    {
        public TagListVM(MainVM mainVm) : base(mainVm)
        {
            LoadData();
        }

        public Visibility LoadingVisibility { get; private set; } = Visibility.Visible;

        public TagDTO[] TagList { get; private set; }

        public async Task LoadData()
        {
            var res = await App.ClientService.Tag.GetAllTags();
            if (res.IsSuccessStatusCode)
            {
                TagList = await res.Content.ReadAsAsync<TagDTO[]>();
                OnPropertyChanged("TagList");
            }
        }
    }
}
