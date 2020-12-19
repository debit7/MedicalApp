using Microsoft.AspNetCore.Mvc;
using Mongo_JWT.Helper;
using Mongo_JWT.JWTConfiguration;
using Mongo_JWT.Models.AuthenticationLog;
using Mongo_JWT.Models.DataBase;
using Mongo_JWT.Models.Login;
using Mongo_JWT.Models.Register;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_JWT.Services.Login
{
    public class LoginService : ILoginService
    { Function Func = new Function();
        private readonly IMongoCollection<CustomerDetail> _col;
        private readonly IMongoCollection<AuthenticationRequest> _authlog;
        private readonly IMongoCollection<AuthenticationResponse> _authresplog;


        public LoginService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _col = database.GetCollection<CustomerDetail>(CustomerDetail.DocumentName);
            _authlog = database.GetCollection<AuthenticationRequest>(AuthenticationRequest.DocumentName);
            _authresplog = database.GetCollection<AuthenticationResponse>(AuthenticationRequest.DocumentName);
        }


        public CustomerDetail GetUser(string email) =>
            _col.Find(u => u.Email == email).FirstOrDefault();

        public void InsertUser(CustomerDetail user) =>
            _col.InsertOne(user);

        public CustomerDetail Find(string email) =>
            _col.Find(sub => sub.Email == email).SingleOrDefault();

        public void InsertLog(AuthenticationRequest log) =>
             _authlog.InsertOne(log);

        public CustomerDetail FindById(Guid id) =>
             _col.Find(sub => sub.Id == id).SingleOrDefault();

        public void UpdateTokenStatus(string token)
        {
            var update = Builders<AuthenticationResponse>.Update.Set(a => a.Status, "OUT");
                _authresplog.UpdateOne(model => model.JWTToken == token, update);
        }

        public string CheckAuthentication(Guid uid, string token)
        {
            AuthenticationRequest logobj = new AuthenticationRequest();
            var check =
                        _authresplog.AsQueryable<AuthenticationResponse>()
                        //.Where(e => e.Status == "IN")
                        .Where(e => e.UId == uid)
                        .OrderByDescending(p => p.LoginTime);


            AuthenticationResponse authenticationresp = new AuthenticationResponse();
            AuthenticationResponse authenticationrescp = new AuthenticationResponse();
            authenticationresp = check.FirstOrDefault();

            if (authenticationresp == null)
            {
                logobj.UId = uid;
                logobj.Status = "IN";
                logobj.JWTToken = token;
                logobj.LoginTime = DateTime.Now;

                InsertLog(logobj);

                return "Success";

            }
            else if (authenticationresp.Status == "OUT" && authenticationresp.JWTToken == token)
            {
                return "Failed";
            }
            else if (authenticationresp.Status == "IN" && authenticationresp.JWTToken == token)
            {
                var update = Builders<AuthenticationResponse>.Update.Set(a => a.LoginTime, DateTime.Now);
                _authresplog.UpdateOne(model => model.Id == authenticationresp.Id, update);

                return "Success";


            }
            else if (authenticationresp.Status == "IN" && authenticationresp.JWTToken != token)
            {
                check =
                        _authresplog.AsQueryable<AuthenticationResponse>()

                        .Where(e => e.JWTToken == token)
                        .OrderByDescending(p => p.LoginTime);
                authenticationrescp = check.FirstOrDefault();

                if (authenticationrescp == null)
                {
                    var update = Builders<AuthenticationResponse>.Update.Set(a => a.Status, "OUT");
                    _authresplog.UpdateMany(model => model.UId == authenticationresp.UId, update);

                    logobj.UId = uid;
                    logobj.Status = "IN";
                    logobj.JWTToken = token;
                    logobj.LoginTime = DateTime.Now;

                    InsertLog(logobj);
                    return "Success";
                }
                else if (authenticationrescp.Status == "OUT")
                {
                    return "Failed";

                }
                else
                {
                    return "Failed";

                }




               
            }
            else if (authenticationresp.Status == "OUT" && authenticationresp.JWTToken != token)
            {
                logobj.UId = uid;
                logobj.Status = "IN";
                logobj.JWTToken = token;
                logobj.LoginTime = DateTime.Now;

                InsertLog(logobj);

                return "Success";


            }
            else
            {
                return "Failed";
            }




        }
        public LoginResponse GetUSerDetails(Guid uid)
        {
            var ur = FindById(uid);
            LoginResponse UserDetail = new LoginResponse();

            UserDetail.Email = ur.Email;
            UserDetail.Gender = ur.Gender;
            UserDetail.Address = ur.Address;
            UserDetail.DateOfBirth = ur.DateOfBirth;
            UserDetail.Blood_Group = ur.Blood_Group;
            UserDetail.Name = ur.Name;
            UserDetail.Profile_Photo = ur.Profile_Photo;
            UserDetail.Phonenumber = ur.Phonenumber;
            UserDetail.Age = ur.Age;
            UserDetail.Profile_Photo = Func.AES_Decrypt_ECB(ur.Profile_Photo);

            return UserDetail;
            


        }





    }
}
