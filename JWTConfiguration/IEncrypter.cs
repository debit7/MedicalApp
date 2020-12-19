using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_JWT.JWTConfiguration
{
    public interface IEncryptor
    {
        string GetSalt(string value);
        string GetHash(string value, string salt);
    }
}
