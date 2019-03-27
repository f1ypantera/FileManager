using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountMongoController : ControllerBase
    {
        private readonly IAccountMongoService accountMongoService;
        public AccountMongoController(IAccountMongoService accountMongoService)
        {
            this.accountMongoService = accountMongoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProvidedRole>>> GetRokes()
        {
            return await accountMongoService.GetAllRole();
        }
        [HttpPost]
        [Route("CreateRole")]
        public async Task<ActionResult> CreateRole(ProvidedRole providedRole)
        {
             await accountMongoService.CreateRole(providedRole);
            return Ok(providedRole);
        }


    }
}