using AutoMapper;
using Dapper;
using Galery.Server.DAL;
using Galery.Server.DAL.Models;
using Galery.Server.Interfaces;
using Galery.Server.Service.DTO.CommentDTO;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.Exceptions;
using Galery.Server.Service.Infrostructure;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using static Galery.Server.DAL.Helpers.QueryBuilder;

namespace Galery.Server.Services
{
    public class PicturesService : IPictureService
    {
        readonly IUnitOfWork uow;
        readonly string _connectionString;
        readonly DbProviderFactory _factory;
        readonly UserManager<User> _userManager;
        readonly IMapper _mapper;
        readonly IFileWorkService _file;

        public PicturesService(IUnitOfWork uow, 
            DbProviderFactory factory, 
            IConfiguration configuration, 
            UserManager<User> userManager,
            IMapper mapper,
            IFileWorkService file)
        {
            this.uow = uow;
            _factory = factory;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _userManager = userManager;
            _mapper = mapper;
            _file = file;
        }
        
        private Task Connect(DbConnection connection)
        {
            connection.ConnectionString = _connectionString;
            return connection.OpenAsync();
        }

        public async Task<OperationResult<PictureInfoDTO>> CreatePictureAsync(CreatePictureDTO model)
        {
            try
            {
                var operRes = new OperationResult<PictureInfoDTO>(true);
                if (!_file.IsExist(model.ImagePath))
                    operRes.AddErrorMessage("ImagePath", "Не удалось загрузить файл");
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                    operRes.AddErrorMessage("UserId",$"Не удалось найти пользователя с id = {model.UserId}");
                var entity = _mapper.Map<Picture>(model);
                entity.DateOfCreation = DateTime.Now;
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    entity = await uow.Pictures.CreateAsync(connection, entity, model.TagIds);
                }
                var result = _mapper.Map<PictureInfoDTO>(entity);
                result.Avatar = user.Avatar;
                result.UserName = user.UserName;
                operRes.Results.Add(result);
                return operRes;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public async Task DeletePictureAsync(int id)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    var entity = uow.Pictures.FindByIdAsync(connection, id);
                    if (entity == null)
                        throw new NotFoundException($"Не удалось найти картину с id = {id}");
                    await uow.Pictures.DeleteAsync(connection, id);
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public async Task<IEnumerable<Picture>> GetByUserAsync(int userId, int skip, int take)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    throw new NotFoundException($"Не удалось найти пользователя с id = {userId}");
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    return await uow.Pictures.GetByAuthorAsync(connection, userId, skip, take);
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<IEnumerable<PictureInfoDTO>> GetLikedByUserAsync(int userId, int skip, int take)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    throw new NotFoundException($"Не удалось найти пользователя с id = {userId}");
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    return await connection.QueryAsync<PictureInfoDTO>("GetLikedByUser", new { userId, skip, take}, null,null, CommandType.StoredProcedure);
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<IEnumerable<PictureInfoDTO>> GetNewPicturesAsync(int skip, int take)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    return await connection.QueryAsync<PictureInfoDTO>("GetNewPictures", new { skip, take }, null, null, CommandType.StoredProcedure);
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<IEnumerable<PictureInfoDTO>> GetPicsFromSubscribes(int userId, int skip, int take)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException($"Не удалось найти пользователя с id = {userId}");
            using (var connection = _factory.CreateConnection())
            {
                await Connect(connection);
                return await connection.QueryAsync<PictureInfoDTO>("GetPicsFromSubscribes", new { userId, skip, take}, null, null, CommandType.StoredProcedure);
            }
        }

        public async Task<PictureFullInfoDTO> GetPictureByIdAnonimousAsync(int id)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    var pic = await uow.Pictures.FindByIdAsync(connection, id);
                    if (pic == null)
                        throw new NotFoundException($"Не удалось найти картину с id = {id}");
                
