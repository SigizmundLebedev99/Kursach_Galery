using Galery.Common.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Galery.VM
{
    class SubscribersVM : BaseWithCommonVM
    {
        public Visibility LoadingVisibility { get; private set; } = Visibility.Visible;

        public List<UserDTO> Users { get; set; }

        private event Action<int> onLoad;

        public SubscribersVM(int userId, MainVM mainVM) : base(mainVM)
        {
            LoadData(userId);
        }

        async void LoadData(int userId)
        {
            var res = await App.ClientService.Subscribe.GetSubscribers(userId);
            if (res.IsSuccessStatusCode)
            {
                Users = (await res.Content.ReadAsAsync<UserDTO[]>()).ToList();
                OnPropertyChanged("Users");
                LoadingVisibility = Visibility.Collapsed;
                OnPropertyChanged("LoadingVisibility");
            }
            else
                await App.ShowErrorMessage("Что-то пошло не так");
        }
    }
}
