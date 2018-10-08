using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.DTO.PictureDTO
{
    public class PictureFullInfoDTO : PictureInfoWithFeedbackDTO
    {
        public bool IsLiked { get; set; }
        public IEnumerable<CommentInfoDTO> CommentList { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public string Description { get; set; }
    }
}