                    var result = await connection.QueryFirstOrDefaultAsync<PictureFullInfoDTO>("GetPicByIdAnonimous", new { id }, null, null, CommandType.StoredProcedure);
                    result.Tags = await connection.QueryAsync<Tag>(m2mJoinQuery<Tag, PictureTag>(e => e.Id, e => e.TagId, e => e.PictureId, nameof(id)), new { id });
                    result.CommentList = await connection.QueryAsync<CommentInfoDTO>("GetCommentsForPicture", new { pictureId = id }, null, null, CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<OperationResult<PictureFullInfoDTO>> GetPictureByIdAsync(int id, int userId)
        {
            try
            {
                var operRes = new OperationResult<PictureFullInfoDTO>(true);
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    operRes.AddErrorMessage("userId", $"Не удалось найти пользователя с id = {userId}");
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    var pic = await uow.Pictures.FindByIdAsync(connection, id);
                    if (pic == null)
                        operRes.AddErrorMessage("id", $"Не удалось найти картину с id = {id}");
                

                    if (!operRes.Succeeded)
                        return operRes;

               
                    var result = await connection.QueryFirstOrDefaultAsync<PictureFullInfoDTO>("GetPictureById", new { id, userId }, null, null, CommandType.StoredProcedure);
                    result.CommentList = await connection.QueryAsync<CommentInfoDTO>("GetCommentsForPicture", new { pictureId = id }, null, null, CommandType.StoredProcedure);
                    result.Tags = await uow.Tags.GetTagsForPicture(connection, id);
                    operRes.Results.Add(result);
                    return operRes;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<IEnumerable<PictureInfoDTO>> GetPicturesByTagAsync(int tagId, int skip, int take)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    return await connection.QueryAsync<PictureInfoDTO>("GetPicturesByTag", new { tagId, skip, take }, null, null, CommandType.StoredProcedure);
                }
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<IEnumerable<PictureInfoWithFeedbackDTO>> GetTopPicturesAsync(int skip, int take)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    return await connection.QueryAsync<PictureInfoWithFeedbackDTO>("GetTopPicturesInfo", new { skip, take }, null, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task RemoveLikeAsync(int userId, int pictureId)
        {
            try
            {
                var like = new PictureLikes { PictureId = pictureId, UserId = userId };
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    if (await uow.Pictures.IsLikeExist(connection, like))
                    {
                        await connection.ExecuteAsync($"DELETE FROM {nameof(PictureLikes)} " +
                            $"WHERE {nameof(PictureLikes.UserId)} = @{nameof(userId)} AND {nameof(PictureLikes.PictureId)} = @{nameof(pictureId)}",
                            new { userId, pictureId });
                    }
                    else
                        throw new NotFoundException($"Не удалось найти лайк пользователя с id = {userId} для картины {pictureId}");
                }
            }
            catch(NotFoundException ex)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new DatabaseException("Не удалось удалить данные", ex.Message);
            }
        }

        public async Task SetLikeAsync(int userId, int pictureId)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    var like = new PictureLikes { PictureId = pictureId, UserId = userId };
                    if (!await uow.Pictures.IsLikeExist(connection, like))
                        await uow.Pictures.PushLike(connection, like);
                    else
                        throw new NotFoundException($"Лайк пользователя с id = {userId} для картины {pictureId} уже существует");
                }
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public async Task<OperationResult<PictureInfoDTO>> UpdatePictureAsync(int id, CreatePictureDTO model)
        {
            try
            {
                var operRes = new OperationResult<PictureInfoDTO>(true);
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                    operRes.AddErrorMessage("userId", $"Не удалось найти пользователя с id = {model.UserId}");
                using (var connection = _factory.CreateConnection())
                {
                    await Connect(connection);
                    var pic = await uow.Pictures.FindByIdAsync(connection, id);
                    if (pic == null)
                        operRes.AddErrorMessage("id", $"Не удалось найти картину с id = {id}");


                    if (!operRes.Succeeded)
                        return operRes;

                    var entity = _mapper.Map<Picture>(model);
                    entity.Id = id;
                    entity.DateOfCreation = pic.DateOfCreation;
                    await uow.Pictures.UpdateAsync(connection, entity, model.TagIds);
                    var result = _mapper.Map<PictureInfoDTO>(entity);
                    result.UserName = user.UserName;
                    result.Avatar = user.Avatar;
                    operRes.Results.Add(result);
                    return operRes;
                }
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось удалить данные", ex.Message);
            }
        }
    }
}
