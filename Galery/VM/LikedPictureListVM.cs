using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galery.Server.Service.DTO.PictureDTO;

namespace Galery.VM
{
    class LikedPictureListVM : BasePictureListVM
    {
        public LikedPictureListVM(MainVM mainVm) : base(mainVm)
        {

        }

        protected override Task<IEnumerable<PictureInfoDTO>> LoadMoreData()
        {
            throw new NotImplementedException();
        }
    }
}
