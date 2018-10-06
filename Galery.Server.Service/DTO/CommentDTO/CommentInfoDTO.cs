using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.DTO.CommentDTO
{
    public class CommentInfoDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PictureId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Text { get; set; }
    }
}
