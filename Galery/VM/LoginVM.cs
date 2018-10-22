using Galery.Server.DTO.User;
using System.Collections.Generic;
using System.Net;
using System.Windows.Input;
using Galery.Server.DTO;
using System.Net.Http;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using System;
using Galery.ClientLogic.Results;

namespace Galery.VM
{
    class LoginVM : BaseVM
    {
        public LoginVM(MainVM mainVM)
        {
            MainVM = mainVM;
        }

        public Func<string> GetPassword;
        public Action CleanPassword;

        private MainVM MainVM { get; set; }

        public string LoginMessage { get; set; } = "Логин";
        public string PasswordMessage { get; set; } = "Пароль";

        string login;
        public string Login { get { return login; } set { login = value; OnPropertyChanged(); } }

        public ICommand LogIn
        {
            get
            {
                return new DelegateCommand
                    (
                    async obj =>
                    {
                        await LogInProcess();
                    },
                    obj =>
                    {
                        return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(GetPassword());
                    }
                    );
            }
        }

        private async Task LogInProcess()
        {
            var res = await App.ClientService.Account.Token(new LoginDTO { Name = Login, Password = GetPassword() });
            if (res.IsSuccessStatusCode)
            {
                ResetMessages();
                App.LogIn(await res.Content.ReadAsAsync<TokenResponse>());
                MainVM.FlipLoginPlateBack();
            }
            else if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                var responce = await res.Content.ReadAsAsync<BadInputResult>();
                if (responce.Email != null && responce.Email.Length > 0)
                {
                    LoginMessage = "Неверный логин";
                    OnPropertyChanged("LoginMessage");
                    Login = string.Empty;
                }
                if (responce.Password != null && responce.Password.Length > 0)
                {
                    PasswordMessage = "Неверный пароль";
                    OnPropertyChanged("PasswordMessage");
                    CleanPassword();
                }
            }
            else
            {
                await App.ShowErrorMessage("Не удалось войти: " + res.ReasonPhrase);
            }
        }

        private void ResetMessages()
        {
            LoginMessage = "Логин";
            OnPropertyChanged("LoginMessage");
            PasswordMessage = "Пароль";
            OnPropertyChanged("PasswordMessage");
            CleanPassword();
            Login = string.Empty;
        }

    }
}
