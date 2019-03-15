using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileManagerAPI.Models
{
    public class Component
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public string  Owner { get; set; }   
       // public UserListComponents UserListComponents { get; set; }
      //  public Owner Owner { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
        [BsonRepresentation(BsonType.ObjectId)]
        public string fileId { get; set; }
    }
}
