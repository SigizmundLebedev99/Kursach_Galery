using Galery.Server.Service.DTO.PictureDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Galery.VM
{
    abstract class BasePictureListVM : BasePageVM
    {
        protected const int PAGE_LENGTH = 50;

        public BasePictureListVM(MainVM mainVM) : base(mainVM)
        {}

        private IEnumerable<PictureInfoDTO> picList;

        public IEnumerable<PictureInfoDTO> PicturesList { get => picList;
            protected set
            {
                picList = value;
                Skip = picList.Count();
                OnPropertyChanged();
            }
        }

        private Visibility loadingVis;
        public Visibility LoadingVisibility { get => loadingVis; protected set { loadingVis = value; OnPropertyChanged(); } }

        private bool isUploadingEnabled;
        public bool IsUploadingEnable { get => isUploadingEnabled; protected set { isUploadingEnabled = value; OnPropertyChanged(); } }

        private string header;

        public string Header { get=>header; protected set { header = value; OnPropertyChanged(); } }

        protected int Skip;

        public ICommand LoadMore
        {
            get
            {
                return new DelegateCommand(async obj=> 
                {
                    var newData = await LoadMoreData();
                    int count = newData.Count();
                    if (newData == null || count == 0)
                        return;
                    if (count < PAGE_LENGTH)
                        isUploadingEnabled = false;
                    PicturesList = PicturesList.Concat(newData);
                    OnPropertyChanged("PicturesList");
                });
            }
        }

        protected async Task<IEnumerable<PictureInfoDTO>> LoadMoreData()
        {
            var res = await UploadAction();
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadAsAsync<IEnumerable<PictureInfoDTO>>();
            }
            await App.ShowErrorMessage("Не удалось извлечь данные\n" + res.ReasonPhrase);
            return null;
        }

        protected async void LoadData()
        {
            var res = await LoadAction();
            if (res.IsSuccessStatusCode)
            {
                LoadingVisibility = Visibility.Collapsed;
                PicturesList = await res.Content.ReadAsAsync<List<PictureInfoDTO>>();
            }
            else
            {
                await App.ShowErrorMessage("Не удалось извлечь данные\n" + res.ReasonPhrase);
            }
        }

        protected abstract Func<Task<HttpResponseMessage>> LoadAction { get; }
        protected abstract Func<Task<HttpResponseMessage>> UploadAction { get; }
    }
}
