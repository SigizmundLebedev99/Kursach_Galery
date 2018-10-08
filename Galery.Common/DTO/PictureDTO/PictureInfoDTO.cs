using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.DTO.PictureDTO
{
    public class PictureInfoDTO
    {
        public int Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Avatar { get; set; }
        public string UserName{get;set;}
        public string Name { get; set; }
        public int UserId { get; set; }
        public string ImagePath { get; set; }
    }
}
