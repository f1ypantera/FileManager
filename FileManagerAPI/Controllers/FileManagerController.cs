using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        private readonly IFileManager fileManager;
        public FileManagerController(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        [HttpPost]
        [Route("InputChunksNew")]
        public async Task<ActionResult> InputChunksNew(ChunksOfFiles chunksOfFiles)
        {
            await fileManager.InputChunks(chunksOfFiles);
            return Ok("Chunks getting");
        }
        [HttpGet]
        [Route("DownoaloadFile")]
        public async Task<ActionResult> DownoaloadFile(string ids)
        {
            string[] idsList = ids.Split(',');
            if (idsList.Count() == 1)
            {
                var file = await fileManager.Getfile(ids);
                return File(file.Item1, System.Net.Mime.MediaTypeNames.Application.Octet, file.Item2);
            }
            var files = await fileManager.GetFileArchive(idsList);
            return File(files, "application/zip", "FIle.zip");
        }
    }
}