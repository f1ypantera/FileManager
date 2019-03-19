using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.IO;
using System.IO.Compression;
using MongoDB.Driver;
using FileManagerAPI.Models;
using FileManagerAPI.Interfaces;

namespace FileManagerAPI.Services
{
    public class RepositoryMService : IRepositoryMService
    {
        private readonly IFileManagerMContext context;
        public RepositoryMService(IFileManagerMContext context)
        {
            this.context = context;
        }
        public async Task<List<UserListComponents>> GetListComponents()
        {
            var result = await context.collectionComponents.FindAsync(c => true);
            return await result.ToListAsync();
        }
        public async Task<List<Component>> GetAll()
        {
            var result = await context.Components.FindAsync(c => true);
            return await result.ToListAsync();
        }
        public async Task<Component> GetbyId(string id)
        {
            var result = await context.Components.FindAsync(c => c.Id == id);
            return await result.FirstOrDefaultAsync();
        }
        public async Task<List<Component>> GetbyIds(string[] id)
        {
            var result = await context.Components.FindAsync(c => id.Contains(c.Id));
            return await result.ToListAsync();
        }
        public async Task Remove(string id)
        {
            await context.Components.DeleteOneAsync(c => c.Id == id);
        }
        public async Task Update(string id, Component component)
        {
            await context.Components.ReplaceOneAsync(c => c.Id == id, component);
        }
        public async Task<(byte[], string)> Getfile(string id)
        {
            var fileComponent = await GetbyId(id);
            var bytes = await context.Bucket.DownloadAsBytesAsync(new ObjectId(id));
            return (bytes, fileComponent.Name);
        }
        public async Task<byte[]> GetFileArchive(string[] id)
        {
            byte[] fileBytesZip = null;
            var result = await context.Components.FindAsync(c => id.Contains(c.Id));
            var res = await result.ToListAsync();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (Component doc in res)
                    {
                        var bytes = await context.Bucket.DownloadAsBytesAsync(new ObjectId(doc.Id));
                        ZipArchiveEntry zipItem = zip.CreateEntry(doc.Name);

                        using (MemoryStream original = new MemoryStream(bytes))
                        using (Stream entryStream = zipItem.Open())
                        {
                            await original.CopyToAsync(entryStream);
                        }
                    }
                }
                fileBytesZip = memoryStream.ToArray();
            }
            return fileBytesZip;
        }
       
        public async Task StoreFile(Stream fileStream, string fileName)
        {
            ObjectId fileStoreId = await context.Bucket.UploadFromStreamAsync(fileName, fileStream);

            var component = new Component()
            {
                Id = fileStoreId.ToString(),
                Name = fileName,
                Size = fileStream.Length,
                Owner = "Admin",
                //UserListComponents = new UserListComponents
                //{
                //    OwnerId = 1,
                    
                //},
                fileId = fileStoreId.ToString(),
            };
            await context.Components.InsertOneAsync(component);
        }
    }
}
