using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galery.VM
{
    class UserVM
    {
        public UserVM(string name, object content)
        {
            Name = name; Content = content;
        }

        public string Name { get; set; }
        public object Content { get; set; }
    }
}
