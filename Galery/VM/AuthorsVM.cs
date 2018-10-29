using Galery.Common.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Galery.VM
{
    class AuthorsVM : BaseWithCommonVM
    {
        private Visibility loadingVisibility = Visibility.Collapsed;
        public Visibility LoadingVisibility { get => loadingVisibility; private set
            {
                loadingVisibility = value;
                OnPropertyChanged();
            }
        }


        public AuthorsVM(MainVM mainVM) : base(mainVM) {}

        public IEnumerable<UserDTO> Users { get; private set; }

        public ICommand Search
        {
            get
            {
                return new DelegateCommand
                    (async obj=> 
                    {
                        LoadingVisibility = Visibility.Visible;
                        string search = (string)obj;
                        var res = await App.ClientService.Subscribe.UserSearch(search);
                        if (res.IsSuccessStatusCode)
                        {
                            Users = await res.Content.ReadAsAsync<IEnumerable<UserDTO>>();
                            OnPropertyChanged("Users");
                        }
                        else
                        {
                            await App.ShowErrorMessage("Что-то пошло не так\n" + res.ReasonPhrase);
                        }
                        LoadingVisibility = Visibility.Collapsed;
                    },
                    obj=>!string.IsNullOrWhiteSpace((string)obj));
            }
        }
    }
}
