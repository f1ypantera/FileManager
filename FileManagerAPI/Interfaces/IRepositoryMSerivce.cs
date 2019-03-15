using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using FileManagerAPI.Models;
using MongoDB.Driver;

namespace FileManagerAPI.Interfaces
{
    public interface IRepositoryMService
    {
        Task<List<Component>> GetAll();
        Task<Component> GetbyId(string id);
        Task<List<Component>> GetbyIds(string[] id);
        Task Remove(string id);
        Task Update(string id, Component c);
        Task<(byte[], string)> Getfile(string id);
        Task<byte[]> GetFileArchive(string[] id);
        Task StoreFile(Stream fileStream, string fileName);
        Task<List<UserListComponents>> GetListComponents();
        Task InputChunks(IEnumerable<ChunksOfFiles> chunksOfFiles);
    }
}
