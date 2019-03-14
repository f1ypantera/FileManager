using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using FileManagerAPI.Models;

namespace FileManagerAPI.Interfaces
{
    public interface IFileManagerMContext
    {
        IMongoCollection<Component> Components { get; }
        IGridFSBucket Bucket { get; }
    }
}
