using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Timers;

namespace FileManagerAPI.Infrastructure
{
    public class FileManager : IFileManager
    {
        private readonly IFileManagerMContext context;
        private readonly DateTime currentTime = DateTime.Now;
        List<DownoloadFile> downoloadFiles = new List<DownoloadFile>();
        public FileManager(IFileManagerMContext context, TimerAlarm timerAlarm)
        {           
           this.context = context;
           timerAlarm.StartTimerEvent();
           
            
        }
        public void CheckFile(Object source, ElapsedEventArgs e)
        {         
            DateTime currentTime = DateTime.Now;
            var oldFile = downoloadFiles.Where(c => c.LastDownoloadTime.AddMinutes(1) < currentTime);
            foreach (var i in oldFile)
            {
                downoloadFiles.Remove(i);
            }
            var archiveFile = string.Join(", ", downoloadFiles.Select(x => x.FileName));
            System.Diagnostics.Debug.WriteLine("Old file was removed.", archiveFile);
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
                        using(MemoryStream memory = new MemoryStream())
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
        public async Task StoredFile(string fileName,byte[] chunkByte)
        {
            var storedFile = new StoredFile
            {
                FileName = fileName,
                ChunkData = chunkByte,
                Size = chunkByte.Length,
                Owner = "Admin",
                dateTimeSave = DateTime.Now,       
            };
            await context.StoredFiles.InsertOneAsync(storedFile);
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
