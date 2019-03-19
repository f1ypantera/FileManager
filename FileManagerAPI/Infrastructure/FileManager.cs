using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Infrastructure
{
    public class FileManager : IFileManager
    {
        private readonly IFileManagerMContext context;
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
                    await StoredFile(res.FileId, res.FileName, chunkData);
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

                await context.DownoloadFile.InsertOneAsync(downoloadFile);
            }
        }
        public async Task StoredFile(string fileId,string fileName,string chunkData)
        {
            var storedFile = new StoredFile
            {
                FileId = fileId,
                FileName = fileName,
                ChunkData = chunkData,
                dateTimeSave = DateTime.Now,       
            };
            await context.StoredFile.InsertOneAsync(storedFile);
        }
    }

}
