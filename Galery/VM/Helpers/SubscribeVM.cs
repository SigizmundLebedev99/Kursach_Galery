using MaterialDesignThemes.Wpf;
using System.Net.Http;
using System.Windows.Input;

namespace Galery.VM
{
    class SubscribeVM : BaseVM
    {
        private bool isSub;
        private bool isSubEnabled;
        private int userId;
        private bool IsSub { get => isSub; set { isSub = value; OnPropertyChanged("ButtonContent"); } }
     
        public bool IsSubEnabled { get => isSubEnabled; private set { isSubEnabled = value; OnPropertyChanged(); } }
        public PackIconKind ButtonContent { get { return isSub ? PackIconKind.AccountMultipleMinus : PackIconKind.AccountMultiplePlus; } }

        public SubscribeVM(bool isSub, int userId)
        {
            this.userId = userId;
            IsSub = isSub;
            IsSubEnabled = true;
        }

        public ICommand SubSwitch
        {
            get
            {
                return new DelegateCommand(async obj =>
                {
                    IsSubEnabled = false;
                    HttpResponseMessage res = null;
                    if (isSub)
                    {
                        res = await App.ClientService.Subscribe.Desubscribing(userId);
                        if (res.IsSuccessStatusCode)
                            IsSub = false;
                    }
                    else
                    {
                        res = await App.ClientService.Subscribe.Subscribing(userId);
                        if (res.IsSuccessStatusCode)
                            IsSub = true;
                    }
                    if (!res.IsSuccessStatusCode)
                        await App.ShowErrorMessage("Что то пошло не так\n" + res.ReasonPhrase);
                    IsSubEnabled = true;
                });
            }
        }
    }
}
