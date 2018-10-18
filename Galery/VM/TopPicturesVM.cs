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
    class TopPicturesVM : BaseVM
    {
        private event Action OnLoaded;
        public TopPicturesVM()
        {
            OnLoaded += async () => await LoadData();
            LoadingVizibility = Visibility.Visible;
            OnLoaded();
        }

        public Visibility LoadingVizibility { get; private set; } = Visibility.Visible;

        public List<PictureInfoWithFeedbackDTO> PicturesTop { get; private set; }
        
        public async Task LoadData()
        {
            var res = await App.ClientService.Picture.GetTopPictures(0, 20);
            if (res.IsSuccessStatusCode)
            {
                PicturesTop = (await res.Content.ReadAsAsync<IEnumerable<PictureInfoWithFeedbackDTO>>()).ToList();
                OnPropertyChanged("PicturesTop");
                LoadingVizibility = Visibility.Collapsed;
                OnPropertyChanged("LoadingVizibility");
            }
            else
            {
                await App.ShowErrorMessage("Не удалось извлечь данные: " + res.ReasonPhrase);
            }
        }
    }
}
