using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using FileManagerBussinessLogic.Infrastructure;
using FileManagerBussinessLogic.Models;
using Newtonsoft.Json;
using System;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="admin")]
    
    public class ComponentController : ControllerBase
    {
        private readonly IRepositoryMongoService repository;

        public ComponentController(IRepositoryMongoService repository)
        {
            this.repository = repository;

        }
        [HttpGet]    
        public async Task<ActionResult<List<StoredFile>>> Get()
        {           
            return await repository.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StoredFile>> GetId(string id)
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
        public async Task<ActionResult<StoredFile>> GetIds(string ids)
        {
            string[] idsList = ids.Split(',');
            var component = await repository.GetbyIds(idsList);
            if (component == null)
            {
                return NotFound();
            }
            return Ok(component);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var component = await repository.GetbyId(id);
            if (component == null)
            {
                return NotFound();
            }
            await repository.Remove(component.FileId);
            return Ok("Has been deleted");
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, StoredFile component)
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