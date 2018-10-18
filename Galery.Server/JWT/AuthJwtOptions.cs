using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.JWT
{
    public static class AuthTokenOptions
    {
        public const string ISSUER = "GaleryServer"; // issuer
        public const string AUDIENCE = "http://localhost:49676/"; // audience
        public const string KEY = "38v37p36e35r34e33d32_31p30o29_28d27o26r25o24g23e22_21i20z19_18z17h16e15l14t13o12g11o10_9k8i7t6p5i4c3h2a1";
        public const int LIFETIME = 60;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
