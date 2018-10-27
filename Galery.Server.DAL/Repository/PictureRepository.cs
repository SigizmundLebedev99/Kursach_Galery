using Dapper;
using Galery.Server.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Galery.Server.DAL.Helpers.QueryBuilder;

namespace Galery.Server.DAL.Repository
{
    public class PictureRepository
    {
        public async Task<Picture> CreateAsync(DbConnection connection, Picture entity, IEnumerable<int> tagIds)
        {
            IDbTransaction transaction = null;
            try
            {
                transaction = connection.BeginTransaction();                   
                entity.Id = await connection.QuerySingleAsync<int>(CreateQuery(entity), entity, transaction);

                if (tagIds != null && tagIds.Count() > 0)
                {
                    var sb = new StringBuilder();
                    sb.Append($"insert into [{nameof(PictureTag)}]([{nameof(PictureTag.PictureId)}], " +
                        $"[{nameof(PictureTag.TagId)}]) values ");
                    foreach (var id in tagIds) sb.Append($"({entity.Id}, {id}),");

                    sb.Remove(sb.Length - 1, 1);

                    await connection.ExecuteAsync(sb.ToString(), null, transaction);
                }
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            
            return entity;
        }

        public Task DeleteAsync(DbConnection connection, int id)
        {   
            return connection.ExecuteAsync($"DELETE FROM [{nameof(Picture)}] WHERE [Id] = @{nameof(id)}", new { id });
        }

        public Task<Picture> FindByIdAsync(DbConnection connection, int id)
        {            
            return connection.QueryFirstOrDefaultAsync<Picture>($"SELECT * FROM [{nameof(Picture)}] WHERE [{nameof(Picture.Id)}] = @{nameof(id)}", new { id });        
        }

        public async Task UpdateAsync(DbConnection connection, Picture entity, IEnumerable<int> tagIds)
        {            
            IDbTransaction transaction = null;
            try
            {
                transaction = connection.BeginTransaction();
                await connection.ExecuteAsync(UpdateQuery(entity), entity, transaction);
                var sb = new StringBuilder();
                sb.Append($"delete from [{nameof(PictureTag)}] where [{nameof(PictureTag.PictureId)}] = {entity.Id};");
                if (tagIds != null && tagIds.Count() > 0)
                {
                    sb.Append($"insert into [{nameof(PictureTag)}]([{nameof(PictureTag.PictureId)}], " +
                        $"[{nameof(PictureTag.TagId)}]) values ");
                    foreach (var id in tagIds) sb.Append($"({entity.Id}, {id}),");
                sb.Remove(sb.Length - 1, 1);
                }
                await connection.ExecuteAsync(sb.ToString(), null, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public Task<IEnumerable<Picture>> GetByAuthorAsync(DbConnection connection, int authorId, int? skip, int? take)
        {
                return connection.QueryAsync<Picture>($"SELECT * FROM [{nameof(Picture)}] WHERE [{nameof(Picture.UserId)}] = @{nameof(authorId)} "
                    + TakeSkipQuery<Picture>(p=>p.Id, skip, take), new { authorId });
        }

        public Task<IEnumerable<Picture>> GetLikedByUserAsync(DbConnection connection, int userId, int? skip, int? take)
        {
                return connection.QueryAsync<Picture>(
                    m2mJoinQuery<Picture, PictureLikes>(
                        pic=>pic.Id, 
                        pl=>pl.PictureId, 
                        pl=>pl.UserId, 
                        "userId") + TakeSkipQuery<Picture>(p=>p.Id, skip, take), 
                    new { userId });
        }

        public Task<int> GetLikesCount(DbConnection connection, int pictureId)
        {
            return connection.QuerySingleAsync<int>(
                $"select count([{nameof(PictureLikes.UserId)}]) " +
                $"from [{nameof(PictureLikes)}] " +
                $"where [{nameof(PictureLikes.PictureId)}] = @{nameof(pictureId)}",
                new { pictureId });
        }

        public Task PushLike(DbConnection connection, PictureLikes like)
        {
            return connection.ExecuteAsync(CreateQuery(like), like);
        }

        public Task<bool> IsLikeExist(DbConnection connection, PictureLikes like)
        {
            return connection.QuerySingleAsync<bool>($"select iif(@{nameof(like.UserId)} = any (select [{nameof(PictureLikes.UserId)}] " +
                $"from [PictureLikes] where PictureId = @{nameof(like.PictureId)}),1,0)", like); 
        }

        public Task<int> PicturesCountAsync(DbConnection connection, int userId)
        {
            return connection.QueryFirstAsync<int>($"select count([{nameof(Picture.Id)}]) from [{nameof(Picture)}] " +
                $"where [{nameof(Picture.UserId)}] = @{nameof(userId)}",
                new { userId });
        }
    }
}
