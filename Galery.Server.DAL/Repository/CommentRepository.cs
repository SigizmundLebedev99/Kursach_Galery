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
    public class CommentRepository
    {
        readonly DbProviderFactory _factory;
        readonly string _connectionString;

        public CommentRepository(DbProviderFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CountForPublication(int publicationId)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QuerySingleAsync<int>(
                    $"select count([{nameof(Comment.Id)}]) " +
                    $"from [{nameof(Comment)}] " +
                    $"where [{nameof(Comment.PictureId)}] = @{nameof(publicationId)}",
                    new { publicationId });
            }
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

        public async Task DeleteAsync(int id)
        {
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                await connection.ExecuteAsync($"delete from [{nameof(Comment)}] where [{nameof(Comment.Id)}] = @{nameof(id)}", new { id });
            }
        }

        public async Task<Comment> FindByIdAsync(int id)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Comment>($"SELECT * FROM [{nameof(Comment)}] WHERE [{nameof(Comment.Id)}] = @{nameof(id)}", new { id });
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsForPublication(int publicationId, int? skip, int? take)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QueryAsync<Comment>(
                    $"SELECT * " +
                    $"FROM [{nameof(Comment)}] " +
                    $"WHERE [{nameof(Comment.PictureId)}] = @{nameof(publicationId)} " + TakeSkipQuery<Comment>(c=>c.PictureId, skip, take), new { publicationId });
            }
        }

        public async Task UpdateAsync(Comment entity)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                await connection.ExecuteAsync(UpdateQuery(entity), entity);
            }
        }
    }
}
