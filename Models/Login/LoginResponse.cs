using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_JWT.Models.Login
{
    public class LoginResponse
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Age { get; set; }

        public string Gender { get; set; }

        public string Phonenumber { get; set; }

        public string Address { get; set; }

        public string Blood_Group { get; set; }

        public string Profile_Photo { get; set; }

        public DateTime DateOfBirth { get; set; }

    }
}
