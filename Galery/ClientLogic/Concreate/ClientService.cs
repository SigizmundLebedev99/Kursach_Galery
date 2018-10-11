using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galery.ClientLogic.Concreate
{
    class ClientService : IClientService
    {
        public IAccountClient Account { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILoadClient Load { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IPictureClient Picture { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICommentClient Comment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISubscribeClient Subscribe { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ITagClient Tag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
