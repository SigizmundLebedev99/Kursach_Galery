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
    class PictureListForTagVM : BasePageVM
    {
        public List<PictureInfoDTO> PicturesList { get; private set; }

        public Visibility LoadingVisibility { get; private set; } = Visibility.Visible;

        public string Header { get; private set; }

        public PictureListForTagVM(MainVM mainVM, Tag tag) : base(mainVM)
        {
            LoadData(tag.Id);
        }

        async Task LoadData(int tagId)
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
