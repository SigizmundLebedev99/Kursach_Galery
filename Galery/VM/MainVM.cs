﻿using Galery.Pages;
using Galery.VM.Helpers;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Galery.App;

namespace Galery.VM
{
    class MainVM : BaseVM
    {
        public CommonVM CommonVM { get; }
        public MenuItem[] MenuItems { get; private set; }

        public LoginVM LoginVM { get; }
        public bool LoginFlip { get; private set; } = false;

        public event Action ContentChanged;

        public MainVM()
        {
            AnonimousUserMenu();
            CommonVM = new CommonVM(this);
            LoggedIn += (userId, isAdmin) => 
            {
                if (isAdmin)
                {
                    CommonVM.CurrentRole = Roles.Admin;
                    AdminMenu();
                }
                else
                {
                    CommonVM.CurrentRole = Roles.Authorized;
                    AuthorizedUserMenu(userId);
                }
                Content = MenuItems[0].GetContent;
                OnPropertyChanged("CurrentUser");
                OnPropertyChanged("MenuItems");
            };
            LoginVM = new LoginVM(this);
            Content = MenuItems[0].GetContent;
        }

        

        public void FlipLoginPlateBack()
        {
            LoginFlip = false;
            OnPropertyChanged("LoginFlip");
        }

        private FrameworkElement _content;

        public FrameworkElement Content {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged();
                ContentChanged?.Invoke();
            }
        }

        public UserVM CurrentUser
        {
            get
            {
                if (User == null)
                    return new UserVM("Вы не авторизованы", new PackIcon()
                    {
                        Kind = PackIconKind.AccountCircle,
                          Height = 160,
                          Width = 160,
                          VerticalAlignment = VerticalAlignment.Center,
                          HorizontalAlignment = HorizontalAlignment.Center
                    });
                if(User.Avatar == null)
                    return new UserVM(User.Username, new PackIcon()
                    {
                        Kind = PackIconKind.AccountBox,
                        Height = 160,
                        Width = 160,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    });
                else
                {
                    return new UserVM(User.Username, new Image
                    {
                        Height = 160,
                        Width = 160,
                        Stretch = Stretch.UniformToFill,
                        Source = (ImageSource)new ImageSourceConverter().ConvertFromString(ServerAdress + User.Avatar)
                    });
                }
            }
        }

        public void SetPassFunc(Func<string> getPass, Action cleanPass)
        {
            LoginVM.GetPassword = getPass;
            LoginVM.CleanPassword = cleanPass;
        }

        private void AuthorizedUserMenu(int userId)
        {
            MenuItems = new MenuItem[]
            {
                new MenuItem("Моя галерея",()=>new UserInfo(), ()=>new UserInfoVM(this, userId, Roles.Authorized)),
                new MenuItem("Новые",()=>new PictureList(), ()=>new PicturesFromSubscribesVM(User.UserId, this)),
                new MenuItem("Топ лучших",()=>new TopPictures(),()=>new TopPicturesVM(this)),
                new MenuItem("Понравившиеся",()=>new PictureList(), ()=>new LikedPictureListVM(User.UserId, this)),
                new MenuItem("Авторы",()=>new Authors(), ()=>new AuthorsVM(this)),
                new MenuItem("Жанры",()=>new TagList(), ()=>new TagListVM(this)),
                new MenuItem("Вся база",()=>new PictureList(), ()=>new AllPictureListVM(this))
            };
        }

        private void AnonimousUserMenu()
        {
            MenuItems = new MenuItem[]
            {
                new MenuItem("Топ лучших",()=>new TopPictures(),()=>new TopPicturesVM(this)),
                new MenuItem("Авторы",()=>new Authors(), ()=>new AuthorsVM(this)),
                new MenuItem("Жанры",()=>new TagList(), ()=>new TagListVM(this)),
                new MenuItem("Вся база",()=>new PictureList(), ()=>new AllPictureListVM(this))
            };
        }

        private void AdminMenu()
        {
            MenuItems = new MenuItem[]
            {
                new MenuItem("Топ лучших",()=>new TopPictures(),()=>new TopPicturesVM(this)),
                new MenuItem("Авторы", ()=>new Authors(), ()=>new AuthorsVM(this)),
                new MenuItem("Жанры",()=>new TagList(), ()=>new TagListVM(this)),
                new MenuItem("Вся база",()=>new PictureList(), ()=>new AllPictureListVM(this))
            };
        }
    }
}
