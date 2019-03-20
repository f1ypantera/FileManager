using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using FileManagerAPI.Models;
using MongoDB.Driver;

namespace FileManagerAPI.Interfaces
{
    public interface IRepositoryMService
    {
        Task<List<StoredFile>> GetAll();
        Task<StoredFile> GetbyId(string id);
        Task<List<StoredFile>> GetbyIds(string[] id);
        Task Remove(string id);
        Task Update(string id, StoredFile c);
      
        Task<List<UserListFiles>> GetListFiles();
       
    }
}
