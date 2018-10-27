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

        public PictureListForTagVM(MainVM mainVM, Tag tag) : base(mainVM)
        {
            Header = tag.Name;
            OnPropertyChanged("Header");
            LoadData(tag.Id);
        }

        async void LoadData(int tagId)
        {
            var res = await App.ClientService.Picture.GetPicturesByTag(tagId, 0, 200);
            if (res.IsSuccessStatusCode)
            {
                LoadingVisibility = Visibility.Collapsed;
                PicturesList = await res.Content.ReadAsAsync<List<PictureInfoDTO>>();
                OnPropertyChanged("LoadingVisibility");
                OnPropertyChanged("PicturesList");
            }
            else
            {
                await App.ShowErrorMessage("Не удалось извлечь данные\n" + res.ReasonPhrase);
            }
        }
    }
}
