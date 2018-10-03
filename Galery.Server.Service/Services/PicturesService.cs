using AutoMapper;
using Dapper;
using Galery.Server.DAL;
using Galery.Server.DAL.Models;
using Galery.Server.DAL.Repository;
using Galery.Server.Interfaces;
using Galery.Server.Service.DTO.CommentDTO;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.Exceptions;
using Galery.Server.Service.Infrostructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Galery.Server.Services
{
    public class PicturesService : IPictureService
    {
        readonly IUnitOfWork uow;
        readonly string _connectionString;
        readonly DbProviderFactory _factory;
        readonly UserManager<User> _userManager;
        readonly IMapper _mapper;

        public PicturesService(IUnitOfWork uow, 
            DbProviderFactory factory, 
            IConfiguration configuration, 
            UserManager<User> userManager,
            IMapper mapper)
        {
            this.uow = uow;
            _factory = factory;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PictureInfoDTO> CreatePictureAsync(CreatePictureDTO model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                    throw new NotFoundException($"Не удалось найти пользователя с id = {model.UserId}");
                var entity = _mapper.Map<Picture>(model);
                entity = await uow.Pictures.CreateAsync(entity, model.TagIds);
                var result = _mapper.Map<PictureInfoDTO>(entity);
                result.Avatar = user.Avatar;
                result.UserName = user.UserName;
                return result;
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
                var entity = uow.Pictures.FindByIdAsync(id);
                if (entity == null)
                    throw new NotFoundException($"Не удалось найти картину с id = {id}");
                await uow.Pictures.DeleteAsync(id);
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

        public async Task<IEnumerable<Picture>> GetByUserAsync(int userId, int? skip, int? take)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    throw new NotFoundException($"Не удалось найти пользователя с id = {userId}");
                return await uow.Pictures.GetByAuthorAsync(userId, skip, take);
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

        public async Task<IEnumerable<PictureInfoDTO>> GetLikedByUserAsync(int userId, int? skip, int? take)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    throw new NotFoundException($"Не удалось найти пользователя с id = {userId}");
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
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

        public async Task<PictureFullInfoDTO> GetPictureByIdAnonimousAsync(int id)
        {
            try
            {
                var pic = await uow.Pictures.FindByIdAsync(id);
                if (pic == null)
                    throw new NotFoundException($"Не удалось найти картину с id = {id}");
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                    var result = await connection.QueryFirstOrDefaultAsync<PictureFullInfoDTO>("GetPicByIdAnonimous", new { id }, null, null, CommandType.StoredProcedure);
                    result.Tags = await uow.Tags.GetTagsForPicture(id);
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
                var pic = await uow.Pictures.FindByIdAsync(id);
                if (pic == null)
                    operRes.AddErrorMessage("id", $"Не удалось найти картину с id = {id}");
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    operRes.AddErrorMessage("userId", $"Не удалось найти пользователя с id = {userId}");

                if (!operRes.Sucseeded)
                    return operRes;

                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                    var result = await connection.QueryFirstOrDefaultAsync<PictureFullInfoDTO>("GetPictureById", new { id, userId }, null, null, CommandType.StoredProcedure);
                    result.CommentList = await connection.QueryAsync<CommentInfoDTO>("GetCommentsForPicture", new { pictureId = id }, null, null, CommandType.StoredProcedure);
                    result.Tags = await uow.Tags.GetTagsForPicture(id);
                    operRes.Results.Add(result);
                    return operRes;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public async Task<IEnumerable<PictureInfoDTO>> GetPictursByTag(int tagId, int? skip, int? take)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
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

        public async Task<IEnumerable<PictureInfoWithFeedbackDTO>> GetTopPicturesAsync(int? skip, int? take)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
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
                if (await uow.Pictures.IsLikeExist(like))
                {
                    using (var connection = _factory.CreateConnection())
                    {
                        connection.ConnectionString = _connectionString;
                        await connection.OpenAsync();
                        await connection.ExecuteAsync($"DELETE FROM {nameof(PictureLikes)} " +
                            $"WHERE {nameof(PictureLikes.UserId)} = @{nameof(userId)} AND {nameof(PictureLikes.PictureId)} = @{nameof(pictureId)}",
                            new { userId, pictureId });
                    }
                }
                throw new NotFoundException($"Не удалось найти лайк пользователя с id = {userId} для картины {pictureId}");
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
                var like = new PictureLikes { PictureId = pictureId, UserId = userId };
                if (!await uow.Pictures.IsLikeExist(like))
                    await uow.Pictures.PushLike(like);
                else
                    throw new NotFoundException($"Лайк пользователя с id = {userId} для картины {pictureId} уже существует");
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

        public async Task<OperationResult<PictureInfoDTO>> UpdatePictureAsync(int id, CreatePictureDTO model)
        {
            try
            {
                var operRes = new OperationResult<PictureInfoDTO>(true);
                var pic = await uow.Pictures.FindByIdAsync(id);
                if (pic == null)
                    operRes.AddErrorMessage("id", $"Не удалось найти картину с id = {id}");
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                    operRes.AddErrorMessage("userId", $"Не удалось найти пользователя с id = {model.UserId}");

                if (!operRes.Sucseeded)
                    return operRes;
                var entity = _mapper.Map<Picture>(model);
                entity.Id = id;
                await uow.Pictures.UpdateAsync(entity, model.TagIds);
                var result = _mapper.Map<PictureInfoDTO>(entity);
                result.UserName = user.UserName;
                result.Avatar = user.Avatar;
                operRes.Results.Add(result);
                return operRes;
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
