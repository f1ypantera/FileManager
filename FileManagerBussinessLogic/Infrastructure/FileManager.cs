using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.IO;
using System.IO.Compression;
using FileManagerBussinessLogic.Interfaces;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using Newtonsoft.Json;
using FileManagerBussinessLogic.Models;

namespace FileManagerBussinessLogic.Infrastructure
{
    public class FileManager:IFileManager
    {
        private readonly IMongoContext context;
        private readonly ITimerAlarm timeAlarm;
        private readonly DateTime currentTime = DateTime.Now;
        private readonly FileSocketManager fileSocketManager;
        List<DownoloadFile> downoloadFiles = new List<DownoloadFile>();
        public FileManager(IMongoContext context, ITimerAlarm timerAlarm, FileSocketManager fileSocketManager)
        {
            this.context = context;
            this.timeAlarm = timerAlarm;
            timeAlarm.Callback = CheckFile;
            timeAlarm.StartTimerEvent();
            this.fileSocketManager = fileSocketManager;
        }
        public void CheckFile()
        {
            DateTime currentTime = DateTime.Now;
            var oldFile = downoloadFiles.Where(c => c.LastDownoloadTime.AddMinutes(10) < currentTime).ToList();
            for (int i = oldFile.Count() - 1; i >= 0; i--)
            {
                var item = oldFile[i];
                downoloadFiles.Remove(item);
                System.Diagnostics.Debug.WriteLine("Old file was removed. File: " + item.FileName);
            }
            var archiveFile = string.Join(", ", downoloadFiles.Select(x => x.FileName));
            System.Diagnostics.Debug.WriteLine("Current active files: " + archiveFile);
        }
        public IEnumerable<StoredFile> GetAll()
        {
            var result =  context.StoredFiles.Find(c => true);
            return result.ToList();
        }
        public async Task<List<StoredFile>> GetAllFile()
        {
            var result = await context.StoredFiles.FindAsync(c => true);
            return await result.ToListAsync();
        }
        public async Task InputChunks(ChunksOfFiles chunksOfFiles)
        {
            var res = downoloadFiles.FirstOrDefault(c => c.FileId == chunksOfFiles.FileId && c.FileName == chunksOfFiles.FileName);
            var sameFile = await context.StoredFiles.FindAsync(c => c.FileName == chunksOfFiles.FileName);
            var file = await sameFile.FirstOrDefaultAsync();


            if (file == null)
            {
                if (res != null)
                {
                    int count = res.chunks.Count;

                    if (count < chunksOfFiles.TotalCounts)
                    {
                        res.chunks.Add(chunksOfFiles);
                    }

                    if (count == chunksOfFiles.TotalCounts - 1)
                    {
                        var listofChunks = res.chunks.OrderBy(c => c.n);
                        byte[] finalChunks = null;
                        using (MemoryStream memory = new MemoryStream())
                        {
                            foreach (var i in listofChunks)
                            {

                                byte[] chunks = Convert.FromBase64String(i.ChunksData);
                                memory.Write(chunks);
                            }
                            finalChunks = memory.ToArray();
                        }

                        await StoredFile(res.FileName, finalChunks);
                        downoloadFiles.Remove(res);
                    }
                }
                else
                {
                    var downoloadFile = new DownoloadFile()
                    {
                        FileId = chunksOfFiles.FileId,
                        FileName = chunksOfFiles.FileName,
                        TotalCount = chunksOfFiles.TotalCounts,
                        chunks = new List<ChunksOfFiles>
                    {
                         chunksOfFiles
                    },
                        LastDownoloadTime = DateTime.Now,
                    };
                    downoloadFiles.Add(downoloadFile);

                    if (chunksOfFiles.TotalCounts == 1)
                    {
                        byte[] chunkByte = Convert.FromBase64String(chunksOfFiles.ChunksData);
                        await StoredFile(chunksOfFiles.FileName, chunkByte);
                        downoloadFiles.Remove(res);
                    }
                }
            }
        }

        public async Task Remove(string id)
        {
          
           // await context.StoredFiles.DeleteManyAsync(c=>id.Contains(c.FileId));

            var filter = Builders<User>.Filter.Eq(s => s.Name, "Admin");

            var update = Builders<User>.Update.Pull("StoreFilesId",id);
            var res  = await context.Users.UpdateManyAsync(filter, update);




            //var removeFile = new Remove
            //{
            //    id = 
            //};
            //await fileSocketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(removeFile));
        }
        public async Task Update(string id, StoredFile component)
        {
            await context.StoredFiles.ReplaceOneAsync(c => c.FileId == id, component);
            var removeFile = new Update
            {
                id = id,
                storedFiles = new List<StoredFile>
                {
                   component
                },
            };
            await fileSocketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(removeFile));
        }   
        public async Task<List<StoredFile>> GetbyIds(string[] id)
        {
            var result = await context.StoredFiles.FindAsync(c => id.Contains(c.FileId));
            return await result.ToListAsync();
        }
        public async Task StoredFile(string fileName, byte[] chunkByte)
        {
            var asyncCursor = await context.Users.FindAsync(c => c.Name == "Admin");
            var user = await asyncCursor.FirstOrDefaultAsync();
            var storedFile = new StoredFile
            {
                FileName = fileName,
                ChunkData = chunkByte,
                Size = chunkByte.Length,
                User = user,          
                dateTimeSave = DateTime.Now,
            };
            await context.StoredFiles.InsertOneAsync(storedFile);
           

            var cursor = await context.StoredFiles.FindAsync(c => c.FileName == storedFile.FileName);
            var file = await cursor.FirstOrDefaultAsync();
            string id = file.FileId;

            var filter = Builders<User>.Filter.Eq(s => s.Name, "Admin");
            var update = Builders<User>.Update.AddToSet(s => s.StoreFilesId, id);
            var result = await context.Users.UpdateOneAsync(filter, update);

            var addFile = new Add
            {            
                storedFiles = new List<StoredFile>
                {
                   storedFile
                },
            };
            await fileSocketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(addFile));
        }
        public async Task<StoredFile> GetbyId(string id)
        {
            var result = await context.StoredFiles.FindAsync(c => c.FileId == id);
            return await result.FirstOrDefaultAsync();
        }
        public async Task<(byte[], string)> Getfile(string id)
        {
            var fileComponent = await GetbyId(id);
            return (fileComponent.ChunkData, fileComponent.FileName);
        }
        public async Task<byte[]> GetFileArchive(string[] id)
        {
            byte[] fileBytesZip = null;
            var result = await context.StoredFiles.FindAsync(c => id.Contains(c.FileId));
            var res = await result.ToListAsync();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (StoredFile doc in res)
                    {
                        ZipArchiveEntry zipItem = zip.CreateEntry(doc.FileName);

                        using (MemoryStream original = new MemoryStream(doc.ChunkData))
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

    }
}
