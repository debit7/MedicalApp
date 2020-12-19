using Mongo_JWT.Models.AuthenticationLog;
using Mongo_JWT.Models.Login;
using Mongo_JWT.Models.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_JWT.Services.Login
{
    public interface ILoginService
    {
        CustomerDetail GetUser(string email);
        void InsertUser(CustomerDetail user);
        void InsertLog(AuthenticationRequest log);

        CustomerDetail Find(string  email);
        CustomerDetail FindById(Guid Id);
        string CheckAuthentication(Guid uid,string token);
        void UpdateTokenStatus(string token);

        LoginResponse GetUSerDetails(Guid userId);

    }
}
