using Galery.Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Galery.VM
{
    class UserPicturesListVM : BaseWithCommonVM
    {
        public Visibility LoadingVizibility { get; private set; }

        public List<Picture> Pictures { get; private set; }

        public UserPicturesListVM(int userId, MainVM mainVM) : base(mainVM) 
        {
            
            LoadingVizibility = Visibility.Visible;
            OnPropertyChanged("LoadingVizibility");
            LoadData(userId);
        }

        public async void LoadData(int userId)
        {
            var res = await App.ClientService.Picture.GetByUser(userId, 0, 200);
            if (res.IsSuccessStatusCode)
            {
                Pictures = (await res.Content.ReadAsAsync<IEnumerable<Picture>>()).ToList();
                OnPropertyChanged("Pictures");
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
