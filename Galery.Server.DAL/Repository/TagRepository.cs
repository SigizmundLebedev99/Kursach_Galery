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
        public async Task<Tag> CreateAsync(DbConnection connection, Tag entity)
        {
            entity.Id = await connection.QuerySingleAsync<int>(CreateQuery(entity), entity);
            return entity;
        }

        public Task DeleteAsync(DbConnection connection, int id)
        {
            return connection.ExecuteAsync($"delete from [{nameof(Tag)}] where [{nameof(Tag.Id)}] = @{nameof(id)}", new { id });
        }

        public Task<Tag> FindByIdAsync(DbConnection connection, int id)
        {     
            return connection.QueryFirstOrDefaultAsync<Tag>($"SELECT * FROM [{nameof(Tag)}] WHERE [{nameof(Tag.Id)}] = @{nameof(id)}", new { id });
        }

        public Task UpdateAsync(DbConnection connection, Tag entity)
        {     
            return connection.ExecuteAsync(UpdateQuery(entity), entity);
        }

        public Task<IEnumerable<Tag>> GetTagsForPicture(DbConnection connection, int pictureId)
        {
            string query = m2mJoinQuery<Tag, PictureTag>(e => e.Id, e => e.TagId, e => e.PictureId, "pictureId");
            return connection.QueryAsync<Tag>(query, new { pictureId}); 
        }
    }
}
