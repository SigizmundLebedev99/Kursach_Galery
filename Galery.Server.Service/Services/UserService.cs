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

        public async Task<IdentityResult> CreateUser(CreateUserDTO model)
        {
            try
            {
                var entity = _mapper.Map<User>(model);
                var res = await _userManager.CreateAsync(entity, model.Password);
                if (!res.Succeeded)
                {
                    res = await _userManager.AddToRoleAsync(entity, "user");
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public async Task<IdentityResult> UpdateUser(int id, CreateUserDTO model)
        {
            try
            {
                var entity = _mapper.Map<User>(model);
                return await _userManager.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось обновить данные", ex.Message);
            }
        }
    }
}
