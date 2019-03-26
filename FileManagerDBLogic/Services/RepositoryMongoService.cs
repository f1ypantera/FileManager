using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;



namespace FileManagerDBLogic.Services
{
    public class RepositoryMongoService:IRepositoryMongoService
    {
        private readonly IMongoContext context;
        public RepositoryMongoService(IMongoContext context)
        {
            this.context = context;
        }   
        public async Task<List<StoredFile>> GetAll()
        {
            var result = await context.StoredFiles.FindAsync(c => true);
            return await result.ToListAsync();
        }
        public async Task<StoredFile> GetbyId(string id)
        {
            var result = await context.StoredFiles.FindAsync(c => c.FileId == id);

            return await result.FirstOrDefaultAsync();
        }
        public async Task<List<StoredFile>> GetbyIds(string[] id)
        {
            var result = await context.StoredFiles.FindAsync(c => id.Contains(c.FileId));
            return await result.ToListAsync();
        }
        public async Task Remove(string id)
        {
            await context.StoredFiles.DeleteOneAsync(c => c.FileId == id);
        }
        public async Task Update(string id, StoredFile component)
        {
            await context.StoredFiles.ReplaceOneAsync(c => c.FileId == id, component);
        }

    }
}
