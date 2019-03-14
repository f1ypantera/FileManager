using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FileManagerAPI.Interfaces
{
    public interface IRepositoryDbService<T>
    {
        IQueryable<T> GetAll();
    }
}
