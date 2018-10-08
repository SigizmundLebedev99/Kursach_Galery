using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(CreateUserDTO model);
        Task<IdentityResult> UpdateUser(int id, CreateUserDTO model);
    }
}
