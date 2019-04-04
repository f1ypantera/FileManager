using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManagerDBLogic.Models
{
    public class UserDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<ObjectId> StoreFilesId { get; set; }
        public UserDTO()
        {
            StoreFilesId = new List<ObjectId>();
        }
    }
}
