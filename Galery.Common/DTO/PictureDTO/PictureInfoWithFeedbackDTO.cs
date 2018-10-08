using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.DTO.PictureDTO
{
    public class PictureInfoWithFeedbackDTO : PictureInfoDTO
    {
        public int Likes { get; set; }
        public int Comments { get; set; }
    }
}
