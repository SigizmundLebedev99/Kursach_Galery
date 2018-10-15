using Galery.Server.Service.DTO.User;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Galery.VM
{
    class RegistrationVM : BaseVM
    {
        readonly Style style = Application.Current.FindResource("MaterialDesignCircularProgressBar") as Style;
        readonly Func<string> GetPassword;
        readonly Func<string> GetPassConfirm;
        readonly OpenFileDialog Dialog = new OpenFileDialog
        {
            Filter = ""
        };
        public RegistrationVM(Func<string> getPass, Func<string> getPassConfirmation)
        {
            GetPassword = getPass;
            GetPassConfirm = getPassConfirmation;
        }

        public string Login { get; set; }
        public string Email { get; set; }

        private string passMess = "Пароль";
        public string PasswordMessage { get { return passMess; } set { passMess = value; OnPropertyChanged(); } }
        private string loginMes = "Логин";
        public string LoginMessage { get { return loginMes; } set { loginMes = value; OnPropertyChanged(); } }
        private string confirmMes = "Подтверждение пароля";
        public string ConfirmPasswordMessage { get { return confirmMes; } set { confirmMes = value; OnPropertyChanged(); } }
        private string emailMess = "Email";
        public string EmailMessage { get { return emailMess; } set { emailMess = value; OnPropertyChanged(); } }

        public string Avatar { get; set; }

        public object Content { get; set; } = new PackIcon()
        {
            Kind = PackIconKind.AccountCircle,
            Height = 160,
            Width = 160,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        private bool isLoadImage;

        public ICommand ChooseAvatar
        {
            get
            {
                return new DelegateCommand(async obj =>
                    {
                        bool? dialogRes = Dialog.ShowDialog();
                        if (dialogRes != null && dialogRes.Value)
                        {
                            isLoadImage = true;
                            Content = new ProgressBar
                            {
                                Style = style,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                IsIndeterminate = true,
                                Value = 0
                            };
                            OnPropertyChanged("Content");
                            var res = await App.ClientService.Load.LoadAvatar(File.ReadAllBytes(Dialog.FileName),
                                Path.GetFileName(Dialog.FileName));
                            if (res.IsSuccessStatusCode)
                            {
                                Avatar = await res.Content.ReadAsAsync<string>();
                                Content = new Image
                                {
                                    Height = 160,
                                    Width = 160,
                                    Stretch = Stretch.UniformToFill,
                                    Source = (ImageSource)new ImageSourceConverter().ConvertFromString(Dialog.FileName)
                                };
                                OnPropertyChanged("Content");
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                            isLoadImage = false;
                        }
                    }, obj => !isLoadImage
                    );
            }
        }

        public ICommand Registration
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    var res = App.ClientService.Account.CreateUser(new CreateUserDTO
                    {
                        Avatar = Avatar,
                        Email = Email,
                        Password = GetPassword(),
                        UserName = Login
                    });
                });
            }
        }
    }
}
