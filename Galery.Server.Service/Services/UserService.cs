using AutoMapper;
using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.User;
using Galery.Server.Service.Exceptions;
using Galery.Server.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<User> _userManager;
        readonly IFileWorkService _file;
        readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IFileWorkService file, IMapper mapper)
        {
            _userManager = userManager;
            _file = file;
            _mapper = mapper;
        }

        public async Task<User> CreateUser(CreateUserDTO model)
        {
            try
            {
                
                var entity = _mapper.Map<User>(model);
                if (model.Avatar != null)
                {
                    entity.Avatar = await _file.SaveAvatar(model.Avatar);
                }
                var res = await _userManager.CreateAsync(entity);
                if (!res.Succeeded)
                {

                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось удалить данные", ex.Message);
            }
        }

        public async Task<User> UpdateUser(int id, CreateUserDTO model)
        {
            try
            {
                var entity = _mapper.Map<User>(model);
                if (model.Avatar != null)
                {
                    entity.Avatar = await _file.SaveAvatar(model.Avatar);
                }
                var res = await _userManager.CreateAsync(entity);
                if (!res.Succeeded)
                {

                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось удалить данные", ex.Message);
            }
        }
    }
}
