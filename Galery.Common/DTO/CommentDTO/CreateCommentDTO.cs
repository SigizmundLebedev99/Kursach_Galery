using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Galery.Server.Service.DTO.CommentDTO
{
    public class CreateCommentDTO
    {
        public int PictureId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
