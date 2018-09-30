using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.DAL.Models
{
    public class PictureLikes
    {
        public int PictureId { get; set; }
        public int UserId { get; set; }
    }
}
