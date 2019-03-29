using FileManagerDBLogic.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.ModelsDTO
{
    public class UserDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<StoredFile> StoreFiles { get; set; }
        public UserDTO()
        {
            StoreFiles = new List<StoredFile>();
        }
    }
}
