using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galery.Server.DTO
{
    public class TokenResponse
    {
        public string Access_token { get; set; }

        public string Username { get; set; }

        public int UserId { get; set; }

        public IList<string> Roles { get; set; }

        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }
    }
}
