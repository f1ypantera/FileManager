using MongoDB.Driver.GridFS;
using Microsoft.Extensions.Options;
using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;
using MongoDB.Driver;

namespace FileManagerAPI.Context
{
    public class FileManagerMContext : IFileManagerMContext
    {
        private readonly IMongoDatabase mongoDatabase;
        private readonly IGridFSBucket bucket;
        public FileManagerMContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            mongoDatabase = client.GetDatabase(options.Value.Database);
            var gridFSBucketOptions = new GridFSBucketOptions()
            {
                BucketName = "files",
                ChunkSizeBytes = 104064, //1mb
            };
            bucket = new GridFSBucket(mongoDatabase, gridFSBucketOptions);
        }
        public IMongoCollection<Component> Components => mongoDatabase.GetCollection<Component>("Components");

        public IGridFSBucket Bucket => bucket;
    }
}
