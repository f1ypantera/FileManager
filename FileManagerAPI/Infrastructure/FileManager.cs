using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Infrastructure
{
    public class FileManager:IFileManager
    {
      private readonly IFileManagerMContext context;
     
        public FileManager(IFileManagerMContext context)
        {
            this.context = context;
        }

        public async Task InputChunks(ChunksOfFiles chunksOfFiles)
        {
            List<ChunksOfFiles> inputChunks = new List<ChunksOfFiles> { chunksOfFiles };
          



            var file = new DownoloadFile()
            {
                FileId = chunksOfFiles.FileId,
                chunks = inputChunks,
                LastDownoloadTime = DateTime.Now,                             
            };


            await context.df.InsertOneAsync(file);
        }
    }
}
