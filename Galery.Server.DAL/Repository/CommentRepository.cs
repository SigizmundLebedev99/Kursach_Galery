using Dapper;
using Galery.Server.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using static Galery.Server.DAL.Helpers.QueryBuilder;

namespace Galery.Server.DAL.Repository
{
    class CommentRepository : IRepository<Comment>
    {
        readonly DbProviderFactory _factory;
        readonly string _connectionString;

        public CommentRepository(DbProviderFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Comment> CreateAsync(Comment entity)
        {
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                entity.Id = await connection.QuerySingleAsync<int>(CreateQuery(entity), entity);
            }
            return entity;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}
