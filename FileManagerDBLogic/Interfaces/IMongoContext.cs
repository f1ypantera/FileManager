using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using FileManagerDBLogic.Models;

namespace FileManagerDBLogic.Interfaces
{
    public interface IMongoContext
    {      
        IMongoCollection<ChunksOfFiles> ChunksOfFiles { get; }
        IMongoCollection<StoredFile> StoredFiles { get; }
        IMongoCollection<DownoloadFile> DownoloadFile { get; }
        IGridFSBucket Bucket { get; }
    }
}
