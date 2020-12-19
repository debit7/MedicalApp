using Mongo_JWT.JWTConfiguration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Mongo_JWT.Helper;

namespace Mongo_JWT.Models.Register
{
    
    public class CustomerDetail
    {
        Function Func = new Function();
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public static readonly string DocumentName = "UserDetail";
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Age")]
        public string Age { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, ErrorMessage = "Must be between 5 and 20 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please enter Phonenumber")]
        public string Phonenumber { get; set; }
        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }
        
        public string Blood_Group { get; set; }

        public string Profile_Photo { get; set; }
        [Required(ErrorMessage = "Please enter DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Salt { get; set; }

        public bool ValidatePassword(string password, IEncryptor encryptor)
        {
            var isValid = Password.Equals(encryptor.GetHash(password, Salt));
            return isValid;
        }
        public void SetPassword(string password, IEncryptor encryptor)
        {
            Salt = encryptor.GetSalt(password);
            Password = encryptor.GetHash(password, Salt);
        }
        public void EncryptImage(string Base64image)
        {
            Profile_Photo = Func.AES_Encrypt_ECB(Base64image);


        }
    }

}
