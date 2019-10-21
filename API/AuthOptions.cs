using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class AuthOptions
    {
        public const string ISSUER = "Calamity";
        public const string AUDIENCE = "http://localhost:44324/";
        const string KEY = "yEv2P1G8DmZ19Zphoo1P5EKG3cnCzEWRMwR1AyCybgR6EJOnqNyLURpMAfeNefkBGQFZ3MXqz8jqS0bdQCMpMY6p4QGZKscdIsj";
        public const int LIFETIME = 15;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
