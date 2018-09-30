using Galery.Server.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.DAL.Models
{
    class Comment : BaseEntity
    {
        public int PictureId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
