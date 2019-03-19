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
        public async Task<ActionResult<List<StoredFile>>> Get()
        {
            return await fileManager.GetAll();
        }
    }
}