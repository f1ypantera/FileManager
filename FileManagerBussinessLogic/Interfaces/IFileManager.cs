using System.Collections.Generic;
using System.Threading.Tasks;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;

namespace FileManagerBussinessLogic.Interfaces
{
    public interface IFileManager
    {
        Task InputChunks(ChunksOfFiles chunksOfFiles);
        Task<(byte[], string)> Getfile(string id);
        Task<byte[]> GetFileArchive(string[] id);
        Task<List<StoredFile>> GetAll();
        Task<StoredFile> GetbyId(string id);
        Task<List<StoredFile>> GetbyIds(string[] id);   
        Task Remove(string[] id);
        Task Update(string id, StoredFile component);

    }
}
