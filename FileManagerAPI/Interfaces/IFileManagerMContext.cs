using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using FileManagerAPI.Models;

namespace FileManagerAPI.Interfaces
{
    public interface IFileManagerMContext
    {
        IMongoCollection<Component> Components { get; }
        IMongoCollection<UserListComponents> collectionComponents { get; }
        IMongoCollection<ChunksOfFiles> ChunksOfFiles { get; }
        IGridFSBucket Bucket { get; }
    }
}
