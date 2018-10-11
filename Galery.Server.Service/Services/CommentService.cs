using AutoMapper;
using Galery.Server.DAL;
using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.CommentDTO;
using Galery.Server.Service.Exceptions;
using Galery.Server.Service.Infrostructure;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Galery.Server.Service.Services
{
    public class CommentService : ICommentService
    {
        readonly IUnitOfWork uow;
        readonly UserManager<User> _userManager;
        readonly IMapper _mapper;
        readonly string _connectionString;
        readonly DbProviderFactory _factory;

        public CommentService(IUnitOfWork uow,
        UserManager<User> userManager,
        IMapper mapper,
        DbProviderFactory factory, IConfiguration config)
        {
            this.uow = uow;
            _userManager = userManager;
            _mapper = mapper;
            _connectionString = config.GetConnectionString("DefaultConnection");
            _factory = factory;
        }

        public async Task<OperationResult<CommentInfoDTO>> CreateCommentAsync(CreateCommentDTO model)
        {
            try
            {
                var operRes = new OperationResult<CommentInfoDTO>(true);
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                    operRes.AddErrorMessage("UserId", $"Не удалось найти пользователя с id = {model.UserId}");
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                    var pic = uow.Pictures.FindByIdAsync(connection, model.PictureId);
                    if (pic == null)
                        operRes.AddErrorMessage("PictureId", $"Не удалось найти картину с id = {model.PictureId}");

                    if (!operRes.Succeeded)
                        return operRes;

                    var entity = _mapper.Map<Comment>(model);
                    entity.DateOfCreation = DateTime.Now;
                    entity = await uow.Comments.CreateAsync(connection, entity);
                    var result = _mapper.Map<CommentInfoDTO>(entity);
                    result.UserName = user.UserName;
                    result.Avatar = user.Avatar;
                    operRes.Results.Add(result);
                    return operRes;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public async Task DeleteCommentAsync(int id)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                    var entity = await uow.Comments.FindByIdAsync(connection, id);
                    if (entity == null)
                        throw new NotFoundException($"Не удалось найти комментарий с id = {id}");
                    await uow.Comments.DeleteAsync(connection, id);
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

        public async Task<OperationResult<CommentInfoDTO>> UpdateCommentAsync(int id, CreateCommentDTO model)
        {
            try
            {
                var operRes = new OperationResult<CommentInfoDTO>(true);
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                    operRes.AddErrorMessage("UserId", $"Не удалось найти пользователя с id = {model.UserId}");
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                    var pic = await uow.Pictures.FindByIdAsync(connection, model.PictureId);
                    if (pic == null)
                        operRes.AddErrorMessage("PictureId", $"Не удалось найти картину с id = {model.PictureId}");
                    var comm = await uow.Comments.FindByIdAsync(connection, id);
                    if (comm == null)
                        operRes.AddErrorMessage("id", $"Не удалось найти комментарий с id = {model.PictureId}");

                    if (!operRes.Succeeded)
                        return operRes;

                    var entity = _mapper.Map<Comment>(model);
                    entity.Id = id;
                    entity.DateOfCreation = comm.DateOfCreation;
                    await uow.Comments.UpdateAsync(connection, entity);
                    var result = _mapper.Map<CommentInfoDTO>(entity);
                    result.UserName = user.UserName;
                    result.Avatar = user.Avatar;
                    operRes.Results.Add(result);
                    return operRes;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }
    }
}
