using Galery.Server.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.DAL
{
    public interface IUnitOfWork
    {
        CommentRepository Comments { get; set; }
        PictureRepository Pictures { get; set; }
        TagRepository Tags { get; set; }
    }
}
