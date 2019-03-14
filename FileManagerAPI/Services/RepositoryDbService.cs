using System.Linq;
using FileManagerAPI.Context;
using FileManagerAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileManagerAPI.Services
{
    public class RepositoryDbService<T> : IRepositoryDbService<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        private readonly FileManagerDBcontext context;

        public RepositoryDbService(FileManagerDBcontext context)
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
