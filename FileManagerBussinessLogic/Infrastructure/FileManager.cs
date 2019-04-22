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
using MongoDB.Bson;
using SocketManagerAPI.WebSockets;
using FileManagerBussinessLogic.Socket;
using Newtonsoft.Json.Serialization;

namespace FileManagerBussinessLogic.Infrastructure
{
    public class FileManager:IFileManager
    {
        private readonly IMongoContext context;
        private readonly ITimerAlarm timeAlarm;
        private readonly DateTime currentTime = DateTime.Now;
        private readonly FileSocketManager socketManager;
        List<DownoloadFile> downoloadFiles = new List<DownoloadFile>();
        public FileManager(IMongoContext context, ITimerAlarm timerAlarm,FileSocketManager socketManager)
        {
            this.context = context;
            this.timeAlarm = timerAlarm;
            timeAlarm.Callback = CheckFile;
            timeAlarm.StartTimerEvent();
            this.socketManager = socketManager;
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

        public async Task<DeleteResult> Remove(string[] id)
        {        
            var result = await context.StoredFiles.DeleteManyAsync(c=>id.Contains(c.FileId));

            for(int i=0;i<id.Length; i++)
            {
                var filter = Builders<User>.Filter.Eq(s => s.Name, "Admin");
                var update = Builders<User>.Update.Pull("StoreFilesId", ObjectId.Parse(id[i]));
                var res = await context.Users.UpdateOneAsync(filter, update);

                var removeFile = new Remove
                {
                    id = id[i]
                };
                await socketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(removeFile));
            }
            return result;
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
            await socketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(removeFile));
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
            var  file = await cursor.FirstOrDefaultAsync();
            string id = file.FileId;

            var filter = Builders<User>.Filter.Eq(s => s.Name, "Admin");
            var update = Builders<User>.Update.AddToSet(s=>s.StoreFilesId , ObjectId.Parse(id));
            var result = await context.Users.UpdateOneAsync(filter, update);

            var addFile = new Add
            {
                storedFiles = new List<StoredFile>
                {
                    storedFile
                },

            };

            JsonSerializerSettings settings = new JsonSerializerSettings();          
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            await socketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(addFile, settings));
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
