using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using FileManagerDBLogic.Models;

namespace FileManagerDBLogic.Interfaces
{
    public interface IMongoContext
    {      
        IMongoCollection<ChunksOfFiles> ChunksOfFiles { get; }
        IMongoCollection<User> Users { get; }
        IMongoCollection<ProvidedRole> ProvidedRoles { get; }
        IMongoCollection<StoredFile> StoredFiles { get; }
        IMongoCollection<DownoloadFile> DownoloadFile { get; }
        IMongoCollection<TestDB> TestDB { get; }
        IGridFSBucket Bucket { get; }
    }
}
