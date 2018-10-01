using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Galery.Server.DAL.Repository;
using Microsoft.Extensions.Configuration;

namespace Galery.Server.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DbProviderFactory factory, IConfiguration configuration)
        {
            Comments = new CommentRepository(factory, configuration);
            Pictures = new PictureRepository(factory, configuration);
            Tags = new TagRepository(factory, configuration);
        }

        public CommentRepository Comments { get; set; }
        public PictureRepository Pictures { get; set; }
        public TagRepository Tags { get; set; }
    }
}
