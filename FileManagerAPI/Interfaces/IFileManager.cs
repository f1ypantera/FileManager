using FileManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Interfaces
{
    public interface IFileManager
    {
        Task InputChunks(ChunksOfFiles chunksOfFiles);
        Task<List<StoredFile>> GetAll();
        Task<StoredFile> GetbyId(string id);
        Task<List<StoredFile>> GetbyIds(string[] id);
        Task<(byte[], string)> Getfile(string id);
        Task<byte[]> GetFileArchive(string[] id);


    }
}
