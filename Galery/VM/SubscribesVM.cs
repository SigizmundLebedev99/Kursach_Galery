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
    class SubscribesVM : BaseWithCommonVM
    {
        public Visibility LoadingVisibility { get; private set; } = Visibility.Visible;

        public List<UserDTO> Users { get; set; }

        private event Action<int> onLoad;

        public SubscribesVM(int userId, MainVM mainVM) : base(mainVM)
        {
            onLoad += async (id) => await LoadData(id);
            onLoad(userId);
        }

        private async Task LoadData(int userId)
        {
            var res = await App.ClientService.Subscribe.GetSubscribes(userId);
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
