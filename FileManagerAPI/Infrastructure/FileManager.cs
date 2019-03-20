using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Infrastructure
{
    public class FileManager : IFileManager
    {
        private readonly IFileManagerMContext context;
        private readonly DateTime currentTime = DateTime.Now;
        List<DownoloadFile> downoloadFiles = new List<DownoloadFile>();
        public FileManager(IFileManagerMContext context)
        {
            this.context = context;
        }

        public async Task InputChunks(ChunksOfFiles chunksOfFiles)
        {

            var res = downoloadFiles.FirstOrDefault(c => c.FileId == chunksOfFiles.FileId);

            if (res != null)
            {
                int count = res.chunks.Count;
                if (count < chunksOfFiles.TotalCounts)
                {
                    res.chunks.Add(chunksOfFiles);                  
                }
                else
                {
                    var listofchunks = res.chunks.OrderBy(c => c.n);                   
                    var chunkData = string.Join("", listofchunks.Select(x => x.ChunksData));
                  //  byte[] chunkByte = System.Text.Encoding.UTF8.GetBytes(chunkData);
                    await StoredFile(res.FileName, chunkData);
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
            }          
        }
        public async Task StoredFile(string fileName,string chunkByte)
        {
            var storedFile = new StoredFile
            {
                FileName = fileName,
                ChunkData = chunkByte,
                dateTimeSave = DateTime.Now,       
            };
            await context.StoredFiles.InsertOneAsync(storedFile);
        }

        public async Task<List<StoredFile>> GetAll()
        {
            var result = await context.StoredFiles.FindAsync(c=>true);
            return await result.ToListAsync();
        }
    }

}
