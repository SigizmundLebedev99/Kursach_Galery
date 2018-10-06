using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Galery.Server.DAL.Repository;

namespace Galery.Server.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            Comments = new CommentRepository();
            Pictures = new PictureRepository();
            Tags = new TagRepository();
        }

        public CommentRepository Comments { get; set; }
        public PictureRepository Pictures { get; set; }
        public TagRepository Tags { get; set; }
    }
}
