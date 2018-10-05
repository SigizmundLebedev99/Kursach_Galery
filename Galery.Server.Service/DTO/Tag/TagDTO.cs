using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.DTO.PictureDTO
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PicturesCount { get; set; }
    }
}
