﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.DTO.PictureDTO
{
    public class CreatePictureDTO
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }

        public IEnumerable<int> TagIds { get; set; }
    }
}
