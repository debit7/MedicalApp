using Mongo_JWT.Models.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_JWT.Services.Register
{
    interface IRegisterService
    {
        Task<RegisterService> Create(CustomerDetail customerdetail);
        Task<RegisterService> Read(CustomerDetail customerdetail);
        Task<RegisterService> Update(CustomerDetail customerdetail);
        Task<RegisterService> Delete(CustomerDetail customerdetail);


    }
}
