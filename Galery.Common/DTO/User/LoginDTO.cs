using System;

namespace Galery.Server.DTO.User
{
    public class LoginDTO
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
