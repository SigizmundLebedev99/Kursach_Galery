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
        public async Task<int> CountForPublication(DbConnection connection, int publicationId)
        {   
            return await connection.QuerySingleAsync<int>(
                $"select count([{nameof(Comment.Id)}]) " +
                $"from [{nameof(Comment)}] " +
                $"where [{nameof(Comment.PictureId)}] = @{nameof(publicationId)}",
                new { publicationId });
            
        }

        public async Task<Comment> CreateAsync(DbConnection connection, Comment entity)
        {
            entity.Id = await connection.QuerySingleAsync<int>(CreateQuery(entity), entity);
            return entity;
        }

        public async Task DeleteAsync(DbConnection connection, int id)
        {
            await connection.ExecuteAsync($"delete from [{nameof(Comment)}] where [{nameof(Comment.Id)}] = @{nameof(id)}", new { id });
        }

        public async Task<Comment> FindByIdAsync(DbConnection connection, int id)
        {
            return await connection.QueryFirstOrDefaultAsync<Comment>($"SELECT * FROM [{nameof(Comment)}] WHERE [{nameof(Comment.Id)}] = @{nameof(id)}", new { id });
        }

        public async Task<IEnumerable<Comment>> GetCommentsForPublication(DbConnection connection, int publicationId, int? skip, int? take)
        {
            return await connection.QueryAsync<Comment>(
                    $"SELECT * " +
                    $"FROM [{nameof(Comment)}] " +
                    $"WHERE [{nameof(Comment.PictureId)}] = @{nameof(publicationId)} " + TakeSkipQuery<Comment>(c=>c.PictureId, skip, take), new { publicationId });
        }

        public async Task UpdateAsync(DbConnection connection, Comment entity)
        {
            await connection.ExecuteAsync(UpdateQuery(entity), entity);
        }
    }
}
