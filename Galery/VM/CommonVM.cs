using Galery.Pages;
using Galery.VM.Helpers;
using System.Windows.Input;

namespace Galery.VM
{
    class CommonVM
    {
        readonly MainVM _mainVm;

        public Roles CurrentRole { get; set; }

        public CommonVM(MainVM mainVM)
        {
            _mainVm = mainVM;
        }

        public ICommand SelectUser
        {
            get
            {
                return new DelegateCommand
                    (
                    obj =>
                    {
                        int userId = (int)obj;

                        _mainVm.Content = new UserInfo(_mainVm, userId, CurrentRole);
                    }
                    );
            }
        }

        public ICommand SelectPicture
        {
            get
            {
                return new DelegateCommand
                    (
                    obj =>
                    {
                        int picId = (int)obj;

                        _mainVm.Content = new Picture(_mainVm, picId, CurrentRole);
                    }
                    );
            }
        }
    }
}
