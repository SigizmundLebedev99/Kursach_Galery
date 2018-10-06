using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserDTO model);
        Task<User> UpdateUser(int id, CreateUserDTO model);
    }
}
