
using System.Linq;


namespace FileManagerDBLogic.Interfaces
{
   public  interface IRepositoryMSSQLService<T>
    {
        IQueryable<T> GetAll();
    }
}
