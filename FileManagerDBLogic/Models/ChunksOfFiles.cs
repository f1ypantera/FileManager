using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileManagerDBLogic.Models
{
    public class ChunksOfFiles
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public int n { get; set; }
        public int TotalCounts { get; set; }
        public string ChunksData { get; set; }
    }
}
