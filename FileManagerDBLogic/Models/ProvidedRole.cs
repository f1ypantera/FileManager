using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace FileManagerDBLogic.Models
{
    public class ProvidedRole
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
