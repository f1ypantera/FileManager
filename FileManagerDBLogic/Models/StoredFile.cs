using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FileManagerDBLogic.Models
{
   public  class StoredFile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FileId { get; set; }
        public string FileName { get; set; }
        public double Size { get; set; }
        public byte[] ChunkData { get; set; }
        public DateTime dateTimeSave { get; set; }
        public User User { get; set; }
    }
}
