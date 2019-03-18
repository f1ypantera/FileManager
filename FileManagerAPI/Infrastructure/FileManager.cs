using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Infrastructure
{
    public class FileManager: IFileManager
    {
        private readonly IFileManagerMContext context;


        public FileManager(IFileManagerMContext context)
        {
            this.context = context;
        }

        public async Task InputChunks(IEnumerable<ChunksOfFiles> chunksOfFiles)
        {
            await context.ChunksOfFiles.InsertManyAsync(chunksOfFiles);
        }
    }
}
