namespace Galery.Server.DAL.Models
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }    

        public string Avatar { get; set; }
    }
}
