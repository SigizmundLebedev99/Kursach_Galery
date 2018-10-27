using Galery.Common.DTO.User;
using Galery.Pages;
using Galery.Resources;
using Galery.VM.Helpers;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Galery.VM
{
    class UserInfoVM : BasePageVM
    {
        public UserInfoVM(MainVM mainVM, int userId, Roles role) : base(mainVM)
        {
            if (role == Roles.Unauthorized)
            {
                SubscribeVisibility = Visibility.Collapsed;
                AddPicVisibility = Visibility.Collapsed;
            }
            else if(role == Roles.Authorized)
            {
                if (userId == App.User.UserId)
                {
                    SubscribeVisibility = Visibility.Collapsed;
                    AddPicVisibility = Visibility.Visible;
                }
                else
                {
                    SubscribeVisibility = Visibility.Visible;
                    AddPicVisibility = Visibility.Collapsed;
                }
            }
            MenuItems = new MenuItem[]
                {
                    new MenuItem("Галерея", ()=>new UserPicturesList(), ()=>new UserPicturesListVM(userId, mainVM)),
                    new MenuItem("Подписчики", ()=>new UserList(), ()=>new SubscribersVM(userId, mainVM)),
                    new MenuItem("Подписки", ()=> new UserList(), ()=>new SubscribesVM(userId, mainVM))
                };
            LoadData(userId, role);
        }

        public MenuItem[] MenuItems { get; set; }

        public Visibility SubscribeVisibility { get; private set; }
        public Visibility AddPicVisibility { get; private set; }
        public Visibility LoadingVizibility { get; private set; }

        public object IconContent
        {
            get
            {
                if (User == null || string.IsNullOrEmpty(User.Avatar))
                    return new PackIcon()
                    {
                        Kind = PackIconKind.AccountBox,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                else
                    return new Image
                    {
                        Stretch = Stretch.Uniform,
                        Source = (ImageSource)new ImageSourceConverter().ConvertFromString(App.ServerAdress + User.Avatar)
                    };
            }
        }

        public ICommand CreatePicture
        {
            get
            {
                return new DelegateCommand(
                    obj=> 
                    {
                        var createPicPage = new CreatePicture(User.Id, MainVM);
                        ((CreatePictureVM)createPicPage.DataContext).PictureCreated += (p) =>
                        {
                            OnPropertyChanged("MenuItems");
                        };
                        MainVM.Content = createPicPage;
                    });
            }
        }

        public UserInfoDTO User { get; private set; }

        private async Task LoadData(int userId, Roles role)
        {
            var res = await App.ClientService.Subscribe.GetUserInfo(userId);
            if (res.IsSuccessStatusCode)
            {
                User = await res.Content.ReadAsAsync<UserInfoDTO>();
                OnPropertyChanged("User");
                LoadingVizibility = Visibility.Collapsed;
                OnPropertyChanged("LoadingVizibility");
                OnPropertyChanged("IconContent");
            }
            else
            {
                await App.ShowErrorMessage("Что-то пошло не так\n" + res.ReasonPhrase);
            }
        } 
    }
}
