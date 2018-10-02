using AutoMapper;
using Dapper;
using Galery.Server.DAL;
using Galery.Server.DAL.Models;
using Galery.Server.DAL.Repository;
using Galery.Server.Interfaces;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.Exceptions;
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
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public Task<PictureFullInfoDTO> GetPictureByIdAnonimousAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PictureFullInfoDTO> GetPictureByIdAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PictureInfoWithFeedbackDTO>> GetTopPicturesAsync(int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLikeAsync(int userId, int pictureId)
        {
            throw new NotImplementedException();
        }

        public Task SetLikeAsync(int userId, int pictureId)
        {
            throw new NotImplementedException();
        }

        public Task<PictureInfoDTO> UpdatePictureAsync(int id, CreatePictureDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
