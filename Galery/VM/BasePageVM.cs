using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Galery.VM
{
    class BasePageVM : BaseVM
    {
        public BasePageVM(MainVM mainVM)
        {
            _mainVM = mainVM;
            PreviousContent = _mainVM.Content;
        }

        private MainVM _mainVM;
        private object PreviousContent;
        public ICommand BackToPrevious
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    _mainVM.Content = PreviousContent;
                }, 
                obj=>PreviousContent != null);
            }
        }
    }
}
