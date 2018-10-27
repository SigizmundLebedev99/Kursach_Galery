using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Galery.VM
{
    abstract class BaseWithCommonVM : BaseVM
    {
        public CommonVM CommonVM { get; }

        public BaseWithCommonVM(MainVM mainVM)
        {
            CommonVM = mainVM.CommonVM;
        }
    }
}
