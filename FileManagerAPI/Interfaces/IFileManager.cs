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
        Task<StoredFile> GetbyId(string id);
        Task<(string, string)> Getfile(string id);
     //   Task<byte[]> GetFileArchive(string[] id);


    }
}
