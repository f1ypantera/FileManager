using FileManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Interfaces
{
    public interface IFileManager
    {
        Task InputChunks(IEnumerable<ChunksOfFiles> chunksOfFiles);
    }
}
