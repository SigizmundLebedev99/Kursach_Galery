using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.DAL.Models
{
    public class Picture : BaseEntity
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; }
    }
}
