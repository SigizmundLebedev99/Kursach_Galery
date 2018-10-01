using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.DTO.CommentDTO
{
    public class CreateCommentDTO
    {
        public int PictureId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
