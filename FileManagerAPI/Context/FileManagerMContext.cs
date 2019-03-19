using MongoDB.Driver.GridFS;
using Microsoft.Extensions.Options;
using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;
using MongoDB.Driver;
using FileManagerAPI.Infrastructure;

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
                ChunkSizeBytes = 1024, //1mb
                
            };
            bucket = new GridFSBucket(mongoDatabase, gridFSBucketOptions);
        }
        public IMongoCollection<Component> Components => mongoDatabase.GetCollection<Component>("Components");
        public IMongoCollection<ChunksOfFiles> ChunksOfFiles => mongoDatabase.GetCollection<ChunksOfFiles>("ChunksOfFiles");
        public IMongoCollection<DownoloadFile> df => mongoDatabase.GetCollection<DownoloadFile>("DownoloadFiles");
        public IMongoCollection<UserListComponents> collectionComponents => mongoDatabase.GetCollection<UserListComponents>("UserListComponents");

        public IGridFSBucket Bucket => bucket;
    }
}
