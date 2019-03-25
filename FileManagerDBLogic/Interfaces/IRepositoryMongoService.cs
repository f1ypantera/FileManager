using System.Collections.Generic;
using System.Threading.Tasks;
using FileManagerDBLogic.Models;

namespace FileManagerDBLogic.Interfaces
{
    public interface IRepositoryMongoService
    {
        Task<List<StoredFile>> GetAll();
        Task<StoredFile> GetbyId(string id);
        Task<List<StoredFile>> GetbyIds(string[] id);
        Task Remove(string id);
        Task Update(string id, StoredFile c);

        Task<List<UserListFiles>> GetListFiles();
    }
}
