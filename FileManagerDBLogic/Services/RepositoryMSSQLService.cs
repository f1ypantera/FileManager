using System.Linq;
using Microsoft.EntityFrameworkCore;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Context;

namespace FileManagerDBLogic.Services
{
    public class RepositoryMSSQLService<T>:IRepositoryMSSQLService<T> where T:class
    {
        private readonly DbSet<T> dbSet;
        private readonly MSSQLContext context;

        public RepositoryMSSQLService(MSSQLContext context)
        {
            dbSet = context.Set<T>();
            this.context = context;
        }
        public IQueryable<T> GetAll()
        {
            return dbSet;
        }
    }
}
