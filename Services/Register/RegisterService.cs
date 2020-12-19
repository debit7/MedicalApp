using Mongo_JWT.Models.DataBase;
using Mongo_JWT.Models.Register;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_JWT.Services.Register
{
    public class RegisterService
    {
        private readonly IMongoCollection<CustomerDetail> _registrations;

        public RegisterService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _registrations = database.GetCollection<CustomerDetail>("CustomerDetail");
        }

        public CustomerDetail Create(CustomerDetail customerDetail)
        {
            _registrations.InsertOne(customerDetail);
            return customerDetail;
        }

        public IList<CustomerDetail> Read() =>
            _registrations.Find(sub => true).ToList();

        //public CustomerDetail Find(string id) =>
        //    _registrations.Find(sub => sub.Id == Guid).SingleOrDefault();

       // public void Update(CustomerDetail customerDetail) =>
       //     _registrations.ReplaceOne(sub => sub.Id == customerDetail.Id, customerDetail);

       // public void Delete(string id) =>
       //     _registrations.DeleteOne(sub => sub.Id == id);
    }
}
