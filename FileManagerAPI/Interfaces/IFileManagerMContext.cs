﻿using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using FileManagerAPI.Models;

namespace FileManagerAPI.Interfaces
{
    public interface IFileManagerMContext
    {
        IMongoCollection<UserListFiles> collectionComponents { get; }
        IMongoCollection<ChunksOfFiles> ChunksOfFiles { get; }
        IMongoCollection<StoredFile> StoredFiles { get; }
        IMongoCollection<DownoloadFile> DownoloadFile { get; }
        IGridFSBucket Bucket { get; }
    }
}
