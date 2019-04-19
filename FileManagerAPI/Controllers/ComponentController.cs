using Newtonsoft.Json;
using FileManagerBussinessLogic.Interfaces;
using FileManagerBussinessLogic.Models;
using FileManagerDBLogic.Models;
using Microsoft.AspNetCore.Mvc;
using SocketManagerAPI.WebSockets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="admin")]
    
    public class ComponentController : ControllerBase
    {
        private readonly IFileManager repository;


        public ComponentController(IFileManager repository)
        {
            this.repository = repository;

        }
        [HttpGet]
        public async Task<IEnumerable<StoredFile>> Get()
        {
            return await repository.GetAllFile();
        }
        [HttpGet]
        [Route("AllComponents")]
        public IEnumerable<StoredFile> GetAllComponent()
        {
            var result = repository.GetAll().ToList();
            return result;

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(string id)
        {
            var component = await repository.GetbyId(id);
            if (component == null)
            {
                return NotFound();
            }
            return Ok(component);
        }
        [HttpGet]
        [Route("GetIds")]
        public async Task<IActionResult> GetIds(string ids)
        {
            string[] idsList = ids.Split(',');
            var component = await repository.GetbyIds(idsList);
            if (component == null)
            {
                return NotFound();
            }
            return Ok(component);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string ids)
        {
            string[] idsList = ids.Split(',');         
            DeleteResult result = await repository.Remove(idsList);
            if(result.DeletedCount == 0)
            {
                return NotFound();
            }
            

            return Ok("Has been deleted");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, StoredFile component)
        {
            var find = await repository.GetbyId(id);
            if (find == null)
            {
                return NotFound();
            }
            await repository.Update(id, component);

            return Ok("Update");
        }        
    }
}