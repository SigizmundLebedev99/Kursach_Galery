﻿using Galery.VM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galery.VM
{
    class UserPageVM : BasePageVM
    {
        public UserPageVM(MainVM mainVM, Roles role) : base(mainVM)
        {

        }

        public MenuItem[] MenuItems { get; set; }
    }
}