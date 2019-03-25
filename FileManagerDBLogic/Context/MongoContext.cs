using MongoDB.Driver.GridFS;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using FileManagerDBLogic.ConnectionSettings;

namespace FileManagerDBLogic.Context
{
    public  class MongoContext:IMongoContext
    {
        private readonly IMongoDatabase mongoDatabase;
        private readonly IGridFSBucket bucket;
        public MongoContext(IOptions<Settings> options)
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
        public IMongoCollection<ChunksOfFiles> ChunksOfFiles => mongoDatabase.GetCollection<ChunksOfFiles>("ChunksOfFiles");
        public IMongoCollection<DownoloadFile> DownoloadFile => mongoDatabase.GetCollection<DownoloadFile>("DownoloadFile");
        public IMongoCollection<StoredFile> StoredFiles => mongoDatabase.GetCollection<StoredFile>("StoredFiles");
        public IMongoCollection<UserListFiles> collectionComponents => mongoDatabase.GetCollection<UserListFiles>("UserListComponents");

        public IGridFSBucket Bucket => bucket;
    }
}
