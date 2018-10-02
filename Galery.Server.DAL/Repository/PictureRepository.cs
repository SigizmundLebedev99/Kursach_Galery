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
        readonly DbProviderFactory _factory;
        readonly string _connectionString;

        public PictureRepository(DbProviderFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Picture> CreateAsync(Picture entity, IEnumerable<int> tagIds)
        {
            using (DbConnection connection = _factory.CreateConnection())
            {
                IDbTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction();
                    connection.ConnectionString = _connectionString;

                    await connection.OpenAsync();
                    entity.Id = await connection.QuerySingleAsync<int>(CreateQuery(entity), entity, transaction);

                    var sb = new StringBuilder();
                    sb.Append($"insert into [{nameof(PictureTag)}]([{nameof(PictureTag.PictureId)}], " +
                        $"[{nameof(PictureTag.TagId)}]) values ");
                    foreach (var id in tagIds) sb.Append($"({entity.Id}, {id}),");

                    sb.Remove(sb.Length - 1, 1);

                    await connection.ExecuteAsync(sb.ToString(),null, transaction);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                await connection.ExecuteAsync($"DELETE FROM [{nameof(Picture)}] WHERE [Id] = @{nameof(id)}", new { id });
            }
        }

        public async Task<Picture> FindByIdAsync(int id)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Picture>($"SELECT * FROM [{nameof(Picture)}] WHERE [{nameof(Picture.Id)}] = @{nameof(id)}", new { id });
            }
        }

        public async Task UpdateAsync(Picture entity)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                await connection.ExecuteAsync(UpdateQuery(entity), entity);
            }
        }

        public async Task<IEnumerable<Picture>> GetByAuthorAsync(int authorId, int? skip, int? take)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QueryAsync<Picture>($"SELECT * FROM [{nameof(Picture)}] WHERE [{nameof(Picture.UserId)}] = @{nameof(authorId)}"
                    + TakeSkipQuery<Picture>(p=>p.Id, skip, take), new { authorId });
            }
        }

        public async Task<IEnumerable<Picture>> GetLikedByUserAsync(int userId, int? skip, int? take)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QueryAsync<Picture>(
                    m2mJoinQuery<Picture, PictureLikes>(
                        pic=>pic.Id, 
                        pl=>pl.PictureId, 
                        pl=>pl.UserId, 
                        nameof(userId)) + TakeSkipQuery<Picture>(p=>p.Id, skip, take), 
                    new { userId });
            }
        }

        public async Task<int> GetLikesCount(int pictureId)
        {
            using (var connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QuerySingleAsync<int>(
                    $"select count([{nameof(PictureLikes.UserId)}]) " +
                    $"from [{nameof(PictureLikes)}] " +
                    $"where [{nameof(PictureLikes.PictureId)}] = @{nameof(pictureId)}", 
                    new { pictureId });
            }
        }

        public async Task PushLike(PictureLikes like)
        {
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                await connection.ExecuteAsync(CreateQuery(like), like);
            }
        }

        public async Task<bool> IsLikeExist(PictureLikes like)
        {
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QuerySingleAsync<bool>($"select iif(@{nameof(like.UserId)} = any (select [{nameof(PictureLikes.UserId)}] " +
                    $"from [PictureLikes] where PictureId = @{nameof(like.PictureId)}),1,0)", like);
            }
        }

        public async Task<int> PicturesCountAsync(int userId)
        {
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync();
                return await connection.QueryFirstAsync<int>($"select count([{nameof(Picture.Id)}]) from [{nameof(Picture)}] " +
                    $"where [{nameof(Picture.UserId)}] = @{nameof(userId)}",
                    new { userId });
            }
        }
    }
}
