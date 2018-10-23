using Galery.Server.Service.DTO.PictureDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Galery.VM
{
    class TopPicturesVM : BaseWithCommonVM
    {
        private event Action OnLoaded;
        public TopPicturesVM(MainVM mainVM) : base(mainVM)
        {
            OnLoaded += async () => await LoadData();
            OnLoaded();
        }

        public Visibility LoadingVizibility { get; private set; } = Visibility.Visible;

        public List<PictureInfoWithFeedbackDTO> PicturesTop { get; private set; }
        
        public ICommand someShit
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    App.ShowErrorMessage("proverka");
                });
            }
        }

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
