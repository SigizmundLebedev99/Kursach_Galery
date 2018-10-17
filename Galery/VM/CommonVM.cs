using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Galery.VM
{
    class CommonVM
    {
        readonly MainVM mainVm;

        public CommonVM(MainVM mainVM)
        {

        }

        public ICommand SelectUser
        {
            get
            {
                return new DelegateCommand
                    (
                    obj =>
                    {
                        
                    }
                    );
            }
        }
    }
}
