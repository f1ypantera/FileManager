using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileManagerAPI.Models;
using FileManagerAPI.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : ControllerBase
    {
        private readonly IRepositoryMService repository;
        public ComponentController(IRepositoryMService repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<List<Component>>> Get()
        {
            return await repository.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Component>> GetId(string id)
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
        public async Task<ActionResult<Component>> GetIds(string ids)
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
            await repository.Remove(component.Id);
            return Ok("Has been deleted");
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Component component)
        {
            var find = await repository.GetbyId(id);
            if (find == null)
            {
                return NotFound();
            }
            await repository.Update(id, component);

            return Ok("Update");
        }
        [HttpPost]
        [Route("UploadFile")]
        public async Task<ActionResult> UploadFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                await repository.StoreFile(uploadedFile.OpenReadStream(), uploadedFile.FileName);
            }
            return Ok("Ok");
        }

        [HttpGet]
        [Route("DownoaloadFile")]
        public async Task<ActionResult> DownoaloadFile(string ids)
        {
            string[] idsList = ids.Split(',');
            if (idsList.Count() == 1)
            {
                var file = await repository.Getfile(ids);
                return File(file.Item1, System.Net.Mime.MediaTypeNames.Application.Octet, file.Item2);
            }
            var files = await repository.GetFileArchive(idsList);
            return File(files, "application/zip", "FIle.zip");
        }
    }
}