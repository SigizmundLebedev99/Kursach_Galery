using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.DAL.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
