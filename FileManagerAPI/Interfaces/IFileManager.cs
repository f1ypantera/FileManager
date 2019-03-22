using FileManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace FileManagerAPI.Interfaces
{
    public interface IFileManager
    {
        Task InputChunks(ChunksOfFiles chunksOfFiles);
        Task<StoredFile> GetbyId(string id);
        Task<(byte[], string)> Getfile(string id);
        Task<byte[]> GetFileArchive(string[] id);
    }
}
