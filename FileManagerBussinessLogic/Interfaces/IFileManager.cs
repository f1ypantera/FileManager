﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using MongoDB.Driver;

namespace FileManagerBussinessLogic.Interfaces
{
    public interface IFileManager
    {
        Task InputChunks(ChunksOfFiles chunksOfFiles);
        Task<(byte[], string)> Getfile(string id);
        Task<byte[]> GetFileArchive(string[] id);
        IEnumerable<StoredFile> GetAll();
        Task<List<StoredFile>> GetAllFile();
        Task<StoredFile> GetbyId(string id);
        Task<List<StoredFile>> GetbyIds(string[] id);   
        Task<DeleteResult>  Remove(string[] id);
        Task Update(string id, StoredFile component);

    }
}
