using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.DTO.User
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Avatar { get; set; }
    }
}
