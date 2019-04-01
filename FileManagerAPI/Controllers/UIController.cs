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
        public ActionResult GetAllUser()
        {
            var result = mapper.Map<IEnumerable<User>, List<UserDTO>>(accountMongoService.GetAll());
            return Ok(result);

        }
        [HttpGet]
        [Route("StoredFile")]
        public ActionResult GetAllFile()
        {
            var result = mapper.Map<IEnumerable<StoredFile>, List<StoredFileDTO>>(fileManager.GetAll());
            return Ok(result);

        }


    }
}