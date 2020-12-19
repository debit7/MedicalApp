using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_JWT.Models.AuthenticationLog
{
    public class AuthenticationResponse
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public static readonly string DocumentName = "AuthenticationLog";

        public ObjectId Id { get; set; }
        public Guid UId { get; set; }

        public string Status { get; set; }
        public string JWTToken { get; set; }

        public DateTime LoginTime { get; set; }
    }
}
