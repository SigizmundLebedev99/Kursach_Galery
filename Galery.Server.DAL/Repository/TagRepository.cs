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

        public async Task DeleteAsync(DbConnection connection, int id)
        {
            await connection.ExecuteAsync($"delete from [{nameof(Tag)}] where [{nameof(Tag.Id)}] = @{nameof(id)}", new { id });
        }

        public async Task<Tag> FindByIdAsync(DbConnection connection, int id)
        {     
            return await connection.QueryFirstOrDefaultAsync<Tag>($"SELECT * FROM [{nameof(Tag)}] WHERE [{nameof(Tag.Id)}] = @{nameof(id)}", new { id });
        }

        public async Task UpdateAsync(DbConnection connection, Tag entity)
        {     
            await connection.ExecuteAsync(UpdateQuery(entity), entity);
        }

        public async Task<IEnumerable<Tag>> GetTagsForPicture(DbConnection connection, int pictureId)
        {
            return await connection.QueryAsync<Tag>(m2mJoinQuery<Tag, PictureTag>(e=>e.Id, e=>e.TagId, e=>e.PictureId, nameof(pictureId)), new { pictureId}); 
        }
    }
}
