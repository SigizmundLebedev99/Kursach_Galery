using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Galery.VM
{
    abstract class BasePageVM : BaseWithCommonVM
    {
        public BasePageVM(MainVM mainVM) : base(mainVM)
        {
            MainVM = mainVM;
            previousContent = MainVM.Content;
            previousContext = previousContent?.DataContext;
        }

        protected MainVM MainVM;
        private FrameworkElement previousContent;
        private object previousContext;

        public ICommand BackToPrevious
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    previousContent.DataContext = previousContext;
                    MainVM.Content = previousContent;
                }, 
                obj=>previousContent != null);
            }
        }
    }
}
