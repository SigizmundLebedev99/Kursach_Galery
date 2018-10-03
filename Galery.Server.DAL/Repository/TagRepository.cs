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
    public class TagRepository
    {
        readonly DbProviderFactory _factory;
        readonly string _connectionString;

        public TagRepository(DbProviderFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Tag> CreateAsync(Tag entity)
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
                await connection.ExecuteAsync($"delete from [{nameof(Tag)}] where [{nameof(Tag.Id)}] = @{nameof(id)}", new { id });
            }
        }

        public async Task<Tag> FindByIdAsync(int id)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Tag>($"SELECT * FROM [{nameof(Tag)}] WHERE [{nameof(Tag.Id)}] = @{nameof(id)}", new { id });
            }
        }

        public async Task UpdateAsync(Tag entity)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                await connection.ExecuteAsync(UpdateQuery(entity), entity);
            }
        }

        public async Task<IEnumerable<Tag>> GetTagsForPicture(int pictureId)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QueryAsync<Tag>(m2mJoinQuery<Tag, PictureTag>(e=>e.Id, e=>e.TagId, e=>e.PictureId, nameof(pictureId)), new { pictureId});
            }
        }
    }
}
