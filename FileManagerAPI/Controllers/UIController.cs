using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Reflection;
using FileManagerDBLogic.Interfaces;
using FileManagerBussinessLogic.Interfaces;
using FileManagerDBLogic.Models;
using FileManagerAPI.ModelsDTO;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UIController : ControllerBase
    {
        private readonly IAccountMongoService accountMongoService;
        private readonly IFileManager fileManager;
        private readonly IMapper mapper;
        public UIController(IAccountMongoService accountMongoService,IFileManager fileManager,IMapper mapper)
        {
            this.accountMongoService = accountMongoService;
            this.fileManager = fileManager;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("Users")]
        public ActionResult<List<UserDTO>> GetUser()
        {
            var result = mapper.Map<UserDTO>(accountMongoService.GetAllUser());
            return Ok(result);

        }
        [HttpGet]
        [Route("Files")]
        public ActionResult<List<StoredFile>> GetFile()
        {
            var result = mapper.Map<StoredFileDTO>(fileManager());
            return Ok(result);

        }

    }
}