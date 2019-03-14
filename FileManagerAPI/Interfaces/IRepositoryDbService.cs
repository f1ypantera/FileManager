using System.Linq;

namespace FileManagerAPI.Interfaces
{
    public interface IRepositoryDbService<T>
    {
        IQueryable<T> GetAll();
    }
}
