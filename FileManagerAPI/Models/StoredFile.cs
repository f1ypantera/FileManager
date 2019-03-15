using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Models
{
    public class StoredFile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FileId { get; set; }
        public string FileName { get; set; }
        public int Length { get; set; }
        public int ChunkSize { get; set; }
       

    }
}
