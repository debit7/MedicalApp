using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_JWT.JWTConfiguration
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
