using System;
using System.ComponentModel.DataAnnotations;

namespace Galery.Server.DTO.User
{
    public class LoginDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
