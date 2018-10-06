using Dapper;
using Galery.Server.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

                var sb = new StringBuilder();
                sb.Append($"insert into [{nameof(PictureTag)}]([{nameof(PictureTag.PictureId)}], " +
                    $"[{nameof(PictureTag.TagId)}]) values ");
                foreach (var id in tagIds) sb.Append($"({entity.Id}, {id}),");

                sb.Remove(sb.Length - 1, 1);

                await connection.ExecuteAsync(sb.ToString(),null, transaction);

                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            
            return entity;
        }

        public async Task DeleteAsync(DbConnection connection, int id)
        {   
            await connection.ExecuteAsync($"DELETE FROM [{nameof(Picture)}] WHERE [Id] = @{nameof(id)}", new { id });
        }

        public async Task<Picture> FindByIdAsync(DbConnection connection, int id)
        {            
            return await connection.QueryFirstOrDefaultAsync<Picture>($"SELECT * FROM [{nameof(Picture)}] WHERE [{nameof(Picture.Id)}] = @{nameof(id)}", new { id });        
        }

        public async Task UpdateAsync(DbConnection connection, Picture entity, IEnumerable<int> tagIds)
        {            
            IDbTransaction transaction = null;
            try
            {
                transaction = connection.BeginTransaction();
                entity.Id = await connection.QuerySingleAsync<int>(UpdateQuery(entity), entity, transaction);
                var sb = new StringBuilder();
                sb.Append($"delete from [{nameof(PictureTag)}] where [{nameof(PictureTag.PictureId)}] = {entity.Id}");
                sb.Append($"insert into [{nameof(PictureTag)}]([{nameof(PictureTag.PictureId)}], " +
                    $"[{nameof(PictureTag.TagId)}]) values ");
                foreach (var id in tagIds) sb.Append($"({entity.Id}, {id}),");

                sb.Remove(sb.Length - 1, 1);

                await connection.ExecuteAsync(sb.ToString(), null, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Picture>> GetByAuthorAsync(DbConnection connection, int authorId, int? skip, int? take)
        {
                return await connection.QueryAsync<Picture>($"SELECT * FROM [{nameof(Picture)}] WHERE [{nameof(Picture.UserId)}] = @{nameof(authorId)} "
                    + TakeSkipQuery<Picture>(p=>p.Id, skip, take), new { authorId });
        }

        public async Task<IEnumerable<Picture>> GetLikedByUserAsync(DbConnection connection, int userId, int? skip, int? take)
        {
                return await connection.QueryAsync<Picture>(
                    m2mJoinQuery<Picture, PictureLikes>(
                        pic=>pic.Id, 
                        pl=>pl.PictureId, 
                        pl=>pl.UserId, 
                        nameof(userId)) + TakeSkipQuery<Picture>(p=>p.Id, skip, take), 
                    new { userId });
        }

        public async Task<int> GetLikesCount(DbConnection connection, int pictureId)
        {
            return await connection.QuerySingleAsync<int>(
                $"select count([{nameof(PictureLikes.UserId)}]) " +
                $"from [{nameof(PictureLikes)}] " +
                $"where [{nameof(PictureLikes.PictureId)}] = @{nameof(pictureId)}",
                new { pictureId });
        }

        public async Task PushLike(DbConnection connection, PictureLikes like)
        {
            await connection.ExecuteAsync(CreateQuery(like), like);
        }

        public async Task<bool> IsLikeExist(DbConnection connection, PictureLikes like)
        {
            return await connection.QuerySingleAsync<bool>($"select iif(@{nameof(like.UserId)} = any (select [{nameof(PictureLikes.UserId)}] " +
                $"from [PictureLikes] where PictureId = @{nameof(like.PictureId)}),1,0)", like); 
        }

        public async Task<int> PicturesCountAsync(DbConnection connection, int userId)
        {
            return await connection.QueryFirstAsync<int>($"select count([{nameof(Picture.Id)}]) from [{nameof(Picture)}] " +
                $"where [{nameof(Picture.UserId)}] = @{nameof(userId)}",
                new { userId });
        }
    }
}
