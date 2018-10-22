using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Galery.VM
{
    class BasePageVM : BaseWithCommonVM
    {
        public BasePageVM(MainVM mainVM) : base(mainVM)
        {
            MainVM = mainVM;
            PreviousContent = MainVM.Content;
        }

        protected MainVM MainVM;
        private object PreviousContent;
        public ICommand BackToPrevious
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    MainVM.Content = PreviousContent;
                }, 
                obj=>PreviousContent != null);
            }
        }
    }
}
